
using System;

using Foundation;
using UIKit;
using System.Drawing;
using System.Collections.Generic;
using iProPQRSPortableLib;
using System.Net;
using Newtonsoft.Json;
using CoreGraphics;

namespace iProPQRS
{
	public delegate void CodePickerSelectedEvent(CodePickerModel Item);

	public partial class CodePicker : UIViewController
	{
		UIViewController ViewController;
		float uvWidth=280;
		float uvheight=400;

		public CodePicker (UIViewController ViewController,float uWidth) : base ("CodePicker", null)
		{
			uvWidth = uWidth;
			this.ViewController=ViewController;
		}

		string searchkey=string.Empty;
		string type=string.Empty;
		public CodePicker (UIViewController ViewController,float uWidth,string searchkey,string type) : base ("CodePicker", null)
		{
			uvWidth = uWidth;
			this.ViewController=ViewController;
			this.searchkey = searchkey;
			if(string.IsNullOrEmpty(searchkey))
				this.searchkey="a";
			this.type = type;
		}
		public CodePicker (UIViewController ViewController) : base ("CodePicker", null)
		{

			this.ViewController=ViewController;
		}
		public UIWebView wvpatient;
		public UIPopoverController popover;
		public event CodePickerSelectedEvent _ValueChanged;
		public List<CodePickerModel> DataSource {
			get;
			set;
		}
		public string SelectedText {
			get;
			set;
		}
		public int	SelectedValue {
			get;
			set;
		}
		public string SelectedCodeValue {
			get;
			set;
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		public void PresentFromPopover(UIView sender,float x,float y,float vwidth)
		{
			popover = new UIPopoverController(this)
			{
				
				PopoverContentSize = new SizeF(vwidth, uvheight)
			};
			popover.PresentFromRect (new CoreGraphics.CGRect (x, y, 1, 1), sender, UIPopoverArrowDirection.Any, true);

		}
		public void DismissPopOver(CodePickerModel Item)
		{
			
			SelectedText = Item.ItemText;
			SelectedValue = Item.ItemID;
			SelectedCodeValue = Item.ItemCode;
			if(string.IsNullOrEmpty(SelectedText))
				popover.Dismiss(false);
			else
			   popover.Dismiss(false);
			
			_ValueChanged += new CodePickerSelectedEvent(checkVal);

			_ValueChanged.Invoke (Item);

		}
		public void checkVal(CodePickerModel Item)
		{
			
			}
		UITableView utListView;
		UINavigationBar NavBar;
		UISearchBar searchBar;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.Layer.Frame = new CoreGraphics.CGRect (0, 0, uvWidth, uvheight);
			NavBar=new UINavigationBar(new CoreGraphics.CGRect (0, 0, uvWidth, 44));
			utListView = new UITableView (new CoreGraphics.CGRect (0, 44, uvWidth, uvheight));
//			UIBarButtonItem TrashBtn = new UIBarButtonItem (UIBarButtonSystemItem.Cancel, TrashBtnClicked);

			UIButton btnCancel = new UIButton (new CGRect (0, 0, 80, 30));
			btnCancel.SetTitleColor (UIColor.Blue, UIControlState.Normal);
			btnCancel.SetTitle ("Cancel", UIControlState.Normal);
			btnCancel.TouchUpInside += (object sender, EventArgs e) => {
				if(popover!=null)
					popover.Dismiss(false);
			};
			UIBarButtonItem TrashBtn = new UIBarButtonItem (btnCancel);

			UINavigationItem navgitem = new UINavigationItem ("Select");
			navgitem.SetLeftBarButtonItem (TrashBtn, true);
			NavBar.PushNavigationItem(navgitem,true);
			searchBar=new UISearchBar(new CoreGraphics.CGRect (100, 0, uvWidth-100, 44));
			this.View.Add (NavBar);
			this.View.AddSubview(utListView);
			this.View.AddSubview(searchBar);
			this.utListView.Source =new CodePickerSource(this);
			searchBar.BecomeFirstResponder ();
			searchBar.Text = searchkey;
			searchBar.TextChanged+= async (object sender, UISearchBarTextChangedEventArgs e) => {
				if(!string.IsNullOrEmpty(searchBar.Text))
				{
					AppDelegate.pb.Start(this.View,"Searching...");
					var webClient = new WebClient();
					string url =  "http://reference.iprocedures.com/"+type+"/"+searchBar.Text.Trim()+"/20";
					string procData = webClient.DownloadString (url);
					procedureItems = (ProcedureDiagnosticMaster)JsonConvert.DeserializeObject (procData, typeof(ProcedureDiagnosticMaster));
					int uwidth = 0;
					DataSource = SetProcedureDataSource(out uwidth);
					this.utListView.Source =new CodePickerSource(this);
					this.utListView.ReloadData();
					AppDelegate.pb.Stop();
				}
					
					//RectangleF fillrect = new RectangleF(0,0,uwidth,uvheight);
					//this.View.Frame=fillrect;


			};

			// Perform any additional setup after loading the view, typically from a nib.
		}
		ProcedureDiagnosticMaster procedureItems;
		public List<CodePickerModel> SetProcedureDataSource(out int wvalue)
		{
			CodePickerModel item;
			int cnt = 1;
			wvalue=280;
			int pcharcount = 0;
			int charcount = 0;
			List<CodePickerModel> dlist = new List<CodePickerModel> ();
			foreach (DataResults procitem in procedureItems.results) {
				item = new CodePickerModel ();
				item.ItemCode = procitem.Code;
				item.ItemText = procitem.Name;
				item.ItemID = procitem.ProcCodeID;
				dlist.Add (item);
				if (pcharcount < item.ItemText.Length) {
					pcharcount = item.ItemText.Length;
				    wvalue = MeasureTextSize (item.ItemText);
				}
				item = null;
				cnt++;
			}
			return dlist;
		}
		public int MeasureTextSize(string txt)
		{
			int uvwidth=280;
			int charcount = 0;
			int mainwidth =Convert.ToInt16(View.Bounds.Width);

			UITableViewCell tbc=new UITableViewCell();
			tbc.TextLabel.Text = txt;
			tbc.TextLabel.Font=UIFont.SystemFontOfSize(20);
			tbc.TextLabel.SizeToFit ();
			int vwidth = Convert.ToInt16(tbc.TextLabel.Bounds.Width);
			if (vwidth < uvwidth)
				uvwidth = 280;
			else
				uvwidth = vwidth;

			if (mainwidth < vwidth)
				uvwidth = mainwidth-100;

			return uvwidth;
		}
		void TrashBtnClicked (object sender, EventArgs e)
		{
			if(popover!=null)
				popover.Dismiss(false);
		}
	}
	public class CodePickerSource : UITableViewSource
	{
		CodePicker CodePickerController;
		public CodePickerSource (CodePicker homeController)
		{
			this.CodePickerController = homeController;
		}
		public override nint NumberOfSections (UITableView tableView)
		{
			// TODO: return the actual number of sections
			return 1;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			// TODO: return the actual number of items in the section
			return CodePickerController.DataSource.Count;
		}
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			CodePickerModel item = CodePickerController.DataSource [indexPath.Row];

			CodePickerModel.SelectedVal = item.ItemID.ToString();	
			this.CodePickerController.DismissPopOver(item);

		}
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			//			var cell = tableView.DequeueReusableCell (MenuDropDownCell.Key) as MenuDropDownCell;
			//			if (cell == null)
			//				cell = new MenuDropDownCell ();
			var cell = tableView.DequeueReusableCell ("TableCell");
			if (cell == null)
				cell = new UITableViewCell ();

			CodePickerModel item = CodePickerController.DataSource  [indexPath.Row];
			// TODO: populate the cell with the appropriate data based on the indexPath
			if (item.ToString() == DropDownModel.DropDownSelectedVal.ToString()) {
				cell.Accessory = UITableViewCellAccessory.Checkmark;
			} else {
				cell.Accessory = UITableViewCellAccessory.None;
			}
			cell.TextLabel.Text = item.ItemText;
			//			cell.DetailTextLabel.Text = "DetailsTextLabel";

			return cell;
		}

	}
	public class CodePickerModel
	{
		public string ItemCode {
			get;
			set;
		}

		public int ItemID {
			get;
			set;
		}
		public string ItemText {
			get;
			set;
		}
		public static string SelectedVal {
			get;
			set;
		}
	}
}


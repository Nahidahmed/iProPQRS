
using System;

using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CoreGraphics;

namespace iProPQRS
{
	public delegate void qmCodePickerSelectedEvent();
	public partial class qmCodePicker : UIViewController
	{
		UIViewController ViewController;
		float uvWidth=280;
		float uvheight=600;
		public qmCodePicker (UIViewController ViewController,float uWidth) : base ("qmCodePicker", null)
		{
			uvWidth = uWidth;
			this.ViewController=ViewController;		
		}
		public bool isIndexrequried=true;
		public qmCodePicker (UIViewController ViewController,float uWidth,List<CodePickerModel> ds) : base ("qmCodePicker", null)
		{
			DataSource = ds;
			tempds = ds;		

			uvWidth = uWidth;
			this.ViewController=ViewController;		
			if (DataSource.Count > 1 && DataSource.Count < 50) {
				uvheight = 300;
				isIndexrequried = false;
			} else if (DataSource.Count > 50 && DataSource.Count < 150) {
				uvheight = 400;
				isIndexrequried = false;
			}
			else if (DataSource.Count > 150 && DataSource.Count < 200)
				uvheight = 500;
			else 
				uvheight = 670;


		}
		public qmCodePicker (UIViewController ViewController) : base ("qmCodePicker", null)
		{			
			this.ViewController = ViewController;		
		}
		public UIWebView wvpatient;
		public UIPopoverController popover;
		public List<CodePickerModel> SelectedItems = new List<CodePickerModel>();
		public string TypeOfList = string.Empty;
		public event qmCodePickerSelectedEvent _ValueChanged;
		static List<CodePickerModel> tempds=new List<CodePickerModel>();
		public void mDataSource(List<CodePickerModel> ds)
		{
			DataSource = ds;
			tempds = ds;		
		}
		public List<CodePickerModel> DataSource {
			get;
			set;
		}
		public void Setwidth()
		{
			//this.View.Layer.Bounds.Width = 400f;
		}
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		public void PresentFromPopover(UIView sender,float x,float y)
		{
			popover = new UIPopoverController(this)
			{

				PopoverContentSize = new SizeF(345, 350)
			};

			popover.PresentFromRect (new CoreGraphics.CGRect (x, y, 1, 1), sender, UIPopoverArrowDirection.Any, true);
			this.View.Layer.Frame = new CoreGraphics.CGRect (0, 0, uvWidth, uvheight);
		}
		public void PresentFromPopover(UIView sender,float x,float y,float vwidth )
		{
			popover = new UIPopoverController(this)
			{

				PopoverContentSize = new SizeF(vwidth, uvheight)

			};

			popover.PresentFromRect (new CoreGraphics.CGRect (x, y, 1, 1), sender, UIPopoverArrowDirection.Any, true);
			this.View.Layer.Frame = new CoreGraphics.CGRect (0, 0, vwidth, uvheight);
		}
		public void DismissPopOver(CodePickerModel item )
		{
			
			popover.Dismiss(false);
			_ValueChanged += new qmCodePickerSelectedEvent(checkVal);
			_ValueChanged.Invoke ();

		}
		public void checkVal()
		{

			//DismissPopOver(this.Selecteditem);
		}
		public bool Issearchactive=false;
		UITableView mutListView;
		UINavigationBar NavBar;
		UISearchBar searchBar;
		public bool  AllowsMultipleSelection=true;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.View.Layer.Frame = new CoreGraphics.CGRect (0, 0, uvWidth, uvheight);
			mutListView = new UITableView (new CoreGraphics.CGRect (0, 45, uvWidth, uvheight));
			mutListView.AllowsMultipleSelection = AllowsMultipleSelection;
			mutListView.AllowsMultipleSelectionDuringEditing = AllowsMultipleSelection;
			NavBar = new UINavigationBar (new CoreGraphics.CGRect (0, 0, uvWidth, 44));
//			UIBarButtonItem bbitemCancel = new UIBarButtonItem (UIBarButtonSystemItem.Cancel, CancelButtonClicked);		
			UIButton btnCancel = new UIButton (new CGRect (0, 0, 80, 30));
			btnCancel.SetTitleColor (UIColor.Blue, UIControlState.Normal);
			btnCancel.SetTitle ("Cancel", UIControlState.Normal);
			btnCancel.TouchUpInside += (object sender, EventArgs e) => {
				popover.Dismiss(false);
			};
			UIBarButtonItem bbitemCancel = new UIBarButtonItem (btnCancel);


			UIBarButtonItem bbitemDone = new UIBarButtonItem (UIBarButtonSystemItem.Trash, DoneButtonClicked);

			UINavigationItem navgitem = new UINavigationItem ("Select");
			navgitem.SetLeftBarButtonItem (bbitemCancel, true);
			navgitem.SetRightBarButtonItem (bbitemDone, true);
			NavBar.PushNavigationItem (navgitem, true);
			if (Issearchactive)
			{
					searchBar = new UISearchBar (new CoreGraphics.CGRect (0, 44, uvWidth, 44));
				this.searchBar.TextChanged += (object sender, UISearchBarTextChangedEventArgs e) => {
					//DataSource.Clear();
					DataSource = tempds.FindAll (u => u.ItemText.ToLower ().Contains (searchBar.Text.ToLower ()));
					this.mutListView.Source =new qmCodePickerSource(this);
					this.mutListView.ReloadData ();
					searchBar.ShowsCancelButton = true;

				};
				this.searchBar.CancelButtonClicked += (object sender, EventArgs e) => {
					searchBar.Text = string.Empty;
					DataSource = tempds;
					searchBar.ResignFirstResponder ();
					searchBar.ShowsCancelButton = false;
					this.mutListView.Source =new qmCodePickerSource(this);
					this.mutListView.ReloadData ();
				};
				mutListView = new UITableView (new CoreGraphics.CGRect (0, 88, uvWidth, uvheight-80));
				this.View.AddSubview(searchBar);
		    }
			this.View.Add (NavBar);
			this.View.AddSubview(mutListView);
			this.mutListView.Source =new qmCodePickerSource(this);
			// Perform any additional setup after loading the view, typically from a nib.
		}
		void CancelButtonClicked (object sender, EventArgs e)
		{
			popover.Dismiss(false);
		}
		void DoneButtonClicked (object sender, EventArgs e)
		{
			SelectedItems.Clear ();
			DismissPopOver(null);
		}
	}
	public class qmCodePickerSource : UITableViewSource
	{
		qmCodePicker CodePickerController;
		Dictionary<string, List<CodePickerModel>> indexedtableitems; 
		string[] keys;
		public qmCodePickerSource (qmCodePicker homeController)
		{
			this.CodePickerController = homeController;

			indexedtableitems = new Dictionary<string,List<CodePickerModel>> ();
			foreach (var t in CodePickerController.DataSource) {
				if (indexedtableitems.ContainsKey (t.ItemText [0].ToString ().ToUpper())) {
					indexedtableitems [t.ItemText [0].ToString ().ToUpper()].Add (t);
				} else {
					indexedtableitems.Add (t.ItemText [0].ToString ().ToUpper(), new List<CodePickerModel> (){ t });
				}
			}
			keys = indexedtableitems.Keys.OrderBy (o=>o.ToString()).ToArray();

		}
		public override string TitleForHeader (UITableView tableView, nint section)
		{
			if (CodePickerController.isIndexrequried)
				return keys [section];
			else
				return "";
			
		}
		public override nint NumberOfSections (UITableView tableView)
		{
			// TODO: return the actual number of sections
			if (CodePickerController.isIndexrequried)
				return keys.Length;
			else
				return 1;
		}

		public override string[] SectionIndexTitles (UITableView tableView)
		{
			if (CodePickerController.isIndexrequried)
				return indexedtableitems.Keys.OrderBy (o => o.ToString ()).ToArray ();
			else
				return null;
			
		}
		public override nint RowsInSection (UITableView tableview, nint section)
		{
			// TODO: return the actual number of items in the section
			//return CodePickerController.DataSource.Count;
			if (CodePickerController.isIndexrequried)
			  return indexedtableitems [keys [section]].Count;
			else
				return CodePickerController.DataSource.Count;
			
		}
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			//CodePickerModel item = CodePickerController.DataSource [indexPath.Row];
			CodePickerModel item = null;
			if (CodePickerController.isIndexrequried)
				item =  indexedtableitems [keys [indexPath.Section]] [indexPath.Row];
			else
				item = CodePickerController.DataSource [indexPath.Row];
			
			this.CodePickerController.SelectedItems.Clear ();
			this.CodePickerController.SelectedItems.Add (item);				
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
			CodePickerModel item = null; 
			//CodePickerModel item = CodePickerController.DataSource  [indexPath.Row];
			if (CodePickerController.isIndexrequried)
				item= indexedtableitems [keys [indexPath.Section]] [indexPath.Row];
			else
				item = CodePickerController.DataSource  [indexPath.Row];
			
			// TODO: populate the cell with the appropriate data based on the indexPath
			if (this.CodePickerController.SelectedItems != null && this.CodePickerController.SelectedItems.Count > 0 && this.CodePickerController.SelectedItems[0].ItemCode == item.ItemCode && this.CodePickerController.SelectedItems[0].ItemID == item.ItemID) {
				cell.Accessory = UITableViewCellAccessory.Checkmark;
			} else {
				cell.Accessory = UITableViewCellAccessory.None;
			}
			cell.TextLabel.Text = item.ItemText;
			//			cell.DetailTextLabel.Text = "DetailsTextLabel";

			return cell;
		}

	}
}


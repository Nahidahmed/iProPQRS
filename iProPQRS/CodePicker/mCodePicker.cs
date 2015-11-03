
using System;

using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CoreGraphics;

namespace iProPQRS
{
	public delegate void mCodePickerSelectedEvent();
	public partial class mCodePicker : UIViewController
	{
		UIViewController ViewController;
		float uvWidth=280;
		float uvheight=400;
		public mCodePicker (UIViewController ViewController,float uWidth ) : base ("mCodePicker", null)
		{
			uvWidth = uWidth;
			this.ViewController=ViewController;		
		}
		public mCodePicker (UIViewController ViewController,float uWidth,List<CodePickerModel> ds ) : base ("mCodePicker", null)
		{
			uvWidth = uWidth;
			this.ViewController=ViewController;		
			DataSource = ds;
		}
		public mCodePicker (UIViewController ViewController) : base ("mCodePicker", null)
		{			
			this.ViewController = ViewController;		

		}
		public UIWebView wvpatient;
		public UIPopoverController popover;
		public List<CodePickerModel> SelectedItems = new List<CodePickerModel>();
		public string TypeOfList = string.Empty;
		public string Title {
			get;
			set;
		}
		public event mCodePickerSelectedEvent _ValueChanged;
		public void mDataSource(List<CodePickerModel> ds)
		{
			DataSource = ds;
			tempds = ds;
			//if (DataSource.Count < )
			//	searchBar.Hidden = true;
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

			//this.View.Layer.Frame = new CoreGraphics.CGRect (0, 0, width, Height+88);
		//	this.mutListView.Layer.Frame = new CoreGraphics.CGRect (0, 88, width, Height);
			//this.NavBar.Layer.Frame = new CoreGraphics.CGRect (0, 0, width, 44);
			//this.searchBar.Layer.Frame = new CoreGraphics.CGRect (0, 44, width, 44);
			//this.mutListView.BackgroundColor = UIColor.Red;
			//mutListView.TranslatesAutoresizingMaskIntoConstraints = false;
			//View.AddConstraint (
			//	NSLayoutConstraint.Create ( mutListView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 0, width)
			//);

			popover.PresentFromRect (new CoreGraphics.CGRect (x, y, 1, 1), sender, UIPopoverArrowDirection.Any, true);
			this.View.Layer.Frame = new CoreGraphics.CGRect (0, 0, vwidth, uvheight);
		}
		public void DismissPopOver()
		{
			popover.Dismiss(false);
			_ValueChanged += new mCodePickerSelectedEvent(checkVal);
			_ValueChanged.Invoke ();

		}
		public void checkVal()
		{

			//DismissPopOver(this.Selecteditem);
		}

		static List<CodePickerModel> tempds=new List<CodePickerModel>();
		UITableView mutListView;
		UINavigationBar NavBar;
		UISearchBar searchBar;
		public bool  AllowsMultipleSelection=true;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.View.Layer.Frame = new CoreGraphics.CGRect (0, 0, uvWidth, uvheight);
			mutListView = new UITableView (new CoreGraphics.CGRect (0, 88, uvWidth, uvheight-100));
			mutListView.AllowsMultipleSelection = AllowsMultipleSelection;
			mutListView.AllowsMultipleSelectionDuringEditing = AllowsMultipleSelection;
			NavBar=new UINavigationBar(new CoreGraphics.CGRect (0, 0, uvWidth, 44));
//			UIBarButtonItem bbitemCancel = new UIBarButtonItem (UIBarButtonSystemItem.Cancel, CancelButtonClicked);
			UIButton btnCancel = new UIButton (new CGRect (0, 0, 80, 30));
			btnCancel.SetTitleColor (UIColor.Blue, UIControlState.Normal);
			btnCancel.SetTitle ("Cancel", UIControlState.Normal);
			btnCancel.TouchUpInside += (object sender, EventArgs e) => {
				popover.Dismiss(false);
			};
			UIBarButtonItem bbitemCancel = new UIBarButtonItem (btnCancel);

//			UIBarButtonItem bbitemDone = new UIBarButtonItem (UIBarButtonSystemItem.Done, DoneButtonClicked);
			UIButton btnDone = new UIButton (new CGRect (0, 0, 80, 30));
			btnDone.SetTitleColor (UIColor.Blue, UIControlState.Normal);
			btnDone.SetTitle ("Done", UIControlState.Normal);
			btnDone.TouchUpInside += (object sender, EventArgs e) => {
				DismissPopOver();
			};
			UIBarButtonItem bbitemDone = new UIBarButtonItem (btnDone);

			UINavigationItem navgitem = new UINavigationItem ("Select");
			navgitem.SetLeftBarButtonItem(bbitemCancel,true);
			navgitem.SetRightBarButtonItem (bbitemDone, true);
			NavBar.PushNavigationItem(navgitem,true);
			searchBar=new UISearchBar(new CoreGraphics.CGRect (0, 44, uvWidth, 44));
			this.View.Add (NavBar);
			this.View.AddSubview(mutListView);
			this.View.AddSubview(searchBar);
			this.mutListView.Source =new mCodePickerSource(this);
			//mutListView.SetContentOffset (new CoreGraphics.CGPoint (0, mutListView.ContentSize.Height - mutListView.Frame.Size.Height), false);
			//CoreGraphics.CGRect fram = mutListView.Frame;
			//fram.Height = mutListView.ContentSize.Height; 
		//	mutListView.Frame = fram;
			this.searchBar.TextChanged += (object sender, UISearchBarTextChangedEventArgs e) => 
			{
				//DataSource.Clear();
				DataSource=tempds.FindAll(u=>u.ItemText.ToLower().Contains(searchBar.Text.ToLower()));
				this.mutListView.Source =new mCodePickerSource(this);
				this.mutListView.ReloadData();
				searchBar.ShowsCancelButton = true;
			
			};

			this.searchBar.CancelButtonClicked += (object sender, EventArgs e) => {
				searchBar.Text=string.Empty;
				DataSource=tempds;
				searchBar.ResignFirstResponder();
				searchBar.ShowsCancelButton = false;
				this.mutListView.ReloadData();
			};
			//lbltitle.Title = "Select";
			//btnDone.Clicked+= (object sender, EventArgs e) => {
			//	DismissPopOver();
			//};
			//BtnCancel.Clicked+= (object sender, EventArgs e) => {
			//	
			//};
			//mTrashBtn.Clicked+= (object sender, EventArgs e) => {
				//if(popover!=null)
				//	popover.Dismiss(false);
			//};
			// Perform any additional setup after loading the view, typically from a nib.

		}

		void CancelButtonClicked (object sender, EventArgs e)
		{
			popover.Dismiss(false);
		}
		void DoneButtonClicked (object sender, EventArgs e)
		{
			DismissPopOver();
		}
	}
	public class mCodePickerSource : UITableViewSource
	{
		mCodePicker mCodePickerController;
		string typeOfList = string.Empty;
		Dictionary<string, List<CodePickerModel>> indexedtableitems; 
		string[] keys;
		public List<iProPQRSPortableLib.Option> masterSubCatList = new List<iProPQRSPortableLib.Option>();
		public mCodePickerSource (mCodePicker mhomeController)
		{
			this.mCodePickerController = mhomeController;
			this.typeOfList = mhomeController.TypeOfList;
			indexedtableitems = new Dictionary<string,List<CodePickerModel>> ();
			foreach (var t in mCodePickerController.DataSource) {
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
			return keys [section];
		}
		public override nint NumberOfSections (UITableView tableView)
		{
			// TODO: return the actual number of sections
			return keys.Length;
		}

		public override string[] SectionIndexTitles (UITableView tableView)
		{
			return indexedtableitems.Keys.OrderBy (o=>o.ToString()).ToArray();
		}
		public override nint RowsInSection (UITableView tableview, nint section)
		{
			// TODO: return the actual number of items in the section
			return indexedtableitems [keys [section]].Count;
		}
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell selectedCell=tableView.CellAt (indexPath);
			//CodePickerModel item = mCodePickerController.DataSource [indexPath.Row];
			CodePickerModel item = indexedtableitems [keys [indexPath.Section]] [indexPath.Row];
			CodePickerModel.SelectedVal = item.ItemID.ToString();	

			CodePickerModel rselecteditem = null;

			if (this.typeOfList == "Options") {
				if (mCodePickerController.SelectedItems.Count > 0)
					rselecteditem = mCodePickerController.SelectedItems.Find (u => u.ItemCode == item.ItemCode);

				if (rselecteditem != null && rselecteditem.ItemCode == item.ItemCode) {
					this.mCodePickerController.SelectedItems.Remove (rselecteditem);
					selectedCell.Accessory=UITableViewCellAccessory.None;
					selectedCell.SetSelected (false, true);
				} else {
					this.mCodePickerController.SelectedItems.Clear ();
					this.mCodePickerController.SelectedItems.Add (item);
					selectedCell.Accessory=UITableViewCellAccessory.Checkmark;
					tableView.ReloadData ();
				}
			} else {
				if (mCodePickerController.SelectedItems.Count > 0)
					rselecteditem = mCodePickerController.SelectedItems.Find (u => u.ItemID == item.ItemID);

				if (rselecteditem!=null && rselecteditem.ItemID==item.ItemID) {
					this.mCodePickerController.SelectedItems.Remove (rselecteditem);
					selectedCell.Accessory=UITableViewCellAccessory.None;
					selectedCell.SetSelected (false, true);
					if (!mCodePickerController.AllowsMultipleSelection) {						
						if (prevselectedCell != null) {
							prevselectedCell.Accessory = UITableViewCellAccessory.None;
							prevselectedCell.SetSelected (false, true);
						}
					}
				} else {
					
					if (!mCodePickerController.AllowsMultipleSelection) {
						this.mCodePickerController.SelectedItems.Clear ();
						this.mCodePickerController.SelectedItems.Add (item);
						selectedCell.Accessory=UITableViewCellAccessory.Checkmark;
						if (prevselectedCell != null) {
							prevselectedCell.Accessory = UITableViewCellAccessory.None;
							prevselectedCell.SetSelected (false, true);
						}
						prevselectedCell = selectedCell;
					} else {
						this.mCodePickerController.SelectedItems.Add (item);
						selectedCell.Accessory=UITableViewCellAccessory.Checkmark;
					}

				}

				if (this.typeOfList == "Participants")
					this.mCodePickerController.DismissPopOver ();
			}
		}
		UITableViewCell prevselectedCell=null;
		public override void RowDeselected (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell selectedCell=tableView.CellAt (indexPath);
			//CodePickerModel item = mCodePickerController.DataSource [indexPath.Row];
			CodePickerModel item = indexedtableitems [keys [indexPath.Section]] [indexPath.Row];
			CodePickerModel.SelectedVal = item.ItemID.ToString();	

			CodePickerModel rselecteditem=null;

			if (this.typeOfList == "Options") {
				if (mCodePickerController.SelectedItems.Count > 0)
					rselecteditem = mCodePickerController.SelectedItems.Find (u => u.ItemCode == item.ItemCode);

				if (rselecteditem != null && rselecteditem.ItemCode == item.ItemCode) {
					this.mCodePickerController.SelectedItems.Remove (rselecteditem);
					selectedCell.Accessory = UITableViewCellAccessory.None;
					selectedCell.SetSelected (false, true);
				} else {
					this.mCodePickerController.SelectedItems.Add (item);
					selectedCell.Accessory = UITableViewCellAccessory.Checkmark;
				}
			} else {
				if (mCodePickerController.SelectedItems.Count > 0)
					rselecteditem = mCodePickerController.SelectedItems.Find (u => u.ItemID == item.ItemID);

				if (rselecteditem != null && rselecteditem.ItemID == item.ItemID) {
					this.mCodePickerController.SelectedItems.Remove (rselecteditem);
					selectedCell.Accessory = UITableViewCellAccessory.None;
					selectedCell.SetSelected (false, true);
				} else {
					this.mCodePickerController.SelectedItems.Add (item);
					selectedCell.Accessory = UITableViewCellAccessory.Checkmark;

				}
			}
		}
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			//			var cell = tableView.DequeueReusableCell (MenuDropDownCell.Key) as MenuDropDownCell;
			//			if (cell == null)
			//				cell = new MenuDropDownCell ();
			var cell = tableView.DequeueReusableCell ("TableCell");
			if (cell == null)
				cell = new UITableViewCell ();

			//CodePickerModel item = mCodePickerController.DataSource  [indexPath.Row];
			CodePickerModel item =indexedtableitems [keys [indexPath.Section]] [indexPath.Row];
			// TODO: populate the cell with the appropriate data based on the indexPath
			CodePickerModel selecteditem=null;

			if (this.typeOfList == "Options") {
				if (mCodePickerController.SelectedItems.Count > 0)
					selecteditem = mCodePickerController.SelectedItems.Find (u => u.ItemCode == item.ItemCode);

				if (selecteditem != null && selecteditem.ItemCode == item.ItemCode) {
					cell.Editing = true;
					cell.SetSelected (true, true);
					cell.Accessory = UITableViewCellAccessory.Checkmark;
				}else{
					cell.Accessory = UITableViewCellAccessory.None;
				}
			} else {
				if (mCodePickerController.SelectedItems.Count > 0)
					selecteditem = mCodePickerController.SelectedItems.Find (u => u.ItemID == item.ItemID);

				if (selecteditem != null && selecteditem.ItemID == item.ItemID) {
					cell.Editing = true;
					cell.SetSelected (true, true);
					cell.Accessory = UITableViewCellAccessory.Checkmark;
					prevselectedCell = cell;
				} else {
					cell.Accessory = UITableViewCellAccessory.None;
				}
			}
			
			cell.TextLabel.Text = item.ItemText;

			return cell;
		}

	}

}



using System;

using Foundation;
using UIKit;
using System.Drawing;
using System.Collections.Generic;

namespace iProPQRS
{
	public delegate void DropDownSelectedEvent();
	public partial class DropDownViewController : UIViewController
	{
		UIViewController ViewController;


		public event DropDownSelectedEvent _Change;
		public List<DropDownModel> DataSource {
			get;
			set;
		}
		public DropDownModel  Selecteditem {
			get;
			set;
		}
		public DropDownViewController (UIViewController ViewController) : base ("DropDownViewController", null)
		{
			this.ViewController = ViewController;
		}

		private UIPopoverController popover;
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			lblTitle.Text = "Select";
			this.ListView.Source = new DropDownSource (this);

			UIView footer = new UIView (new CoreGraphics.CGRect (0, 0, 0, 0));
			ListView.TableFooterView = footer;
			// Perform any additional setup after loading the view, typically from a nib.

		}
		public void PresentFromPopover(UIView sender,int width,int height)
		{
			popover = new UIPopoverController(this)
			{
//				PopoverContentSize = new SizeF(320, height)
				PopoverContentSize = new SizeF(width, height)
			};

//			var frame = new RectangleF(0, 0,(float)sender.Frame.Width, (float)height);
			var frame = new RectangleF(0, 0,(float)width, (float)height);
			popover.PresentFromRect(frame, sender, UIPopoverArrowDirection.Right, true);
		}
		public void PresentFromPopover(UIView sender)
		{
			popover = new UIPopoverController(this)
			{
				PopoverContentSize = new SizeF(320, 1000)
			};

			var frame = new RectangleF(0, 0,(float)sender.Frame.Width, (float)sender.Frame.Height);
			this.View.Layer.Frame = new CoreGraphics.CGRect (0, 0, 150, 800);
			popover.PresentFromRect(frame, sender, UIPopoverArrowDirection.Any, true);
		}
		public void DismissPopOver(DropDownModel Item)
		{
			SelectedValue = Item.DropDownID;
			SelectedText = Item.DropDownText;
			_Change += new DropDownSelectedEvent(checkVal);
			popover.Dismiss(false);
			_Change.Invoke ();
		}
		public int SelectedValue {
			get;
			set;
		}
		public string SelectedText {
			get;
			set;
		}
		public void checkVal()
		{
			//DismissPopOver(this.Selecteditem);
		}
	}
	public class DropDownSource : UITableViewSource
	{
		DropDownViewController DropDownController;
		public DropDownSource (DropDownViewController homeController)
		{
			this.DropDownController = homeController;
		}
		public override nint NumberOfSections (UITableView tableView)
		{
			// TODO: return the actual number of sections
			return 1;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			// TODO: return the actual number of items in the section
			return DropDownController.DataSource.Count;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell selectedCell = tableView.CellAt (indexPath);

			DropDownModel item = DropDownController.DataSource [indexPath.Row];
			DropDownModel.DropDownSelectedVal = item.DropDownID;
			DropDownController.SelectedValue = item.DropDownID;

			if (selectedCell.Accessory == UITableViewCellAccessory.None) {
				selectedCell.Editing = true;
				selectedCell.SetSelected (true, true);
				selectedCell.Accessory = UITableViewCellAccessory.Checkmark;
			} else {
				selectedCell.Accessory = UITableViewCellAccessory.None;
				DropDownController.SelectedValue =  item.DropDownID;
				item.DropDownID =  item.DropDownID;
				item.DropDownText = string.Empty;
			}


			this.DropDownController.DismissPopOver(item);
		}
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			//			var cell = tableView.DequeueReusableCell (MenuDropDownCell.Key) as MenuDropDownCell;
			//			if (cell == null)
			//				cell = new MenuDropDownCell ();
			var cell = tableView.DequeueReusableCell ("TableCell");
			if (cell == null)
				cell = new UITableViewCell ();

			DropDownModel item = DropDownController.DataSource  [indexPath.Row];
			// TODO: populate the cell with the appropriate data based on the indexPath
			if (item.DropDownID.ToString() == DropDownController.SelectedValue.ToString()) {
				cell.Editing = true;
				cell.SetSelected (true, true);
				cell.Accessory = UITableViewCellAccessory.Checkmark;
			} else {
				cell.Accessory = UITableViewCellAccessory.None;
			}
			cell.TextLabel.Text = item.DropDownText;
			//			cell.DetailTextLabel.Text = "DetailsTextLabel";

			return cell;
		}

	}
	public class DropDownModel
	{
		public int	DropDownID {
			get;
			set;
		}		
		public string DropDownText {
			get;
			set;
		}
		static	public int DropDownSelectedVal {
			get;
			set;
		}
	}
}



using System;

using Foundation;
using UIKit;
using System.Drawing;
using iProPQRSPortableLib;

namespace iProPQRS
{
	public partial class FacilityDropDownViewController : UIViewController
	{
		PatientListView patListView;
		public FacilityDropDownViewController (PatientListView patListView) : base ("FacilityDropDownViewController", null)
		{
			this.patListView = patListView;
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
			this.facilityTable.Source = new MenuDropDownSource (this);
			// Perform any additional setup after loading the view, typically from a nib.
		}
		public void PresentFromPopover(UIView sender)
		{
			popover = new UIPopoverController(this)
			{
				PopoverContentSize = new SizeF(320, 350)
			};

			var frame = new RectangleF(0, 0,(float)sender.Frame.Width, (float)sender.Frame.Height);
			popover.PresentFromRect(frame, sender, UIPopoverArrowDirection.Down, true);
		}
		public void DismissPopOver(FacilityDetails facility)
		{
			//if (patListView.FacilityDropDownBtn.TitleLabel.Text != facility.FacilityName) {
				this.patListView.SetSelectedFacility (facility.FacilityName);
			//}
			popover.Dismiss(false);
		}

	}

	public class MenuDropDownSource : UITableViewSource
	{
		FacilityDropDownViewController facilityDropDownController;
		public MenuDropDownSource (FacilityDropDownViewController homeController)
		{
			this.facilityDropDownController = homeController;
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return iProPQRSPortableLib.Consts.Facilities.result.Count;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			FacilityDetails facility = iProPQRSPortableLib.Consts.Facilities.result[indexPath.Row];
			iProPQRSPortableLib.Consts.SelectedFacilityID = facility.FMID.ToString();
			this.facilityDropDownController.DismissPopOver(facility);

		}
		//		public override string TitleForHeader (UITableView tableView, nint section)
		//		{
		//			return "Header";
		//		}
		//
		//		public override string TitleForFooter (UITableView tableView, nint section)
		//		{
		//			return "Footer";
		//		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			//			var cell = tableView.DequeueReusableCell (MenuDropDownCell.Key) as MenuDropDownCell;
			//			if (cell == null)
			//				cell = new MenuDropDownCell ();
			var cell = tableView.DequeueReusableCell ("TableCell");
			if (cell == null)
				cell = new UITableViewCell ();

			FacilityDetails facility = iProPQRSPortableLib.Consts.Facilities.result[indexPath.Row];
			// TODO: populate the cell with the appropriate data based on the indexPath
			if (facility.FMID.ToString() == iProPQRSPortableLib.Consts.SelectedFacilityID) {
				cell.Accessory = UITableViewCellAccessory.Checkmark;
			} else {
				cell.Accessory = UITableViewCellAccessory.None;
			}
			cell.TextLabel.Text = facility.FacilityName;
			//			cell.DetailTextLabel.Text = "DetailsTextLabel";

			return cell;
		}
	}
}


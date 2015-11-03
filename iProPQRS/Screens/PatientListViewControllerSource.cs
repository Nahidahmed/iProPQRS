
using System;

using Foundation;
using UIKit;
using System.Collections.Generic;

namespace iProPQRS
{
	public class PatientListViewControllerSource : UITableViewSource
	{
		public List<PatientItemGroup> tableItems;

		public PatientListViewControllerSource (List<PatientItemGroup> items)
		{
			this.tableItems = items;
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			// TODO: return the actual number of sections
			return tableItems.Count;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			// TODO: return the actual number of items in the section
			return tableItems [(int)section].ListItems.Count;
		}

		public override string TitleForHeader (UITableView tableView, nint section)
		{
			return "Header";
		}

		public override string TitleForFooter (UITableView tableView, nint section)
		{
			return "Footer";
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
//			var cell = tableView.DequeueReusableCell (PatientListViewControllerCell.Key) as PatientListViewControllerCell;
//			if (cell == null)
//				cell = new PatientListViewControllerCell ();

			UITableViewCell cell = tableView.DequeueReusableCell ("TableCell");

			if (cell == null)
				cell = new UITableViewCell (UITableViewCellStyle.Subtitle, "TableCell");
			
			// TODO: populate the cell with the appropriate data based on the indexPath
			cell.TextLabel.Text= tableItems [indexPath.Section].ListItems [indexPath.Row].Name;
			cell.DetailTextLabel.Text = tableItems[indexPath.Section].ListItems[indexPath.Row].MRNumber;

			
			return cell;
		}
	}
}



using System;

using Foundation;
using UIKit;

namespace iProPQRS
{
	public class PatientListViewControllerCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString ("PatientListViewControllerCell");

		public PatientListViewControllerCell () : base (UITableViewCellStyle.Value1, Key)
		{
			// TODO: add subviews to the ContentView, set various colors, etc.
			TextLabel.Text = "TextLabel";
		}
	}
}


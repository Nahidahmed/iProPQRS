// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace iProPQRS
{
	[Register ("PatientListView")]
	partial class PatientListView
	{
		[Outlet]
		UIKit.UIButton AddNewPatientBtn { get; set; }

		[Outlet]
		UIKit.UIButton datePickerBtn { get; set; }

		[Outlet]
		UIKit.UIButton facilityDropDownBtn { get; set; }

		[Outlet]
		UIKit.UIButton logOutBtn { get; set; }

		[Outlet]
		UIKit.UITableView mTableView { get; set; }

		[Outlet]
		UIKit.UIButton refreshBtn { get; set; }

		[Outlet]
		UIKit.UISearchBar searchBar { get; set; }

		[Action ("datePickerClicked:")]
		partial void datePickerClicked (Foundation.NSObject sender);

		[Action ("facilityClicked:")]
		partial void facilityClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (AddNewPatientBtn != null) {
				AddNewPatientBtn.Dispose ();
				AddNewPatientBtn = null;
			}

			if (datePickerBtn != null) {
				datePickerBtn.Dispose ();
				datePickerBtn = null;
			}

			if (facilityDropDownBtn != null) {
				facilityDropDownBtn.Dispose ();
				facilityDropDownBtn = null;
			}

			if (logOutBtn != null) {
				logOutBtn.Dispose ();
				logOutBtn = null;
			}

			if (mTableView != null) {
				mTableView.Dispose ();
				mTableView = null;
			}

			if (refreshBtn != null) {
				refreshBtn.Dispose ();
				refreshBtn = null;
			}

			if (searchBar != null) {
				searchBar.Dispose ();
				searchBar = null;
			}
		}
	}
}

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
	[Register ("FacilityDropDownViewController")]
	partial class FacilityDropDownViewController
	{
		[Outlet]
		UIKit.UITableView facilityTable { get; set; }

		[Outlet]
		UIKit.UINavigationBar toolbar { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (toolbar != null) {
				toolbar.Dispose ();
				toolbar = null;
			}

			if (facilityTable != null) {
				facilityTable.Dispose ();
				facilityTable = null;
			}
		}
	}
}

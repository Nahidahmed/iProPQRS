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
	[Register ("PatientSectionHeaderView")]
	partial class PatientSectionHeaderView
	{
		[Outlet]
		UIKit.UIView customBackGroundView { get; set; }

		[Outlet]
		UIKit.UIButton disclosurebtn { get; set; }

		[Outlet]
		UIKit.UILabel titleLbl { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (disclosurebtn != null) {
				disclosurebtn.Dispose ();
				disclosurebtn = null;
			}

			if (titleLbl != null) {
				titleLbl.Dispose ();
				titleLbl = null;
			}

			if (customBackGroundView != null) {
				customBackGroundView.Dispose ();
				customBackGroundView = null;
			}
		}
	}
}

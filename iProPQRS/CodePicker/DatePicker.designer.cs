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
	[Register ("DatePicker")]
	partial class DatePicker
	{
		[Outlet]
		UIKit.UIButton btnCancel { get; set; }

		[Outlet]
		UIKit.UIButton btnDone { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem btnTrash { get; set; }

		[Outlet]
		UIKit.UIDatePicker uvDate { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (uvDate != null) {
				uvDate.Dispose ();
				uvDate = null;
			}

			if (btnDone != null) {
				btnDone.Dispose ();
				btnDone = null;
			}

			if (btnCancel != null) {
				btnCancel.Dispose ();
				btnCancel = null;
			}

			if (btnTrash != null) {
				btnTrash.Dispose ();
				btnTrash = null;
			}
		}
	}
}

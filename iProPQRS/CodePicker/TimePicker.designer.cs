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
	[Register ("TimePicker")]
	partial class TimePicker
	{
		[Outlet]
		UIKit.UIButton btnCancel { get; set; }

		[Outlet]
		UIKit.UIButton btnDone { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem btnTrash { get; set; }

		[Outlet]
		UIKit.UIDatePicker uvTimePicker { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnTrash != null) {
				btnTrash.Dispose ();
				btnTrash = null;
			}

			if (btnCancel != null) {
				btnCancel.Dispose ();
				btnCancel = null;
			}

			if (uvTimePicker != null) {
				uvTimePicker.Dispose ();
				uvTimePicker = null;
			}

			if (btnDone != null) {
				btnDone.Dispose ();
				btnDone = null;
			}
		}
	}
}

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
	[Register ("DateTimePicker")]
	partial class DateTimePicker
	{
		[Outlet]
		UIKit.UIToolbar cancelBtn { get; set; }

		[Outlet]
		UIKit.UILabel dateLbl { get; set; }

		[Outlet]
		UIKit.UIDatePicker datePicker { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem doneBtn { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem trashBtn { get; set; }

		[Action ("cancelBtnClicked:")]
		partial void cancelBtnClicked (Foundation.NSObject sender);

		[Action ("doneBtnClicked:")]
		partial void doneBtnClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (cancelBtn != null) {
				cancelBtn.Dispose ();
				cancelBtn = null;
			}

			if (dateLbl != null) {
				dateLbl.Dispose ();
				dateLbl = null;
			}

			if (datePicker != null) {
				datePicker.Dispose ();
				datePicker = null;
			}

			if (doneBtn != null) {
				doneBtn.Dispose ();
				doneBtn = null;
			}

			if (trashBtn != null) {
				trashBtn.Dispose ();
				trashBtn = null;
			}
		}
	}
}

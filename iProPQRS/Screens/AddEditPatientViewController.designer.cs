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
	[Register ("AddEditPatientViewController")]
	partial class AddEditPatientViewController
	{
		[Outlet]
		UIKit.UIButton BackBtn { get; set; }

		[Outlet]
		UIKit.UIButton BtnSubmit { get; set; }

		[Outlet]
		UIKit.UILabel lblAccount { get; set; }

		[Outlet]
		UIKit.UILabel lblhmrn { get; set; }

		[Outlet]
		UIKit.UILabel lblPatientName { get; set; }

		[Outlet]
		UIKit.UISegmentedControl mMenu { get; set; }

		[Outlet]
		UIKit.UIWebView webView { get; set; }

		[Outlet]
		UIKit.UIWebView wvpatient { get; set; }

		[Action ("btnSubmitClicked:")]
		partial void btnSubmitClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (BtnSubmit != null) {
				BtnSubmit.Dispose ();
				BtnSubmit = null;
			}

			if (BackBtn != null) {
				BackBtn.Dispose ();
				BackBtn = null;
			}

			if (lblAccount != null) {
				lblAccount.Dispose ();
				lblAccount = null;
			}

			if (lblhmrn != null) {
				lblhmrn.Dispose ();
				lblhmrn = null;
			}

			if (lblPatientName != null) {
				lblPatientName.Dispose ();
				lblPatientName = null;
			}

			if (mMenu != null) {
				mMenu.Dispose ();
				mMenu = null;
			}

			if (webView != null) {
				webView.Dispose ();
				webView = null;
			}

			if (wvpatient != null) {
				wvpatient.Dispose ();
				wvpatient = null;
			}
		}
	}
}

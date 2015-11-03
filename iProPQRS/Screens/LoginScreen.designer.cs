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
	[Register ("LoginScreen")]
	partial class LoginScreen
	{
		[Outlet]
		UIKit.UIActivityIndicatorView actIndicator { get; set; }

		[Outlet]
		UIKit.UIView actIndicatorView { get; set; }

		[Outlet]
		UIKit.UIButton btnBackFromDomainScreen { get; set; }

		[Outlet]
		UIKit.UIButton BtnCodeOk { get; set; }

		[Outlet]
		UIKit.UIButton btnDomainCode { get; set; }

		[Outlet]
		UIKit.UILabel lblerrormsg { get; set; }

		[Outlet]
		UIKit.UIButton LoginBtn { get; set; }

		[Outlet]
		UIKit.UIScrollView loginScrollView { get; set; }

		[Outlet]
		UIKit.UISwitch rememberLoginUser { get; set; }

		[Outlet]
		UIKit.UITextField TxtCode { get; set; }

		[Outlet]
		UIKit.UITextField txtPassword { get; set; }

		[Outlet]
		UIKit.UITextField txtUserName { get; set; }

		[Outlet]
		UIKit.UIView uvCode { get; set; }

		[Outlet]
		UIKit.UIView uvlog { get; set; }

		[Action ("btnLogin:")]
		partial void btnLogin (Foundation.NSObject sender);

		[Action ("rememberSwitched:")]
		partial void rememberSwitched (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (actIndicator != null) {
				actIndicator.Dispose ();
				actIndicator = null;
			}

			if (actIndicatorView != null) {
				actIndicatorView.Dispose ();
				actIndicatorView = null;
			}

			if (BtnCodeOk != null) {
				BtnCodeOk.Dispose ();
				BtnCodeOk = null;
			}

			if (lblerrormsg != null) {
				lblerrormsg.Dispose ();
				lblerrormsg = null;
			}

			if (LoginBtn != null) {
				LoginBtn.Dispose ();
				LoginBtn = null;
			}

			if (loginScrollView != null) {
				loginScrollView.Dispose ();
				loginScrollView = null;
			}

			if (rememberLoginUser != null) {
				rememberLoginUser.Dispose ();
				rememberLoginUser = null;
			}

			if (TxtCode != null) {
				TxtCode.Dispose ();
				TxtCode = null;
			}

			if (txtPassword != null) {
				txtPassword.Dispose ();
				txtPassword = null;
			}

			if (txtUserName != null) {
				txtUserName.Dispose ();
				txtUserName = null;
			}

			if (uvCode != null) {
				uvCode.Dispose ();
				uvCode = null;
			}

			if (uvlog != null) {
				uvlog.Dispose ();
				uvlog = null;
			}

			if (btnDomainCode != null) {
				btnDomainCode.Dispose ();
				btnDomainCode = null;
			}

			if (btnBackFromDomainScreen != null) {
				btnBackFromDomainScreen.Dispose ();
				btnBackFromDomainScreen = null;
			}
		}
	}
}

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
	[Register ("QualityMetricsASA10")]
	partial class QualityMetricsASA10
	{
		[Outlet]
		UIKit.UIButton brnOk { get; set; }

		[Outlet]
		UIKit.UIButton btnCancel { get; set; }

		[Outlet]
		UIKit.UILabel ulMCItem1 { get; set; }

		[Outlet]
		UIKit.UILabel ulMCItem2 { get; set; }

		[Outlet]
		UIKit.UISwitch usMCItem1 { get; set; }

		[Outlet]
		UIKit.UISwitch usMCItem2 { get; set; }

		[Outlet]
		UIKit.UIScrollView usSubCatView { get; set; }

		[Outlet]
		UIKit.UIView uvMainCat { get; set; }

		[Outlet]
		UIKit.UIView uvSubCatView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnCancel != null) {
				btnCancel.Dispose ();
				btnCancel = null;
			}

			if (brnOk != null) {
				brnOk.Dispose ();
				brnOk = null;
			}

			if (ulMCItem1 != null) {
				ulMCItem1.Dispose ();
				ulMCItem1 = null;
			}

			if (ulMCItem2 != null) {
				ulMCItem2.Dispose ();
				ulMCItem2 = null;
			}

			if (usMCItem1 != null) {
				usMCItem1.Dispose ();
				usMCItem1 = null;
			}

			if (usMCItem2 != null) {
				usMCItem2.Dispose ();
				usMCItem2 = null;
			}

			if (usSubCatView != null) {
				usSubCatView.Dispose ();
				usSubCatView = null;
			}

			if (uvMainCat != null) {
				uvMainCat.Dispose ();
				uvMainCat = null;
			}

			if (uvSubCatView != null) {
				uvSubCatView.Dispose ();
				uvSubCatView = null;
			}
		}
	}
}

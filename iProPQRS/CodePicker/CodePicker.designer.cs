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
	[Register ("CodePicker")]
	partial class CodePicker
	{
		[Outlet]
		UIKit.UIBarButtonItem TrashBtn { get; set; }

		[Outlet]
		UIKit.UITableView utablelist { get; set; }


		
		void ReleaseDesignerOutlets ()
		{
			if (utablelist != null) {
				utablelist.Dispose ();
				utablelist = null;
			}

			if (utListView != null) {
				utListView.Dispose ();
				utListView = null;
			}

			if (TrashBtn != null) {
				TrashBtn.Dispose ();
				TrashBtn = null;
			}
		}
	}
}

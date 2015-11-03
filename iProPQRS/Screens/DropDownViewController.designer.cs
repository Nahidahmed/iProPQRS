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
	[Register ("DropDownViewController")]
	partial class DropDownViewController
	{
		[Outlet]
		UIKit.UILabel lblTitle { get; set; }

		[Outlet]
		UIKit.UITableView ListView { get; set; }

		[Outlet]
		UIKit.UINavigationBar ToolBar { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ListView != null) {
				ListView.Dispose ();
				ListView = null;
			}

			if (ToolBar != null) {
				ToolBar.Dispose ();
				ToolBar = null;
			}

			if (lblTitle != null) {
				lblTitle.Dispose ();
				lblTitle = null;
			}
		}
	}
}

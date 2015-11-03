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
	[Register ("ImagesGalleryView")]
	partial class ImagesGalleryView
	{
		[Outlet]
		UIKit.UIScrollView svimagegallry { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (svimagegallry != null) {
				svimagegallry.Dispose ();
				svimagegallry = null;
			}
		}
	}
}

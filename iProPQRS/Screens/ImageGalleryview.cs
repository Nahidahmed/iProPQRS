
using System;

using Foundation;
using UIKit;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

namespace iProPQRS
{
	public partial class ImageGalleryview : UIViewController
	{
		public ImageGalleryview () : base ("ImageGalleryview", null)
		{
		}
		public UIPopoverController popover;
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		public void PresentFromPopover(UIView sender,float x,float y,float vwidth)
		{
			popover = new UIPopoverController(this)
			{

				PopoverContentSize = new SizeF(vwidth, 600)
			};
			popover.PresentFromRect (new CoreGraphics.CGRect (x, y, 1, 1), sender, UIPopoverArrowDirection.Any, true);

		}
		public void DismissPopOver()
		{
			popover.Dismiss(false);
			popover.Dispose ();
		}
		int yc=0;
		int xc=0;
		int cnt=0;
		public List<UIImageView> currentImagelist=new List<UIImageView>(); 
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

//
//			foreach (var item in currentImagelist) {
//				if (xc >	450) {
//					xc = 0;
//					yc = yc + 115;
//				}
//				item.Frame = new CoreGraphics.CGRect (xc, yc, 250, 210);
//				xc = xc + 155;
//
//				if (item.Image == null) {
//					string str = "";
//				}
//				item.Layer.BorderColor = UIColor.Red.CGColor;
//				item.Layer.BorderWidth = 1;
//				svimagegallry.Add (item);
//
//			}
//			if (currentImagelist.Count > 14) {
//				svimagegallry.SizeToFit ();
//				svimagegallry.ContentSize = new SizeF (float.Parse (svimagegallry.Frame.Width.ToString ()), float.Parse (svimagegallry.Frame.Height.ToString ())+200);
//			}


			//string jpgFilename = System.IO.Path.Combine (documentsDirectory, "Photo.jpg");
			//UIImage currentImage = UIImage.FromFile (jpgFilename);

			// Perform any additional setup after loading the view, typically from a nib.
		}
	}
}


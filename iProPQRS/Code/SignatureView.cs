using System;
using UIKit;
using System.Drawing;
using Foundation;

namespace iProPQRS
{
	public class SignatureView : UIView
	{       

		public delegate void SignatureChanged ();

		public SignatureChanged OnSignatureChanged;
		private bool _empty = true;


		UIBezierPath path;

		UIImage incrementalImage;

		PointF[] pts = new PointF[5];

		uint ctr;

		[Export ("initWithFrame:")]

		public SignatureView (RectangleF rect): base(rect)

		{

			this.MultipleTouchEnabled = false;

			this.BackgroundColor = UIColor.Clear;

			path = new UIBezierPath();

			path.LineWidth = 2;

		}
		public bool IsEmpty()
		{
			return incrementalImage == null && ctr == 0;
		}
		public void Clear()
		{
			if(incrementalImage != null)
			{
				incrementalImage.Dispose ();
				incrementalImage = null;
			}
			path.RemoveAllPoints ();
			SetNeedsDisplay ();

		}

		[Export("initWithCoder:")]

		public SignatureView (NSCoder coder) : base(coder)

		{

			this.MultipleTouchEnabled = false;

			this.BackgroundColor = UIColor.Clear;

			path = new UIBezierPath();

			path.LineWidth = 2;

		}
		public override void Draw (CoreGraphics.CGRect rect)
		{
			if (incrementalImage != null)
				incrementalImage.Draw(rect);

			path.Stroke();
			//base.Draw (rect);
		}
		//public void Draw (RectangleF rect)
		//{

		//	if (incrementalImage != null)
		//		incrementalImage.Draw(rect);

		//	path.Stroke();

		//}

		public override void TouchesBegan (NSSet touches, UIEvent evt)

		{


			ctr = 0;

			UITouch touch = touches.AnyObject as UITouch;

			pts[0] = (PointF)touch.LocationInView(this);

		}

		public override void TouchesMoved (NSSet touches, UIEvent evt)
		{
			if(OnSignatureChanged != null)
				OnSignatureChanged ();

			UITouch touch = touches.AnyObject as UITouch;

			PointF p = (PointF)touch.LocationInView(this);

			ctr++;

			pts[ctr] = p;


			if (ctr == 3)
			{
				pts[2] = new PointF((pts[1].X + pts[3].X)/2.0f, (pts[1].Y + pts[3].Y)/2.0f);
				path.MoveTo(pts[0]);
				path.AddQuadCurveToPoint (pts [2], pts [1]);

				this.SetNeedsDisplay ();
				pts[0] = pts[2];
				pts[1] = pts[3];
				ctr = 1;
			}

		}

		public override void TouchesEnded (NSSet touches, UIEvent evt)

		{

			if (ctr == 0) // only one point acquired = user tapped on the screen
			{
				path.AddArc (pts [0], path.LineWidth / 2, 0, (float)(Math.PI * 2), true);
			}
			else if (ctr == 1)
			{
				path.MoveTo (pts [0]);
				path.AddLineTo (pts [1]);
			}
			else if (ctr == 2)
			{
				path.MoveTo (pts [0]);
				path.AddQuadCurveToPoint (pts [2], pts [1]);
			}

			this.drawBitmap();
			this.SetNeedsDisplay();


			path.RemoveAllPoints();

			ctr = 0;

		}

		public override void TouchesCancelled (NSSet touches, UIEvent evt)

		{

			this.TouchesEnded(touches, evt);

		}
		public UIImage GetDrawingImage ()
		{
			UIGraphics.BeginImageContextWithOptions(this.Bounds.Size, false, 0);

			if(incrementalImage == null)
			{
				incrementalImage = new UIImage ();
				UIBezierPath rectPath = UIBezierPath.FromRect(this.Bounds);
				UIColor.Clear.SetFill();
				rectPath.Fill();
			}

			incrementalImage.Draw(new PointF(0,0));

			UIColor.Black.SetStroke();

			path.Stroke();

			incrementalImage = UIGraphics.GetImageFromCurrentImageContext();

			UIGraphics.EndImageContext();
			return incrementalImage;
		}
		public void drawBitmap()
		{
			UIGraphics.BeginImageContextWithOptions(this.Bounds.Size, false, 0);

			if(incrementalImage == null)
			{
				incrementalImage = new UIImage ();
				UIBezierPath rectPath = UIBezierPath.FromRect(this.Bounds);
				UIColor.Clear.SetFill();
				rectPath.Fill();
			}

			incrementalImage.Draw(new PointF(0,0));

			UIColor.Black.SetStroke();

			path.Stroke();

			incrementalImage = UIGraphics.GetImageFromCurrentImageContext();

			UIGraphics.EndImageContext();

		}


	}
}


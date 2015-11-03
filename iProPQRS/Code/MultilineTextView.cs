using System;
using System.Drawing;
using UIKit;
using CoreGraphics;


namespace iProPQRS
{
	public class MultilineTextView:UITextView
	{
		public MultilineTextView ()
		{
			this.Layer.BorderColor=new CoreGraphics.CGColor(0.5f, 0.5f); 
			this.Layer.BorderWidth = 1.3f;
			this.Layer.CornerRadius = 6;
			this.ClipsToBounds = true;
			this.Font = UIFont.FromName ("ArialMT", 15);
		}
		public MultilineTextView (CGRect Frame):base(Frame)
		{
			this.Frame = Frame;
			//Black
			this.Layer.BorderColor=new CoreGraphics.CGColor(0.5f, 0.5f); 
			this.Layer.BorderWidth = 1.3f;
			this.Layer.CornerRadius = 6;
			this.ClipsToBounds = true;
			this.Font = UIFont.FromName ("ArialMT", 15);
		}

	}
}



using System;

using Foundation;
using UIKit;
using System.Drawing;
using System.Globalization;

namespace iProPQRS
{
	public delegate void TimePickerSelectedEvent();
	public partial class TimePicker : UIViewController
	{
		public TimePicker () : base ("TimePicker", null)
		{
		}
		private UIPopoverController popover;
		public event TimePickerSelectedEvent _ValueChanged;
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();


			btnCancel.TouchUpInside += (object sender, EventArgs e) => {
				popover.Dismiss (false);
			};

			btnDone.TouchUpInside += (object sender, EventArgs e) => {
				DismissPopOver(string.Empty);
			};

			btnTrash.Clicked += (object sender, EventArgs e) => {
//				DismissPopOver("trashedClicked");
				popover.Dismiss (false);
				DateTime dt = new DateTime ();
				dt = DateTime.MinValue;
				SelectedTime = dt.ToString ("HH:mm");

				_ValueChanged += new TimePickerSelectedEvent(checkVal);

				_ValueChanged.Invoke ();

			};
			// Perform any additional setup after loading the view, typically from a nib.
		}
		public string SelectedTime {
			get;
			set;
		} 
		public void DismissPopOver(string trashedClicked)
		{
			popover.Dismiss (false);
			DateTime dt = new DateTime ();	
//			dt = NSDateToDateTime (uvTimePicker.Date);
			dt = NSDateToDateTime1(uvTimePicker.Date);
			if (trashedClicked == string.Empty)
				SelectedTime = dt.ToString ("HH:mm");
			else
				SelectedTime = "";
			
			_ValueChanged += new TimePickerSelectedEvent(checkVal);

			_ValueChanged.Invoke ();
		}
		public static DateTime NSDateToDateTime(NSDate date)
		{
			DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime( 
				new DateTime(2001, 1, 1, 0, 0, 0) );
			return reference.AddSeconds(date.SecondsSinceReferenceDate);
		}

		public static DateTime NSDateToDateTime1(NSDate date)
		{
			NSDate sourceDate = date;

			NSTimeZone sourceTimeZone = new NSTimeZone ("UTC");
			NSTimeZone destinationTimeZone = NSTimeZone.LocalTimeZone;

			int sourceGMTOffset =(int) sourceTimeZone.SecondsFromGMT (sourceDate);
			int destinationGMTOffset =(int) destinationTimeZone.SecondsFromGMT (sourceDate);
			int interval = destinationGMTOffset - sourceGMTOffset;

			var destinationDate = sourceDate.AddSeconds (interval);

			var dateTime = new DateTime(2001, 1, 1, 0, 0,0).AddSeconds(destinationDate.SecondsSinceReferenceDate);

			return dateTime;
		}
		public void checkVal()
		{

			//DismissPopOver(this.Selecteditem);
		}
		public void PresentFromPopover(UIView sender,float x,float y){
			popover = new UIPopoverController(this)
			{
				PopoverContentSize = new SizeF(383, 337)
			};
			popover.PresentFromRect (new CoreGraphics.CGRect (x, y, 1, 1), sender, UIPopoverArrowDirection.Any, true);
		}
	}
}


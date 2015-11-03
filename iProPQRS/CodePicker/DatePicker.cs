
using System;

using Foundation;
using UIKit;
using System.Drawing;

namespace iProPQRS
{
	public delegate void DatePickerSelectedEvent();
	public partial class DatePicker : UIViewController
	{
		public DatePicker () : base ("DatePicker", null)
		{
		}

		private UIPopoverController popover;
		public event DatePickerSelectedEvent _ValueChanged;
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
		public DateTime MaximumDate {
			get;
			set;
		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();	

			DateTime defaultdate=Convert.ToDateTime("1/1/0001 12:00:00 AM");

			if (MaximumDate != null&& MaximumDate != defaultdate)
				uvDate.MaximumDate = (NSDate) (DateTime.SpecifyKind(MaximumDate, DateTimeKind.Utc));
			// Perform any additional setup after loading the view, typically from a nib.

			if (SelectedDateValue != null && SelectedDateValue != defaultdate) {
				NSDate pdate =(NSDate) (DateTime.SpecifyKind(SelectedDateValue, DateTimeKind.Utc));
				uvDate.SetDate (pdate, true);
			}

			btnDone.TouchUpInside += (object sender, EventArgs e) => {
				DismissPopOver();
			};

			btnCancel.TouchUpInside += (object sender, EventArgs e) => {
				popover.Dismiss (false);
			};

			btnTrash.Clicked += (object sender, EventArgs e) => {
				popover.Dismiss (false);
				DateTime dt = new DateTime ();
				dt = DateTime.MinValue;
				SelectedDateValue = dt;
				SelectedDate = dt.ToString ("MM/dd/yyyy");

				_ValueChanged += new DatePickerSelectedEvent(checkVal);

				_ValueChanged.Invoke ();

			};

		}

		public DateTime SelectedDateValue {
			get;
			set;
		}
		public string SelectedDate {
			get;
			set;
		} 
		public void DismissPopOver()
		{
			popover.Dismiss (false);
			DateTime dt = new DateTime ();
			dt = (DateTime)uvDate.Date;
			SelectedDateValue = dt;
			SelectedDate = dt.ToString ("MM/dd/yyyy");
				
			_ValueChanged += new DatePickerSelectedEvent(checkVal);

			_ValueChanged.Invoke ();
		}
		public void checkVal()
		{

			//DismissPopOver(this.Selecteditem);
		}
		public void PresentFromPopover(UIView sender,float x,float y)
		{
			popover = new UIPopoverController(this)
			{
				PopoverContentSize = new SizeF(390, 300)
			};
			popover.PresentFromRect (new CoreGraphics.CGRect (x, y, 1, 1), sender, UIPopoverArrowDirection.Any, true);

		}
	}
}


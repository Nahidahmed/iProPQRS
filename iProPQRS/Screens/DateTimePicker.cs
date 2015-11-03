
using System;

using Foundation;
using UIKit;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using iProPQRSPortableLib;

namespace iProPQRS
{
	public class DatePickerNotify :INotifyPropertyChanged
	{
		private static DatePickerNotify instance;

		public DateTime SelectedDateValue {get;set;}

		private string labelText;

		public string LabelText
		{
			get { return labelText; }
			set
			{
				if (labelText != value)
				{
					labelText = value;
					NotifyChange();
				}
			}
		}

		public static DatePickerNotify Instance
		{
			get { return instance ?? (instance = new DatePickerNotify()); }
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		private void NotifyChange([CallerMemberName] String propertyName = "")
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	public partial class DateTimePicker : UIViewController
	{
		PatientListView patListView;
		UIActivityIndicatorView actView;

		public DateTimePicker (PatientListView patListView) : base ("DateTimePicker", null)
		{
			this.patListView = patListView;
		}
		private UIPopoverController popover;

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		public DateTime SelectedDateValue;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		
			DatePickerNotify.Instance.SelectedDateValue = SelectedDateValue;
			if (SelectedDateValue != null )
				datePicker.Date = (NSDate) (DateTime.SpecifyKind(SelectedDateValue, DateTimeKind.Utc));
			
			datePicker.ValueChanged += (sender, args) => {
				DatePickerNotify.Instance.SelectedDateValue = (DateTime)datePicker.Date;
				DatePickerNotify.Instance.LabelText = ((DateTime)datePicker.Date).ToString ("d");
				iProPQRSPortableLib.Consts.DataRetrieveDate = DatePickerNotify.Instance.SelectedDateValue.ToString("yyyyMMdd");
				SelectedDateValue=DatePickerNotify.Instance.SelectedDateValue;
//				iProPQRSPortableLib.Consts.DataRetrieveDate = DatePickerNotify.Instance.LabelText;
			};

			doneBtn.Clicked += async (sender, e) => {
				iProPQRSPortableLib.Consts.DataRetrieveDate = DatePickerNotify.Instance.SelectedDateValue.ToString("yyyyMMdd");
				this.patListView.dismissDatePicker("done");
			};
			// Perform any additional setup after loading the view, typically from a nib.

		}

		partial void cancelBtnClicked (NSObject sender)
		{
			this.patListView.dismissDatePicker("cancel");
		}


		public void PresentFromPopover(UIView sender)
		{
			popover = new UIPopoverController(this)
			{
				PopoverContentSize = new SizeF(320, 250)
			};

			var frame = new RectangleF(0, 0,(float)sender.Frame.Width, (float)sender.Frame.Height);
			popover.PresentFromRect(frame, sender, UIPopoverArrowDirection.Down, true);
		}
	}

}


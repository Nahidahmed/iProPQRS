
using System;

using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Drawing;

namespace iProPQRS
{
	public delegate void mlsCodePickerSelectedEvent();
	public partial class mlsCodePicker : UIViewController
	{
		public mlsCodePicker () : base ("mlsCodePicker", null)
		{
		}
		public float uvWidth=800;
		public float uvheight=480;
		public int TypeItemID=406;
		public string TypeValue="0582F";
		List<CodePickerModel> RootData;
		public List<CodePickerModel> SubRootData;
		public List<CodePickerModel> SelectedSubItems = new List<CodePickerModel>();
		public List<CodePickerModel> SelectedItems = new List<CodePickerModel>();
		UIViewController ViewController=null;
		public UIPopoverController popover;
		public event mlsCodePickerSelectedEvent _ValueChanged;
		public bool isMultiSelect=true;
		public bool isMainCatMultiSelect=false;
		public mlsCodePicker (UIViewController ViewController,float uWidth,List<CodePickerModel> RootData ) : base ("mlsCodePicker", null)
		{
			uvWidth = uWidth;
			this.RootData = RootData;
			this.ViewController=ViewController;		
		}
		public mlsCodePicker (UIViewController ViewController,float uWidth,List<CodePickerModel> RootData,List<CodePickerModel> SubRootData ) : base ("mlsCodePicker", null)
		{
			uvWidth = uWidth;
			this.RootData = RootData;
			this.SubRootData = SubRootData;
			this.ViewController=ViewController;		
		}
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		public  RootGroupView rvc;
		public   SubGroupView sgv;	
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			sgv = new SubGroupView ("g", this,SubRootData);
			rvc = new RootGroupView (RootData,this,sgv);
			sgv.Selecteditems = SelectedSubItems;
			sgv.View.Hidden = true;		
			this.View.Add (rvc.View);
			this.View.Add (sgv.View);

			// Perform any additional setup after loading the view, typically from a nib.
		}
		public void RemoveSelectedItem(CodePickerModel item)
		{
			SelectedItems.Remove(item);
		}
		public void PresentFromPopover(UIView sender,float x,float y)
		{
			popover = new UIPopoverController(this)
			{

				PopoverContentSize = new SizeF(uvWidth, 550)
			};

			popover.PresentFromRect (new CoreGraphics.CGRect (x, y, 1, 1), sender, UIPopoverArrowDirection.Any, true);
			this.View.Layer.Frame = new CoreGraphics.CGRect (0, 0, uvWidth, uvheight);
			this.View.Frame = new CoreGraphics.CGRect (0, 0, uvWidth, uvheight);
		}
		public string SelectedText {
			get;
			set;
		}
		public void DismissPopOver()
		{			
			popover.Dismiss(false);
			_ValueChanged += new mlsCodePickerSelectedEvent(checkVal);
			_ValueChanged.Invoke ();

		}
		public void checkVal()
		{

			//DismissPopOver(this.Selecteditem);
		}
	}

}


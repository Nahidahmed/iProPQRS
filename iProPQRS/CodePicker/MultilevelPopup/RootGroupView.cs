using System;
using UIKit;
using System.Collections.Generic;
using iProPQRSPortableLib;
using CoreGraphics;

namespace iProPQRS
{
	public class RootGroupView:UIViewController
	{
		public static mlsCodePicker pview;
		SubGroupView subview;
		public RootGroupView (List<CodePickerModel>  RootData ,mlsCodePicker popoverview,SubGroupView subview)
		{
			this.RootData = RootData;
			pview = popoverview;
			this.subview = subview;
		}
		public List<CodePickerModel>  RootData {
			get;
			set;
		}
		UINavigationBar NavBar;
		UISearchBar searchBar;
		public  RootViewController rvc;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			NavBar=new UINavigationBar(new CoreGraphics.CGRect (0, 0, pview.uvWidth, 44));
			NavBar.BackgroundColor = UIColor.Red;
//			UIBarButtonItem bbitemCancel = new UIBarButtonItem (UIBarButtonSystemItem.Cancel, CancelButtonClicked);
			UIButton btnCancel = new UIButton (new CGRect (0, 0, 80, 30));
			btnCancel.SetTitleColor (UIColor.Blue, UIControlState.Normal);
			btnCancel.SetTitle ("Cancel", UIControlState.Normal);
			btnCancel.TouchUpInside += (object sender, EventArgs e) => {
				pview.popover.Dismiss(false);
			};
			UIBarButtonItem bbitemCancel = new UIBarButtonItem (btnCancel);

//			UIBarButtonItem bbitemDone = new UIBarButtonItem (UIBarButtonSystemItem.Done, DoneButtonClicked);
			UIButton btnDone = new UIButton (new CGRect (0, 0, 80, 30));
			btnDone.SetTitleColor (UIColor.Blue, UIControlState.Normal);
			btnDone.SetTitle ("Done", UIControlState.Normal);
			btnDone.TouchUpInside += (object sender, EventArgs e) => {
				pview.DismissPopOver ();
			};
			UIBarButtonItem bbitemDone = new UIBarButtonItem (btnDone);

			UINavigationItem navgitem = new UINavigationItem ("Select");
			navgitem.SetLeftBarButtonItem(bbitemCancel,true);
			navgitem.SetRightBarButtonItem (bbitemDone, true);
			NavBar.PushNavigationItem(navgitem,true);
			this.View.Add (NavBar);
			searchBar=new UISearchBar(new CoreGraphics.CGRect (0, 44, pview.uvWidth, 44));
			this.View.Add(searchBar);
			rvc = new RootViewController (RootData,pview);
			rvc.View.Frame = new CoreGraphics.CGRect (0, 88, pview.uvWidth, 600);
			this.subview.SetRootview(rvc);
			this.View.Add (rvc.View);
		}
		void CancelButtonClicked (object sender, EventArgs e)
		{
			pview.popover.Dismiss(false);
		}
		async void DoneButtonClicked (object sender, EventArgs e)		{
			pview.DismissPopOver ();
			//DismissPopOver();
		}
	}
}


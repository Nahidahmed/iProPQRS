using System;
using UIKit;
using MBProgressHUD;


namespace iProPQRS
{
	public class ProgressElement
	{
		MTMBProgressHUD hud;
		public ProgressElement()
		{
		 
		}
		public void Start(UIView View,string caption)
		{
			hud = new MTMBProgressHUD (View) {
				LabelText = caption,
				RemoveFromSuperViewOnHide = true
			};
			View.AddSubview (hud);
			hud.Show (animated: true);
		}
		public void Stop()
		{
			if(hud!=null)
			  hud.Hide (animated: true, delay: 0);
		}

	}
}


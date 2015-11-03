using System;
using Foundation;
using UIKit;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using iProPQRSPortableLib;
using System.Threading.Tasks;
using System.Json;
using iProPQRSPortableLib.BL;
using System.Threading;
using System.Collections.Generic;
using Twitter;
using MessageUI;
using CoreGraphics;
using Newtonsoft.Json.Schema;
using System.Drawing;

namespace iProPQRS
{
	public partial class LoginScreen : UIViewController
	{
//		RazorViewController webViewController;

		public LoginScreen () : base ("LoginScreen", null)
		{
		}



		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}
		 
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			btnBackFromDomainScreen.Hidden = true;
			var value = NSUserDefaults.StandardUserDefaults;
			if (string.IsNullOrEmpty (value.StringForKey ("authcode"))) {
				loginScrollView.Hidden = true;
				uvCode.BecomeFirstResponder ();
				uvCode.Hidden = false;
			} else {
				loginScrollView.Hidden = false;
				loginScrollView.BecomeFirstResponder ();
				uvCode.Hidden = true;
			}

			btnDomainCode.TouchUpInside += (object sender, EventArgs e) => {
				btnBackFromDomainScreen.Hidden = false;
				loginScrollView.Hidden = true;
				uvCode.BecomeFirstResponder ();
				uvCode.Hidden = false;
			};

			btnBackFromDomainScreen.TouchUpInside += (object sender, EventArgs e) => {
				btnBackFromDomainScreen.Hidden = true;
				loginScrollView.Hidden = false;
				loginScrollView.BecomeFirstResponder ();
				uvCode.Hidden = true;
			};
			this.txtUserName.ShouldReturn += (textField) => {
				textField.ResignFirstResponder();
				txtPassword.BecomeFirstResponder();
				return true;
			};
			this.txtPassword.ShouldReturn += (textField) => {
				EventArgs e=null;
				textField.ResignFirstResponder();
				HandleLoginClick(textField,e);
				return true;
			};

			this.NavigationController.NavigationBarHidden = true;
//			var value = NSUserDefaults.StandardUserDefaults;
			rememberLoginUser.On = value.BoolForKey ("Remember");

			if (rememberLoginUser.On)
				txtUserName.Text = value.StringForKey ("txtUserName");

			uvCode.Alpha = 1.0f;
			uvCode.BackgroundColor=

				UIColor.FromWhiteAlpha(0.0f, 0.0f);
				BtnCodeOk.TouchUpInside+= async (object sender, EventArgs e) => {
				AppDelegate.pb.Start(this.View,"Please wait...");
				lblerrormsg.Text = string.Empty;
				if(!string.IsNullOrEmpty(TxtCode.Text))
				{	
					RcvdJSONData context= await AppDelegate.Current.pqrsMgr.AuthenticateCode(TxtCode.Text.Trim());

					if(context!=null && context.results != null && context.status !=null && context.status.ToUpper() =="OK" )
					{	
						AuthCode authcode = (AuthCode)JsonConvert.DeserializeObject(context.results.ToString() , typeof(AuthCode));
						if(context.status.ToUpper() == "OK")
						{
							iProPQRSPortableLib.Consts.BaseServerURL = authcode.ServiceUrl;

							value.SetString(TxtCode.Text.Trim(),"authcode");
							loginScrollView.Hidden = false;
							uvCode.Hidden = true;
						}
						else					
							lblerrormsg.Text = context.message;
					}
					else					
						lblerrormsg.Text = context.message;

				}
				else
					lblerrormsg.Text = "Please Enter Code";

				AppDelegate.pb.Stop();
			};
			//Test Json schmea generation
//			Newtonsoft.Json.Schema.JsonSchemaGenerator 
//			JsonSchemaGenerator jSonSchemaGen = new JsonSchemaGenerator();
//			JsonSchema jschema = jSonSchemaGen.Generate(typeof(UserMaster));
//			Console.WriteLine(jschema.ToString());

//			webViewController = new RazorViewController ();
//			CGRect bouns = this.View.Bounds;		
//			NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Add), false);
//			NavigationItem.RightBarButtonItem.Clicked += (sender, e) => { 
////				var template = new ToDoView () { Model = new ToDoItem() };
////				var page = template.GenerateString ();
////				webViewController.webView.LoadHtmlString (page, NSBundle.MainBundle.BundleUrl);
////				NavigationController.PushViewController(webViewController, true);
//
//				AddEditPatientViewController addpatient = new AddEditPatientViewController(new PatientProfile());
//				this.NavigationController.PushViewController(addpatient,true);
//
//			};


//			var webView = new UIWebView (UIScreen.MainScreen.Bounds);
//			string contentDirectoryPath = Path.Combine (NSBundle.MainBundle.BundlePath, "Content/");
//			var html = "<html><h1>Hello</h1><p>World</p></html>";
//			webView.LoadHtmlString(html, NSBundle.MainBundle.BundleUrl);
//			webView.ScalesPageToFit = false;
//			webView.ShouldStartLoad += HandleShouldStartLoad;

//			if(this.txtUserName.Text.Length==0){
//				this.txtUserName.BecomeFirstResponder();
//			}else if(this.txtPassword.Text.Length==0){
//				this.txtPassword.BecomeFirstResponder();
//			}

//			this.txtUserName.Delegate = new ValidateTextFieldDelegate (25," Email ");

			// make 'return' on last text field save and close the form
			//this.txtPassword.ShouldReturn = delegate(UITextField textField) {
			//	this.txtPassword.ResignFirstResponder();
		//		return true;
		//	};

//			this.txtPassword.Delegate = new ValidateTextFieldDelegate (25," Password ");

			//this.actIndicatorView.Hidden = true;
			//actIndicator.StopAnimating ();

			LoginBtn.TouchUpInside += async (sender, e) => {
				txtUserName.ResignFirstResponder();
				txtPassword.ResignFirstResponder();
				HandleLoginClick(sender,e);
				//	this.actIndicatorView.Hidden = true;
				//	actIndicator.StopAnimating();

			};
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public async void HandleLoginClick(object sender,EventArgs e){
			try{
				 
				//this.actIndicatorView.Hidden = false;
				//actIndicator.StartAnimating();
				AppDelegate.pb.Start(this.View,"Loading");
				NetworkStatus internetStatus = Reachability.InternetConnectionStatus();

				if (internetStatus != NetworkStatus.NotReachable) {
					PatientProcedureInfo rootObject = await AppDelegate.Current.pqrsMgr.AuthenticationGateway(txtUserName.Text.Trim(),txtPassword.Text.Trim());
					iProPQRSPortableLib.Consts.RcvdJSONDataResult =await AppDelegate.Current.pqrsMgr.GetMACCodes();
					this.BeginInvokeOnMainThread(
						() => {
							Console.WriteLine("Gateway: "+ rootObject.status);
							if(rootObject.status == "ok"){
								iProPQRSPortableLib.Consts.mId=iProPQRSPortableLib.Consts.AuthenticationUserID;	//"560";

								iProPQRSPortableLib.Consts.LoginUserName = this.txtUserName.Text.Trim();
								PatientListView patListView = new PatientListView(rootObject);
								this.NavigationController.PushViewController(patListView,false);
							}else{
								AppDelegate.pb.Stop();
								ShowAlertMessage("Login Failed. Please enter valid email/password.");
							}
						}
					);
				}else{
					AppDelegate.pb.Stop();
					ShowAlertMessage("Please check the network connectivity.");
				}
			}
			catch ( Exception ex)
			{
				ShowAlertMessage("Error  "+ex.Message );
				AppDelegate.pb.Stop();
			}
		}
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			var value = NSUserDefaults.StandardUserDefaults;
			if (!value.BoolForKey ("Remember")) {
				txtUserName.Text = string.Empty;
				txtPassword.Text = string.Empty;
			}

		}
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			txtUserName.BecomeFirstResponder ();
		}
		partial void rememberSwitched (NSObject sender)
		{
			var user = NSUserDefaults.StandardUserDefaults;
			user.SetBool(rememberLoginUser.On,"Remember");

			if(!rememberLoginUser.On){
				user.SetString(string.Empty,"txtUserName");
			}else{
				user.SetString(txtUserName.Text,"txtUserName");
			}
		}

		private bool loginValidations()
		{
			bool retVal = true;

			if (this.txtUserName.Text.Trim () == string.Empty || this.txtPassword.Text.Trim () == string.Empty) {
				//actIndicator.StopAnimating ();
			//	this.actIndicatorView.Hidden = true;
				ShowAlertMessage ("Please enter valid email/password.");	
				retVal = false;
			}

			return retVal;
		}

		private void ShowAlertMessage(string alrtMessag)
		{
			UIAlertView alrtMsg = new UIAlertView();
			alrtMsg.Message = alrtMessag;
			alrtMsg.AddButton("Ok"); 
			alrtMsg.Show();
		}
	}
}


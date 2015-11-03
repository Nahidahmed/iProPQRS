using Foundation;
using UIKit;
using iProPQRSPortableLib.BL.PQRSManager;

namespace iProPQRS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		UINavigationController navController;
		UIViewController loginscreen;
		public static iProPQRS.ProgressElement pb=new ProgressElement();
		public static AppDelegate Current { get; private set; }
		public PQRSManager pqrsMgr { get; set; }

		public override UIWindow Window {
			get;
			set;
		}
		public static void Navigatelogin()
		{	
			
			//NavigationController.PopViewController(true);
		}
		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			Current = this;
			// Override point for customization after application launch.
			// If not required for your application you can safely delete this method

			// Code to start the Xamarin Test Cloud Agent
			#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
			#endif
		
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			// make the window visible
			window.MakeKeyAndVisible ();

			// create our nav controller
			navController = new UINavigationController ();

			// create our home controller based on the device
			//			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
			loginscreen = new iProPQRS.LoginScreen();
//			loginscreen = new iProPQRS.PatientListView();

		
			// Styling
			UINavigationBar.Appearance.TintColor = UIColor.FromRGB (38, 117 ,255); // nice blue
			UITextAttributes ta = new UITextAttributes();
			ta.Font = UIFont.FromName ("AmericanTypewriter-Bold", 0f);
			UINavigationBar.Appearance.SetTitleTextAttributes(ta);
			ta.Font = UIFont.FromName ("AmericanTypewriter", 0f);
			UIBarButtonItem.Appearance.SetTitleTextAttributes(ta, UIControlState.Normal);

			//To-Do
			//DB initialization

			// push the view controller onto the nav controller and show the window
			navController.PushViewController(loginscreen, false);
			window.RootViewController = navController;



			window.MakeKeyAndVisible ();
			pqrsMgr = new PQRSManager ();

			return true;
		}

		public override void OnResignActivation (UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground (UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
		}

		public override void WillEnterForeground (UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated (UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate (UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}
	}
}




using System;

using Foundation;
using UIKit;
using System.Collections.Generic;
using iProPQRSPortableLib;
using System.Globalization;
using System.Drawing;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace iProPQRS
{
	public partial class PatientListView : UIViewController
	{
		PQRSServices serv=new PQRSServices();
		DateTimePicker datePicker;
		PatientTableSource tableSource;
		List<PatientItemGroup> tableItems;
		List<ProcedureItemGroup> procTableItems;
		bool isViewPushed = false;

		public UIButton FacilityDropDownBtn
		{
			get{ return this.facilityDropDownBtn;}
			set{ this.facilityDropDownBtn = value;}
		}
		public PatientProcedureInfo rootObj;

		public PatientListView (PatientProcedureInfo rootObj) : base ("PatientListView", null)
		{
			this.rootObj = rootObj;
			isViewPushed = true;
		}

		public UITableView PatientTableView
		{
			get{return this.mTableView; }
			set{ this.mTableView = value ;}
		}
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewWillAppear (bool animated)
		{
			
			base.ViewWillAppear (animated);
			if (isViewPushed == false) {				
				RefreshData ();
			}
		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			SelectedDateValue = DateTime.Now;
			this.NavigationController.NavigationBarHidden = true;
			this.searchBar.SetScopeBarButtonBackgroundImage (new UIImage ("tallToolBarOff"), UIControlState.Normal);
			this.searchBar.SetScopeBarButtonBackgroundImage (new UIImage ("tallToolBarOn"), UIControlState.Selected);		
			this.searchBar.TintColor = UIColor.Black;
			PopulateTableItems ();
			this.mTableView.Source = tableSource;		
			this.searchBar.TextChanged += (object sender, UISearchBarTextChangedEventArgs e) => 
			{
				
				searchBar.ShowsCancelButton = true;
				searchTable();
			};

			searchBar.SelectedScopeButtonIndexChanged += (object sender, UISearchBarButtonIndexEventArgs e) => {
				searchBar.ShowsCancelButton = true;
				searchTable();
			};
//			searchBar.SelectedScopeButtonIndexChanged+= (object sender, UISearchBarButtonIndexEventArgs e) => {
//
//				PatientTableSource tableSource;
//				List<PatientItemGroup> tableItems=new List<PatientItemGroup>();//null;
//				PatientData pData=new PatientData();
//				if(e.SelectedScope==0)
//					tableItems=pData.Gettab1Patient(this.rootObj);
//      			else if(e.SelectedScope==1)
//					tableItems=pData.Gettab2Surgeon(this.rootObj);
//				else if(e.SelectedScope==2)
//					tableItems=pData.Gettab3AnesName(this.rootObj);
//				else if(e.SelectedScope==3)
//				{
//
//				}
//
//				if(tableItems!=null)
//				{
//
//					tableSource = new PatientTableSource (tableItems,this);
//					this.mTableView.Source=tableSource;
//					this.mTableView.ReloadData();
//				}
//			};

			this.searchBar.SearchButtonClicked+= (object sender, EventArgs e) => {
				this.searchBar.EndEditing(true);
				searchBar.ShowsCancelButton = false;
				this.searchBar.ResignFirstResponder();
			};

			this.searchBar.CancelButtonClicked += (object sender, EventArgs e) => {
				searchBar.Text=string.Empty;
				searchTable();
				searchBar.ResignFirstResponder();
				searchBar.ShowsCancelButton = false;
			};

			logOutBtn.TouchUpInside += (object sender, EventArgs e) => {
				this.NavigationController.PopViewController(false);
			};

			if(datePickerBtn.TitleLabel.Text == null)
				datePickerBtn.SetTitle (DateTime.Now.ToString("d"), UIControlState.Normal);


			DatePickerNotify.Instance.PropertyChanged += (sender, args) => {
				this.InvokeOnMainThread (() => datePickerBtn.SetTitle (DatePickerNotify.Instance.LabelText, UIControlState.Normal));
			};

			refreshBtn.TouchUpInside += async (sender, e) => {
				RefreshData();
			};

			if (FacilityDropDownBtn.TitleLabel.Text == null) {
				foreach (FacilityDetails fac in iProPQRSPortableLib.Consts.Facilities.result) {
					if(fac.FMID.ToString() == iProPQRSPortableLib.Consts.SelectedFacilityID)
						facilityDropDownBtn.SetTitle (fac.FacilityName, UIControlState.Normal);
				}
			}
		
			this.AddNewPatientBtn.TouchUpInside+= async (object sender, EventArgs e) => {	
				isViewPushed = false;
				AppDelegate.pb.Start(this.View,"Loading");
				AddPatientViewController addpatient = new AddPatientViewController();
				if(iProPQRSPortableLib.Consts.lstOfUsers != null)
				{
					addpatient.listOfAnestheologists = iProPQRSPortableLib.Consts.lstOfUsers .FindAll(x => x.Role == "Anesthesiologist");
					addpatient.listOfCRNAs = iProPQRSPortableLib.Consts.lstOfUsers .FindAll(x => x.Role == "CRNA");
				}
				AppDelegate.pb.Stop();
				iProPQRSPortableLib.Consts.SelectedProcAttribtslist = null;
				this.NavigationController.PushViewController(addpatient,true);
			};
			AppDelegate.pb.Stop();
		}
		public DateTime SelectedDateValue;
		partial void datePickerClicked (NSObject sender)
		{
			datePicker = new DateTimePicker(this);
			datePicker.SelectedDateValue = SelectedDateValue;
			datePicker.PresentFromPopover(datePickerBtn);
		}

		partial void facilityClicked (NSObject sender)
		{
			FacilityDropDownViewController facilityView = new FacilityDropDownViewController(this);
			facilityView.PresentFromPopover(facilityDropDownBtn);
		}

		public void SetSelectedFacility(string selectedFacility)
		{
			facilityDropDownBtn.SetTitle (selectedFacility, UIControlState.Normal);
			DownloadData ();
		}

		public async Task<PatientProcedureInfo> DownloadData()
		{
			
			UIActivityIndicatorView actView = new UIActivityIndicatorView();
			actView.Frame = this.View.Bounds;
			actView.BackgroundColor = UIColor.LightTextColor;
			actView.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.WhiteLarge;
			actView.Color = UIColor.Black;
			actView.HidesWhenStopped = true;
			//this.View.Add(actView);
			//actView.StartAnimating();
			AppDelegate.pb.Start (this.View, "Loading");
			NetworkStatus internetStatus = Reachability.InternetConnectionStatus();

			if (internetStatus != NetworkStatus.NotReachable) {
				
				this.rootObj = await AppDelegate.Current.pqrsMgr.GetPatientProcedureInfo ();

				this.BeginInvokeOnMainThread(
					() => {
						//actView.StopAnimating();

							
						AppDelegate.pb.Stop();
						PopulateTableItems();
						this.mTableView.Source = tableSource;
						this.mTableView.ReloadData();
					});
			}else{
				//actView.StopAnimating();
				AppDelegate.pb.Stop();
			}

			return this.rootObj;
		}

		public void dismissDatePicker(string cancelDone)
		{
			if (cancelDone == "done") {
				DownloadData ();
			}
			datePicker.DismissModalViewController (false);
			SelectedDateValue = datePicker.SelectedDateValue;
		}

		protected void PopulateTableItems()
		{
			procTableItems = new List<ProcedureItemGroup> ();
		

			ProcedureItemGroup tGroup = new ProcedureItemGroup ();
			tGroup.StatusName = "Incomplete Cases";
			if (iProPQRSPortableLib.Consts.PatientInfo.result != null) {

				List<PatientProcedureDetails> inCompleteCases = iProPQRSPortableLib.Consts.PatientInfo.result.FindAll ((PatientProcedureDetails obj) => obj.Status == "Incomplete");
				foreach (PatientProcedureDetails patDet in inCompleteCases) {
					patDet.PatientName = patDet.LastName + ", " + patDet.FirstName;
					tGroup.PatientProcedureListItems.Add (patDet);
				}
				procTableItems.Add (tGroup);

				tGroup = new ProcedureItemGroup ();
				tGroup.StatusName = "Scheduled Cases";

				List<PatientProcedureDetails> scheduledCases = iProPQRSPortableLib.Consts.PatientInfo.result.FindAll ((PatientProcedureDetails obj) => obj.Status == "Scheduled");
				foreach (PatientProcedureDetails patDet in scheduledCases) {
					patDet.PatientName = patDet.LastName + ", " + patDet.FirstName;
					tGroup.PatientProcedureListItems.Add (patDet);
				}
				procTableItems.Add (tGroup);

				tGroup = new ProcedureItemGroup ();
				tGroup.StatusName = "In Process Cases";

				List<PatientProcedureDetails> inProcessCases = iProPQRSPortableLib.Consts.PatientInfo.result.FindAll ((PatientProcedureDetails obj) => obj.Status == "In Process");
				foreach (PatientProcedureDetails patDet in inProcessCases) {
					patDet.PatientName = patDet.LastName + ", " + patDet.FirstName;
					tGroup.PatientProcedureListItems.Add (patDet);
				}
			}
			procTableItems.Add (tGroup);


			tGroup = new ProcedureItemGroup ();
			tGroup.StatusName = "Completed Cases";

			List<PatientProcedureDetails> completedCases = iProPQRSPortableLib.Consts.PatientInfo.result.FindAll ((PatientProcedureDetails obj) => obj.Status == "Completed");
			foreach (PatientProcedureDetails patDet in completedCases) {
				patDet.PatientName = patDet.LastName + ", " + patDet.FirstName;
				tGroup.PatientProcedureListItems.Add (patDet);
			}
			procTableItems.Add (tGroup);

			tGroup = new ProcedureItemGroup ();
			tGroup.StatusName = "Cancelled Cases";

			List<PatientProcedureDetails> cancelledCases = iProPQRSPortableLib.Consts.PatientInfo.result.FindAll ((PatientProcedureDetails obj) => obj.Status == "Cancelled");
			foreach (PatientProcedureDetails patDet in cancelledCases) {
				patDet.PatientName = patDet.LastName + ", " + patDet.FirstName;
				tGroup.PatientProcedureListItems.Add (patDet);
			}
			procTableItems.Add (tGroup);
			tableSource = new PatientTableSource (procTableItems,this);

		}

		private void searchTable()
		{			
			this.mTableView.Source  = this.tableSource.FilterTable(this.searchBar,procTableItems,this);
			this.mTableView.ReloadData();
		}

		public void ShowPatientEditor(PatientProcedureDetails patientProcedureDetails)
		{
			AppDelegate.pb.Start(this.View,"");
			isViewPushed = false;
			InvokeOnMainThread (async () => {				
				ReceiveContext context = new ReceiveContext ();

				PatientProcedureFullDetails  procedureDetails = null;
				ReceiveContext procdetailsontext = await AppDelegate.Current.pqrsMgr.GetPatientProcFullDetails(patientProcedureDetails.ProcID);
				if(procdetailsontext != null && procdetailsontext.result != null && procdetailsontext.status !=null && procdetailsontext.status.ToUpper() =="OK" ) 
				{
					procedureDetails= (PatientProcedureFullDetails)JsonConvert.DeserializeObject (procdetailsontext.result.ToString (), typeof(PatientProcedureFullDetails));
					

					context = await serv.GetPatientDetails (patientProcedureDetails.PatientID);
					Patient selectedPatientDetails=null;
					if(context != null && context.result != null && context.status !=null && context.status.ToUpper() =="OK" ) 
						 selectedPatientDetails = (Patient)JsonConvert.DeserializeObject (context.result.ToString (), typeof(Patient));
					else
						NavigationController.PopToRootViewController(true);
					

					var rootobject = await AppDelegate.Current.pqrsMgr.GetProcedureDiagnosticMaster(patientProcedureDetails.ProcID);
					List<DataResults> DiagnosticList = rootobject.result.FindAll(u=>u.ProcCodeTypeID == 2);
					selectedPatientDetails.DiagnosticList = DiagnosticList;
					List<DataResults> ProcedureList = rootobject.result.FindAll(u=> u.ProcCodeTypeID == 1);
					selectedPatientDetails.ProcedureList = ProcedureList;
					selectedPatientDetails.MACCodesList = rootobject.result.FindAll(u=> u.ProcCodeTypeID == 418);

					iProPQRSPortableLib.Consts.SelectedProcAttribtslist  = await AppDelegate.Current.pqrsMgr.GetAllAttribTypesOfAProcedure(patientProcedureDetails.ProcID);		


					AddPatientViewController addpatient = new AddPatientViewController (selectedPatientDetails,procedureDetails);
					//ReceiveContext users = await AppDelegate.Current.pqrsMgr.GetUsers();
				//	List<UserDetails> lstOfUsers = new List<UserDetails>();
					//if(users != null && users.result != null) {
						//lstOfUsers = (List<UserDetails>)JsonConvert.DeserializeObject (users.result.ToString (), typeof(List<UserDetails>));

				//	}
					if(iProPQRSPortableLib.Consts.lstOfUsers != null)
					{
					addpatient.listOfAnestheologists = iProPQRSPortableLib.Consts.lstOfUsers .FindAll(x => x.Role == "Anesthesiologist");
					addpatient.listOfCRNAs = iProPQRSPortableLib.Consts.lstOfUsers .FindAll(x => x.Role == "CRNA");
					}
					this.NavigationController.PushViewController(addpatient,true);
			   }
				else
					NavigationController.PopToRootViewController(true);

			});		
		}

		private async void RefreshData()
		{
			UIActivityIndicatorView actView = new UIActivityIndicatorView();
			actView.Frame = this.View.Bounds;
			actView.BackgroundColor = UIColor.LightTextColor;
			actView.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.WhiteLarge;
			actView.Color = UIColor.Black;
			actView.HidesWhenStopped = true;
			this.View.Add(actView);
			//actView.StartAnimating();
			AppDelegate.pb.Start (this.View, "Loading");
			NetworkStatus internetStatus = Reachability.InternetConnectionStatus();

			if (internetStatus != NetworkStatus.NotReachable) {
				this.rootObj = await AppDelegate.Current.pqrsMgr.GetPatientProcedureInfo ();
				this.BeginInvokeOnMainThread(
					() => {
						if(rootObj.result!=null && rootObj.status !=null && rootObj.status.ToUpper() =="OK")
						{
						AppDelegate.pb.Stop();
						PopulateTableItems();
						searchTable();
					}
					else
					NavigationController.PopToRootViewController(true);
						//this.mTableView.Source = tableSource;
						//this.mTableView.ReloadData();
					});
			}else{
				AppDelegate.pb.Stop();
			}
		}
	}
}


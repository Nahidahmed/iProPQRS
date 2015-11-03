//Added to GitHub
using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Net;
using iProPQRSPortableLib;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Drawing;
using CoreGraphics;
using System.Threading.Tasks;
using System.Text;

namespace iProPQRS
{
	
	public partial class AddPatientViewController : UIViewController
	{
		ImagesGalleryView viewimg=null;
		PQRSServices serv=new PQRSServices();
		Patient patientProfile;
		int physicalStatus;
		int encounterTypeid;
		int patientGenderID;
		int SurgeonID;
		ProcedureDiagnosticMaster lastSelectedDiagnosisObj;
		ProcedureDiagnosticMaster lastSelectedProceduresObj;
		PatientProcedureDetails patientProcedureDetails;
		PatientProcedureFullDetails procedureDetails;
		ProcedureDiagnosticMaster procedureItems;
		List<LastUsedProcedureDiagnosisDetails> lastUsedProcedureItemsList;
		CodePicker cp;
		mCodePicker mcp;
		mlsCodePicker mlcp;
		UIScrollView usuvPatientInfo=new UIScrollView();
		List<CodePickerModel> ATlist = new List<CodePickerModel>();
		QMValidation asa7ItemRemovedPrev = new QMValidation();
		QMValidation asa8ItemRemovedPrev = new QMValidation();

		string[] anesTechItems;

		#region MasterList
		List<iProPQRSPortableLib.Type> requiredLineTypeItems = new List<iProPQRSPortableLib.Type>();
		List<iProPQRSPortableLib.Type> requiredAnesTechTypes = new List<iProPQRSPortableLib.Type>();
		List<iProPQRSPortableLib.Type> requiredNerveBlockTypes = new List<iProPQRSPortableLib.Type>();
		List<iProPQRSPortableLib.Type> requiredSpecialTechTypes = new List<iProPQRSPortableLib.Type>();
		iProPQRSPortableLib.Type requiredUltraSoundGuidance = new iProPQRSPortableLib.Type();
		iProPQRSPortableLib.Type requiredContCatheter = new iProPQRSPortableLib.Type();
		iProPQRSPortableLib.Type requiredPostAnalgesia = new iProPQRSPortableLib.Type();
		iProPQRSPortableLib.Type requiredOpAnesthesia = new iProPQRSPortableLib.Type();

		List<iProPQRSPortableLib.AttribType> allCVCSterileTechsOptions = new List<iProPQRSPortableLib.AttribType>();
		List<iProPQRSPortableLib.Option> requiredCVCSterileTechsTypes = new List<iProPQRSPortableLib.Option>();

		List<SurgeonDetails> lstOfSurgeons = new List<SurgeonDetails>();
		List<ProcedureSurgeonDetails> selectedSurgeon = new List<ProcedureSurgeonDetails>();
		#endregion

		#region SelectedList
		List<int>  selectedAnesTechTypesIds=new List<int>();
		List<string>  selectedsubAnesTechTypesIds=new List<string>();
		List<int>  selectedLineTypeItemsIds=new List<int>();
		List<string>  selectedLineCVCSterileTechIds=new List<string>();
		List<int>  selectedNerveBlockIds=new List<int>();
		List<int>  selectedSpecialTechIds=new List<int>();
		#endregion

		int baseYDiagnosisDescriptionValue = 8;
		int baseYDiagnosisCodeValue = 8;
		int baseYProcDescriptionValue = 8;
		int baseYProcCodeValue = 8;

		//Participants
		int baseYAnesLbl = 109;
		int baseYAnesNameBtn = 89;
		int baseYAnesStartTimeLbl = 131;
		int baseYAnesStartTimeBtn = 127;
		int baseYAnesEndTimeLbl = 131;
		int baseYAnesEndTimeBtn = 127;

		int baseYCRNALbl = 109;
		int baseYCRNANameBtn = 89;
		int baseYCRNAStartTimeLbl = 131;
		int baseYCRNAStartTimeBtn = 127;
		int baseYCRNAEndTimeLbl = 131;
		int baseYCRNAEndTimeBtn = 127;

		bool showKeyBoard = false;

		private UIView activeView;
		private nfloat scroll_amount = 0.0f;   //amount to scroll
		private nfloat bottom = 0.0f;          //bottom point
		private nfloat offset = 100.0f;         //extra offset
		private bool moveViewUp = false;      //direction up or down

		List<DropDownModel> ddlPatientGenderList;
		List<DropDownModel> dEncounterTypelist;
		List<DropDownModel> dlPhyStatusList;
		List<int> selectedDiagnosisCodeid=new List<int>();
		List<int> selectedprocedureCodeid=new List<int>();

		List<ProcedureParticipantDetails> listOfProcParticiPants = new List<ProcedureParticipantDetails>();
		public List<UserDetails> listOfAnestheologists = new List<UserDetails>();
		List<ProcedureParticipantDetails> selectedAnestheologistList = new List<ProcedureParticipantDetails>();

		public List<UserDetails> listOfCRNAs = new List<UserDetails>();
		List<ProcedureParticipantDetails> selectedCRNAList = new List<ProcedureParticipantDetails>();

		nint diagnosiswithTag = 200;
		nint diagnosiscodewithTag = 100;
		nint procedurewithTag = 200;
		nint procedurecodewithTag = 100;

		bool previousALineTypeValue;
		bool pervPACatheterType;
		bool prevCVPType;
		bool prevUltraSoundGuidance;
		bool prevContCatheterType;
		bool prevOpAnesthesiaType;
		bool prevPostOpAnalgesiaType;

		#region QUALITY METRICS
		List<iProPQRSPortableLib.Type> pqrsTypes = new List<iProPQRSPortableLib.Type>();
		List<iProPQRSPortableLib.Option> pqrsTypeOptions = new List<iProPQRSPortableLib.Option>();
		List<iProPQRSPortableLib.Type> nonPqrsTypes = new List<iProPQRSPortableLib.Type>();
		List<iProPQRSPortableLib.Option> nonPqrsTypeOptions = new List<iProPQRSPortableLib.Option>();
		List<iProPQRSPortableLib.Type> nonPqrsTypeAS10Options = new List<iProPQRSPortableLib.Type>();
		List<int>  selectednonPqrsTypeAS10OptionsIds=new List<int>();
		List<iProPQRSPortableLib.Option> ASA8MasterList = new List<iProPQRSPortableLib.Option>();
		List<int>  selectedMasterASA8OptionsIds = new List<int>();
		List<iProPQRSPortableLib.Option> ASA8SubMasterList = new List<iProPQRSPortableLib.Option>();
		List<int>  selectedSubMasterASA8OptionsIds = new List<int>();

		List<iProPQRSPortableLib.Option> ASA9MasterList = new List<iProPQRSPortableLib.Option>();
		List<int>  selectedMasterASA9OptionsIds = new List<int>();
		List<iProPQRSPortableLib.Option> ASA9SubMasterList = new List<iProPQRSPortableLib.Option>();
		List<int>  selectedSubMasterASA9OptionsIds = new List<int>();
		UIScrollView svuvBillingInfo=new UIScrollView();
		float hUV = 0;
		float yUV = 0;
		#endregion
		nint aneswithTag = 200;

		public AddPatientViewController () : base ("AddPatientViewController", null)
		{
		}

		public string Facility {
			get;
			set;
		}
		public AddPatientViewController (PatientProcedureDetails patientProcedureDetails) : base ("AddPatientViewController", null)
		{
			this.patientProcedureDetails = patientProcedureDetails;
		}
		public AddPatientViewController (Patient patientProfile,PatientProcedureDetails patientProcedureDetails) : base ("AddPatientViewController", null)
		{
			this.patientProcedureDetails = patientProcedureDetails;
			this.patientProfile = patientProfile;
		}

		public AddPatientViewController (Patient patientProfile,PatientProcedureFullDetails procedureDetails) : base ("AddPatientViewController", null)
		{
			this.procedureDetails = procedureDetails;
			this.patientProfile=patientProfile;
		}

		string[] ET="InPatient , OutPatient , ASC Encounter , Office Based".Split(',');
		string[] PS="ASA 1,ASA 2,ASA 3,ASA 4,ASA 5,ASA 6".Split(','); 
		string[] PatientGenderList ="Male,Female".Split(','); 

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		public void SetBorderColor(UIView uv)
		{			
			if (uv != null) {
				uv.Layer.BorderColor = UIColor.Gray.CGColor;
				uv.Layer.BorderWidth = 1f;
			}
		}

		void SetViewBorderColor ()
		{
			SetBorderColor (vuPI);
			SetBorderColor (uvDiagnosis);
			SetBorderColor (uvAnesthesia);
			SetBorderColor (uvProcedure);
			SetBorderColor (uvLocationInformation);
			SetBorderColor (uvAnesthesiologist);
			SetBorderColor (uvCRNA);
			SetBorderColor (uvSurgeons);
			//Quality metrics
			SetBorderColor (uvCaseCancellation);
			SetBorderColor (uvPreIntraOp);
			SetBorderColor (uvSCIPPQRS);
			SetBorderColor (uvPACUPostOp);
			SetBorderColor (uvPACUdischarge);
			//uvPACUdischarge
			SetBorderColor (uv17daysoutcomes);
			SetBorderColor (uvNurseSignature);
			SetBorderColor (uvPatientSignature);

			SetBorderColor (svBillingInfo);
			SetBorderColor (uvAnesthTech);
			SetBorderColor (uvLines);
			SetBorderColor (uvRegional);
			SetBorderColor (uvSpecialTech);
			SetBorderColor (uvAnesthImageCap);
//			SetBorderColor (uvBillingProcedureInfo);
		}

		void BindDropDown ()
		{
			DropDownModel item;
			int cnt = 1;
			dEncounterTypelist = new List<DropDownModel> ();

			foreach (string str in ET) {
				item = new DropDownModel ();
				item.DropDownID = cnt;
				item.DropDownText = str;
				dEncounterTypelist.Add (item);
				item = null;
				cnt++;
			}

			uvMain.AddSubview (uvMainPatientinfo);
			dlPhyStatusList = new List<DropDownModel> ();
			cnt = 1;
			foreach (string str in PS) {
				item = new DropDownModel ();
				item.DropDownID = cnt;
				item.DropDownText = str;
				dlPhyStatusList.Add (item);
				item = null;
				cnt++;
			}

			ddlPatientGenderList = new  List<DropDownModel> ();
			cnt = 1;
			foreach (string str in PatientGenderList) {
				item = new DropDownModel ();
				item.DropDownID = cnt;
				item.DropDownText = str;
				ddlPatientGenderList.Add (item);
				item = null;
				cnt++;
			}

			DropDownViewController ddlbtn;// = new DropDownViewController (this);
			EncounterBtn.TouchUpInside += (object sender, EventArgs e) =>  {
				ddlbtn = new DropDownViewController (this);
				ddlbtn.SelectedValue=encounterTypeid;
				ddlbtn.DataSource = dEncounterTypelist;
				ddlbtn.PresentFromPopover (EncounterBtn,320,250);
				ddlbtn._Change += delegate {
					EncounterBtn.SetTitle (ddlbtn.SelectedText, UIControlState.Normal);
					if(ddlbtn.SelectedText != string.Empty){
						encounterTypeid = ddlbtn.SelectedValue;
					}else{
						EncounterBtn.SetTitle ("Select", UIControlState.Normal);
						encounterTypeid = 0;
						cnt = 1;
						dEncounterTypelist.Clear();
						foreach (string str in ET) {
							item = new DropDownModel ();
							item.DropDownID = cnt;
							item.DropDownText = str;
							dEncounterTypelist.Add (item);
							item = null;
							cnt++;
						}
					}
					if(ddlbtn.SelectedText != string.Empty){
						ValidateAndUpdateProcedure("EncounterType",ddlbtn.SelectedText);
					}else{
						ValidateAndUpdateProcedure("EncounterType",string.Empty);
					}
				};
			};

			PhysicalStatusBtn.TouchUpInside += (object sender, EventArgs e) => {
				ddlbtn = new DropDownViewController (this);
				ddlbtn.SelectedValue = physicalStatus;
				ddlbtn.DataSource = dlPhyStatusList;
				ddlbtn.PresentFromPopover (PhysicalStatusBtn,320,300);
				ddlbtn._Change += delegate {
					PhysicalStatusBtn.SetTitle (ddlbtn.SelectedText, UIControlState.Normal);
					if(ddlbtn.SelectedText != string.Empty){
						physicalStatus = ddlbtn.SelectedValue;
					}else{
						PhysicalStatusBtn.SetTitle ("Select", UIControlState.Normal);
						physicalStatus = 0;
						dlPhyStatusList.Clear();
						cnt = 1;
						foreach (string str in PS) {
							item = new DropDownModel ();
							item.DropDownID = cnt;
							item.DropDownText = str;
							dlPhyStatusList.Add (item);
							item = null;
							cnt++;
						}
					}
					if(ddlbtn.SelectedText != string.Empty){
						ValidateAndUpdateProcedure("PhysicalStatus",ddlbtn.SelectedValue.ToString());
					}else{
						ValidateAndUpdateProcedure("PhysicalStatus",string.Empty);
					}
				};
			};

			uSEmergency.ValueChanged += async (object sender, EventArgs e) => {
				string SelectedValue = string.Empty;
				if (uSEmergency.On)
					SelectedValue = "1";
				else
					SelectedValue = "0";
				ValidateAndUpdateProcedure ("Emergency", SelectedValue);
			};

			btnPatientGender.TouchUpInside += (object sender, EventArgs e) => {
				
				txtpmrn.ResignFirstResponder();
				txtpFirstName.ResignFirstResponder();
				txtpLastName.ResignFirstResponder();

				ddlbtn = new DropDownViewController (this);
				ddlbtn.SelectedValue = patientGenderID;
				ddlbtn.DataSource = ddlPatientGenderList;
				ddlbtn.PresentFromPopover (btnPatientGender,300,130);
				ddlbtn._Change += delegate {
					btnPatientGender.SetTitle (ddlbtn.SelectedText, UIControlState.Normal);
					if(ddlbtn.SelectedText != string.Empty){
						patientGenderID = ddlbtn.SelectedValue;
					}else{
						btnPatientGender.SetTitle ("Select", UIControlState.Normal);
						patientGenderID = 0;
						ddlPatientGenderList.Clear();
						cnt = 1;
						foreach (string str in PatientGenderList) {
							item = new DropDownModel ();
							item.DropDownID = cnt;
							item.DropDownText = str;
							ddlPatientGenderList.Add (item);
							item = null;
							cnt++;
						}
					}
					SavePatientInfo();
				};
			};
		}

		public bool validatedata()
		{
			bool Isvalidate=true;
			string emsg = string.Empty;
			if (string.IsNullOrEmpty (txtpmrn.Text)) {
				Isvalidate = false;
				emsg = "Please Enter MRN Number.";
			} else if (string.IsNullOrEmpty (txtpFirstName.Text)) {
				Isvalidate = false;
				emsg = "Please Enter First Name.";
			} else if (string.IsNullOrEmpty (txtpLastName.Text)) {
				Isvalidate = false;
				emsg = "Please Enter Last Name.";
			}
			
			if (!Isvalidate) {
//				new UIAlertView ("Validation Error", emsg, null, "ok", null).Show ();
				Console.WriteLine ("Validation Error:: "+ emsg);
			}
			
			return Isvalidate;
		}
		#region Billing Info
		 UIView uvBillingProcedureInfo;
		int Anesheight=40; 
		int AnesCount=0;
		UIView uvProviderView=new UIView(new CGRect(0,100,1008,200));
		int crnahheight=40;
		int crnacount=0;
		UIView uvDynamicSurgeonview=new UIView(new CGRect(0,0,1008,200));
		#endregion
		public void AddBasicProviderView()
		{
			uvProviderView.BackgroundColor=UIColor.White;
			UIImageView titleimg = new UIImageView (new CoreGraphics.CGRect (0, 0, 1008, 40));
			titleimg.Image=UIImage.FromFile(@"headerBarTall.png");
			UILabel lbltitle = new UILabel (new CoreGraphics.CGRect (8, 8, 140, 21));
			lbltitle.Text = "Provider";
			UIButton btaddnanes = new UIButton (new CGRect (407, 5, 30, 30));
			btaddnanes.SetBackgroundImage (UIImage.FromFile (@"addDiagProc.png"), UIControlState.Normal);
			btaddnanes.TouchUpInside += async (object sender, EventArgs e) => {
				AddAnesView (null);
			};
			//AddProviderView ();
			UIButton btnDelanes = new UIButton (new CGRect (452, 5, 30, 30));
			btnDelanes.SetBackgroundImage (UIImage.FromFile (@"removeDiagProc.png"), UIControlState.Normal);
			btnDelanes.TouchUpInside += async (object sender, EventArgs e) => {
				RemoveAnes();
			};
			UIButton btaddcrna = new UIButton (new CGRect (906, 5, 30, 30));
			btaddcrna.SetBackgroundImage (UIImage.FromFile (@"addDiagProc.png"), UIControlState.Normal);
			btaddcrna.TouchUpInside += async (object sender, EventArgs e) => {
				AddCRNAView(null);
			};
			UIButton btnDelcrna = new UIButton (new CGRect (946, 5, 30, 30));
			btnDelcrna.SetBackgroundImage (UIImage.FromFile (@"removeDiagProc.png"), UIControlState.Normal);
			//RemoveCRNA
			btnDelcrna.TouchUpInside += async (object sender, EventArgs e) => {
				RemoveCRNA();
			};
			uvProviderView.Add (titleimg);
			uvProviderView.Add (lbltitle);
			uvProviderView.Add (btaddnanes);
			uvProviderView.Add (btnDelanes);
			uvProviderView.Add (btaddcrna);
			uvProviderView.Add (btnDelcrna);


		}
		UIButton crnaStarttime=null;
		UIButton crnaendtime=null;
		int PrevcrnaHourTime;
		int PrevcrnaminTime;
		bool crnaadded = false;
		public void AddCRNAView(ProcedureParticipantDetails participantDetails)
		{
			crnaadded = true;
			UIView view = uvProviderView.ViewWithTag(crnacount+200);
			if (view != null) {
				UIButton checkbtn = (UIButton)view.ViewWithTag(2);
				if (checkbtn != null && string.IsNullOrEmpty (checkbtn.TitleLabel.Text)) {
					new UIAlertView("CRNA Info", "Please Update CRNA"+crnacount+" Record."
						, null, "ok", null).Show();
					return; 
				}
			}


			crnacount = crnacount + 1;
			UIView  uvblock= new UIView();
			uvblock.Tag = crnacount+200;
			uvblock.Frame = new CoreGraphics.CGRect (498, crnahheight, 530, 100);
			crnahheight=crnahheight+100;
			UITextView hideid=new UITextView();
			hideid.Hidden=true;
			UILabel lbltitle= new UILabel(new CoreGraphics.CGRect(5, 10,100,40));
			lbltitle.Text="CRNA ("+crnacount+"):";			    		    
			UIButton crnaNameBtn=new UIButton(new CoreGraphics.CGRect(145, 5,350,30));
			crnaNameBtn.Tag =  2;
			crnaNameBtn.Layer.BorderWidth = 1;
			crnaNameBtn.Layer.BorderColor = UIColor.DarkGray.CGColor;
			crnaNameBtn.SetTitleColor (UIColor.Black, UIControlState.Normal);
			if (participantDetails != null && !string.IsNullOrEmpty (participantDetails.Name)) {
				crnaNameBtn.SetTitle (participantDetails.Name, UIControlState.Normal);
				crnaNameBtn.Tag = participantDetails.ProcParticipantID;
			} else {
//				if (crnacount ==1 && crnaNameBtn.Tag == 2 && !string.IsNullOrEmpty(crnaNameBtn.CurrentTitle)) {
				if (crnacount ==1 && crnaNameBtn.Tag == 2) {
					if(iProPQRSPortableLib.Consts.Role.Trim().ToLower() == "crna")
						crnaNameBtn.SetTitle (iProPQRSPortableLib.Consts.LoginUserFullName, UIControlState.Normal);
					
					if(participantDetails == null)
						participantDetails = new ProcedureParticipantDetails();

					UserDetails userDetails = null;
					try{
						userDetails = listOfCRNAs.Find (x => x.LastName == iProPQRSPortableLib.Consts.UserLastName && x.FirstName == iProPQRSPortableLib.Consts.UserFirstName);
//						 userDetails = listOfCRNAs.Find (x => x.LastName.Trim().ToLower() == iProPQRSPortableLib.Consts.UserLastName.ToLower() && x.FirstName.Trim().ToLower() == iProPQRSPortableLib.Consts.UserFirstName.ToLower());
					}catch(Exception ex){
						Console.WriteLine ("AddCRNAView Ex: "+ex.Message);
					}

					if (userDetails != null) {
						participantDetails.UserID = userDetails.ID;
						participantDetails.ProcParticipantID = 0;
						participantDetails.RoleID = 6;
						participantDetails.Name = crnaNameBtn.CurrentTitle;//crnaNameBtn.CurrentTitle.Trim();
						if (crnaNameBtn != null && crnaNameBtn.CurrentTitle.Trim () != string.Empty) {
							AddUpdateProcedureParticipants (participantDetails, crnaNameBtn);
						}
					}
				}
			}


			crnaNameBtn.Layer.CornerRadius = 5; 
			crnaNameBtn.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			crnaNameBtn.TouchUpInside += async delegate {
				if(listOfCRNAs.Count > 0){
					
					if(participantDetails == null)
						participantDetails = new ProcedureParticipantDetails();

					participantDetails.RoleID = 6;
					BindParticipantsPopupover(listOfCRNAs,crnaNameBtn,(int)crnaNameBtn.Frame.Y,participantDetails);
				}
			};
			UILabel slblstar = new UILabel (new CoreGraphics.CGRect (214, 47, 10, 41));
			slblstar.Text = "*";
			slblstar.TextColor = UIColor.Red;

			UILabel lblstart = new UILabel(new CoreGraphics.CGRect(135, 50,80,30));
			lblstart.Text = "Start Time";
			UIButton endTimeBtn=new UIButton(new CoreGraphics.CGRect(415, 50,80,30));
			UIButton startTimeBtn= new UIButton(new CoreGraphics.CGRect(230, 50,80,30));
			startTimeBtn.Tag = 11;
			startTimeBtn.Layer.BorderWidth = 1;
			startTimeBtn.Layer.BorderColor = UIColor.DarkGray.CGColor;
			startTimeBtn.Layer.CornerRadius = 5; 
			startTimeBtn.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			startTimeBtn.SetTitleColor (UIColor.Black, UIControlState.Normal);
			if (participantDetails != null && !string.IsNullOrEmpty (participantDetails.StartTime)) {
				DateTime dt = DateTime.Parse(participantDetails.StartTime);
				//dt.ToString("HH:mm");
				if(dt.ToString("HH:mm") == "00:00")
					startTimeBtn.SetTitle (string.Empty, UIControlState.Normal);
				else
					startTimeBtn.SetTitle (dt.ToString("HH:mm"), UIControlState.Normal);
			}

			if(participantDetails != null)
				startTimeBtn.Tag = participantDetails.ProcParticipantID;

			if(participantDetails == null && PrevcrnaHourTime !=0)
			{
				if(PrevcrnaminTime != 60)
				{						
					//PrevHourTime=PrevHourTime+1;
					PrevcrnaminTime=+PrevcrnaminTime+1;
				}
				DateTime dt=new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,PrevcrnaHourTime,PrevcrnaminTime,0);


				string strtime=PrevHourTime.ToString();
				strtime=dt.ToShortTimeString();
				startTimeBtn.SetTitle(strtime,UIControlState.Normal);
			}

			startTimeBtn.TouchUpInside += (object sender, EventArgs e) => {
				if(crnaNameBtn.Tag==2)
				{
					new UIAlertView("Anes Info", "Please Select CRNA Name"
						, null, "ok", null).Show();
					return;							
				}
				TimePicker  tp= new TimePicker();
				tp.PresentFromPopover(startTimeBtn,40,35);
				tp._ValueChanged += delegate {
					try
					{
					if (participantDetails == null)
						participantDetails = new ProcedureParticipantDetails ();

					participantDetails.StartTime = tp.SelectedTime;
					if(tp.SelectedTime == "00:00")
						tp.SelectedTime = string.Empty;

					startTimeBtn.SetTitle(tp.SelectedTime,UIControlState.Normal);
					
					participantDetails.EndTime = endTimeBtn.CurrentTitle;
					participantDetails.ProcParticipantID= Convert.ToInt16(crnaNameBtn.Tag);

					if(!string.IsNullOrEmpty(crnaNameBtn.CurrentTitle)){
						if(selectedCRNAList.Count > 0){
//							participantDetails.UserID = selectedCRNAList[(int)(startTimeBtn.Tag - 1)].UserID;
//							participantDetails.ProcParticipantID = selectedCRNAList[(int)(startTimeBtn.Tag - 1)].ProcParticipantID;
//
//							if(startTimeBtn.Tag != 0){
//								participantDetails.UserID = selectedCRNAList.Find(x => x.ProcParticipantID == startTimeBtn.Tag).UserID;
//								participantDetails.ProcParticipantID = (int)startTimeBtn.Tag;
//							}else{
//									participantDetails.UserID = selectedCRNAList.Find(x => x.Name!= null && x.Name.Trim().ToLower() == crnaNameBtn.CurrentTitle.Trim().ToLower()).UserID;
//									participantDetails.ProcParticipantID = selectedCRNAList.Find(x => x.Name!= null && x.Name.Trim().ToLower() == crnaNameBtn.CurrentTitle.Trim().ToLower()).ProcParticipantID;
//							}

							participantDetails.RoleID = 6;
							UpdateProcedureParticipants(participantDetails,crnaNameBtn);
						}
					}
					}
					catch
					{
						
					}
				};
			};

			UILabel elblstar = new UILabel (new CoreGraphics.CGRect (403, 47, 10, 41));
			elblstar.Text = "*";
			elblstar.TextColor = UIColor.Red;

			UILabel lblendtime= new UILabel(new CoreGraphics.CGRect(330, 50,80,30));
			lblendtime.Text = "End Time";
			endTimeBtn.Tag = 22;
			endTimeBtn.Layer.BorderWidth = 1;
			endTimeBtn.Layer.BorderColor = UIColor.DarkGray.CGColor;
			endTimeBtn.Layer.CornerRadius = 5; 
			endTimeBtn.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			endTimeBtn.SetTitleColor (UIColor.Black, UIControlState.Normal);
			if (participantDetails != null && !string.IsNullOrEmpty (participantDetails.EndTime)) {
				DateTime dt = DateTime.Parse(participantDetails.EndTime);
				//dt.ToString("HH:mm");
				if(dt.ToString("HH:mm") == "00:00")
					endTimeBtn.SetTitle (string.Empty, UIControlState.Normal);
				else
					endTimeBtn.SetTitle (dt.ToString("HH:mm"), UIControlState.Normal);

				if(dt.ToString("HH:mm") != "00:00"){
					PrevcrnaHourTime=dt.Hour;
					PrevcrnaminTime=dt.Minute;
				}
			}	

			if(participantDetails != null)
				endTimeBtn.Tag = participantDetails.ProcParticipantID;
			
			endTimeBtn.TouchUpInside += async delegate {
				if(crnaNameBtn.Tag==2)
				{
					new UIAlertView("Anes Info", "Please Select CRNA Name"
						, null, "ok", null).Show();
					return;							
				}


				TimePicker  tp=new TimePicker();
				tp.PresentFromPopover(endTimeBtn,50,35);
				tp._ValueChanged += delegate {		
					try
					{
					if (participantDetails == null)
						participantDetails = new ProcedureParticipantDetails ();

					participantDetails.EndTime = tp.SelectedTime;
					if(tp.SelectedTime == "00:00")
						tp.SelectedTime = string.Empty;

					endTimeBtn.SetTitle(tp.SelectedTime,UIControlState.Normal);
					

					participantDetails.StartTime = startTimeBtn.CurrentTitle;
					participantDetails.ProcParticipantID= Convert.ToInt16(crnaNameBtn.Tag);

					if(tp.SelectedTime != string.Empty){
						DateTime dt=Convert.ToDateTime(tp.SelectedTime);
						PrevcrnaHourTime=dt.Hour;
						PrevcrnaminTime=dt.Minute;
					}

					if(!string.IsNullOrEmpty(crnaNameBtn.CurrentTitle)){
						if(selectedCRNAList.Count > 0){

//							if(endTimeBtn.Tag != 0){
//								participantDetails.UserID = selectedCRNAList.Find(x => x.ProcParticipantID == endTimeBtn.Tag).UserID;
//								participantDetails.ProcParticipantID = (int)startTimeBtn.Tag;
//							}else{
//									participantDetails.UserID = selectedCRNAList.Find(x => x.Name!= null &&  x.Name.Trim().ToLower() == crnaNameBtn.CurrentTitle.Trim().ToLower()).UserID;
//									participantDetails.ProcParticipantID = selectedCRNAList.Find(x => x.Name!= null &&  x.Name.Trim().ToLower() == crnaNameBtn.CurrentTitle.Trim().ToLower()).ProcParticipantID;
//							}

							participantDetails.RoleID = 6;
							UpdateProcedureParticipants(participantDetails,crnaNameBtn);
						}
					}
					}
					catch
					{
						
					}

				};
			};
			hideid.Tag=4;
			hideid.Text=crnaNameBtn.Tag.ToString();
			uvblock.Add(lblstart);
			uvblock.Add (slblstar);
			uvblock.Add(crnaNameBtn);
			uvblock.Add(lbltitle);
			uvblock.Add(startTimeBtn);
			uvblock.Add(lblendtime);
			uvblock.Add(endTimeBtn);
			uvblock.Add (elblstar);
			uvblock.Add(hideid);
			crnaStarttime = startTimeBtn;
			crnaendtime = endTimeBtn;
			UIView uvsp = new UIView(new CGRect(0,96,520,1));
			uvsp.BackgroundColor = UIColor.Black;
			UIView uvleftline = new UIView(new CGRect(0,0,1,100));
			uvleftline.BackgroundColor = UIColor.LightGray;
			uvblock.Add(uvleftline);
			uvblock.Add(uvsp);
			//uvProviderView
			uvProviderView.Add(uvblock);
			//svBillingInfo.Add(uvblock);
			//Anesheight - crnahheight
			if (( crnahheight-Anesheight) >= 100) {
				uvProviderView.Frame = new CGRect (0,100, 1008, float.Parse (crnahheight.ToString ()) );

				uvBillingProcedureInfo.Frame = new CoreGraphics.CGRect (0, float.Parse (crnahheight.ToString ())+100 , 1008, 800);
				svBillingInfo.SizeToFit ();
				svBillingInfo.ContentSize = new SizeF (float.Parse (svBillingInfo.Frame.Width.ToString ()), float.Parse (svBillingInfo.Frame.Height.ToString ()) + crnahheight + 200);
			}


		}
		public async void RemoveCRNA()
		{
			UIView view = uvProviderView.ViewWithTag(crnacount+200);
			if (view != null) {
				crnacount=crnacount-1;
				if ((crnahheight-Anesheight) >= 100) {
					uvProviderView.Frame = new CGRect (0, 100, 1008, float.Parse (crnahheight.ToString ()) );

					uvBillingProcedureInfo.Frame = new CoreGraphics.CGRect (0, float.Parse (crnahheight.ToString ()) , 1008, 800);
					svBillingInfo.SizeToFit ();
					svBillingInfo.ContentSize = new SizeF (float.Parse (svBillingInfo.Frame.Width.ToString ()), float.Parse (svBillingInfo.Frame.Height.ToString ()) + crnahheight + 200);
				}
				//UIButton checkbtn=(UIButton)view.ViewWithTag(2);
				UITextView checkbtn= (UITextView)view.ViewWithTag(4);
				if (checkbtn != null && !string.IsNullOrEmpty (checkbtn.Text) && int.Parse(checkbtn.Text) != 2) {		
					selectedCRNAList = selectedCRNAList.OrderByDescending (x => x.ProcParticipantID).ToList ();
					int procParticipantID = 0;
					string anesName = string.Empty;
					if (selectedCRNAList.Count > 0) {
						//procParticipantID = selectedCRNAList [0].ProcParticipantID;
						procParticipantID = selectedCRNAList.Find (u => u.ProcParticipantID == Convert.ToInt16(checkbtn.Text)).ProcParticipantID;// [0].ProcParticipantID;
					} 

					if (selectedCRNAList.Count > 0) {
						ReceiveContext deletecontext = await AppDelegate.Current.pqrsMgr.DeleteProcedureParticipant (procParticipantID);
						if (deletecontext != null && deletecontext.status != null && deletecontext.status.ToUpper () == "OK") {							
							selectedCRNAList.Remove(selectedCRNAList.Find (u => u.ProcParticipantID == Convert.ToInt16(checkbtn.Text)));
						}
					}
				}
				crnahheight = crnahheight - (int)view.Frame.Height;
				view.RemoveFromSuperview ();
				PrevcrnaminTime = PrevcrnaminTime - 1;
			}

		}
		public async void RemoveAnes()
		{
			UIView view = uvProviderView.ViewWithTag(AnesCount+100);
			if (view != null) {
				AnesCount=AnesCount-1;
				if ((Anesheight - crnahheight) >= 100) {
					uvProviderView.Frame = new CGRect (0, 100, 1008, float.Parse (Anesheight.ToString ()) );

					uvBillingProcedureInfo.Frame = new CoreGraphics.CGRect (0, float.Parse (Anesheight.ToString ()) , 1008, 800);
					svBillingInfo.SizeToFit ();
					svBillingInfo.ContentSize = new SizeF (float.Parse (svBillingInfo.Frame.Width.ToString ()), float.Parse (svBillingInfo.Frame.Height.ToString ()) + Anesheight + 200);
				}
				//UIButton checkbtn= (UIButton)view.ViewWithTag(2);
				UITextView checkbtn= (UITextView)view.ViewWithTag(4);
				if (checkbtn != null && !string.IsNullOrEmpty (checkbtn.Text) && int.Parse(checkbtn.Text) != 2) {				
					selectedAnestheologistList = selectedAnestheologistList.OrderByDescending (x => x.ProcParticipantID).ToList ();
					int procParticipantID = 0;
					string anesName = string.Empty;
					if (selectedAnestheologistList.Count > 0) {
						procParticipantID = int.Parse(checkbtn.Text);//selectedAnestheologistList.Find (u => u.ProcParticipantID == Convert.ToInt16(checkbtn.Text)).ProcParticipantID;// [0].ProcParticipantID;
						anesName = selectedAnestheologistList.Find (u => u.ProcParticipantID == Convert.ToInt16(checkbtn.Text)).Name;
					} 

					if (selectedAnestheologistList.Count > 0) {
						ReceiveContext deletecontext = await AppDelegate.Current.pqrsMgr.DeleteProcedureParticipant (procParticipantID);
						if (deletecontext != null && deletecontext.status != null && deletecontext.status.ToUpper () == "OK") {
							Console.WriteLine ("Participant: " + anesName + " Deleted Successfully");
							selectedAnestheologistList.Remove(selectedAnestheologistList.Find (u => u.ProcParticipantID == Convert.ToInt16(checkbtn.Text)));
						}
					}
				}
				Anesheight = Anesheight - (int)view.Frame.Height;
				view.RemoveFromSuperview ();
				PrevminTime = PrevminTime - 1;
			}

		}
		UIButton Anesstarttimebtn;
		UIButton Anesendtimebtn;
		int PrevHourTime;
		int PrevminTime;
		bool anesadded = false;
		public void AddAnesView(ProcedureParticipantDetails participantDetails)
		{
			try
			{
				anesadded = true;
				UIView view = uvProviderView.ViewWithTag(AnesCount+100);
				if (view != null) {
					UIButton checkbtn=(UIButton)view.ViewWithTag(2);
					if (checkbtn != null && string.IsNullOrEmpty (checkbtn.TitleLabel.Text)) {
						new UIAlertView("Anes Info", "Please Update Anes"+AnesCount+" Record."
												, null, "ok", null).Show();
						return; 
					}
				}

				AnesCount = AnesCount + 1;
				UIView  uvblock=new UIView();
				uvblock.Tag = AnesCount+100;
				uvblock.Frame = new CoreGraphics.CGRect (0, Anesheight, 497, 100);
				Anesheight = Anesheight+100;
				UILabel lbltitle=new UILabel(new CoreGraphics.CGRect(5, 10,100,40));
				lbltitle.Text="Anes ("+AnesCount+"):";	

				UITextView hideid=new UITextView();
				hideid.Hidden=true;
				UIButton anesNameBtn = new UIButton(new CoreGraphics.CGRect(145, 5,350,30));
				anesNameBtn.Tag =  2;
				anesNameBtn.Layer.BorderWidth = 1;
				anesNameBtn.Layer.BorderColor = UIColor.DarkGray.CGColor;
				anesNameBtn.SetTitleColor (UIColor.Black, UIControlState.Normal);
				if (participantDetails != null && !string.IsNullOrEmpty (participantDetails.Name)) {
						anesNameBtn.SetTitle (participantDetails.Name, UIControlState.Normal);
						anesNameBtn.Tag =  participantDetails.ProcParticipantID;
				} else {
					if (AnesCount == 1) {
						if(iProPQRSPortableLib.Consts.Role.Trim().ToLower() == "anesthesiologist")
							anesNameBtn.SetTitle (iProPQRSPortableLib.Consts.LoginUserFullName, UIControlState.Normal);
						
							if(participantDetails == null)
								participantDetails = new ProcedureParticipantDetails();
						
//							UserDetails userDetails = listOfAnestheologists.Find (x => x.LastName.Trim().ToLower() == iProPQRSPortableLib.Consts.UserLastName.ToLower() && x.FirstName.Trim().ToLower() == iProPQRSPortableLib.Consts.UserFirstName.ToLower());
							UserDetails userDetails = listOfAnestheologists.Find (x => x.LastName == iProPQRSPortableLib.Consts.UserLastName && x.FirstName == iProPQRSPortableLib.Consts.UserFirstName);
							if(userDetails != null)
							{
								participantDetails.UserID = userDetails.ID;
								participantDetails.ProcParticipantID = 0;
								participantDetails.RoleID = 2;
								participantDetails.Name = anesNameBtn.CurrentTitle;
								if(anesNameBtn != null && anesNameBtn.CurrentTitle.Trim() != string.Empty){
									AddUpdateProcedureParticipants(participantDetails,anesNameBtn);
								}
							}
					}
				}
				anesNameBtn.Layer.CornerRadius = 5; 
				anesNameBtn.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
				anesNameBtn.TouchUpInside += async delegate {
					try
					{
						if(listOfAnestheologists.Count > 0){
							if(participantDetails == null)
								participantDetails = new ProcedureParticipantDetails();

							participantDetails.RoleID = 2;
							BindParticipantsPopupover(listOfAnestheologists,anesNameBtn,(int)anesNameBtn.Frame.Y,participantDetails);
						}
					}
					catch{
						
					}
			};
			UILabel slblstar = new UILabel (new CoreGraphics.CGRect (214, 47, 10, 41));
			slblstar.Text = "*";
			slblstar.TextColor = UIColor.Red;

			UILabel lblstart= new UILabel(new CoreGraphics.CGRect(135, 50,80,30));
			lblstart.Text = "Start Time";

			UIButton endTimeBtn=new UIButton(new CoreGraphics.CGRect(415, 50,80,30));
			UIButton startTimeBtn=new UIButton(new CoreGraphics.CGRect(230, 50,80,30));
			startTimeBtn.Tag=11;
			startTimeBtn.Layer.BorderWidth = 1;
			startTimeBtn.Layer.BorderColor = UIColor.DarkGray.CGColor;
			startTimeBtn.Layer.CornerRadius = 5; 
			startTimeBtn.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			startTimeBtn.SetTitleColor (UIColor.Black, UIControlState.Normal);			
			if (participantDetails != null && !string.IsNullOrEmpty (participantDetails.StartTime)) {
				DateTime dt = DateTime.Parse(participantDetails.StartTime);
				if(dt.ToString("HH:mm") == "00:00")
					startTimeBtn.SetTitle (string.Empty, UIControlState.Normal);
				else
					startTimeBtn.SetTitle (dt.ToString("HH:mm"), UIControlState.Normal);
			}
			if(participantDetails != null)
				{
				startTimeBtn.Tag = participantDetails.ProcParticipantID;
					ProcParticipantID=participantDetails.ProcParticipantID;
				}
				if(participantDetails == null && PrevHourTime !=0){
					if(PrevminTime != 60){						
						PrevminTime=+PrevminTime+1;
					}
					DateTime dt=new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,PrevHourTime,PrevminTime,0);

				
					string strtime=PrevHourTime.ToString();
					strtime=dt.ToString("HH:mm");
					if(dt.ToString("HH:mm") == "00:00")
						strtime = string.Empty;;

					startTimeBtn.SetTitle(strtime,UIControlState.Normal);
				}
			startTimeBtn.TouchUpInside += async delegate {
					
					if(anesNameBtn.Tag==2)
					{
						new UIAlertView("Anes Info", "Please Select Anes Name"
							, null, "ok", null).Show();
						return;							
					}

				TimePicker  tp = new TimePicker();
				tp.PresentFromPopover(startTimeBtn,40,35);
				tp._ValueChanged += delegate {								
						participantDetails.StartTime = tp.SelectedTime;
						if(tp.SelectedTime == "00:00")
							tp.SelectedTime = string.Empty;
						
						startTimeBtn.SetTitle(tp.SelectedTime,UIControlState.Normal);

						participantDetails.EndTime = endTimeBtn.CurrentTitle;
						participantDetails.ProcParticipantID=Convert.ToInt16(anesNameBtn.Tag);
						UpdateProcedureParticipants(participantDetails,anesNameBtn);

//						try
//						{
//					if (participantDetails == null)
//						participantDetails = new ProcedureParticipantDetails ();
//
//
//					startTimeBtn.SetTitle(tp.SelectedTime,UIControlState.Normal);
//					participantDetails.EndTime = endTimeBtn.CurrentTitle;
//					participantDetails.StartTime = startTimeBtn.CurrentTitle;
//
//					if(!string.IsNullOrEmpty(anesNameBtn.CurrentTitle)){
//						if(selectedAnestheologistList.Count > 0){
//							if(startTimeBtn.Tag != 0){
//										participantDetails.UserID = selectedAnestheologistList.Find(x => x.ProcParticipantID == ProcParticipantID).UserID;
//										participantDetails.ProcParticipantID = (int)ProcParticipantID;
//							}else{
//										participantDetails.UserID = selectedAnestheologistList.Find(x => x.Name !=null && x.Name.Trim().ToLower() == anesNameBtn.CurrentTitle.Trim().ToLower()).UserID;
//										participantDetails.ProcParticipantID=selectedAnestheologistList.Find(x => x.Name !=null &&  x.Name.Trim().ToLower() == anesNameBtn.CurrentTitle.Trim().ToLower()).ProcParticipantID;
//										//participantDetails.UserID = selectedAnestheologistList.Find(x => x.ProcParticipantID == ProcParticipantID).UserID;
//										//participantDetails.ProcParticipantID =ProcParticipantID;
//
//							}
//						}else{
//									UserDetails userDetails = listOfAnestheologists.Find (x => x.LastName != null && x.LastName.Trim().ToLower() + ", " + x.LastName !=null && x.FirstName.Trim().ToLower() == iProPQRSPortableLib.Consts.LoginUserFullName.Trim().ToLower());
//							participantDetails.UserID = userDetails.ID;
//						}
//						participantDetails.RoleID = 2;
//						UpdateProcedureParticipants(participantDetails,anesNameBtn);
//					}
//						}
//						catch (Exception ex)
//						{
//							
//						}
				};
			};

			UILabel elblstar = new UILabel (new CoreGraphics.CGRect (403, 47, 10, 41));
			elblstar.Text = "*";
			elblstar.TextColor = UIColor.Red;

			UILabel lblendtime= new UILabel(new CoreGraphics.CGRect(330, 50,80,30));
			lblendtime.Text = "End Time";

//			UIButton endTimeBtn=new UIButton(new CoreGraphics.CGRect(415, 50,80,30));
			
			endTimeBtn.Layer.BorderWidth = 1;
			endTimeBtn.Layer.BorderColor = UIColor.DarkGray.CGColor;
			endTimeBtn.Layer.CornerRadius = 5; 
			endTimeBtn.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			endTimeBtn.SetTitleColor (UIColor.Black, UIControlState.Normal);
			if (participantDetails != null && !string.IsNullOrEmpty (participantDetails.EndTime)) {
				DateTime dt = DateTime.Parse(participantDetails.EndTime);
					if(dt.ToString("HH:mm") == "00:00")
						endTimeBtn.SetTitle (string.Empty, UIControlState.Normal);
					else
						endTimeBtn.SetTitle (dt.ToString("HH:mm"), UIControlState.Normal);
					
					if(dt.ToString("HH:mm") != "00:00"){
						PrevHourTime=dt.Hour;
						PrevminTime=dt.Minute;
					}
			}	
			if(participantDetails != null)
				endTimeBtn.Tag = participantDetails.ProcParticipantID;
			
			endTimeBtn.TouchUpInside += async delegate {

					if(anesNameBtn.Tag==2)
					{
						new UIAlertView("Anes Info", "Please Select Anes Name"
							, null, "ok", null).Show();
						return;							
					}

				TimePicker  tp=new TimePicker();
				tp.PresentFromPopover(endTimeBtn,50,35);
				tp._ValueChanged += delegate {	
						participantDetails.EndTime = tp.SelectedTime;
						if(tp.SelectedTime == "00:00")
							tp.SelectedTime = string.Empty;
						
						endTimeBtn.SetTitle(tp.SelectedTime,UIControlState.Normal);
						participantDetails.StartTime = startTimeBtn.CurrentTitle;

						participantDetails.ProcParticipantID=Convert.ToInt16(anesNameBtn.Tag);
						UpdateProcedureParticipants(participantDetails,anesNameBtn);

						if(tp.SelectedTime != string.Empty){
							DateTime dt=Convert.ToDateTime(tp.SelectedTime);
							PrevHourTime=dt.Hour;
							PrevminTime=dt.Minute;
						}
//						try
//						{
//					if (participantDetails == null)
//						participantDetails = new ProcedureParticipantDetails ();
//					endTimeBtn.SetTitle(tp.SelectedTime,UIControlState.Normal);
//					participantDetails.EndTime = endTimeBtn.CurrentTitle;
//					participantDetails.StartTime = startTimeBtn.CurrentTitle;
//
//					if(!string.IsNullOrEmpty(anesNameBtn.CurrentTitle)){
//						if(selectedAnestheologistList.Count > 0){
//							if(endTimeBtn.Tag != 0){
//								participantDetails.UserID = selectedAnestheologistList.Find(x => x.ProcParticipantID == endTimeBtn.Tag).UserID;
//								participantDetails.ProcParticipantID = (int)endTimeBtn.Tag;
//							}else{
//
//									//	if(selectedAnestheologistList.Count> 0 && selectedAnestheologistList[0].Name != null )
//										//{
//									//	participantDetails.UserID = selectedAnestheologistList.Find(x => x.Name !=null && x.Name == anesNameBtn.CurrentTitle).UserID;
//										//participantDetails.ProcParticipantID = selectedAnestheologistList.Find(x => x.Name !=null && x.Name == anesNameBtn.CurrentTitle).ProcParticipantID;
//										//}
//										participantDetails.UserID = selectedAnestheologistList.Find(x => x.ProcParticipantID == ProcParticipantID).UserID;
//										participantDetails.ProcParticipantID =ProcParticipantID;
//
//							}
//
//						}else{
//							UserDetails userDetails = listOfAnestheologists.Find (x => x.LastName.Trim().ToLower() + ", " + x.FirstName.Trim().ToLower() == iProPQRSPortableLib.Consts.LoginUserFullName.Trim().ToLower());
//							participantDetails.UserID = userDetails.ID;
//						}
//						participantDetails.RoleID = 2;
//						UpdateProcedureParticipants(participantDetails,anesNameBtn);
//					}
//						}
//						catch (Exception ex)
//						{
//							
//						}
				};
			};

			hideid.Tag=4;
				hideid.Text=anesNameBtn.Tag.ToString();
			uvblock.Add(lblstart);
			uvblock.Add(slblstar);
			uvblock.Add(anesNameBtn);
			uvblock.Add(lbltitle);
			uvblock.Add(startTimeBtn);
			uvblock.Add(lblendtime);
			uvblock.Add(elblstar);
			uvblock.Add(endTimeBtn);
			uvblock.Add(hideid);
			Anesstarttimebtn=startTimeBtn;
			Anesendtimebtn=endTimeBtn;
			UIView uvsp=new UIView(new CGRect(0,96,497,1));
			uvsp.BackgroundColor=UIColor.Black;
			UIView uvleftline=new UIView(new CGRect(497,0,1,100));
			uvleftline.BackgroundColor=UIColor.LightGray;
			uvblock.Add(uvleftline);
			uvblock.Add(uvsp);
			//uvProviderView
			uvProviderView.Add(uvblock);
			//svBillingInfo.Add(uvblock);
			if ((Anesheight - crnahheight) >= 100) {			    
				uvProviderView.Frame = new CGRect (0, 100, 1008, float.Parse (Anesheight.ToString ()) );

				uvBillingProcedureInfo.Frame = new CoreGraphics.CGRect (0, float.Parse (Anesheight.ToString ())+100 , 1008, 800);
				svBillingInfo.SizeToFit ();
				svBillingInfo.ContentSize = new SizeF (float.Parse (svBillingInfo.Frame.Width.ToString ()), float.Parse (svBillingInfo.Frame.Height.ToString ()) + Anesheight + 200);
			}
			}
			catch (Exception ex){
				string str = "";
			}
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad();
			AppDelegate.pb.Stop();
			try
			{

			//uvDynamicSurgeonview.Add (uvMainSurgeon);
			//svBillingInfo.Add (uvDynamicSurgeonview);
			svBillingInfo.Add(uvProviderView);
			uvBillingProcedureInfo = new UIView (new CGRect (0, Anesheight + 150, 1008, 800));
			uvBillingProcedureInfo.Add (uvProcedureInfo);
			svBillingInfo.Add (uvBillingProcedureInfo);
			
			UITapGestureRecognizer uvpatinetinfoTap = new UITapGestureRecognizer(() => {
				txtpmrn.ResignFirstResponder();
				txtpFirstName.ResignFirstResponder();
				txtpLastName.ResignFirstResponder();
			});
			uvPatientInfo.UserInteractionEnabled = true;
			uvPatientInfo.AddGestureRecognizer(uvpatinetinfoTap);

			txtNotes.Layer.BorderColor = UIColor.Gray.CGColor;
			txtNotes.Layer.BorderWidth = 1;
			txtpmrn.ShouldChangeCharacters = (textField, range, replacement) =>
			{
				int number;
				return replacement.Length == 0 || int.TryParse(replacement, out number);
			};
				this.txtpmrn.ShouldReturn += (textField) => {
					textField.ResignFirstResponder();
					return true;
				};

			
			txtFacility.Enabled = false;
			txtpAge.Enabled = false;

			if (this.procedureDetails != null) {
				if (this.procedureDetails.StatusID == "1" || this.procedureDetails.StatusID == "2")
					btnSubmit.SetTitle (" Finalize", UIControlState.Normal);
					//btnSubmit.text

				if (this.procedureDetails.StatusID == "3") {
					btnSubmit.SetTitle ("Unlock", UIControlState.Normal);
					uvPatientInfo.UserInteractionEnabled = false;
					uvBillingInfo.UserInteractionEnabled = false;
					uvQualityMetrics.UserInteractionEnabled = false;
				}
			}


			InvokeOnMainThread (async () => {	
				ReceiveContext surgeons = await AppDelegate.Current.pqrsMgr.GetSurgeons();
				if (surgeons != null && surgeons.result != null && surgeons.status !=null && surgeons.status.ToUpper() =="OK") {
					lstOfSurgeons = (List<SurgeonDetails>)JsonConvert.DeserializeObject (surgeons.result.ToString (), typeof(List<SurgeonDetails>));
				 }
				 else
					NavigationController.PopToRootViewController(true);

				if (this.procedureDetails != null) {
					ReceiveContext selectedSurgeonOfProcedure = await AppDelegate.Current.pqrsMgr.GetSelectedSurgeonOfProcedure(this.procedureDetails.ID);
						if (selectedSurgeonOfProcedure != null && selectedSurgeonOfProcedure.result != null && surgeons.status !=null && surgeons.status.ToUpper() =="OK") {
						selectedSurgeon = (List<ProcedureSurgeonDetails>)JsonConvert.DeserializeObject (selectedSurgeonOfProcedure.result.ToString (), typeof(List<ProcedureSurgeonDetails>));
						if(selectedSurgeon.Count > 0)
						{
							lblSurgeon.Text = selectedSurgeon[0].Name;
							SurgeonID=selectedSurgeon[0].SurgeonID;
						}
					}
					else if (selectedSurgeonOfProcedure != null && selectedSurgeonOfProcedure.result != null && surgeons.status !=null && surgeons.status.ToUpper() =="ERROR")
						NavigationController.PopToRootViewController(true);						
					else
					   selectedSurgeon[0].ProcSurgeonID = 0;
				}
				BindSurgeon(lstOfSurgeons);

			});
				AddBasicProviderView ();
//			InvokeOnMainThread (async () => {	
//				ReceiveContext users = await AppDelegate.Current.pqrsMgr.GetUsers();
//				List<UserDetails> lstOfUsers = new List<UserDetails>();
//				if(users != null && users.result != null) {
//					lstOfUsers = (List<UserDetails>)JsonConvert.DeserializeObject (users.result.ToString (), typeof(List<UserDetails>));
//					listOfAnestheologists = lstOfUsers.FindAll(x => x.Role == "Anesthesiologist");
//					listOfCRNAs = lstOfUsers.FindAll(x => x.Role == "CRNA");
//						AddBasicProviderView ();
//
//				}
//			});

//			btnCamera.TouchUpInside += async delegate {
//				Camera.TakePicture (this, (obj) =>{
//					var photo = obj.ValueForKey(new NSString("UIImagePickerControllerOriginalImage")) as UIImage;
//					var documentsDirectory = Environment.GetFolderPath
//						(Environment.SpecialFolder.Personal);
//					string jpgFilename = System.IO.Path.Combine (documentsDirectory, DateTime.Now.Millisecond+".jpg"); // hardcoded filename, overwritten each time
//					NSData imgData = photo.AsJPEG();
//					NSError err = null;
//					if (imgData.Save(jpgFilename, false, out err)) {
//						Console.WriteLine("saved as " + jpgFilename);
//					} else {
//						Console.WriteLine("NOT saved as " + jpgFilename + " because" + err.LocalizedDescription);
//					}
//				});					
//			};

//				btnViewCam.TouchUpInside += async delegate {
//					try
//					{
//						var documentsDirectory = Environment.GetFolderPath
//							(Environment.SpecialFolder.Personal);
//
//						string jpgFilename = System.IO.Path.Combine (documentsDirectory, "PQRS.jpg");
//						List<UIImageView> currentImagelist=new List<UIImageView>(); 
//						UIImageView viewi = null;
//
//						UIImage imageToAdd = UIImage.FromFile(jpgFilename);
//						viewi= new UIImageView ();
//						if(imageToAdd == null)
//							return;
//
//						viewi.Image=imageToAdd;
//						currentImagelist.Add(viewi);
//
//						//				foreach (string eachImage in Directory.GetFiles(documentsDirectory, "*jpg"))
//						//				{
//						//					viewi= new UIImageView ();
//						//
//						//
//						//					using (UIImage imageToAdd = UIImage.FromFile(eachImage))
//						//					{
//						//						viewi.Image=imageToAdd;
//						//						currentImagelist.Add(viewi);
//						//					}
//						//
//						//				}
//
//						viewimg=new ImagesGalleryView();
//						if(currentImagelist.Count>0)
//							viewimg.currentImagelist.Add(currentImagelist[currentImagelist.Count-1]);
//
//						viewimg.PresentFromPopover(btnViewCam,10,10,480);
//						viewimg.Dispose();
//					}
//					catch (Exception ex)
//					{
//						string str=ex.Message;
//					}					
//				};

//				btnFaceShtViewCam.TouchUpInside += async delegate {
//					try
//					{
//						var documentsDirectory = Environment.GetFolderPath
//							(Environment.SpecialFolder.Personal);
//
//						string jpgFilename = System.IO.Path.Combine (documentsDirectory, "PQRS.jpg");
//						List<UIImageView> currentImagelist=new List<UIImageView>(); 
//						UIImageView viewi = null;
//
//						UIImage imageToAdd = UIImage.FromFile(jpgFilename);
//						viewi= new UIImageView ();
//						if(imageToAdd == null)
//							return;
//
//						viewi.Image=imageToAdd;
//						currentImagelist.Add(viewi);
//
//						//				foreach (string eachImage in Directory.GetFiles(documentsDirectory, "*jpg"))
//						//				{
//						//					viewi= new UIImageView ();
//						//
//						//
//						//					using (UIImage imageToAdd = UIImage.FromFile(eachImage))
//						//					{
//						//						viewi.Image=imageToAdd;
//						//						currentImagelist.Add(viewi);
//						//					}
//						//
//						//				}
//
//						viewimg=new ImagesGalleryView();
//						if(currentImagelist.Count>0)
//							viewimg.currentImagelist.Add(currentImagelist[currentImagelist.Count-1]);
//
//						viewimg.PresentFromPopover(btnFaceShtViewCam,10,10,480);
//						viewimg.Dispose();
//					}
//					catch (Exception ex)
//					{
//						string str=ex.Message;
//					}					
//				};
//			btnFaceShtCam.TouchUpInside += async delegate {
//				Camera.TakePicture (this, (obj) =>{
//					var photo = obj.ValueForKey(new NSString("UIImagePickerControllerOriginalImage")) as UIImage;
//					var documentsDirectory = Environment.GetFolderPath
//						(Environment.SpecialFolder.Personal);
//					string jpgFilename = System.IO.Path.Combine (documentsDirectory, DateTime.Now.Millisecond+".jpg"); // hardcoded filename, overwritten each time
//					NSData imgData = photo.AsJPEG();
//					NSError err = null;
//					if (imgData.Save(jpgFilename, false, out err)) {
//						Console.WriteLine("saved as " + jpgFilename);
//					} else {
//						Console.WriteLine("NOT saved as " + jpgFilename + " because" + err.LocalizedDescription);
//					}
//				});					
//			};


			ProcAttribTypes();

			ViewQualityMetrics (pqrsTypes, pqrsTypeOptions, "PQRS");	
			ViewQualityMetrics (nonPqrsTypes, nonPqrsTypeOptions, "NON PQRS");	
			svQualityMetrics.SizeToFit ();
			svQualityMetrics.ContentSize = new SizeF (float.Parse (svQualityMetrics.Frame.Width.ToString ()), float.Parse (svQualityMetrics.Frame.Height.ToString ())+hUV-250);
			BindAllTypes();
			BindAllControlWithText();

				if (patientProfile != null && patientProfile.ID != 0) {
					if (this.procedureDetails.StatusID == "2" || this.procedureDetails.StatusID == "1")
					{
						//uvQualityMetrics.UserInteractionEnabled = true;
						Setenableanddiablelbl(true);
					}
					else if (this.procedureDetails.StatusID == "3")
					{
						//uvDynamicQualityMetrics.UserInteractionEnabled = false;
						//svQualityMetrics.UserInteractionEnabled = false;
						Setenableanddiablelbl(false);
					}
					btnSubmit.Hidden=false;
				}
				else
					btnSubmit.Hidden=true;
			if (this.procedureDetails == null) {
				if (iProPQRSPortableLib.Consts.Role == "Anesthesiologist") {
					//btnAnes1.SetTitle (iProPQRSPortableLib.Consts.LoginUserFullName, UIControlState.Normal);
				}
			} else {
				if (this.procedureDetails.Anesthesiologist1 != null && this.procedureDetails.Anesthesiologist1.Trim () == string.Empty) {
					if (iProPQRSPortableLib.Consts.Role == "Anesthesiologist") {
						//btnAnes1.SetTitle (iProPQRSPortableLib.Consts.LoginUserFullName, UIControlState.Normal);
					}
				}

				InvokeOnMainThread (async () => {	
					ReceiveContext procParticipants = await AppDelegate.Current.pqrsMgr.GetProcedureParticipants(this.procedureDetails.ID);
						if(procParticipants != null && procParticipants.result != null && procParticipants.status !=null && procParticipants.status.ToUpper() =="OK" ) {
						listOfProcParticiPants = (List<ProcedureParticipantDetails>)JsonConvert.DeserializeObject (procParticipants.result.ToString (), typeof(List<ProcedureParticipantDetails>));
						selectedAnestheologistList = listOfProcParticiPants.FindAll(x => x.RoleID == 2);
						selectedAnestheologistList = selectedAnestheologistList.OrderBy( x => x.ProcParticipantID).ToList();

						selectedCRNAList = listOfProcParticiPants.FindAll(x => x.RoleID == 6);
						selectedCRNAList = selectedCRNAList.OrderBy( x => x.ProcParticipantID).ToList();

						int i = 0;
						foreach (var item in selectedAnestheologistList) {
							AddAnesView(selectedAnestheologistList[i]);
							i++;
						}

						int j = 0;
						foreach (var item in selectedCRNAList) {
							AddCRNAView(selectedCRNAList[j]);
							j++;
						}

						if(selectedAnestheologistList.Count == 0)
							AddAnesView(null);

						if(selectedCRNAList.Count == 0)
							AddCRNAView(null);		
					}
					else
						NavigationController.PopToRootViewController(true);
				});


			}

			if (this.procedureDetails != null) {
				if (this.procedureDetails.StatusID == "0" || this.procedureDetails.StatusID == "1")
						tctSurgeryDate.Enabled = false;
				
				if (this.procedureDetails.StatusID == "2" || this.procedureDetails.StatusID == "3")
					tctSurgeryDate.Enabled = false;
			}

			tctSurgeryDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
			uSEmergency.SetState (false, false);

			FacilityDetails fac = iProPQRSPortableLib.Consts.Facilities.result.Find (u => u.FMID.ToString() == iProPQRSPortableLib.Consts.SelectedFacilityID);
			txtFacility.Text = fac.FacilityName;

			NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.DidShowNotification, KeyBoardUpNotification);
			NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillHideNotification, KeyBoardDownNotification);


			PatientsBackBtn.TouchUpInside+= (object sender, EventArgs e) => {
				this.NavigationController.PopViewController(true);
			};
			//start patient infromation
		
			SetViewBorderColor ();

			BindDropDown ();
			if (procedureDetails == null) {
				sCtab.SetEnabled (false, 1);
				sCtab.SetEnabled (false, 2);
			} else {
				sCtab.SetEnabled (true, 1);
				sCtab.SetEnabled (true, 2);
			}
			svuvBillingInfo.Frame=uvBillingInfo.Frame;
				svuvBillingInfo.SizeToFit ();
				svuvBillingInfo.ContentSize = new SizeF (float.Parse (svuvBillingInfo.Frame.Width.ToString ()), float.Parse (svuvBillingInfo.Frame.Height.ToString ())+430);
			svuvBillingInfo.Add(uvBillingInfo);

			sCtab.ValueChanged += (object sender, EventArgs e) => {
				uvMainPatientinfo.RemoveFromSuperview ();
				uvDynamicQualityMetrics.RemoveFromSuperview ();
					svuvBillingInfo.RemoveFromSuperview ();
				if (sCtab.SelectedSegment == 0){
					uvMain.AddSubview (uvPatientInfo);
				}else if (sCtab.SelectedSegment == 1){
						if(!anesadded){
							AddAnesView(null);
						}
						if(!crnaadded){
							AddCRNAView(null);
						}
						uvMain.AddSubview (svuvBillingInfo);
				}else if (sCtab.SelectedSegment == 2) {
					uvMain.AddSubview (uvDynamicQualityMetrics);
				} 

			};
				lblProcID.Hidden = true;
			///End patient infromation
			// Perform any additional setup after loading the view, typically from a nib.

			// start Quality metrics
			DropReason.Hidden=true;
			uSwitchCaseCanceled.ValueChanged+= (object sender, EventArgs e) => {
				if(uSwitchCaseCanceled.On)
				{
					uvPreIntraOp.Hidden=true;
					DropReason.Hidden=false;
				}
				else
				{
					uvPreIntraOp.Hidden=false;
					DropReason.Hidden=true;
				}
			};

			SliderPreopPainScore.ValueChanged+= (object sender, EventArgs e) => {				
				LblPreopPainScoreValue.Text=((int)Math.Round(SliderPreopPainScore.Value,0)).ToString();
			};

			uvScrollview.ContentSize = new CoreGraphics.CGSize (0, 1350);
			dropPotoc.Hidden = true;
			scPotoc.ValueChanged+= (object sender, EventArgs e) => {
				dropPotoc.Hidden=false;
			};
			scAnyUnwanted.ValueChanged+= (object sender, EventArgs e) => {

				if(scAnyUnwanted.SelectedSegment==0)
					dropAnyUnwanted.Hidden=false;
				else
					dropAnyUnwanted.Hidden=true;
			};
			// End Quality metrics
//			SignatureView vns=new SignatureView(new System.Drawing.RectangleF(0f,0f,(float)uvNurseSignaturevalue.Bounds.Width,(float)uvNurseSignaturevalue.Bounds.Height));
//			uvNurseSignaturevalue.AddSubview(vns);
//			SignatureView vps=new SignatureView(new System.Drawing.RectangleF(0f,0f,(float)uvPatientSignature.Bounds.Width,(float)uvPatientSignature.Bounds.Height));
//			uvPatientSignature.AddSubview (vps);

			if (patientProfile != null && patientProfile.ID != 0) {
				lblFullName.Text = patientProfile.LastName.Trim () + ", " + patientProfile.FirstName.Trim ();
				mrnLbl.Text = "MRN: " + patientProfile.MRN.Trim ();
				txtpmrn.Text = patientProfile.MRN;
				txtpmrn.Enabled = false;
				txtpFirstName.Text = patientProfile.FirstName;
				txtpLastName.Text = patientProfile.LastName;

				if (txtpmrn.Text.Trim () != string.Empty) {
//					lblMRN.Text = "MRN";
					txtpmrn.Layer.BorderColor = UIColor.Gray.CGColor;
				} else {
//					lblMRN.Text = "*MRN";
					txtpmrn.Layer.BorderColor = UIColor.Red.CGColor;
					txtpmrn.Layer.BorderWidth = 1f;
					txtpmrn.Layer.CornerRadius = (nfloat)8.0; 
				}

				if (txtpFirstName.Text.Trim () != string.Empty) {
//					lblFirstName.Text = "First Name";
					txtpFirstName.Layer.BorderColor = UIColor.Gray.CGColor;
				} else {
//					lblFirstName.Text = "*First Name";
					txtpFirstName.Layer.BorderColor = UIColor.Red.CGColor;
					txtpFirstName.Layer.BorderWidth = 1f;
					txtpFirstName.Layer.CornerRadius = (nfloat)8.0; 
				}

				if (txtpLastName.Text.Trim () != string.Empty) {
//					lblLastName.Text = "Last Name";
					txtpLastName.Layer.BorderColor = UIColor.Gray.CGColor;
				} else {
//					lblLastName.Text = "*Last Name";
					txtpLastName.Layer.BorderColor = UIColor.Red.CGColor;
					txtpLastName.Layer.BorderWidth = 1f;
					txtpLastName.Layer.CornerRadius = (nfloat)8.0; 
				}

				string patientGender = string.Empty;
				if (!string.IsNullOrEmpty (patientProfile.Sex)) {
					switch (patientProfile.Sex.Trim ().ToLower ()) {
					case	"m":
						patientGender = "Male";
							patientGenderID=1;
						break;
					case "f":
						patientGender = "Female";
							patientGenderID=2;
						break;
					//case "u":
					//	patientGender = "Unknown";
						//	patientGenderID=3;
					//	break;
					default:
						break;
					} 
				}

				btnPatientGender.SetTitle (patientGender, UIControlState.Normal);
				string tempDOB;
					//patientGenderID

				if (!string.IsNullOrEmpty (patientProfile.DOB)) {
					tempDOB = Convert.ToDateTime (patientProfile.DOB).ToString ("MM/dd/yyyy");
					txtpDOB.Text = tempDOB;

					int age = DateTime.Now.Year - Convert.ToDateTime (patientProfile.DOB).Year;
					txtpAge.Text = age.ToString ();
				}

				string surgeryDate;
				if (!string.IsNullOrEmpty (procedureDetails.OperationDate)) {
					surgeryDate = Convert.ToDateTime (procedureDetails.OperationDate).ToString ("MM/dd/yyyy");
					tctSurgeryDate.Text = surgeryDate;
				}

				if (patientProfile.DiagnosticList != null && patientProfile.DiagnosticList.Count > 0) {
					foreach (var item in patientProfile.DiagnosticList) {
						NewDiagnosis (item.Code, item.Name);
						selectedDiagnosisCodeid.Add (item.ProcCodeID);
					}
				} else 
					NewDiagnosis (string.Empty, string.Empty);

				if(patientProfile.ProcedureList != null && patientProfile.ProcedureList.Count > 0){
					foreach (var item in patientProfile.ProcedureList) {
						AddProcedure (item.Code, item.Name);
						selectedprocedureCodeid.Add (item.ProcCodeID);
					}
				}else
					AddProcedure (string.Empty, string.Empty);
			} else {
//				lblMRN.Text = "*MRN";
//				lblFirstName.Text = "*First Name";
//				lblLastName.Text = "*Last Name";
				txtpmrn.Layer.BorderColor = UIColor.Red.CGColor;
				txtpFirstName.Layer.BorderColor = UIColor.Red.CGColor;
				txtpLastName.Layer.BorderColor = UIColor.Red.CGColor;
				txtpmrn.Layer.BorderWidth = 1f;
				txtpFirstName.Layer.BorderWidth = 1f;
				txtpLastName.Layer.BorderWidth = 1f;
				txtpmrn.Layer.CornerRadius = (nfloat)8.0; 
				txtpFirstName.Layer.CornerRadius = (nfloat)8.0; 
				txtpLastName.Layer.CornerRadius = (nfloat)8.0; 
			}

			//Kumar start
			if (procedureDetails == null ) {
				EncounterBtn.Enabled = false;
				PhysicalStatusBtn.Enabled = false;
				uSEmergency.Enabled = false;
				addNewDiagnosis.Enabled = false;
				deleteDiagnosis.Enabled = false;
				removeProc.Enabled = false;
				addNewProc.Enabled = false;
				NewDiagnosis (string.Empty, string.Empty);
				AddProcedure (string.Empty, string.Empty);
			}else
					lblProcID.Text = "Proc ID: "+procedureDetails.ID.ToString ();
			
			if (procedureDetails != null &&  !string.IsNullOrEmpty(procedureDetails.ddlEncounterType)) {
				EncounterBtn.SetTitle (procedureDetails.ddlEncounterType, UIControlState.Normal);
				if(dEncounterTypelist != null && dEncounterTypelist.Count > 0)
					encounterTypeid = dEncounterTypelist.Where (w => w.DropDownText.ToUpper().Trim() == procedureDetails.ddlEncounterType.ToUpper().Trim()).Select (s => s.DropDownID).SingleOrDefault ();
			}
			if (procedureDetails != null &&  !string.IsNullOrEmpty(procedureDetails.ddlASAType)) {
				string btntxt;
				if (dlPhyStatusList != null && dlPhyStatusList.Count > 0)
					physicalStatus = Convert.ToInt16 (procedureDetails.ddlASAType);
				btntxt = dlPhyStatusList.Where (w => w.DropDownID ==Convert.ToInt16(procedureDetails.ddlASAType)).Select (s => s.DropDownText).SingleOrDefault ();
				PhysicalStatusBtn .SetTitle (btntxt, UIControlState.Normal);

			}
			if (procedureDetails != null && !string.IsNullOrEmpty (procedureDetails.cbEmergency)) {
				Emergency = procedureDetails.cbEmergency;
				if (procedureDetails.cbEmergency == "1")
					uSEmergency.On = true;
				else
					uSEmergency.On = false;
			}
			//Kumar end
			
				tctSurgeryDate.ShouldBeginEditing += delegate {
					DatePicker  dp=new DatePicker();

					CoreGraphics.CGRect f=tctSurgeryDate.Frame;
					dp.PresentFromPopover(this.View,float.Parse(f.X.ToString()),float.Parse(f.Y.ToString())+100);
					dp._ValueChanged += delegate {	
						tctSurgeryDate.Text=dp.SelectedDate;
						string format = "MM/dd/yyyy"; 
						DateTime surgeryDate = DateTime.ParseExact(tctSurgeryDate.Text, format,System.Globalization.CultureInfo.CurrentCulture);

						if(DateTime.Now.Date < surgeryDate){
							sCtab.SetEnabled (false, 1);
							sCtab.SetEnabled (false, 2);
						}
					};
					return false;
				};

			//tctSurgeryDate.EditingDidBegin+= (object sender, EventArgs e) => {
					
			//};

			txtpDOB.ShouldBeginEditing += delegate {
					txtpmrn.ResignFirstResponder();
					txtpFirstName.ResignFirstResponder();
					txtpLastName.ResignFirstResponder();
			 	DatePicker  dp = new DatePicker();
					dp.MaximumDate=DateTime.Now;
				if(!string.IsNullOrEmpty(txtpDOB.Text))
					{
						//DateTime dtval=DateTime.ParseExact(txtpDOB.Text,"MM/dd/yyyy",System.Globalization.CultureInfo.InvariantCulture);
						string format = "MM/dd/yyyy"; 
						DateTime dt = DateTime.ParseExact(txtpDOB.Text, format,System.Globalization.CultureInfo.CurrentCulture);
						dp.SelectedDateValue=dt;
					}

				CoreGraphics.CGRect f = txtpDOB.Frame;
				dp.PresentFromPopover(this.View,float.Parse(f.X.ToString()),float.Parse(f.Y.ToString())+100);
				dp._ValueChanged += delegate {	
					if(dp.SelectedDateValue != DateTime.MinValue){	
						int age = DateTime.Now.Year - dp.SelectedDateValue.Year;
						if((dp.SelectedDateValue.Month > DateTime.Now.Month) || (dp.SelectedDateValue.Month == DateTime.Now.Month && dp.SelectedDateValue.Day > DateTime.Now.Day))
							age--;
						//this.mPatient.DOB = dp.SelectedDate;
								
							txtpDOB.Text=dp.SelectedDateValue.ToString ("MM/dd/yyyy");
						txtpAge.Text=age.ToString();
					}else{
						txtpDOB.Text = string.Empty;
						txtpAge.Text = string.Empty;
					}
					SavePatientInfo();
				};

				return false;
			};


			txtpFirstName.EditingDidEnd += async (object sender, EventArgs e) => {
					showKeyBoard = false;
					txtpmrn.ResignFirstResponder();
					txtpFirstName.ResignFirstResponder();
					txtpLastName.ResignFirstResponder();

				SavePatientInfo();
				if (txtpFirstName.Text.Trim () != string.Empty){
//					lblFirstName.Text = "First Name";
					txtpFirstName.Layer.BorderColor = UIColor.Gray.CGColor;
				}else{
//					lblFirstName.Text = "*First Name";
					txtpFirstName.Layer.BorderColor = UIColor.Red.CGColor;
					txtpFirstName.Layer.CornerRadius = (nfloat)8.0; 
				}
			};

			txtpLastName.EditingDidEnd += async (object sender, EventArgs e) => {
					showKeyBoard = false;
					txtpmrn.ResignFirstResponder();
					txtpFirstName.ResignFirstResponder();
					txtpLastName.ResignFirstResponder();

				SavePatientInfo();
				if (txtpLastName.Text.Trim () != string.Empty){
//					lblLastName.Text = "Last Name";
					txtpLastName.Layer.BorderColor = UIColor.Gray.CGColor;
				}else{
//					lblLastName.Text = "*Last Name";
					txtpLastName.Layer.BorderColor = UIColor.Red.CGColor;
					txtpLastName.Layer.CornerRadius = (nfloat)8.0; 
				}
			};	
				this.View.EndEditing(true);
			//	txtpmrn.Delegate= new KeyBoardHideTextFieldDelegate();
			txtpmrn.EditingDidEnd+= async (object sender, EventArgs e) => {
					showKeyBoard = false;
					txtpmrn.ResignFirstResponder();
					txtpFirstName.ResignFirstResponder();
					txtpLastName.ResignFirstResponder();

				if(!string.IsNullOrEmpty(txtpmrn.Text))
				{
					AppDelegate.pb.Start(this.View,"Please wait...");
					ReceiveContext context = new ReceiveContext();
					context = await serv.CheckExistingPatintInfo(txtpmrn.Text,iProPQRSPortableLib.Consts.SelectedFacilityID);				
					if(context.result!=null && context.status !=null && context.status.ToUpper() =="OK")
					{
						Patient patientDetails = (Patient)JsonConvert.DeserializeObject(context.result.ToString() , typeof(Patient));
						EncounterBtn.Enabled = true;
						PhysicalStatusBtn.Enabled = true;
						uSEmergency.Enabled = true;
						addNewDiagnosis.Enabled = true;
						deleteDiagnosis.Enabled = true;
						removeProc.Enabled = true;
						addNewProc.Enabled = true;

						if(patientDetails!=null){						
							patientProfile = patientDetails;
								lblFullName.Text = patientDetails.LastName.Trim() + ", " + patientDetails.FirstName.Trim();
							mrnLbl.Text = "MRN: " + patientDetails.MRN.Trim();

							txtpmrn.Text = patientDetails.MRN;
							txtpFirstName.Text = patientDetails.FirstName;
							txtpLastName.Text = patientDetails.LastName;
							if (txtpmrn.Text.Trim () != string.Empty) {
//								lblMRN.Text = "MRN";
								txtpmrn.Layer.BorderColor = UIColor.Gray.CGColor;
							} else {
//								lblMRN.Text = "*MRN";
								txtpmrn.Layer.BorderColor = UIColor.Red.CGColor;
								txtpmrn.Layer.BorderWidth = 1f;
							}

							if (txtpFirstName.Text.Trim () != string.Empty) {
//								lblFirstName.Text = "First Name";
								txtpFirstName.Layer.BorderColor = UIColor.Gray.CGColor;
							} else {
//								lblFirstName.Text = "*First Name";
								txtpFirstName.Layer.BorderColor = UIColor.Red.CGColor;
								txtpFirstName.Layer.BorderWidth = 1f;
							}

							if (txtpLastName.Text.Trim () != string.Empty) {
//								lblLastName.Text = "Last Name";
								txtpLastName.Layer.BorderColor = UIColor.Gray.CGColor;
							} else {
//								lblLastName.Text = "*Last Name";
								txtpLastName.Layer.BorderColor = UIColor.Red.CGColor;
								txtpLastName.Layer.BorderWidth = 1f;
							}
								DateTime tDOB=Convert.ToDateTime (patientDetails.DOB);
								txtpDOB.Text = tDOB.ToString ("MM/dd/yyyy");

							int age = DateTime.Now.Year - Convert.ToDateTime (patientDetails.DOB).Year;
								if((tDOB.Month > DateTime.Now.Month) || (tDOB.Month == DateTime.Now.Month && tDOB.Day > DateTime.Now.Day))
									age--;
							txtpAge.Text = age.ToString ();
								//btnSubmit.Hidden=false
							//Nahid Ahmed Aug 4, 2015
							//As per Raj, dont populate Physical Status, Encounter and Emergency from Patient Details
							//as these items belong to procedures.
							/* DropDownModel pitem = dlPhyStatusList.Find (u => u.DropDownText == patientDetails.ddlASAType);
							if (pitem != null) {
								PhysicalStatusBtn.SetTitle (pitem.DropDownText, UIControlState.Normal);
								physicalStatus = pitem.DropDownID;
							} else {
								PhysicalStatusBtn.SetTitle ("Select", UIControlState.Normal);
								physicalStatus = 0;
							}

							DropDownModel eitem = dEncounterTypelist.Find (u => u.DropDownText == patientDetails.ddlEncounterType);
							if (eitem != null) {
								EncounterBtn.SetTitle (eitem.DropDownText, UIControlState.Normal);
								encounterTypeid = eitem.DropDownID;
							} else {
								EncounterBtn.SetTitle ("Select", UIControlState.Normal);
								encounterTypeid = 0;
							}

							if (patientDetails.cbEmergency != null && patientDetails.cbEmergency.Trim().ToLower () == "on")
								uSEmergency.SetState (true, false);
							else
								uSEmergency.SetState (false, false); */

						}
							else if(context.result!=null && context.status !=null && context.status.ToUpper() =="ERROR")
								NavigationController.PopToRootViewController(true);
							else{
							//	txtpmrn.Text = "";
							lblMRN.Text = "MRN";
							txtpmrn.Layer.BorderColor = UIColor.Gray.CGColor;
							txtpmrn.Layer.BorderWidth = 1f;

//							lblFirstName.Text = "*First Name";
//							lblLastName.Text = "*Last Name";
							txtpFirstName.Layer.BorderColor = UIColor.Red.CGColor;
							txtpLastName.Layer.BorderColor = UIColor.Red.CGColor;

							txtpFirstName.Layer.BorderWidth = 1f;
							txtpLastName.Layer.BorderWidth = 1f;
							txtpmrn.Layer.CornerRadius = (nfloat)8.0; 
							txtpFirstName.Layer.CornerRadius = (nfloat)8.0; 
							txtpLastName.Layer.CornerRadius = (nfloat)8.0; 


							txtpFirstName.Text = "";
							txtpLastName.Text = "";
							txtpDOB.Text = "";
							mrnLbl.Text = "";
							lblFullName.Text = "";
						}
						AppDelegate.pb.Stop();
					}
						else if(context.result!=null && context.status !=null && context.status.ToUpper() =="ERROR")
						NavigationController.PopToRootViewController(true);
					else{
						//	txtpmrn.Text = "";
							//	txtpmrn.Text = "";
							if (txtpmrn.Text.Trim () != string.Empty) {
//								lblMRN.Text = "MRN";
								txtpmrn.Layer.BorderColor = UIColor.Gray.CGColor;
							} else {
//								lblMRN.Text = "*MRN";
								txtpmrn.Layer.BorderColor = UIColor.Red.CGColor;
								txtpmrn.Layer.BorderWidth = 1f;
								txtpmrn.Layer.CornerRadius = (nfloat)8.0; 
							}
//							lblFirstName.Text = "*First Name";
//							lblLastName.Text = "*Last Name";						
							txtpFirstName.Layer.BorderColor = UIColor.Red.CGColor;
							txtpLastName.Layer.BorderColor = UIColor.Red.CGColor;						
							txtpFirstName.Layer.BorderWidth = 1f;
							txtpLastName.Layer.BorderWidth = 1f;

							txtpFirstName.Layer.CornerRadius = (nfloat)8.0; 
							txtpLastName.Layer.CornerRadius = (nfloat)8.0; 

							txtpFirstName.Text = "";
							txtpLastName.Text = "";
							txtpDOB.Text = "";
							mrnLbl.Text="";
							lblFullName.Text="";
							EncounterBtn.Enabled = false;
							PhysicalStatusBtn.Enabled = false;
							uSEmergency.Enabled = false;
							addNewDiagnosis.Enabled = false;
							deleteDiagnosis.Enabled = false;
							removeProc.Enabled = false;
							addNewProc.Enabled = false;

					}
				}else{
//					lblMRN.Text = "*MRN";
					txtpmrn.Layer.BorderColor = UIColor.Red.CGColor;
					txtpmrn.Layer.CornerRadius = (nfloat)8.0; 
				}
				
				AppDelegate.pb.Stop();
			};
			
				this.txtpmrn.ShouldReturn += (textField) => { 
					txtpmrn.ResignFirstResponder();
					txtpFirstName.ResignFirstResponder();
					txtpLastName.ResignFirstResponder();
					textField.ResignFirstResponder();
					return true; 
				};
				txtpFirstName.ShouldReturn += (textField) => { 
					txtpmrn.ResignFirstResponder();
					txtpFirstName.ResignFirstResponder();
					txtpLastName.ResignFirstResponder();

					textField.ResignFirstResponder();
					return true; 
				};
				txtpLastName.ShouldReturn += (textField) => { 
					txtpmrn.ResignFirstResponder();
					txtpFirstName.ResignFirstResponder();
					txtpLastName.ResignFirstResponder();
					textField.ResignFirstResponder();
					return true; 
				};
				txtpDOB.ShouldReturn += (textField) => { 
					txtpmrn.ResignFirstResponder();
					txtpFirstName.ResignFirstResponder();
					txtpLastName.ResignFirstResponder();
					textField.ResignFirstResponder();
					return true; 
				};
			btnSubmit.TouchUpInside+= async (object sender, EventArgs e) => {
//				SavePatientInfo();
				if(this.procedureDetails != null){

					
						
						bool chechvalidation=await submitFinalizecase();

						
					//ValidateAndUpdateProcedure(string.Empty,string.Empty);
				}
			};

			addNewDiagnosis.TouchUpInside += (object sender, EventArgs e) => {
				NewDiagnosis (string.Empty,string.Empty);
			};
			deleteDiagnosis.TouchUpInside += async (object sender, EventArgs e) => {
				//RemoveDiagnosis();
				DeleteDiagnosis();
			};


		




			addNewProc.TouchUpInside += (object sender, EventArgs e) => {
				AddProcedure(string.Empty,string.Empty);
			};
			removeProc.TouchUpInside += async (object sender, EventArgs e) => {
				//RemoveProcedures();
				DeleteProcedures();
			};
			 
			//billing info start
			svBillingInfo.SizeToFit ();
			svBillingInfo.ContentSize = new SizeF (float.Parse (svBillingInfo.Frame.Width.ToString ()), float.Parse (svBillingInfo.Frame.Height.ToString ())+500);
			//billing info End


				int mw=MeasureTextLine(lblNerveBlock.Text);
				int maxheight=30;
				if(mw>500)
				{
					maxheight=55;
					lblNerveBlock.Lines=2;
				}
				else if(mw> 1000)
				{
					maxheight=80;
					lblNerveBlock.Lines=2;
				}
				CoreGraphics.CGRect fram=lblNerveBlock.Frame;
				fram.Height=maxheight;
				lblNerveBlock.Frame=fram;


			}
			catch (Exception ex) {
				NavigationController.PopToRootViewController(true);
				//new UIAlertView("Procedure Info", "Successfully Saved."	, null, "ok", null).Show();
			}
		}

		public void ProcAttribTypes()
		{
			#region QUALITY METRICS
			pqrsTypes = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Types.FindAll(x => x.ProcAttribGroupID == 48 && x.IsActive == true && x.UIControlType == "dd");

			pqrsTypeOptions = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.FindAll(x => pqrsTypes.Select(y => y.ProcAttribTypeID).Contains(x.ProcAttribTypeID)); 
			nonPqrsTypes = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Types.FindAll(x => x.ProcAttribGroupID == 49 && x.IsActive == true && x.UIControlType == "dd");
			nonPqrsTypeOptions = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.FindAll(x => nonPqrsTypes.Select(y => y.ProcAttribTypeID).Contains(x.ProcAttribTypeID));
			nonPqrsTypeAS10Options = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Types.FindAll(u=> iProPQRSPortableLib.Consts.as10Options.Contains(u.ProcAttribTypeID) && u.IsActive == true); 
//			iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.OrderBy(x => x.ProcAttribTypeID).ToList();
//			iProPQRSPortableLib.Consts.ProcAttribTypes.result.Types = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Types.OrderBy(x => x.ProcAttribTypeID).ToList();
			#endregion
			requiredAnesTechTypes = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Types.FindAll(u=> iProPQRSPortableLib.Consts.RequiredAnesthesiaTechIDs.Contains(u.ProcAttribTypeID)); 
			requiredLineTypeItems = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Types.FindAll(u=> iProPQRSPortableLib.Consts.RequiredLineTypeIDs.Contains(u.ProcAttribTypeID)); 
			requiredNerveBlockTypes = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Types.FindAll (u => iProPQRSPortableLib.Consts.RequiredNerveBlockIDs.Contains (u.ProcAttribTypeID));
			requiredSpecialTechTypes = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Types.FindAll (u => iProPQRSPortableLib.Consts.RequiredSpecialTechIDs.Contains (u.ProcAttribTypeID));
			requiredCVCSterileTechsTypes = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.FindAll (u => u.ProcAttribTypeID == iProPQRSPortableLib.Consts.procAttribTypeIDOfCVCSterileTech);
			requiredUltraSoundGuidance = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Types.Find (u => u.ProcAttribTypeID == 474);
			requiredContCatheter = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Types.Find (u => u.ProcAttribTypeID == 473);
			requiredOpAnesthesia = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Types.Find (u => u.ProcAttribTypeID == 480);
			requiredPostAnalgesia = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Types.Find (u => u.ProcAttribTypeID == 481);

			ASA8MasterList = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.FindAll(x => x.ProcAttribTypeID == 685);
			ASA8SubMasterList = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.FindAll(x => x.ProcAttribTypeID == 607);
			ASA9MasterList = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.FindAll(x => x.ProcAttribTypeID == 687);
			ASA9SubMasterList = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.FindAll(x => x.ProcAttribTypeID == 656);

			if (iProPQRSPortableLib.Consts.SelectedProcAttribtslist != null) {

				#region ASA8
				selectedMasterASA8OptionsIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll(u => (ASA8MasterList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).Select(x=>x.ProcAttribTypeID).ToList();
				selectedSubMasterASA8OptionsIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll(u => (ASA8SubMasterList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).Select(x=>x.ProcAttribTypeID).ToList();
				#endregion


				#region ASA9
				selectedMasterASA9OptionsIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll(u => (ASA9MasterList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).Select(x=>x.ProcAttribTypeID).ToList();
				selectedSubMasterASA9OptionsIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll(u => (ASA9SubMasterList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).Select(x=>x.ProcAttribTypeID).ToList();
				#endregion

				selectedAnesTechTypesIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (requiredAnesTechTypes.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).Select (x => x.ProcAttribTypeID).ToList ();
				selectedLineTypeItemsIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll(u => (requiredLineTypeItems.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).Select(x=>x.ProcAttribTypeID).ToList();
				selectedNerveBlockIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll(u => (requiredNerveBlockTypes.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).Select(x=>x.ProcAttribTypeID).ToList();
				selectedSpecialTechIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll(u => (requiredSpecialTechTypes.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).Select(x=>x.ProcAttribTypeID).ToList();
				selectednonPqrsTypeAS10OptionsIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll(u => (nonPqrsTypeAS10Options.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).Select(x=>x.ProcAttribTypeID).ToList();
				if (iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.Find(x => x.ProcAttribTypeID == 655) != null) {
					txtNotes.Text = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.Find (x => x.ProcAttribTypeID == 655).Value;
				}

				if (iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.Find (x => x.ProcAttribTypeID == 602) != null) {
					allCVCSterileTechsOptions = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (x => x.ProcAttribTypeID == 602);
					selectedLineCVCSterileTechIds = allCVCSterileTechsOptions.FindAll(u => (requiredCVCSterileTechsTypes.Select (z => z.Value)).Contains (u.Value)).Select(x=>x.Value).ToList();
				}

				if (iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.Find (u => u.ProcAttribTypeID == 474) != null) {
					if (iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.Find (u => u.ProcAttribTypeID == 474).Value == "1")
						swUltraSoundGuidance.SetState (true, false);
					else
						swUltraSoundGuidance.SetState (false, false);
				}

				if (iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.Find (u => u.ProcAttribTypeID == 473) != null) {
					if (iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.Find (u => u.ProcAttribTypeID == 473).Value == "1")
						swContCatheterType.SetState (true, false);
					else
						swContCatheterType.SetState (false, false);
				}

				if (iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.Find (u => u.ProcAttribTypeID == 480) != null) {
					if (iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.Find (u => u.ProcAttribTypeID == 480).Value == "1")
						swOpAnesthesiaType.SetState (true, false);
					else
						swOpAnesthesiaType.SetState (false, false);
				}

				if (iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.Find (u => u.ProcAttribTypeID == 481) != null) {
					if (iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.Find (u => u.ProcAttribTypeID == 481).Value == "1")
						swPostOpAnalgesiaType.SetState (true, false);
					else
						swPostOpAnalgesiaType.SetState (false, false);
				}

			}

			if(requiredUltraSoundGuidance != null)
				lblUltraSoundGuidance.Text = requiredUltraSoundGuidance.Label; 

			if(requiredContCatheter != null)
				lblContCatheterType.Text = requiredContCatheter.Label; 

			if(requiredPostAnalgesia != null)
				lblPostOpAnalgesiaType.Text = requiredPostAnalgesia.Label; 

			if(requiredOpAnesthesia != null)
				lblOpAnesthesiaType.Text = requiredOpAnesthesia.Label; 

		}
		public async void UpdateProcedureStatus()
		{			
			this.procedureDetails.StatusID = "2";
			ReceiveContext result =await AppDelegate.Current.pqrsMgr.UpdatePatintProcedureInfo (procedureDetails);
		}
		bool Notekeyboard=false;
		private void BindAllTypes()
		{
			lblCVCSterileTechs.Layer.BorderColor = UIColor.Gray.CGColor;
			lblCVCSterileTechs.Layer.BorderWidth = (nfloat)0.5;
			lblCVCSterileTechs.Layer.CornerRadius = (nfloat)8.0; 
			UITapGestureRecognizer lblCVCSterileTechsTap = new UITapGestureRecognizer(() => {
				//BindPopupover(requiredCVCSterileTechsTypes,selectedLineCVCSterileTechIds,lblCVCSterileTechs,0,"CVCSterileTech");
				//
				qmBindPopupover(requiredCVCSterileTechsTypes,lblCVCSterileTechs,0,602);
			});
			lblCVCSterileTechs.UserInteractionEnabled = true;
			lblCVCSterileTechs.AddGestureRecognizer(lblCVCSterileTechsTap);

			lblALineType.Text = requiredLineTypeItems.Find (x => x.ProcAttribTypeID == 626).Label; 
			lblPACatheterType.Text = requiredLineTypeItems.Find (x => x.ProcAttribTypeID == 627).Label; 
			lblCVPType.Text = requiredLineTypeItems.Find (x => x.ProcAttribTypeID == 628).Label; 

			if (selectedLineTypeItemsIds.Count > 0) {
				if (selectedLineTypeItemsIds.Contains (626))
					swALineType.SetState (true, false);

				if (selectedLineTypeItemsIds.Contains (627))
					swPACatheterType.SetState (true, false);

				if (selectedLineTypeItemsIds.Contains (628))
					swCVPType.SetState (true, false);
			}

			previousALineTypeValue = swALineType.On;
			swALineType.ValueChanged += async delegate {				
				if(swALineType.On != previousALineTypeValue)
				{
					iProPQRSPortableLib.Type lineTypeObj = requiredLineTypeItems.Find(x => x.ProcAttribTypeID == 626);
					UpdateLineType(swALineType,lineTypeObj);
					previousALineTypeValue=swALineType.On;
				}
			};
			pervPACatheterType = swPACatheterType.On;
			swPACatheterType.ValueChanged += async delegate {	
				if(swPACatheterType.On != pervPACatheterType)
				{
					iProPQRSPortableLib.Type lineTypeObj = requiredLineTypeItems.Find(x => x.ProcAttribTypeID == 627);
					UpdateLineType(swPACatheterType,lineTypeObj);				
					pervPACatheterType=swPACatheterType.On;
				}
			};
			prevCVPType = swCVPType.On;
			swCVPType.ValueChanged += async delegate {
				if(prevCVPType != swCVPType.On)
				{
					iProPQRSPortableLib.Type lineTypeObj = requiredLineTypeItems.Find(x => x.ProcAttribTypeID == 628);
					UpdateLineType(swCVPType,lineTypeObj);
					prevCVPType = swCVPType.On;
				}
			};
			prevUltraSoundGuidance = swUltraSoundGuidance.On;
			swUltraSoundGuidance.ValueChanged += async delegate {
				if(prevUltraSoundGuidance != swUltraSoundGuidance.On)
				{	
					UpdateLineType(swUltraSoundGuidance, requiredUltraSoundGuidance);
					prevUltraSoundGuidance=swUltraSoundGuidance.On;
				}
			};
			prevContCatheterType = swContCatheterType.On;
			swContCatheterType.ValueChanged += async delegate {
				if(prevContCatheterType != swContCatheterType.On)
				{
					UpdateLineType(swContCatheterType, requiredContCatheter);
					prevContCatheterType=swContCatheterType.On;
				}
			};
			prevOpAnesthesiaType = swOpAnesthesiaType.On;
			swOpAnesthesiaType.ValueChanged += async delegate {
				if(prevOpAnesthesiaType != swOpAnesthesiaType.On)
				{
					UpdateLineType(swOpAnesthesiaType, requiredOpAnesthesia);
					prevOpAnesthesiaType=swOpAnesthesiaType.On;
				}
			};
			prevPostOpAnalgesiaType = swPostOpAnalgesiaType.On;
			swPostOpAnalgesiaType.ValueChanged += async delegate {

				if(prevPostOpAnalgesiaType != swPostOpAnalgesiaType.On)
				{
					UpdateLineType(swPostOpAnalgesiaType, requiredPostAnalgesia);
					prevPostOpAnalgesiaType=swPostOpAnalgesiaType.On;
				}
			};
			//txtNotes.Text = "kumar";
			txtNotes.ShouldBeginEditing += delegate {
				svBillingInfo.SetContentOffset(new CGPoint(svBillingInfo.Frame.X,svBillingInfo.Frame.Height-380),false);
				return true;
			};
			txtNotes.ShouldEndEditing += delegate {
				
				UpdateOtherNotes();	
				svBillingInfo.ScrollsToTop=true;
				return true;
			};

			lblAnesTechs.Layer.BorderColor = UIColor.Gray.CGColor;
			lblAnesTechs.Layer.BorderWidth = (nfloat)0.5;
			lblAnesTechs.Layer.CornerRadius = (nfloat)8.0; 
			lblAnesTechs.TextColor = UIColor.Black;
			UITapGestureRecognizer lblAnesTechTechTap = new UITapGestureRecognizer(() => {
				requiredAnesTechTypes = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Types.FindAll(u=> iProPQRSPortableLib.Consts.RequiredAnesthesiaTechIDs.Contains(u.ProcAttribTypeID)); 

				if(iProPQRSPortableLib.Consts.SelectedProcAttribtslist!=null)					
				selectedAnesTechTypesIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (requiredAnesTechTypes.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).Select (x => x.ProcAttribTypeID).ToList ();
				
				BindmultilevelPopupover(requiredAnesTechTypes,selectedAnesTechTypesIds,selectedAnesTechTypesIds,lblAnesTechs,0,"AnesthesiaTechs");
				//BindPopupover(requiredAnesTechTypes,selectedAnesTechTypesIds,lblAnesTechs,0,"AnesthesiaTechs");
			});
			lblAnesTechs.UserInteractionEnabled = true;
			lblAnesTechs.AddGestureRecognizer(lblAnesTechTechTap);

			lblNerveBlock.Layer.BorderColor = UIColor.Gray.CGColor;
			lblNerveBlock.Layer.BorderWidth = (nfloat)0.5;
			lblNerveBlock.Layer.CornerRadius = (nfloat)8.0; 

			UITapGestureRecognizer lblNerveBlockTechTap = new UITapGestureRecognizer(() => {
				BindPopupover(requiredNerveBlockTypes,selectedNerveBlockIds,lblNerveBlock,0,"NerveBlock");
			});
			lblNerveBlock.UserInteractionEnabled = true;
			lblNerveBlock.AddGestureRecognizer(lblNerveBlockTechTap);

			lblSpecialTechs.Layer.BorderColor = UIColor.Gray.CGColor;
			lblSpecialTechs.Layer.BorderWidth = (nfloat)0.5;
			lblSpecialTechs.Layer.CornerRadius = (nfloat)8.0; 

			UITapGestureRecognizer lblSpecialTechTap = new UITapGestureRecognizer(() => {
				BindPopupover(requiredSpecialTechTypes,selectedSpecialTechIds,lblSpecialTechs,0,"SpecialTechs");
			});
			lblSpecialTechs.UserInteractionEnabled = true;
			lblSpecialTechs.AddGestureRecognizer(lblSpecialTechTap);
		}

		private async void UpdateLineType(UISwitch switchObj,iProPQRSPortableLib.Type lineTypeObj)
		{
			List<AttribType>  procAttribTypelist=new List<AttribType>();
			AttribType typeAttrib = new AttribType();
			typeAttrib.ProcID = procedureDetails.ID;
			typeAttrib.ProcAttribTypeID = lineTypeObj.ProcAttribTypeID;
			if(switchObj.On)
				typeAttrib.Value = "1";
			else
				typeAttrib.Value = "";
			typeAttrib.IsHighLighted = false;
			procAttribTypelist.Add(typeAttrib);

			var procAttribtsobject  = await AppDelegate.Current.pqrsMgr.UpdateProcAttribs(procAttribTypelist);
			if(procAttribtsobject != null && procAttribtsobject.status != null && procAttribtsobject.status.ToUpper() == "OK"){
				UpdateProcedureStatus ();
				Console.WriteLine ("Procedure Info. Successfully Saved.");
//				new UIAlertView("Procedure Info", "Successfully Saved."
//					, null, "ok", null).Show();
			}else{
				Console.WriteLine ("Procedure Info. Save failed.");
//				new UIAlertView("Error", procAttribtsobject.message
//					, null, "ok", null).Show();
			}
		}

		private async void UpdateOtherNotes()
		{
			List<AttribType>  procAttribTypelist=new List<AttribType>();
			AttribType typeAttrib = new AttribType();
			typeAttrib.ProcID = procedureDetails.ID;
			typeAttrib.ProcAttribTypeID = 655;
			typeAttrib.Value = txtNotes.Text;
			typeAttrib.IsHighLighted = false;
			procAttribTypelist.Add(typeAttrib);

			var procAttribtsobject  = await AppDelegate.Current.pqrsMgr.UpdateProcAttribs(procAttribTypelist);
			if(procAttribtsobject != null && procAttribtsobject.status != null && procAttribtsobject.status.ToUpper() == "OK"){
				UpdateProcedureStatus ();
				Console.WriteLine ("Procedure Info. Successfully Saved.");
//				new UIAlertView("Procedure Info", "Successfully Saved."
//					, null, "ok", null).Show();
			}else{
				Console.WriteLine ("Procedure Info. Save failed.");
//				new UIAlertView("Error", procAttribtsobject.message
//					, null, "ok", null).Show();
			}
		}

		public void BindPopupover(List<iProPQRSPortableLib.Option> items,List<string> Selecteditems,UILabel lblField,int FrameY,string labelName)
		{
			int uvwidth;
			ATlist.Clear ();
			List<CodePickerModel> alist = SetTypeDataSource(items, out  uvwidth);

			mcp = new mCodePicker(this,uvwidth);
			mcp.TypeOfList = "Options";

			// need to set Selected Items
			if (Selecteditems != null && Selecteditems.Count > 0) {
				CodePickerModel sitem;	
				foreach (var item in Selecteditems) {
					if (item != "selected") {
						sitem = new CodePickerModel ();
						sitem.ItemText = alist.Where (u => u.ItemCode == item).Select (s => s.ItemText).Single ();
						sitem.ItemID = items [0].ProcAttribTypeID; //For all Options, the ProcTypeID will be same
						sitem.ItemCode = item;
						ATlist.Add (sitem);
					}
				}
			}
			if (ATlist.Count > 0)
				mcp.SelectedItems = ATlist;
			

			float x = (float)lblField.Frame.X;
			float y = (float)lblField.Frame.Y+FrameY;

			mcp.PresentFromPopover(lblField, x, y,uvwidth);
			mcp.mDataSource(alist);
			mcp._ValueChanged += async delegate
			{
				int ProcID=0;
				if(procedureDetails != null && procedureDetails.ID != 0)
					ProcID = procedureDetails.ID;
				
				if(mcp.SelectedItems.Count > 0){
					ATlist = mcp.SelectedItems;
					string finalText = " ";

					List<AttribType>  procalist=new List<AttribType>();
					AttribType procitem; 


					foreach (var item in ATlist)
					{					
						procitem = new AttribType();
						procitem.ProcAttribTypeID = item.ItemID;
						procitem.ProcID = ProcID;
						procitem.Value = item.ItemCode;
						procitem.IsHighLighted = false;
						procalist.Add(procitem);
						procitem = null;
						finalText = finalText + ", " + item.ItemText;
					}
					lblField.Text = finalText.Trim().TrimStart(',');

					foreach (var sitem in Selecteditems) {
						string spid = ATlist.Where(w=>w.ItemCode == sitem).Select(s=>s.ItemCode).SingleOrDefault();
						if(spid == null)
						{
							procitem=new AttribType();
							procitem.ProcAttribTypeID = items[0].ProcAttribTypeID;
							procitem.ProcID = ProcID;
							procitem.Value = "";
							procitem.IsHighLighted=false;
							procalist.Add(procitem);
							procitem=null;
						}
					}


					if(procalist != null && procalist.Count > 0 ){
						var procAttribtsobject  = await AppDelegate.Current.pqrsMgr.UpdateProcAttribs(procalist);
						if(procAttribtsobject != null && procAttribtsobject.status != null && procAttribtsobject.status.ToUpper() == "OK"){
							Console.WriteLine("Procedure, " +labelName + " Info. Successfully Saved.");	
	//						new UIAlertView("Procedure Info", "Successfully Saved."
	//							, null, "ok", null).Show();
						}else{
							Console.WriteLine("Procedure, " +labelName + " Info. Save failed.");	
	//						new UIAlertView("Error", procAttribtsobject.message
	//							, null, "ok", null).Show();
						}
					}
		
					if(labelName == "CVCSterileTech"){
						selectedLineCVCSterileTechIds.Clear();
						foreach (var item in ATlist) {
							selectedLineCVCSterileTechIds.Add(item.ItemCode);
						}
					}
					ATlist.Clear();
				}
				else
				{
					List<AttribType>  procalist=new List<AttribType>();
					AttribType procitem; 

					foreach (var sitem in Selecteditems) {
						string spid = ATlist.Where(w=>w.ItemCode == sitem).Select(s=>s.ItemCode).SingleOrDefault();
						if(spid == null)
						{
							procitem=new AttribType();
							procitem.ProcAttribTypeID = items[0].ProcAttribTypeID;
							procitem.ProcID = ProcID;
							procitem.Value = "";
							procitem.IsHighLighted=false;
							procalist.Add(procitem);
							procitem=null;
						}
					}

					if(procalist != null && procalist.Count > 0 ){
						var procAttribtsobject  = await AppDelegate.Current.pqrsMgr.UpdateProcAttribs(procalist);
						if(procAttribtsobject != null && procAttribtsobject.status != null && procAttribtsobject.status.ToUpper() == "OK"){
							Console.WriteLine("Procedure, " +labelName + " Info. Successfully Saved.");	
							//						new UIAlertView("Procedure Info", "Successfully Saved."
							//							, null, "ok", null).Show();
						}else{
							Console.WriteLine("Procedure, " +labelName + " Info. Save failed.");	
							//						new UIAlertView("Error", procAttribtsobject.message
							//							, null, "ok", null).Show();
						}
					}
					if(labelName == "CVCSterileTech"){
						selectedLineCVCSterileTechIds.Clear();

					}
					lblField.Text="";
				}
			};
		}
		qmCodePicker qmcp;
		public void qmBindPopupover(List<iProPQRSPortableLib.Option> items,UILabel lblField,int FrameY,int AttribTypeID)
		{
			int uvwidth;
			ATlist.Clear ();
			List<CodePickerModel> alist = SetTypeDataSource(items, out  uvwidth);

			qmcp = new qmCodePicker(this,uvwidth,alist);

			AttribType selectedattribtitem = new AttribType();
			if (iProPQRSPortableLib.Consts.SelectedProcAttribtslist == null)
				selectedattribtitem = null;
			// need to set Selected Items
			else
				selectedattribtitem = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.Where (u => u.ProcAttribTypeID == AttribTypeID).SingleOrDefault ();
			if (selectedattribtitem != null ) {
				CodePickerModel sitem;	
				if (selectedattribtitem != null ) {
					sitem = new CodePickerModel ();
					sitem = alist.Where (u => u.ItemID == selectedattribtitem.ProcAttribTypeID && u.ItemCode == selectedattribtitem.Value).SingleOrDefault ();
					if(sitem != null)
						ATlist.Add (sitem);
				}

			}
			if (ATlist.Count > 0)
				qmcp.SelectedItems = ATlist;


			float x = (float)lblField.Frame.X;
			float y = (float)lblField.Frame.Y+FrameY;

			qmcp.PresentFromPopover(lblField, x, y,uvwidth);
			//qmcp.mDataSource(alist);
			qmcp._ValueChanged += async delegate
			{
				List<AttribType>  procalist=new List<AttribType>();
				AttribType procitem; 
				int ProcID=0;
				if(procedureDetails != null && procedureDetails.ID != 0)
					ProcID = procedureDetails.ID;
				if(qmcp.SelectedItems.Count > 0){
					ATlist = qmcp.SelectedItems;
					string finalText = " ";

					if(selectedattribtitem != null)							
					{
						selectedattribtitem.Value="";
						procalist.Add(selectedattribtitem);
					}			

					foreach (var item in ATlist)
					{					
						procitem = new AttribType();
						procitem.ProcAttribTypeID = item.ItemID;
						procitem.ProcID = ProcID;
						procitem.Value = item.ItemCode;
						procitem.IsHighLighted = false;
						procalist.Add(procitem);
						procitem = null;
						finalText = finalText + ", " + item.ItemText;
					}
					lblField.Text = finalText.Trim().TrimStart(',');




					if(procalist != null && procalist.Count > 0 ){
						var procAttribtsobject  = await AppDelegate.Current.pqrsMgr.UpdateProcAttribs(procalist);
						if(procAttribtsobject != null && procAttribtsobject.status != null && procAttribtsobject.status.ToUpper() == "OK"){
							UpdateProcedureStatus ();
							Console.WriteLine("Procedure, Info. Successfully Saved.");	
							//						new UIAlertView("Procedure Info", "Successfully Saved."
							//							, null, "ok", null).Show();
						}else{
							Console.WriteLine("Procedure,  Info. Save failed.");	
							//						new UIAlertView("Error", procAttribtsobject.message
							//							, null, "ok", null).Show();
						}//
					}


					int lbldescwidth = 40;
					lblField.Lines = 1;
					int lbltextcount = lblField.Text.Length;
					if (lbltextcount < 58) {
						lbldescwidth = 40;
						lblField.Lines = 1;
					} else if (lbltextcount > 58 && lbltextcount < 114) {
						lbldescwidth = 55;
						lblField.Lines = 2;
					} else if (lbltextcount > 114) {
						lbldescwidth = 75;
						lblField.Lines = 3;
					}
					if(AttribTypeID != 602)
					 lblField.Frame = new CoreGraphics.CGRect (500, 8, 480, lbldescwidth);

				}
				else
				{
					       
					if(selectedattribtitem != null)							
					{
						selectedattribtitem.Value="";
					     procalist.Add(selectedattribtitem);
					}			



					if(procalist != null && procalist.Count > 0 ){
						var procAttribtsobject  = await AppDelegate.Current.pqrsMgr.UpdateProcAttribs(procalist);
						if(procAttribtsobject != null && procAttribtsobject.status != null && procAttribtsobject.status.ToUpper() == "OK"){
							lblField.Text = "";
							if(AttribTypeID != 602)
							 lblField.Frame = new CoreGraphics.CGRect (500, 8, 480, 40);
						}
					}
				}
				ATlist.Clear();
				iProPQRSPortableLib.Consts.SelectedProcAttribtslist  = await AppDelegate.Current.pqrsMgr.GetAllAttribTypesOfAProcedure(ProcID);
			};
		}
		public void BindPopupover(List<iProPQRSPortableLib.Option> items,List<Tuple<string,string>> Selecteditems,UILabel lblField,int FrameY,UILabel hidenval)
		{
			int uvwidth;
			ATlist.Clear ();
			List<CodePickerModel> alist = SetTypeDataSource(items, out  uvwidth);

			mcp = new mCodePicker(this,uvwidth);
			mcp.TypeOfList = "Options";

			// need to set Selected Items
			if (Selecteditems != null && Selecteditems.Count > 0) {
				CodePickerModel sitem;	
				if (Selecteditems != null && Selecteditems.Count > 0 && !string.IsNullOrEmpty(Selecteditems[0].Item1)) {
					sitem = new CodePickerModel ();
					sitem = alist.Where (u => u.ItemID == Convert.ToInt16 (Selecteditems[0].Item1) && u.ItemCode == Selecteditems[0].Item2).SingleOrDefault ();
					if(sitem != null)
					  ATlist.Add (sitem);
				}

			}
			if (ATlist.Count > 0)
				mcp.SelectedItems = ATlist;


			float x = (float)lblField.Frame.X;
			float y = (float)lblField.Frame.Y+FrameY;

			mcp.PresentFromPopover(lblField, x, y,uvwidth);
			mcp.mDataSource(alist);
			mcp._ValueChanged += async delegate
			{
				List<AttribType>  procalist=new List<AttribType>();
				AttribType procitem; 
				int ProcID=0;
				if(procedureDetails != null && procedureDetails.ID != 0)
					ProcID = procedureDetails.ID;
				
				if(mcp.SelectedItems.Count > 0){
					ATlist = mcp.SelectedItems;
					string finalText = " ";				

					foreach (var sitem in Selecteditems) {
						if(!string.IsNullOrEmpty(sitem.Item1) && !string.IsNullOrEmpty(sitem.Item2))
						{
							string spid = ATlist.Where( w => w.ItemID == Convert.ToInt16(sitem.Item1) && w.ItemCode == sitem.Item2).Select(s=>s.ItemCode).SingleOrDefault();
							if(spid == null)
							{
								procitem=new AttribType();
								procitem.ProcAttribTypeID = Convert.ToInt16(sitem.Item1);
								procitem.ProcID = ProcID;
								procitem.Value = "";
								procitem.IsHighLighted=false;
								procalist.Add(procitem);
								procitem=null;
							}
						}
					}

					foreach (var item in ATlist)
					{					
						procitem = new AttribType();
						procitem.ProcAttribTypeID = item.ItemID;
						procitem.ProcID = ProcID;
						procitem.Value = item.ItemCode;
						procitem.IsHighLighted = false;
						procalist.Add(procitem);
						procitem = null;
						finalText = finalText + ", " + item.ItemText;
					}
					lblField.Text = finalText.Trim().TrimStart(',');




					if(procalist != null && procalist.Count > 0 ){
						var procAttribtsobject  = await AppDelegate.Current.pqrsMgr.UpdateProcAttribs(procalist);
						if(procAttribtsobject != null && procAttribtsobject.status != null && procAttribtsobject.status.ToUpper() == "OK"){
							Console.WriteLine("Procedure, Info. Successfully Saved.");	
							//						new UIAlertView("Procedure Info", "Successfully Saved."
							//							, null, "ok", null).Show();
						}else{
							Console.WriteLine("Procedure,  Info. Save failed.");	
							//						new UIAlertView("Error", procAttribtsobject.message
							//							, null, "ok", null).Show();
						}//
					}
					foreach (var item in ATlist) {
						hidenval.Tag=item.ItemID;
						hidenval.Text=item.ItemCode;
					}
					ATlist.Clear();

					int lbldescwidth = 40;
					lblField.Lines = 1;
					int lbltextcount = lblField.Text.Length;
					if (lbltextcount < 58) {
						lbldescwidth = 40;
						lblField.Lines = 1;
					} else if (lbltextcount > 58 && lbltextcount < 114) {
						lbldescwidth = 55;
						lblField.Lines = 2;
					} else if (lbltextcount > 114) {
						lbldescwidth = 75;
						lblField.Lines = 3;
					}

					lblField.Frame = new CoreGraphics.CGRect (500, 8, 480, lbldescwidth);

				}

			};
		}
		public void BindmultilevelPopupover(List<iProPQRSPortableLib.Option> masterMainList,List<iProPQRSPortableLib.Option> masterSubCatList,UILabel btnField,int FrameY,UILabel hidenval,UILabel lblhidensubcatval,bool isMultiSelect,int TypeItemID,string TypeValue)
		{
			int uvwidth;
			int vw;
			ATlist.Clear ();
			SelectedSubcodeitems=new List<CodePickerModel>();
			List<CodePickerModel> alist = SetTypeDataSource(masterMainList, out  uvwidth);
			List<CodePickerModel> SubRootList  = SetTypeDataSource(masterSubCatList, out  vw);
			uvwidth = 800;
			var	selectedMainListIDs = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (masterMainList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).ToList ();
		
			if (selectedMainListIDs != null && selectedMainListIDs.Count > 0) {
				//ATlist = = SetTypeDataSource(masterSubCatList, out  uvwidth);
				CodePickerModel sitem;	
				foreach (var smid in selectedMainListIDs) {
					var desclist=masterMainList.Where(u=>u.ProcAttribTypeID==smid.ProcAttribTypeID && u.Value == smid.Value).ToList();
					sitem = new CodePickerModel ();
					sitem.ItemCode = smid.Value;
					sitem.ItemID = smid.ProcAttribTypeID;
					if (desclist.Count > 0)
						sitem.ItemText = desclist [0].Description;
					ATlist.Add (sitem);					
		
				}
		
			}


			var selectedSubCatListIDs = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll(u => (masterSubCatList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).ToList();

			if (selectedSubCatListIDs != null && selectedSubCatListIDs.Count > 0) {
				CodePickerModel sitem;	
			
				foreach (var smid in selectedSubCatListIDs) {
					var desclist=masterSubCatList.Where(u=>u.ProcAttribTypeID==smid.ProcAttribTypeID && u.Value == smid.Value).ToList();
					sitem = new CodePickerModel ();
					sitem.ItemCode = smid.Value;
					sitem.ItemID = smid.ProcAttribTypeID;
					if (desclist.Count > 0)
						sitem.ItemText = desclist [0].Description;
					
			
					SelectedSubcodeitems.Add (sitem);					
				}
			
			}
			//string TypeValue = "0582F";
			if (alist.Count > 0 && SelectedSubcodeitems.Count > 0) {
				//"0582F"
				List<CodePickerModel> changeitems=new List<CodePickerModel>();
				foreach (var Maincatitem in alist) {
					//"0582F"
					if (Maincatitem.ItemCode == TypeValue) {
						TypeValue = Maincatitem.ItemCode;
						string finaltext =Maincatitem.ItemText+ " (";
						foreach (var item in SelectedSubcodeitems) {
							finaltext = finaltext + item.ItemText + ",";
						}
						finaltext = finaltext.TrimEnd(',') + " )";
						Maincatitem.ItemText = finaltext;
						changeitems.Add(Maincatitem);
				
					}
				}
				foreach (var item in changeitems) {
					var changeitem = alist.Where (u => u.ItemCode == item.ItemCode).ToList();
					alist.Remove (changeitem[0]);
					alist.Add (changeitem [0]);
				}

			}
			//FullDescription

			mlcp = new mlsCodePicker(this,uvwidth,alist,SubRootList);
			mlcp.SelectedItems = ATlist;
			if(SelectedSubcodeitems.Count > 0)
				mlcp.SelectedSubItems =    SelectedSubcodeitems;

			float xcord = 10;//(float)btnField.Frame.X;
			float ycord = 10;//(float)btnField.Frame.Y+FrameY;
			mlcp.TypeItemID=TypeItemID;//alist[0].ItemID;
			mlcp.TypeValue=TypeValue;
			mlcp.isMultiSelect = isMultiSelect;
			mlcp.PresentFromPopover(btnField, xcord, ycord);

			//SubRootData
			mlcp._ValueChanged += async delegate
			{
				ATlist = mlcp.SelectedItems;
				if(ATlist[0].ItemID ==TypeItemID && ATlist[0].ItemCode=="0581F" || ATlist[0].ItemCode=="A")
					mlcp.SelectedSubItems.Clear();
				SelectedSubcodeitems=mlcp.SelectedSubItems;
				string finalText = " ";

				List<AttribType>  procalist=new List<AttribType>();
				AttribType procitem; 
				int ProcID=0;
				if(procedureDetails != null && procedureDetails.ID != 0)
					ProcID = procedureDetails.ID;

				foreach (var item in ATlist)
				{					
					procitem = new AttribType();
					procitem.ProcAttribTypeID = item.ItemID;
					procitem.ProcID = ProcID;
					procitem.Value = item.ItemCode;
					procitem.IsHighLighted = false;
					procalist.Add(procitem);
					procitem = null;
					finalText = item.ItemText;
				}
				btnField.Text=finalText.Trim();
				foreach (var sitem in masterMainList) {
					string spid = ATlist.Where(w=>w.ItemID == sitem.ProcAttribTypeID).Select(s=>s.ItemCode).SingleOrDefault();
					if(spid == string.Empty)
					{
						procitem=new AttribType();
						procitem.ProcAttribTypeID = masterMainList[0].ProcAttribTypeID;
						procitem.ProcID = ProcID;
						procitem.Value = "";
						procitem.IsHighLighted=false;
						procalist.Add(procitem);
						procitem=null;
					}
				}
				//SelectedSubitems.Clear();
				string subtitles=string.Empty;
				if(SelectedSubcodeitems.Count > 0)
				{
					subtitles=" (";
					foreach (var sitem in SelectedSubcodeitems)
					{					
						procitem = new AttribType();
						procitem.ProcAttribTypeID = sitem.ItemID;
						procitem.ProcID = ProcID;
						procitem.Value = sitem.ItemCode;
						procitem.IsHighLighted = false;
						procalist.Add(procitem);
						procitem = null;
						subtitles=subtitles+sitem.ItemText +",";

					}
					subtitles=subtitles.TrimEnd(',')+")";
					//0583F
					if( mlcp.TypeValue != "0581F")
					   btnField.Text=btnField.Text+subtitles;
				}

				List<int> removeid=new List<int>(); 
				foreach (var rsitem in selectedSubCatListIDs) {
					var removeitem = mlcp.SelectedSubItems.Where(w=>w.ItemID == rsitem.ProcAttribTypeID).ToList();
					if(removeitem == null || removeitem.Count == 0)
					{
						procitem=new AttribType();
						procitem.ProcAttribTypeID = rsitem.ProcAttribTypeID;
						procitem.ProcID = ProcID;
						procitem.Value = "";
						procitem.IsHighLighted=false;
						procalist.Add(procitem);
						procitem=null;
						removeid.Add(rsitem.ProcAttribTypeID);
					}
				}


				selectedSubCatListIDs.Clear();

				if(procalist != null && procalist.Count > 0 ){
					var procAttribtsobject  = await AppDelegate.Current.pqrsMgr.UpdateProcAttribs(procalist);
					if(procAttribtsobject != null && procAttribtsobject.status != null && procAttribtsobject.status.ToUpper() == "OK"){
						Console.WriteLine ("Patient Info. Successfully Saved.");
						//						new UIAlertView("Procedure Info", "Successfully Saved.", null, "ok", null).Show();
					}else{
						new UIAlertView("Error", procAttribtsobject.message, null, "ok", null).Show();
					}
				}


				foreach (var item in ATlist) {
					hidenval.Tag=item.ItemID;
					hidenval.Text=item.ItemCode;
				}
				ATlist.Clear();
				iProPQRSPortableLib.Consts.SelectedProcAttribtslist  = await AppDelegate.Current.pqrsMgr.GetAllAttribTypesOfAProcedure(ProcID);
			};
		}
		List<CodePickerModel> SelectedSubcodeitems=null;//new List<CodePickerModel>();
		public void BindmultilevelPopupover(List<iProPQRSPortableLib.Option> items,List<Tuple<string,string>> Selecteditems, List<int> SelectedSubitems,UILabel btnField,int FrameY,UILabel hidenval,bool isMultiSelect)
		{
			int uvwidth;
			int vw;
			ATlist.Clear ();
			SelectedSubcodeitems=new List<CodePickerModel>();
			List<CodePickerModel> alist = SetTypeDataSource(items, out  uvwidth);
			List<CodePickerModel> SubRootList  = SetTypeDataSource(nonPqrsTypeAS10Options, out  uvwidth);
			mlcp = new mlsCodePicker(this,uvwidth,alist,SubRootList);
			uvwidth = 700;
			if (Selecteditems != null && Selecteditems.Count > 0) {
				CodePickerModel sitem;	
				if (Selecteditems != null && Selecteditems.Count > 0 && !string.IsNullOrEmpty(Selecteditems[0].Item1)) {
					sitem = new CodePickerModel ();
					sitem = alist.Where (u => u.ItemID == Convert.ToInt16 (Selecteditems[0].Item1) && u.ItemCode == Selecteditems[0].Item2).SingleOrDefault ();
					ATlist.Add (sitem);
				}

			}


			if (SelectedSubitems != null && SelectedSubitems.Count > 0) {
				//CodePickerModel sitem;	
				foreach (var subitem in SelectedSubitems) {					
						//sitem = new CodePickerModel ();
					var  sitem= SubRootList.Where (u => u.ItemID == subitem).ToList();				 
					if(sitem.Count >0 )
						SelectedSubcodeitems.Add (sitem[0]);
				}
			}
			if (ATlist.Count > 0) {
				string finaltext = "Serious adverse event (";
				if (ATlist [0]!=null && ATlist [0].ItemID == 608 && ATlist [0].ItemCode == "B" && SelectedSubcodeitems.Count > 0) {
					foreach (var item in SelectedSubcodeitems) {
						finaltext = finaltext + item.ItemText + ",";
					}
					finaltext = finaltext.TrimEnd(',') + " )";
					ATlist [0].ItemText = finaltext;
				}
				mlcp.SelectedItems = ATlist;
			}
			
			//if(SelectedSubcodeitems.Count > 0)
			mlcp.SelectedSubItems =    SelectedSubcodeitems;

			float x = 10;//(float)btnField.Frame.X;
			float y = 10;//(float)btnField.Frame.Y+FrameY;
			mlcp.TypeItemID=608;
			mlcp.TypeValue="B"; 
			mlcp.isMultiSelect = isMultiSelect;
			mlcp.PresentFromPopover(btnField, x, y);

			//SubRootData
			mlcp._ValueChanged += async delegate
			{
				ATlist = mlcp.SelectedItems;
				SelectedSubcodeitems=mlcp.SelectedSubItems;
				string finalText = " ";

				List<AttribType>  procalist=new List<AttribType>();
				AttribType procitem; 
				int ProcID=0;
				if(procedureDetails != null && procedureDetails.ID != 0)
					ProcID = procedureDetails.ID;

				foreach (var item in ATlist)
				{					
					procitem = new AttribType();
					procitem.ProcAttribTypeID = item.ItemID;
					procitem.ProcID = ProcID;
					procitem.Value = item.ItemCode;
					procitem.IsHighLighted = false;
					procalist.Add(procitem);
					procitem = null;
					finalText = item.ItemText;
				}
				btnField.Text=finalText.Trim();
				foreach (var sitem in Selecteditems) {
					string spid = ATlist.Where(w=>w.ItemCode == sitem.Item1).Select(s=>s.ItemCode).SingleOrDefault();
					if(spid == string.Empty)
					{
						procitem=new AttribType();
						procitem.ProcAttribTypeID = items[0].ProcAttribTypeID;
						procitem.ProcID = ProcID;
						procitem.Value = "";
						procitem.IsHighLighted=false;
						procalist.Add(procitem);
						procitem=null;
					}
				}
				//SelectedSubitems.Clear();
				foreach (var sitem in SelectedSubcodeitems)
				{					
					procitem = new AttribType();
					procitem.ProcAttribTypeID = sitem.ItemID;
					procitem.ProcID = ProcID;
					procitem.Value = sitem.ItemText;
					procitem.IsHighLighted = false;
					procalist.Add(procitem);
					procitem = null;

				}
				List<int> removeid=new List<int>(); 
				foreach (var rsitem in SelectedSubitems) {
					var removeitem = mlcp.SelectedSubItems.Where(w=>w.ItemID == rsitem).ToList();
					if(removeitem == null || removeitem.Count == 0)
					{
						procitem=new AttribType();
						procitem.ProcAttribTypeID = rsitem;
						procitem.ProcID = ProcID;
						procitem.Value = "";
						procitem.IsHighLighted=false;
						procalist.Add(procitem);
						procitem=null;
						removeid.Add(rsitem);
					}
				}

				foreach (var ri in removeid) {
					SelectedSubitems.Remove(SelectedSubitems.Where(u=>u==ri).SingleOrDefault());
					mlcp.SelectedSubItems.Remove (mlcp.SelectedSubItems.Where(u=>u.ItemID==ri).SingleOrDefault());
				}
				//SelectedSubitems.Clear();

				if(procalist != null && procalist.Count > 0 ){
					var procAttribtsobject  = await AppDelegate.Current.pqrsMgr.UpdateProcAttribs(procalist);
					if(procAttribtsobject != null && procAttribtsobject.status != null && procAttribtsobject.status.ToUpper() == "OK"){
						Console.WriteLine ("Patient Info. Successfully Saved.");
						//						new UIAlertView("Procedure Info", "Successfully Saved.", null, "ok", null).Show();
					}else{
						new UIAlertView("Error", procAttribtsobject.message, null, "ok", null).Show();
					}
				}


					foreach (var item in ATlist) {
					hidenval.Tag=item.ItemID;
					hidenval.Text=item.ItemCode;
				 }
				ATlist.Clear();
				iProPQRSPortableLib.Consts.SelectedProcAttribtslist  = await AppDelegate.Current.pqrsMgr.GetAllAttribTypesOfAProcedure(ProcID);
				selectednonPqrsTypeAS10OptionsIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll(u => (nonPqrsTypeAS10Options.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).Select(xx=>xx.ProcAttribTypeID).ToList();

			};
		}
		List<string> maccode=new List<string>();
		public void BindTextToLabel(List<int> itemlist, UILabel lblfield)
		{
			string finaltxt = " ";
			if (itemlist != null && itemlist.Count > 0) {
				itemlist.Sort ();
				foreach (var itemid in itemlist) {
					if (itemid == 406) {
						string selectedMACCodes = string.Empty;
						foreach (var item in patientProfile.MACCodesList) {
							if (!maccode.Contains (item.Code)) {
								selectedMACCodes = selectedMACCodes + item.Code + ",";
								maccode.Add (item.Code);
							}
						}
						if (!string.IsNullOrEmpty (selectedMACCodes))
							finaltxt = finaltxt + "MAC(" + selectedMACCodes.TrimEnd (',') + "),";
						else
							finaltxt = finaltxt + "MAC";
						
					} else {
						finaltxt = finaltxt + iProPQRSPortableLib.Consts.ProcAttribTypes.result.Types.Where (u => u.ProcAttribTypeID == itemid).Select (s => s.Label).SingleOrDefault ()+" ,";	
					}
				}
				lblfield.Text = finaltxt.TrimEnd (',').TrimStart(',');
			}
		}
		List<MACCodesDetails> masterlistOfMacCodes;
		List<string> checkmaccode=new List<string>();
		public async void BindmultilevelPopupover(List<iProPQRSPortableLib.Type> items,List<int> Selecteditems,List<int> SelectedSubitems,UILabel lblField,int FrameY,string labelName)
		{
			AppDelegate.pb.Start(this.View,"PLease wait");


			items.Find (u => u.ProcAttribTypeID == 406).Label = "MAC";
			if(labelName == "AnesthesiaTechs"){
				//iProPQRSPortableLib.Consts.RcvdJSONDataResult =await AppDelegate.Current.pqrsMgr.GetMACCodes();
				RcvdJSONData macCodes = iProPQRSPortableLib.Consts.RcvdJSONDataResult;
				if (macCodes != null && macCodes.results != null) {
					masterlistOfMacCodes = (List<MACCodesDetails>)JsonConvert.DeserializeObject (macCodes.results.ToString (), typeof(List<MACCodesDetails>));
				}
			}

			int uvwidth;
			ATlist.Clear();

			//patientProfile
			List<CodePickerModel> mainsubCatList=new List<CodePickerModel>(); //patientProfile.MACCodesList
			if (masterlistOfMacCodes != null) {
				foreach (var scitem in masterlistOfMacCodes) {
					CodePickerModel sc = new CodePickerModel ();
					sc.ItemCode = scitem.Code;
					sc.ItemID = 0;
					sc.ItemText = scitem.Name;
					mainsubCatList.Add (sc);
					sc = null;
				}
			}

			// need to set Selected Items
			if (Selecteditems != null && Selecteditems.Count > 0) {
				CodePickerModel sitem;	
				foreach (var item in Selecteditems) {
					sitem = new CodePickerModel ();
					sitem.ItemText = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Types.Where(u=>u.ProcAttribTypeID==item).Select(s=>s.Label).Single();
					sitem.ItemID = item;
					ATlist.Add (sitem);
				}
			}

			//
			string subcattext=string.Empty;
			SelectedSubcodeitems = new List<CodePickerModel> ();
			checkmaccode.Clear ();
			if (SelectedSubitems != null && SelectedSubitems.Count > 0 && patientProfile.MACCodesList != null) {
				subcattext = string.Empty;
				selectedsubAnesTechTypesIds.Clear ();
				CodePickerModel sitem;	
				foreach (var item in patientProfile.MACCodesList) {
					selectedsubAnesTechTypesIds.Add (item.ProcCodeID.ToString ());
					if (!checkmaccode.Contains (item.Code)) {
						CodePickerModel sc = new CodePickerModel ();

						sc.ItemCode = item.Code;
						sc.ItemID = item.ProcCodeID;
						sc.ItemText = item.Name;
						SelectedSubcodeitems.Add (sc);
						subcattext = subcattext + item.Code + ",";
						checkmaccode.Add (item.Code);
						sc = null;
					}

				}
				if(!string.IsNullOrEmpty(subcattext))					
					subcattext=" ("+subcattext.TrimEnd(',')+" )";
				
				iProPQRSPortableLib.Type typeitem = items.Where (u => u.ProcAttribTypeID == 406).SingleOrDefault ();
				items.Remove (typeitem);

				typeitem.Label ="MAC " + subcattext;
				items.Add (typeitem);
				items.Sort((xx,yy)=> xx.ProcAttribTypeID.CompareTo(yy.ProcAttribTypeID));
			}

			List<CodePickerModel> alist = SetTypeDataSource(items, out  uvwidth);
			uvwidth = 600;
			mlcp = new mlsCodePicker(this,uvwidth,alist,mainsubCatList);
			uvwidth = 600;
			if (ATlist.Count > 0)
				mlcp.SelectedItems = ATlist;
			
			mlcp.SelectedSubItems = SelectedSubcodeitems;

			float x = (float)lblField.Frame.X;
			float y = (float)lblField.Frame.Y+FrameY;
			mlcp.TypeItemID = 406;
			mlcp.TypeValue = null;
			mlcp.isMainCatMultiSelect = true;
			mlcp.isMultiSelect = true;
			mlcp.PresentFromPopover(lblField, 10, y);
			AppDelegate.pb.Stop ();
			mlcp._ValueChanged += async delegate
			{
				int ProcID=0;
				if(procedureDetails != null && procedureDetails.ID != 0)
					ProcID = procedureDetails.ID;
				
				AppDelegate.pb.Start(this.View,"PLease wait");
				if(mlcp.SelectedItems.Count > 0){
					ATlist = mlcp.SelectedItems;
					SelectedSubcodeitems=mlcp.SelectedSubItems;
					string finalText = string.Empty;
					List<AttribType>  procalist=new List<AttribType>();
					AttribType procitem; 


					string txtmac=string.Empty;
					foreach (var item in ATlist){
						
							procitem=new AttribType();
							procitem.ProcAttribTypeID=item.ItemID;
							procitem.ProcID=ProcID;
							procitem.Value = "1";
							procitem.IsHighLighted=false;
							procalist.Add(procitem);
							procitem=null;
						  if(item.ItemID != 406)						   
							finalText = finalText + ", " + item.ItemText;						
						  else
							txtmac="MAC";
					}

					//				SetTextboxValue(mCurrentTextBoxID, finalText.TrimStart(','));
					lblField.Text = finalText.TrimStart(',').TrimEnd(',');

					foreach (var sitem in Selecteditems) {
						
						List<int> spid = ATlist.Where(w=>w.ItemID == sitem).Select(s=>s.ItemID).ToList();
						if(spid!=null && spid.Count == 0 )
						{
							procitem=new AttribType();
							procitem.ProcAttribTypeID=sitem;
							procitem.ProcID=ProcID;
							procitem.Value="";
							procitem.IsHighLighted=false;
							procalist.Add(procitem);
							procitem=null;
						}
					}


					if(procalist != null && procalist.Count > 0 ){
						var procAttribtsobject  = await AppDelegate.Current.pqrsMgr.UpdateProcAttribs(procalist);
						if(procAttribtsobject != null && procAttribtsobject.status != null && procAttribtsobject.status.ToUpper() == "OK")
						{
							List<int> Subids=new List<int>();
							if(labelName=="AnesthesiaTechs"){
								//var  SelectedSubcode=SelectedSubcodeitems.Distinct();
								foreach (var id in selectedsubAnesTechTypesIds) {
									int proccodeid=Convert.ToInt16(id);
									if(proccodeid!=0)
									{
										ReceiveContext deletecontext=await AppDelegate.Current.pqrsMgr.DeleteProcedureDiagnostic(proccodeid);
										if (deletecontext != null && deletecontext.status != null && deletecontext.status.ToUpper () == "OK")
										{
											
										}

									}
								}
								checkmaccode.Clear();
								if(txtmac !=null && txtmac != "")
								{
										foreach (var subitem in SelectedSubcodeitems){										
											checkmaccode.Add(subitem.ItemCode);
											//finalText = finalText + ", " + subitem.ItemCode;
											UpdateProcedureDiagnostic(0,subitem.ItemCode,subitem.ItemText,418);
											selectedsubAnesTechTypesIds.Add(subitem.ItemID.ToString());

											}
								}
								if(!string.IsNullOrEmpty(txtmac) && checkmaccode.Count > 0)
								{
									finalText=finalText.TrimEnd(',').TrimStart(',') +", MAC (" +String.Join(", ", checkmaccode.ToArray())+")";
									lblField.Text=finalText;
								}
								else if(!string.IsNullOrEmpty(txtmac))
								{
									lblField.Text=finalText.TrimEnd(',').TrimStart(',') +" ,MAC";
								}

							


								//lblField.Text = finalText.TrimStart(',');

							}
							Console.WriteLine("Procedure Info. Successfully Saved.");	
							//						new UIAlertView("Procedure Info", "Successfully Saved.", null, "ok", null).Show();
						}else{
							NavigationController.PopToRootViewController(true);
							Console.WriteLine("Procedure Info. Save failed.");	
							//						new UIAlertView("Error", procAttribtsobject.message, null, "ok", null).Show();
						}
					}

					if(labelName=="AnesthesiaTechs"){
						selectedAnesTechTypesIds.Clear();
						foreach (var item in ATlist) {
							selectedAnesTechTypesIds.Add(item.ItemID);
						}
						//selectedsubAnesTechTypesIds
						//selectedsubAnesTechTypesIds.Clear();
						//foreach (var item in SelectedSubcodeitems) {
							//selectedsubAnesTechTypesIds.Add(item.ItemID.ToString());
						//}
					}else if(labelName=="NerveBlock"){
						selectedNerveBlockIds.Clear();
						foreach (var item in ATlist) {
							selectedNerveBlockIds.Add(item.ItemID);
						}
					}else if(labelName=="SpecialTechs"){
						selectedSpecialTechIds.Clear();
						foreach (var item in ATlist) {
							selectedSpecialTechIds.Add(item.ItemID);
						}
					}


					ATlist.Clear();

				}
				else
				{
					lblField.Text="";
					List<AttribType>  procalist=new List<AttribType>();
					AttribType procitem; 
					foreach (var sitem in Selecteditems) {

						List<int> spid = ATlist.Where(w=>w.ItemID == sitem).Select(s=>s.ItemID).ToList();
						if(spid!=null && spid.Count == 0 )
						{
							procitem=new AttribType();
							procitem.ProcAttribTypeID=sitem;
							procitem.ProcID=ProcID;
							procitem.Value="";
							procitem.IsHighLighted=false;
							procalist.Add(procitem);
							procitem=null;
						}
					}
						if(procalist != null && procalist.Count > 0 ){
							var procAttribtsobject  = await AppDelegate.Current.pqrsMgr.UpdateProcAttribs(procalist);
							if(procAttribtsobject != null && procAttribtsobject.status != null && procAttribtsobject.status.ToUpper() == "OK")
							{
								List<int> Subids=new List<int>();
								if(labelName=="AnesthesiaTechs"){
									//var  SelectedSubcode=SelectedSubcodeitems.Distinct();
									foreach (var id in selectedsubAnesTechTypesIds) {
										int proccodeid=Convert.ToInt16(id);
										if(proccodeid!=0)
										{
											ReceiveContext deletecontext=await AppDelegate.Current.pqrsMgr.DeleteProcedureDiagnostic(proccodeid);
											if (deletecontext != null && deletecontext.status != null && deletecontext.status.ToUpper () == "OK")
											{

											}

										}
									}
								}
							}
					   }
					selectedAnesTechTypesIds.Clear();
					if(patientProfile.MACCodesList!=null)
					patientProfile.MACCodesList.Clear();
				}
				iProPQRSPortableLib.Consts.SelectedProcAttribtslist  = await AppDelegate.Current.pqrsMgr.GetAllAttribTypesOfAProcedure(ProcID);
				var rootobject = await AppDelegate.Current.pqrsMgr.GetProcedureDiagnosticMaster(ProcID);
				patientProfile.MACCodesList = rootobject.result.FindAll(u=> u.ProcCodeTypeID == 418);
				AppDelegate.pb.Stop ();
				lblField.Text=lblField.Text.TrimStart(',').TrimEnd(',');
				UpdateProcedureStatus ();
			};
		}
		public void BindPopupover(List<iProPQRSPortableLib.Type> items,List<int> Selecteditems,UILabel lblField,int FrameY,string labelName)
		{
			int uvwidth;
			ATlist.Clear();
			items.Sort((xx,yy)=> xx.Label.CompareTo(yy.Label));
			List<CodePickerModel> alist = SetTypeDataSource(items, out  uvwidth);
			mcp = new mCodePicker(this,uvwidth,alist);
			// need to set Selected Items
			if (Selecteditems != null && Selecteditems.Count > 0) {
				CodePickerModel sitem;	
				foreach (var item in Selecteditems) {
					sitem = new CodePickerModel ();
					sitem.ItemText = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Types.Where(u=>u.ProcAttribTypeID==item).Select(s=>s.Label).Single();
					sitem.ItemID = item;
					ATlist.Add (sitem);
				}
			}

			if (ATlist.Count > 0)
				mcp.SelectedItems = ATlist;
			//

			float x = (float)lblField.Frame.X;
			float y = (float)lblField.Frame.Y+FrameY;

			mcp.PresentFromPopover(lblField, x, y,uvwidth);
			mcp.mDataSource(alist);
			mcp._ValueChanged += async delegate
			{
				int ProcID=0;
				if(procedureDetails != null && procedureDetails.ID != 0)
					ProcID = procedureDetails.ID;
				
				if(mcp.SelectedItems.Count > 0){
					ATlist = mcp.SelectedItems;
					string finalText = string.Empty;
					List<AttribType>  procalist=new List<AttribType>();
					AttribType procitem; 


					foreach (var item in ATlist){					
						procitem=new AttribType();
						procitem.ProcAttribTypeID=item.ItemID;
						procitem.ProcID=ProcID;
						procitem.Value = "1";
						procitem.IsHighLighted=false;
						procalist.Add(procitem);
						procitem=null;
						finalText = finalText + "," + item.ItemText;
					}
					//				SetTextboxValue(mCurrentTextBoxID, finalText.TrimStart(','));
					lblField.Text = finalText.TrimStart(',');
					int mw=MeasureTextLine(finalText);
					int maxheight=30;
					if(mw>500)
					{
						maxheight=60;
						lblField.Lines=2;
					}
					else if(mw> 1000)
					{
						maxheight=90;
						lblField.Lines=2;
					}
					CoreGraphics.CGRect fram=lblField.Frame;
					fram.Height=maxheight;
					lblField.Frame=fram;
					foreach (var sitem in Selecteditems) {
						int spid = ATlist.Where(w=>w.ItemID == sitem).Select(s=>s.ItemID).SingleOrDefault();
						if(spid==0)
						{
							procitem=new AttribType();
							procitem.ProcAttribTypeID=sitem;
							procitem.ProcID=ProcID;
							procitem.Value="";
							procitem.IsHighLighted=false;
							procalist.Add(procitem);
							procitem=null;
						}
					}

					if(procalist != null && procalist.Count > 0 ){
						var procAttribtsobject  = await AppDelegate.Current.pqrsMgr.UpdateProcAttribs(procalist);
						if(procAttribtsobject != null && procAttribtsobject.status != null && procAttribtsobject.status.ToUpper() == "OK")
						{
							Console.WriteLine("Procedure Info. Successfully Saved.");	
	//						new UIAlertView("Procedure Info", "Successfully Saved.", null, "ok", null).Show();
						}else{
							NavigationController.PopToRootViewController(true);
							Console.WriteLine("Procedure Info. Save failed.");	
	//						new UIAlertView("Error", procAttribtsobject.message, null, "ok", null).Show();
						}
					}

					if(labelName=="AnesthesiaTechs"){
						selectedAnesTechTypesIds.Clear();
						foreach (var item in ATlist) {
							selectedAnesTechTypesIds.Add(item.ItemID);
						}
					}else if(labelName=="NerveBlock"){
						selectedNerveBlockIds.Clear();
						foreach (var item in ATlist) {
							selectedNerveBlockIds.Add(item.ItemID);
						}
					}else if(labelName=="SpecialTechs"){
						selectedSpecialTechIds.Clear();
						foreach (var item in ATlist) {
							selectedSpecialTechIds.Add(item.ItemID);
						}
					}

					ATlist.Clear();
				}
				else
				{
					List<AttribType>  procalist=new List<AttribType>();
					AttribType procitem; 

					foreach (var sitem in Selecteditems) {
						int spid = ATlist.Where(w=>w.ItemID == sitem).Select(s=>s.ItemID).SingleOrDefault();
						if(spid==0)
						{
							procitem=new AttribType();
							procitem.ProcAttribTypeID=sitem;
							procitem.ProcID=ProcID;
							procitem.Value="";
							procitem.IsHighLighted=false;
							procalist.Add(procitem);
							procitem=null;
						}
					}

					if(procalist != null && procalist.Count > 0 ){
						var procAttribtsobject  = await AppDelegate.Current.pqrsMgr.UpdateProcAttribs(procalist);
						if(procAttribtsobject != null && procAttribtsobject.status != null && procAttribtsobject.status.ToUpper() == "OK")
						{
							Console.WriteLine("Procedure Info. Successfully Saved.");	
							//						new UIAlertView("Procedure Info", "Successfully Saved.", null, "ok", null).Show();
						}else{
							NavigationController.PopToRootViewController(true);
							Console.WriteLine("Procedure Info. Save failed.");	
							//						new UIAlertView("Error", procAttribtsobject.message, null, "ok", null).Show();
						}
					}
					if(labelName=="SpecialTechs")
						selectedSpecialTechIds.Clear();
					else if(labelName=="SpecialTechs")
						selectedSpecialTechIds.Clear();
					else if(labelName=="NerveBlock")
						selectedNerveBlockIds.Clear();
					
					lblField.Text="";
				}
				UpdateProcedureStatus ();
			};
		}


		public List<CodePickerModel> SetTypeDataSource(List<iProPQRSPortableLib.Option> tList,out int wvalue)
		{
			wvalue=0;
			CodePickerModel item;
			int pcharcount = 0;
			List<CodePickerModel> dlist = new List<CodePickerModel> ();
			foreach (iProPQRSPortableLib.Option titem in tList) {
				item = new CodePickerModel ();
				item.ItemID = titem.ProcAttribTypeID;
				item.ItemText = titem.Description;
				item.ItemCode = titem.Value;
				dlist.Add (item);
				if (pcharcount < item.ItemText.Length) {
					pcharcount = item.ItemText.Length;
					wvalue = MeasureTextSize (item.ItemText);
				}
				item = null;
			}
			return dlist;
		}

		public List<CodePickerModel> SetTypeDataSource(List<iProPQRSPortableLib.Type> tList,out int wvalue)
		{
			wvalue=0;
			CodePickerModel item;
			int pcharcount = 0;
			List<CodePickerModel> dlist = new List<CodePickerModel> ();
			foreach (iProPQRSPortableLib.Type titem in tList) {
				item = new CodePickerModel ();
				item.ItemID = titem.ProcAttribTypeID;
				item.ItemText = titem.Label;
				dlist.Add (item);
				if (pcharcount < item.ItemText.Length) {
					pcharcount = item.ItemText.Length;
					wvalue = MeasureTextSize (item.ItemText);
				}
				item = null;
			}
			return dlist;
		}

		public void BindParticipantsPopupover(List<UserDetails> listOfUsers,UIButton btnField,int FrameY,ProcedureParticipantDetails participantDetails)
		{
			
			if(btnField.CurrentTitle != "" && btnField.CurrentTitle != null){
				RemovePreviousParticipant(btnField,participantDetails.RoleID);
			}

			int uvwidth;
			ATlist.Clear();
			List<CodePickerModel> alist = SetParticipantsDataSource(listOfUsers, out  uvwidth);
			mcp = new mCodePicker(this,uvwidth,alist);
			mcp.TypeOfList = "Participants";

			float x = (float)btnField.Frame.X;
			float y = (float)btnField.Frame.Y+FrameY;

			mcp.PresentFromPopover(btnField, x, y,uvwidth);
			mcp.mDataSource(alist);
			mcp._ValueChanged += async delegate
			{
				try
				{
				if(mcp.SelectedItems.Count > 0){
					ATlist = mcp.SelectedItems;
					btnField.SetTitle(" " + ATlist[0].ItemText,UIControlState.Normal);
					participantDetails.UserID = ATlist[0].ItemID;
					ATlist.Clear();
					if(participantDetails.ProcParticipantID == 0)
					{
						participantDetails.ProcParticipantID = 0;
						participantDetails.Name = btnField.CurrentTitle.Trim();
						AddUpdateProcedureParticipants(participantDetails,btnField);
					}
					else
					{
						UpdateProcedureParticipants(participantDetails,btnField);
					}
				}
				}
				catch
				{
					
				}

			};

		}
		public List<CodePickerModel> SetParticipantsDataSource(List<UserDetails> listOfUsers,out int wvalue)
		{
			wvalue=0;
			CodePickerModel item;
			int pcharcount = 0;
			List<CodePickerModel> dlist = new List<CodePickerModel> ();
			foreach (UserDetails user in listOfUsers) {
				item = new CodePickerModel ();
				item.ItemID = user.ID;
				item.ItemText = user.LastName + ", " + user.FirstName;
				dlist.Add (item);
				if (pcharcount < item.ItemText.Length) {
					pcharcount = item.ItemText.Length;
					wvalue = MeasureTextSize (item.ItemText);
				}
				item = null;
			}
			return dlist.OrderBy(x => x.ItemText).ToList();
		}


		public void BindAllControlWithText()
		{
			//iProPQRSPortableLib.Consts.SelectedProcAttribtslist.
			BindTextToLabel(selectedAnesTechTypesIds,lblAnesTechs);
			BindTextToLabel(selectedNerveBlockIds,lblNerveBlock);


			BindTextToLabel (selectedSpecialTechIds, lblSpecialTechs);

			string finaltxt=string.Empty;
			if (selectedLineCVCSterileTechIds != null && selectedLineCVCSterileTechIds.Count > 0) {
				selectedLineCVCSterileTechIds.Sort ();
				foreach (var itemid in selectedLineCVCSterileTechIds) {
					finaltxt = finaltxt + iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.Find (u => u.Value == itemid).Description+" ,";	
				}
				lblCVCSterileTechs.Text = finaltxt.TrimEnd (',');
			}

		}
		public List<CodePickerModel> mSetDataSource(string[] sList,out int wvalue)
		{
			wvalue=0;
			CodePickerModel item;
			int cnt = 0;
			int pcharcount = 0;
			List<CodePickerModel> dlist = new List<CodePickerModel> ();
			foreach (string str in sList) {
				item = new CodePickerModel ();
				item.ItemID = cnt;
				item.ItemText = str;
				dlist.Add (item);
				cnt++;
				if (pcharcount < item.ItemText.Length) {
					pcharcount = item.ItemText.Length;
					wvalue = MeasureTextSize (item.ItemText);
				}


				item = null;
			}
			return dlist;
		}

		public string ddlEncounterType {
			get;
			set;
		}
		public string Emergency {
			get;
			set;
		}

		private void KeyBoardDownNotification(NSNotification notification)
		{
			if (moveViewUp) {
				ScrollTheView (false);
				Notekeyboard = false;
			}
		}

		private void KeyBoardUpNotification(NSNotification notifcation)
		{
			try
			{
				if(showKeyBoard == false)
					return;
				
				CoreGraphics.CGRect r = UIKeyboard.BoundsFromNotification (notifcation);

				foreach (UIView view in this.View.Subviews[3].Subviews) {
					if(view.GetType().Name == "UIScollView"){
						//					if (view.IsFirstResponder)
						activeView = view;
					}
				}
				//baseYDiagnosisDescriptionValue
				if(baseYDiagnosisDescriptionValue >200)
					diagnosisScrollView.SetContentOffset(new CGPoint(0,(nfloat)baseYDiagnosisDescriptionValue-80),false);
						
					//diagnosisScrollView.ContentSize = new SizeF (float.Parse (diagnosisScrollView.Frame.Width.ToString ()), float.Parse (diagnosisScrollView.Frame.Height.ToString ())+baseYDiagnosisDescriptionValue);

				if(baseYProcDescriptionValue >200)
					procedureScrollView.SetContentOffset(new CGPoint(0,(nfloat)baseYProcDescriptionValue-80),false);
				

				//if(Notekeyboard == true)
					//svBillingInfo.SetContentOffset(new CGPoint(svBillingInfo.Frame.X,svBillingInfo.Frame.Height),false);

				activeView = diagnosisScrollView ;

				//bottom of the controller = initial pos + height + offset
				bottom = (activeView.Frame.Y + activeView.Frame.Height + offset)+50;

				//calculate how far we want to scroll
				scroll_amount = (r.Height - (View.Frame.Size.Height - bottom));

				//Perform the scrolling
				if (scroll_amount > 0) {
					moveViewUp = true;
					ScrollTheView (moveViewUp);
				} else {
					moveViewUp = false;
				}
			}
			catch(Exception e) {
				NavigationController.PopToRootViewController(true);
				Console.WriteLine ("KeyBoardUpNotification ex: " + e.Message);
			}
		}

		private void ScrollTheView(bool move)
		{
			//scroll the view up or down
			try{
				UIView.BeginAnimations (string.Empty, System.IntPtr.Zero);
				UIView.SetAnimationDuration (0.3);

				CoreGraphics.CGRect frame = View.Frame;

				if (move) {
					frame.Y -= scroll_amount;
				} else {
					frame.Y += scroll_amount;
					scroll_amount = 0;
				}

				View.Frame = frame;
				UIView.CommitAnimations ();
			}
			catch(Exception e) {
				NavigationController.PopToRootViewController(true);
				Console.WriteLine ("ScrollTheView ex: "+e.Message);
			}
		}







		private async void RemovePreviousParticipant(UIButton btn,int roleID)
		{
			ProcedureParticipantDetails participantDetails = new ProcedureParticipantDetails();
			if(roleID == 2){
				participantDetails = selectedAnestheologistList.Find (x => x.Name.Trim() == btn.TitleLabel.Text.Trim());
			}
			if(roleID == 6){
				participantDetails = selectedCRNAList.Find (x => x.Name.Trim() == btn.TitleLabel.Text.Trim());
			}

			if (participantDetails != null) {
				if (participantDetails.ProcParticipantID != 0) {
					ReceiveContext deletecontext = await AppDelegate.Current.pqrsMgr.DeleteProcedureParticipant (participantDetails.ProcParticipantID);
					if (deletecontext != null && deletecontext.status != null && deletecontext.status.ToUpper () == "OK") {
						Console.WriteLine ("Participant: " + btn.TitleLabel.Text + " Deleted Successfully");
						if (roleID == 2)
							selectedAnestheologistList.Remove (participantDetails);

						if (roleID == 6)
							selectedCRNAList.Remove (participantDetails);

					}
					else
						NavigationController.PopToRootViewController(true);
				}
			}
		}





		public void NewDiagnosis(string code,string description)
		{
			UITextView txtdesc;
			UIView view=diagnosisScrollView.ViewWithTag(diagnosiswithTag);
			if (view != null) {
				txtdesc = view as UITextView;
				if (string.IsNullOrEmpty (txtdesc.Text)) {
					return;
				}
			}

			CoreGraphics.CGRect descriptionFrame = new CoreGraphics.CGRect(8,baseYDiagnosisDescriptionValue,377,30);
			CoreGraphics.CGRect codeFrame = new CoreGraphics.CGRect(400,baseYDiagnosisCodeValue,70,30);

			UITextField codeText = new UITextField(codeFrame);
			//UITextField descriptionText = new UITextField(descriptionFrame);
			MultilineTextView descriptionText = new MultilineTextView(descriptionFrame);
			codeText.Text=code;
			codeText.BorderStyle = UITextBorderStyle.RoundedRect;
			diagnosiscodewithTag = diagnosiscodewithTag + 1;
			codeText.Tag = diagnosiscodewithTag;

			codeText.ShouldBeginEditing += delegate {
				if(this.procedureDetails == null){
					showKeyBoard = false;
					return false;
				}else{
					showKeyBoard = true;
					return true;
				}
			};

			codeText.EditingDidEnd += (object sender, EventArgs e) => {
				showKeyBoard = false;
			};

			descriptionText.ShouldBeginEditing += delegate {
				if(this.procedureDetails == null){
					return false;
				}else{
					showKeyBoard = true;
					return true;
				}
			};

			descriptionText.ShouldEndEditing += delegate {
				showKeyBoard = false;
				return true;
			};


			diagnosiswithTag = diagnosiswithTag+1 ;
			descriptionText.Tag = diagnosiswithTag;

			int mh=MeasureTextLine(description);
			nfloat fh=30;
			if (mh == 280) {
				
					baseYDiagnosisDescriptionValue += 10;
				baseYDiagnosisCodeValue += 10;	
				fh=30;
			}
			else if(mh>600 && mh < 899){								
				baseYDiagnosisDescriptionValue += 20;
				baseYDiagnosisCodeValue += 20;	
				fh=50;
			}
			else if (mh > 900) {
				baseYDiagnosisDescriptionValue += 30;
				baseYDiagnosisCodeValue += 30;	
				fh = 60;
			} else if (mh > 1200) {
				baseYDiagnosisDescriptionValue += 40;
				baseYDiagnosisCodeValue += 40;	
				fh = 77;
			}
			descriptionText.Layer.Frame=new CoreGraphics.CGRect(descriptionText.Frame.X,descriptionText.Frame.Y,descriptionText.Frame.Width,fh);	
			descriptionText.Text=description;

			descriptionText.Changed+= (object sender, EventArgs e) => {

				txtpmrn.ResignFirstResponder();
				txtpFirstName.ResignFirstResponder();
				txtpLastName.ResignFirstResponder();

				if(descriptionText.Text.Length  > 1)
				{
//					bool itemPreviouslySearched = false;
//					string lastSelectedProcedures = ReadFile("lastSelectedDiagnosis.txt");
//					if(lastSelectedProcedures != string.Empty){
//						lastSelectedDiagnosisObj = (ProcedureDiagnosticMaster)JsonConvert.DeserializeObject(lastSelectedProcedures,typeof(ProcedureDiagnosticMaster));
//						foreach (DataResults item in lastSelectedDiagnosisObj.results) {
//							if(item.Name != null){
//								if(item.Name.ToLower().Contains(descriptionText.Text.Trim().ToLower())){
//									itemPreviouslySearched = true;
//									break;
//								}
//							}
//						}
//					}

//					if(itemPreviouslySearched){
//						int uvWidth=280;
//						List<CodePickerModel> list = SetDataSource(out uvWidth,lastSelectedDiagnosisObj);
//						float x = (float)descriptionText.Frame.X;
//						float y = (float)descriptionText.Frame.Y;
//
//						cp	=new CodePicker(this,uvWidth,descriptionText.Text,"ICD9");
//						cp.PresentFromPopover(descriptionText,x,y,uvWidth);
//						cp.DataSource=list;
//						cp._ValueChanged += async delegate {		
//							int ProcCodeID = cp.SelectedValue;
//							codeText.Text = cp.SelectedCodeValue;
//							int nl=MeasureTextLine(cp.SelectedText);
//							nfloat th=30;
//							if (nl == 280) {
//
//								baseYDiagnosisDescriptionValue += 0;
//								baseYDiagnosisCodeValue += 0;	
//								nl=30;
//							}
//							else if(nl>600 && nl < 899){								
//								baseYDiagnosisDescriptionValue += 10;
//								baseYDiagnosisCodeValue += 10;	
//								th=50;
//							}
//							else if(nl>900){
//								baseYDiagnosisDescriptionValue += 20;
//									baseYDiagnosisCodeValue += 20;	
//								th=60;
//							}
//							else if(nl>1200){
//								baseYDiagnosisDescriptionValue += 30;
//								baseYDiagnosisCodeValue += 30;	
//								th=77;
//							}
//							descriptionText.Layer.Frame=new CoreGraphics.CGRect(descriptionText.Frame.X,descriptionText.Frame.Y,descriptionText.Frame.Width,th);	
//							descriptionText.Text = cp.SelectedText;
//
//							UpdateProcedureDiagnostic(ProcCodeID,cp.SelectedCodeValue,cp.SelectedText,2);
//							selectedDiagnosisCodeid.Add(ProcCodeID);
//						}; 
//					}else{
						if(descriptionText.Text.Trim().Length > 0)
							DownloadData("ICD9",descriptionText.Text,codeText,descriptionText);

//					}	
				}
			}; 

			diagnosisScrollView.AddSubview(codeText);
			baseYDiagnosisCodeValue += 44;

			diagnosisScrollView.AddSubview(descriptionText);
			baseYDiagnosisDescriptionValue += 44;

			//diagnosisScrollView.SizeToFit ();
			//baseYDiagnosisDescriptionValue
			diagnosisScrollView.ContentSize = new SizeF (float.Parse (diagnosisScrollView.Frame.Width.ToString ()), float.Parse (diagnosisScrollView.Frame.Height.ToString ())+50);

		}

		public void AddProcedure(string code,string description)
		{
			
			UITextView txtdesc;
			UIView view = procedureScrollView.ViewWithTag(procedurewithTag);
			if (view != null) {
				txtdesc = view as UITextView;
				if (string.IsNullOrEmpty (txtdesc.Text)) {
					return;
				}
			}

			CoreGraphics.CGRect descriptionFrame = new CoreGraphics.CGRect(14,baseYProcDescriptionValue,377,30);
			CoreGraphics.CGRect codeFrame = new CoreGraphics.CGRect(405,baseYProcCodeValue,70,30);

			UITextField codeText = new UITextField(codeFrame);
			MultilineTextView descriptionText = new MultilineTextView(descriptionFrame);
			codeText.Text=code;
			codeText.BorderStyle = UITextBorderStyle.RoundedRect;

			procedurecodewithTag = procedurecodewithTag + 1;
			codeText.Tag = procedurecodewithTag;
			codeText.ShouldBeginEditing += delegate {
				if(this.procedureDetails == null){
					showKeyBoard = false;
					return false;
				}else{
					showKeyBoard = true;
					return true;
				}
			};

			codeText.EditingDidEnd += (object sender, EventArgs e) => {
				showKeyBoard = false;
			};

			descriptionText.ShouldBeginEditing += delegate {
				if(this.procedureDetails == null){
					return false;
				}else{
					showKeyBoard = true;
					return true;
				}
			};

			descriptionText.ShouldEndEditing += delegate {
				showKeyBoard = false;
				return true;
			};


			procedurewithTag = procedurewithTag+1 ;
			descriptionText.Tag =procedurewithTag;

			int mh=MeasureTextLine(description);
			nfloat fh=30;
			if (mh == 280) {

				baseYProcDescriptionValue += 10;
				baseYProcCodeValue += 10;	
				mh=30;
			}
			else if(mh>600 && mh < 899){								
				baseYProcDescriptionValue += 20;
				baseYProcCodeValue += 20;	
				fh=50;
			}
			else if (mh > 900) {
				baseYProcDescriptionValue += 30;
				baseYProcCodeValue += 30;	
				fh = 60;
			} else if (mh > 1200) {
				baseYProcDescriptionValue += 40;
				baseYProcCodeValue += 40;	
				fh = 77;
			}
			descriptionText.Layer.Frame=new CoreGraphics.CGRect(descriptionText.Frame.X,descriptionText.Frame.Y,descriptionText.Frame.Width,fh);	
			descriptionText.Text=description;

			//descriptionText.Changed += (senderDesc, e) => {
			descriptionText.Changed += (senderDesc, e) => {

				txtpmrn.ResignFirstResponder();
				txtpFirstName.ResignFirstResponder();
				txtpLastName.ResignFirstResponder();

				if(descriptionText.Text.Length > 1)
				{	
//					bool itemPreviouslySearched = false;
//					string lastSelectedProcedures = ReadFile("lastSelectedProcedures.txt");
//					if(lastSelectedProcedures != string.Empty){
//						lastSelectedProceduresObj = (ProcedureDiagnosticMaster)JsonConvert.DeserializeObject(lastSelectedProcedures,typeof(ProcedureDiagnosticMaster));
//						foreach (DataResults item in lastSelectedProceduresObj.results) {
//							if(item.Name != null){
//								if(item.Name.ToLower().Contains(descriptionText.Text.Trim().ToLower())){
//									itemPreviouslySearched = true;
//									break;
//								}
//							}
//						}
//					}

//					if(itemPreviouslySearched){
//						int uvWidth=280;
//						List<CodePickerModel> list=SetDataSource(out uvWidth,lastSelectedProceduresObj);
//						float x = (float)descriptionText.Frame.X;
//						float y = (float)descriptionText.Frame.Y;
//
//						cp	=new CodePicker(this,uvWidth,descriptionText.Text,"CPT");
//						cp.PresentFromPopover(descriptionText,x,y,uvWidth);
//						cp.DataSource=list;
//						cp._ValueChanged +=  async delegate {		
//							int ProcCodeID = cp.SelectedValue;
//							codeText.Text = cp.SelectedCodeValue;
//							int nl=MeasureTextLine(cp.SelectedText);
//							nfloat th=30;
//							if (nl == 280) {
//
//								baseYProcDescriptionValue += 0;
//								baseYProcCodeValue += 0;	
//								mh=30;
//							}
//							else if(nl>600 && nl < 899){								
//								baseYProcDescriptionValue += 10;
//								baseYProcCodeValue += 10;	
//								th=50;
//							}
//							else if(nl>900){
//
//								baseYProcDescriptionValue += 20;
//								baseYProcCodeValue += 20;	
//								th=60;
//							}
//							else if(nl>1200){
//								baseYProcDescriptionValue += 30;
//								baseYProcCodeValue += 30;	
//								th=77;
//							}
//							descriptionText.Layer.Frame=new CoreGraphics.CGRect(descriptionText.Frame.X,descriptionText.Frame.Y,descriptionText.Frame.Width,th);	
//							descriptionText.Text = cp.SelectedText;
//
//							UpdateProcedureDiagnostic(ProcCodeID,cp.SelectedCodeValue,cp.SelectedText,1);
//							selectedprocedureCodeid.Add(ProcCodeID);
//						}; 
//					}else{
//						if(descriptionText.Text.Trim().Length > 0)
//						{
							DownloadData("CPT",descriptionText.Text,codeText,descriptionText);

//						}
//					}
				}

			};


			procedureScrollView.AddSubview(codeText);
			baseYProcCodeValue += 54;

			procedureScrollView.AddSubview(descriptionText);
			baseYProcDescriptionValue += 54;

			procedureScrollView.SizeToFit ();

			procedureScrollView.ContentSize = new SizeF (float.Parse (procedureScrollView.Frame.Width.ToString ()), float.Parse (procedureScrollView.Frame.Height.ToString ())+55
			);
		}
		private async void RemoveDiagnosis ()
		{
			int proccodeid=0;
			if(selectedDiagnosisCodeid.Count>0)
				proccodeid=selectedDiagnosisCodeid[selectedDiagnosisCodeid.Count-1];

			foreach(UIView view in diagnosisScrollView.Subviews){
				if(view.GetType().FullName.Contains("UITextField") ){
					if((view.Frame.Y == (baseYDiagnosisDescriptionValue - 44)) && (view.Tag == 1)){						
						view.RemoveFromSuperview();
						baseYDiagnosisDescriptionValue -= 44;
						ReceiveContext deletecontext = await AppDelegate.Current.pqrsMgr.DeleteProcedureDiagnostic(proccodeid);
						if (deletecontext != null && deletecontext.status != null && deletecontext.status.ToUpper () == "OK")
							selectedDiagnosisCodeid.Remove (proccodeid);

					}

					if((view.Frame.Y == (baseYDiagnosisCodeValue - 44)) && (view.Tag == 2)){
						view.RemoveFromSuperview();
						baseYDiagnosisCodeValue -= 44;

					}

				}
			}
		}

		private async void RemoveProcedures ()
		{
			int proccodeid=0;
			if(selectedprocedureCodeid.Count>0)
				proccodeid=selectedprocedureCodeid[selectedprocedureCodeid.Count-1];

			foreach(UIView view in procedureScrollView.Subviews){
				if(view.GetType().FullName.Contains("UITextField") ){
					if((view.Frame.Y == (baseYProcDescriptionValue - 44)) && (view.Tag == 1)){
						view.RemoveFromSuperview();
						baseYProcDescriptionValue -= 44;
						ReceiveContext deletecontext=await AppDelegate.Current.pqrsMgr.DeleteProcedureDiagnostic(proccodeid);
						if (deletecontext != null && deletecontext.status != null && deletecontext.status.ToUpper () == "OK")
							selectedprocedureCodeid.Remove (proccodeid);
					}
					if((view.Frame.Y == (baseYProcCodeValue - 44)) && (view.Tag == 2)){
						view.RemoveFromSuperview();
						baseYProcCodeValue -= 44;
					}

				}
			}
		}

		public async void DownloadData(string type,string searchParam,UITextField codeField,UITextField descriptionField)
		{
			var webClient = new WebClient();
			string url =  "http://reference.iprocedures.com/"+type+"/"+searchParam.Trim()+"/40";
			string procData = webClient.DownloadString (url);
			procedureItems = (ProcedureDiagnosticMaster)JsonConvert.DeserializeObject (procData, typeof(ProcedureDiagnosticMaster));

			float x = (float)descriptionField.Frame.X;
			float y = (float)descriptionField.Frame.Y;
			int uvwidth = 0;
			List<CodePickerModel> list = SetProcedureDataSource (out uvwidth);
    		cp	= new CodePicker (this,uvwidth,searchParam.Trim(),type);

			if(searchParam.Trim().Length > 0)
				cp.PresentFromPopover (descriptionField, x, y,uvwidth);

			cp.DataSource = list;
			cp._ValueChanged += async delegate {
				DataResults selectedObj = new DataResults{Name = cp.SelectedText,Code=cp.SelectedCodeValue,ProcCodeID = cp.SelectedValue};
				string fileName = string.Empty;
				if(type == "CPT")
					fileName = "lastSelectedProcedures.txt";
				else
					fileName = "lastSelectedDiagnosis.txt";

				SaveJsonToFile(selectedObj,fileName);

				codeField.Text = cp.SelectedCodeValue;
				descriptionField.Text = cp.SelectedText;
				int ProcCodeType;
				if(type == "CPT")
					ProcCodeType = 1 ;
				else
				{
					selectedDiagnosisCodeid.Add(cp.SelectedValue);
					ProcCodeType = 2 ;
				}

				UpdateProcedureDiagnostic(cp.SelectedValue,cp.SelectedCodeValue,cp.SelectedText,ProcCodeType);
			};
		}

		public async void DownloadData(string type,string searchParam,UITextField codeField,UITextView descriptionField)
		{
//			var webClient = new WebClient();
//			string url =  "http://reference.iprocedures.com/"+type+"/"+searchParam.Trim()+"/40";
//			string procData = webClient.DownloadString (url);
//			procedureItems = (ProcedureDiagnosticMaster)JsonConvert.DeserializeObject (procData, typeof(ProcedureDiagnosticMaster));

			//1 - procedure 2 - diagnosis
			string procCodeType = "1";
			if(type == "ICD9")
				procCodeType = "2";

			ReceiveContext procDiagProcedureLastUsedItems = await AppDelegate.Current.pqrsMgr.GetLastUsedProceduresDiagnosis(procCodeType);
			if (procDiagProcedureLastUsedItems != null && procDiagProcedureLastUsedItems.result != null) {
				lastUsedProcedureItemsList = (List<LastUsedProcedureDiagnosisDetails>)JsonConvert.DeserializeObject (procDiagProcedureLastUsedItems.result.ToString (), typeof(List<LastUsedProcedureDiagnosisDetails>));
			}

			float x = (float)descriptionField.Frame.X;
			float y = (float)descriptionField.Frame.Y;
			int uvwidth = 0;
			List<CodePickerModel> list=SetProcedureDataSource (out uvwidth);

			cp	= new CodePicker (this,800,searchParam.Trim(),type);

			if(searchParam.Trim().Length > 0)
				cp.PresentFromPopover (descriptionField, x, y,800);

			cp.DataSource = list;
			cp._ValueChanged += new CodePickerSelectedEvent(checkVal);
			//cp._ValueChanged+= async (Item) =>  {};
			//cp._ValueChanged += async delegate {
			cp._ValueChanged+= async (Item) =>  {	
				if(cpSelectedItem != null)
				{
					cp.SelectedText = cpSelectedItem.ItemText;
					cp.SelectedValue = cpSelectedItem.ItemID;
					cp.SelectedCodeValue = cpSelectedItem.ItemCode;
				}
				if(cp.SelectedText!=null)
				{
					DataResults selectedObj = new DataResults{Name = cp.SelectedText,Code=cp.SelectedCodeValue,ProcCodeID = cp.SelectedValue};
					string fileName = string.Empty;
					if(type == "CPT")
						fileName = "lastSelectedProcedures.txt";
					else
						fileName = "lastSelectedDiagnosis.txt";

					SaveJsonToFile(selectedObj,fileName);

					codeField.Text = cp.SelectedCodeValue;
					descriptionField.Text = cp.SelectedText;
					int ProcCodeType;
					if(type == "CPT")
						ProcCodeType = 1 ;
					else
					{
						selectedDiagnosisCodeid.Add(cp.SelectedValue);
						ProcCodeType = 2 ;
					}

					UpdateProcedureDiagnostic(cp.SelectedValue,cp.SelectedCodeValue,cp.SelectedText,ProcCodeType);

					int nl=MeasureTextLine(cp.SelectedText);
					nfloat th=30;
					if (nl == 280) {

						baseYDiagnosisDescriptionValue += 0;
						baseYDiagnosisCodeValue += 0;	
						nl=30;
					}
					else if(nl>550 && nl < 899){	
						if(ProcCodeType == 2)
						{
							baseYProcDescriptionValue += 10;
							baseYProcCodeValue += 10;	
						}
						else
						{
							baseYDiagnosisDescriptionValue += 10;
							baseYDiagnosisCodeValue += 10;	
						}
						th=50;
					}
					else if(nl>900){
						if(ProcCodeType==2)
						{
							baseYProcDescriptionValue += 20;
							baseYProcCodeValue += 20;	
						}
						else
						{
							baseYDiagnosisDescriptionValue += 20;
							baseYDiagnosisCodeValue += 20;	
						}
						th=60;
					}
					else if(nl>1200){
						if(ProcCodeType==2)
						{
							baseYProcDescriptionValue += 30;
							baseYProcCodeValue += 30;	
						}
						else
						{
							baseYDiagnosisDescriptionValue += 30;
							baseYDiagnosisCodeValue += 30;	
						}
						th=77;
					}
					descriptionField.Layer.Frame=new CoreGraphics.CGRect(descriptionField.Frame.X,descriptionField.Frame.Y,descriptionField.Frame.Width,th);


				}
				else
				{
					//new UIAlertView("", "Please Selecte again." , null, "ok", null).Show(); 
				}

			};
		}
		CodePickerModel cpSelectedItem;
		public void checkVal(CodePickerModel Item)
		{
			cpSelectedItem = Item;
		}
		public List<CodePickerModel> SetProcedureDataSource(out int wvalue)
		{
			CodePickerModel item;
			int cnt = 1;
			wvalue=280;
			int pcharcount = 0;
			int charcount = 0;
			List<CodePickerModel> dlist = new List<CodePickerModel> ();
//			foreach (DataResults procitem in procedureItems.results) {
//				item = new CodePickerModel ();
//				item.ItemCode = procitem.Code;
//				item.ItemText = procitem.Name;
//				dlist.Add (item);
//				if (pcharcount < item.ItemText.Length) {
//					pcharcount = item.ItemText.Length;
//					wvalue = MeasureTextSize (item.ItemText);
//				}
//				item = null;
//				cnt++;
//			}
			foreach (LastUsedProcedureDiagnosisDetails procitem in lastUsedProcedureItemsList) {
				item = new CodePickerModel ();
				item.ItemCode = procitem.Code;
				item.ItemText = procitem.Name;
				dlist.Add (item);
				if (pcharcount < item.ItemText.Length) {
					pcharcount = item.ItemText.Length;
					wvalue = MeasureTextSize (item.ItemText);
				}
				item = null;
				cnt++;
			}
			return dlist;
		}

		public List<CodePickerModel> SetDataSource(out int uvWidth,ProcedureDiagnosticMaster procDiagObj)
		{
			uvWidth=280;
			int pcharcount = 0;
			CodePickerModel item;
			int cnt = 1;
			List<CodePickerModel> dlist = new List<CodePickerModel> ();
			foreach (DataResults procItem in procDiagObj.results) {
				item = new CodePickerModel ();
				item.ItemID = procItem.ProcCodeID;
				item.ItemText = procItem.Name;
				item.ItemCode = procItem.Code;
				dlist.Add (item);
				if (pcharcount < item.ItemText.Length) {
					pcharcount = item.ItemText.Length;
					uvWidth = MeasureTextSize (item.ItemText);
				}
				item = null;
				cnt++;
			}
			return dlist;
		}
		public int MeasureTextSize(string txt)
		{
			int uvwidth=280;
			int charcount = 0;
			int mainwidth =Convert.ToInt16(View.Bounds.Width);

			UITableViewCell tbc=new UITableViewCell();
			tbc.TextLabel.Text = txt;
			tbc.TextLabel.Font=UIFont.SystemFontOfSize(20);
			tbc.TextLabel.SizeToFit ();
			int vwidth = Convert.ToInt16(tbc.TextLabel.Bounds.Width);
			if (vwidth < uvwidth)
				uvwidth = 280;
			else
				uvwidth = vwidth;

			if (mainwidth < vwidth)
				uvwidth = mainwidth-100;

			return uvwidth;
		}

		public void SaveJsonToFile(DataResults selectedObj,string fileName)
		{
			string json = string.Empty;

			string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string filePath = Path.Combine(path, fileName);

			List<DataResults> lstOfSelectedProcs = new List<DataResults>();
			lstOfSelectedProcs.Add(selectedObj);

			if (!File.Exists (filePath)) {
				ProcedureDiagnosticMaster lastSelectProcs = new ProcedureDiagnosticMaster ();
				lastSelectProcs.results = lstOfSelectedProcs;
				json = JsonConvert.SerializeObject (lastSelectProcs);
			} else {
				string lastContents = ReadFile (filePath);
				File.Delete (filePath);
				ProcedureDiagnosticMaster lastSelectedProceduresObj = (ProcedureDiagnosticMaster)JsonConvert.DeserializeObject(lastContents,typeof(ProcedureDiagnosticMaster));
				lastSelectedProceduresObj.results.Add (selectedObj);
				json = JsonConvert.SerializeObject (lastSelectedProceduresObj);
			}


			using (var file = File.Open(filePath, FileMode.Append, FileAccess.Write))
			using (var strm = new StreamWriter(file))
			{
				strm.Write(json);

			}
		}

		public string ReadFile(string fileName)
		{
			string content = string.Empty;
			string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string filename = Path.Combine(path, fileName);

			if(File.Exists(filename)){
				using (var streamReader = new StreamReader(filename))
				{
					content = streamReader.ReadToEnd();
				}
			}

			return content;
		}

		public bool ValidatefiledsFinalizecase()
		{
			bool isvalidate = true;
			StringBuilder errormsg = new StringBuilder ();
			bool firttab = false;
			if (string.IsNullOrEmpty (txtpDOB.Text) || txtpDOB.Text.Contains("Select")) {
			//	txtpDOB.BecomeFirstResponder  ();
				//new UIAlertView("", "DOB is Required.", null, "ok", null).Show();
				firttab=true;
				errormsg.Append("DOB. \n");
				isvalidate = false;
				//return false;
			}
			if (string.IsNullOrEmpty (EncounterBtn.TitleLabel.Text) || EncounterBtn.TitleLabel.Text.Contains("Select")) {				
			//	EncounterBtn.BecomeFirstResponder  ();
				//new UIAlertView("", "Encounter Type is Required.", null, "ok", null).Show();
				//return false;
				firttab=true;
				errormsg.Append("Encounter Type. \n");
				isvalidate = false;
			}
			if (string.IsNullOrEmpty (PhysicalStatusBtn.TitleLabel.Text) || PhysicalStatusBtn.TitleLabel.Text.Contains("Select")) {
				//PhysicalStatusBtn.BecomeFirstResponder  ();
				//new UIAlertView("", "Physical Status is Required.", null, "ok", null).Show();
				//return false;
				firttab=true;
				errormsg.Append("PhysicalStatu. \n");
				isvalidate = false;
			}


			UITextView txtdesc;
			UIView view=diagnosisScrollView.ViewWithTag(diagnosiswithTag);
			if (view != null) {
				txtdesc = view as UITextView;
				if (string.IsNullOrEmpty (txtdesc.Text)) {
					//txtdesc.BecomeFirstResponder ();
					firttab=true;
					errormsg.Append("Diagnosis . \n");
					isvalidate = false;
				//	new UIAlertView ("Diagnosis Info", "Diagnosis is Required.", null, "ok", null).Show ();
				//	return false;
				}
			} else {
				new UIAlertView("Diagnosis Info", "Diagnosis .", null, "ok", null).Show();
				return false;
			}


			UITextView ptxtdesc;
			UIView pview = procedureScrollView.ViewWithTag(procedurewithTag);
			if (pview != null) {
				ptxtdesc = pview as UITextView;
				if (string.IsNullOrEmpty (ptxtdesc.Text)) {
					//ptxtdesc.BecomeFirstResponder  ();
					//new UIAlertView ("Procedure Info", "Procedure is Required.", null, "ok", null).Show ();
					//return false;
					firttab=true;
					errormsg.Append("Procedure . \n");
					isvalidate = false;
				}
			} else {
				new UIAlertView ("Procedure Info", "Procedure .", null, "ok", null).Show ();
				return false;
			}
			bool secantab = false;
			if (string.IsNullOrEmpty (lblSurgeon.Text)) {
				//sCtab.SelectedSegment = 1;
				//Selectedtab (1);
				lblSurgeon.BecomeFirstResponder  ();
				//new UIAlertView ("", "Surgeon is Required.", null, "ok", null).Show ();
				//rturn false;
				secantab=true;
				errormsg.Append("Surgeon . \n");
				isvalidate = false;
			}

			UIView viewanes = uvProviderView.ViewWithTag(AnesCount+100);
			if (viewanes != null) {
				UIButton checkbtn=(UIButton)viewanes.ViewWithTag(2);
				if (checkbtn != null &&   !string.IsNullOrEmpty (checkbtn.TitleLabel.Text)) {
					//sCtab.SelectedSegment = 1;
					//Selectedtab (1);
					checkbtn.BecomeFirstResponder  ();
					//new UIAlertView ("Provider info", "Anes  is Required.", null, "ok", null).Show ();
					//return false; 
					//secantab=true;
					//errormsg.Append("Anes Name. \n");
					//isvalidate = false;

				
				//Anesstarttimebtn=startTimeBtn;
				//Anesendtimebtn=endTimeBtn;
					if (Anesstarttimebtn != null &&  string.IsNullOrEmpty (Anesstarttimebtn.TitleLabel.Text)) {
						//sCtab.SelectedSegment = 1;
						//Selectedtab (1);
						Anesstarttimebtn.BecomeFirstResponder  ();
						//new UIAlertView ("Provider info", "Anes Start Time is Required.", null, "ok", null).Show ();
						//return false;
						secantab=true;
						errormsg.Append("Anes Start Time . \n");
						isvalidate = false;
					}			
					if (Anesendtimebtn != null && string.IsNullOrEmpty (Anesendtimebtn.TitleLabel.Text)) {
						//sCtab.SelectedSegment = 1;
						//Selectedtab (1);
						Anesendtimebtn.BecomeFirstResponder  ();
						//new UIAlertView ("Provider info", "Anes End Time is Required.", null, "ok", null).Show ();
						//return false; 
						secantab=true;
						errormsg.Append("Anes End Time . \n");
						isvalidate = false;
					}
				}
			}
//			else {
//				secantab=true;
//				errormsg.Append("Anes Name. \n");
//				isvalidate = false;
//
//			}


			UIView viewcrna = uvProviderView.ViewWithTag(crnacount+200);
			if (viewcrna != null) {
				UIButton checkbtncrna = (UIButton)viewcrna.ViewWithTag(2);
				if (checkbtncrna != null && !string.IsNullOrEmpty (checkbtncrna.TitleLabel.Text)) {
					//sCtab.SelectedSegment = 1;
					//Selectedtab (1);
					//secantab=true;
					//checkbtncrna.BecomeFirstResponder  ();
					//new UIAlertView("CRNA Info", "Please Update CRNA"+crnacount+" Record."
					//	, null, "ok", null).Show();
					//return false; 
					//errormsg.Append("Please Update CRNA Name. \n");
					//isvalidate = false;
				
				//crnaStarttime = startTimeBtn;
				//crnaendtime = endTimeBtn;			
					if (crnaStarttime != null && string.IsNullOrEmpty (crnaStarttime.TitleLabel.Text)) {
						//sCtab.SelectedSegment = 1;
						//Selectedtab (1);
						crnaStarttime.BecomeFirstResponder  ();
						//new UIAlertView ("Provider info", "CRNA Start Time is Required.", null, "ok", null).Show ();
						//return false; 
						secantab=true;
						errormsg.Append("CRNA Start Time. \n");
						isvalidate = false;
					}

					if (crnaendtime != null && string.IsNullOrEmpty (crnaendtime.TitleLabel.Text)) {
						//sCtab.SetEnabled (true, 1);
						//Selectedtab (1);
						crnaendtime.BecomeFirstResponder  ();
						//new UIAlertView ("Provider info", "CRNA End Time is Required.", null, "ok", null).Show ();
						//return false; 
						secantab=true;
						errormsg.Append("CRNA End Time. \n");
						isvalidate = false;
					}
				}
			}
//			else {
//				//new UIAlertView ("Provider info", "CRNA  Name.", null, "ok", null).Show ();
//				//return false;
//				secantab=true;
//				errormsg.Append("CRNA Name. \n");
//				isvalidate = false;
//			}

			if (swCVPType.On) {
				
				if (string.IsNullOrEmpty (lblCVCSterileTechs.Text)) {
					//sCtab.SelectedSegment = 1;
					//Selectedtab (1);
					lblCVCSterileTechs.BecomeFirstResponder  ();
					//new UIAlertView ("", "CVCSterileTechs is Required.", null, "ok", null).Show ();
					//return false;
					secantab=true;
					errormsg.Append("CVCSterileTechs is Required. \n");
					isvalidate = false;
				}
			}
			bool thridtab = false;
			//IsRequiredlabels
			string templblname=string.Empty;
			foreach (var rfiled in IsRequiredlabels) {
				if(rfiled != null && rfiled.lbldesc != null && string.IsNullOrEmpty(rfiled.lbldesc.Text))
				{	
					//sCtab.SelectedSegment = 2;
					//Selectedtab (2);

					//new UIAlertView ("", rfiled.lblname+" is Required.", null, "ok", null).Show ();
					//scPotoc.SetEnabled (true, 2);
					rfiled.lbldesc.BecomeFirstResponder  ();
					//return false; 
					if (templblname != rfiled.lblname) {
						thridtab = true;
						errormsg.Append (rfiled.lblname + " . \n");
						isvalidate = false;
						templblname = rfiled.lblname;
					}
				}
			}
			if (!isvalidate) {
				if (firttab) {
					Selectedtab (0);
					sCtab.SelectedSegment = 0;
				} else if (secantab) {
					Selectedtab (1);
					sCtab.SelectedSegment = 1;
				} else if (thridtab) {
					Selectedtab (2);
					sCtab.SelectedSegment = 2;
				}
				
				new UIAlertView ("Incomplete items", errormsg.ToString(), null, "ok", null).Show ();
				return false; 
			}



			return isvalidate;

		}
		public void Selectedtab(int SelectedSegment)
		{
			uvMainPatientinfo.RemoveFromSuperview ();
			uvDynamicQualityMetrics.RemoveFromSuperview ();
			svuvBillingInfo.RemoveFromSuperview ();
			if (SelectedSegment == 0){
//				lblProcID.Hidden = false;
				uvMain.AddSubview (uvPatientInfo);
			}else if (SelectedSegment == 1){
//				lblProcID.Hidden = true;
				uvMain.AddSubview (svuvBillingInfo);
			}else if (SelectedSegment == 2) {
//				lblProcID.Hidden = true;
				uvMain.AddSubview (uvDynamicQualityMetrics);
			}
		}
		public async  Task<bool> submitFinalizecase()
		{
			bool boolresult=false;
			if (ValidatefiledsFinalizecase ()) {
				string alrtmsg = "Are you sure you want to " + btnSubmit.CurrentTitle.ToLower() + " the case?";
				var finalizeConfirm = new UIAlertView ("Confirmation", alrtmsg, null, "Cancel", "Yes");
				finalizeConfirm.Show ();
				finalizeConfirm.Clicked += async (object senders, UIButtonEventArgs es) => {
					if (es.ButtonIndex == 0) {
						boolresult = false;
					}
					else
					{
						boolresult=true;
					if (patientProfile != null && patientProfile.ID != 0) {
						//				AppDelegate.pb.Start(this.View,"Updating ...");
						int ProcID;
						string Mrn;
						string PatientID;
						if (procedureDetails == null) {
							procedureDetails = new PatientProcedureFullDetails ();
							ProcID = 0;
							Mrn = patientProfile.MRN;
							PatientID = patientProfile.ID.ToString ();	
						} else {
							ProcID = procedureDetails.ID;
							Mrn = procedureDetails.Mrn;
							PatientID = procedureDetails.PatientID;	
						}
						procedureDetails.ID = ProcID;
						procedureDetails.Mrn = patientProfile.MRN;
						procedureDetails.PatientID = patientProfile.ID.ToString ();		


						procedureDetails.ddlEncounterType = EncounterBtn.TitleLabel.Text;

						procedureDetails.ddlASAType = physicalStatus.ToString();


						procedureDetails.ListType = "MAIN";
						procedureDetails.Location = iProPQRSPortableLib.Consts.SelectedFacilityID;
						if(btnSubmit.CurrentTitle.Trim() == "Finalize")							
							this.procedureDetails.StatusID = "3";

						if(btnSubmit.CurrentTitle == "Unlock")
							this.procedureDetails.StatusID = "2";


						ReceiveContext result =await AppDelegate.Current.pqrsMgr.UpdatePatintProcedureInfo (procedureDetails);
						if (result != null && result.result != null && result.status != null && result.status.ToUpper () == "OK") {	
							if (ProcID == 0) {
								if (result != null && result.result != null && result.status != null && result.status.ToUpper () == "OK")
									procedureDetails = (PatientProcedureFullDetails)JsonConvert.DeserializeObject (result.result.ToString (), typeof(PatientProcedureFullDetails));


								lblProcID.Text = "Proc ID: " + procedureDetails.ID;
							}
						}
						else
							NavigationController.PopToRootViewController(true);

						if (string.IsNullOrEmpty (result.message) && result.status != null && result.status.ToUpper ().Trim () == "OK") {
							//					AppDelegate.pb.Stop();
							Console.WriteLine ("Patient Procedure Info. Successfully Saved.");	
							//					new UIAlertView("Patient Procedure Info", "Successfully Saved."
							//						, null, "ok", null).Show();


							if (this.procedureDetails.StatusID == "0" || this.procedureDetails.StatusID == "1")
								tctSurgeryDate.Enabled = true;

							if (this.procedureDetails.StatusID == "2" || this.procedureDetails.StatusID == "3")
								tctSurgeryDate.Enabled = false;


							if (this.procedureDetails.StatusID == "3") {
								btnSubmit.SetTitle ("Unlock", UIControlState.Normal);
								btnSubmit.BackgroundColor = UIColor.Gray;
								uvPatientInfo.UserInteractionEnabled = false;
								uvBillingInfo.UserInteractionEnabled = false;
								//uvDynamicQualityMetrics.UserInteractionEnabled = false;
								//svQualityMetrics.UserInteractionEnabled = false;
								Setenableanddiablelbl(false);
							}


							if (this.procedureDetails.StatusID == "2" || this.procedureDetails.StatusID == "1") {
								btnSubmit.SetTitle (" Finalize", UIControlState.Normal);

								uvPatientInfo.UserInteractionEnabled = true;
								string format = "MM/dd/yyyy"; 
								DateTime dt = DateTime.ParseExact(tctSurgeryDate.Text, format,System.Globalization.CultureInfo.CurrentCulture);
								if (DateTime.Now.Date < dt) {
									uvBillingInfo.UserInteractionEnabled = false;
									uvQualityMetrics.UserInteractionEnabled = false;
								} else {
									uvBillingInfo.UserInteractionEnabled = true;
									uvQualityMetrics.UserInteractionEnabled = true;
								}
							}

							//If the Billing Info and Quality Metrics are disabled, enable them after procedure creation
							sCtab.SetEnabled (true, 1);
							sCtab.SetEnabled (true, 2);
						} else {
							NavigationController.PopToRootViewController(true);
							//					AppDelegate.pb.Stop();
							Console.WriteLine ("Patient Procedure Info. Save failed.");	
							//					new UIAlertView("Procedure eroor", result.message
							//						, null, "ok", null).Show();
						}
					} else {
						new UIAlertView ("Patient Info", "Please Update Patient Info"
							, null, "ok", null).Show ();
					}
					if (this.procedureDetails.StatusID == "3") {
						NavigationController.PopViewController(true);
					}
					boolresult= true;
					}
				};
			}
			return boolresult;
			
		}
		public async void ValidateAndUpdateProcedure(string paramname, string paramvalue)
		{
			if (patientProfile != null && patientProfile.ID != 0) {
//				AppDelegate.pb.Start(this.View,"Updating ...");
//				procedureDetails.ID = 0;
//				procedureDetails.Mrn = patientProfile.MRN;
//				procedureDetails.PatientID = patientProfile.ID.ToString();						
//				procedureDetails.StatusID = "1";
//
//				procedureDetails.OperationDate = tctSurgeryDate.Text;
//				procedureDetails.ListType = "MAIN";
//				procedureDetails.Location =  iProPQRSPortableLib.Consts.SelectedFacilityID;


				int ProcID;
				string Mrn;
				string PatientID;
				int StatusID;

				if (procedureDetails == null) {
					procedureDetails = new PatientProcedureFullDetails ();
					ProcID = 0;
					Mrn = patientProfile.MRN;
					PatientID = patientProfile.ID.ToString ();	
					StatusID = 1;
				} else {
					ProcID = procedureDetails.ID;
					Mrn = procedureDetails.Mrn;
					PatientID = procedureDetails.PatientID;	
					if (procedureDetails.StatusID != null)
						StatusID = Convert.ToInt16 (procedureDetails.StatusID);
					else
						StatusID = 1;
				}
				procedureDetails.ID = ProcID;
				procedureDetails.Mrn = patientProfile.MRN;
				procedureDetails.PatientID = patientProfile.ID.ToString ();		
				procedureDetails.StatusID = StatusID.ToString ();
				if (paramname == "EncounterType")
					procedureDetails.ddlEncounterType = paramvalue;
				else if (paramname == "PhysicalStatus")
					procedureDetails.ddlASAType = paramvalue;
				else if (paramname == "Emergency")
					procedureDetails.cbEmergency = paramvalue;

				procedureDetails.ListType = "MAIN";
				procedureDetails.Location = iProPQRSPortableLib.Consts.SelectedFacilityID;
				ReceiveContext result = await AppDelegate.Current.pqrsMgr.UpdatePatintProcedureInfo (procedureDetails);
				if (result != null && result.result != null && result.status != null && result.status.ToUpper () == "OK") {
					if (ProcID == 0) {
						if (result != null && result.result != null && result.status != null && result.status.ToUpper () == "OK")
							procedureDetails = (PatientProcedureFullDetails)JsonConvert.DeserializeObject (result.result.ToString (), typeof(PatientProcedureFullDetails));
						else
							NavigationController.PopToRootViewController (true);
						
						lblProcID.Text = "Proc ID: " + procedureDetails.ID;
					}

					if (string.IsNullOrEmpty (result.message) && result.status != null && result.status.ToUpper ().Trim () == "OK") {
//					AppDelegate.pb.Stop();
						Console.WriteLine ("Patient Procedure Info. Successfully Saved.");	
//					new UIAlertView("Patient Procedure Info", "Successfully Saved."
//						, null, "ok", null).Show();

						if (this.procedureDetails.StatusID == "0" || this.procedureDetails.StatusID == "1")
							tctSurgeryDate.Enabled = true;

						if (this.procedureDetails.StatusID == "2" || this.procedureDetails.StatusID == "3")
							tctSurgeryDate.Enabled = false;
					

						if (this.procedureDetails.StatusID == "3") {
							btnSubmit.SetTitle ("Unlock", UIControlState.Normal);
							uvPatientInfo.UserInteractionEnabled = false;
							uvBillingInfo.UserInteractionEnabled = false;
							uvQualityMetrics.UserInteractionEnabled = false;
						}
						
						btnSubmit.Hidden = false;
						if (this.procedureDetails.StatusID == "1" || this.procedureDetails.StatusID == "2")
							btnSubmit.SetTitle (" Finalize", UIControlState.Normal);
					
						if (this.procedureDetails.StatusID == "2" || this.procedureDetails.StatusID == "1") {
							//btnSubmit.SetTitle ("Finalize", UIControlState.Normal);
							uvPatientInfo.UserInteractionEnabled = true;
							string format = "MM/dd/yyyy"; 
							DateTime dt = DateTime.ParseExact (tctSurgeryDate.Text, format, System.Globalization.CultureInfo.CurrentCulture);
							if (DateTime.Now.Date < dt) {
								uvBillingInfo.UserInteractionEnabled = false;
								uvQualityMetrics.UserInteractionEnabled = false;
							} else {
								uvBillingInfo.UserInteractionEnabled = true;
								uvQualityMetrics.UserInteractionEnabled = true;
							}
						}
						//If the Billing Info and Quality Metrics are disabled, enable them after procedure creation
						sCtab.SetEnabled (true, 1);
						sCtab.SetEnabled (true, 2);
					} else {
//					AppDelegate.pb.Stop();
						Console.WriteLine ("Patient Procedure Info. Save failed.");	
//					new UIAlertView("Procedure eroor", result.message
//						, null, "ok", null).Show();
					}
				} else {
					NavigationController.PopToRootViewController(true);
					//new UIAlertView ("Patient Info", "Please Update Patient Info"
					//, null, "ok", null).Show ();
				}
			}
		}

		public async void UpdateProcedureDiagnostic(int ProcCodeID,string Code,string Name, int ProcCodeTypeID)
		{
			string msgtitle = string.Empty;
			if (ProcCodeTypeID == 1)
				msgtitle = "Procedure";
			else if (ProcCodeTypeID == 418)
				msgtitle = "MAC CODE is " + Code;
			else
				msgtitle = "Diagnostic";
			
			if (ProcCodeTypeID != 418)
			   AppDelegate.pb.Start(this.View,msgtitle+" Updating...");

			DataResults procedureitem = new DataResults();

			int ProcID;
			string Mrn;
			string PatientID;
			if (procedureDetails == null) {
				procedureDetails = new PatientProcedureFullDetails ();
				ProcID = 0;
				Mrn = patientProfile.MRN;
				PatientID = patientProfile.ID.ToString ();	

				procedureDetails.ID=ProcID;
				procedureDetails.Mrn = patientProfile.MRN;
				procedureDetails.PatientID = patientProfile.ID.ToString();		
				procedureDetails.ListType = "MAIN";
				procedureDetails.Location =  iProPQRSPortableLib.Consts.SelectedFacilityID;

				ReceiveContext result = await AppDelegate.Current.pqrsMgr.UpdatePatintProcedureInfo (procedureDetails);
				if (ProcID == 0) {
					if(result != null && result.result != null &&  result.status != null && result.status.ToUpper () == "OK")
						procedureDetails= (PatientProcedureFullDetails)JsonConvert.DeserializeObject (result.result.ToString (), typeof(PatientProcedureFullDetails));
					else
						NavigationController.PopToRootViewController(true);
					

					lblProcID.Text = "Proc ID: " + procedureDetails.ID;
				}
			}

			procedureitem.ProcID = procedureDetails.ID.ToString();
			procedureitem.ProcCodeID = ProcCodeID;
			procedureitem.Code = Code;
			procedureitem.Name = Name;
			procedureitem.ProcCodeTypeID = ProcCodeTypeID;
			ReceiveContext context=	await AppDelegate.Current.pqrsMgr.UpdateProcedureDiagnostic(procedureitem);
			if (context != null && context.status != null && context.status.ToUpper () == "OK") {				
				AppDelegate.pb.Stop ();
				sCtab.SetEnabled (true, 1);
				sCtab.SetEnabled (true, 2);
				//new UIAlertView (msgtitle, "Successfully Saved", null, "ok", null).Show ();
			} else {

				AppDelegate.pb.Stop ();
				new UIAlertView (msgtitle, context.message, null, "ok", null).Show ();
			}

		}

		public async void AddUpdateProcedureParticipants(ProcedureParticipantDetails procParticipant,UIButton btnField)
		{
//			AppDelegate.pb.Start(this.View,"Updating Participants...");
			if(procParticipant.StartTime == string.Empty)
				procParticipant.StartTime = "00:00";

			if(procParticipant.EndTime == string.Empty)
				procParticipant.EndTime = "00:00";

			procParticipant.ProcID = procedureDetails.ID;

			ReceiveContext context=	await AppDelegate.Current.pqrsMgr.AddUpdateProcedureParticipants(procParticipant);
			if (context != null && context.status != null && context.status.ToUpper () == "OK") {

				ProcedureParticipantDetails participant = (ProcedureParticipantDetails)JsonConvert.DeserializeObject (context.result.ToString (), typeof(ProcedureParticipantDetails));
				//if(btnField.Tag == 0)
				btnField.Tag = participant.ProcParticipantID;

				procParticipant.ProcParticipantID = participant.ProcParticipantID;
				if (procParticipant.RoleID == 2) {
					selectedAnestheologistList.Remove (selectedAnestheologistList.Find (u => u.ProcParticipantID == participant.ProcParticipantID));
					selectedAnestheologistList.Add (participant);
				} else {
					selectedCRNAList.Remove(selectedCRNAList.Find(u=>u.ProcParticipantID==participant.ProcParticipantID));
					selectedCRNAList.Add (participant);
				}
				UpdateProcedureStatus ();
//				AppDelegate.pb.Stop ();
//				new UIAlertView ("Participants", "Successfully Saved", null, "ok", null).Show ();
			} else {
//				AppDelegate.pb.Stop ();
//				new UIAlertView ("Participants", context.message, null, "ok", null).Show ();
			}
		}

		int ProcParticipantID=0;
		private async void UpdateProcedureParticipants(ProcedureParticipantDetails procParticipant,UIButton btnField)
		{
			if(procParticipant.StartTime == string.Empty)
				procParticipant.StartTime  = "00:00";

			if(procParticipant.EndTime == string.Empty)
				procParticipant.EndTime = "00:00";
			
			if (btnField.CurrentTitle != "" && btnField.CurrentTitle != null) {
				    procParticipant.ProcID = procedureDetails.ID;
				ReceiveContext updContext = await AppDelegate.Current.pqrsMgr.AddUpdateProcedureParticipants(procParticipant);
     			if (updContext != null && updContext.status != null && updContext.status.ToUpper () == "OK") {
						Console.WriteLine ("Participant: " + btnField.TitleLabel.Text + " Updated Successfully");
						if (procParticipant.RoleID == 2 && selectedAnestheologistList.Count > 0) 
							selectedAnestheologistList.RemoveAt (selectedAnestheologistList.FindIndex (x => x.UserID == procParticipant.UserID));


						if (procParticipant.RoleID == 6 && selectedCRNAList.Count > 0)
							selectedCRNAList.RemoveAt (selectedCRNAList.FindIndex(x => x.UserID == procParticipant.UserID));

						
					UpdateProcedureStatus ();
						ProcedureParticipantDetails participant = (ProcedureParticipantDetails)JsonConvert.DeserializeObject (updContext.result.ToString (), typeof(ProcedureParticipantDetails));
					    ProcParticipantID=participant.ProcParticipantID;
						//if (btnField.Tag == 0)
						//	btnField.Tag = participant.ProcParticipantID;


					if (procParticipant.RoleID == 2) {
						selectedAnestheologistList.Remove(selectedAnestheologistList.Find(u=>u.ProcParticipantID==participant.ProcParticipantID));
						selectedAnestheologistList.Add (participant);
					}

					if (procParticipant.RoleID == 6) {
						selectedCRNAList.Remove(selectedCRNAList.Find(u=>u.ProcParticipantID==participant.ProcParticipantID));
						selectedCRNAList.Add (participant);
					}

					} else {
					    NavigationController.PopToRootViewController(true);
						Console.WriteLine ("Error Updating..UpdateProcedureParticipants(ProcedureParticipantDetails procParticipant,UIButton btnField)... ");							
					}
			} 
		}

		private async void DeleteDiagnosis ()
		{
			UIView view=diagnosisScrollView.ViewWithTag(diagnosiswithTag);
			if (view != null) {
				baseYDiagnosisDescriptionValue -= (int)view.Frame.Height == 30 ? 54 : (int)view.Frame.Height == 50 ? 64 : (int)view.Frame.Height == 60 ? 70 : 80;
				diagnosiswithTag -= 1;
				view.RemoveFromSuperview ();

				UIView codeview = diagnosisScrollView.ViewWithTag (diagnosiscodewithTag);
				baseYDiagnosisCodeValue = baseYDiagnosisDescriptionValue;
				diagnosiscodewithTag -= 1;
				codeview.RemoveFromSuperview ();

				int proccodeid=0;
				if(selectedDiagnosisCodeid.Count>0)
					proccodeid=selectedDiagnosisCodeid[selectedDiagnosisCodeid.Count-1];

				ReceiveContext deletecontext=await AppDelegate.Current.pqrsMgr.DeleteProcedureDiagnostic(proccodeid);
				if (deletecontext != null && deletecontext.status != null && deletecontext.status.ToUpper () == "OK")
					selectedDiagnosisCodeid.Remove (proccodeid);
			}
		}

		public int MeasureTextLine(string txt)
		{
			int uvwidth=280;
			int charcount = 0;
			int mainwidth =Convert.ToInt16(View.Bounds.Width);

			MultilineTextView tbc = new MultilineTextView();
			tbc.Text = txt;
			tbc.Font=UIFont.SystemFontOfSize(20);
			tbc.SizeToFit ();
			int vwidth = Convert.ToInt16(tbc.Frame.Width);
			if (vwidth < uvwidth)
				uvwidth = 280;
			else
				uvwidth = vwidth;

			if (mainwidth < vwidth)
				uvwidth = mainwidth-100;

			return uvwidth;
		}

		private async void DeleteProcedures ()
		{
			
			UIView view=procedureScrollView.ViewWithTag(procedurewithTag);
			if (view != null) {
				//baseYProcDescriptionValue -= (int)view.Frame.Height == 30 ? 44 : (int)view.Frame.Height == 50 ? 54 : (int)view.Frame.Height == 60 ? 64 : 64;
				baseYProcDescriptionValue -= (int)view.Frame.Height == 30 ? 54 : (int)view.Frame.Height == 50 ? 64 : (int)view.Frame.Height == 60 ? 70 : 80;
				procedurewithTag -= 1;
				view.RemoveFromSuperview ();

				UIView codeview = procedureScrollView.ViewWithTag (procedurecodewithTag);
				baseYProcCodeValue = baseYProcDescriptionValue;
				procedurecodewithTag -= 1;
				codeview.RemoveFromSuperview ();

				int proccodeid=0;
				if(selectedprocedureCodeid.Count>0)
					proccodeid = selectedprocedureCodeid[selectedprocedureCodeid.Count-1];

				ReceiveContext deletecontext=await AppDelegate.Current.pqrsMgr.DeleteProcedureDiagnostic(proccodeid);
				if (deletecontext != null && deletecontext.status != null && deletecontext.status.ToUpper () == "OK")
					selectedprocedureCodeid.Remove (proccodeid);

			}
		}

		private void BindSurgeon(List<SurgeonDetails> listOfSurgeons)
		{
			lblSurgeon.Layer.BorderColor = UIColor.Gray.CGColor;
			lblSurgeon.Layer.BorderWidth = (nfloat)0.5;
			lblSurgeon.Layer.CornerRadius = (nfloat)8.0; 
			UITapGestureRecognizer lblSurgeonTap = new UITapGestureRecognizer(() => {
				BindSurgeonPopupover(listOfSurgeons,lblSurgeon,(int)lblSurgeon.Frame.Y);
			});
			lblSurgeon.UserInteractionEnabled = true;
			lblSurgeon.AddGestureRecognizer(lblSurgeonTap);
		}

		public void BindSurgeonPopupover(List<SurgeonDetails> listOfSurgeons,UILabel lblField,int FrameY)
		{
			int uvwidth;
			ATlist.Clear();
			List<CodePickerModel> alist = SetSurgeonDataSource(listOfSurgeons, out  uvwidth);
			SurgeonDetails sd = listOfSurgeons.Find (u => u.ID == SurgeonID);
			CodePickerModel selecteditem = new CodePickerModel ();
			if (sd != null) {				
				selecteditem.ItemID = sd.ID;
				selecteditem.ItemText = sd.Name;
			}
			// SurgeonID
			uvwidth=500;
			qmcp = new qmCodePicker(this,uvwidth,alist);
			qmcp.Issearchactive = true;
			qmcp.TypeOfList = "Surgeons";
			qmcp.AllowsMultipleSelection = false;
			qmcp.SelectedItems.Add (selecteditem);
			float x = 100;
			float y = (float)lblField.Frame.Y+FrameY  ;

			qmcp.PresentFromPopover(lblField, x, y,uvwidth);
			//qmcp.mDataSource(alist);
			qmcp._ValueChanged += async delegate
			{
				if(qmcp.SelectedItems.Count > 0){
					ATlist = qmcp.SelectedItems;
					lblField.Text =  " " + ATlist[0].ItemText;
					SurgeonID=ATlist[0].ItemID;
					ProcedureSurgeonDetails procSurgeon = new ProcedureSurgeonDetails();
					procSurgeon.Name = lblField.Text;
					procSurgeon.SurgeonID = ATlist[0].ItemID;
					procSurgeon.ProcID = procedureDetails.ID;
					if(selectedSurgeon.Count >0)
						procSurgeon.ProcSurgeonID = selectedSurgeon[0].ProcSurgeonID;
					else
						procSurgeon.ProcSurgeonID = 0;
					AddUpdateProcedureSurgeon(procSurgeon);
					ATlist.Clear();


				}
			};
		}

		public List<CodePickerModel> SetSurgeonDataSource(List<SurgeonDetails> listOfSurgeons,out int wvalue)
		{
			wvalue=0;
			CodePickerModel item;
			int pcharcount = 0;
			List<CodePickerModel> dlist = new List<CodePickerModel> ();
			foreach (SurgeonDetails surgeon in listOfSurgeons) {
				item = new CodePickerModel ();
				item.ItemID = surgeon.ID;
				item.ItemText = surgeon.FirstName + " " + surgeon.LastName;
				dlist.Add (item);
				if (pcharcount < item.ItemText.Length) {
					pcharcount = item.ItemText.Length;
					wvalue = MeasureTextSize (item.ItemText);
				}
				item = null;
			}
			return dlist;
		}

		public async void AddUpdateProcedureSurgeon(ProcedureSurgeonDetails procSurgeon)
		{
			ReceiveContext context=	await AppDelegate.Current.pqrsMgr.AddUpdateProcedureSurgeon(procSurgeon);
			if (context != null && context.status != null && context.status.ToUpper () == "OK") {
				UpdateProcedureStatus ();
				if(selectedSurgeon.Count > 0)
					selectedSurgeon.RemoveAt (0);
				
				selectedSurgeon.Add(procSurgeon);
				Console.WriteLine ("Surgeon: "+procSurgeon.Name + " saved successfully to procedure: "+ procSurgeon.ProcID.ToString() );
			} else {
				Console.WriteLine ("Surgeon: "+procSurgeon.Name + " failed to assign to procedure: "+ procSurgeon.ProcID.ToString() );			
			}
		}
		List<QMValidation> IsRequiredlabels=new List<QMValidation>();
		List<UILabel>  qmDisablelbl=new List<UILabel>();
		public void Setenableanddiablelbl(bool status)
		{
			foreach (var item in qmDisablelbl) {
				item.UserInteractionEnabled = status;
			}
		}
		public void ViewQualityMetrics(List<iProPQRSPortableLib.Type> Types,List<iProPQRSPortableLib.Option> DropdownOption, string HeaderTitle)
		{
			float hUVB=200;
			UIView finalView=new UIView();
			UIView uvBlock = new UIView ();
			UIImageView titleimg = new UIImageView (new CoreGraphics.CGRect (0, 0, 992, 40));
			titleimg.Image=UIImage.FromFile(@"headerBarTall.png");
			UILabel lbltitle = new UILabel (new CoreGraphics.CGRect (8, 8, 140, 21));
			lbltitle.Text = HeaderTitle;
			uvBlock.Add (titleimg);
			uvBlock.Add (lbltitle);
			float yuvc = 50;
			UIView uvcontrol;
			//mainrootview.RootData.Sort((x,y)=> x.ItemID.CompareTo(y.ItemID));
			Types.Sort((xx,yy)=> xx.Priority.CompareTo(yy.Priority));
			for (int i = 0; i < Types.Count; i++) {

				List<iProPQRSPortableLib.Option> masterMainList = new List<iProPQRSPortableLib.Option>();
				List<iProPQRSPortableLib.Option> masterSubCatList = new List<iProPQRSPortableLib.Option>();


				if (!string.IsNullOrEmpty (Types [i].Label)) {
					uvcontrol = new UIView (new CoreGraphics.CGRect (0, yuvc, 992, 85));				        
					UILabel lblname = new UILabel (new CoreGraphics.CGRect (20, 8, 480, 41));
					lblname.Lines = 3;
					lblname.Text = Types [i].Label;
					string selectedtext = string.Empty;
					string selectedid = string.Empty;
					string selectedsubitemtext = string.Empty;
					string Attriblabel = string.Empty;
					int AttribTypeID=0;

					if (iProPQRSPortableLib.Consts.SelectedProcAttribtslist != null) {						
						var selecteditem = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.Where (u => u.ProcAttribTypeID == Types [i].ProcAttribTypeID ).SingleOrDefault ();
						if (selecteditem != null) {
							AttribTypeID = selecteditem.ProcAttribTypeID;
							Attriblabel = selecteditem.Value;
							string[] arrytxt;
							//var ditem = DropdownOption.Where (u => u.Value == selecteditem.Value && u.ProcAttribTypeID == selecteditem.ProcAttribTypeID).ToList ();
							arrytxt = DropdownOption.Where (u => u.Value == selecteditem.Value && u.ProcAttribTypeID == selecteditem.ProcAttribTypeID).Select (s => s.Description).ToArray ();
							if (arrytxt != null && arrytxt.Length > 0) {
								selectedtext = arrytxt [0];
							}
						}
					}
					int mlpopid = 0;
					int TypeItemID;
					string TypeValue;
					if (Types [i].ProcAttribTypeID == 606) {
						string str = "";
					}
					if (Types [i].ProcAttribTypeID == 605) {
						masterMainList = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.FindAll (x => x.ProcAttribTypeID == 686);
						masterSubCatList = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.FindAll (x => x.ProcAttribTypeID == 605);
						mlpopid = 686;
						if (masterMainList.Count > 1) {
							TypeItemID=masterMainList[1].ProcAttribTypeID;
							TypeValue=masterMainList[1].Value; 
						}


					} else if (Types [i].ProcAttribTypeID == 607) {
						mlpopid = 685;
						masterMainList = ASA8MasterList;
						masterSubCatList = ASA8SubMasterList;
						if (masterMainList.Count > 1) {
							TypeItemID=masterMainList[1].ProcAttribTypeID;
							TypeValue=masterMainList[1].Value; 
						}

					}
					else if (Types [i].ProcAttribTypeID == 656) {
						mlpopid = 687;
						masterMainList = ASA9MasterList;
						masterSubCatList = ASA9SubMasterList;
						if (masterMainList.Count > 1) {
							TypeItemID=masterMainList[0].ProcAttribTypeID;
							TypeValue=masterMainList[0].Value; 
						}

					}
					else {
						mlpopid = Types [i].ProcAttribTypeID ;
						masterMainList = DropdownOption.FindAll (u => u.ProcAttribTypeID == Types [i].ProcAttribTypeID);
					}
					if (Types [i].ProcAttribTypeID == 605 || Types [i].ProcAttribTypeID == 607 || Types [i].ProcAttribTypeID == 656) {
						//&& Attriblabel=="0582F"
						string str=mlpopid.ToString();
						if (iProPQRSPortableLib.Consts.SelectedProcAttribtslist != null) {
							var	selectedMainListIDs = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (masterMainList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).ToList ();
							if (selectedMainListIDs.Count > 0) {
								string[] arrytxt;
								arrytxt = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.Where (u => u.Value == selectedMainListIDs [0].Value && u.ProcAttribTypeID == selectedMainListIDs [0].ProcAttribTypeID).Select (s => s.Description).ToArray ();
								if (arrytxt.Length > 0)
									selectedtext = arrytxt [0];
							}
							var selectedSubCatListIDs = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (masterSubCatList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).ToList ();
							if (selectedSubCatListIDs.Count > 0) {
								selectedsubitemtext = selectedsubitemtext + "(";
								foreach (var SIT in selectedSubCatListIDs) {
									string[] arrytxt;
									arrytxt = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.Where (u => u.Value == SIT.Value && u.ProcAttribTypeID == SIT.ProcAttribTypeID).Select (s => s.Description).ToArray ();
									if (arrytxt.Length > 0)
										selectedsubitemtext = selectedsubitemtext + arrytxt [0] + ",";
								}
								selectedsubitemtext = selectedsubitemtext.TrimEnd (',') + " )";
								//0582F  "0582F"
								if (selectedMainListIDs.Count > 0 && selectedMainListIDs [0].Value == TypeValue)
									selectedtext = selectedtext + selectedsubitemtext;
							}
						}
					}
					if (Types [i].ProcAttribTypeID == 606) {
						string lbltext = string.Empty;
						if (iProPQRSPortableLib.Consts.SelectedProcAttribtslist != null) {
							var	ASA7MasterList = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.FindAll (x => x.ProcAttribTypeID == 606);
							var ASA7N1MasterList = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.FindAll (x => x.ProcAttribTypeID == 683);
							var ASA7N2MasterList = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.FindAll (x => x.ProcAttribTypeID == 684);

							var selectedMasterASA7OptionsIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (ASA7MasterList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).ToList ();
							var selectedMasterASA7N1OptionsIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (ASA7N1MasterList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).ToList ();
							var selectedMasterASA7N2OptionsIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (ASA7N2MasterList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).ToList ();
							foreach (var item in selectedMasterASA7N1OptionsIds) {
								Option opitem = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.Find (u => u.ProcAttribTypeID == item.ProcAttribTypeID && u.Value == item.Value);
								if (opitem != null) {
									lbltext = lbltext + opitem.Description + ", ";
								}
							}
							foreach (var item in selectedMasterASA7N2OptionsIds) {
								Option opitem = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.Find (u => u.ProcAttribTypeID == item.ProcAttribTypeID && u.Value == item.Value);
								if (opitem != null) {
									lbltext = lbltext + opitem.Description + ", ";
								}
							}
							foreach (var item in selectedMasterASA7OptionsIds) {
								Option opitem = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.Find (u => u.ProcAttribTypeID == item.ProcAttribTypeID && u.Value == item.Value);
								if (opitem != null) {
									lbltext = lbltext + opitem.Description + ",";
								}
							}
							selectedtext = lbltext;
						}
					}

					if (Types [i].ProcAttribTypeID == 607) {
						string lbltext = string.Empty;
						if (iProPQRSPortableLib.Consts.SelectedProcAttribtslist != null) {
							var	ASA8MasterList1 = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.FindAll (x => x.ProcAttribTypeID == 685);
							var ASA8MasterList2 = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.FindAll (x => x.ProcAttribTypeID == 1001);
							var ASA8SubList = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.FindAll (x => x.ProcAttribTypeID == 607);

							var selectedASA8MasterList1OptionsIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (ASA8MasterList1.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).ToList ();
							var selectedASA8MasterList2OptionsIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (ASA8MasterList2.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).ToList ();
							var selectedMasterSubListOptionsIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (ASA8SubList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).ToList ();
							foreach (var item in selectedASA8MasterList1OptionsIds) {
								Option opitem = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.Find (u => u.ProcAttribTypeID == item.ProcAttribTypeID && u.Value == item.Value);
								if (opitem != null) {
									lbltext = lbltext + opitem.Description + ", ";
								}
							}
							foreach (var item in selectedASA8MasterList2OptionsIds) {
								Option opitem = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.Find (u => u.ProcAttribTypeID == item.ProcAttribTypeID && u.Value == item.Value);
								if (opitem != null) {
									lbltext = lbltext + opitem.Description + ", ";
								}
							}
							foreach (var item in selectedMasterSubListOptionsIds) {
								Option opitem = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.Find (u => u.ProcAttribTypeID == item.ProcAttribTypeID && u.Value == item.Value);
								if (opitem != null) {
									lbltext = lbltext + opitem.Description + ",";
								}
							}
							selectedtext = lbltext;
						}
					}

					//if (masterMainList.Count > 0)
					//	mlpopid = Types [i].ProcAttribTypeID;//masterMainList [0].ProcAttribTypeID;
					int tempAttribTypeID = Types [i].ProcAttribTypeID;

					UILabel lblDesc = new UILabel ();
					lblDesc.Lines = 3;
					if (AttribTypeID == 608) {
						if (selectedtext.Contains ("NO Serious adverse event"))
							lblDesc.Text = selectedtext;
						else {
							string finaltext = string.Empty;
							foreach (var sitem in selectednonPqrsTypeAS10OptionsIds) {
								//nonPqrsTypeAS10Options
								var itemtype = nonPqrsTypeAS10Options.Where (u => u.ProcAttribTypeID == sitem).ToList ();
								if (itemtype.Count > 0)
									finaltext = finaltext + itemtype [0].Label + ",";
							}
							if (!string.IsNullOrEmpty (finaltext))
								lblDesc.Text = selectedtext + " ( " + finaltext.TrimEnd (',') + " )";
							else
								lblDesc.Text = selectedtext;
						}
					}
					else
						lblDesc.Text = selectedtext;

					int descwidth = 40;
					lblDesc.Lines = 1;
					if (lblDesc.Text != null) {
						int textcount = lblDesc.Text.Length;
						if (textcount < 58) {
							descwidth = 40;
							lblDesc.Lines = 1;
						} else if (textcount > 58 && textcount <= 114) {
							descwidth = 55;
							lblDesc.Lines = 2;
						} else if (textcount > 114) {
							descwidth = 75;
							lblDesc.Lines = 3;
						}
					}

					lblDesc.Frame = new CoreGraphics.CGRect (500, 8, 480, descwidth);
					if (mlpopid == 608 || mlpopid==624 || mlpopid==606 || mlpopid==615 ) {
						uvcontrol.Frame = new CoreGraphics.CGRect (0, yuvc, 992, 100);
						//lblDesc.Frame = new CoreGraphics.CGRect (500, 0, 480, descwidth);
						yuvc = yuvc + 15;
					}
					lblDesc.Layer.BorderColor = UIColor.Gray.CGColor;
					lblDesc.Layer.BorderWidth = (nfloat)0.5;
					lblDesc.Layer.CornerRadius = (nfloat)8.0; 
					UILabel lblhidenmaincatval = new UILabel ();
					lblhidenmaincatval.Hidden = true;
					lblhidenmaincatval.Text = Attriblabel;
					lblhidenmaincatval.Tag = AttribTypeID;

					UILabel lblhidensubcatval = new UILabel ();
					lblhidensubcatval.Hidden = true;
					UITapGestureRecognizer lblDescTap = new UITapGestureRecognizer( async()  => {
						List<Tuple<string, string>>  selectedrootitem=new List<Tuple<string, string>>();
						if(lblhidenmaincatval.Tag != 0)
							selectedrootitem.Add(new Tuple<string,string>(lblhidenmaincatval.Tag.ToString(),lblhidenmaincatval.Text));//,);
						//string name=Types [i].Label;

						if(mlpopid== 608)
						{
							QualityMetricsASA10 qmasafrm=new QualityMetricsASA10(lblDesc);
							qmasafrm.masterMainList=masterMainList;
							qmasafrm.masterSubCatList=nonPqrsTypeAS10Options;
							if(procedureDetails != null && procedureDetails.ID != 0)
								qmasafrm.ProcID=procedureDetails.ID ;

							qmasafrm.PresentFromPopover(lblDesc, (float)lblDesc.Frame.X, (float)lblDesc.Frame.Y);

							//if(masterMainList.Count>0)
							//	BindmultilevelPopupover(masterMainList,selectedrootitem,selectednonPqrsTypeAS10OptionsIds,lblDesc,(int)lblDesc.Frame.Y,lblhidenmaincatval,true);
						}
						else if(mlpopid== 686 || mlpopid== 685 || mlpopid== 687)
						{ //605=686
							QualityMetricsASA qmasafrm=new QualityMetricsASA(lblDesc);
							qmasafrm.masterMainList=masterMainList;
							qmasafrm.masterSubCatList=masterSubCatList;
							qmasafrm._ValueChanged += delegate {
								if (lblDesc.Text.Trim () != string.Empty){
									foreach (QMValidation item in IsRequiredlabels) {
										if(item.proctAttribTypeID == 606){
											item.lbldesc.Layer.BorderColor = UIColor.Gray.CGColor;
											item.lbldesc.Layer.BorderWidth = 1;
										}
									}
									QMValidation itemToRemove = this.IsRequiredlabels.Find(x => x.proctAttribTypeID == 606 && x.lblname.Trim().ToLower() == "non pqrs");
									asa7ItemRemovedPrev = itemToRemove;
									this.IsRequiredlabels.Remove (itemToRemove);
								}else{
									if(asa7ItemRemovedPrev != null){
										QMValidation searchItem = IsRequiredlabels.Find(x => x.proctAttribTypeID == asa7ItemRemovedPrev.proctAttribTypeID);
										if(searchItem == null){
											IsRequiredlabels.Add(asa7ItemRemovedPrev);
											foreach (QMValidation item in IsRequiredlabels) {
												if(item.proctAttribTypeID == 606){
													item.lbldesc.Layer.BorderColor = UIColor.Red.CGColor;
													item.lbldesc.Layer.BorderWidth = 1;
												}
											}
										}
									}else{
										QMValidation searchItem = IsRequiredlabels.Find(x => x.proctAttribTypeID == 606);
										if(searchItem == null){
											IsRequiredlabels.Add(asa8ItemRemovedPrev);
											foreach (QMValidation item in IsRequiredlabels) {
												if(item.proctAttribTypeID == 606){
													item.lbldesc.Layer.BorderColor = UIColor.Red.CGColor;
													item.lbldesc.Layer.BorderWidth = 1;
												}
											}
										}
									}

								}
							};
							if(procedureDetails != null && procedureDetails.ID != 0)
								qmasafrm.ProcID=procedureDetails.ID ;

							qmasafrm.PresentFromPopover(lblDesc, (float)lblDesc.Frame.X, (float)lblDesc.Frame.Y);

							//if(masterMainList.Count>0)
							//	BindmultilevelPopupover(masterMainList,masterSubCatList,lblDesc,(int)lblDesc.Frame.Y,lblhidenmaincatval,lblhidensubcatval,false,TypeItemID,TypeValue);
						}else if(mlpopid == 606){
							QualityMetricsForm qmFrm = new QualityMetricsForm(lblDesc);
							qmFrm._ValueChanged += delegate {
								if (lblDesc.Text.Trim () != string.Empty) {
									foreach (QMValidation item in IsRequiredlabels) {
										if(item.proctAttribTypeID == 607){
											item.lbldesc.Layer.BorderColor = UIColor.Gray.CGColor;
											item.lbldesc.Layer.BorderWidth = 1;
										}
									}
									QMValidation itemToRemove = this.IsRequiredlabels.Find(x => x.proctAttribTypeID == 607 && x.lblname.Trim().ToLower() == "non pqrs");
									asa8ItemRemovedPrev = itemToRemove;
									this.IsRequiredlabels.Remove (itemToRemove);
								}else{
									if(asa8ItemRemovedPrev != null){
										QMValidation searchItem = IsRequiredlabels.Find(x => x.proctAttribTypeID == asa8ItemRemovedPrev.proctAttribTypeID);
										if(searchItem == null){
											IsRequiredlabels.Add(asa8ItemRemovedPrev);
											foreach (QMValidation item in IsRequiredlabels) {
												if(item.proctAttribTypeID == 607){
													item.lbldesc.Layer.BorderColor = UIColor.Red.CGColor;
													item.lbldesc.Layer.BorderWidth = 1;
												}
											}
										}
									}else{
										QMValidation searchItem = IsRequiredlabels.Find(x => x.proctAttribTypeID == 607);
										if(searchItem == null){
											IsRequiredlabels.Add(asa8ItemRemovedPrev);
											foreach (QMValidation item in IsRequiredlabels) {
												if(item.proctAttribTypeID == 607){
													item.lbldesc.Layer.BorderColor = UIColor.Red.CGColor;
													item.lbldesc.Layer.BorderWidth = 1;
												}
											}
										}
									}
								}
							};

							if(procedureDetails != null && procedureDetails.ID != 0)
								qmFrm.ProcID=procedureDetails.ID ;
							qmFrm.PresentFromPopover(lblDesc, (float)lblDesc.Frame.X, (float)lblDesc.Frame.Y);
						}else{
							if(masterMainList.Count>0)
								qmBindPopupover(masterMainList,lblDesc,(int)lblDesc.Frame.Y,tempAttribTypeID);
						}
						int ProcID=0;
						if(procedureDetails != null && procedureDetails.ID != 0)
							ProcID = procedureDetails.ID;
						iProPQRSPortableLib.Consts.SelectedProcAttribtslist  = await AppDelegate.Current.pqrsMgr.GetAllAttribTypesOfAProcedure(ProcID);

						//IsRequiredlabels


						

					});
					lblDesc.UserInteractionEnabled = true;
					lblDesc.AddGestureRecognizer(lblDescTap);


					//UIButton btncontrol = new UIButton (new CoreGraphics.CGRect (550, 8, 319, 30));
					//if(!string.IsNullOrEmpty(selectedtext))
					//	btncontrol.SetTitle (selectedtext, UIControlState.Normal);
					//btncontrol.SetBackgroundImage (UIImage.FromFile (@"textBoxDropDown.png"), UIControlState.Normal);
					//btncontrol.SetTitleColor (UIColor.Black, UIControlState.Normal);

					//btncontrol.TouchUpInside += async (object sender, EventArgs e) => {
					//	List<string>  selectedi=new List<string>{"selected"};
					//string name=Types [i].Label;
					//	if(AttribTypeID== 608)
					//{
					//if(Dropdownlist.Count>0)
					//BindmultilevelPopupover(Dropdownlist,selectedi,btncontrol,(int)btncontrol.Frame.Y,"name");
					//}
					//else
					//{
					//if(Dropdownlist.Count>0)
					//	BindPopupover(Dropdownlist,selectedi,btncontrol,(int)btncontrol.Frame.Y,"name");
					//}
					//};
					//btncontrol.SetTitle (" btn Name " + i, UIControlState.Normal);

					if (Types [i].IsRequired) {
//						lblDesc.Layer.BorderColor = UIColor.FromRGB (255, 102, 102).CGColor;
						lblDesc.Layer.BorderColor = UIColor.Red.CGColor;
						lblDesc.Layer.BorderWidth = 1;
						QMValidation v=new QMValidation();
						v.lbldesc = lblDesc;
						v.lblname = HeaderTitle;
						v.proctAttribTypeID = Types [i].ProcAttribTypeID;
						IsRequiredlabels.Add(v);
					}

					uvcontrol.Add (lblname);
					if (Types [i].IsRequired) {
						CoreGraphics.CGRect frm = lblname.Frame;
						frm.Width= lblname.IntrinsicContentSize.Width;
						lblname.Frame = frm;
						UILabel lblstar = new UILabel (new CoreGraphics.CGRect (lblname.IntrinsicContentSize.Width+20, 8, 10, 41));
						lblstar.Text = "*";
						lblstar.TextColor = UIColor.Red;
						uvcontrol.Add (lblstar);
					}
					uvcontrol.Add (lblname);
					uvcontrol.Add (lblDesc);	

					qmDisablelbl.Add (lblDesc);
					uvcontrol.Add (lblhidenmaincatval);
					uvcontrol.Add (lblhidensubcatval);
					yuvc = yuvc + 65;
					uvBlock.Add (uvcontrol);
					uvcontrol = null;
				}
			}
			uvBlock.Frame = new CoreGraphics.CGRect (0, 0, 992, yuvc + 45);
			uvBlock.Layer.BorderColor = UIColor.Gray.CGColor;
			uvBlock.Layer.BorderWidth = 1;
			//new CoreGraphics.CGRect(xUV,0,992,hUV
			hUV = yuvc + 45;
			finalView.Frame = new CoreGraphics.CGRect (0,yUV,992, hUV);
			finalView.BackgroundColor = UIColor.White;
			yUV = yUV+hUV+5;
			finalView.Add (uvBlock);
			svQualityMetrics.Add(finalView);

			QMValidation asa7Item = IsRequiredlabels.Find(x => x.proctAttribTypeID == 606);
			QMValidation asa8Item = IsRequiredlabels.Find(x => x.proctAttribTypeID == 607);
			QMValidation asa7ItemToRemove = new QMValidation();
			QMValidation asa8ItemToRemove = new QMValidation();
			foreach (QMValidation item in IsRequiredlabels) {
				if (item.proctAttribTypeID != 0 && item.proctAttribTypeID == 606) {
					if (asa8Item != null) {
						if (asa8Item.lbldesc.Text.Trim () != string.Empty) {
							item.lbldesc.Layer.BorderColor = UIColor.Gray.CGColor;
							item.lbldesc.Layer.BorderWidth = 1;
							asa7ItemToRemove = item;
						}
					}
				}
				if (item.proctAttribTypeID != 0 && item.proctAttribTypeID == 607) {
					if (asa7Item != null) {
						if (asa7Item.lbldesc.Text.Trim () != string.Empty) {
							item.lbldesc.Layer.BorderColor = UIColor.Gray.CGColor;
							item.lbldesc.Layer.BorderWidth = 1;
							asa8ItemToRemove = item;
						}
					}
				}
			}
			if (asa7ItemToRemove.lblname != string.Empty) {
				asa7ItemRemovedPrev = asa7ItemToRemove;
				QMValidation itemToRemove = this.IsRequiredlabels.Find(x => x.proctAttribTypeID == asa7ItemRemovedPrev.proctAttribTypeID && x.lblname.Trim().ToLower() == asa7ItemRemovedPrev.lblname.Trim().ToLower());
				if(itemToRemove != null && itemToRemove.proctAttribTypeID != 0)
					IsRequiredlabels.Remove (itemToRemove);
			}

			if (asa8ItemToRemove.lblname != string.Empty) {
				asa8ItemRemovedPrev = asa8ItemToRemove;
				QMValidation itemToRemove = this.IsRequiredlabels.Find(x => x.proctAttribTypeID == asa8ItemRemovedPrev.proctAttribTypeID && x.lblname.Trim().ToLower() == asa8ItemRemovedPrev.lblname.Trim().ToLower());
				if(itemToRemove != null && itemToRemove.proctAttribTypeID != 0)
					IsRequiredlabels.Remove (itemToRemove);
			}

		}

		public void BindPopupover(List<iProPQRSPortableLib.Option> items,List<string> Selecteditems,UIButton btnField,int FrameY,string labelName)
		{
			int uvwidth;
			ATlist.Clear ();
			List<CodePickerModel> alist = SetTypeDataSource(items, out  uvwidth);

			mcp = new mCodePicker(this,uvwidth);
			mcp.TypeOfList = "Options";



			if (ATlist.Count > 0)
				mcp.SelectedItems = ATlist;


			float x = 10;//(float)btnField.Frame.X;
			float y = 10;//(float)btnField.Frame.Y+FrameY;

			mcp.PresentFromPopover(btnField, x, y,uvwidth);
			mcp.mDataSource(alist);
			mcp._ValueChanged += async delegate
			{
				ATlist = mcp.SelectedItems;
				string finalText = " ";

				List<AttribType>  procalist=new List<AttribType>();
				AttribType procitem; 
				int ProcID=0;
				if(procedureDetails != null && procedureDetails.ID != 0)
					ProcID = procedureDetails.ID;

				foreach (var item in ATlist)
				{					
					procitem = new AttribType();
					procitem.ProcAttribTypeID = item.ItemID;
					procitem.ProcID = ProcID;
					procitem.Value = item.ItemCode;
					procitem.IsHighLighted = false;
					procalist.Add(procitem);
					procitem = null;
					finalText = item.ItemText;
				}
				btnField.SetTitle(finalText.Trim(),UIControlState.Normal);;

				foreach (var sitem in Selecteditems) {
					string spid = ATlist.Where(w=>w.ItemCode == sitem).Select(s=>s.ItemCode).SingleOrDefault();
					if(spid == string.Empty)
					{
						procitem=new AttribType();
						procitem.ProcAttribTypeID = items[0].ProcAttribTypeID;
						procitem.ProcID = ProcID;
						procitem.Value = "";
						procitem.IsHighLighted=false;
						procalist.Add(procitem);
						procitem=null;
					}
				}


				if(procalist != null && procalist.Count > 0 ){
					var procAttribtsobject  = await AppDelegate.Current.pqrsMgr.UpdateProcAttribs(procalist);
					if(procAttribtsobject != null && procAttribtsobject.status != null && procAttribtsobject.status.ToUpper() == "OK"){
						Console.WriteLine ("Patient Info. Successfully Saved.");
//						new UIAlertView("Procedure Info", "Successfully Saved.", null, "ok", null).Show();
					}else{
						new UIAlertView("Error", procAttribtsobject.message, null, "ok", null).Show();
					}
				}

				if(labelName == "CVCSterileTech"){
					selectedLineCVCSterileTechIds.Clear();
					foreach (var item in ATlist) {
						selectedLineCVCSterileTechIds.Add(item.ItemCode);
					}
				}

				ATlist.Clear();
			};
		}

		#region Patient Saving
		private async void SavePatientInfo()
		{
			try
			{
			if (validatedata ()) {
				Patient profile = new Patient ();

				if (patientProfile != null)
					profile.ID = patientProfile.ID;
				else
					profile.ID = 0;		

//			if(uSEmergency.On)
//				profile.cbEmergency = "on";
//			else
//				profile.cbEmergency = string.Empty;

				profile.CreatedOn = DateTime.Now.ToShortDateString ();
//			profile.ddlEncounterType=EncounterBtn.TitleLabel.Text;
				profile.LastModifiedDate = DateTime.Now.ToShortDateString ();
				profile.FacilityID = iProPQRSPortableLib.Consts.SelectedFacilityID;
				profile.FirstName = txtpFirstName.Text;
				profile.LastName = txtpLastName.Text;
				profile.DOB = txtpDOB.Text;	
				profile.MRN = txtpmrn.Text;
			    if(!string.IsNullOrEmpty(btnPatientGender.CurrentTitle))
				   profile.Sex = btnPatientGender.CurrentTitle.Substring(0,1);

//			switch (PhysicalStatusBtn.TitleLabel.Text) {
//			case "ASA 1":
//				profile.ddlASAType = "1";
//				break;
//			case "ASA 2":
//				profile.ddlASAType = "2";
//				break;
//			case "ASA 3":
//				profile.ddlASAType = "3";
//				break;
//			case "ASA 4":
//				profile.ddlASAType = "4";
//				break;
//			case "ASA 5":
//				profile.ddlASAType = "5";
//				break;
//			case "ASA 6":
//				profile.ddlASAType = "6";
//				break;
//			}

				ReceiveContext result1 = await AppDelegate.Current.pqrsMgr.AddUpdatePatintInfo (profile);
					if(result1 != null && result1.result != null && result1.status !=null && result1.status.ToUpper() =="OK" ) 
					{
						Patient patientDetails = (Patient)JsonConvert.DeserializeObject(result1.result.ToString() , typeof(Patient));
						patientProfile = patientDetails;
						Patient piResult = null;
						if (string.IsNullOrEmpty (result1.message)) {
							Console.WriteLine ("Patient Info. Successfully Saved.");	
		//					new UIAlertView ("Patient Info", "Successfully Saved."
		//					, null, "ok", null).Show ();

							EncounterBtn.Enabled = true;
							PhysicalStatusBtn.Enabled = true;
							uSEmergency.Enabled = true;
							addNewDiagnosis.Enabled = true;
							deleteDiagnosis.Enabled = true;
							removeProc.Enabled = true;
							addNewProc.Enabled = true;

								lblFullName.Text = patientDetails.LastName.Trim() + ", " + patientDetails.FirstName.Trim();
								mrnLbl.Text = "MRN: " + patientDetails.MRN.Trim();
								lblFullName.Hidden=false;
								mrnLbl.Hidden=false;
							#region Procedure Creation
							if (procedureDetails == null) {
								procedureDetails = new PatientProcedureFullDetails ();
								procedureDetails.ID = 0;
								procedureDetails.Mrn = patientProfile.MRN;
								procedureDetails.PatientID = patientProfile.ID.ToString();						
								procedureDetails.StatusID = "1";
								
								procedureDetails.OperationDate = tctSurgeryDate.Text;
								procedureDetails.ListType = "MAIN";
								procedureDetails.Location =  iProPQRSPortableLib.Consts.SelectedFacilityID;


								ReceiveContext result = await AppDelegate.Current.pqrsMgr.UpdatePatintProcedureInfo (procedureDetails);
								if (result != null && result.result != null) 
									procedureDetails = (PatientProcedureFullDetails)JsonConvert.DeserializeObject (result.result.ToString (), typeof(PatientProcedureFullDetails));

								lblProcID.Text = "Proc ID: " + procedureDetails.ID;
									btnSubmit.Hidden=false;
								if (this.procedureDetails.StatusID == "1" || this.procedureDetails.StatusID == "2")
										btnSubmit.SetTitle (" Finalize", UIControlState.Normal);
									
								if(string.IsNullOrEmpty(result.message) &&  result.status!=null && result.status.ToUpper().Trim() == "OK" ){
									Console.WriteLine("Procedure Info. Successfully Saved.");	
								}
							}
							#endregion Procedure Creation
						} else {
							new UIAlertView ("Patient eroor", result1.message
							, null, "ok", null).Show ();
						}
					}
					else
						NavigationController.PopToRootViewController(true);
					
			}
			}
			catch {
				string str = "";
			}
		}
		#endregion
	}
	public class QMValidation
	{
		public UILabel lbldesc {get;set;}
		public string lblname {get;set;}
		public int proctAttribTypeID {get;set;}

	}
}


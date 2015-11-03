
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

namespace iProPQRS
{
	public partial class AddPatientViewController : UIViewController
	{
		PQRSServices serv=new PQRSServices();
		Patient patientProfile;
		int physicalStatus;
		int encounterTypeid;
		ProcedureDiagnosticMaster lastSelectedDiagnosisObj;
		ProcedureDiagnosticMaster lastSelectedProceduresObj;
		PatientProcedureDetails patientProcedureDetails;
		ProcedureDiagnosticMaster procedureItems;
		CodePicker cp;
		mCodePicker mcp;
		List<CodePickerModel> ATlist=new List<CodePickerModel>();
		string[] anesTechItems;

		int baseYDiagnosisDescriptionValue = 8;
		int baseYDiagnosisCodeValue = 8;
		int baseYProcDescriptionValue = 8;
		int baseYProcCodeValue = 8;
		bool showKeyBoard = false;

		private UIView activeView;
		private nfloat scroll_amount = 0.0f;   //amount to scroll
		private nfloat bottom = 0.0f;          //bottom point
		private nfloat offset = 100.0f;         //extra offset
		private bool moveViewUp = false;      //direction up or down

		List<DropDownModel> dEncounterTypelist;
		List<DropDownModel> dlPhyStatusList;

		public AddPatientViewController () : base ("AddPatientViewController", null)
		{
		}

		public string Facility {
			get;
			set;
		}
		public AddPatientViewController (PatientProcedureDetails patientProcedureDetails) : base ("AddPatientViewController", null)
		{
			this.patientProcedureDetails=patientProcedureDetails;
		}
		public AddPatientViewController (Patient patientProfile,PatientProcedureDetails patientProcedureDetails) : base ("AddPatientViewController", null)
		{
			this.patientProcedureDetails = patientProcedureDetails;
			this.patientProfile=patientProfile;
		}


		string[] ET="InPatient , OutPatient , ASC Encounter , Office Based".Split(',');
		string[] PS="ASA 1,ASA 2,ASA 3,ASA 4,ASA 5,ASA 6".Split(','); 
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
			SetBorderColor (uvProviders);
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

			DropDownViewController ddlbtn;// = new DropDownViewController (this);
			EncounterBtn.TouchUpInside += (object sender, EventArgs e) =>  {
				ddlbtn = new DropDownViewController (this);
				ddlbtn.SelectedValue=encounterTypeid;
				ddlbtn.DataSource = dEncounterTypelist;
				ddlbtn.PresentFromPopover (EncounterBtn);
				ddlbtn._Change += delegate {
					EncounterBtn.SetTitle (ddlbtn.SelectedText, UIControlState.Normal);
					encounterTypeid = ddlbtn.SelectedValue;
				};
			};

			PhysicalStatusBtn.TouchUpInside += (object sender, EventArgs e) => {
				ddlbtn = new DropDownViewController (this);
				ddlbtn.SelectedValue = physicalStatus;
				ddlbtn.DataSource = dlPhyStatusList;
				ddlbtn.PresentFromPopover (PhysicalStatusBtn);
				ddlbtn._Change += delegate {
					PhysicalStatusBtn.SetTitle (ddlbtn.SelectedText, UIControlState.Normal);
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
			
			if(!Isvalidate)
			 new UIAlertView("Validation Error", emsg, null, "ok", null).Show();
			
			return Isvalidate;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			ProcAttribTypes ();
			txtAnesthesiaTechs.EditingDidBegin += (object sender, EventArgs e) => {
				BindAnesthesiaTechniques(anesTechItems);
			};
			tctSurgeryDate.Enabled = false;
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
			sCtab.ValueChanged += (object sender, EventArgs e) =>  {
				uvMainPatientinfo.RemoveFromSuperview ();
				uvQualityMetrics.RemoveFromSuperview ();
				uvBillingInfo.RemoveFromSuperview ();
				if (sCtab.SelectedSegment == 0)
					uvMain.AddSubview (uvPatientInfo);
				else
					if (sCtab.SelectedSegment == 2)
						uvMain.AddSubview (uvQualityMetrics);
					else
						if (sCtab.SelectedSegment == 1)
							uvMain.AddSubview (uvBillingInfo);
			};
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

			uvScrollview.ContentSize = new CoreGraphics.CGSize (0, 1450);
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
				lblFullName.Text = patientProfile.FirstName.Trim() + ", " + patientProfile.LastName.Trim();
				mrnLbl.Text = "MRN: " + patientProfile.MRN.Trim();
				txtpmrn.Text = patientProfile.MRN;
				txtpFirstName.Text = patientProfile.FirstName;
				txtpLastName.Text = patientProfile.LastName;
				string tempDOB;

				if (!string.IsNullOrEmpty (patientProfile.DOB)) {
					tempDOB = Convert.ToDateTime (patientProfile.DOB).ToString ("MM/dd/yyyy");
					txtpDOB.Text = tempDOB;

					int age = DateTime.Now.Year - Convert.ToDateTime (patientProfile.DOB).Year;
					txtpAge.Text = age.ToString ();
				}

				string surgeryDate;
				if (!string.IsNullOrEmpty (patientProcedureDetails.OperationDate))
					surgeryDate = Convert.ToDateTime (patientProcedureDetails.OperationDate).ToString ("MM/dd/yyyy");
				tctSurgeryDate.Text = surgeryDate;

				if(patientProfile.DiagnosticList!=null && patientProfile.DiagnosticList.Count>0){
					foreach (var item in patientProfile.DiagnosticList) {
						NewDiagnosis (item.Code, item.Name);
					}
				}

				if(patientProfile.ProcedureList != null && patientProfile.ProcedureList.Count > 0){
					foreach (var item in patientProfile.ProcedureList) {
						AddProcedure (item.Code, item.Name);
					}
				}
			}

			tctSurgeryDate.EditingDidBegin+= (object sender, EventArgs e) => {
				DatePicker  dp=new DatePicker();
				CoreGraphics.CGRect f=tctSurgeryDate.Frame;
				dp.PresentFromPopover(this.View,float.Parse(f.X.ToString()),float.Parse(f.Y.ToString())+100);
				dp._ValueChanged += delegate {	
					int age = DateTime.Now.Year - dp.SelectedDateValue.Year;
					//this.mPatient.DOB = dp.SelectedDate;
					tctSurgeryDate.Text=dp.SelectedDate;
				};
			};
			txtpDOB.EditingDidBegin+= (object sender, EventArgs e) => {
				DatePicker  dp=new DatePicker();
				CoreGraphics.CGRect f=txtpDOB.Frame;
				dp.PresentFromPopover(this.View,float.Parse(f.X.ToString()),float.Parse(f.Y.ToString())+100);
				dp._ValueChanged += delegate {	
					int age = DateTime.Now.Year - dp.SelectedDateValue.Year;
					//this.mPatient.DOB = dp.SelectedDate;
					txtpDOB.Text=dp.SelectedDate;
					txtpAge.Text=age.ToString();
				};
			};
			txtpmrn.EditingDidEnd+= async (object sender, EventArgs e) => {
				if(!string.IsNullOrEmpty(txtpmrn.Text))
				{
					AppDelegate.pb.Start(this.View,"Please wait...");
					ReceiveContext context = new ReceiveContext();
					context = await serv.CheckExistingPatintInfo(txtpmrn.Text,iProPQRSPortableLib.Consts.SelectedFacilityID);				
					if(context.result!=null)
					{
						Patient patientDetails = (Patient)JsonConvert.DeserializeObject(context.result.ToString() , typeof(Patient));

						if(patientDetails!=null){						

							lblFullName.Text = patientDetails.FirstName.Trim() + ", " + patientDetails.LastName.Trim();
							mrnLbl.Text = "MRN: " + patientDetails.MRN.Trim();

							txtpmrn.Text = patientDetails.MRN;
							txtpFirstName.Text = patientDetails.FirstName;
							txtpLastName.Text = patientDetails.LastName;

							txtpDOB.Text = Convert.ToDateTime (patientDetails.DOB).ToString ("MM/dd/yyyy");

							int age = DateTime.Now.Year - Convert.ToDateTime (patientDetails.DOB).Year;
							txtpAge.Text = age.ToString ();

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

						}else{
							//	txtpmrn.Text = "";
							txtpFirstName.Text = "";
							txtpLastName.Text = "";
							txtpDOB.Text = "";
							mrnLbl.Text="";
							lblFullName.Text="";
						}
						AppDelegate.pb.Stop();
					}
					else{
						//	txtpmrn.Text = "";
						txtpFirstName.Text = "";
						txtpLastName.Text = "";
						txtpDOB.Text = "";
						mrnLbl.Text="";
						lblFullName.Text="";
					}
				}
				AppDelegate.pb.Stop();
			};

			btnSubmit.TouchUpInside+= async (object sender, EventArgs e) => {
				if(validatedata())
				{
				AppDelegate.pb.Start(this.View,"Please wait...");
				Patient profile=new Patient();

				if(patientProfile!=null)
					profile.ID=patientProfile.ID;
				else
					profile.ID=0;		


				if(uSEmergency.On)
					profile.cbEmergency = "on";
				else
					profile.cbEmergency = string.Empty;

				profile.CreatedOn=DateTime.Now.ToShortDateString();
				profile.ddlEncounterType=EncounterBtn.TitleLabel.Text;
				profile.LastModifiedDate=DateTime.Now.ToShortDateString();
				profile.FacilityID=iProPQRSPortableLib.Consts.SelectedFacilityID;
				profile.FirstName=txtpFirstName.Text;
				profile.LastName=txtpLastName.Text;
				profile.DOB=txtpDOB.Text;	
				profile.MRN=txtpmrn.Text;

					switch (PhysicalStatusBtn.TitleLabel.Text) {
					case "ASA 1":
						profile.ddlASAType = "1";
						break;
					case "ASA 2":
						profile.ddlASAType = "2";
						break;
					case "ASA 3":
						profile.ddlASAType = "3";
						break;
					case "ASA 4":
						profile.ddlASAType = "4";
						break;
					case "ASA 5":
						profile.ddlASAType = "5";
						break;
					case "ASA 6":
						profile.ddlASAType = "6";
						break;
					}
				ReceiveContext result1=await serv.SavePatintInfo(profile);
				Patient piResult=null;
				if(string.IsNullOrEmpty(result1.message))
				{
					new UIAlertView("Patient Info", "Successfully Saved."
						, null, "ok", null).Show();
				}
				else
				{
					new UIAlertView("Patient eroor", result1.message
						, null, "ok", null).Show();
				}
				AppDelegate.pb.Stop();
			}
			};

			addNewDiagnosis.TouchUpInside += (object sender, EventArgs e) => {
				NewDiagnosis (string.Empty,string.Empty);
			};
			deleteDiagnosis.TouchUpInside += (object sender, EventArgs e) => {
				RemoveDiagnosis();
			};


			addNewProc.TouchUpInside += (object sender, EventArgs e) => {
				AddProcedure(string.Empty,string.Empty);
			};
			removeProc.TouchUpInside += (object sender, EventArgs e) => {
				RemoveProcedures();
			};

			svBillingInfo.SizeToFit ();
			svBillingInfo.ContentSize = new SizeF (float.Parse (svBillingInfo.Frame.Width.ToString ()), float.Parse (svBillingInfo.Frame.Height.ToString ())+150);
		}

		public void ProcAttribTypes()
		{


			/* iProPQRSPortableLib.Category pqrsProcCategory = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Categories.Find(x => x.Name.Trim().ToLower() == "pqrs");

			List<iProPQRSPortableLib.Group> procGroups = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Groups;

			iProPQRSPortableLib.Group pqrsProcGroup = procGroups.Find(x => x.ProcAttribCategoryID == pqrsProcCategory.ProcAttribCategoryID && x.Name == "PQRS");

			List<iProPQRSPortableLib.Type> procTypes = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Types.FindAll (x => x.ProcAttribGroupID == pqrsProcGroup.ProcAttribGroupID);

			iProPQRSPortableLib.Category checkListProcCategory = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Categories.Find(x => x.Name.Trim().ToLower() == "checklist");
			iProPQRSPortableLib.Group checkListProcGroup = procGroups.Find(x => x.ProcAttribCategoryID == checkListProcCategory.ProcAttribCategoryID && x.Name == "Anesthesia Techniques");
			List<iProPQRSPortableLib.Type> checkListTypes = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Types.FindAll (x => x.ProcAttribGroupID == checkListProcGroup.ProcAttribGroupID);

			List<iProPQRSPortableLib.Type> anesthesiaTechniquesTypes = checkListTypes.FindAll (x => x.UIControlType == "cb");
			anesTechItems = anesthesiaTechniquesTypes.Select (x => x.Name).ToArray (); */

			List<iProPQRSPortableLib.Type> requiredAnesTechTypes = new List<iProPQRSPortableLib.Type>();
			foreach (int requiredID in iProPQRSPortableLib.Consts.RequiredAnesthesiaTechIDs) {
				requiredAnesTechTypes.Add(iProPQRSPortableLib.Consts.ProcAttribTypes.result.Types.Find (x => x.ProcAttribTypeID == requiredID));
			}
			anesTechItems = requiredAnesTechTypes.Select (x => x.Name).ToArray();
			Array.Sort (anesTechItems);
//			BindAnesthesiaTechniques (anesTechItems);
		}

		private void BindAnesthesiaTechniques(string[] anesTechItems)
		{
			int uvwidth;
			List<CodePickerModel> alist=mSetDataSource(anesTechItems, out  uvwidth);
			mcp = new mCodePicker(this,uvwidth);
			// need to set Selected Items
			if (ATlist.Count > 0)
				mcp.SelectedItems = ATlist;
			//

			float x = (float)txtAnesthesiaTechs.Frame.X;
			float y = (float)txtAnesthesiaTechs.Frame.Y;

			mcp.PresentFromPopover(txtAnesthesiaTechs, x, y,uvwidth);
			mcp.mDataSource(alist);
			mcp._ValueChanged += delegate
			{
				ATlist = mcp.SelectedItems;
				string finalText = string.Empty;
				foreach (var item in ATlist)
				{
					finalText = finalText + ", " + item.ItemText;
				}
//				SetTextboxValue(mCurrentTextBoxID, finalText.TrimStart(','));
				txtAnesthesiaTechs.Text = finalText.TrimStart(',');
			};
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

				activeView = diagnosisScrollView ;

				//bottom of the controller = initial pos + height + offset
				bottom = (activeView.Frame.Y + activeView.Frame.Height + offset);

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
				Console.WriteLine ("ScrollTheView ex: "+e.Message);
			}
		}



		private void ShowAlertMessage(string alrtMessag)
		{
			UIAlertView alrtMsg = new UIAlertView();
			alrtMsg.Message = alrtMessag;
			alrtMsg.AddButton("Ok"); 
			alrtMsg.Show();
		}

		public void NewDiagnosis(string code,string description)
		{
			CoreGraphics.CGRect descriptionFrame = new CoreGraphics.CGRect(8,baseYDiagnosisDescriptionValue,377,30);
			CoreGraphics.CGRect codeFrame = new CoreGraphics.CGRect(400,baseYDiagnosisCodeValue,70,30);

			UITextField codeText = new UITextField(codeFrame);
			UITextField descriptionText = new UITextField(descriptionFrame);
			codeText.Text=code;
			codeText.BorderStyle = UITextBorderStyle.RoundedRect;
			codeText.Tag = 2;
			codeText.EditingDidBegin += (object sender, EventArgs e) => {
				showKeyBoard = true;
			};

			codeText.EditingDidEnd += (object sender, EventArgs e) => {
				showKeyBoard = false;
			};

			descriptionText.BorderStyle = UITextBorderStyle.RoundedRect;
			descriptionText.Tag = 1;
			descriptionText.Text=description;
			descriptionText.EditingDidBegin += (object sender, EventArgs e) => {
				showKeyBoard = true;
			};

			descriptionText.EditingDidEnd += (object sender, EventArgs e) => {
				showKeyBoard = false;
			};

			descriptionText.EditingDidEnd += (senderDesc, e) => {

				bool itemPreviouslySearched = false;

				string lastSelectedProcedures = ReadFile("lastSelectedDiagnosis.txt");
				if(lastSelectedProcedures != string.Empty){
					lastSelectedDiagnosisObj = (ProcedureDiagnosticMaster)JsonConvert.DeserializeObject(lastSelectedProcedures,typeof(ProcedureDiagnosticMaster));
					foreach (DataResults item in lastSelectedDiagnosisObj.results) {
						if(item.Name != null){
							if(item.Name.ToLower().Contains(descriptionText.Text.Trim().ToLower())){
								itemPreviouslySearched = true;
								break;
							}
						}
					}
				}

				if(itemPreviouslySearched){
					int uvWidth=280;
					List<CodePickerModel> list=SetDataSource(out uvWidth,lastSelectedDiagnosisObj);
					float x = (float)descriptionText.Frame.X;
					float y = (float)descriptionText.Frame.Y;

					cp	=new CodePicker(this,uvWidth,descriptionText.Text,"ICD9");
					cp.PresentFromPopover(descriptionText,x,y,uvWidth);
					cp.DataSource=list;
					cp._ValueChanged += delegate {		
						codeText.Text = cp.SelectedCodeValue;
						descriptionText.Text = cp.SelectedText;
					}; 
				}else{
					if(descriptionText.Text.Trim().Length > 0)
						DownloadData("ICD9",descriptionText.Text,codeText,descriptionText);
				}
			};


			diagnosisScrollView.AddSubview(codeText);
			baseYDiagnosisCodeValue += 44;

			diagnosisScrollView.AddSubview(descriptionText);
			baseYDiagnosisDescriptionValue += 44;
		}

		public void AddProcedure(string code,string description)
		{
			CoreGraphics.CGRect descriptionFrame = new CoreGraphics.CGRect(14,baseYProcDescriptionValue,377,30);
			CoreGraphics.CGRect codeFrame = new CoreGraphics.CGRect(405,baseYProcCodeValue,70,30);

			UITextField codeText = new UITextField(codeFrame);
			UITextField descriptionText = new UITextField(descriptionFrame);
			codeText.Text=code;
			codeText.BorderStyle = UITextBorderStyle.RoundedRect;
			codeText.Tag = 2;
			codeText.EditingDidBegin += (object sender, EventArgs e) => {
				showKeyBoard = true;
			};

			codeText.EditingDidEnd += (object sender, EventArgs e) => {
				showKeyBoard = false;
			};

			descriptionText.BorderStyle = UITextBorderStyle.RoundedRect;
			descriptionText.Tag = 1;
			descriptionText.Text=description;
			descriptionText.EditingDidBegin += (object sender, EventArgs e) => {
				showKeyBoard = true;
			};

			descriptionText.EditingDidEnd += (object sender, EventArgs e) => {
				showKeyBoard = false;
			};

			descriptionText.EditingDidEnd += (senderDesc, e) => {


				bool itemPreviouslySearched = false;

				string lastSelectedProcedures = ReadFile("lastSelectedProcedures.txt");
				if(lastSelectedProcedures != string.Empty){
					lastSelectedProceduresObj = (ProcedureDiagnosticMaster)JsonConvert.DeserializeObject(lastSelectedProcedures,typeof(ProcedureDiagnosticMaster));
					foreach (DataResults item in lastSelectedProceduresObj.results) {
						if(item.Name != null){
							if(item.Name.ToLower().Contains(descriptionText.Text.Trim().ToLower())){
								itemPreviouslySearched = true;
								break;
							}
						}
					}
				}

				if(itemPreviouslySearched){
					int uvWidth=280;
					List<CodePickerModel> list=SetDataSource(out uvWidth,lastSelectedProceduresObj);
					float x = (float)descriptionText.Frame.X;
					float y = (float)descriptionText.Frame.Y;

					cp	=new CodePicker(this,uvWidth,descriptionText.Text,"CPT");
					cp.PresentFromPopover(descriptionText,x,y,uvWidth);
					cp.DataSource=list;
					cp._ValueChanged += delegate {		
						codeText.Text = cp.SelectedCodeValue;
						descriptionText.Text = cp.SelectedText;
					}; 
				}else{
					if(descriptionText.Text.Trim().Length > 0)
						DownloadData("CPT",descriptionText.Text,codeText,descriptionText);
				}

			};


			procedureScrollView.AddSubview(codeText);
			baseYProcCodeValue += 44;

			procedureScrollView.AddSubview(descriptionText);
			baseYProcDescriptionValue += 44;
		}

		private void RemoveDiagnosis ()
		{
			foreach(UIView view in diagnosisScrollView.Subviews){
				if(view.GetType().FullName.Contains("UITextField") ){
					if((view.Frame.Y == (baseYDiagnosisDescriptionValue - 44)) && (view.Tag == 1)){
						view.RemoveFromSuperview();
						baseYDiagnosisDescriptionValue -= 44;
					}

					if((view.Frame.Y == (baseYDiagnosisCodeValue - 44)) && (view.Tag == 2)){
						view.RemoveFromSuperview();
						baseYDiagnosisCodeValue -= 44;
					}
				}
			}
		}

		private void RemoveProcedures ()
		{
			foreach(UIView view in procedureScrollView.Subviews){
				if(view.GetType().FullName.Contains("UITextField") ){
					if((view.Frame.Y == (baseYProcDescriptionValue - 44)) && (view.Tag == 1)){
						view.RemoveFromSuperview();
						baseYProcDescriptionValue -= 44;
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
			List<CodePickerModel> list=SetProcedureDataSource (out uvwidth);
    		cp	= new CodePicker (this,uvwidth,searchParam.Trim(),type);

			if(searchParam.Trim().Length > 0)
				cp.PresentFromPopover (descriptionField, x, y,uvwidth);

			cp.DataSource = list;
			cp._ValueChanged += delegate {
				DataResults selectedObj = new DataResults{Name = cp.SelectedText,Code=cp.SelectedCodeValue};
				string fileName = string.Empty;
				if(type == "CPT")
					fileName = "lastSelectedProcedures.txt";
				else
					fileName = "lastSelectedDiagnosis.txt";

				SaveJsonToFile(selectedObj,fileName);

				codeField.Text = cp.SelectedCodeValue;
				descriptionField.Text = cp.SelectedText;
			};
		}

		public List<CodePickerModel> SetProcedureDataSource(out int wvalue)
		{
			CodePickerModel item;
			int cnt = 1;
			wvalue=280;
			int pcharcount = 0;
			int charcount = 0;
			List<CodePickerModel> dlist = new List<CodePickerModel> ();
			foreach (DataResults procitem in procedureItems.results) {
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
				item.ItemID = cnt;
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

	}
}


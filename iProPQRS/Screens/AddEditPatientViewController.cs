
using System;

using Foundation;
using UIKit;
using System.IO;
using System.Collections.Generic;
using iProPQRSPortableLib;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Xml.Serialization;
using System.Text;
using System.ComponentModel;

namespace iProPQRS
{
	public partial class AddEditPatientViewController : UIViewController
	{
		ProcedureDiagnosticMaster procedureItems;
		string AnesthesiaTech="General,MAC,Regional,Inhalation,GA-TIVA,LocaL,Spinal,Epidural";
		List<CodePickerModel> ATlist=new List<CodePickerModel>();
		string Lines="A-Line,PA-Catheter,CVP";
		List<CodePickerModel> Linelist=new List<CodePickerModel>();
		string CVCSterileTec="Sterile Techniques Followed,Sterile Techniques Not Followed-Medical Reasons,Sterile Techniques Not Followed-Unspecified Reasons";
		List<CodePickerModel> CVCTlist=new List<CodePickerModel>();
		string NerveBlack="A,B,C,D";
		List<CodePickerModel> NBlist=new List<CodePickerModel>();
		string SterileTec="Sterile Techniques Followed,Sterile Techniques Not Followed";
		List<CodePickerModel> STlist=new List<CodePickerModel>();
		string SpecialTech="Delib Hypotension,Delib Hypothermia,CPB/OPB/VVB,Criculary Arrest,OLV";
		List<CodePickerModel> SPTlist=new List<CodePickerModel>();

		ProcedureDiagnosticMaster lastSelectedProceduresObj;
		public AddEditPatientViewController () : base ("AddEditPatientViewController", null)
		{

		}
		PatientProfile mPatient;
		PatientProcedureDetails patientProcedureDetails;
		public AddEditPatientViewController (PatientProfile Patient) : base ("AddEditPatientViewController", null)
		{
			this.mPatient=Patient;
		}

		public AddEditPatientViewController (PatientProcedureDetails patientProcedureDetails) : base ("AddEditPatientViewController", null)
		{
			this.patientProcedureDetails=patientProcedureDetails;
		}

		string[] PS="ASA 1,ASA 2,ASA 3,ASA 4,ASA 5,ASA 6".Split(','); 
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		public void SetTextboxValue(string txtid, string txtval)
		{
			try
			{
				//wvpatient.DangerousRelease();
			    wvpatient.EvaluateJavascript("SetTextboxValue('"+txtid+"','"+txtval+"')");

			}
			catch {
				
			}
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
			//if(!string.IsNullOrEmpty(txt))
			//	charcount=txt.Length;
			//if (charcount > 45) {			

			//	int calval = charcount - 45;
			//	uvwidth = uvwidth + (calval * 20);
			//}
			return uvwidth;
		}
		public List<CodePickerModel> SetDataSource(out int uvWidth)
		{
			uvWidth=280;
			int pcharcount = 0;
			CodePickerModel item;
			int cnt = 1;
			List<CodePickerModel> dlist = new List<CodePickerModel> ();
			foreach (DataResults procItem in lastSelectedProceduresObj.results) {
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
		CodePicker cp;
		string PatientId=string.Empty;
		AddPatient ap = new AddPatient ();
		Common comn=new Common();
		public void LoadWevView()
		{
			string fileName = "Content/pqrs.html"; // remember case-sensitive
			string content = File.ReadAllText(fileName);
			string Finalcontent=ap.LoadHtml1 (content, patientProcedureDetails);
			wvpatient.LoadHtmlString(Finalcontent, NSUrl.FromFilename(fileName));
		}

		public void AlertMsg(string MSG)
		{
			var Confirm  = new UIAlertView("Confirmation", MSG,null,"Cancel","Yes");
			Confirm.Show();
			Confirm.Clicked += (object senders, UIButtonEventArgs es) => 
			{
				if (es.ButtonIndex == 0 ) {
					// do something if cancel
				}else
				{
					// Do something if yes
				}
			};
		}

        List<CodePickerModel> list=new List<CodePickerModel>();
		public override void ViewDidLoad ()
		{
			
			base.ViewDidLoad ();
			wvpatient.EndEditing (true);
			BackBtn.TouchUpInside+= (object sender, EventArgs e) => {
				this.NavigationController.PopViewController(true);
			};
			mMenu.ValueChanged+= (object sender, EventArgs e) => {

				wvpatient.EvaluateJavascript("bindPanel("+mMenu.SelectedSegment+")");
			};
			BtnSubmit.TouchUpInside+=async (object sender, EventArgs e) => {
				AppDelegate.pb.Start(this.View,"Please wait...");
				string str=wvpatient.EvaluateJavascript("GetTextboxValue()");
				XmlSerializer xs = new XmlSerializer(typeof(ProfileInfo));
				ProfileInfo objpi = (ProfileInfo)xs.Deserialize(new StringReader(str));

				PQRSServices services=new PQRSServices();
				Patient Profile=new Patient();

				if(patientProcedureDetails!=null)
					Profile.ID=patientProcedureDetails.PatientID;
				else
					Profile.ID=0;				
				Profile.cbEmergency=objpi.cbEmergency;

				if(mPatient!=null)
					Profile.CreatedOn = mPatient.OperationDate;
				else
					Profile.CreatedOn = DateTime.Now.ToShortDateString();
			
				Profile.ddlASAType=objpi.ddlASAType;
				Profile.ddlEncounterType=objpi.ddlEncounterType;
				Profile.LastModifiedDate=DateTime.Now.ToShortDateString();
				Profile.FacilityID=iProPQRSPortableLib.Consts.SelectedFacilityID;
				Profile.FirstName=objpi.Firstname;
				Profile.LastName=objpi.Lastname;
				Profile.DOB=objpi.DOB;	
				Profile.MRN=objpi.MRN;
				ReceiveContext result1 = await AppDelegate.Current.pqrsMgr.AddUpdatePatintInfo (Profile);
				Patient piResult=null;
				if(string.IsNullOrEmpty(result1.message))
				{
					piResult = (Patient)JsonConvert.DeserializeObject(result1.result.ToString() , typeof(Patient));
					//new UIAlertView("Patient Info", "Successfully Saved."
					//	, null, "ok", null).Show();
				}
				else
				{
					new UIAlertView("Patient eroor", result1.message
						, null, "ok", null).Show();
				}
				PatientProcedureFullDetails PDetails=new PatientProcedureFullDetails();
				if(patientProcedureDetails!=null)
				{
					PDetails.ID=patientProcedureDetails.ProcID;
					PDetails.PatientID=patientProcedureDetails.PatientID.ToString();
					PDetails.StatusID=patientProcedureDetails.StatusID.ToString();
				}
				else
				{
					if(piResult==null)
					{
						PDetails.ID=0;
						PDetails.PatientID="0";
					}
					else
					{

						PDetails.ID=0;
						PDetails.PatientID=piResult.ID.ToString();
					}	
				}


				PDetails.Mrn=objpi.MRN;
				PDetails.OperationDate=objpi.OperationDate;
				//PDetails.PatientID=objpi.PatientID;
				//PDetails.Location=objpi.Location;
				//PDetails.ORNumber=objpi.;
				PDetails.Anesthesiologist=objpi.Anesthesiologist1_label;
				PDetails.Anesthesiologist1=objpi.Anesthesiologist2_label;
				PDetails.Anesthesiologist2=objpi.Anesthesiologist2_label;
				PDetails.Anesthesiologist3=objpi.Anesthesiologist2_label;
				PDetails.Crna=objpi.CRNA1_label;
				PDetails.Crna1=objpi.CRNA1_label;
				PDetails.Crna2=objpi.CRNA2_label;
				PDetails.Crna3=objpi.CRNA3_label;
				//PDetails.RlfTime1=objpi.tim;
				//PDetails.RlfTime2
				//PDetails.RlfTime3
				//PDetails.RlfTime4
				PDetails.CrnaRlfTime1=objpi.CrnaStrtTime1;
				PDetails.CrnaRlfTime2=objpi.CrnaStrtTime2;
				PDetails.Surgeon=objpi.Surgeon;
				PDetails.Surgeon2=objpi.Surgeon2;

				PDetails.Surgeon3=objpi.Surgeon3;
			    //PDetails.Procedures=objpi.Procedures1;
				//PDetails.Diagnosis=objpi.CRNA4_label;
				//PDetails.ScheduleStartTime=objpi.;
				//PDetails.StatusID=objpi.CRNA4_label;
				//PDetails.RoomIn=objpi.CRNA4_label;
				PDetails.AnesStart=objpi.AnesStrtTime1;
				//PDetails.SurgeryStart=objpi.OperationDate;
				//PDetails.IncisionTime=objpi.time;
				//PDetails.SurgeryEnd=objpi.CRNA4_label;
				PDetails.AnesEnd=objpi.AnesEndTime1;
				PDetails.LastUpdatedTime=DateTime.Now.ToString();
				//PDetails.tbPreIndBP=objpi.CRNA4_label;
				//PDetails.tbPreIndP=objpi.CRNA4_label;
				//PDetails.tbPreIndSPO2=objpi.CRNA4_label;
				//PDetails.tbPreIndResp=objpi.CRNA4_label;
				PDetails.Procedure1=objpi.Procedures1;
				PDetails.Procedure2=objpi.Procedures2;
				PDetails.Procedure3=objpi.Procedures3;
				PDetails.Procedure4=objpi.Procedures4;
				PDetails.ProcedureCode1=objpi.ProcedureCode1;
				PDetails.ProcedureCode2=objpi.ProcedureCode2;
				PDetails.ProcedureCode3=objpi.ProcedureCode3;
				PDetails.ProcedureCode4=objpi.ProcedureCode4;
				//PDetails.ProcedureUnit1
				//PDetails.ProcedureUnit2
				//PDetails.ProcedureUnit3
				//PDetails.ProcedureUnit4
				PDetails.Diagnosis1=objpi.Diagnosis1;
				PDetails.Diagnosis2=objpi.Diagnosis2;
				PDetails.Diagnosis3=objpi.Diagnosis3;
				PDetails.Diagnosis4=objpi.Diagnosis4;

				PDetails.DiagnosisCode1=objpi.DiagnosisCode1;
				PDetails.DiagnosisCode2=objpi.DiagnosisCode2;
				PDetails.DiagnosisCode3=objpi.DiagnosisCode3;
				PDetails.DiagnosisCode4=objpi.DiagnosisCode4;
				//DiagnosisUnit1
				//DiagnosisUnit2
				//DiagnosisUnit3
				//DiagnosisUnit4
				//Modifier1
				//Modifier2
				//Modifier3
				//Modifier4
				PDetails.Duration=objpi.delayedDuration;
				//SurveyComplete
				//BillingInfo
				//PDFStatus
				//PatientClass
				//CancellationReason
				PDetails.CancellationReasonID=objpi.ddlCancellationReasons;
				//OrderNumber
				PDetails.ddlEncounterType=objpi.ddlEncounterType;
				PDetails.ddlASAType=objpi.ddlASAType;
				PDetails.cbEmergency=objpi.cbEmergency;
				PDetails.Anesthesiologist4=objpi.Anesthesiologist4;
				//RlfTime3
				PDetails.Crna4=objpi.CRNA4_label;
				//CrnaRlfTime3
				//Srna1
				//Srna2
				//SrnaRlfTime1
				//Source
				//Induction
				PDetails.AnesStrtTime1=objpi.AnesStrtTime1;
				PDetails.AnesEndTime1=objpi.AnesEndTime1;
				PDetails.AnesStrtTime2=objpi.AnesStrtTime2;
				PDetails.AnesEndTime2=objpi.AnesEndTime2;
				PDetails.AnesStrtTime3=objpi.AnesStrtTime3;
				PDetails.AnesEndTime3=objpi.AnesEndTime3;
				PDetails.AnesStrtTime4=objpi.AnesStrtTime4;
				PDetails.AnesEndTime4=objpi.AnesEndTime4;
				PDetails.CrnaStrtTime1=objpi.CrnaStrtTime1;
				PDetails.CrnaEndTime1=objpi.CrnaEndTime1;
				PDetails.CrnaStrtTime2=objpi.CrnaStrtTime2;
				PDetails.CrnaEndTime2=objpi.CrnaEndTime2;
				PDetails.CrnaStrtTime3=objpi.CrnaStrtTime3;
				PDetails.CrnaEndTime3=objpi.CrnaEndTime3;
				PDetails.CrnaStrtTime4=objpi.CrnaStrtTime4;
				PDetails.CrnaEndTime4=objpi.CrnaEndTime4;
				//SrnaStrtTime1
				//SrnaEndTime1
				//ListType
				ReceiveContext result2=await services.UpdatePatintProcedureInfo(PDetails);
				if(string.IsNullOrEmpty(result2.message))
				{
					BtnSubmit.Enabled=false;
					AppDelegate.pb.Stop();
					new UIAlertView("Patient Procedure Info", "Successfully Saved."
						, null, "ok", null).Show();
				}
				else
				{
					AppDelegate.pb.Stop();
					new UIAlertView("Patient eroor", result2.message
						, null, "ok", null).Show();
				}


				//Nahid Ahmed Testing patient upload
//				Patient pat = new Patient();
//				pat.ID = 17517;
//				pat.MRN = "54576576876";
//				pat.FirstName = "John6";
//				pat.LastName = "DoelMansuri";
//				pat.Sex = "M";
//				pat.Height = "5)6";
//				pat.Weight = "67";
//				pat.Allergies = "Tablet";
//				pat.PersonProvidingInfo = "Patient";
//				pat.DOB = "1962-02-02T00:00:00";
//				pat.AccountNo = "3564546466";
//				AppDelegate.Current.pqrsMgr.UploadPatientData(pat);

			};
			this.NavigationController.NavigationBarHidden = true;		
			if (mPatient != null && mPatient.PatientID != null) {
				lblPatientName.Text = mPatient.PatientName;
				lblhmrn.Text = "MRN :" + mPatient.MRN;
				lblAccount.Text = "ACCT :" + mPatient.AccountNo;
			} else {
				lblPatientName.Text = "";
				lblhmrn.Text = "";
				lblAccount.Text = "";
			}
			LoadWevView ();
			wvpatient.LoadFinished+= WvPatientProcedure_LoadFinished;
			#region ShouldStartLoad
			wvpatient.ShouldStartLoad = (webView, request, navType) => {
				string requestString=request.Url.Description;
				string param=string.Empty;
				if(requestString.Contains("pro://showAlert/"))
				{
					string message=requestString.Remove(0,15);
					AlertMsg(message);
				}
				else if(requestString.Contains("pro://openDatePicker/"))
				{
					param=requestString.Remove(0,22);
					string[] pary=param.Split(',');
					string mCurrentTextBoxID=pary[0];
					float x=float.Parse(pary[1]);
					float y=float.Parse(pary[2]);
					if(mCurrentTextBoxID=="OperationDate" || mCurrentTextBoxID=="DOB")
					{
						DatePicker  dp=new DatePicker();
						dp.PresentFromPopover(wvpatient,x,y);
						dp._ValueChanged += delegate {	
							int age = DateTime.Now.Year - dp.SelectedDateValue.Year;
							//this.mPatient.DOB = dp.SelectedDate;
							SetTextboxValue(mCurrentTextBoxID,dp.SelectedDate);
							SetTextboxValue("Age",age.ToString());
						};
					}
					else
					{
						TimePicker  tp=new TimePicker();
						tp.PresentFromPopover(wvpatient,x,y);
						tp._ValueChanged += delegate {						
							SetTextboxValue(mCurrentTextBoxID,tp.SelectedTime);
						};
					}

				}
				else if(requestString.Contains("pro://openCodePicker/Diagnosis"))
				{
					param=requestString.Remove(0,22);
					string[] pary=param.Split(',');
					string currentDiagnosticCodeTextID = string.Empty;

					if(requestString.Contains("Diagnosis1"))
						currentDiagnosticCodeTextID = "DiagnosisCode1";
					else if(requestString.Contains("Diagnosis2"))
						currentDiagnosticCodeTextID = "DiagnosisCode2";
					else if(requestString.Contains("Diagnosis3"))
						currentDiagnosticCodeTextID = "DiagnosisCode3";
					else if(requestString.Contains("Diagnosis4"))
						currentDiagnosticCodeTextID = "DiagnosisCode4";

					bool itemPreviouslySearched = false;

					string lastSelectedProcedures = ReadFile("lastSelectedDiagnosis.txt");
					if(lastSelectedProcedures != string.Empty){
						lastSelectedProceduresObj = (ProcedureDiagnosticMaster)JsonConvert.DeserializeObject(lastSelectedProcedures,typeof(ProcedureDiagnosticMaster));
						foreach (DataResults item in lastSelectedProceduresObj.results) {
							if(item.Name != null){
								if(item.Name.ToLower().Contains(pary[4].ToLower())){
									itemPreviouslySearched = true;
									break;
								}
							}
						}
					}

					if(cp != null){
						if(cp.popover != null)
							cp.popover.Dismiss(false);
					}


					if(itemPreviouslySearched){
						int uvWidth=280;
						List<CodePickerModel> list=SetDataSource(out uvWidth);
						string mCurrentTextBoxID=pary[0];
						float x=float.Parse(pary[1]);
						float y=float.Parse(pary[2]);
						cp	=new CodePicker(this,uvWidth);
						cp.PresentFromPopover(wvpatient,x,y,uvWidth);
						cp.DataSource=list;
						cp._ValueChanged += delegate {						
							SetTextboxValue(mCurrentTextBoxID,cp.SelectedText);
							SetTextboxValue(currentDiagnosticCodeTextID,cp.SelectedCodeValue);
						}; 
					}else{
						if(pary[4].Replace("%20"," ").Trim().Length > 0)
							DownloadData("ICD9",pary,currentDiagnosticCodeTextID);
					}
				}
				else if(requestString.Contains("pro://openCodePicker/Procedures"))
				{
					param=requestString.Remove(0,22);
					string[] pary=param.Split(',');

					string currentProceduresCodeTextID = string.Empty;

					if(requestString.Contains("Procedures1"))
						currentProceduresCodeTextID = "ProcedureCode1";
					else if(requestString.Contains("Procedures2"))
						currentProceduresCodeTextID = "ProcedureCode2";
					else if(requestString.Contains("Procedures3"))
						currentProceduresCodeTextID = "ProcedureCode3";
					else if(requestString.Contains("Procedures4"))
						currentProceduresCodeTextID = "ProcedureCode4";
					
					bool itemPreviouslySearched = false;

					string lastSelectedProcedures = ReadFile("lastSelectedProcedures.txt");
					if(lastSelectedProcedures != string.Empty){
						lastSelectedProceduresObj = (ProcedureDiagnosticMaster)JsonConvert.DeserializeObject(lastSelectedProcedures,typeof(ProcedureDiagnosticMaster));
						foreach (DataResults item in lastSelectedProceduresObj.results) {
							if(item.Name != null){
								if(item.Name.ToLower().Contains(pary[4].ToLower())){
									itemPreviouslySearched = true;
									break;
								}
							}
						}
					}
						
 					if(cp != null){
						if(cp.popover != null)
							cp.popover.Dismiss(false);
					}
					
					if(itemPreviouslySearched){
						int uvWidth=280;
						List<CodePickerModel> list=SetDataSource(out uvWidth);
						string mCurrentTextBoxID=pary[0];
						float x=float.Parse(pary[1]);
						float y=float.Parse(pary[2]);
						cp	=new CodePicker(this,uvWidth);

						if(pary[4].Trim().Length > 0)
							cp.PresentFromPopover(wvpatient,x,y,uvWidth);
						
						cp.DataSource=list;
						cp._ValueChanged += delegate {	
							SetTextboxValue(mCurrentTextBoxID,cp.SelectedText);
							SetTextboxValue(currentProceduresCodeTextID,cp.SelectedCodeValue);
						}; 
					}else{
						if(pary[4].Replace("%20"," ").Trim().Length > 0)
							DownloadData("CPT",pary,currentProceduresCodeTextID);
					}
				}
				else if(requestString.Contains("ipro://Checkmrncode/"))
				{

					param = requestString.Remove(0, 23);
					string[] pary = param.Split(',');
					string mCurrentTextBoxID = pary[3];
					float x = float.Parse(pary[1]);
					float y = float.Parse(pary[2]);
					string mrn=wvpatient.EvaluateJavascript("GetVal('"+mCurrentTextBoxID+"')");
					PQRSServices serv=new PQRSServices();
					ReceiveContext context=new ReceiveContext();
					if(!string.IsNullOrEmpty(mrn))
					{
						AppDelegate.pb.Start(this.View,"Checking MRN number");

						InvokeOnMainThread ( async () =>  {
							context=await serv.CheckExistingPatintInfo(mrn,"1");
							string strtemp=context.result.ToString();
							List<Patient> objpi = (List<Patient>)JsonConvert.DeserializeObject(context.result.ToString() , typeof(List<Patient>));
							//"MRN", "Firstname", "Lastname","DOB","Age","ddlEncounterType","ddlASAType","cbEmergency"
							Patient tempProfil;
							if(objpi.Count>0)
							{
								if(objpi.Count>1)
								{

									int maxval=FindMaxID(objpi);
									tempProfil=objpi.Find(u=>u.ID==maxval);
								}
								else
								{								
									tempProfil=objpi[0];
								}
								SetTextboxValue("Firstname",tempProfil.FirstName);
								SetTextboxValue("Lastname",tempProfil.LastName);
								if(!string.IsNullOrEmpty(tempProfil.DOB))
								{
									SetTextboxValue("DOB",tempProfil.DOB);
									var now = float.Parse(DateTime.Now.ToString("yyyy.MMdd"));
									var dob = float.Parse(Convert.ToDateTime(tempProfil.DOB).ToString("yyyy.MMdd"));
									var age = (int)(now - dob);
									SetTextboxValue("Age", age.ToString());
								}							
								lblhmrn.Text = "MRN :" + tempProfil.MRN;
								lblAccount.Text = "ACCT :" + tempProfil.AccountNo;
								lblPatientName.Text = tempProfil.FirstName+" , "+tempProfil.LastName;
								SetTextboxValue("ddlEncounterType",tempProfil.ddlEncounterType);
								SetTextboxValue("ddlASAType",tempProfil.ddlASAType);
								SetTextboxValue("cbEmergency",tempProfil.cbEmergency);
								AppDelegate.pb.Stop();
							}
							else
							{
								SetTextboxValue("Firstname","");
								SetTextboxValue("Lastname","");
								SetTextboxValue("DOB","");
								SetTextboxValue("Age", "");
								lblhmrn.Text = "";
								lblAccount.Text = "";
								lblPatientName.Text = "";
								SetTextboxValue("ddlEncounterType","");
								SetTextboxValue("ddlASAType","");
								SetTextboxValue("cbEmergency","");
								AppDelegate.pb.Stop();
							}

						});
					}


				}
                else if (requestString.Contains("ipro://mopenCodePicker/"))
                {
					wvpatient.KeyboardDisplayRequiresUserAction = false;
					UIView ruv=wvpatient.InputAccessoryView;
					if(ruv!=null)
						ruv.Hidden=true;
					
                    mCodePicker mcp;

                    param = requestString.Remove(0, 23);
                    string[] pary = param.Split(',');
                    string mCurrentTextBoxID = pary[0];
                    float x = float.Parse(pary[1]);
                    float y = float.Parse(pary[2]);
                    if (mCurrentTextBoxID == "AnesthesiaTech")
                    {
						int uvwidth;
						List<CodePickerModel> alist=mSetDataSource(AnesthesiaTech.Split(','), out  uvwidth);
						mcp = new mCodePicker(this,uvwidth);
                        // need to set Selected Items
                        if (ATlist.Count > 0)
                            mcp.SelectedItems = ATlist;
                        //
						mcp.PresentFromPopover(wvpatient, x, y,uvwidth);
						mcp.mDataSource(alist);
                        mcp._ValueChanged += delegate
                        {
                            ATlist = mcp.SelectedItems;
                            string finalText = string.Empty;
                            foreach (var item in ATlist)
                            {
                                finalText = finalText + ", " + item.ItemText;
                            }
                            SetTextboxValue(mCurrentTextBoxID, finalText.TrimStart(','));
                        };
                    }
                    else if (mCurrentTextBoxID == "Lines")
                    {
						int uvwidth;
						List<CodePickerModel> list=mSetDataSource(Lines.Split(','), out  uvwidth);
                        mcp = new mCodePicker(this);
                        // need to set Selected Items
                        if (Linelist.Count > 0)
                            mcp.SelectedItems = Linelist;
                        //
						mcp.PresentFromPopover(wvpatient, x, y,uvwidth);
						mcp.mDataSource(list);
                        mcp._ValueChanged += delegate
                        {
                            Linelist = mcp.SelectedItems;
                            string finalText = string.Empty;
                            foreach (var item in Linelist)
                            {
                                finalText = finalText + ", " + item.ItemText;
                            }
                            SetTextboxValue(mCurrentTextBoxID, finalText.TrimStart(','));
                        };
                    }
                    else if (mCurrentTextBoxID == "CVCSterileTec")
                    {
						int uvwidth;
						List<CodePickerModel> list=mSetDataSource(CVCSterileTec.Split(','), out  uvwidth);
						mcp = new mCodePicker(this,uvwidth);
                        mcp.Setwidth();
                        // need to set Selected Items
                        if (CVCTlist.Count > 0)
                            mcp.SelectedItems = CVCTlist;
                        //
						mcp.PresentFromPopover(wvpatient, x, y,uvwidth);

						mcp.mDataSource(list);
                        mcp._ValueChanged += delegate
                        {
                            CVCTlist = mcp.SelectedItems;
                            string finalText = string.Empty;
                            foreach (var item in CVCTlist)
                            {
                                finalText = finalText + ", " + item.ItemText;
                            }
                            SetTextboxValue(mCurrentTextBoxID, finalText.TrimStart(','));
                        };
                    }
                    else if (mCurrentTextBoxID == "NerveBlack")
                    {
						int uvwidth;
						List<CodePickerModel> list=mSetDataSource(NerveBlack.Split(','), out uvwidth);
						mcp = new mCodePicker(this,uvwidth);
                        // need to set Selected Items
                        if (NBlist.Count > 0)
                            mcp.SelectedItems = NBlist;
                        //
						mcp.PresentFromPopover(wvpatient, x, y,uvwidth);
						mcp.mDataSource(list);
                        mcp._ValueChanged += delegate
                        {
                            NBlist = mcp.SelectedItems;
                            string finalText = string.Empty;
                            foreach (var item in NBlist)
                            {
                                finalText = finalText + ", " + item.ItemText;
                            }
                            SetTextboxValue(mCurrentTextBoxID, finalText.TrimStart(','));
                        };
                    }
                    else if (mCurrentTextBoxID == "SterileTec")
                    {
						int uvwidth;
						List<CodePickerModel> list=mSetDataSource(SterileTec.Split(','), out uvwidth);
						mcp = new mCodePicker(this,uvwidth);
                        // need to set Selected Items
                        if (STlist.Count > 0)
                            mcp.SelectedItems = STlist;
                        //
						mcp.PresentFromPopover(wvpatient, x, y,uvwidth);
						mcp.mDataSource(list);
                        mcp._ValueChanged += delegate
                        {
                            STlist = mcp.SelectedItems;
                            string finalText = string.Empty;
                            foreach (var item in STlist)
                            {
                                finalText = finalText + ", " + item.ItemText;
                            }
                            SetTextboxValue(mCurrentTextBoxID, finalText.TrimStart(','));
                        };
                    }
                    else if (mCurrentTextBoxID == "SpecialTech")
                    {
						int uvwidth;
						List<CodePickerModel> list=mSetDataSource(SpecialTech.Split(','), out uvwidth);
						mcp = new mCodePicker(this,uvwidth);
                        // need to set Selected Items
                        if (SPTlist.Count > 0)
                            mcp.SelectedItems = SPTlist;
                        //
						mcp.PresentFromPopover(wvpatient, x, y,uvwidth);
						mcp.mDataSource(list);
                        mcp._ValueChanged += delegate
                        {
                            SPTlist = mcp.SelectedItems;
                            string finalText = string.Empty;
                            foreach (var item in SPTlist)
                            {
                                finalText = finalText + ", " + item.ItemText;
                            }
                            SetTextboxValue(mCurrentTextBoxID, finalText.TrimStart(','));
                        };
                    }
                    wvpatient.ResignFirstResponder();
                }
				return true;
			};
			#endregion
			// Perform any additional setup after loading the view, typically from a nib.
		}
		public int FindMaxID(List<Patient> list)
		{
			if (list.Count == 0)
			{
				throw new InvalidOperationException("Empty list");
			}
			int maxID = int.MinValue;
			foreach (Patient type in list)
			{
				if (type.ID > maxID)
				{
					maxID = type.ID;
				}
			}
			return maxID;
		}
		#region DownloadData
		public async void DownloadData(string type,string[] searchParam,string codeID)
		{
			var webClient = new WebClient();
			string url =  "http://reference.iprocedures.com/"+type+"/"+searchParam[4].Trim().Replace("%20"," ")+"/40";
			string procData = webClient.DownloadString (url);
			procedureItems = (ProcedureDiagnosticMaster)JsonConvert.DeserializeObject (procData, typeof(ProcedureDiagnosticMaster));

			string mCurrentTextBoxID = searchParam [0];
			float x = float.Parse (searchParam [1]);
			float y = float.Parse (searchParam [2]);
			int uvwidth = 0;
			List<CodePickerModel> list=SetProcedureDataSource (out uvwidth);
			cp	= new CodePicker (this,uvwidth);

			if(searchParam[4].Trim().Length > 0)
				cp.PresentFromPopover (wvpatient, x, y,uvwidth);
	
			cp.DataSource = list;
			cp._ValueChanged += delegate {
				DataResults selectedObj = new DataResults{Name = cp.SelectedText,Code=cp.SelectedCodeValue};
				string fileName = string.Empty;
				if(type == "CPT")
					fileName = "lastSelectedProcedures.txt";
				else
					fileName = "lastSelectedDiagnosis.txt";

				SaveJsonToFile(selectedObj,fileName);

				SetTextboxValue (mCurrentTextBoxID, cp.SelectedText);
				SetTextboxValue(codeID,cp.SelectedCodeValue);
				//Nahid Ahmed My Test method
//				UpdatePatientObject(codeID);
			};
		}
		#endregion

		#region WvPatientProcedure_LoadFinished
		void WvPatientProcedure_LoadFinished (object sender, EventArgs e)
		{
			if (patientProcedureDetails == null)
				patientProcedureDetails = new PatientProcedureDetails ();


			SetTextboxValue ("MRN", patientProcedureDetails.Mrn);
			SetTextboxValue ("Firstname", patientProcedureDetails.FirstName);
			SetTextboxValue ("Lastname", patientProcedureDetails.LastName);
//			SetTextboxValue ("Allergies", mPatient.Allergies);
//			SetTextboxValue ("AccountNo", mPatient.AccountNo);
//			SetTextboxValue ("Height", mPatient.Height);
//			SetTextboxValue ("Weight", mPatient.Weight);
//			SetTextboxValue ("Sex", mPatient.Sex);
//			SetTextboxValue ("ddlASAType", mPatient.ddlASAType);
//			SetTextboxValue ("ddlHghtType", mPatient.ddlHghtType);
//			wvpatient.EvaluateJavascript ("calcBMI()");

//			if (!string.IsNullOrEmpty (mPatient.cbEmergency)) {
//				if (mPatient.cbEmergency.Equals (@"ON")) {
//					wvpatient.EvaluateJavascript (@"setCheck('cbEmergency')");
//				} else {
//					wvpatient.EvaluateJavascript (@"setCheckOff('cbEmergency')");
//				}
//			}
//			if (!string.IsNullOrEmpty (mPatient.cbNKDA)) {
//				if(mPatient.cbNKDA.Equals(@"ON"))
//					wvpatient.EvaluateJavascript (@"setCheck('cbNKDA')");
//				else
//					wvpatient.EvaluateJavascript (@"setCheckOff('cbNKDA')");
//			}

//			SetTextboxValue ("AnesStrtTime1", mPatient.AnesStrtTime1);
//			SetTextboxValue ("AnesStrtTime2", mPatient.AnesStrtTime2);
//			SetTextboxValue ("AnesStrtTime3", mPatient.AnesStrtTime3);
//			SetTextboxValue ("AnesStrtTime4", mPatient.AnesStrtTime4);
//			SetTextboxValue ("AnesEndTime1", mPatient.AnesEndTime1);
//			SetTextboxValue ("AnesEndTime2", mPatient.AnesEndTime2);
//			SetTextboxValue ("AnesEndTime3", mPatient.AnesEndTime3);
//			SetTextboxValue ("AnesEndTime4", mPatient.AnesEndTime4);
//			SetTextboxValue ("CrnaStrtTime1", mPatient.CrnaStrtTime1);
//			SetTextboxValue ("CrnaStrtTime2", mPatient.CrnaStrtTime2);
//			SetTextboxValue ("CrnaStrtTime3", mPatient.CrnaStrtTime3);
//			SetTextboxValue ("CrnaStrtTime4", mPatient.CrnaStrtTime4);
//			SetTextboxValue ("CrnaEndTime1", mPatient.CrnaEndTime1);
//			SetTextboxValue ("CrnaEndTime2", mPatient.CrnaEndTime2);
//			SetTextboxValue ("CrnaEndTime3", mPatient.CrnaEndTime3);
//			SetTextboxValue ("CrnaEndTime4", mPatient.CrnaEndTime4);
//			SetTextboxValue ("SrnaStrtTime1", mPatient.SrnaStrtTime1);
//			SetTextboxValue ("SrnaEndTime1", mPatient.SrnaEndTime1);

			SetTextboxValue ("AnesStrtTime1", "");
			SetTextboxValue ("AnesStrtTime2", "");
			SetTextboxValue ("AnesStrtTime3", "");
			SetTextboxValue ("AnesStrtTime4", "");
			SetTextboxValue ("AnesEndTime1", "");
			SetTextboxValue ("AnesEndTime2", "");
			SetTextboxValue ("AnesEndTime3", "");
			SetTextboxValue ("AnesEndTime4", "");

			SetTextboxValue ("Anesthesiologist1", "0");
			SetTextboxValue ("Anesthesiologist1_label", patientProcedureDetails.Anestheologist);
			SetTextboxValue ("Anesthesiologist2", "1");
			SetTextboxValue ("Anesthesiologist2_label", "");
			SetTextboxValue ("Anesthesiologist3", "2");
			SetTextboxValue ("Anesthesiologist3_label", "");
			SetTextboxValue ("Anesthesiologist4", "3");
			SetTextboxValue ("Anesthesiologist4_label", "");

//			if (iProPQRSPortableLib.Consts.Role.Equals ("Anesthesiologist") && !string.IsNullOrEmpty (mPatient.Anesthesiologist1.ToString()) && isFutureDate (mPatient.OperationDate)) {
//				SetTextboxValue ("Anesthesiologist1", iProPQRSPortableLib.Consts.mId);
//				SetTextboxValue ("Anesthesiologist1_label", iProPQRSPortableLib.Consts.Getpersonnel(iProPQRSPortableLib.Consts.mId,"Anesthesiologist"));
//			} else {
//				SetTextboxValue ("Anesthesiologist1", mPatient.Anesthesiologist1.ToString());
//				SetTextboxValue ("Anesthesiologist1_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.Anesthesiologist1.ToString(),"Anesthesiologist"));
//			}
//
//			SetTextboxValue ("Anesthesiologist2", mPatient.Anesthesiologist2.ToString());	
//			SetTextboxValue ("Anesthesiologist2_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.Anesthesiologist2.ToString(),"Anesthesiologist"));	
//			SetTextboxValue ("Anesthesiologist3", mPatient.Anesthesiologist3.ToString());	
//			SetTextboxValue ("Anesthesiologist3_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.Anesthesiologist3.ToString(),"Anesthesiologist"));	
//			SetTextboxValue ("Anesthesiologist4", mPatient.Anesthesiologist4.ToString());	
//			SetTextboxValue ("Anesthesiologist4_label",  iProPQRSPortableLib.Consts.Getpersonnel(mPatient.Anesthesiologist4.ToString(),"Anesthesiologist"));	


			SetTextboxValue ("CRNA1", patientProcedureDetails.CRNA);
//			if (iProPQRSPortableLib.Consts.Role == "CRNA" && string.IsNullOrEmpty (mPatient.CRNA1.ToString()) && !isFutureDate (mPatient.OperationDate)) {
//				SetTextboxValue ("CRNA1", iProPQRSPortableLib.Consts.mId);
//				SetTextboxValue ("CRNA1_label", iProPQRSPortableLib.Consts.Getpersonnel(iProPQRSPortableLib.Consts.mId,"CRNA"));
//			} else {
//				SetTextboxValue ("CRNA1", mPatient.CRNA1.ToString());
//				SetTextboxValue("CRNA1_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.CRNA1.ToString(),"CRNA"));
//			}
//
//			SetTextboxValue ("CRNA2", mPatient.CRNA2.ToString ());
//			SetTextboxValue ("CRNA2_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.CRNA2.ToString(),"CRNA"));
//			SetTextboxValue ("CRNA3", mPatient.CRNA3.ToString ());
//			SetTextboxValue ("CRNA3_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.CRNA2.ToString(),"CRNA"));
//			SetTextboxValue ("CRNA4", mPatient.CRNA4.ToString ());
//			SetTextboxValue ("CRNA4_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.CRNA2.ToString(),"CRNA"));

//			if (iProPQRSPortableLib.Consts.Role == "SRNA" && string.IsNullOrEmpty (mPatient.SRNA1.ToString ()) && isFutureDate (mPatient.OperationDate)) {
//				SetTextboxValue ("SRNA1", iProPQRSPortableLib.Consts.mId);
//				SetTextboxValue ("SRNA1_label", iProPQRSPortableLib.Consts.Getpersonnel(iProPQRSPortableLib.Consts.mId,"SRNA"));
//			} else {
//
//				SetTextboxValue ("SRNA1",mPatient.SRNA1.ToString());
//				SetTextboxValue ("SRNA1_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.SRNA1.ToString(),"SRNA"));
//			}
//			SetTextboxValue ("SRNA2", mPatient.SRNA2.ToString ());
//			SetTextboxValue ("SRNA2_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.SRNA2.ToString(),"SRNA"));

			SetTextboxValue ("Surgeon_label", patientProcedureDetails.surgeon);
//			SetTextboxValue ("Surgeon_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.Surgeon.ToString(),"Surgeon"));
//			SetTextboxValue ("Surgeon2", mPatient.Surgeon2.ToString ());
//			SetTextboxValue ("Surgeon2_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.Surgeon2.ToString(),"Surgeon"));
//			SetTextboxValue ("Surgeon3", mPatient.Surgeon3.ToString ());
//			SetTextboxValue ("Surgeon3_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.Surgeon3.ToString(),"Surgeon"));


			//start BILLING info
			SetTextboxValue ("AnesthesiaTech", "");
			SetTextboxValue ("Lines", "");
			SetTextboxValue ("CVCSterileTec", "");
			SetTextboxValue ("NerveBlack", "");

			string cbUltrasound = "OFF";
			if (!string.IsNullOrEmpty (cbUltrasound)) {
				if(cbUltrasound.Equals(@"ON"))
					wvpatient.EvaluateJavascript (@"setCheck('cbUltrasound')");
				else
					wvpatient.EvaluateJavascript (@"setCheckOff('cbUltrasound')");
			}

			string cbContCatheter = "OFF";
			if (!string.IsNullOrEmpty (cbContCatheter)) {
				if(cbContCatheter.Equals(@"ON"))
					wvpatient.EvaluateJavascript (@"setCheck('cbContCatheter')");
				else
					wvpatient.EvaluateJavascript (@"setCheckOff('cbContCatheter')");
			}

			string cbIndication = "OFF";
			if (!string.IsNullOrEmpty (cbIndication)) {
				if(cbIndication.Equals(@"ON"))
					wvpatient.EvaluateJavascript (@"setCheck('cbIndication')");
				else
					wvpatient.EvaluateJavascript (@"setCheckOff('cbIndication')");
			}

			string cboperativeanesthesia = "OFF";
			if (!string.IsNullOrEmpty (cboperativeanesthesia)) {
				if(cboperativeanesthesia.Equals(@"ON"))
					wvpatient.EvaluateJavascript (@"setCheck('cboperativeanesthesia')");
				else
					wvpatient.EvaluateJavascript (@"setCheckOff('cboperativeanesthesia')");
			}

			SetTextboxValue ("SterileTec", "");
			SetTextboxValue ("SpecialTech", "");

			//end billing info
			wvpatient.EvaluateJavascript ("initDD()");

			if(patientProcedureDetails.StatusID==2)
				wvpatient.EvaluateJavascript ("bindForInProcessRecord()");
			else
				wvpatient.EvaluateJavascript ("bindForNewRecord()");

//			SetTextboxValue ("ddlEncounterType", mPatient.ddlEncounterType);
//			SetTextboxValue ("ORNumber", mPatient.ORNumber.ToString());
		}
		#endregion

		#region Wvpatient_LoadFinished
		//PatientProfile mPatient=new PatientProfile();
		void Wvpatient_LoadFinished (object sender, EventArgs e)
		{
			if (mPatient == null)
				mPatient = new PatientProfile ();
			

			SetTextboxValue ("MRN", mPatient.MRN);
			SetTextboxValue ("Firstname", mPatient.FirstName);
			SetTextboxValue ("Lastname", mPatient.LastName);
			SetTextboxValue ("Allergies", mPatient.Allergies);
			SetTextboxValue ("AccountNo", mPatient.AccountNo);
			SetTextboxValue ("Height", mPatient.Height);
			SetTextboxValue ("Weight", mPatient.Weight);
			SetTextboxValue ("Sex", mPatient.Sex);
			SetTextboxValue ("ddlASAType", mPatient.ddlASAType);
			SetTextboxValue ("ddlHghtType", mPatient.ddlHghtType);
			wvpatient.EvaluateJavascript ("calcBMI()");

			if (!string.IsNullOrEmpty (mPatient.cbEmergency)) {
				if (mPatient.cbEmergency.Equals (@"ON")) {
					wvpatient.EvaluateJavascript (@"setCheck('cbEmergency')");
				} else {
					wvpatient.EvaluateJavascript (@"setCheckOff('cbEmergency')");
				}
			}
			if (!string.IsNullOrEmpty (mPatient.cbNKDA)) {
				if(mPatient.cbNKDA.Equals(@"ON"))
					wvpatient.EvaluateJavascript (@"setCheck('cbNKDA')");
				else
					wvpatient.EvaluateJavascript (@"setCheckOff('cbNKDA')");
			}
		
			SetTextboxValue ("AnesStrtTime1", mPatient.AnesStrtTime1);
			SetTextboxValue ("AnesStrtTime2", mPatient.AnesStrtTime2);
			SetTextboxValue ("AnesStrtTime3", mPatient.AnesStrtTime3);
			SetTextboxValue ("AnesStrtTime4", mPatient.AnesStrtTime4);
			SetTextboxValue ("AnesEndTime1", mPatient.AnesEndTime1);
			SetTextboxValue ("AnesEndTime2", mPatient.AnesEndTime2);
			SetTextboxValue ("AnesEndTime3", mPatient.AnesEndTime3);
			SetTextboxValue ("AnesEndTime4", mPatient.AnesEndTime4);
			SetTextboxValue ("CrnaStrtTime1", mPatient.CrnaStrtTime1);
			SetTextboxValue ("CrnaStrtTime2", mPatient.CrnaStrtTime2);
			SetTextboxValue ("CrnaStrtTime3", mPatient.CrnaStrtTime3);
			SetTextboxValue ("CrnaStrtTime4", mPatient.CrnaStrtTime4);
			SetTextboxValue ("CrnaEndTime1", mPatient.CrnaEndTime1);
			SetTextboxValue ("CrnaEndTime2", mPatient.CrnaEndTime2);
			SetTextboxValue ("CrnaEndTime3", mPatient.CrnaEndTime3);
			SetTextboxValue ("CrnaEndTime4", mPatient.CrnaEndTime4);
			SetTextboxValue ("SrnaStrtTime1", mPatient.SrnaStrtTime1);
			SetTextboxValue ("SrnaEndTime1", mPatient.SrnaEndTime1);
			if (iProPQRSPortableLib.Consts.Role.Equals ("Anesthesiologist") && !string.IsNullOrEmpty (mPatient.Anesthesiologist1.ToString()) && isFutureDate (mPatient.OperationDate)) {
				SetTextboxValue ("Anesthesiologist1", iProPQRSPortableLib.Consts.mId);
				SetTextboxValue ("Anesthesiologist1_label", iProPQRSPortableLib.Consts.Getpersonnel(iProPQRSPortableLib.Consts.mId,"Anesthesiologist"));
			} else {
				SetTextboxValue ("Anesthesiologist1", mPatient.Anesthesiologist1.ToString());
				SetTextboxValue ("Anesthesiologist1_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.Anesthesiologist1.ToString(),"Anesthesiologist"));
			}

			SetTextboxValue ("Anesthesiologist2", mPatient.Anesthesiologist2.ToString());	
			SetTextboxValue ("Anesthesiologist2_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.Anesthesiologist2.ToString(),"Anesthesiologist"));	
			SetTextboxValue ("Anesthesiologist3", mPatient.Anesthesiologist3.ToString());	
			SetTextboxValue ("Anesthesiologist3_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.Anesthesiologist3.ToString(),"Anesthesiologist"));	
			SetTextboxValue ("Anesthesiologist4", mPatient.Anesthesiologist4.ToString());	
			SetTextboxValue ("Anesthesiologist4_label",  iProPQRSPortableLib.Consts.Getpersonnel(mPatient.Anesthesiologist4.ToString(),"Anesthesiologist"));	



			if (iProPQRSPortableLib.Consts.Role == "CRNA" && string.IsNullOrEmpty (mPatient.CRNA1.ToString()) && !isFutureDate (mPatient.OperationDate)) {
				SetTextboxValue ("CRNA1", iProPQRSPortableLib.Consts.mId);
				SetTextboxValue ("CRNA1_label", iProPQRSPortableLib.Consts.Getpersonnel(iProPQRSPortableLib.Consts.mId,"CRNA"));
			} else {
				SetTextboxValue ("CRNA1", mPatient.CRNA1.ToString());
				SetTextboxValue("CRNA1_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.CRNA1.ToString(),"CRNA"));
			}

			SetTextboxValue ("CRNA2", mPatient.CRNA2.ToString ());
			SetTextboxValue ("CRNA2_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.CRNA2.ToString(),"CRNA"));
			SetTextboxValue ("CRNA3", mPatient.CRNA3.ToString ());
			SetTextboxValue ("CRNA3_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.CRNA2.ToString(),"CRNA"));
			SetTextboxValue ("CRNA4", mPatient.CRNA4.ToString ());
			SetTextboxValue ("CRNA4_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.CRNA2.ToString(),"CRNA"));
			if (iProPQRSPortableLib.Consts.Role == "SRNA" && string.IsNullOrEmpty (mPatient.SRNA1.ToString ()) && isFutureDate (mPatient.OperationDate)) {
				SetTextboxValue ("SRNA1", iProPQRSPortableLib.Consts.mId);
				SetTextboxValue ("SRNA1_label", iProPQRSPortableLib.Consts.Getpersonnel(iProPQRSPortableLib.Consts.mId,"SRNA"));
			} else {

				SetTextboxValue ("SRNA1",mPatient.SRNA1.ToString());
				SetTextboxValue ("SRNA1_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.SRNA1.ToString(),"SRNA"));
			}
			SetTextboxValue ("SRNA2", mPatient.SRNA2.ToString ());
			SetTextboxValue ("SRNA2_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.SRNA2.ToString(),"SRNA"));

			SetTextboxValue ("Surgeon", mPatient.Surgeon.ToString ());
			SetTextboxValue ("Surgeon_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.Surgeon.ToString(),"Surgeon"));
			//setJSVal ("Surgeon_label", mPatient.SurgeonName);
			SetTextboxValue ("Surgeon2", mPatient.Surgeon2.ToString ());
			SetTextboxValue ("Surgeon2_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.Surgeon2.ToString(),"Surgeon"));
			SetTextboxValue ("Surgeon3", mPatient.Surgeon3.ToString ());
			SetTextboxValue ("Surgeon3_label", iProPQRSPortableLib.Consts.Getpersonnel(mPatient.Surgeon3.ToString(),"Surgeon"));


			//start BILLING info
			SetTextboxValue ("AnesthesiaTech", "");
			SetTextboxValue ("Lines", "");
			SetTextboxValue ("CVCSterileTec", "");
			SetTextboxValue ("NerveBlack", "");

			string cbUltrasound = "OFF";
			if (!string.IsNullOrEmpty (cbUltrasound)) {
				if(cbUltrasound.Equals(@"ON"))
					wvpatient.EvaluateJavascript (@"setCheck('cbUltrasound')");
				else
					wvpatient.EvaluateJavascript (@"setCheckOff('cbUltrasound')");
			}

			string cbContCatheter = "OFF";
			if (!string.IsNullOrEmpty (cbContCatheter)) {
				if(cbContCatheter.Equals(@"ON"))
					wvpatient.EvaluateJavascript (@"setCheck('cbContCatheter')");
				else
					wvpatient.EvaluateJavascript (@"setCheckOff('cbContCatheter')");
			}

			string cbIndication = "OFF";
			if (!string.IsNullOrEmpty (cbIndication)) {
				if(cbIndication.Equals(@"ON"))
					wvpatient.EvaluateJavascript (@"setCheck('cbIndication')");
				else
					wvpatient.EvaluateJavascript (@"setCheckOff('cbIndication')");
			}

			string cboperativeanesthesia = "OFF";
			if (!string.IsNullOrEmpty (cboperativeanesthesia)) {
				if(cboperativeanesthesia.Equals(@"ON"))
					wvpatient.EvaluateJavascript (@"setCheck('cboperativeanesthesia')");
				else
					wvpatient.EvaluateJavascript (@"setCheckOff('cboperativeanesthesia')");
			}

			SetTextboxValue ("SterileTec", "");
			SetTextboxValue ("SpecialTech", "");

			//end billing info
			wvpatient.EvaluateJavascript ("initDD()");

			if(mPatient.StatusID==2)
				wvpatient.EvaluateJavascript ("bindForInProcessRecord()");
			else
				wvpatient.EvaluateJavascript ("bindForNewRecord()");

			SetTextboxValue ("ddlEncounterType", mPatient.ddlEncounterType);
			SetTextboxValue ("ORNumber", mPatient.ORNumber.ToString());
		}
		#endregion
		public void setJSVal(string idAttr,string inVal)
		{
			wvpatient.EvaluateJavascript(@"setValue('"+idAttr+"','"+inVal+"')");
		}
		public bool isFutureDate(string str)
		{
			//char[] array = str.ToCharArray ();
			string temp=str;
			int yyyy=Convert.ToInt16(temp.Substring(0,4));
			int dd = Convert.ToInt16 (temp.Substring (temp.Length-2, 2));
			int MM=Convert.ToInt16(temp.Substring(temp.Length-4,2));


			DateTime sdt=new DateTime(yyyy,MM,dd);
			bool res;
				if (sdt > DateTime.Now.Date)
				return res=true;
			else
				return res=false;
			return res;
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

		//partial void btnSubmitClicked (NSObject sender)
		//{
			//Console.WriteLine("############### 1 ######################");
		//	Console.WriteLine("Diagnosis1: "+this.mPatient.Diagnosis1);
		//	Console.WriteLine("DiagnosisCode1: "+this.mPatient.DiagnosisCode1);
		//	Console.WriteLine("Procedures1: "+this.mPatient.Procedures1);
		//	Console.WriteLine("ProcedureCode1: "+this.mPatient.ProcedureCode1);
		//	Console.WriteLine();
		//
		//	Console.WriteLine("############### 2 ######################");
			//Console.WriteLine("Diagnosis2: "+this.mPatient.Diagnosis2);
		//	Console.WriteLine("DiagnosisCode2: "+this.mPatient.DiagnosisCode2);
		//	Console.WriteLine("Procedures2: "+this.mPatient.Procedures2);
		//	Console.WriteLine("ProcedureCode2: "+this.mPatient.ProcedureCode2);
		//	Console.WriteLine();
		//				Console.WriteLine("############### 3 ######################");
		//	Console.WriteLine("Diagnosis3: "+this.mPatient.Diagnosis3);
		//	Console.WriteLine("DiagnosisCode3: "+this.mPatient.DiagnosisCode3);
		//	Console.WriteLine("Procedures3: "+this.mPatient.Procedures3);
		//	Console.WriteLine("ProcedureCode3: "+this.mPatient.ProcedureCode3);
		//	Console.WriteLine();
		//
		//	Console.WriteLine("############### 4 ######################");
		//	Console.WriteLine("Diagnosis4: "+this.mPatient.Diagnosis4);
		///	Console.WriteLine("DiagnosisCode4: "+this.mPatient.DiagnosisCode4);
		//	Console.WriteLine("Procedures4: "+this.mPatient.Procedures4);
		//	Console.WriteLine("ProcedureCode4: "+this.mPatient.ProcedureCode4);
		//	Console.WriteLine();
		//}


		#region UpdatePatientObject
		private void UpdatePatientObject(string diagNosticProcedureCode)
		{
			if (diagNosticProcedureCode.Contains ("Diagnosis")) {
				if (diagNosticProcedureCode.Contains ("DiagnosisCode1")) {
					this.mPatient.Diagnosis1 = wvpatient.EvaluateJavascript("GetTextBoxValue('Diagnosis1')");
					this.mPatient.DiagnosisCode1 = wvpatient.EvaluateJavascript("GetTextBoxValue('DiagnosisCode1')");
				} else if (diagNosticProcedureCode.Contains ("DiagnosisCode2")) {
					this.mPatient.Diagnosis2 = wvpatient.EvaluateJavascript("GetTextBoxValue('Diagnosis2')");
					this.mPatient.DiagnosisCode2 = wvpatient.EvaluateJavascript("GetTextBoxValue('DiagnosisCode2')");
				} else if (diagNosticProcedureCode.Contains ("DiagnosisCode3")) {
					this.mPatient.Diagnosis3 = wvpatient.EvaluateJavascript("GetTextBoxValue('Diagnosis3')");
					this.mPatient.DiagnosisCode3 = wvpatient.EvaluateJavascript("GetTextBoxValue('DiagnosisCode3')");
				} else if (diagNosticProcedureCode.Contains ("DiagnosisCode4")) {
					this.mPatient.Diagnosis4 = wvpatient.EvaluateJavascript("GetTextBoxValue('Diagnosis4')");
					this.mPatient.DiagnosisCode4 = wvpatient.EvaluateJavascript("GetTextBoxValue('DiagnosisCode4')");
				}
			} else {
				if (diagNosticProcedureCode.Contains ("ProcedureCode1")) {
					this.mPatient.Procedures1 = wvpatient.EvaluateJavascript("GetTextBoxValue('Procedures1')");
					this.mPatient.ProcedureCode1 = wvpatient.EvaluateJavascript("GetTextBoxValue('ProcedureCode1')");
				} else if (diagNosticProcedureCode.Contains ("ProcedureCode2")) {
					this.mPatient.Procedures2 = wvpatient.EvaluateJavascript("GetTextBoxValue('Procedures2')");
					this.mPatient.ProcedureCode2 = wvpatient.EvaluateJavascript("GetTextBoxValue('ProcedureCode2')");
				} else if (diagNosticProcedureCode.Contains ("ProcedureCode3")) {
					this.mPatient.Procedures3 = wvpatient.EvaluateJavascript("GetTextBoxValue('Procedures3')");
					this.mPatient.ProcedureCode3 = wvpatient.EvaluateJavascript("GetTextBoxValue('ProcedureCode3')");
				} else if (diagNosticProcedureCode.Contains ("ProcedureCode4")) {
					this.mPatient.Procedures4 = wvpatient.EvaluateJavascript("GetTextBoxValue('Procedures4')");
					this.mPatient.ProcedureCode4 = wvpatient.EvaluateJavascript("GetTextBoxValue('ProcedureCode4')");
				}
			}
		}
		#endregion
	}

	public class ProfileInfo
	{
		public int ID {
			get;
			set;
		}
		public string MRN {
			get;
			set;
		}
		public string Firstname {
			get;
			set;
		}
		public string Lastname {
			get;
			set;
		}
		public string DOB {
			get;
			set;
		}
		public string Age {
			get;
			set;
		}
		public string ddlEncounterType {
			get;
			set;
		}
		public string ddlASAType {
			get;
			set;
		}
		public string cbEmergency {
			get;
			set;
		}
		public string OperationDate {
			get;
			set;
		}
		public string Facility {
			get;
			set;
		}
		public string Diagnosis1 {
			get;
			set;
		}
		public string Diagnosis2 {
			get;
			set;
		}
		public string Diagnosis3 {
			get;
			set;
		}
		public string Diagnosis4 {
			get;
			set;
		}
		public string Procedures1 {
			get;
			set;
		}
		public string Procedures2 {
			get;
			set;
		}
		public string Procedures3 {
			get;
			set;
		}
		public string Procedures4 {
			get;
			set;
		}
		public string Anesthesiologist1_label {
			get;
			set;
		}
		public string AnesStrtTime1 {
			get;
			set;
		}
		public string AnesEndTime1 {
			get;
			set;
		}
		public string Anesthesiologist2_label {
			get;
			set;
		}
		public string AnesStrtTime2 {
			get;
			set;
		}
		public string AnesEndTime2 {
			get;
			set;
		}
		public string Anesthesiologist3_label {
			get;
			set;
		}
		public string AnesStrtTime3 {
			get;
			set;
		}
		public string AnesEndTime3 {
			get;
			set;
		}
		public string Anesthesiologist4_label {
			get;
			set;
		}
		public string AnesStrtTime4 {
			get;
			set;
		}
		public string AnesEndTime4 {
			get;
			set;
		}
		public string CRNA1 {
			get;
			set;
		}
		public string CRNA1_label {
			get;
			set;
		}
		public string CrnaStrtTime1 {
			get;
			set;
		}
		public string CrnaEndTime1 {
			get;
			set;
		}
		public string cbMedicalDirection1 {
			get;
			set;
		}
		public string cbMedicalSupervision1 {
			get;
			set;
		}
		public string CRNA2 {
			get;
			set;
		}
		public string CRNA2_label {
			get;
			set;
		}
		public string CrnaStrtTime2 {
			get;
			set;
		}
		public string CrnaEndTime2 {
			get;
			set;
		}
		public string cbMedicalDirection2 {
			get;
			set;
		}
		public string cbMedicalSupervision2 {
			get;
			set;
		}
		public string CRNA3 {
			get;
			set;
		}
		public string CRNA3_label {
			get;
			set;
		}
		public string CrnaStrtTime3 {
			get;
			set;
		}
		public string CrnaEndTime3 {
			get;
			set;
		}
		public string cbMedicalDirection3 {
			get;
			set;
		}
		public string cbMedicalSupervision3 {
			get;
			set;
		}
		public string CRNA4 {
			get;
			set;
		}
		public string CRNA4_label {
			get;
			set;
		}
		public string CrnaStrtTime4 {
			get;
			set;
		}
		public string CrnaEndTime4 {
			get;
			set;
		}
		public string MyProperty {
			get;
			set;
		}
		public string cbMedicalDirection4 {
			get;
			set;
		}
		public string cbMedicalSupervision4 {
			get;
			set;
		}
		public string Surgeon {
			get;
			set;
		}
		public string Surgeon1 {
			get;
			set;
		}
		public string Surgeon2 {
			get;
			set;
		}
		public string Surgeon3 {
			get;
			set;
		}
		public string ProcedureCode1 {
			get;
			set;
		}
		public string ProcedureCode2 {
			get;
			set;
		}
		public string ProcedureCode3 {
			get;
			set;
		}
		public string ProcedureCode4 {
			get;
			set;
		}
		public string DiagnosisCode1 {
			get;
			set;
		}
		public string DiagnosisCode2 {
			get;
			set;
		}
		public string DiagnosisCode3 {
			get;
			set;
		}
		public string DiagnosisCode4 {
			get;
			set;
		}
		public string delayedDuration {
			get;
			set;
		}
		public string Anesthesiologist1 {
			get;
			set;
		}
		public string Anesthesiologist2 {
			get;
			set;
		}
		public string Anesthesiologist3 {
			get;
			set;
		}
		public string Anesthesiologist4 {
			get;
			set;
		}
		public string ddlCancellationReasons {
			get;
			set;
		}
	}

}


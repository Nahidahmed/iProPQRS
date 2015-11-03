using System;
using System.IO;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Globalization;

namespace iProPQRSPortableLib
{
	public class AddPatient
	{
		public AddPatient ()
		{
		//	Isloaded = false;
			//PCLReadFile ("pqrs.html");
		}
		PatientProfile mPatient;
		PatientProcedureDetails patientProcedureDetails;
		Common cmn = new Common ();
		public string LoadHtml1(string Content,PatientProcedureDetails patientProcedure)
		{			
			patientProcedureDetails = patientProcedure;
			if (patientProcedureDetails == null)
				patientProcedureDetails = new PatientProcedureDetails ();
			
			string orNumStr = @"<option value='0'>-Select-</option>";

			List<ORList> orlist = Consts.Personnel.results.ORList;
			foreach (var item in orlist) {
				orNumStr = orNumStr + @"<option value='"+item.ORID+"'>'"+item.ORNO+"'</option>";
			}

			Content = Content.Replace (@"{{{ORNumber}}}", orNumStr);
			Content = Content.Replace (@"{{{Facility}}}", cmn.getCurrentFacilityName());

			cmn.updateCurrentFacility ();
			if (Consts.ProvidersOption == "Y") 
				Content = Content.Replace (@"{{{ProvidersOptionStyle}}}", "");
			else
				Content = Content.Replace (@"{{{ProvidersOptionStyle}}}", @"display:none");

			if(patientProcedureDetails!=null&& string.IsNullOrEmpty(patientProcedureDetails.ProcID.ToString()))
				Content = Content.Replace (@"{{{title}}}", @"New Patient");
			else
				Content = Content.Replace (@"{{{title}}}", @"Edit Patient");

			if (patientProcedureDetails!=null&&string.IsNullOrEmpty (patientProcedureDetails.DOB))
				Content = Content.Replace (@"{{{DOB}}}", @"");
			else {
				if (patientProcedureDetails == null && patientProcedureDetails.DOB == @"00010101" || patientProcedureDetails.DOB == @"") {
					Content = Content.Replace (@"{{{DOB}}}", @"01/01/1960");
				}else {
					DateTime dob = Convert.ToDateTime (patientProcedureDetails.DOB);
					Content = Content.Replace (@"{{{DOB}}}", dob.ToString("MM/dd/yyyy"));
					int age = DateTime.Now.Year - dob.Year;
					Content = Content.Replace (@"{{{Age}}}", age.ToString());
				}
			}
			// Hidden Carry Over
			//Content = Content.Replace("","");
//			Content = Content.Replace(@"{{{Sex}}}",mPatient.Sex);
			Content = Content.Replace(@"{{{MRN}}}",patientProcedureDetails.Mrn);
			Content = Content.Replace(@"{{{Firstname}}}",patientProcedureDetails.FirstName);
			Content = Content.Replace(@"{{{Lastname}}}",patientProcedureDetails.LastName);
//			Content = Content.Replace(@"{{{Height}}}",mPatient.Height);
//			Content = Content.Replace(@"{{{Weight}}}",mPatient.Weight);
//			Content = Content.Replace(@"{{{Allergies}}}",mPatient.Allergies);
//			Content = Content.Replace(@"{{{AccountNo}}}",mPatient.AccountNo);
			if (string.IsNullOrEmpty (patientProcedureDetails.ProcID.ToString())) {
				Content = Content.Replace (@"{{{OperationDate}}}", Consts.DataRetrieveDate);
				Content = Content.Replace (@"{{{ScheduleStartTime}}}", "");
			} else {
				Content = Content.Replace (@"{{{OperationDate}}}", patientProcedureDetails.OperationDate);
//				Content = Content.Replace (@"{{{ScheduleStartTime}}}", mPatient.ScheduleStartTime);
			}


			Content = Content.Replace(@"{{{Anesthesiologist1}}}","0");
			Content = Content.Replace(@"{{{Anesthesiologist1_label}}}",patientProcedureDetails.Anestheologist);
			Content = Content.Replace(@"{{{Anesthesiologist2}}}","1");
			Content = Content.Replace(@"{{{Anesthesiologist2_label}}}","");
			Content = Content.Replace(@"{{{Anesthesiologist3}}}","2");
			Content = Content.Replace(@"{{{Anesthesiologist3_label}}}","");
			Content = Content.Replace(@"{{{Anesthesiologist4}}}","3");
			Content = Content.Replace(@"{{{Anesthesiologist4_label}}}","");


			Content = Content.Replace(@"{{{AnesStrtTime1}}}","");
			Content = Content.Replace(@"{{{AnesStrtTime2}}}","");
			Content = Content.Replace(@"{{{AnesStrtTime3}}}","");
			Content = Content.Replace(@"{{{AnesStrtTime4}}}","");

//			Content = Content.Replace(@"{{{AnesStrtTime1}}}",mPatient.AnesStrtTime1);
//			Content = Content.Replace(@"{{{AnesStrtTime2}}}",mPatient.AnesStrtTime2);
//			Content = Content.Replace(@"{{{AnesStrtTime3}}}",mPatient.AnesStrtTime3);
//			Content = Content.Replace(@"{{{AnesStrtTime4}}}",mPatient.AnesStrtTime4);
//
			Content = Content.Replace(@"{{{CrnaStrtTime1}}}","");
			Content = Content.Replace(@"{{{CrnaStrtTime2}}}","");
			Content = Content.Replace(@"{{{CrnaStrtTime3}}}","");
			Content = Content.Replace(@"{{{CrnaStrtTime4}}}","");

//			Content = Content.Replace(@"{{{CrnaStrtTime1}}}",mPatient.CrnaStrtTime1);
//			Content = Content.Replace(@"{{{CrnaStrtTime2}}}",mPatient.CrnaStrtTime2);
//			Content = Content.Replace(@"{{{CrnaStrtTime3}}}",mPatient.CrnaStrtTime3);
//			Content = Content.Replace(@"{{{CrnaStrtTime4}}}",mPatient.CrnaStrtTime4);
//
			Content = Content.Replace(@"{{{AnesEndTime1}}}","");
			Content = Content.Replace(@"{{{AnesEndTime2}}}","");
			Content = Content.Replace(@"{{{AnesEndTime3}}}","");
			Content = Content.Replace(@"{{{AnesEndTime4}}}","");

//			Content = Content.Replace(@"{{{AnesEndTime1}}}",mPatient.AnesEndTime1);
//			Content = Content.Replace(@"{{{AnesEndTime2}}}",mPatient.AnesEndTime2);
//			Content = Content.Replace(@"{{{AnesEndTime3}}}",mPatient.AnesEndTime3);
//			Content = Content.Replace(@"{{{AnesEndTime4}}}",mPatient.AnesEndTime4);
//
			Content = Content.Replace(@"{{{CrnaEndTime1}}}","");
			Content = Content.Replace(@"{{{CrnaEndTime2}}}","");
			Content = Content.Replace(@"{{{CrnaEndTime3}}}","");
			Content = Content.Replace(@"{{{CrnaEndTime4}}}","");

//			Content = Content.Replace(@"{{{CrnaEndTime1}}}",mPatient.CrnaEndTime1);
//			Content = Content.Replace(@"{{{CrnaEndTime2}}}",mPatient.CrnaEndTime2);
//			Content = Content.Replace(@"{{{CrnaEndTime3}}}",mPatient.CrnaEndTime3);
//			Content = Content.Replace(@"{{{CrnaEndTime4}}}",mPatient.CrnaEndTime4);
//
			Content = Content.Replace(@"{{{SrnaStrtTime1}}}","");
			Content = Content.Replace (@"{{{SrnaEndTime1}}}", "");
				
//			Content = Content.Replace(@"{{{SrnaStrtTime1}}}",mPatient.SrnaStrtTime1);
//			Content = Content.Replace(@"{{{SrnaEndTime1}}}",mPatient.SrnaEndTime1);
//
			Content = Content.Replace(@"{{{Diagnosis1}}}","");
			Content = Content.Replace(@"{{{Diagnosis2}}}","");
			Content = Content.Replace(@"{{{Diagnosis3}}}","");
			Content = Content.Replace(@"{{{Diagnosis4}}}","");

//			Content = Content.Replace(@"{{{Diagnosis1}}}",mPatient.Diagnosis1);
//			Content = Content.Replace(@"{{{Diagnosis2}}}",mPatient.Diagnosis2);
//			Content = Content.Replace(@"{{{Diagnosis3}}}",mPatient.Diagnosis3);
//			Content = Content.Replace(@"{{{Diagnosis4}}}",mPatient.Diagnosis4);

			Content = Content.Replace(@"{{{DiagnosisCode1}}}","");
			Content = Content.Replace(@"{{{DiagnosisCode2}}}","");
			Content = Content.Replace(@"{{{DiagnosisCode3}}}","");
			Content = Content.Replace(@"{{{DiagnosisCode4}}}","");

//			Content = Content.Replace(@"{{{DiagnosisCode1}}}",mPatient.DiagnosisCode1);
//			Content = Content.Replace(@"{{{DiagnosisCode2}}}",mPatient.DiagnosisCode2);
//			Content = Content.Replace(@"{{{DiagnosisCode3}}}",mPatient.DiagnosisCode3);
//			Content = Content.Replace(@"{{{DiagnosisCode4}}}",mPatient.DiagnosisCode4);
//
			Content = Content.Replace(@"{{{Procedures1}}}","");
			Content = Content.Replace(@"{{{Procedures2}}}","");
			Content = Content.Replace(@"{{{Procedures3}}}","");
			Content = Content.Replace(@"{{{Procedures4}}}","");

//			Content = Content.Replace(@"{{{Procedures1}}}",mPatient.Procedures1);
//			Content = Content.Replace(@"{{{Procedures2}}}",mPatient.Procedures2);
//			Content = Content.Replace(@"{{{Procedures3}}}",mPatient.Procedures3);
//			Content = Content.Replace(@"{{{Procedures4}}}",mPatient.Procedures4);
//
			Content = Content.Replace(@"{{{ProcedureCode1}}}","");
			Content = Content.Replace(@"{{{ProcedureCode2}}}","");
			Content = Content.Replace(@"{{{ProcedureCode3}}}","");
			Content = Content.Replace(@"{{{ProcedureCode4}}}","");

//			Content = Content.Replace(@"{{{ProcedureCode1}}}",mPatient.ProcedureCode1);
//			Content = Content.Replace(@"{{{ProcedureCode2}}}",mPatient.ProcedureCode2);
//			Content = Content.Replace(@"{{{ProcedureCode3}}}",mPatient.ProcedureCode3);
//			Content = Content.Replace(@"{{{ProcedureCode4}}}",mPatient.ProcedureCode4);
			return Content;
		}
		public string LoadHtml(string Content,PatientProfile Patient)
		{			
			mPatient = Patient;
			if (mPatient == null)
				mPatient = new PatientProfile ();
			string orNumStr = @"<option value='0'>-Select-</option>";
			List<ORList> orlist = Consts.Personnel.results.ORList;
			foreach (var item in orlist) {
				orNumStr = orNumStr + @"<option value='"+item.ORID+"'>'"+item.ORNO+"'</option>";
			}
			Content = Content.Replace (@"{{{ORNumber}}}", orNumStr);
			Content = Content.Replace (@"{{{Facility}}}", cmn.getCurrentFacilityName());
			cmn.updateCurrentFacility ();
			if (Consts.ProvidersOption == "Y") 
				Content = Content.Replace (@"{{{ProvidersOptionStyle}}}", "");
			else
				Content = Content.Replace (@"{{{ProvidersOptionStyle}}}", @"display:none");

			if(mPatient!=null&& string.IsNullOrEmpty(mPatient.ProcID.ToString()))
				Content = Content.Replace (@"{{{title}}}", @"New Patient");
			else
				Content = Content.Replace (@"{{{title}}}", @"Edit Patient");

			if (mPatient!=null&&string.IsNullOrEmpty (mPatient.DOB))
				Content = Content.Replace (@"{{{DOB}}}", @"");
			else {
				if (mPatient == null && mPatient.DOB == @"00010101" || mPatient.DOB == @"") {
					Content = Content.Replace (@"{{{DOB}}}", @"01/01/1960");
				}else {
					DateTime dob = DateTime.ParseExact (mPatient.DOB, "yyyyMMdd", CultureInfo.InvariantCulture);
					Content = Content.Replace (@"{{{DOB}}}", dob.ToString("yyyy/MM/dd"));
					int age = DateTime.Now.Year - dob.Year;
					Content = Content.Replace (@"{{{Age}}}", age.ToString());
				}
			}
						// Hidden Carry Over
			//Content = Content.Replace("","");
			Content = Content.Replace(@"{{{Sex}}}",mPatient.Sex);
			Content = Content.Replace(@"{{{MRN}}}",mPatient.MRN);
			Content = Content.Replace(@"{{{Firstname}}}",mPatient.FirstName);
			Content = Content.Replace(@"{{{Lastname}}}",mPatient.LastName);
			Content = Content.Replace(@"{{{Height}}}",mPatient.Height);
			Content = Content.Replace(@"{{{Weight}}}",mPatient.Weight);
			Content = Content.Replace(@"{{{Allergies}}}",mPatient.Allergies);
			Content = Content.Replace(@"{{{AccountNo}}}",mPatient.AccountNo);
			if (string.IsNullOrEmpty (mPatient.ProcID.ToString())) {
				Content = Content.Replace (@"{{{OperationDate}}}", Consts.DataRetrieveDate);
				Content = Content.Replace (@"{{{ScheduleStartTime}}}", "");
			} else {
				Content = Content.Replace (@"{{{OperationDate}}}", mPatient.OperationDate);
				Content = Content.Replace (@"{{{ScheduleStartTime}}}", mPatient.ScheduleStartTime);
			}

			Content = Content.Replace(@"{{{AnesStrtTime1}}}",mPatient.AnesStrtTime1);
			Content = Content.Replace(@"{{{AnesStrtTime2}}}",mPatient.AnesStrtTime2);
			Content = Content.Replace(@"{{{AnesStrtTime3}}}",mPatient.AnesStrtTime3);
			Content = Content.Replace(@"{{{AnesStrtTime4}}}",mPatient.AnesStrtTime4);

			Content = Content.Replace(@"{{{CrnaStrtTime1}}}",mPatient.CrnaStrtTime1);
			Content = Content.Replace(@"{{{CrnaStrtTime2}}}",mPatient.CrnaStrtTime2);
			Content = Content.Replace(@"{{{CrnaStrtTime3}}}",mPatient.CrnaStrtTime3);
			Content = Content.Replace(@"{{{CrnaStrtTime4}}}",mPatient.CrnaStrtTime4);

			Content = Content.Replace(@"{{{AnesEndTime1}}}",mPatient.AnesEndTime1);
			Content = Content.Replace(@"{{{AnesEndTime2}}}",mPatient.AnesEndTime2);
			Content = Content.Replace(@"{{{AnesEndTime3}}}",mPatient.AnesEndTime3);
			Content = Content.Replace(@"{{{AnesEndTime4}}}",mPatient.AnesEndTime4);

			Content = Content.Replace(@"{{{CrnaEndTime1}}}",mPatient.CrnaEndTime1);
			Content = Content.Replace(@"{{{CrnaEndTime2}}}",mPatient.CrnaEndTime2);
			Content = Content.Replace(@"{{{CrnaEndTime3}}}",mPatient.CrnaEndTime3);
			Content = Content.Replace(@"{{{CrnaEndTime4}}}",mPatient.CrnaEndTime4);

			Content = Content.Replace(@"{{{SrnaStrtTime1}}}",mPatient.SrnaStrtTime1);
			Content = Content.Replace(@"{{{SrnaEndTime1}}}",mPatient.SrnaEndTime1);

			Content = Content.Replace(@"{{{Diagnosis1}}}",mPatient.Diagnosis1);
			Content = Content.Replace(@"{{{Diagnosis2}}}",mPatient.Diagnosis2);
			Content = Content.Replace(@"{{{Diagnosis3}}}",mPatient.Diagnosis3);
			Content = Content.Replace(@"{{{Diagnosis4}}}",mPatient.Diagnosis4);

			Content = Content.Replace(@"{{{DiagnosisCode1}}}",mPatient.DiagnosisCode1);
			Content = Content.Replace(@"{{{DiagnosisCode2}}}",mPatient.DiagnosisCode2);
			Content = Content.Replace(@"{{{DiagnosisCode3}}}",mPatient.DiagnosisCode3);
			Content = Content.Replace(@"{{{DiagnosisCode4}}}",mPatient.DiagnosisCode4);

			Content = Content.Replace(@"{{{Procedures1}}}",mPatient.Procedures1);
			Content = Content.Replace(@"{{{Procedures2}}}",mPatient.Procedures2);
			Content = Content.Replace(@"{{{Procedures3}}}",mPatient.Procedures3);
			Content = Content.Replace(@"{{{Procedures4}}}",mPatient.Procedures4);

			Content = Content.Replace(@"{{{ProcedureCode1}}}",mPatient.ProcedureCode1);
			Content = Content.Replace(@"{{{ProcedureCode2}}}",mPatient.ProcedureCode2);
			Content = Content.Replace(@"{{{ProcedureCode3}}}",mPatient.ProcedureCode3);
			Content = Content.Replace(@"{{{ProcedureCode4}}}",mPatient.ProcedureCode4);
			return Content;
		}
		public string CheckText(string Val)
		{
			if (string.IsNullOrEmpty (Val))
				Val = "";
			return Val;
		}
		public Task<string>  Htmlstr {
			get;
			set;
		}
		public bool Isloaded {
			get;
			set;
		}
		public string ErrorMsg {
			get;
			set;
		}
		//public async Task<string> PCLReadFile (string name)
		///{
			//try
			//{
			//IFolder localStorage = FileSystem. Current. LocalStorage;

			//IFolder contentFolder = await localStorage. GetFolderAsync ("Content");

			//IFile file = await contentFolder. GetFileAsync (name);
			//Isloaded = true;
			//return await file. ReadAllTextAsync ();
			//}
			//catch (Exception ex) {
			//	ErrorMsg = ex.Message;
			//	return null;
			//}
		//}

	}

}


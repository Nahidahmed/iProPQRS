using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace iProPQRSPortableLib.BL.PQRSManager
{
	public class PQRSManager
	{
		PQRSServices salObj;
		public PQRSManager ()
		{
			salObj = new PQRSServices ();
		}

		public Task<PatientProcedureInfo> AuthenticationGateway(string username,string password)
		{
			return salObj.AuthGatway (username,password);
		}

		public Task<RcvdJSONData> AuthenticateCode(string Code)
		{
			return salObj.AuthenticateCode (Code);
		}

		public Task<PatientProcedureInfo> GetPatientProcedureInfo()
		{
			return salObj.GetProcedurePatientList ();
		}

		public Task<AttribTypesOfProcedure> GetAllAttribTypesOfAProcedure(int procID)
		{
			return salObj.GetAllAttribTypesOfAProcedure (procID);
		}

		public Task<RootObject> GetPatientData()
		{
			if(Consts.SelectedFacilityID == string.Empty)
				Consts.SelectedFacilityID = "1";

			string patientURL = Consts.BaseServerURL + "/"+Consts.WebServiceVersion+"/newpatientlist.aspx?token=" + Consts.AuthenticationToken + "&Date="+ Consts.DataRetrieveDate +"&type=Main&facilityID="+Consts.SelectedFacilityID+"&userid=" + Consts.AuthenticationUserID;
			return salObj.GetPatientData (patientURL);
		}
		public Task<ProcedureDiagnosticMaster> GetProcedureDiagnosticMaster(int ProcID)
		{
			string url =  Consts.BaseServerURL+ "/anesservices/ProcCodes/"+ProcID;
			return salObj.GetProcedureDiagnosticMaster (url);
		}

		public Task<ReceiveContext> GetProcedureParticipants(int ProcID)
		{
			string url =  Consts.BaseServerURL+ "/anesservices/ProcParticipants/"+ProcID;
			return salObj.GetDetails (url);
		}

		public Task<ReceiveContext> DeleteProcedureParticipant(int ProcParticipantID)
		{
			string url =  Consts.BaseServerURL+ "/anesservices/ProcParticipants/"+ProcParticipantID.ToString();
			return salObj.DeleteItem (url);
		}

//		public Task<ReceiveContext> UpdateProcedureParticipant(ProcedureParticipantDetails participant)
//		{
//			string url =  Consts.BaseServerURL+ "/anesservices/ProcParticipants/"+participant.ProcParticipantID.ToString();
//			return salObj.UpdateProcedureParticipant (url,participant);
//		}

		public Task<ReceiveContext> GetUsers()
		{
			string url =  Consts.BaseServerURL+ "/anesservices/users";
			return salObj.GetDetails (url);
		}

		public Task<ReceiveContext> GetSurgeons()
		{
			string url =  Consts.BaseServerURL+ "/anesservices/Surgeons";
			return salObj.GetDetails (url);
		}

		public Task<RcvdJSONData> GetMACCodes()
		{
			string url =  "http://reference.iprocedures.com/mac";
			return salObj.GetJSONData (url);
		}


		public Task<ReceiveContext> GetLastUsedProceduresDiagnosis(string codeType)
		{
			string url =  Consts.BaseServerURL+ "/anesservices/proccodes/lastn/"+codeType +"/5";
			return salObj.GetDetails (url);
		}

		public Task<ReceiveContext> GetSelectedSurgeonOfProcedure(int procID)
		{
			string url =  Consts.BaseServerURL+ "/anesservices/ProcSurgeons/"+procID.ToString();
			return salObj.GetDetails (url);
		}

		public void UploadPatientData(Patient patient)
		{
			salObj.UploadPatientData (patient);
		}

		public Task<ReceiveContext> UpdateProcAttribs(List<AttribType> procattribtslist)
		{
			return salObj.UpdateProcAttribs (procattribtslist);
		}
		public Task<ReceiveContext> UpdatePatintProcedureInfo(PatientProcedureFullDetails ProcDetails)
		{
			return salObj.UpdatePatintProcedureInfo (ProcDetails);
		}
		public Task<ReceiveContext> GetPatientProcFullDetails(int ProcID)
		{
			return salObj.GetPatientProcFullDetails (ProcID);
		}

		public Task<ReceiveContext> UpdateProcedureDiagnostic(DataResults procedurediagnosticlist)
		{
			return salObj.UpdateProcedureDiagnostic (procedurediagnosticlist);
		}

		public Task<ReceiveContext> AddUpdateProcedureParticipants(ProcedureParticipantDetails participant)
		{
			string url =  Consts.BaseServerURL+ "/anesservices/ProcParticipants/"+participant.ProcID.ToString();
			string putData = JsonConvert.SerializeObject(participant,Formatting.Indented);			
			return salObj.UpdateDetails (url,putData);
		}

		public Task<ReceiveContext> AddUpdatePatintInfo(Patient Profile)
		{
			string url =  Consts.BaseServerURL+"/anesservices/patients";
			string putData = JsonConvert.SerializeObject(Profile,Formatting.Indented);			
			return salObj.UpdateDetails (url,putData);
		}

		public Task<ReceiveContext> AddUpdateProcedureSurgeon(ProcedureSurgeonDetails procSurgeon)
		{
			string url =  Consts.BaseServerURL+ "/anesservices/ProcSurgeons";
			string putData = JsonConvert.SerializeObject(procSurgeon,Formatting.Indented);			
			return salObj.UpdateDetails (url,putData);
		}

		public Task<ReceiveContext> DeleteProcedureDiagnostic(int ProcCodeID)
		{
			return salObj.DeleteProcedureDiagnostic(ProcCodeID);
		}
	}
}


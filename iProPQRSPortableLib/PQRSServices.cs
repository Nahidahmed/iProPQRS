using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using iProPQRSPortableLib.BL;
using System.Threading.Tasks;
using System.Net.Http;



namespace iProPQRSPortableLib
{
	public class PQRSServices
	{
		public PQRSServices ()
		{
		}
		Patient patient;
		ResponseInfo responseObj;
		private static ManualResetEvent allDone = new ManualResetEvent(false);

		public async Task<PatientProcedureInfo> AuthGatway(string username,string password)
		{
			if (Consts.BaseServerURL == string.Empty)
				Consts.BaseServerURL = "http://test.iprocedures.com";

			Uri uri = new Uri(Consts.BaseServerURL+"/anesservices/Authenticate");

			PatientProcedureInfo pats = new PatientProcedureInfo();
			FacilityInfo facilities = new FacilityInfo ();
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			request.Method = "GET";
			request.Headers ["Username"] = username;
			request.Headers ["Password"] = password;
			string received;

			using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null)))
			{
				using (var responseStream = response.GetResponseStream())
				{
					using (var sr = new StreamReader(responseStream))
					{
						received = await sr.ReadToEndAsync();
					}
				}
			}

			AuthGateway responseObj = (AuthGateway)JsonConvert.DeserializeObject(received , typeof(AuthGateway));


			if (responseObj.status == "ok") {
				Consts.BaseServerURL = "http://test.iprocedures.com";//authenticatedUser.url;
				Consts.SelectedFacilityID = responseObj.result[0].DefaultFacilityID.ToString();
				Consts.AuthenticationToken = responseObj.result[0].Token;//  authenticatedUser.token;
				Consts.Role = responseObj.result[0].Role;
				Consts.UserFirstName = responseObj.result [0].FirstName.Trim ();
				Consts.UserLastName = responseObj.result [0].LastName.Trim ();
				Consts.LoginUserFullName = responseObj.result[0].LastName.Trim() + ", " + responseObj.result[0].FirstName.Trim();
				facilities = await GetFacilities (Consts.BaseServerURL);

				pats =  await GetProcedurePatientList ();
				string url =  Consts.BaseServerURL+ "/anesservices/users";
				ReceiveContext users = await GetDetails(url);
				if (users != null && users.result != null) {
					Consts.lstOfUsers = (List<UserDetails>)JsonConvert.DeserializeObject (users.result.ToString (), typeof(List<UserDetails>));
				}
				Consts.ProcAttribTypes = await GetProcAttribTypes (Consts.BaseServerURL+"/anesservices/ProcAttribTypes");
				Consts.ProcAttribTypes.result.Types.Sort((xx,yy)=> xx.Priority.CompareTo(yy.Priority));
			} 

			return pats;
		}



		public async Task<RootObject> GetPatientData(string url)
		{
			Uri uri = new Uri(url);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			string received;

			using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null)))
			{
				using (var responseStream = response.GetResponseStream())
				{
					using (var sr = new StreamReader(responseStream))
					{
						received = await sr.ReadToEndAsync();
					}
				}
			}

			RootObject responseObj = (RootObject)JsonConvert.DeserializeObject(received , typeof(RootObject));
			return responseObj;
		}

		public async Task<ProcedureDiagnosticMaster> GetProcedureDiagnosticMaster(string url)
		{
			Uri uri = new Uri(url);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			request.Headers ["Token"] =Consts.AuthenticationToken;
			string received;

			using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null))){
				using (var responseStream = response.GetResponseStream()){
					using (var sr = new StreamReader(responseStream)){
						received = await sr.ReadToEndAsync();
					}
				}
			}

			ProcedureDiagnosticMaster responseObj = (ProcedureDiagnosticMaster)JsonConvert.DeserializeObject(received , typeof(ProcedureDiagnosticMaster));
			//Consts.Facilities = responseObj;
			return responseObj;
		}

		public async Task<ProcAttribTypes> GetProcAttribTypes(string url)
		{
			Uri uri = new Uri(url);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			request.Headers ["Token"] =Consts.AuthenticationToken;

			string received;

			using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null))){
				using (var responseStream = response.GetResponseStream()){
					using (var sr = new StreamReader(responseStream)){
						received = await sr.ReadToEndAsync();
					}
				}
			}

			ProcAttribTypes responseObj = (ProcAttribTypes)JsonConvert.DeserializeObject(received , typeof(ProcAttribTypes));
			return responseObj;
		}

		public async Task<FacilityInfo> GetFacilities(string url)
		{
			url += "/anesservices/Facilities";
			Uri uri = new Uri(url);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			request.Headers ["Token"] =Consts.AuthenticationToken;
			string received;

			using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null)))
			{
				using (var responseStream = response.GetResponseStream())
				{
					using (var sr = new StreamReader(responseStream))
					{
						received = await sr.ReadToEndAsync();
					}
				}
			}

			FacilityInfo responseObj = (FacilityInfo)JsonConvert.DeserializeObject(received , typeof(FacilityInfo));
			Consts.Facilities = responseObj;
			return responseObj;
		}

		public async Task<PatientProcedureInfo> GetProcedurePatientList()
		{
			if(Consts.SelectedFacilityID == string.Empty)
				Consts.SelectedFacilityID = "1";
			
			string url = Consts.BaseServerURL + "/anesservices/Procs"+"/"+Consts.DataRetrieveDate+"/"+Consts.SelectedFacilityID+"/Main";

			//call format: Procs/date/facilityID/listType
			Uri uri = new Uri(url);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			request.Headers ["Token"] = Consts.AuthenticationToken;
			string received;

			using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null)))
			{
				using (var responseStream = response.GetResponseStream())
				{
					using (var sr = new StreamReader(responseStream))
					{
						received = await sr.ReadToEndAsync();
					}
				}
			}

			Consts.PatientInfo = (PatientProcedureInfo)JsonConvert.DeserializeObject(received , typeof(PatientProcedureInfo));
			return Consts.PatientInfo;
		}

		public async void UploadPatientData(Patient patient)
		{
			this.patient = patient;
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Consts.BaseServerURL + "/anesservices/Patients");


			request.ContentType = "application/json; charset=utf-8";
			request.Headers ["Token"] = Consts.AuthenticationToken;
			// Set the Method property to 'POST' to post data to the URI.
			request.Method = "PUT";

			string received;

			string jsonData = JsonConvert.SerializeObject(this.patient, Formatting.Indented);
			byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);

			using (var stream = await Task.Factory.FromAsync<Stream>(request.BeginGetRequestStream,request.EndGetRequestStream,null))
			{
				stream.Write (byteArray, 0, byteArray.Length);
			}

			using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null)))
			{
				using (var responseStream = response.GetResponseStream())
				{
					using (var sr = new StreamReader(responseStream))
					{
						received = await sr.ReadToEndAsync();
					}
				}
			}

			System.Diagnostics.Debug.WriteLine(received);
		}


		public async Task<ReceiveContext> UpdatePatintProcedureInfo(PatientProcedureFullDetails Profile)
		{
			Stream requestWriter;
			string received=string.Empty;
			ReceiveContext responseObj;
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Consts.BaseServerURL + "/anesservices/procs");


				request.ContentType = "application/json; charset=utf-8";

				// Set the Method property to 'POST' to post data to the URI.
				request.Method = "PUT";
				request.Headers ["Token"] =Consts.AuthenticationToken;

				requestWriter = await request.GetRequestStreamAsync();
				string postData=JsonConvert.SerializeObject(Profile,Formatting.Indented);
				//string postData = "{\n      \"ID\": "+Profile.ID+",\n      \"Mrn\": \"54576576876\",\n      \"FirstName\": \"Johnkumar\",\n      \"LastName\": \"Doe1\",\n      \"Sex\": \"M\",\n      \"Height\": \"5)6\",\n      \"Weight\": \"67\",\n      \"Allergies\": \"Tablet\",\n      \"PersonProvidingInfo\": \"Patient\",\n      \"DOB\": \"1962-02-02T00:00:00\",\n      \"AccountNo\": \"3564546466\",\n      \"HNENumber\": null,\n      \"ddlEncounterType\": null,\n      \"ddlASAType\": null,\n      \"cbEmergency\": null,\n      \"Race\": null,\n      \"DeathDate\": null,\n      \"DeathIndicator\": null,\n      \"CreatedOn\": null,\n      \"LastModifiedDate\": \"2015-07-15T06:12:11.52\",\n      \"LastModifiedBy\": \"admin\",\n      \"RecordType\": null,\n      \"FacilityID\": null,\n      \"SSN\": null,\n      \"PrimaryLanguage\": null,\n      \"Ethnicity\": null,\n      \"HomePhone\": null,\n      \"CellPhone\": null,\n      \"MaritalStatus\": null,\n      \"QuickAdd\": \"N\",\n      \"AptID\": null,\n      \"isDFTExported\": null,\n      \"ddlHghtType\": null,\n      \"ddlWghtType\": null,\n      \"tbBMI\": null,\n      \"cbNKDA\": null,\n      \"cbIVDye\": null\n    }\n";
				//string postData = "{\n      \"ID\": 17517,\n      \"Mrn\": \"54576576876\",\n      \"FirstName\": \"Johnkumar\",\n      \"LastName\": \"Doe1\",\n      \"Sex\": \"M\",\n      \"Height\": \"5)6\",\n      \"Weight\": \"67\",\n      \"Allergies\": \"Tablet\",\n      \"PersonProvidingInfo\": \"Patient\",\n      \"DOB\": \"1962-02-02T00:00:00\",\n      \"AccountNo\": \"3564546466\",\n      \"HNENumber\": null,\n      \"ddlEncounterType\": null,\n      \"ddlASAType\": null,\n      \"cbEmergency\": null,\n      \"Race\": null,\n      \"DeathDate\": null,\n      \"DeathIndicator\": null,\n      \"CreatedOn\": null,\n      \"LastModifiedDate\": \"2015-07-15T06:12:11.52\",\n      \"LastModifiedBy\": \"admin\",\n      \"RecordType\": null,\n      \"FacilityID\": null,\n      \"SSN\": null,\n      \"PrimaryLanguage\": null,\n      \"Ethnicity\": null,\n      \"HomePhone\": null,\n      \"CellPhone\": null,\n      \"MaritalStatus\": null,\n      \"QuickAdd\": \"N\",\n      \"AptID\": null,\n      \"isDFTExported\": null,\n      \"ddlHghtType\": null,\n      \"ddlWghtType\": null,\n      \"tbBMI\": null,\n      \"cbNKDA\": null,\n      \"cbIVDye\": null\n    }\n";
				byte[] data = Encoding.UTF8.GetBytes(postData);
				requestWriter.Write(data,0,data.Length);
				WebResponse resp = await request.GetResponseAsync();
				using (var responseStream = resp.GetResponseStream())
				{
					using (var sr = new StreamReader(responseStream))
					{
						received = await sr.ReadToEndAsync();
					}
				}
			}
			catch {

			}
			responseObj = (ReceiveContext)JsonConvert.DeserializeObject(received , typeof(ReceiveContext));
			return responseObj;
		}

		public async Task<ReceiveContext> GetPatientDetails(int patientID)
		{
			Stream requestWriter;
			string received=string.Empty;
			ReceiveContext responseObj;
			try
			{
//				HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://test.iprocedures.com/anesservices/Patients/"+patientID);
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Consts.BaseServerURL + "/anesservices/Patients/"+patientID);

				request.ContentType = "application/json; charset=utf-8";

				// Set the Method property to 'POST' to post data to the URI.
				request.Method = "GET";
				request.Headers ["Token"] =Consts.AuthenticationToken;

				using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null)))
				{
					using (var responseStream = response.GetResponseStream())
					{
						using (var sr = new StreamReader(responseStream))
						{
							received = await sr.ReadToEndAsync();
						}
					}
				}
			}
			catch (Exception ex)
			{
				string str = ex.Message;
			}
			responseObj = (ReceiveContext)JsonConvert.DeserializeObject(received , typeof(ReceiveContext));
			return responseObj;
		}

		public async Task<ReceiveContext> CheckExistingPatintInfo(string mrn,string facilityID)
		{
			Stream requestWriter;
			string received=string.Empty;
			ReceiveContext responseObj;
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Consts.BaseServerURL+"/anesservices/Patients/mrn/"+mrn+"/"+facilityID);

				request.ContentType = "application/json; charset=utf-8";

				// Set the Method property to 'POST' to post data to the URI.
				request.Method = "GET";
				request.Headers ["Token"] =Consts.AuthenticationToken;

				using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null)))
				{
					using (var responseStream = response.GetResponseStream())
					{
						using (var sr = new StreamReader(responseStream))
						{
							received = await sr.ReadToEndAsync();
						}
					}
				}
			}
			catch (Exception ex)
			{
				string str = ex.Message;
			}
			responseObj = (ReceiveContext)JsonConvert.DeserializeObject(received , typeof(ReceiveContext));
			return responseObj;
		}

		public async Task<RcvdJSONData> AuthenticateCode(string Code)
		{
//			Stream requestWriter;
			string received=string.Empty;
			RcvdJSONData responseObj;
			try
			{
//				HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://test.iprocedures.com/anesservices/Domain/"+Code);
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://reference.iprocedures.com/domain/"+Code);
//				request.ContentType = "application/json; charset=utf-8";

				// Set the Method property to 'POST' to post data to the URI.
				request.Method = "GET";

				using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null))){
					using (var responseStream = response.GetResponseStream()){
						using (var sr = new StreamReader(responseStream)){
							received = await sr.ReadToEndAsync();
						}
					}
				}
			}catch (Exception ex){
				string str = ex.Message;
			}

			responseObj = (RcvdJSONData)JsonConvert.DeserializeObject(received , typeof(RcvdJSONData));
			return responseObj;


//			Uri uri = new Uri(url);
//			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
//			string received;
//
//			using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null))){
//				using (var responseStream = response.GetResponseStream()){
//					using (var sr = new StreamReader(responseStream)){
//						received = await sr.ReadToEndAsync();
//					}
//				}
//			}
//
//			ReceiveContext responseObj = (ReceiveContext)JsonConvert.DeserializeObject(received , typeof(ReceiveContext));
//			return responseObj;
		}

		public async Task<AttribTypesOfProcedure> GetAllAttribTypesOfAProcedure(int procID)
		{
			string url = Consts.BaseServerURL + "/anesservices/ProcAttribs/" + procID.ToString();
			Uri uri = new Uri(url);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			request.Headers ["Token"] = Consts.AuthenticationToken;

			string received;

			using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null))){
				using (var responseStream = response.GetResponseStream()){
					using (var sr = new StreamReader(responseStream)){
						received = await sr.ReadToEndAsync();
					}
				}
			}

			try {
				AttribTypesOfProcedure responseObj = (AttribTypesOfProcedure)JsonConvert.DeserializeObject(received , typeof(AttribTypesOfProcedure));
				return responseObj;
			}catch(Exception ex){
				System.Diagnostics.Debug.WriteLine (ex.Message);
				return null;
			}

		}

		public async Task<ReceiveContext> UpdateProcAttribs(List<AttribType> procattribtslist)
		{
			Stream requestWriter;
			string received=string.Empty;
			ReceiveContext responseObj;
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Consts.BaseServerURL+"/anesservices/ProcAttribs");

				request.ContentType = "application/json; charset=utf-8";
				// Set the Method property to 'POST' to post data to the URI.
				request.Method = "PUT";
				request.Headers ["Token"] =Consts.AuthenticationToken;
				requestWriter = await request.GetRequestStreamAsync();
				string postData=JsonConvert.SerializeObject(procattribtslist,Formatting.Indented);			
				byte[] data = Encoding.UTF8.GetBytes(postData);
				requestWriter.Write(data,0,data.Length);
				WebResponse resp = await request.GetResponseAsync();
				using (var responseStream = resp.GetResponseStream())
				{
					using (var sr = new StreamReader(responseStream))
					{
						received = await sr.ReadToEndAsync();
					}
				}
			}
			catch(Exception ex) {
				System.Diagnostics.Debug.WriteLine ("UpdateProcAttribs Exception: "+ex.Message);
			}
			responseObj = (ReceiveContext)JsonConvert.DeserializeObject(received , typeof(ReceiveContext));
			return responseObj;
		}

		public async Task<ReceiveContext> GetPatientProcFullDetails(int ProcId)
		{
			Stream requestWriter;
			string received=string.Empty;
			ReceiveContext responseObj;
			try
			{				
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Consts.BaseServerURL + "/anesservices/procs/"+ProcId);
				request.ContentType = "application/json; charset=utf-8";
				// Set the Method property to 'POST' to post data to the URI.
				request.Method = "GET";
				request.Headers ["Token"] =Consts.AuthenticationToken;

				using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null)))
				{
					using (var responseStream = response.GetResponseStream())
					{
						using (var sr = new StreamReader(responseStream))
						{
							received = await sr.ReadToEndAsync();
						}
					}
				}
			}
			catch (Exception ex)
			{
				string str = ex.Message;
			}
			responseObj = (ReceiveContext)JsonConvert.DeserializeObject(received , typeof(ReceiveContext));
			return responseObj;
		}

		public async Task<ReceiveContext> UpdateProcedureDiagnostic(DataResults procedurediagnosticlist)
		{
			Stream requestWriter;
			string received=string.Empty;
			ReceiveContext responseObj;
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Consts.BaseServerURL+"/anesservices/ProcCodes");

				request.ContentType = "application/json; charset=utf-8";
				// Set the Method property to 'POST' to post data to the URI.
				request.Method = "PUT";
				request.Headers ["Token"] =Consts.AuthenticationToken;
				requestWriter = await request.GetRequestStreamAsync();
				string postData=JsonConvert.SerializeObject(procedurediagnosticlist,Formatting.Indented);			
				byte[] data = Encoding.UTF8.GetBytes(postData);
				requestWriter.Write(data,0,data.Length);
				WebResponse resp = await request.GetResponseAsync();
				using (var responseStream = resp.GetResponseStream())
				{
					using (var sr = new StreamReader(responseStream))
					{
						received = await sr.ReadToEndAsync();
					}
				}
			}
			catch {

			}
			responseObj = (ReceiveContext)JsonConvert.DeserializeObject(received , typeof(ReceiveContext));
			return responseObj;
		}

		public async Task<ReceiveContext> GetDetails(string url)
		{
			Uri uri = new Uri(url);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			request.Headers ["Token"] = Consts.AuthenticationToken;
			string received;

			using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null))){
				using (var responseStream = response.GetResponseStream()){
					using (var sr = new StreamReader(responseStream)){
						received = await sr.ReadToEndAsync();
					}
				}
			}

			ReceiveContext responseObj = (ReceiveContext)JsonConvert.DeserializeObject(received , typeof(ReceiveContext));
			return responseObj;
		}

		public async Task<RcvdJSONData> GetJSONData(string url)
		{
			Uri uri = new Uri(url);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			request.Headers ["Token"] = Consts.AuthenticationToken;
			string received;

			using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null))){
				using (var responseStream = response.GetResponseStream()){
					using (var sr = new StreamReader(responseStream)){
						received = await sr.ReadToEndAsync();
					}
				}
			}

			RcvdJSONData responseObj = (RcvdJSONData)JsonConvert.DeserializeObject(received , typeof(RcvdJSONData));
			iProPQRSPortableLib.Consts.RcvdJSONDataResult = responseObj;
			return responseObj;
		}

		public async Task<ReceiveContext> UpdateDetails(string url,string dataToPUT)
		{
			Stream requestWriter;
			string received=string.Empty;
			ReceiveContext responseObj;
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.ContentType = "application/json; charset=utf-8";
				request.Method = "PUT";
				request.Headers ["Token"] = Consts.AuthenticationToken;
				requestWriter = await request.GetRequestStreamAsync();
				byte[] data = Encoding.UTF8.GetBytes(dataToPUT);
				requestWriter.Write(data,0,data.Length);
				WebResponse resp = await request.GetResponseAsync();
				using (var responseStream = resp.GetResponseStream()){
					using (var sr = new StreamReader(responseStream)){
						received = await sr.ReadToEndAsync();
					}
				}
			}catch {

			}
			responseObj = (ReceiveContext)JsonConvert.DeserializeObject(received , typeof(ReceiveContext));
			return responseObj;
		}

		public async Task<ReceiveContext> DeleteItem(string url)
		{
			Uri uri = new Uri(url);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			request.Headers ["Token"] = Consts.AuthenticationToken;
			request.Method = "DELETE";
			string received;

			using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null))){
				using (var responseStream = response.GetResponseStream()){
					using (var sr = new StreamReader(responseStream)){
						received = await sr.ReadToEndAsync();
					}
				}
			}

			ReceiveContext responseObj = (ReceiveContext)JsonConvert.DeserializeObject(received , typeof(ReceiveContext));
			return responseObj;
		}

//		public async Task<ReceiveContext> UpdateProcedureParticipant(string url,ProcedureParticipantDetails participant)
//		{
//			url =  Consts.BaseServerURL+ "/anesservices/ProcParticipants/"+participant.ProcID.ToString();
//			string putData = JsonConvert.SerializeObject(participant,Formatting.Indented);			
//			ReceiveContext responseObj = await UpdateDetails (url,putData);
//
//			return responseObj;
//		}

		public async Task<ReceiveContext> DeleteProcedureDiagnostic(int ProcCodeID)
		{
			Stream requestWriter;
			string received=string.Empty;
			ReceiveContext responseObj;
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Consts.BaseServerURL + "/anesservices/ProcCodes/"+ProcCodeID);


				request.ContentType = "application/json; charset=utf-8";

				// Set the Method property to 'POST' to post data to the URI.
				request.Method = "DELETE";
				request.Headers ["Token"] =Consts.AuthenticationToken;

				requestWriter = await request.GetRequestStreamAsync();
				WebResponse resp = await request.GetResponseAsync();
				using (var responseStream = resp.GetResponseStream())
				{
					using (var sr = new StreamReader(responseStream))
					{
						received = await sr.ReadToEndAsync();
					}
				}
			}
			catch {

			}
			responseObj = (ReceiveContext)JsonConvert.DeserializeObject(received , typeof(ReceiveContext));
			return responseObj;
		}
	}

	public class ReceiveContext
	{
		//{"status":"ok","message":null,"result":null}
		public string status {
			get;
			set;
		}
		public string message {
			get;
			set;
		}
		public object result {
			get;
			set;
		}
	}

	public class RcvdJSONData
	{
		//{"status":"ok","message":null,"result":null}
		public string status {
			get;
			set;
		}
		public string message {
			get;
			set;
		}
		public object results {
			get;
			set;
		}
	}
}


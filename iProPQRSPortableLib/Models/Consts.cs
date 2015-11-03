using System;
using System.Collections.Generic;

namespace iProPQRSPortableLib
{
	public class Consts
	{
		public Consts ()
		{
		}
		public static string BaseAuthenticationURL = "http://login.iprocedures.com/gateway.aspx";
		public static string LoginUserName = string.Empty;
		public static string UserFirstName = string.Empty;
		public static string UserLastName = string.Empty;
		public static string BaseServerURL = string.Empty;
		public static string WebServiceVersion = "V10.1";
		public static string AuthenticationToken = string.Empty;
		public static string AuthenticationUserID = string.Empty;
		public static string DataRetrieveDate = DateTime.Now.ToString("yyyyMMdd");
		public static string SelectedFacilityID = string.Empty;
		public static string mId=string.Empty;
		public static string Role=string.Empty;
		public static string LoginUserFullName = string.Empty;
		public static Personnel Personnel;
		public static FacilityInfo Facilities;
		public static PatientProcedureInfo PatientInfo;
		public static ProcAttribTypes ProcAttribTypes;
		public static AttribTypesOfProcedure SelectedProcAttribtslist;
		public static List<UserDetails> lstOfUsers;
		public static List<int> RequiredAnesthesiaTechIDs = new List<int> (new int[] { 403,411,413,406,404,405,412,402});
		public static List<int> RequiredLineTypeIDs = new List<int> (new int[] { 626,627,628});
//		public static List<int> RequiredLineCVCSterileTechIDs = new List<int> (new int[] { 427,428,429});
		public static List<int> RequiredNerveBlockIDs = new List<int> (new int[] { 629,630,631,632,633,634,635,636,637,638,639,640,642,643,644,645,646,647,654});
		public static List<int> RequiredSpecialTechIDs = new List<int> (new int[] { 512,513,650,651,652,653});
		public static int procAttribTypeIDOfCVCSterileTech = 602;
		public static RcvdJSONData RcvdJSONDataResult; 
		#region Quality Metrics
		public static List<int> as10Options = new List<int> (new int[] { 658,659,660,661,662,663,664,665,666,667,668,669,670,671,672,673,674,675,676,677,678,679,680,681,682});
		#endregion

		//
		// update Facility
		public static string Preop;
		public static string ConsentForm;
		public static string Facesheet;
		public static string CustomValidation;
		public static string TemplateWithValues;
		public static string Postop;
		public static string ProvidersOption;
		public static string PreopCamera;
		public static string OB;

		public static string GetRole(string iD)
		{
			string retval=string.Empty;
			foreach (Usr usrItem in Consts.Personnel.results.Usrs) {
				if (usrItem.ID.ToString () == iD) {
					return retval = usrItem.Role;
				}
			}
			return retval;
		}
		public static string Getpersonnel(string iD, string type)
		{
			string retval = string.Empty;

			if (type.Trim ().ToLower () == "any") {
				foreach (Usr usrItem in Consts.Personnel.results.Usrs) {
					if (usrItem.ID.ToString () == iD && (usrItem.Role.Trim ().ToLower () == "anesthesiologist"
					    || usrItem.Role.Trim ().ToLower () == "crna"
					    || usrItem.Role.Trim ().ToLower () == "srna")) {
						retval = usrItem.FirstName + " " + usrItem.LastName;
					}
				}
			} else {
				if (type == "Surgeon") {
					foreach (Surgeon surgeonItem in Consts.Personnel.results.Surgeons) {
						if (surgeonItem.ID.ToString () == iD ) {
							retval = surgeonItem.Name;
						}
					}
				} else {
					foreach (Usr usrItem in Consts.Personnel.results.Usrs) {
						if (usrItem.ID.ToString () == iD && (usrItem.Role.Trim ().ToLower () == type.Trim ().ToLower ())) {
							retval = usrItem.FirstName + " " + usrItem.LastName;
						}
					}
				}
			}

			return retval;
		}

		public static string GetSurgeon(Dictionary<string, int?> lstsurgeon)
		{
			string surgeonName = string.Empty;

			if (lstsurgeon ["Surgeon3"] != null) {
				surgeonName = iProPQRSPortableLib.Consts.Getpersonnel (lstsurgeon ["Surgeon3"].ToString (), "Surgeon");
			} else if (lstsurgeon ["Surgeon2"] != null) {
				surgeonName = iProPQRSPortableLib.Consts.Getpersonnel (lstsurgeon ["Surgeon2"].ToString (), "Surgeon");
			} else if (lstsurgeon ["Surgeon"] != null) {
				surgeonName = iProPQRSPortableLib.Consts.Getpersonnel (lstsurgeon ["Surgeon"].ToString (), "Surgeon");
			}

			return surgeonName;
		}

		public static string GetCRNAPersonnel(Dictionary<string, int?> lstCRNApersonnel)
		{
			string crnaPersonnel = string.Empty;

			if (lstCRNApersonnel ["CRNA4"] != null) {
				crnaPersonnel = iProPQRSPortableLib.Consts.Getpersonnel (lstCRNApersonnel ["CRNA4"].ToString (), "CRNA");
			} if (lstCRNApersonnel ["CRNA3"] != null) {
				crnaPersonnel = iProPQRSPortableLib.Consts.Getpersonnel (lstCRNApersonnel ["CRNA3"].ToString (), "CRNA");
			} else if (lstCRNApersonnel ["CRNA2"] != null) {
				crnaPersonnel = iProPQRSPortableLib.Consts.Getpersonnel (lstCRNApersonnel ["CRNA2"].ToString (), "CRNA");
			} else if (lstCRNApersonnel ["CRNA1"] != null) {
				crnaPersonnel = iProPQRSPortableLib.Consts.Getpersonnel (lstCRNApersonnel ["CRNA1"].ToString (), "CRNA");
			} 

			return crnaPersonnel;
		}

		public static string GetAnesthesiologist(Dictionary<string, int?> lstAnesthesiologists)
		{
			string anesthesiologists = string.Empty;

			if (lstAnesthesiologists ["Anesthesiologist4"] != null) {
				anesthesiologists = iProPQRSPortableLib.Consts.Getpersonnel (lstAnesthesiologists ["Anesthesiologist4"].ToString (), "Anesthesiologist");
			} if (lstAnesthesiologists ["Anesthesiologist3"] != null) {
				anesthesiologists = iProPQRSPortableLib.Consts.Getpersonnel (lstAnesthesiologists ["Anesthesiologist3"].ToString (), "Anesthesiologist");
			} else if (lstAnesthesiologists ["Anesthesiologist2"] != null) {
				anesthesiologists = iProPQRSPortableLib.Consts.Getpersonnel (lstAnesthesiologists ["Anesthesiologist2"].ToString (), "Anesthesiologist");
			} else if (lstAnesthesiologists ["Anesthesiologist1"] != null) {
				anesthesiologists = iProPQRSPortableLib.Consts.Getpersonnel (lstAnesthesiologists ["Anesthesiologist1"].ToString (), "Anesthesiologist");
			} 

			return anesthesiologists;
		}


	}
}


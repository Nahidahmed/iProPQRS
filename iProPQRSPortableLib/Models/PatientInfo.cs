using System;
using System.Collections.Generic;

namespace iProPQRSPortableLib
{
	public class PatientProcedureInfo
	{
		public string status { get; set; }
		public string message { get; set; }
		public List<PatientProcedureDetails> result { get; set; }
	}	

	public class PatientProcedureDetails
	{
		public int ProcID { get; set; }
		public string OperationDate { get; set; }
		public int PatientID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PatientName { get; set; }
		public string ddlASAType { get; set; }
		public string ddlEncounterType { get; set; }
		public string Mrn { get; set; }
		public string DOB { get; set; }
		public string Anestheologist { get; set; }
		public string CRNA { get; set; }
		public string SRNA { get; set; }
		public string surgeon { get; set; }
		public int StatusID { get; set; }
		public string Status { get; set; }
		public bool IsMyCase { get; set; }
	}

	public class PatientProcedureFullDetails
	{
		public int ID {
			get;
			set;
		}
		public string Mrn {
			get;
			set;
		}
		public string OperationDate {
			get;
			set;
		}
		public string PatientID {
			get;
			set;
		}
		public string Location {
			get;
			set;
		}
		public string ORNumber {
			get;
			set;
		}
		public string Anesthesiologist {
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
		public string Crna {
			get;
			set;
		}
		public string Crna1 {
			get;
			set;
		}
		public string Crna2 {
			get;
			set;
		}
		public string Crna3 {
			get;
			set;
		}
		public string RlfTime1 {
			get;
			set;
		}
		public string RlfTime2 {
			get;
			set;
		}
		public string CrnaRlfTime1 {
			get;
			set;
		}
		public string CrnaRlfTime2 {
			get;
			set;
		}
		public string Surgeon {
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
		public string Procedures {
			get;
			set;
		}
		public string Diagnosis {
			get;
			set;
		}
		public string ScheduleStartTime {
			get;
			set;
		}
		public string StatusID {
			get;
			set;
		}
		public string RoomIn {
			get;
			set;
		}
		public string AnesStart {
			get;
			set;
		}
		public string SurgeryStart {
			get;
			set;
		}
		public string IncisionTime {
			get;
			set;
		}
		public string SurgeryEnd {
			get;
			set;
		}
		public string AnesEnd {
			get;
			set;
		}
		public string LastUpdatedTime {
			get;
			set;
		}
		public string tbPreIndBP {
			get;
			set;
		}
		public string tbPreIndP {
			get;
			set;
		}
		public string tbPreIndSPO2 {
			get;
			set;
		}
		public string tbPreIndResp {
			get;
			set;
		}
		public string Procedure1 {
			get;
			set;
		}
		public string Procedure2 {
			get;
			set;
		}
		public string Procedure3 {
			get;
			set;
		}
		public string Procedure4 {
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
		public string ProcedureUnit1 {
			get;
			set;
		}
		public string ProcedureUnit2 {
			get;
			set;
		}
		public string ProcedureUnit3 {
			get;
			set;
		}
		public string ProcedureUnit4 {
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
		public string DiagnosisUnit1 {
			get;
			set;
		}
		public string DiagnosisUnit2 {
			get;
			set;
		}
		public string DiagnosisUnit3 {
			get;
			set;
		}
		public string DiagnosisUnit4 {
			get;
			set;
		}
		public string Modifier1 {
			get;
			set;
		}
		public string Modifier2 {
			get;
			set;
		}
		public string Modifier3 {
			get;
			set;
		}
		public string Modifier4 {
			get;
			set;
		}
		public string Duration {
			get;
			set;
		}
		public string SurveyComplete {
			get;
			set;
		}
		public string BillingInfo {
			get;
			set;
		}
		public string PDFStatus {
			get;
			set;
		}
		public string PatientClass {
			get;
			set;
		}
		public string CancellationReason {
			get;
			set;
		}
		public string CancellationReasonID {
			get;
			set;
		}
		public string OrderNumber {
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
		public string Anesthesiologist4 {
			get;
			set;
		}
		public string RlfTime3 {
			get;
			set;
		}
		public string Crna4 {
			get;
			set;
		}
		public string CrnaRlfTime3 {
			get;
			set;
		}
		public string Srna1 {
			get;
			set;
		}
		public string Srna2 {
			get;
			set;
		}
		public string SrnaRlfTime1 {
			get;
			set;
		}
		public string Source {
			get;
			set;
		}
		public string Induction {
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
		public string AnesStrtTime2 {
			get;
			set;
		}
		public string AnesEndTime2 {
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
		public string AnesStrtTime4 {
			get;
			set;
		}
		public string AnesEndTime4 {
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
		public string CrnaStrtTime2 {
			get;
			set;
		}
		public string CrnaEndTime2 {
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
		public string CrnaStrtTime4 {
			get;
			set;
		}
		public string CrnaEndTime4 {
			get;
			set;
		}
		public string SrnaStrtTime1 {
			get;
			set;
		}
		public string SrnaEndTime1 {
			get;
			set;
		}
		public string ListType {
			get;
			set;
		}

	}
}


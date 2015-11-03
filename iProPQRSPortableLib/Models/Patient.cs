using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProPQRSPortableLib
{
	public class RootObject
	{
		public string status { get; set; }
		public object message { get; set; }
		public Patients Patients { get; set; }
		public MRUList MRUList { get; set; }
	}

	public class Patient
	{
		public int ID { get; set; }
		public string MRN { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Sex { get; set; }
		public string Height { get; set; }
		public string Weight { get; set; }
		public string Allergies { get; set; }
		public string PersonProvidingInfo { get; set; }
		public string DOB { get; set; }
		public string AccountNo { get; set; }
		public string HNENumber { get; set; }
		public string ddlEncounterType { get; set; }
		public string ddlASAType { get; set; }
		public string cbEmergency { get; set; }
		public string Race { get; set; }
		public string DeathDate { get; set; }
		public string DeathIndicator { get; set; }
		public string CreatedOn { get; set; }
		public string LastModifiedDate { get; set; }
		public string LastModifiedBy { get; set; }
		public string RecordType { get; set; }
		public string FacilityID { get; set; }
		public string SSN { get; set; }
		public string PrimaryLanguage { get; set; }
		public string Ethnicity { get; set; }
		public string HomePhone { get; set; }
		public string CellPhone { get; set; }
		public string MaritalStatus { get; set; }
		public string QuickAdd { get; set; }
		public string AptID { get; set; }
		public string isDFTExported { get; set; }
		public string ddlHghtType { get; set; }
		public string ddlWghtType { get; set; }
		public string tbBMI { get; set; }
		public string cbNKDA { get; set; }
		public string cbIVDye { get; set; }
		public string OperationDate {
			get;
			set;
		}
		public List<DataResults> DiagnosticList { get; set; }
		public List<DataResults> ProcedureList { get; set; }
		public List<DataResults> MACCodesList { get; set; }

	}

	public class PatientProfile
	{
		public string Allergies { get; set; }
		public string cbNKDA { get; set; }
		public string cbIVDye { get; set; }
		public int? Anesthesiologist1 { get; set; }
		public int? Anesthesiologist2 { get; set; }
		public int? Anesthesiologist3 { get; set; }
		public int? Anesthesiologist4 { get; set; }
		public int? CRNA1 { get; set; }
		public int? CRNA2 { get; set; }
		public int? CRNA3 { get; set; }
		public int? CRNA4 { get; set; }
		public int? SRNA1 { get; set; }
		public int? SRNA2 { get; set; }
		public string DOB { get; set; }
		public string PatientName { get; set; }
		public string FirstName { get; set; }
		public string Height { get; set; }
		public string LastName { get; set; }
		public int Location { get; set; }
		public string MRN { get; set; }
		public int? ORNumber { get; set; }
		public string OperationDate { get; set; }
		public int PatientID { get; set; }
		public int ProcID { get; set; }
		public string Procedures1 { get; set; }
		public string Procedures2 { get; set; }
		public string Procedures3 { get; set; }
		public string Procedures4 { get; set; }
		public string ProcedureCode1 { get; set; }
		public string ProcedureCode2 { get; set; }
		public string ProcedureCode3 { get; set; }
		public string ProcedureCode4 { get; set; }
		public string Diagnosis1 { get; set; }
		public string Diagnosis2 { get; set; }
		public string Diagnosis3 { get; set; }
		public string Diagnosis4 { get; set; }
		public string DiagnosisCode1 { get; set; }
		public string DiagnosisCode2 { get; set; }
		public string DiagnosisCode3 { get; set; }
		public string DiagnosisCode4 { get; set; }
		public string ScheduleStartTime { get; set; }
		public string Sex { get; set; }
		public string Status { get; set; }
		public int StatusID { get; set; }
		public int? Surgeon { get; set; }
		public string SurgeonName { get; set; }
		public int? Surgeon2 { get; set; }
		public int? Surgeon3 { get; set; }
		public string Weight { get; set; }
		public string ddlHghtType { get; set; }
		public string ddlWghtType { get; set; }
		public string AccountNo { get; set; }
		public string ddlEncounterType { get; set; }
		public string ddlASAType { get; set; }
		public string cbEmergency { get; set; }
		public string QuickAdd { get; set; }
		public string RWUser { get; set; }
		public string RWStatus { get; set; }
		public string previousrecord { get; set; }
		public string AnesStrtTime1 { get; set; }
		public string AnesStrtTime2 { get; set; }
		public string AnesStrtTime3 { get; set; }
		public string AnesStrtTime4 { get; set; }
		public string AnesEndTime1 { get; set; }
		public string AnesEndTime2 { get; set; }
		public string AnesEndTime3 { get; set; }
		public string AnesEndTime4 { get; set; }
		public string CrnaStrtTime1 { get; set; }
		public string CrnaStrtTime2 { get; set; }
		public string CrnaStrtTime3 { get; set; }
		public string CrnaStrtTime4 { get; set; }
		public string CrnaEndTime1 { get; set; }
		public string CrnaEndTime2 { get; set; }
		public string CrnaEndTime3 { get; set; }
		public string CrnaEndTime4 { get; set; }
		public string SrnaStrtTime1 { get; set; }
		public string SrnaEndTime1 { get; set; }
		public string IntraOpComplete { get; set; }
		public string PreOpComplete { get; set; }
		public string PostOpComplete { get; set; }
		public object Anes1ProviderCode { get; set; }
		public object Anes2ProviderCode { get; set; }
		public object Anes3ProviderCode { get; set; }
		public object Anes4ProviderCode { get; set; }
		public object Crna1ProviderCode { get; set; }
		public object Crna2ProviderCode { get; set; }
		public object Crna3ProviderCode { get; set; }
		public object Crna4ProviderCode { get; set; }
		public string IsFinalizedBefore { get; set; }
		public int? UnlockedUserID { get; set; }
	}
    public class Patients
    {
		public List<IncompleteCases> IncompleteCases { get; set; }
		public List<ScheduledCases> ScheduledCases { get; set; }
		public List<InProcessCases> InProcessCases { get; set; }
		public List<CompletedCases> CompletedCases { get; set; }
		public List<CancelledCases> CancelledCases { get; set; }
    }

	public class IncompleteCases : PatientProfile
    {
    }

	public class InProcessCases : PatientProfile
	{
	}

	public class CancelledCases : PatientProfile
	{
	}

	public class ScheduledCases : PatientProfile
    {
    }

	public class CompletedCases : PatientProfile
    {
    }

    public class MRUList
    {
        public List<object> TopNProcedures { get; set; }
        public List<object> TopNDiagCode { get; set; }
        public List<object> TopNPreMed { get; set; }
    }

}

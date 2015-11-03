using System;
using System.Collections.Generic;

namespace iProPQRSPortableLib
{
	public class ProcedureDiagnosticMaster
	{
		public ProcedureDiagnosticMaster()
		{
		}

		public string status { get; set; }
	    public string message { get; set; }
		public List<DataResults> results { get; set; }
		public List<DataResults> result { get; set; }
	}

	public class DataResults
	{
		public string Name { get; set; }
		public string Code { get; set; }
		public int ProcCodeID {get;set;}
		public string ProcID {get;set;}
		public int ProcCodeTypeID {get;set;}
	}

	public class LastUsedProcedureDiagnosis
	{
		public string status { get; set; }
		public string message { get; set; }
		public List<LastUsedProcedureDiagnosisDetails> result { get; set; }
	}
	public class LastUsedProcedureDiagnosisDetails
	{
		public string Name { get; set; }
		public string Code { get; set; }
	}

}


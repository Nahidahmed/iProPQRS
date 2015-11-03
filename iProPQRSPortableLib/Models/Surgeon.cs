using System;
using System.Collections.Generic;

namespace iProPQRSPortableLib
{
	public class Surgeons
	{
		public string status { get; set; }
		public string message { get; set; }
		public List<SurgeonDetails> result { get; set; }	
	}

	public class SurgeonDetails
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string NPI { get; set; }
		public string Speciality { get; set; }
		public string Designation { get; set; }
	}

	public class ProcedureSurgeonDetails
	{
		public int ProcSurgeonID { get; set; }
		public int ProcID { get; set; }
		public int SurgeonID { get; set; }
		public string Name { get; set; }
	}
}


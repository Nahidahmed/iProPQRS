using System;
using System.Collections.Generic;

namespace iProPQRSPortableLib
{
	public class ProcedureParticipants
	{
		public string status { get; set; }
		public string message { get; set; }
		public List<ProcedureParticipantDetails> result { get; set; }	
	}

	public class ProcedureParticipantDetails
	{
		public int ProcParticipantID { get; set; }
		public int ProcID { get; set; }
		public int RoleID { get; set; }
		public int UserID { get; set; }
		public string StartTime { get; set; }
		public string EndTime { get; set; }
		public string Name { get; set; }
	}
}


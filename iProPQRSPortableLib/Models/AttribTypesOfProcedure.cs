using System;
using System.Collections.Generic;

namespace iProPQRSPortableLib
{
	public class AttribTypesOfProcedure
	{
		public string status { get; set; }
		public string message { get; set; }
		public List<AttribType> result { get; set; }	
	}

	public class AttribType
	{
		public int ProcID { get; set; }
		public int ProcAttribTypeID { get; set; }
		public string Value { get; set; }
		public string Comment { get; set; }
		public bool IsHighLighted { get; set; }
	}
}


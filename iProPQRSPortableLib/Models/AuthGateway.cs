using System;
using System.Collections.Generic;

namespace iProPQRSPortableLib
{
	public class AuthGateway
	{
		public string status { get; set; }
		public string url { get; set; }
		public string message { get; set; }
		public List<Result> result { get; set; }
	}

	public class Result
	{
		public string FirstName{ get; set;}
		public string LastName{ get; set;}
		public string Role { get; set;}
		public string Token { get; set;}
		public int DefaultFacilityID { get; set;}
	}

	public class AuthCode
	{
		public string Name {
			get;
			set;
		}
		public string ServiceUrl {
			get;
			set;
		}
	}
}


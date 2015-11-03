using System;
using System.Collections.Generic;

namespace iProPQRSPortableLib
{
	public class User
	{
		public string status { get; set; }
		public string message { get; set; }
		public List<UserDetails> result { get; set; }	
	}

	public class UserDetails
	{
		public int ID { get; set; }
		public string UserID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Role { get; set; }
		public int? DefaultFacilityID { get; set; }
		public bool ActiveDirectoryAuthenticate { get; set; }
		public bool IsActive { get; set; }
	}
}


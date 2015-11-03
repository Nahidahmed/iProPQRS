using System;

namespace iProPQRSPortableLib.BL
{
	public class UserMaster
	{
		public int ID { get; set; }

		public string UserID { get; set; }

		public string AuthToken { get; set; }

		public DateTime? TokenTimeOut { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		public string Role { get; set; }

		public int? DefaultFacilityID { get; set; }

		public string PasswordSalt { get; set; }

		public string PasswordHash { get; set; }


		public string Status { get; set; }
	}

	public class UserMasterRequest : UserMaster
	{

		public int? PageNo { get; set; }

		public int? PageSize { get; set; }

		public string SortColumn { get; set; }

		public string SortOrder { get; set; }

		public int? TotalCount { get; set; }
	}
}



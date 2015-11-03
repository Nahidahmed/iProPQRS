using System;

namespace iProPQRSPortableLib
{
	public class ResponseInfo
	{
		public int ErrorCode{get;set;}
		public string ErrorMessage { get; set; }
		public string UserID { get; set; }
		public string PasswordHash { get; set; }
		public string Role { get; set; }
		public bool Status{ get; set; }
	}
}

//iProPQRS[3658:100180] 
//{"ErrorCode":0,"ErrorMessage":null,"UserID":"test","PasswordHash":"e6ca1453ee4b40742d8f5b40a1b49f1785a97d000f30e2c851560b29af178945","Role":"GroupAdmin","Status":true}
//{"ErrorCode":0,"ErrorMessage":null,"UserID":"test","PasswordHash":"e6ca1453ee4b40742d8f5b40a1b49f1785a97d000f30e2c851560b29af178945","Role":"GroupAdmin","Status":true}


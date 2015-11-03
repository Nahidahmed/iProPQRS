using System;
using System.Collections.Generic;

namespace iProPQRSPortableLib
{
	public class MACCodes
	{
		public string status { get; set; }
		public string message { get; set; }
		public List<MACCodesDetails> result { get; set; }
	}
	public class MACCodesDetails
	{
		public string Name { get; set; }
		public string Code { get; set; }
	}
}


using System;
using System.Collections.Generic;

namespace iProPQRSPortableLib
{
	public class FacilityInfo
	{
		public string status { get; set; }
		public object message { get; set; }
		public List<FacilityDetails> result { get; set; }
	}

	public class FacilityDetails
	{
		public int FMID { get; set; }
		public string FacilityName { get; set; }
		public string Default { get; set; }
		public string Timezone { get; set; }
		public string ConsentForm { get; set; }
		public string Preop { get; set; }
		public string ConsentFormFlag { get; set; }
		public string Facesheet { get; set; }
		public string TemplateWithValues { get; set; }
		public string ProvidersOption { get; set; }
		public string Postop { get; set; }
		public string PreopCamera { get; set; }
		public string CustomValidation { get; set; }
		public string OB { get; set; }
		public string Type { get; set; }
		public List<OperatingRooms> OperatingRooms { get; set; }
	}

	public class OperatingRooms
	{
		public int OperatingRoomId { get; set; }
		public string Name { get; set; }
		public int FMID { get; set; }
		public string Type { get; set; }
	}
}


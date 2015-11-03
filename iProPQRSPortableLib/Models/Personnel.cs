using System;
using System.Collections.Generic;

namespace iProPQRSPortableLib
{
	public class Personnel
	{
		public string status { get; set; }
		public Results results { get; set; }
	}

	public class Usr
	{
		public int ID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Role { get; set; }
		public string UserName { get; set; }
	}

	public class Surgeon
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Speciality { get; set; }
	}

	public class Facility
	{
		public int FMID { get; set; }
		public string FacilityName { get; set; }
		public string Preop { get; set; }
		public string ConsentFormFlag { get; set; }
		public string TemplateWithValues { get; set; }
		public int? DefaultFacilityID { get; set; }
		public string FaceSheet { get; set; }
		public string Postop { get; set; }
		public string ProvidersOption { get; set; }
		public string PreopCamera { get; set; }
		public string CustomValidation { get; set; }
		public string OB { get; set; }
	}

	public class ORList
	{
		public int ORID { get; set; }
		public string ORNO { get; set; }
		public int FMID { get; set; }
		public string TYPE { get; set; }
	}

	public class Results
	{
		public List<Usr> Usrs { get; set; }
		public List<Surgeon> Surgeons { get; set; }
		public List<Facility> Facilities { get; set; }
		public List<ORList> ORList { get; set; }
	}
}


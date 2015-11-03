using System;
using System.Collections.Generic;

namespace iProPQRSPortableLib
{
	public class ProcAttribTypes
	{
		public string status { get; set; }
		public object message { get; set; }
		public result result { get; set; }
	}

	public class result
	{
		public List<Type> Types { get; set; }
		public List<Group> Groups { get; set; }
		public List<Category> Categories { get; set; }
		public List<Option> Options { get; set; }
	}


	public class Type
	{
		public int ProcAttribTypeID { get; set; }
		public string Name { get; set; }
		public string Label { get; set; }
		public int ProcAttribGroupID { get; set; }
		public bool IsActive { get; set; }
		public string Description { get; set; }
		public string UIControlType { get; set; }
		public bool IsRequired { get; set; }
		public int Priority { get; set; }

	}

	public class Group
	{
		public int ProcAttribGroupID { get; set; }
		public string Name { get; set; }
		public int ProcAttribCategoryID { get; set; }
	}

	public class Category
	{
		public int ProcAttribCategoryID { get; set; }
		public string Name { get; set; }
	}

	public class Option
	{
		public int ProcAttribTypeID { get; set;}
		public string Value { get; set;}
		public string Description {get;set;}
	}

}


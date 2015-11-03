using System;
//using SQLite;

namespace iProPQRSPortableLib
{
	public class ToDoItem
	{
		public ToDoItem ()
		{
		}

//		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public string Name { get; set; }
		public string Notes { get; set; }
		public bool Done { get; set; }
	}
}


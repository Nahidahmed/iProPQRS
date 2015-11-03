using System;
using System.Collections.Generic;
using iProPQRSPortableLib;

namespace iProPQRS
{
	public class ProcedureItemGroup
	{
		public string StatusName{ get; set;}
		protected List<PatientProcedureDetails> patientProcedureListItems = new List<PatientProcedureDetails> ();
		public List<PatientProcedureDetails> PatientProcedureListItems
		{
			get{ return patientProcedureListItems;}
			set{ patientProcedureListItems = value; }
		}
	}

	public class PatientItemGroup
	{
		public string Name { get; set;}
		protected List<object> listItems = new List<object>();
		public List<object>  ListItems
		{
			get{ return listItems;}
			set{ listItems = value; }
		}
	}
	public class PatientData
	{
		public List<PatientItemGroup>	Gettab1Patient(RootObject rootObj)
		{
			List<PatientItemGroup> tableItems = new List<PatientItemGroup> ();

			// declare vars
			PatientItemGroup tGroup = new PatientItemGroup();
			tGroup.Name = "Incomplete Cases";
			if (rootObj.Patients.IncompleteCases != null) {
				foreach (IncompleteCases inCompleteCase in rootObj.Patients.IncompleteCases.FindAll(u=>u.FirstName!=null || u.LastName!=null)) {
					tGroup.ListItems.Add (inCompleteCase);
				}
			}
			tableItems.Add (tGroup);


			PatientItemGroup tGroup2 = new PatientItemGroup();
			tGroup2.Name = "Scheduled Cases";
			if (rootObj.Patients.ScheduledCases != null) {
				foreach (ScheduledCases scheduledCase in rootObj.Patients.ScheduledCases.FindAll(u=>u.FirstName!=null || u.LastName!=null)) {
					tGroup2.ListItems.Add (scheduledCase);
				}
			}
			tableItems.Add (tGroup2);

			PatientItemGroup tGroup3 = new PatientItemGroup();
			tGroup3.Name = "In Process Cases";
			if (rootObj.Patients.InProcessCases != null) {
				foreach (InProcessCases inProcessCase in rootObj.Patients.InProcessCases.FindAll(u=>u.FirstName!=null || u.LastName!=null)) {
					tGroup3.ListItems.Add (inProcessCase);
				}
			}
			tableItems.Add (tGroup3);

			PatientItemGroup tGroup4 = new PatientItemGroup();
			tGroup4.Name = "Completed Cases";
			if (rootObj.Patients.CompletedCases != null) {
				foreach (CompletedCases completeCase in rootObj.Patients.CompletedCases.FindAll(u=>u.FirstName!=null || u.LastName!=null)) {
					tGroup4.ListItems.Add (completeCase);
				}
			}
			tableItems.Add (tGroup4);

			PatientItemGroup tGroup5 = new PatientItemGroup();
			tGroup5.Name = "Cancelled Cases";
			if (rootObj.Patients.CancelledCases != null) {
				foreach (CancelledCases cancelCase in rootObj.Patients.CancelledCases.FindAll(u=>u.FirstName!=null || u.LastName!=null)) {
					tGroup5.ListItems.Add (cancelCase);
				}
			}
			tableItems.Add (tGroup5);

			return tableItems;
		}
		public List<PatientItemGroup>	Gettab2Surgeon(RootObject rootObj)
		{
			List<PatientItemGroup> tableItems = new List<PatientItemGroup> ();

			// declare vars
			PatientItemGroup tGroup = new PatientItemGroup();
			tGroup.Name = "Incomplete Cases";
			if (rootObj.Patients.IncompleteCases != null) {
				foreach (IncompleteCases inCompleteCase in rootObj.Patients.IncompleteCases.FindAll(u=>u.Surgeon!=null || u.Surgeon2!=null|| u.Surgeon3!=null)) {
					tGroup.ListItems.Add (inCompleteCase);
				}
			}
			tableItems.Add (tGroup);


			PatientItemGroup tGroup2 = new PatientItemGroup();
			tGroup2.Name = "Scheduled Cases";
			if (rootObj.Patients.ScheduledCases != null) {
				foreach (ScheduledCases scheduledCase in rootObj.Patients.ScheduledCases.FindAll(u=>u.Surgeon!=null || u.Surgeon2!=null|| u.Surgeon3!=null)) {
					tGroup2.ListItems.Add (scheduledCase);
				}
			}
			tableItems.Add (tGroup2);

			PatientItemGroup tGroup3 = new PatientItemGroup();
			tGroup3.Name = "In Process Cases";
			if (rootObj.Patients.InProcessCases != null) {
				foreach (InProcessCases inProcessCase in rootObj.Patients.InProcessCases.FindAll(u=>u.Surgeon!=null || u.Surgeon2!=null|| u.Surgeon3!=null)) {
					tGroup3.ListItems.Add (inProcessCase);
				}
			}
			tableItems.Add (tGroup3);

			PatientItemGroup tGroup4 = new PatientItemGroup();
			tGroup4.Name = "Completed Cases";
			if (rootObj.Patients.CompletedCases != null) {
				foreach (CompletedCases completeCase in rootObj.Patients.CompletedCases.FindAll(u=>u.Surgeon!=null || u.Surgeon2!=null|| u.Surgeon3!=null)) {
					tGroup4.ListItems.Add (completeCase);
				}
			}
			tableItems.Add (tGroup4);

			PatientItemGroup tGroup5 = new PatientItemGroup();
			tGroup5.Name = "Cancelled Cases";
			if (rootObj.Patients.CancelledCases != null) {
				foreach (CancelledCases cancelCase in rootObj.Patients.CancelledCases.FindAll(u=>u.Surgeon!=null || u.Surgeon2!=null|| u.Surgeon3!=null)) {
					tGroup5.ListItems.Add (cancelCase);
				}
			}
			tableItems.Add (tGroup5);

			return tableItems;
		}
		public List<PatientItemGroup>	Gettab3AnesName(RootObject rootObj)
		{
			List<PatientItemGroup> tableItems = new List<PatientItemGroup> ();

			// declare vars
			PatientItemGroup tGroup = new PatientItemGroup();
			tGroup.Name = "Incomplete Cases";
			if (rootObj.Patients.IncompleteCases != null) {
				foreach (IncompleteCases inCompleteCase in rootObj.Patients.IncompleteCases.FindAll(u=>u.Anesthesiologist1!=null || u.Anesthesiologist2!=null|| u.Anesthesiologist3!=null|| u.Anesthesiologist4!=null)) {
					tGroup.ListItems.Add (inCompleteCase);
				}
			}
			tableItems.Add (tGroup);


			PatientItemGroup tGroup2 = new PatientItemGroup();
			tGroup2.Name = "Scheduled Cases";
			if (rootObj.Patients.ScheduledCases != null) {
				foreach (ScheduledCases scheduledCase in rootObj.Patients.ScheduledCases.FindAll(u=>u.Anesthesiologist1!=null || u.Anesthesiologist2!=null|| u.Anesthesiologist3!=null|| u.Anesthesiologist4!=null)) {
					tGroup2.ListItems.Add (scheduledCase);
				}
			}
			tableItems.Add (tGroup2);

			PatientItemGroup tGroup3 = new PatientItemGroup();
			tGroup3.Name = "In Process Cases";
			if (rootObj.Patients.InProcessCases != null) {
				foreach (InProcessCases inProcessCase in rootObj.Patients.InProcessCases.FindAll(u=>u.Anesthesiologist1!=null || u.Anesthesiologist2!=null|| u.Anesthesiologist3!=null|| u.Anesthesiologist4!=null)) {
					tGroup3.ListItems.Add (inProcessCase);
				}
			}
			tableItems.Add (tGroup3);

			PatientItemGroup tGroup4 = new PatientItemGroup();
			tGroup4.Name = "Completed Cases";
			if (rootObj.Patients.CompletedCases != null) {
				foreach (CompletedCases completeCase in rootObj.Patients.CompletedCases.FindAll(u=>u.Anesthesiologist1!=null || u.Anesthesiologist2!=null|| u.Anesthesiologist3!=null|| u.Anesthesiologist4!=null)) {
					tGroup4.ListItems.Add (completeCase);
				}
			}
			tableItems.Add (tGroup4);

			PatientItemGroup tGroup5 = new PatientItemGroup();
			tGroup5.Name = "Cancelled Cases";
			if (rootObj.Patients.CancelledCases != null) {
				foreach (CancelledCases cancelCase in rootObj.Patients.CancelledCases.FindAll(u=>u.Anesthesiologist1!=null || u.Anesthesiologist2!=null|| u.Anesthesiologist3!=null|| u.Anesthesiologist4!=null)) {
					tGroup5.ListItems.Add (cancelCase);
				}
			}
			tableItems.Add (tGroup5);

			return tableItems;
		}
		//
	}
}


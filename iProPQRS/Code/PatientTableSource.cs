
using System;

using Foundation;
using UIKit;
using System.Collections.Generic;
using iProPQRSPortableLib;
using System.Linq;
using System.Globalization;
using ObjCRuntime;

namespace iProPQRS
{
	public class PatientTableSource : UITableViewSource
	{
		public List<PatientItemGroup> tableItems;
		public List<ProcedureItemGroup> procTableItems;
		PatientListView patListController;
		public bool sectionOpenedFlag = false;
		public Dictionary<string, int> lstCollapsedSections;

		public PatientTableSource (List<PatientItemGroup> items)
		{
			this.tableItems = items;
		}

		public PatientTableSource (List<ProcedureItemGroup> items)
		{
			this.procTableItems = items;
		}

		public PatientTableSource (List<PatientItemGroup> items,PatientListView patListControllerObj)
		{
			this.tableItems = items;
			this.patListController = patListControllerObj;
			lstCollapsedSections = new Dictionary<string, int> ();
			lstCollapsedSections.Add ("Section 0", 0);
			lstCollapsedSections.Add ("Section 1", 0);
			lstCollapsedSections.Add ("Section 2", 0);
			lstCollapsedSections.Add ("Section 3", 0);
			lstCollapsedSections.Add ("Section 4", 0);

		}

		public PatientTableSource (List<ProcedureItemGroup> items,PatientListView patListControllerObj)
		{
			this.procTableItems = items;
			this.patListController = patListControllerObj;
			lstCollapsedSections = new Dictionary<string, int> ();
			lstCollapsedSections.Add ("Section 0", 0);
			lstCollapsedSections.Add ("Section 1", 0);
			lstCollapsedSections.Add ("Section 2", 0);
			lstCollapsedSections.Add ("Section 3", 0);
			lstCollapsedSections.Add ("Section 4", 0);
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			// TODO: return the actual number of sections
			return procTableItems.Count;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
//			// TODO: return the actual number of items in the section
			switch (section) {
			case 0:
				if (lstCollapsedSections ["Section 0"] == 0)
					return 0;
				else
					return procTableItems [(int)section].PatientProcedureListItems.Count;

				break;
			case 1:
				if (lstCollapsedSections ["Section 1"] == 0)
					return 0;
				else
					return procTableItems [(int)section].PatientProcedureListItems.Count;
			case 2:
				if (lstCollapsedSections ["Section 2"] == 0)
					return 0;
				else
					return procTableItems [(int)section].PatientProcedureListItems.Count;
			case 3:
				if (lstCollapsedSections ["Section 3"] == 0)
					return 0;
				else
					return procTableItems [(int)section].PatientProcedureListItems.Count;
			case 4:
				if (lstCollapsedSections ["Section 4"] == 0)
					return 0;
				else
					return procTableItems [(int)section].PatientProcedureListItems.Count;
			default:
				return 0;
//				return tableItems [(int)section].ListItems.Count;
			}
		}


		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 70f;
		}

		public override nfloat GetHeightForHeader (UITableView tableView, nint section)
		{
			return 41f;
		}
		public override UIView GetViewForHeader (UITableView tableView, nint section)
		{
			
			PatientSectionHeaderView patSectionHeader = new PatientSectionHeaderView (this,this.patListController,section);

			return patSectionHeader.View;
		}
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
//			PatientProfile selectedPatientProfile;
//			selectedPatientProfile = (PatientProfile)tableItems [indexPath.Section].ListItems [indexPath.Row];
//			patListController.ShowPatientEditor (selectedPatientProfile);
			PatientProcedureDetails selectedPatientProcedureDetails;
			selectedPatientProcedureDetails = (PatientProcedureDetails) procTableItems[indexPath.Section].PatientProcedureListItems [indexPath.Row];
			patListController.ShowPatientEditor (selectedPatientProcedureDetails);

		}
//		public override string TitleForHeader (UITableView tableView, nint section)
//		{
//			return "Header";
//		}
//
//		public override string TitleForFooter (UITableView tableView, nint section)
//		{
//			return "Footer";
//		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{

			var cell = tableView.DequeueReusableCell ("CustomWOTableView") as PatientTableViewCell;

			if (cell == null) {
				cell = PatientTableViewCell.Create ();
			}

			cell.UserInteractionEnabled = true;

			//Nahid Ahmed: May 22,2015 As per discussion with Gavi, the below buttons are hidden
			cell.PDFButton.Hidden = true;
			cell.PreOPButton.Hidden = true;
			cell.PostOPButton.Hidden = true;
			cell.IntraOPButton.Hidden = true;


			cell.PatientName.Text = procTableItems [indexPath.Section].PatientProcedureListItems [indexPath.Row].PatientName;
			cell.MRNumber.Text = procTableItems [indexPath.Section].PatientProcedureListItems [indexPath.Row].Mrn;
			DateTime DOB = Convert.ToDateTime(procTableItems [indexPath.Section].PatientProcedureListItems [indexPath.Row].DOB);
			cell.PatientDOB.Text = DOB.ToString ("MM/dd/yyyy");
			cell.SurgeonName.Text = procTableItems [indexPath.Section].PatientProcedureListItems [indexPath.Row].surgeon;
			cell.AnesthesiologistLbl.Text = procTableItems [indexPath.Section].PatientProcedureListItems [indexPath.Row].Anestheologist;
			cell.CRNALbl.Text  = procTableItems [indexPath.Section].PatientProcedureListItems [indexPath.Row].CRNA;
			DateTime schedOperationDate = Convert.ToDateTime(procTableItems [indexPath.Section].PatientProcedureListItems [indexPath.Row].OperationDate);
			cell.ScheduledDateTime.Text = schedOperationDate.ToString("MM/dd/yyyy") ;

			return cell;
		}

		public PatientTableSource FilterTable(UISearchBar searchBar,List<ProcedureItemGroup> sentTableItems,PatientListView patListControllerObj)
		{
			PatientTableSource tableSource;
			List<ProcedureItemGroup> filteredTableItems = new List<ProcedureItemGroup> ();

			ProcedureItemGroup tGroup;
			List<PatientProcedureDetails> inCompleteCases = new List<PatientProcedureDetails>();
			List<PatientProcedureDetails> scheduledCases = new List<PatientProcedureDetails>();
			List<PatientProcedureDetails> inProcessCases = new List<PatientProcedureDetails>();
			List<PatientProcedureDetails> completedCases = new List<PatientProcedureDetails>();
			List<PatientProcedureDetails> cancelledCases = new List<PatientProcedureDetails>();

			if (searchBar.SelectedScopeButtonIndex == 0) {
				inCompleteCases = sentTableItems [0].PatientProcedureListItems.FindAll (x => x.PatientName.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()));
				scheduledCases = sentTableItems [1].PatientProcedureListItems.FindAll (x => x.PatientName.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()));
				inProcessCases = sentTableItems [2].PatientProcedureListItems.FindAll (x => x.PatientName.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()));
				completedCases = sentTableItems [3].PatientProcedureListItems.FindAll (x => x.PatientName.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()));
				cancelledCases = sentTableItems [4].PatientProcedureListItems.FindAll (x => x.PatientName.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()));
			}

			if (searchBar.SelectedScopeButtonIndex == 1) {
				inCompleteCases = sentTableItems [0].PatientProcedureListItems.FindAll (x => x.surgeon != null && x.surgeon.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()));
				scheduledCases = sentTableItems [1].PatientProcedureListItems.FindAll (x => x.surgeon != null &&  x.surgeon.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()));
				inProcessCases = sentTableItems [2].PatientProcedureListItems.FindAll (x => x.surgeon != null &&  x.surgeon.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()));
				completedCases = sentTableItems [3].PatientProcedureListItems.FindAll (x => x.surgeon != null &&  x.surgeon.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()));
				cancelledCases = sentTableItems [4].PatientProcedureListItems.FindAll (x => x.surgeon != null &&  x.surgeon.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()));
			}

			if (searchBar.SelectedScopeButtonIndex == 2) {                                     
				inCompleteCases = sentTableItems [0].PatientProcedureListItems.FindAll (x => x.Anestheologist!=null && x.Anestheologist.Trim().ToLower().Contains(searchBar.Text.Trim().ToLower()));
				scheduledCases = sentTableItems [1].PatientProcedureListItems.FindAll (x => x.Anestheologist!=null &&  x.Anestheologist.Trim().ToLower().Contains(searchBar.Text.Trim().ToLower()));
				inProcessCases = sentTableItems [2].PatientProcedureListItems.FindAll (x => x.Anestheologist!=null &&  x.Anestheologist.Trim().ToLower().Contains(searchBar.Text.Trim().ToLower()));
				completedCases = sentTableItems [3].PatientProcedureListItems.FindAll (x => x.Anestheologist!=null &&  x.Anestheologist.Trim().ToLower().Contains(searchBar.Text.Trim().ToLower()));
				cancelledCases = sentTableItems [4].PatientProcedureListItems.FindAll (x => x.Anestheologist!=null &&  x.Anestheologist.Trim().ToLower().Contains(searchBar.Text.Trim().ToLower()));
			}

			if (searchBar.SelectedScopeButtonIndex == 3 && !string.IsNullOrEmpty(iProPQRSPortableLib.Consts.mId)) {
				//NAHID Ahmed, July 23, 2015
				//Need to discuss with Anand and get details related to Anesthesiologist1,2,3,4 and 
				//CRNA1,2,3,4 and SRNA1
			}

			if (searchBar.SelectedScopeButtonIndex == 3 ) {
//				inCompleteCases = sentTableItems [0].PatientProcedureListItems.FindAll (x => x.IsMyCase == true &&  x.Anestheologist!=null && x.Anestheologist.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()));
//				scheduledCases = sentTableItems [1].PatientProcedureListItems.FindAll (x => x.IsMyCase == true &&  x.Anestheologist!=null && x.Anestheologist.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()));
//				inProcessCases = sentTableItems [2].PatientProcedureListItems.FindAll (x => x.IsMyCase == true &&  x.Anestheologist!=null && x.Anestheologist.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()));
//				completedCases = sentTableItems [3].PatientProcedureListItems.FindAll (x => x.IsMyCase == true &&  x.Anestheologist!=null && x.Anestheologist.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()));
//				cancelledCases = sentTableItems [4].PatientProcedureListItems.FindAll (x => x.IsMyCase == true &&  x.Anestheologist!=null && x.Anestheologist.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()));
//				string anestheologist = string.Empty;
//				if (iProPQRSPortableLib.Consts.LoginUserFullName.Trim () != string.Empty) {
//					string[] anesArr = iProPQRSPortableLib.Consts.LoginUserFullName.Split (',');
//					if(anesArr.Length > 1)
//						anestheologist = anesArr [1].Trim().ToLower() + " " + anesArr [0].Trim().ToLower();
//				}
//				inCompleteCases = sentTableItems [0].PatientProcedureListItems.FindAll (x => x.IsMyCase == true &&  x.Anestheologist!=null && x.Anestheologist.Trim ().ToLower ().Contains (anestheologist));
//				scheduledCases = sentTableItems [1].PatientProcedureListItems.FindAll (x => x.IsMyCase == true &&  x.Anestheologist!=null && x.Anestheologist.Trim ().ToLower ().Contains (anestheologist));
//				inProcessCases = sentTableItems [2].PatientProcedureListItems.FindAll (x => x.IsMyCase == true &&  x.Anestheologist!=null && x.Anestheologist.Trim ().ToLower ().Contains (anestheologist));
//				completedCases = sentTableItems [3].PatientProcedureListItems.FindAll (x => x.IsMyCase == true &&  x.Anestheologist!=null && x.Anestheologist.Trim ().ToLower ().Contains (anestheologist));
//				cancelledCases = sentTableItems [4].PatientProcedureListItems.FindAll (x => x.IsMyCase == true &&  x.Anestheologist!=null && x.Anestheologist.Trim ().ToLower ().Contains (anestheologist));
				inCompleteCases = sentTableItems [0].PatientProcedureListItems.FindAll (x => x.IsMyCase == true &&  x.Anestheologist!=null);
				scheduledCases = sentTableItems [1].PatientProcedureListItems.FindAll (x => x.IsMyCase == true &&  x.Anestheologist!=null);
				inProcessCases = sentTableItems [2].PatientProcedureListItems.FindAll (x => x.IsMyCase == true &&  x.Anestheologist!=null);
				completedCases = sentTableItems [3].PatientProcedureListItems.FindAll (x => x.IsMyCase == true &&  x.Anestheologist!=null);
				cancelledCases = sentTableItems [4].PatientProcedureListItems.FindAll (x => x.IsMyCase == true &&  x.Anestheologist!=null);
			}

			tGroup = new ProcedureItemGroup () { 
				StatusName="Incomplete Cases"
			};
			foreach (PatientProcedureDetails filtereditem in inCompleteCases) {
				tGroup.PatientProcedureListItems.Add (filtereditem);
			}
			filteredTableItems.Add (tGroup);

			tGroup = new ProcedureItemGroup () { 
				StatusName="Scheduled Cases"
			};
			foreach (PatientProcedureDetails filtereditem in scheduledCases) {
				tGroup.PatientProcedureListItems.Add (filtereditem);
			}
			filteredTableItems.Add (tGroup);

			tGroup = new ProcedureItemGroup () { 
				StatusName="Inprocess Cases"
			};
			foreach (PatientProcedureDetails filtereditem in inProcessCases) {
				tGroup.PatientProcedureListItems.Add (filtereditem);
			}
			filteredTableItems.Add (tGroup);

			tGroup = new ProcedureItemGroup () { 
				StatusName="Completed Cases"
			};
			foreach (PatientProcedureDetails filtereditem in completedCases) {
				tGroup.PatientProcedureListItems.Add (filtereditem);
			}
			filteredTableItems.Add (tGroup);

			tGroup = new ProcedureItemGroup () { 
				StatusName="Cancelled Cases"
			};
			foreach (PatientProcedureDetails filtereditem in cancelledCases) {
				tGroup.PatientProcedureListItems.Add (filtereditem);
			}
			filteredTableItems.Add (tGroup);

			tableSource = new PatientTableSource (filteredTableItems, patListControllerObj);

			return tableSource;

		}

		public PatientTableSource Filter(UISearchBar searchBar,List<PatientItemGroup> tableItems,PatientListView patListControllerObj)
		{
			
			PatientTableSource tableSource;

			//if (searchBar.Text.Trim () != string.Empty) {
				List<PatientItemGroup> filteredTableItems = new List<PatientItemGroup> ();

				List<IncompleteCases> incompList = (tableItems [0].ListItems).Cast<IncompleteCases> ().ToList ();

				if(searchBar.SelectedScopeButtonIndex ==0)
					incompList = incompList.FindAll (x => x.PatientName.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()));

				if (searchBar.SelectedScopeButtonIndex == 1) {
					List<IncompleteCases> surgeonIncompList = new List<IncompleteCases> ();
					foreach (IncompleteCases inCompCase in incompList) {
						Dictionary<string, int?> lstSurgeon = new Dictionary<string, int?> ();
						lstSurgeon.Add ("Surgeon", inCompCase.Surgeon);
						lstSurgeon.Add ("Surgeon2", inCompCase.Surgeon2);
						lstSurgeon.Add ("Surgeon3", inCompCase.Surgeon3);

						string surgeonName = iProPQRSPortableLib.Consts.GetSurgeon (lstSurgeon);

						if(surgeonName.Trim().ToLower().Contains(searchBar.Text.Trim().ToLower()) || searchBar.Text.Trim() == string.Empty )
							surgeonIncompList.Add(inCompCase);
					}
					incompList = surgeonIncompList;
				}

				if (searchBar.SelectedScopeButtonIndex == 2) {
					List<IncompleteCases> anesthesioIncompList = new List<IncompleteCases> ();
					foreach (IncompleteCases inCompCase in incompList) {
						Dictionary<string, int?> lstAnesthesiologists = new Dictionary<string, int?> ();
						lstAnesthesiologists.Add ("Anesthesiologist1", inCompCase.Anesthesiologist1);
						lstAnesthesiologists.Add ("Anesthesiologist2", inCompCase.Anesthesiologist2);
						lstAnesthesiologists.Add ("Anesthesiologist3", inCompCase.Anesthesiologist3);
						lstAnesthesiologists.Add ("Anesthesiologist4", inCompCase.Anesthesiologist4);

						string anesthesiologist = iProPQRSPortableLib.Consts.GetAnesthesiologist (lstAnesthesiologists);

						if(anesthesiologist.Trim().ToLower().Contains(searchBar.Text.Trim().ToLower()) || searchBar.Text.Trim() == string.Empty)
							anesthesioIncompList.Add(inCompCase);
					}
					incompList = anesthesioIncompList;
				}

				if (searchBar.SelectedScopeButtonIndex == 3 && !string.IsNullOrEmpty(iProPQRSPortableLib.Consts.mId)) {
					List<IncompleteCases> anesthesioIncompList = new List<IncompleteCases> ();					
					foreach (IncompleteCases myCase in incompList.FindAll(u=>u.Anesthesiologist1.ToString()==iProPQRSPortableLib.Consts.mId
					||u.Anesthesiologist2.ToString()==iProPQRSPortableLib.Consts.mId
					||u.Anesthesiologist3.ToString()==iProPQRSPortableLib.Consts.mId
					||u.Anesthesiologist4.ToString()==iProPQRSPortableLib.Consts.mId
					||u.CRNA1.ToString()==iProPQRSPortableLib.Consts.mId
					||u.CRNA2.ToString()==iProPQRSPortableLib.Consts.mId
					||u.CRNA3.ToString()==iProPQRSPortableLib.Consts.mId
					||u.CRNA4.ToString()==iProPQRSPortableLib.Consts.mId
					||u.SRNA1.ToString()==iProPQRSPortableLib.Consts.mId)) {
						if (searchBar.Text.Trim() == string.Empty|| myCase.PatientName.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()))
							anesthesioIncompList.Add (myCase);
					}
					incompList = anesthesioIncompList;
				}

				PatientItemGroup tGroup = new PatientItemGroup () { 
					Name="Incomplete Cases"
				};
				foreach (IncompleteCases filtereditem in incompList) {
					tGroup.ListItems.Add (filtereditem);
				}
				filteredTableItems.Add (tGroup);
					

				List<ScheduledCases> schedList = (tableItems [1].ListItems).Cast<ScheduledCases> ().ToList ();
				if(searchBar.SelectedScopeButtonIndex ==0)
					schedList = schedList.FindAll (x => x.PatientName.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()));
			
				if (searchBar.SelectedScopeButtonIndex == 1) {
					List<ScheduledCases> surgeonSchedList = new List<ScheduledCases> ();
					foreach (ScheduledCases schedCase in schedList) {
						Dictionary<string, int?> lstSurgeon = new Dictionary<string, int?> ();
						lstSurgeon.Add ("Surgeon", schedCase.Surgeon);
						lstSurgeon.Add ("Surgeon2", schedCase.Surgeon2);
						lstSurgeon.Add ("Surgeon3", schedCase.Surgeon3);

						string surgeonName = iProPQRSPortableLib.Consts.GetSurgeon (lstSurgeon);

						if(surgeonName.Trim().ToLower().Contains(searchBar.Text.Trim().ToLower()) || searchBar.Text.Trim() == string.Empty)
							surgeonSchedList.Add(schedCase);
					}
					schedList = surgeonSchedList;
				}

				if (searchBar.SelectedScopeButtonIndex == 2) {
					List<ScheduledCases> anesthesioSchedList = new List<ScheduledCases> ();
					foreach (ScheduledCases schedCase in schedList) {
						Dictionary<string, int?> lstAnesthesiologists = new Dictionary<string, int?> ();
						lstAnesthesiologists.Add ("Anesthesiologist1", schedCase.Anesthesiologist1);
						lstAnesthesiologists.Add ("Anesthesiologist2", schedCase.Anesthesiologist2);
						lstAnesthesiologists.Add ("Anesthesiologist3", schedCase.Anesthesiologist3);
						lstAnesthesiologists.Add ("Anesthesiologist4", schedCase.Anesthesiologist4);

						string anesthesiologist = iProPQRSPortableLib.Consts.GetAnesthesiologist (lstAnesthesiologists);

						if(anesthesiologist.Trim().ToLower().Contains(searchBar.Text.Trim().ToLower()) || searchBar.Text.Trim() == string.Empty)
							anesthesioSchedList.Add(schedCase);
					}
					schedList = anesthesioSchedList;
				}
			if (searchBar.SelectedScopeButtonIndex == 3 && !string.IsNullOrEmpty(iProPQRSPortableLib.Consts.mId)) {

				List<ScheduledCases> ScheduledCasesList = new List<ScheduledCases> ();					
				foreach (ScheduledCases myCase in ScheduledCasesList.FindAll(u=>u.Anesthesiologist1.ToString()==iProPQRSPortableLib.Consts.mId||u.Anesthesiologist2.ToString()==iProPQRSPortableLib.Consts.mId||u.Anesthesiologist3.ToString()==iProPQRSPortableLib.Consts.mId||u.Anesthesiologist4.ToString()==iProPQRSPortableLib.Consts.mId||u.CRNA1.ToString()==iProPQRSPortableLib.Consts.mId||u.CRNA2.ToString()==iProPQRSPortableLib.Consts.mId||u.CRNA3.ToString()==iProPQRSPortableLib.Consts.mId||u.CRNA4.ToString()==iProPQRSPortableLib.Consts.mId||u.SRNA1.ToString()==iProPQRSPortableLib.Consts.mId)) {
					if (searchBar.Text.Trim() == string.Empty|| myCase.PatientName.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()))
						ScheduledCasesList.Add (myCase);
				}
				schedList = ScheduledCasesList;
			}

				PatientItemGroup tGroup2 = new PatientItemGroup () { 
					Name="Scheduled Cases"
				};
				foreach (ScheduledCases filtereditem in schedList) {
					tGroup2.ListItems.Add (filtereditem);
				}
				filteredTableItems.Add (tGroup2);

				//In Processs Cases

				List<InProcessCases> inProcesssList = (tableItems [2].ListItems).Cast<InProcessCases> ().ToList ();
				if(searchBar.SelectedScopeButtonIndex ==0)
					inProcesssList = inProcesssList.FindAll (x => x.PatientName.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()));

				if (searchBar.SelectedScopeButtonIndex == 1) {
					List<InProcessCases> surgeonInProcessList = new List<InProcessCases> ();
					foreach (InProcessCases inProcessCase in inProcesssList) {
						Dictionary<string, int?> lstSurgeon = new Dictionary<string, int?> ();
						lstSurgeon.Add ("Surgeon", inProcessCase.Surgeon);
						lstSurgeon.Add ("Surgeon2", inProcessCase.Surgeon2);
						lstSurgeon.Add ("Surgeon3", inProcessCase.Surgeon3);

						string surgeonName = iProPQRSPortableLib.Consts.GetSurgeon (lstSurgeon);

						if(surgeonName.Trim().ToLower().Contains(searchBar.Text.Trim().ToLower()) || searchBar.Text.Trim() == string.Empty)
							surgeonInProcessList.Add(inProcessCase);
					}
					inProcesssList = surgeonInProcessList;
				}

				if (searchBar.SelectedScopeButtonIndex == 2) {
					List<InProcessCases> anesthesioInProcessList = new List<InProcessCases> ();
					foreach (InProcessCases inProcessCase in inProcesssList) {
						Dictionary<string, int?> lstAnesthesiologists = new Dictionary<string, int?> ();
						lstAnesthesiologists.Add ("Anesthesiologist1", inProcessCase.Anesthesiologist1);
						lstAnesthesiologists.Add ("Anesthesiologist2", inProcessCase.Anesthesiologist2);
						lstAnesthesiologists.Add ("Anesthesiologist3", inProcessCase.Anesthesiologist3);
						lstAnesthesiologists.Add ("Anesthesiologist4", inProcessCase.Anesthesiologist4);

						string anesthesiologist = iProPQRSPortableLib.Consts.GetAnesthesiologist (lstAnesthesiologists);

						if(anesthesiologist.Trim().ToLower().Contains(searchBar.Text.Trim().ToLower()) || searchBar.Text.Trim() == string.Empty)
							anesthesioInProcessList.Add(inProcessCase);
					}
					inProcesssList = anesthesioInProcessList;
				}
			if (searchBar.SelectedScopeButtonIndex == 3 && !string.IsNullOrEmpty(iProPQRSPortableLib.Consts.mId)) {

				List<InProcessCases> InProcessCasesList = new List<InProcessCases> ();					
				foreach (InProcessCases myCase in InProcessCasesList.FindAll(u=>u.Anesthesiologist1.ToString()==iProPQRSPortableLib.Consts.mId||u.Anesthesiologist2.ToString()==iProPQRSPortableLib.Consts.mId||u.Anesthesiologist3.ToString()==iProPQRSPortableLib.Consts.mId||u.Anesthesiologist4.ToString()==iProPQRSPortableLib.Consts.mId||u.CRNA1.ToString()==iProPQRSPortableLib.Consts.mId||u.CRNA2.ToString()==iProPQRSPortableLib.Consts.mId||u.CRNA3.ToString()==iProPQRSPortableLib.Consts.mId||u.CRNA4.ToString()==iProPQRSPortableLib.Consts.mId||u.SRNA1.ToString()==iProPQRSPortableLib.Consts.mId)) {
						if (searchBar.Text.Trim() == string.Empty|| myCase.PatientName.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()))
						InProcessCasesList.Add (myCase);
					}
				inProcesssList = InProcessCasesList;
				}
				PatientItemGroup tGroup3 = new PatientItemGroup () { 
					Name="In Processs Cases"
				};
				foreach (InProcessCases filtereditem in inProcesssList) {
					tGroup3.ListItems.Add (filtereditem);
				}
				filteredTableItems.Add (tGroup3);

				// Completed Cases

				List<CompletedCases> CompletedList = (tableItems [3].ListItems).Cast<CompletedCases> ().ToList ();
				if(searchBar.SelectedScopeButtonIndex ==0)
					CompletedList = CompletedList.FindAll (x => x.PatientName.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()));

				if (searchBar.SelectedScopeButtonIndex == 1) {
					List<CompletedCases> surgeonCompleteList = new List<CompletedCases> ();
					foreach (CompletedCases completeCase in CompletedList) {
						Dictionary<string, int?> lstSurgeon = new Dictionary<string, int?> ();
						lstSurgeon.Add ("Surgeon", completeCase.Surgeon);
						lstSurgeon.Add ("Surgeon2", completeCase.Surgeon2);
						lstSurgeon.Add ("Surgeon3", completeCase.Surgeon3);

						string surgeonName = iProPQRSPortableLib.Consts.GetSurgeon (lstSurgeon);

						if(surgeonName.Trim().ToLower().Contains(searchBar.Text.Trim().ToLower()) || searchBar.Text.Trim() == string.Empty)
							surgeonCompleteList.Add(completeCase);
					}
					CompletedList = surgeonCompleteList;
				}

				if (searchBar.SelectedScopeButtonIndex == 2) {
					List<CompletedCases> anesthesioCompleteList = new List<CompletedCases> ();
					foreach (CompletedCases completeCase in CompletedList) {
						Dictionary<string, int?> lstAnesthesiologists = new Dictionary<string, int?> ();
						lstAnesthesiologists.Add ("Anesthesiologist1", completeCase.Anesthesiologist1);
						lstAnesthesiologists.Add ("Anesthesiologist2", completeCase.Anesthesiologist2);
						lstAnesthesiologists.Add ("Anesthesiologist3", completeCase.Anesthesiologist3);
						lstAnesthesiologists.Add ("Anesthesiologist4", completeCase.Anesthesiologist4);

						string anesthesiologist = iProPQRSPortableLib.Consts.GetAnesthesiologist (lstAnesthesiologists);

						if(anesthesiologist.Trim().ToLower().Contains(searchBar.Text.Trim().ToLower()) || searchBar.Text.Trim() == string.Empty)
							anesthesioCompleteList.Add(completeCase);
					}
					CompletedList = anesthesioCompleteList;
				}
			if (searchBar.SelectedScopeButtonIndex == 3 && !string.IsNullOrEmpty(iProPQRSPortableLib.Consts.mId)) {

				List<CompletedCases> CompletedCasesList = new List<CompletedCases> ();					
				foreach (CompletedCases myCase in CompletedCasesList.FindAll(u=>u.Anesthesiologist1.ToString()==iProPQRSPortableLib.Consts.mId||u.Anesthesiologist2.ToString()==iProPQRSPortableLib.Consts.mId||u.Anesthesiologist3.ToString()==iProPQRSPortableLib.Consts.mId||u.Anesthesiologist4.ToString()==iProPQRSPortableLib.Consts.mId||u.CRNA1.ToString()==iProPQRSPortableLib.Consts.mId||u.CRNA2.ToString()==iProPQRSPortableLib.Consts.mId||u.CRNA3.ToString()==iProPQRSPortableLib.Consts.mId||u.CRNA4.ToString()==iProPQRSPortableLib.Consts.mId||u.SRNA1.ToString()==iProPQRSPortableLib.Consts.mId)) {
						if (searchBar.Text.Trim() == string.Empty|| myCase.PatientName.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()))
						CompletedCasesList.Add (myCase);
					}
				CompletedList = CompletedCasesList;
				}
				PatientItemGroup tGroup4 = new PatientItemGroup () { 
					Name="Completed Cases"
				};
				foreach (CompletedCases filtereditem in CompletedList) {
					tGroup4.ListItems.Add (filtereditem);
				}
				filteredTableItems.Add (tGroup4);

				//Cancelled cases

				List<CancelledCases> CancelledList = (tableItems [4].ListItems).Cast<CancelledCases> ().ToList ();
				if(searchBar.SelectedScopeButtonIndex ==0)
					CancelledList = CancelledList.FindAll (x => x.PatientName.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()));

				if (searchBar.SelectedScopeButtonIndex == 1) {
					List<CancelledCases> surgeonCancelledList = new List<CancelledCases> ();
					foreach (CancelledCases cancelledCase in CancelledList) {
						Dictionary<string, int?> lstSurgeon = new Dictionary<string, int?> ();
						lstSurgeon.Add ("Surgeon", cancelledCase.Surgeon);
						lstSurgeon.Add ("Surgeon2", cancelledCase.Surgeon2);
						lstSurgeon.Add ("Surgeon3", cancelledCase.Surgeon3);

						string surgeonName = iProPQRSPortableLib.Consts.GetSurgeon (lstSurgeon);

						if(surgeonName.Trim().ToLower().Contains(searchBar.Text.Trim().ToLower()) || searchBar.Text.Trim() == string.Empty)
							surgeonCancelledList.Add(cancelledCase);
					}
					CancelledList = surgeonCancelledList;
				}

				if (searchBar.SelectedScopeButtonIndex == 2) {
					List<CancelledCases> anesthesioCancelledList = new List<CancelledCases> ();
					foreach (CancelledCases cancelledCase in CancelledList) {
						Dictionary<string, int?> lstAnesthesiologists = new Dictionary<string, int?> ();
						lstAnesthesiologists.Add ("Anesthesiologist1", cancelledCase.Anesthesiologist1);
						lstAnesthesiologists.Add ("Anesthesiologist2", cancelledCase.Anesthesiologist2);
						lstAnesthesiologists.Add ("Anesthesiologist3", cancelledCase.Anesthesiologist3);
						lstAnesthesiologists.Add ("Anesthesiologist4", cancelledCase.Anesthesiologist4);

						string anesthesiologist = iProPQRSPortableLib.Consts.GetAnesthesiologist (lstAnesthesiologists);

						if(anesthesiologist.Trim().ToLower().Contains(searchBar.Text.Trim().ToLower()) || searchBar.Text.Trim() == string.Empty)
							anesthesioCancelledList.Add(cancelledCase);
					}
					CancelledList = anesthesioCancelledList;
				}
			if (searchBar.SelectedScopeButtonIndex == 3 && !string.IsNullOrEmpty(iProPQRSPortableLib.Consts.mId)) {

				List<CancelledCases> CancelledCasesList = new List<CancelledCases> ();					
				foreach (CancelledCases myCase in CancelledCasesList.FindAll(u=>u.Anesthesiologist1.ToString()==iProPQRSPortableLib.Consts.mId||u.Anesthesiologist2.ToString()==iProPQRSPortableLib.Consts.mId||u.Anesthesiologist3.ToString()==iProPQRSPortableLib.Consts.mId||u.Anesthesiologist4.ToString()==iProPQRSPortableLib.Consts.mId||u.CRNA1.ToString()==iProPQRSPortableLib.Consts.mId||u.CRNA2.ToString()==iProPQRSPortableLib.Consts.mId||u.CRNA3.ToString()==iProPQRSPortableLib.Consts.mId||u.CRNA4.ToString()==iProPQRSPortableLib.Consts.mId||u.SRNA1.ToString()==iProPQRSPortableLib.Consts.mId)) {
						if (searchBar.Text.Trim() == string.Empty|| myCase.PatientName.Trim ().ToLower ().Contains (searchBar.Text.Trim ().ToLower ()))
						CancelledCasesList.Add (myCase);
					}
				CancelledList = CancelledCasesList;
				}
				PatientItemGroup tGroup5 = new PatientItemGroup () { 
					Name="Cancelled cases"
				};
				foreach (CancelledCases filtereditem in CancelledList) {
					tGroup4.ListItems.Add (filtereditem);
				}
				filteredTableItems.Add (tGroup5);


				tableSource = new PatientTableSource (filteredTableItems, patListControllerObj);
			//}else
			//	tableSource = new PatientTableSource (tableItems, patListControllerObj);
			
			return tableSource;
		}
	}
}


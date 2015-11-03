
using System;

using Foundation;
using UIKit;

namespace iProPQRS
{
	public partial class PatientSectionHeaderView : UIViewController
	{
		PatientTableSource patTableSource;
		PatientListView patListController;


		public UILabel TitleLabel
		{
			get{return this.titleLbl;}
			set{this.titleLbl = value;}
		}

		public UIButton DisClosureButton
		{
			get{return this.disclosurebtn; }
			set{this.disclosurebtn = value; }
		}

		nint section = -1;

		public PatientSectionHeaderView (PatientTableSource patTableSource,PatientListView patListController,nint section) : base ("PatientSectionHeaderView", null)
		{
			this.patTableSource = patTableSource;
			this.patListController = patListController;
			this.section = section;

		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.DisClosureButton.SetImage(UIImage.FromFile("carat-open.png"),UIControlState.Selected);

			this.customBackGroundView.BackgroundColor = UIColor.FromPatternImage (UIImage.FromFile ("header_patient.png"));


			UITapGestureRecognizer tapGesture = new UITapGestureRecognizer (ExpandCollapseRows);
			tapGesture.NumberOfTapsRequired = 1;
			tapGesture.NumberOfTouchesRequired = 1;
			this.customBackGroundView.AddGestureRecognizer (tapGesture);

			string rowCount = this.patTableSource.procTableItems [(int)this.section].PatientProcedureListItems.Count.ToString ();
			this.TitleLabel.Text = this.patTableSource.procTableItems [(int)this.section].StatusName + " ("+ rowCount +")";

			if (this.section == 0) {
				if (this.patTableSource.lstCollapsedSections ["Section 0"] == 0) {
					this.DisClosureButton.Selected = false;
				} else {
					this.DisClosureButton.Selected = true;
				}
			}

			if (this.section == 1) {
				if (this.patTableSource.lstCollapsedSections ["Section 1"] == 0) {
					this.DisClosureButton.Selected = false;
				} else {
					this.DisClosureButton.Selected = true;
				}
			}

			if (this.section == 2) {
				if (this.patTableSource.lstCollapsedSections ["Section 2"] == 0) {
					this.DisClosureButton.Selected = false;
				} else {
					this.DisClosureButton.Selected = true;
				}
			}

			if (this.section == 3) {
				if (this.patTableSource.lstCollapsedSections ["Section 3"] == 0) {
					this.DisClosureButton.Selected = false;
				} else {
					this.DisClosureButton.Selected = true;
				}
			}

			if (this.section == 4) {
				if (this.patTableSource.lstCollapsedSections ["Section 4"] == 0) {
					this.DisClosureButton.Selected = false;
				} else {
					this.DisClosureButton.Selected = true;
				}
			}

			/*if (this.section == 0) {
				string rowCount = this.patTableSource.tableItems [(int)this.section].ListItems.Count.ToString ();
				this.TitleLabel.Text = "Incomplete Cases ("+ rowCount +")";
				if (this.patTableSource.lstCollapsedSections ["Section 0"] == 0) {
					this.DisClosureButton.Selected = false;
				} else {
					this.DisClosureButton.Selected = true;
				}
			}

			if (this.section == 1) {
				string rowCount = this.patTableSource.tableItems [(int)this.section].ListItems.Count.ToString ();
				this.TitleLabel.Text = "Scheduled Cases ("+ rowCount +")";
				if (this.patTableSource.lstCollapsedSections ["Section 1"] == 0) {
					this.DisClosureButton.Selected = false;
				} else {
					this.DisClosureButton.Selected = true;
				}
			}*/
			
			this.DisClosureButton.TouchUpInside += (object sender, EventArgs e) => {
				ExpandCollapseRows();
			};
		}

		private void ExpandCollapseRows()
		{

			if(this.section == 0){
				int val = this.patTableSource.lstCollapsedSections["Section 0"];
				this.patTableSource.lstCollapsedSections.Remove("Section 0");
				if(val == 0){
					this.patTableSource.lstCollapsedSections.Add ("Section 0", 1) ;
				}else{
					this.patTableSource.lstCollapsedSections.Add ("Section 0", 0);
				}
			}

			if(this.section == 1){
				int val = this.patTableSource.lstCollapsedSections["Section 1"];
				this.patTableSource.lstCollapsedSections.Remove("Section 1");
				if(val == 0){
					this.patTableSource.lstCollapsedSections.Add ("Section 1", 1) ;
				}else{
					this.patTableSource.lstCollapsedSections.Add ("Section 1", 0);
				}
			}
			if (this.section == 2) {
				int val = this.patTableSource.lstCollapsedSections ["Section 2"];
				this.patTableSource.lstCollapsedSections.Remove ("Section 2");
				if (val == 0){
					this.disclosurebtn.Selected = true;
					this.patTableSource.lstCollapsedSections.Add ("Section 2", 1);
				}else{
					this.disclosurebtn.Selected = false;
					this.patTableSource.lstCollapsedSections.Add ("Section 2", 0);
				}
			}
			if (this.section == 3) {
				int val = this.patTableSource.lstCollapsedSections ["Section 3"];
				this.patTableSource.lstCollapsedSections.Remove ("Section 3");
				if (val == 0){
					this.disclosurebtn.Selected = true;
					this.patTableSource.lstCollapsedSections.Add ("Section 3", 1);
				}else{
					this.disclosurebtn.Selected = false;
					this.patTableSource.lstCollapsedSections.Add ("Section 3", 0);
				}
			}
			if (this.section == 4) {
				int val = this.patTableSource.lstCollapsedSections ["Section 4"];
				this.patTableSource.lstCollapsedSections.Remove ("Section 4");
				if (val == 0){
					this.disclosurebtn.Selected = true;
					this.patTableSource.lstCollapsedSections.Add ("Section 4", 1);
				}else{
					this.disclosurebtn.Selected = false;
					this.patTableSource.lstCollapsedSections.Add ("Section 4", 0);
				}
			}

			this.patListController.PatientTableView.ReloadData();
		}

	}
}


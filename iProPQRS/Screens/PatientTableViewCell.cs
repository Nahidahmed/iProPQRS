using System;
using Foundation;
using UIKit;

namespace iProPQRS
{
	public partial class PatientTableViewCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("PatientTableViewCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("PatientTableViewCell");

		public UIButton PreOPButton
		{
			get{ return this.preopBtn;}
			set{ this.preopBtn = value;}
		}

		public UIButton PostOPButton
		{
			get{ return this.postOpBtn;}
			set{ this.postOpBtn = value;}
		}

		public UIButton IntraOPButton
		{
			get{ return this.intraOpBtn;}
			set{ this.intraOpBtn = value;}
		}

		public UIButton PDFButton
		{
			get{ return this.pdfButton;}
			set{ this.pdfButton = value;}
		}
		public UILabel AnesthesiologistLbl
		{
			get{return this.anesthesiologistLbl; }
			set{ this.anesthesiologistLbl = value;}
		}
		public UILabel CRNALbl
		{
			get{ return this.crnaLbl;}
			set{ this.crnaLbl = value;}
		}

		public UILabel ScheduledDateTime
		{
			get{ return this.scheduledDate;}
			set{ this.scheduledDate = value;}
		}
		public UILabel RWUser
		{
			get{return this.rwUser;}
			set{this.rwUser = value;}
		}

		public UILabel UnLockedLbl
		{
			get{return this.unLockedLbl;}
			set{this.unLockedLbl = value;}
		}

		public UILabel PatientDOB
		{
			get{ return this.patientDOB;}
			set{ this.patientDOB = value;}
		}
		public UILabel PatientName
		{
			get{return this.patientName;}
			set{this.patientName = value;}
		}

		public UILabel MRNumber
		{
			get{return this.mrNumber;}
			set{this.mrNumber = value;}
		}

		public UILabel SurgeonName
		{
			get{ return this.surgeonName;}
			set{ this.surgeonName = value;}
		}

		public PatientTableViewCell (IntPtr handle) : base (handle)
		{
		}

		public static PatientTableViewCell Create ()
		{
			return (PatientTableViewCell)Nib.Instantiate (null, null) [0];
		}
	}
}


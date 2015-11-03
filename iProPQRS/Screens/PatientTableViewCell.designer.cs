// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace iProPQRS
{
	[Register ("PatientTableViewCell")]
	partial class PatientTableViewCell
	{
		[Outlet]
		UIKit.UILabel anesthesiologistLbl { get; set; }

		[Outlet]
		UIKit.UILabel crnaLbl { get; set; }

		[Outlet]
		UIKit.UIButton intraOpBtn { get; set; }

		[Outlet]
		UIKit.UILabel mrNumber { get; set; }

		[Outlet]
		UIKit.UILabel patientDOB { get; set; }

		[Outlet]
		UIKit.UILabel patientName { get; set; }

		[Outlet]
		UIKit.UIButton pdfButton { get; set; }

		[Outlet]
		UIKit.UIButton postOpBtn { get; set; }

		[Outlet]
		UIKit.UIButton preopBtn { get; set; }

		[Outlet]
		UIKit.UILabel rwUser { get; set; }

		[Outlet]
		UIKit.UILabel scheduledDate { get; set; }

		[Outlet]
		UIKit.UILabel surgeonName { get; set; }

		[Outlet]
		UIKit.UILabel unLockedLbl { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (intraOpBtn != null) {
				intraOpBtn.Dispose ();
				intraOpBtn = null;
			}

			if (postOpBtn != null) {
				postOpBtn.Dispose ();
				postOpBtn = null;
			}

			if (preopBtn != null) {
				preopBtn.Dispose ();
				preopBtn = null;
			}

			if (anesthesiologistLbl != null) {
				anesthesiologistLbl.Dispose ();
				anesthesiologistLbl = null;
			}

			if (crnaLbl != null) {
				crnaLbl.Dispose ();
				crnaLbl = null;
			}

			if (mrNumber != null) {
				mrNumber.Dispose ();
				mrNumber = null;
			}

			if (patientDOB != null) {
				patientDOB.Dispose ();
				patientDOB = null;
			}

			if (patientName != null) {
				patientName.Dispose ();
				patientName = null;
			}

			if (pdfButton != null) {
				pdfButton.Dispose ();
				pdfButton = null;
			}

			if (rwUser != null) {
				rwUser.Dispose ();
				rwUser = null;
			}

			if (scheduledDate != null) {
				scheduledDate.Dispose ();
				scheduledDate = null;
			}

			if (surgeonName != null) {
				surgeonName.Dispose ();
				surgeonName = null;
			}

			if (unLockedLbl != null) {
				unLockedLbl.Dispose ();
				unLockedLbl = null;
			}
		}
	}
}

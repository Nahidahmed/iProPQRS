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
	[Register ("AddPatientViewController")]
	partial class AddPatientViewController
	{
		[Outlet]
		UIKit.UIButton addNewDiagnosis { get; set; }

		[Outlet]
		UIKit.UIButton addNewProc { get; set; }

		[Outlet]
		UIKit.UIButton BackBtn { get; set; }

		[Outlet]
		UIKit.UIButton btnSubmit { get; set; }

		[Outlet]
		UIKit.UIButton deleteDiagnosis { get; set; }

		[Outlet]
		UIKit.UIScrollView diagnosisScrollView { get; set; }

		[Outlet]
		UIKit.UIButton dropAnyUnwanted { get; set; }

		[Outlet]
		UIKit.UIButton dropPotoc { get; set; }

		[Outlet]
		UIKit.UIButton DropReason { get; set; }

		[Outlet]
		UIKit.UIButton EncounterBtn { get; set; }

		[Outlet]
		UIKit.UILabel lblFullName { get; set; }

		[Outlet]
		UIKit.UILabel LblPreopPainScoreValue { get; set; }

		[Outlet]
		UIKit.UILabel lblTopAccountNo { get; set; }

		[Outlet]
		UIKit.UILabel lbltopmrn { get; set; }

		[Outlet]
		UIKit.UILabel mrnLbl { get; set; }

		[Outlet]
		UIKit.UIButton PatientsBackBtn { get; set; }

		[Outlet]
		UIKit.UIButton PhysicalStatusBtn { get; set; }

		[Outlet]
		UIKit.UIScrollView procedureScrollView { get; set; }

		[Outlet]
		UIKit.UIButton removeProc { get; set; }

		[Outlet]
		UIKit.UISegmentedControl scAnyUnwanted { get; set; }

		[Outlet]
		UIKit.UISegmentedControl scPotoc { get; set; }

		[Outlet]
		UIKit.UISegmentedControl sCtab { get; set; }

		[Outlet]
		UIKit.UISlider SliderPreopPainScore { get; set; }

		[Outlet]
		UIKit.UIScrollView svBillingInfo { get; set; }

		[Outlet]
		UIKit.UITextField tctSurgeryDate { get; set; }

		[Outlet]
		UIKit.UITextField TxtAge { get; set; }

		[Outlet]
		UIKit.UITextField txtAnesthesiaTechs { get; set; }

		[Outlet]
		UIKit.UITextField txtCodeDiagnosis1 { get; set; }

		[Outlet]
		UIKit.UITextField txtcodeProcedure1 { get; set; }

		[Outlet]
		UIKit.UITextField txtDiagnosis1 { get; set; }

		[Outlet]
		UIKit.UITextField txtDOB { get; set; }

		[Outlet]
		UIKit.UITextField txtFacility { get; set; }

		[Outlet]
		UIKit.UITextField TxtFirstName { get; set; }

		[Outlet]
		UIKit.UITextField TxtLastName { get; set; }

		[Outlet]
		UIKit.UITextField TxtMRN { get; set; }

		[Outlet]
		UIKit.UITextField txtpAge { get; set; }

		[Outlet]
		UIKit.UITextField txtpDOB { get; set; }

		[Outlet]
		UIKit.UITextField txtpFirstName { get; set; }

		[Outlet]
		UIKit.UITextField txtpLastName { get; set; }

		[Outlet]
		UIKit.UITextField txtpmrn { get; set; }

		[Outlet]
		UIKit.UITextField txtProcedure1 { get; set; }

		[Outlet]
		UIKit.UISwitch uSEmergency { get; set; }

		[Outlet]
		UIKit.UISwitch uSwitchCaseCanceled { get; set; }

		[Outlet]
		UIKit.UIView uv17daysoutcomes { get; set; }

		[Outlet]
		UIKit.UIView uvAnesthesia { get; set; }

		[Outlet]
		UIKit.UIView uvAnesthesiologist { get; set; }

		[Outlet]
		UIKit.UIView uvAnesthImageCap { get; set; }

		[Outlet]
		UIKit.UIView uvAnesthTech { get; set; }

		[Outlet]
		UIKit.UIView uvBillingInfo { get; set; }

		[Outlet]
		UIKit.UIView uvCaseCancellation { get; set; }

		[Outlet]
		UIKit.UIView uvCRNA { get; set; }

		[Outlet]
		UIKit.UIView uvDiagnosis { get; set; }

		[Outlet]
		UIKit.UIView uvLines { get; set; }

		[Outlet]
		UIKit.UIView uvLocationInformation { get; set; }

		[Outlet]
		UIKit.UIView uvMain { get; set; }

		[Outlet]
		UIKit.UIView uvMainPatientinfo { get; set; }

		[Outlet]
		UIKit.UIView uvNurseSignature { get; set; }

		[Outlet]
		UIKit.UIView uvNurseSignaturevalue { get; set; }

		[Outlet]
		UIKit.UIView uvPACUdischarge { get; set; }

		[Outlet]
		UIKit.UIView uvPACUPostOp { get; set; }

		[Outlet]
		UIKit.UIView uvPatientInfo { get; set; }

		[Outlet]
		UIKit.UIView uvPatientSignature { get; set; }

		[Outlet]
		UIKit.UIView uvPreIntraOp { get; set; }

		[Outlet]
		UIKit.UIView uvProcedure { get; set; }

		[Outlet]
		UIKit.UIView uvProviders { get; set; }

		[Outlet]
		UIKit.UIView uvQualityMetrics { get; set; }

		[Outlet]
		UIKit.UIView uvRegional { get; set; }

		[Outlet]
		UIKit.UIView uvSCIPPQRS { get; set; }

		[Outlet]
		UIKit.UIScrollView uvScrollview { get; set; }

		[Outlet]
		UIKit.UIView uvSpecialTech { get; set; }

		[Outlet]
		UIKit.UIView uvSurgeons { get; set; }

		[Outlet]
		UIKit.UIView vuPI { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (addNewDiagnosis != null) {
				addNewDiagnosis.Dispose ();
				addNewDiagnosis = null;
			}

			if (addNewProc != null) {
				addNewProc.Dispose ();
				addNewProc = null;
			}

			if (BackBtn != null) {
				BackBtn.Dispose ();
				BackBtn = null;
			}

			if (btnSubmit != null) {
				btnSubmit.Dispose ();
				btnSubmit = null;
			}

			if (deleteDiagnosis != null) {
				deleteDiagnosis.Dispose ();
				deleteDiagnosis = null;
			}

			if (diagnosisScrollView != null) {
				diagnosisScrollView.Dispose ();
				diagnosisScrollView = null;
			}

			if (dropAnyUnwanted != null) {
				dropAnyUnwanted.Dispose ();
				dropAnyUnwanted = null;
			}

			if (dropPotoc != null) {
				dropPotoc.Dispose ();
				dropPotoc = null;
			}

			if (DropReason != null) {
				DropReason.Dispose ();
				DropReason = null;
			}

			if (EncounterBtn != null) {
				EncounterBtn.Dispose ();
				EncounterBtn = null;
			}

			if (lblFullName != null) {
				lblFullName.Dispose ();
				lblFullName = null;
			}

			if (LblPreopPainScoreValue != null) {
				LblPreopPainScoreValue.Dispose ();
				LblPreopPainScoreValue = null;
			}

			if (lblTopAccountNo != null) {
				lblTopAccountNo.Dispose ();
				lblTopAccountNo = null;
			}

			if (lbltopmrn != null) {
				lbltopmrn.Dispose ();
				lbltopmrn = null;
			}

			if (mrnLbl != null) {
				mrnLbl.Dispose ();
				mrnLbl = null;
			}

			if (PatientsBackBtn != null) {
				PatientsBackBtn.Dispose ();
				PatientsBackBtn = null;
			}

			if (PhysicalStatusBtn != null) {
				PhysicalStatusBtn.Dispose ();
				PhysicalStatusBtn = null;
			}

			if (procedureScrollView != null) {
				procedureScrollView.Dispose ();
				procedureScrollView = null;
			}

			if (removeProc != null) {
				removeProc.Dispose ();
				removeProc = null;
			}

			if (scAnyUnwanted != null) {
				scAnyUnwanted.Dispose ();
				scAnyUnwanted = null;
			}

			if (scPotoc != null) {
				scPotoc.Dispose ();
				scPotoc = null;
			}

			if (sCtab != null) {
				sCtab.Dispose ();
				sCtab = null;
			}

			if (SliderPreopPainScore != null) {
				SliderPreopPainScore.Dispose ();
				SliderPreopPainScore = null;
			}

			if (svBillingInfo != null) {
				svBillingInfo.Dispose ();
				svBillingInfo = null;
			}

			if (tctSurgeryDate != null) {
				tctSurgeryDate.Dispose ();
				tctSurgeryDate = null;
			}

			if (TxtAge != null) {
				TxtAge.Dispose ();
				TxtAge = null;
			}

			if (txtCodeDiagnosis1 != null) {
				txtCodeDiagnosis1.Dispose ();
				txtCodeDiagnosis1 = null;
			}

			if (txtcodeProcedure1 != null) {
				txtcodeProcedure1.Dispose ();
				txtcodeProcedure1 = null;
			}

			if (txtDiagnosis1 != null) {
				txtDiagnosis1.Dispose ();
				txtDiagnosis1 = null;
			}

			if (txtDOB != null) {
				txtDOB.Dispose ();
				txtDOB = null;
			}

			if (txtFacility != null) {
				txtFacility.Dispose ();
				txtFacility = null;
			}

			if (TxtFirstName != null) {
				TxtFirstName.Dispose ();
				TxtFirstName = null;
			}

			if (TxtLastName != null) {
				TxtLastName.Dispose ();
				TxtLastName = null;
			}

			if (TxtMRN != null) {
				TxtMRN.Dispose ();
				TxtMRN = null;
			}

			if (txtpAge != null) {
				txtpAge.Dispose ();
				txtpAge = null;
			}

			if (txtpDOB != null) {
				txtpDOB.Dispose ();
				txtpDOB = null;
			}

			if (txtpFirstName != null) {
				txtpFirstName.Dispose ();
				txtpFirstName = null;
			}

			if (txtpLastName != null) {
				txtpLastName.Dispose ();
				txtpLastName = null;
			}

			if (txtpmrn != null) {
				txtpmrn.Dispose ();
				txtpmrn = null;
			}

			if (txtProcedure1 != null) {
				txtProcedure1.Dispose ();
				txtProcedure1 = null;
			}

			if (uSEmergency != null) {
				uSEmergency.Dispose ();
				uSEmergency = null;
			}

			if (uSwitchCaseCanceled != null) {
				uSwitchCaseCanceled.Dispose ();
				uSwitchCaseCanceled = null;
			}

			if (uv17daysoutcomes != null) {
				uv17daysoutcomes.Dispose ();
				uv17daysoutcomes = null;
			}

			if (uvAnesthesia != null) {
				uvAnesthesia.Dispose ();
				uvAnesthesia = null;
			}

			if (uvAnesthesiologist != null) {
				uvAnesthesiologist.Dispose ();
				uvAnesthesiologist = null;
			}

			if (uvAnesthImageCap != null) {
				uvAnesthImageCap.Dispose ();
				uvAnesthImageCap = null;
			}

			if (uvAnesthTech != null) {
				uvAnesthTech.Dispose ();
				uvAnesthTech = null;
			}

			if (uvBillingInfo != null) {
				uvBillingInfo.Dispose ();
				uvBillingInfo = null;
			}

			if (uvCaseCancellation != null) {
				uvCaseCancellation.Dispose ();
				uvCaseCancellation = null;
			}

			if (uvCRNA != null) {
				uvCRNA.Dispose ();
				uvCRNA = null;
			}

			if (uvDiagnosis != null) {
				uvDiagnosis.Dispose ();
				uvDiagnosis = null;
			}

			if (uvLines != null) {
				uvLines.Dispose ();
				uvLines = null;
			}

			if (uvLocationInformation != null) {
				uvLocationInformation.Dispose ();
				uvLocationInformation = null;
			}

			if (uvMain != null) {
				uvMain.Dispose ();
				uvMain = null;
			}

			if (uvMainPatientinfo != null) {
				uvMainPatientinfo.Dispose ();
				uvMainPatientinfo = null;
			}

			if (uvNurseSignature != null) {
				uvNurseSignature.Dispose ();
				uvNurseSignature = null;
			}

			if (uvNurseSignaturevalue != null) {
				uvNurseSignaturevalue.Dispose ();
				uvNurseSignaturevalue = null;
			}

			if (uvPACUdischarge != null) {
				uvPACUdischarge.Dispose ();
				uvPACUdischarge = null;
			}

			if (uvPACUPostOp != null) {
				uvPACUPostOp.Dispose ();
				uvPACUPostOp = null;
			}

			if (uvPatientInfo != null) {
				uvPatientInfo.Dispose ();
				uvPatientInfo = null;
			}

			if (uvPatientSignature != null) {
				uvPatientSignature.Dispose ();
				uvPatientSignature = null;
			}

			if (uvPreIntraOp != null) {
				uvPreIntraOp.Dispose ();
				uvPreIntraOp = null;
			}

			if (uvProcedure != null) {
				uvProcedure.Dispose ();
				uvProcedure = null;
			}

			if (uvProviders != null) {
				uvProviders.Dispose ();
				uvProviders = null;
			}

			if (uvQualityMetrics != null) {
				uvQualityMetrics.Dispose ();
				uvQualityMetrics = null;
			}

			if (uvRegional != null) {
				uvRegional.Dispose ();
				uvRegional = null;
			}

			if (uvSCIPPQRS != null) {
				uvSCIPPQRS.Dispose ();
				uvSCIPPQRS = null;
			}

			if (uvScrollview != null) {
				uvScrollview.Dispose ();
				uvScrollview = null;
			}

			if (uvSpecialTech != null) {
				uvSpecialTech.Dispose ();
				uvSpecialTech = null;
			}

			if (uvSurgeons != null) {
				uvSurgeons.Dispose ();
				uvSurgeons = null;
			}

			if (vuPI != null) {
				vuPI.Dispose ();
				vuPI = null;
			}

			if (txtAnesthesiaTechs != null) {
				txtAnesthesiaTechs.Dispose ();
				txtAnesthesiaTechs = null;
			}
		}
	}
}

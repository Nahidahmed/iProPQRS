using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using iProPQRSPortableLib;

namespace iProPQRS
{
	public delegate void UpdateQualityMetricsView();

	public partial class QualityMetricsForm : UIViewController
	{
		public event UpdateQualityMetricsView _ValueChanged;
		public UIPopoverController popover;
		List<iProPQRSPortableLib.Option> ASA7MasterList = new List<iProPQRSPortableLib.Option>();
		List<iProPQRSPortableLib.Option> ASA7N1MasterList = new List<iProPQRSPortableLib.Option>();
		List<iProPQRSPortableLib.Option> ASA7N2MasterList = new List<iProPQRSPortableLib.Option>();
		public int ProcID=0;
		List<int>  selectedMasterASA7OptionsIds = new List<int>();
		List<int>  selectedMasterASA7N1OptionsIds = new List<int>();
		List<int>  selectedMasterASA7N2OptionsIds = new List<int>();
		List<AttribType> SelectedAllType=new List<AttribType>();
		List<AttribType> SaveAttribTypeList=new List<AttribType>();
		UILabel lblDesc;
		public QualityMetricsForm () : base ("QualityMetricsForm", null)
		{
		}
		public QualityMetricsForm (UILabel lblDesc) : base ("QualityMetricsForm", null)
		{
			this.lblDesc = lblDesc;
		}
		List<string> lbltexts=new List<string>(); 
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			if (iProPQRSPortableLib.Consts.SelectedProcAttribtslist != null) {
				SelectedAllType = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result;
				ASA7MasterList = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.FindAll (x => x.ProcAttribTypeID == 606);
				ASA7N1MasterList = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.FindAll (x => x.ProcAttribTypeID == 683);
				ASA7N2MasterList = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.FindAll (x => x.ProcAttribTypeID == 684);
		
				selectedMasterASA7OptionsIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll(u => (ASA7MasterList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).Select(x=>x.ProcAttribTypeID).ToList();
				selectedMasterASA7N1OptionsIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll(u => (ASA7N1MasterList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).Select(x=>x.ProcAttribTypeID).ToList();
				selectedMasterASA7N2OptionsIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll(u => (ASA7N2MasterList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).Select(x=>x.ProcAttribTypeID).ToList();
			}

			#region ASA7 Numerator 1
			lblASA7N1Option1.Text = ASA7N1MasterList [0].Description;
			lblASA7N1Option2.Text = ASA7N1MasterList [1].Description;
			#endregion

			#region ASA7 Numerator 2
			lblASA7N2Option1.Text = ASA7N2MasterList [0].Description;
			lblASA7N2Option2.Text = ASA7N2MasterList [1].Description;
			#endregion

			#region ASA7 Options
			lblASA7Option1.Text = ASA7MasterList [0].Description;
			lblASA7Option2.Text = ASA7MasterList [1].Description;
			lblASA7Option3.Text = ASA7MasterList [2].Description;
			#endregion

			btnCancel.TouchUpInside += (object sender, EventArgs e) => {
//				this.NavigationController.PopViewController(false);
				DismissViewController(false,null);
			};
			//start N1
			swASA7N1Option1.ValueChanged += (object sender, EventArgs e) => {
				//SingleSelectionOfSwitches(swASA7N1Option1,swASA7N1Option2);
//					EnableDisableOptions();

				if(swASA7N1Option1.On)
				{
					lbltexts.Add (ASA7N1MasterList [0].Description);
					swASA7N1Option2.On=false;
				}
				else
					lbltexts.Remove (ASA7N1MasterList [0].Description);
				

			};

			swASA7N1Option2.ValueChanged += (object sender, EventArgs e) => {
				//SingleSelectionOfSwitches(swASA7N1Option2,swASA7N1Option1);
//				EnableDisableOptions();

				if(swASA7N1Option2.On)
				{
					lbltexts.Add (ASA7N1MasterList [1].Description);
					swASA7N1Option1.On=false;
				}
				else
					lbltexts.Remove (ASA7N1MasterList [1].Description);

			};

			if (ASA7N1MasterList != null && ASA7N1MasterList.Count > 1) {
				swASA7N1Option1.Tag = (nint)ASA7N1MasterList [0].ProcAttribTypeID;
				AttribType sitem1 = SelectedAllType.Find (u => u.ProcAttribTypeID == ASA7N1MasterList [0].ProcAttribTypeID && u.Value == ASA7N1MasterList [0].Value);
				if (sitem1 !=null) {
					lbltexts.Add (ASA7N1MasterList [0].Description);
					swASA7N1Option1.On = true;
				}

				swASA7N1Option2.Tag = (nint)ASA7N1MasterList [1].ProcAttribTypeID;
				AttribType sitem2 = SelectedAllType.Find (u => u.ProcAttribTypeID == ASA7N1MasterList [1].ProcAttribTypeID && u.Value == ASA7N1MasterList [1].Value);
				if (sitem2 !=null) {
					lbltexts.Add (ASA7N1MasterList [1].Description);
					swASA7N1Option2.On = true;
				}
			}

			//End N1

			//start N2
			swASA7N2Option1.ValueChanged += (object sender, EventArgs e) => {
			//	SingleSelectionOfSwitches(swASA7N2Option1,swASA7N2Option2);
//				EnableDisableOptions();

				if(swASA7N2Option1.On)
				{
					swASA7N2Option2.On=false;
					lbltexts.Add (ASA7N2MasterList [0].Description);
				}
				else
					lbltexts.Remove (ASA7N2MasterList [0].Description);

			};

			swASA7N2Option2.ValueChanged += (object sender, EventArgs e) => {
				//SingleSelectionOfSwitches(swASA7N2Option2,swASA7N2Option1);
//				EnableDisableOptions();

				if(swASA7N2Option2.On)
				{
					swASA7N2Option1.On=false;
					lbltexts.Add (ASA7N2MasterList [1].Description);
				}
				else
					lbltexts.Remove (ASA7N2MasterList [1].Description);

			};

			if (ASA7N2MasterList != null && ASA7N2MasterList.Count > 1) {
				swASA7N2Option1.Tag = (nint)ASA7N2MasterList [0].ProcAttribTypeID;
				AttribType n2option1 = SelectedAllType.Find (u => u.ProcAttribTypeID == ASA7N2MasterList [0].ProcAttribTypeID && u.Value == ASA7N2MasterList [0].Value);
				if (n2option1 != null) {
					lbltexts.Add (ASA7N2MasterList [0].Description);
					swASA7N2Option1.On = true;
				}

				swASA7N2Option2.Tag = (nint)ASA7N2MasterList [1].ProcAttribTypeID;
				AttribType n2option2 = SelectedAllType.Find (u => u.ProcAttribTypeID == ASA7N2MasterList [1].ProcAttribTypeID && u.Value == ASA7N2MasterList [1].Value);
				if (n2option2 != null) {
					lbltexts.Add (ASA7N2MasterList [1].Description);
					swASA7N2Option2.On = true;
				}
			}
			//End N2
			//start N3
			swASA7Option1.ValueChanged += (object sender, EventArgs e) => {				
				//SingleSelectionOfSwitches(swASA7Option1,swASA7Option2,swASA7Option3);
				swASA7Option2.On = false;
				swASA7Option3.On = false;
				if(swASA7Option1.On)
				{
					swASA7Option2.On=false;
					swASA7Option3.On=false;
					lbltexts.Add (ASA7MasterList [0].Description);
				}
				else
					lbltexts.Remove (ASA7MasterList [0].Description);
			};
			swASA7Option2.ValueChanged += (object sender, EventArgs e) => {				
				//SingleSelectionOfSwitches(swASA7Option2,swASA7Option1,swASA7Option3);
				swASA7Option1.On = false;
				swASA7Option3.On = false;
				if(swASA7Option2.On)
				{
					swASA7Option1.On=false;
					swASA7Option3.On=false;
					lbltexts.Add (ASA7MasterList [1].Description);
				}
				else
					lbltexts.Remove (ASA7MasterList [1].Description);
			};
			swASA7Option3.ValueChanged += (object sender, EventArgs e) => {			
				//SingleSelectionOfSwitches(swASA7Option3,swASA7Option2,swASA7Option1);
				swASA7Option1.On = false;
				swASA7Option2.On = false;
				if(swASA7Option3.On)
					lbltexts.Add (ASA7MasterList [2].Description);
				else
					lbltexts.Remove (ASA7MasterList [2].Description);
				
			};
			if (ASA7MasterList != null && ASA7MasterList.Count > 2) {
				swASA7Option1.Tag = ASA7MasterList [0].ProcAttribTypeID;
				AttribType option1 = SelectedAllType.Find (u => u.ProcAttribTypeID == ASA7MasterList [0].ProcAttribTypeID && u.Value == ASA7MasterList [0].Value);
				if (option1 != null) {
					lbltexts.Add (ASA7MasterList [0].Description);
					swASA7Option1.On = true;

				}

				swASA7Option2.Tag = ASA7MasterList [1].ProcAttribTypeID;
				AttribType option2 = SelectedAllType.Find (u => u.ProcAttribTypeID == ASA7MasterList [1].ProcAttribTypeID && u.Value == ASA7MasterList [1].Value);
				if (option2 !=null) {
					lbltexts.Add (ASA7MasterList [1].Description);
					swASA7Option2.On = true;

				}

				swASA7Option3.Tag = ASA7MasterList [2].ProcAttribTypeID;
				AttribType option3 = SelectedAllType.Find (u => u.ProcAttribTypeID == ASA7MasterList [2].ProcAttribTypeID && u.Value == ASA7MasterList [2].Value);
				if (option3 != null) {
					lbltexts.Add (ASA7MasterList [2].Description);
					swASA7Option3.On = true;

				}
			}

			btnOk.TouchUpInside+=async (object sender, EventArgs e) => {
				btnOk.Enabled=false;

				if(ProcID != 0 )
				{
					//swASA7N1Option1.On = true; 
					if(swASA7N1Option1.On)
						AddAttribtype(swASA7N1Option1.Tag,"4554F");
					else		
						AddAttribtype(swASA7N1Option1.Tag,"");
					
					//swASA7N1Option2.On = true; 
					if(swASA7N1Option2.On)
						AddAttribtype(swASA7N1Option2.Tag,"4555F");
					else		
						AddAttribtype(swASA7N1Option2.Tag,"");

					//swASA7N2Option1.On = true; 
					if(swASA7N2Option1.On)
						AddAttribtype(swASA7N2Option1.Tag,"4556F");
					else		
						AddAttribtype(swASA7N2Option1.Tag,"");
					//swASA7N2Option2.On = true; 
					if(swASA7N2Option2.On)
						AddAttribtype(swASA7N2Option2.Tag,"4557F");
					else		
						AddAttribtype(swASA7N2Option2.Tag,"");

					//swASA7Option1.On = true; 
					if(swASA7Option1.On)
						AddAttribtype(swASA7Option1.Tag,"4558F");
					else		
						AddAttribtype(swASA7Option1.Tag,"");
					//swASA7Option2.On = true; 
					if(swASA7Option2.On)
						AddAttribtype(swASA7Option2.Tag,"4558F-1P");
					else		
						AddAttribtype(swASA7Option2.Tag,"");
					//swASA7Option3.On = true; 
					if(swASA7Option3.On)
						AddAttribtype(swASA7Option3.Tag,"4558F-8P");
					else		
						AddAttribtype(swASA7Option3.Tag,"");

					SaveAttribtype();
//					lbltexts.Clear();
//					foreach (var item in procalist) {
//						if(!string.IsNullOrEmpty(item.Value))
//						{
//							var Option=iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.Find(u=>u.ProcAttribTypeID == item.ProcAttribTypeID && u.Value==item.Value);
//							if(Option != null)
//							{
//								lbltexts.Add(Option.Description);
//							}
//						}
//					}
//
//					if(lbltexts.Count >0 )
//						lblDesc.Text=String.Join(", ", lbltexts.ToArray());

				}
			};
//			EnableDisableOptions ();
				
			// Perform any additional setup after loading the view, typically from a nib.
		}
		List<AttribType>  procalist=new List<AttribType>();
		public async void AddAttribtype(nint ProcAttribTypeID,string Value)
		{

			AttribType procitem=new AttribType();
			procitem.ProcAttribTypeID=(int)ProcAttribTypeID;
			procitem.ProcID=ProcID;
			procitem.Value = Value;
			procitem.IsHighLighted=false;
			procalist.Add(procitem);
		}
		public async void SaveAttribtype()
		{
			//procalist.Sort(
		
			procalist.Sort((xx,yy)=> xx.Value.CompareTo(yy.Value));
			if(procalist != null && procalist.Count > 0 ){
				var procAttribtsobject  = await AppDelegate.Current.pqrsMgr.UpdateProcAttribs(procalist);
				if(procAttribtsobject != null && procAttribtsobject.status != null && procAttribtsobject.status.ToUpper() == "OK")
				{
					iProPQRSPortableLib.Consts.SelectedProcAttribtslist  = await AppDelegate.Current.pqrsMgr.GetAllAttribTypesOfAProcedure(ProcID);

					int descwidth = 40;
					lblDesc.Lines = 1;
					int textcount = lblDesc.Text.Length;
					if (textcount < 58) {
						descwidth = 40;
						lblDesc.Lines = 1;
					} else if (textcount > 58 && textcount <= 114) {
						descwidth = 55;
						lblDesc.Lines = 2;
					} else if (textcount > 114) {
						descwidth = 75;
						lblDesc.Lines = 3;
					}

					lblDesc.Frame = new CoreGraphics.CGRect (500, 8, 480, descwidth);


					lbltexts.Clear();
					lblDesc.Text = string.Empty;
					var selectedMasterASA7OptionsIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (ASA7MasterList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).ToList ();
					var selectedMasterASA7N1OptionsIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (ASA7N1MasterList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).ToList ();
					var selectedMasterASA7N2OptionsIds = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (ASA7N2MasterList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).ToList ();
					foreach (var item in selectedMasterASA7N1OptionsIds) {
						Option opitem = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.Find (u => u.ProcAttribTypeID == item.ProcAttribTypeID && u.Value == item.Value);
						if (opitem != null) {
							lblDesc.Text = lblDesc.Text + opitem.Description + ", ";
						}
					}
					foreach (var item in selectedMasterASA7N2OptionsIds) {
						Option opitem = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.Find (u => u.ProcAttribTypeID == item.ProcAttribTypeID && u.Value == item.Value);
						if (opitem != null) {
							lblDesc.Text = lblDesc.Text + opitem.Description + ", ";
						}
					}
					foreach (var item in selectedMasterASA7OptionsIds) {
						Option opitem = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.Find (u => u.ProcAttribTypeID == item.ProcAttribTypeID && u.Value == item.Value);
						if (opitem != null) {
							lblDesc.Text = lblDesc.Text + opitem.Description + ", ";
						}
					}

					_ValueChanged += new UpdateQualityMetricsView(checkVal);
					_ValueChanged.Invoke ();

					DismissViewController(false,null);
				}
			}
		}

		private void checkVal()
		{
		}
		public void PresentFromPopover(UIView sender,float x,float y)
		{
			popover = new UIPopoverController(this)
			{
				PopoverContentSize = new SizeF(400, 511)
			};
			popover.PresentFromRect (new CoreGraphics.CGRect (x, y, 1, 1), sender, UIPopoverArrowDirection.Any, true);
			this.View.Layer.Frame = new CoreGraphics.CGRect (0, 0, 400, 511);
		}

		private void SingleSelectionOfSwitches(UISwitch switch1,UISwitch switch2){
			if (switch1.On)
				switch2.On = false;
			else
				switch2.On = true;
		}

		private void SingleSelectionOfSwitches(UISwitch switch1,UISwitch switch2,UISwitch switch3){
			if (switch1.On) {
				switch2.On = false;
				switch3.On = false;
			} else {
				switch2.On = true;
				switch3.On = true;
			}
		}
		private void EnableDisableOptions()
		{
			if (swASA7N2Option1.On == false) {
				swASA7Option1.On = false;
				swASA7Option2.On = false;
				swASA7Option3.On = false;
				swASA7Option1.Enabled = false;
				swASA7Option2.Enabled = false;
				swASA7Option3.Enabled = false;
			} else {
				swASA7Option1.Enabled = true;
				swASA7Option2.Enabled = true;
				swASA7Option3.Enabled = true;
			}
		}
	}
}


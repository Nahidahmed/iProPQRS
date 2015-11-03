
using System;

using Foundation;
using UIKit;
using System.Collections.Generic;
using iProPQRSPortableLib;
using System.Drawing;
using System.Linq;

namespace iProPQRS
{
	public partial class QualityMetricsASA : UIViewController
	{
		public event UpdateQualityMetricsView _ValueChanged;
		public UIPopoverController popover;
		public int ProcID=0;
		public List<iProPQRSPortableLib.Option> masterMainList = new List<iProPQRSPortableLib.Option>();
		public List<iProPQRSPortableLib.Option> masterSubCatList = new List<iProPQRSPortableLib.Option>();

		public List<iProPQRSPortableLib.Option> masterMainList2 = new List<iProPQRSPortableLib.Option>();

		public List<AttribType> SelectedmasterMainList=new List<AttribType>();
		public List<AttribType> SelectedmasterMainList2 = new List<AttribType>();
		public List<AttribType> SelectedmasterSubCatList=new List<AttribType>();
		List<AttribType> SelectedAllType=new List<AttribType>();
		List<AttribType> SaveAttribTypeList=new List<AttribType>();
		AttribType MainCat=new AttribType();
		AttribType MainCat2=new AttribType();
		AttribType MainSubCat=new AttribType();
		UILabel lblDesc;
		string buttonDesc;
		string subcatdesc=string.Empty;
		public QualityMetricsASA () : base ("QualityMetricsASA", null)
		{
		}

		public QualityMetricsASA (UILabel lblDesc) : base ("QualityMetricsASA", null)
		{
			this.lblDesc = lblDesc;
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		private void CreateCurveAndShadow(UIView view)
		{
			view.Layer.MasksToBounds = false;
			view.Layer.CornerRadius = 10;
			view.Layer.ShadowColor = UIColor.DarkGray.CGColor;
			view.Layer.ShadowOpacity = 1.0f;
			view.Layer.ShadowRadius = 6.0f;
			view.Layer.ShadowOffset = new System.Drawing.SizeF(0f, 3f);
		}
		bool PrevStatususMCItem1;
		bool PrevStatususMCItem2;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			masterMainList2 = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.FindAll(x => x.ProcAttribTypeID == 1001);
			if (iProPQRSPortableLib.Consts.SelectedProcAttribtslist != null) {
				SelectedAllType = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result;
				SelectedmasterMainList = SelectedAllType.FindAll (u => (masterMainList.Select (z => z.ProcAttribTypeID).Contains (u.ProcAttribTypeID)));
				SelectedmasterMainList2 = SelectedAllType.FindAll (u => (masterMainList2.Select (z => z.ProcAttribTypeID).Contains (u.ProcAttribTypeID)));
				SelectedmasterSubCatList = SelectedAllType.FindAll (u => (masterSubCatList.Select (z => z.ProcAttribTypeID).Contains (u.ProcAttribTypeID)));

//				SelectedmasterMainList = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (masterMainList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).ToList ();
//				SelectedmasterMainList2 = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (masterMainList2.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).ToList ();
//				SelectedmasterSubCatList = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (masterSubCatList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).ToList ();

			}
			//CreateCurveAndShadow (uvMainCatView);
			//CreateCurveAndShadow (uvSubCatView);
			uvMainCatView.Layer.BorderColor=UIColor.Black.CGColor;
			uvMainCatView.Layer.BorderWidth = 1;

			uvMainCatView2.Layer.BorderColor=UIColor.Black.CGColor;
			uvMainCatView2.Layer.BorderWidth = 1;

			uvSubCatView.Layer.BorderColor=UIColor.Black.CGColor;
			uvSubCatView.Layer.BorderWidth = 1;

//			usSCItem1.Enabled = false;
//			usSCItem2.Enabled = false;
//			usSCItem3.Enabled = false;
			if (masterMainList2 != null && masterMainList2.Count > 0) {
				if (masterMainList2.Count > 0) {
					ulMC2Item1.Text = masterMainList2 [0].Description;
					usMC2Item1.Tag = masterMainList2 [0].ProcAttribTypeID;

					if (SelectedmasterMainList2.Count > 0) {
						AttribType selecteditem = SelectedAllType.Find (u => u.ProcAttribTypeID == masterMainList2 [0].ProcAttribTypeID && u.Value == masterMainList2 [0].Value);
						if (selecteditem != null) {
							usMC2Item1.On = true;
							buttonDesc=masterMainList2 [0].Description;
							SaveAttribTypeList.Add (selecteditem);
							usMC2Item2.On=false;
						}else
							usMC2Item1.On = false;
					}
				}

				PrevStatususMCItem2 = usMCItem1.On;
				usMC2Item1.ValueChanged += (object sender, EventArgs e) => {
					AttribType additem=new AttribType();
					additem.IsHighLighted=false;
					additem.ProcAttribTypeID= masterMainList2 [0].ProcAttribTypeID;
					additem.ProcID=ProcID;
					additem.Value = masterMainList2 [0].Value;
					buttonDesc = masterMainList2[0].Description;
					if(usMC2Item1.On){
						List<AttribType> removeitems=new List<AttribType>();
						foreach (var item in SaveAttribTypeList) {
							item.Value="";
							removeitems.Add(item);
						}
						SaveAttribTypeList=removeitems;
						SaveAttribTypeList.Add(additem);
						usMC2Item2.On=false;
						PrevStatususMCItem2=usMC2Item1.On;
					}else{
						usMC2Item2.On=false;
						buttonDesc="";
						List<AttribType> removeitems=new List<AttribType>();
						foreach (var item in SaveAttribTypeList) {
							item.Value="";
							removeitems.Add(item);
						}
						SaveAttribTypeList=removeitems;
					}
				};

				ulMC2Item2.Text = masterMainList2 [1].Description;
				usMC2Item2.Tag = masterMainList2 [1].ProcAttribTypeID;
				if (SelectedmasterMainList2.Count > 0) {
					AttribType selecteditem = SelectedAllType.Find (u => u.ProcAttribTypeID == masterMainList2 [1].ProcAttribTypeID && u.Value == masterMainList2 [1].Value);
					if (selecteditem != null) {
						buttonDesc = masterMainList [1].Description;
						usMC2Item2.On = true;
						SaveAttribTypeList.Add (selecteditem);
					} else {
						usMC2Item2.On = false;
					}
				}
				usMC2Item2.ValueChanged += (object sender, EventArgs e) => {
					AttribType additem=new AttribType();
					additem.IsHighLighted=false;
					additem.ProcAttribTypeID= masterMainList2 [1].ProcAttribTypeID;
					additem.ProcID=ProcID;
					additem.Value=masterMainList2 [1].Value;
					buttonDesc=masterMainList2[1].Description;
					if(usMC2Item2.On){	
						usMC2Item1.On=false;
						MainCat2=additem;
					}else{
						usMC2Item1.On=false;
						buttonDesc="";
						AttribType checkmItem=SaveAttribTypeList.Find(u=>u.ProcAttribTypeID == masterMainList2 [1].ProcAttribTypeID);
						if(checkmItem != null){
							SaveAttribTypeList.Remove(checkmItem);
							checkmItem.Value="";
							SaveAttribTypeList.Add(checkmItem);
						}
					}
				};
			}

			if (masterMainList != null && masterMainList.Count > 0) {

				if (masterMainList.Count > 0) {
					ulMCItem1.Text = masterMainList [0].Description;
					usMCItem1.Tag = masterMainList [0].ProcAttribTypeID;

					if (SelectedmasterMainList.Count > 0) {
						AttribType selecteditem = SelectedAllType.Find (u => u.ProcAttribTypeID == masterMainList [0].ProcAttribTypeID && u.Value == masterMainList [0].Value);
						if (selecteditem != null) {
							usMCItem1.On = true;
							buttonDesc=masterMainList [0].Description;
							SaveAttribTypeList.Add (selecteditem);

							usMCItem2.On=false;
						}else
							usMCItem1.On = false;
					}
				}

				PrevStatususMCItem1 = usMCItem1.On;
				usMCItem1.ValueChanged += (object sender, EventArgs e) => {
					AttribType additem=new AttribType();
					additem.IsHighLighted=false;
					additem.ProcAttribTypeID= masterMainList [0].ProcAttribTypeID;
					additem.ProcID=ProcID;
					additem.Value=masterMainList [0].Value;
					buttonDesc=masterMainList[0].Description;
					if(usMCItem1.On){
						//MainCat=additem;
						List<AttribType> removeitems=new List<AttribType>();
						foreach (var item in SaveAttribTypeList) {
							item.Value="";
							removeitems.Add(item);
						}
						SaveAttribTypeList=removeitems;
						SaveAttribTypeList.Add(additem);

						usMCItem2.On=false;
						PrevStatususMCItem1=usMCItem1.On;
					}else{
						usMCItem2.On=false;
						buttonDesc="";
						List<AttribType> removeitems=new List<AttribType>();
						foreach (var item in SaveAttribTypeList) {
							item.Value="";
							removeitems.Add(item);
						}
						SaveAttribTypeList=removeitems;
					}
				};
				if (masterMainList.Count > 1) {
					ulMCItem2.Text = masterMainList [1].Description;
					usMCItem2.Tag = masterMainList [1].ProcAttribTypeID;
					if (SelectedmasterMainList.Count > 0) {
						AttribType selecteditem = SelectedAllType.Find (u => u.ProcAttribTypeID == masterMainList [1].ProcAttribTypeID && u.Value == masterMainList [1].Value);
						if (selecteditem != null) {
							buttonDesc = masterMainList [1].Description;
							usMCItem2.On = true;
							SaveAttribTypeList.Add (selecteditem);
						} else {
							usMCItem2.On = false;
						}
					}
					usMCItem2.ValueChanged += (object sender, EventArgs e) => {
						AttribType additem=new AttribType();
						additem.IsHighLighted=false;
						additem.ProcAttribTypeID= masterMainList [1].ProcAttribTypeID;
						additem.ProcID=ProcID;
						additem.Value=masterMainList [1].Value;
						buttonDesc=masterMainList[1].Description;
						if(usMCItem2.On){	
							usMCItem1.On=false;
							MainCat=additem;
						}else{
							usMCItem1.On=false;
							buttonDesc="";
							AttribType checkmItem=SaveAttribTypeList.Find(u=>u.ProcAttribTypeID == masterMainList [1].ProcAttribTypeID);
							if(checkmItem != null){
								SaveAttribTypeList.Remove(checkmItem);
								checkmItem.Value="";
								SaveAttribTypeList.Add(checkmItem);
							}
						}
					};
				}


			}
			//svSubCatList
			if (masterSubCatList != null && masterSubCatList.Count > 0) {
				if (masterSubCatList.Count > 0) {
					ulSCItem1.Text = masterSubCatList [0].Description;
					usSCItem1.Tag = masterSubCatList [0].ProcAttribTypeID;
					if (SelectedmasterSubCatList.Count > 0) {
						AttribType selecteditem = SelectedAllType.Find (u => u.ProcAttribTypeID == masterSubCatList [0].ProcAttribTypeID && u.Value == masterSubCatList [0].Value);
						if (selecteditem != null) {
							usSCItem1.On = true;
							subcatdesc=masterSubCatList [0].Description;
							SaveAttribTypeList.Add (selecteditem);
						}
						else
							usSCItem1.On = false;
					}

				}
				usSCItem1.ValueChanged += (object sender, EventArgs e) => {

					AttribType additem=new AttribType();
					additem.IsHighLighted=false;
					additem.ProcAttribTypeID= masterSubCatList [0].ProcAttribTypeID;
					additem.ProcID=ProcID;
					additem.Value=masterSubCatList [0].Value;
					subcatdesc=masterSubCatList [0].Description;
					if(usSCItem1.On){	
						usSCItem2.On=false;
						usSCItem3.On=false;
						MainSubCat=additem;
					}else{
						
						usSCItem2.On=false;
						usSCItem3.On=false;
					}
				};
				if (masterSubCatList.Count > 1) {
					ulSCItem2.Text = masterSubCatList [1].Description;
					usSCItem2.Tag = masterSubCatList [1].ProcAttribTypeID;
					if (SelectedmasterSubCatList.Count > 0) {
						AttribType selecteditem = SelectedAllType.Find (u => u.ProcAttribTypeID == masterSubCatList [1].ProcAttribTypeID && u.Value == masterSubCatList [1].Value);
						if (selecteditem != null) {
							usSCItem2.On = true;
							subcatdesc=masterSubCatList [1].Description;
							SaveAttribTypeList.Add (selecteditem);
						}
						else
							usSCItem2.On = false;
					}
					usSCItem2.ValueChanged += (object sender, EventArgs e) => {

						AttribType additem=new AttribType();
						additem.IsHighLighted=false;
						additem.ProcAttribTypeID= masterSubCatList [1].ProcAttribTypeID;
						additem.ProcID=ProcID;
						additem.Value=masterSubCatList [1].Value;
						subcatdesc=masterSubCatList [1].Description;
						if(usSCItem2.On){	
							usSCItem1.On=false;
							usSCItem3.On=false;
							MainSubCat=additem;
						}else{
							usSCItem1.On=false;
							usSCItem3.On=false;
						}
					};

				}
				if (masterSubCatList.Count > 2) {
					ulSCItem3.Text = masterSubCatList [2].Description;
					usSCItem3.Tag = masterSubCatList [2].ProcAttribTypeID;
					if (SelectedmasterSubCatList.Count > 0) {
						AttribType selecteditem = SelectedAllType.Find (u => u.ProcAttribTypeID == masterSubCatList [2].ProcAttribTypeID && u.Value == masterSubCatList [2].Value);
						if (selecteditem != null) {
							subcatdesc=masterSubCatList [2].Description;
							usSCItem3.On = true;
							SaveAttribTypeList.Add (selecteditem);
						}
						else
							usSCItem3.On = false;
					}
					usSCItem3.ValueChanged += (object sender, EventArgs e) => {

						AttribType additem=new AttribType();
						additem.IsHighLighted=false;
						additem.ProcAttribTypeID= masterSubCatList [2].ProcAttribTypeID;
						additem.ProcID=ProcID;
						additem.Value=masterSubCatList [2].Value;
						subcatdesc=masterSubCatList [2].Description;
						if(usSCItem3.On){	
							usSCItem1.On=false;
							usSCItem2.On=false;
							MainSubCat=additem;

						}else{
							usSCItem1.On=false;
							usSCItem2.On=false;
						}
					};
				}
			}
			btnCancel.TouchUpInside+= (object sender, EventArgs e) => {
				DismissViewController(false,null);
			};
			btnok.TouchUpInside += async (object sender, EventArgs e) => {
				List<AttribType> AttribTypelist=new List<AttribType>();
				//removing duplicate records 
				foreach (var item1 in SaveAttribTypeList) {

					var ci=AttribTypelist.Find(u=>u.ProcAttribTypeID==item1.ProcAttribTypeID && u.Value == item1.Value);
					if(ci == null)
					  AttribTypelist.Add(item1);
				}
				//adding uniq records
				SaveAttribTypeList=AttribTypelist;
				btnok.Enabled=false;
				AttribType checkmItem=SaveAttribTypeList.Find(u=>u.ProcAttribTypeID == MainCat.ProcAttribTypeID);
				if(checkmItem != null){
					SaveAttribTypeList.Remove(checkmItem);
					checkmItem.Value="";
					SaveAttribTypeList.Add(checkmItem);
				}

				if(MainCat!=null && MainCat.ProcAttribTypeID != 0)
				  SaveAttribTypeList.Add(MainCat);

				AttribType checkm2Item=SaveAttribTypeList.Find(u=>u.ProcAttribTypeID == MainCat2.ProcAttribTypeID);
				if(checkm2Item != null){
					SaveAttribTypeList.Remove(checkm2Item);
					checkm2Item.Value="";
					SaveAttribTypeList.Add(checkm2Item);
				}
				if(MainCat2!=null && MainCat2.ProcAttribTypeID != 0)
					SaveAttribTypeList.Add(MainCat2);

				AttribType checksItem=SaveAttribTypeList.Find(u=>u.ProcAttribTypeID == MainSubCat.ProcAttribTypeID);
				if(checksItem != null)
				{
					SaveAttribTypeList.Remove(checksItem);
					checksItem.Value="";
					SaveAttribTypeList.Add(checksItem);
				}
				if(MainSubCat!=null && MainSubCat.ProcAttribTypeID != 0)
				{
				  SaveAttribTypeList.Add(MainSubCat);
					buttonDesc=buttonDesc+" ("+subcatdesc+ ")";
				}

//				if(buttonDesc != null)
//					lblDesc.Text=buttonDesc;
				
				SaveAttribTypeList.Sort((xx,yy)=> xx.Value.CompareTo(yy.Value));
				if(SaveAttribTypeList != null && SaveAttribTypeList.Count > 0 ){
					SaveAttribTypeList = SaveAttribTypeList.FindAll(u => u.Value != "");
					var procAttribtsobject  = await AppDelegate.Current.pqrsMgr.UpdateProcAttribs(SaveAttribTypeList);
					if(procAttribtsobject != null && procAttribtsobject.status != null && procAttribtsobject.status.ToUpper() == "OK")
					{
						
						iProPQRSPortableLib.Consts.SelectedProcAttribtslist  = await AppDelegate.Current.pqrsMgr.GetAllAttribTypesOfAProcedure(ProcID);

						int descwidth = 40;
						lblDesc.Lines = 1;
//						int textcount = lblDesc.Text.Length;
//						if (textcount < 58) {
//							descwidth = 40;
//							lblDesc.Lines = 1;
//						} else if (textcount > 58 && textcount <= 114) {
//							descwidth = 55;
//							lblDesc.Lines = 2;
//						} else if (textcount > 114) {
//							descwidth = 75;
//							lblDesc.Lines = 3;
//						}

						lblDesc.Frame = new CoreGraphics.CGRect (500, 8, 480, descwidth);

						var SelectedmasterMainList = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (masterMainList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).ToList ();
						var SelectedmasterMainList2 = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (masterMainList2.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).ToList ();
						var SelectedmasterSubCatList = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (masterSubCatList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).ToList ();
						foreach (var item in SelectedmasterMainList) {
							Option opitem = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.Find (u => u.ProcAttribTypeID == item.ProcAttribTypeID && u.Value == item.Value);
							if (opitem != null) {
								lblDesc.Text = lblDesc.Text + opitem.Description + ", ";
							}
						}
						foreach (var item in SelectedmasterMainList2) {
							Option opitem = iProPQRSPortableLib.Consts.ProcAttribTypes.result.Options.Find (u => u.ProcAttribTypeID == item.ProcAttribTypeID && u.Value == item.Value);
							if (opitem != null) {
								lblDesc.Text = lblDesc.Text + opitem.Description + ", ";
							}
						}
						foreach (var item in SelectedmasterSubCatList) {
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

			};

			//SelectedmasterMainList = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result.FindAll (u => (masterMainList.Select (z => z.ProcAttribTypeID)).Contains (u.ProcAttribTypeID)).ToList ();

			// Perform any additional setup after loading the view, typically from a nib.
		}

		private void checkVal()
		{
		}

		public void PresentFromPopover(UIView sender,float x,float y)
		{
			popover = new UIPopoverController(this)
			{
				PopoverContentSize = new SizeF(400, 590)
			};
			popover.PresentFromRect (new CoreGraphics.CGRect (x, y, 1, 1), sender, UIPopoverArrowDirection.Any, true);
			this.View.Layer.Frame = new CoreGraphics.CGRect (0, 0, 400, 590);
		}
	}
}


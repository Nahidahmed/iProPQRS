
using System;

using Foundation;
using UIKit;
using System.Collections.Generic;
using iProPQRSPortableLib;
using System.Drawing;

namespace iProPQRS
{
	public partial class QualityMetricsASA10 : UIViewController
	{
		public UIPopoverController popover;
		public int ProcID=0;
		public List<iProPQRSPortableLib.Option> masterMainList = new List<iProPQRSPortableLib.Option>();
		public List<iProPQRSPortableLib.Type> masterSubCatList = new List<iProPQRSPortableLib.Type>();
		List<AttribType> SelectedAllType=new List<AttribType>();
		List<AttribType> SaveAttribTypeList=new List<AttribType>();
		string textvalue=string.Empty;
		List<string> SubCattextvalue=new List<string>();
		int SelectedMCid;
		UILabel lblDesc;
		public QualityMetricsASA10 () : base ("QualityMetricsASA10", null)
		{
		}
		public QualityMetricsASA10 (UILabel lblDesc) : base ("QualityMetricsASA10", null)
		{
			this.lblDesc = lblDesc;
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
			if (iProPQRSPortableLib.Consts.SelectedProcAttribtslist != null) {
				SelectedAllType = iProPQRSPortableLib.Consts.SelectedProcAttribtslist.result;
			}
			uvMainCat.Layer.BorderColor=UIColor.Black.CGColor;
			uvMainCat.Layer.BorderWidth = 1;
			usSubCatView.UserInteractionEnabled = false;
			uvSubCatView.Layer.BorderColor=UIColor.Black.CGColor;
			uvSubCatView.Layer.BorderWidth = 1;
			if (masterMainList != null && masterMainList.Count > 0) {
				if (masterMainList.Count > 0) {
					ulMCItem1.Text = masterMainList [0].Description;
					usMCItem1.Tag = masterMainList [0].ProcAttribTypeID;
					AttribType SelectedItem=SelectedAllType.Find(u=>u.ProcAttribTypeID == masterMainList [0].ProcAttribTypeID && u.Value == masterMainList [0].Value);
					if (SelectedItem != null) {
						usMCItem1.On = true;
						SaveAttribTypeList.Add (SelectedItem);	
						textvalue = masterMainList [0].Description;
						usSubCatView.UserInteractionEnabled = false;
					}
					usMCItem1.ValueChanged += (object sender, EventArgs e) => {
						AttribType additem=new AttribType();
						additem.IsHighLighted=false;
						additem.ProcAttribTypeID= masterMainList [0].ProcAttribTypeID;
						additem.ProcID=ProcID;
						additem.Value=masterMainList [0].Value;
						textvalue=masterMainList [0].Description;
						AttribType existingitem= SelectedAllType.Find(u=>u.ProcAttribTypeID == masterMainList [0].ProcAttribTypeID && u.Value == masterMainList [0].Value);
						if(usMCItem1.On)
						{
							SelectedMCid=(int)usMCItem1.Tag;

							usSubCatView.UserInteractionEnabled=false;
							foreach (var switchitem in uslist) {
								if(switchitem !=null)
									switchitem.On=false;
							}
							SubCattextvalue.Clear();
							List<AttribType> chaneitems=new List<AttribType>();

							foreach (var Saveitem in SaveAttribTypeList) {
								Saveitem.Value="";
								chaneitems.Add(Saveitem);
							}

							SaveAttribTypeList=chaneitems;

							if(existingitem == null)
								SaveAttribTypeList.Add (additem);				
							

							usMCItem2.On=false;
						}
						else
						{
							if(existingitem != null)
							{
								existingitem.Value="";
								SaveAttribTypeList.Remove (existingitem);
							}
							textvalue="";
						}
					};
				}
				if (masterMainList.Count > 1) {
					ulMCItem2.Text = masterMainList [1].Description;
					usMCItem2.Tag = masterMainList [1].ProcAttribTypeID;
					AttribType SelectedItem=SelectedAllType.Find(u=>u.ProcAttribTypeID == masterMainList [1].ProcAttribTypeID && u.Value == masterMainList [1].Value);
					if (SelectedItem != null) {
						usMCItem2.On = true;
						SaveAttribTypeList.Add (SelectedItem);	
						textvalue = masterMainList [1].Description;
						usSubCatView.UserInteractionEnabled = true;
					}
					usMCItem2.ValueChanged += (object sender, EventArgs e) => {
						AttribType additem=new AttribType();
						additem.IsHighLighted=false;
						additem.ProcAttribTypeID= masterMainList [1].ProcAttribTypeID;
						additem.ProcID=ProcID;
						additem.Value=masterMainList [1].Value;
						textvalue=masterMainList [1].Description;
						AttribType existingitem= SelectedAllType.Find(u=>u.ProcAttribTypeID == masterMainList [1].ProcAttribTypeID && u.Value == masterMainList [1].Value);
						if(usMCItem2.On)
						{
							SelectedMCid=(int)usMCItem2.Tag;
							usMCItem1.On=false;
							if(existingitem == null)
								SaveAttribTypeList.Add (additem);			

							usSubCatView.UserInteractionEnabled=true;
						}
						else
						{
							if(existingitem != null)
							{
								existingitem.Value="";
								SaveAttribTypeList.Remove (existingitem);
							}
							textvalue="";
						}
					};
				}
			}
			if (masterSubCatList != null && masterSubCatList.Count > 0) {
				foreach (var item in masterSubCatList) {
					AddSubCatItem (item);
				}
			}
			btnCancel.TouchUpInside+= (object sender, EventArgs e) => {
				DismissViewController(false,null);
			};
			brnOk.TouchUpInside += async (object sender, EventArgs e) => {

			
				List<AttribType> AttribTypelist=new List<AttribType>();
				//removing duplicate records 
				foreach (var item1 in SaveAttribTypeList) {

					var ci=AttribTypelist.Find(u=>u.ProcAttribTypeID==item1.ProcAttribTypeID && u.Value == item1.Value);
					if(ci == null)
						AttribTypelist.Add(item1);
					
				}
				//adding uniq records
				//var result = String.Join(", ", SubCattextvalue.ToArray());

				if(SubCattextvalue.Count >0 )
					lblDesc.Text=textvalue +" ("+String.Join(", ", SubCattextvalue.ToArray())+ " )";				   
				else
					lblDesc.Text=textvalue;

				
				SaveAttribTypeList=AttribTypelist;
				SaveAttribTypeList.Sort((xx,yy)=> xx.Value.CompareTo(yy.Value));
				if(SaveAttribTypeList != null && SaveAttribTypeList.Count > 0 ){
					var procAttribtsobject  = await AppDelegate.Current.pqrsMgr.UpdateProcAttribs(SaveAttribTypeList);
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

						DismissViewController(false,null);

					}
				}

			};
			// Perform any additional setup after loading the view, typically from a nib.
		}
		nfloat Ycord=0;
		List<UISwitch> uslist=new List<UISwitch>();
		public void AddSubCatItem(iProPQRSPortableLib.Type item)
		{
			AttribType SelectedItem=SelectedAllType.Find(u=>u.ProcAttribTypeID == item.ProcAttribTypeID );
			UIView uvblock=new UIView(new CoreGraphics.CGRect(0,Ycord,380,60));
			UISwitch usOption=new UISwitch(new CoreGraphics.CGRect(8,10,51,31));
			usOption.Tag = item.ProcAttribTypeID;
			if (SelectedItem != null) {
				usOption.On = true;
				SaveAttribTypeList.Add (SelectedItem);
				SubCattextvalue.Add(item.Label);
			}
			usOption.ValueChanged += (object sender, EventArgs e) => {
				AttribType additem=new AttribType();
				additem.IsHighLighted=false;
				additem.ProcAttribTypeID= item.ProcAttribTypeID;
				additem.ProcID=ProcID;
				additem.Value=item.Label;

				AttribType existingitem= SelectedAllType.Find(u=>u.ProcAttribTypeID == item.ProcAttribTypeID );
				if(usOption.On)
				{
					if(existingitem == null)
						SaveAttribTypeList.Add (additem);

					SubCattextvalue.Add(item.Label);
				}
				else
				{
					SubCattextvalue.Remove(item.Label);
					if(existingitem != null)
					{
						existingitem.Value="";
						SaveAttribTypeList.Remove (existingitem);
						SaveAttribTypeList.Add (existingitem);
					}
					
				}
			};
			UILabel lbldesc=new UILabel(new CoreGraphics.CGRect(78,10,300,50));
			lbldesc.Lines = 2;
			lbldesc.Text = item.Label;
			uslist.Add (usOption);

			UIView uvbreackline=new UIView(new CoreGraphics.CGRect(0,60,380,1));
			uvbreackline.BackgroundColor = UIColor.Gray;

			uvblock.Add (usOption);
			uvblock.Add (lbldesc);
			uvblock.Add (uvbreackline);

			usSubCatView.Add (uvblock);

			Ycord = Ycord + 61;

			usSubCatView.SizeToFit ();
			usSubCatView.ContentSize = new SizeF (float.Parse (usSubCatView.Frame.Width.ToString ()), float.Parse (usSubCatView.Frame.Height.ToString ()) + (float)Ycord-100 );
			
		}
		public void PresentFromPopover(UIView sender,float x,float y)
		{
			popover = new UIPopoverController(this)
			{
				PopoverContentSize = new SizeF(400, 650)
			};
			popover.PresentFromRect (new CoreGraphics.CGRect (x, y, 1, 1), sender, UIPopoverArrowDirection.Any, true);
			this.View.Layer.Frame = new CoreGraphics.CGRect (0, 0, 400, 650);
		}

	}
}


using System;
using UIKit;
using System.Collections.Generic;
using System.Linq;

namespace iProPQRS
{
	public class SubGroupView:UIViewController
	{
		public string SelectedGroup;
		public static RootViewController mainrootview;
		mlsCodePicker pview;
		public List<CodePickerModel> Selecteditems = new List<CodePickerModel> ();
		public SubGroupView (string selectedGroup,mlsCodePicker pview)
		{
			this.SelectedGroup = selectedGroup;	
			this.pview = pview;

		}
		public SubGroupView (string selectedGroup,mlsCodePicker pview,List<CodePickerModel> SubRootData)
		{
			this.SelectedGroup = selectedGroup;	
			this.pview = pview;
			this.DataSource = SubRootData;
		}
		//List<CodePickerModel> DataSource
		public void SetRootview(RootViewController rootview)
		{
			mainrootview = rootview;
		}

		public SubGroupView (string selectedGroup,RootViewController rootview)
		{
			this.SelectedGroup = selectedGroup;	
			mainrootview = rootview;

		}
		public List<CodePickerModel> DataSource = new List<CodePickerModel>();
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			UIButton btnback=new UIButton(new CoreGraphics.CGRect(0,0,100,30));
			UISearchBar searchBar=new UISearchBar(new CoreGraphics.CGRect (0, 32, pview.uvWidth, 40));

			this.Add (searchBar);
			SubGroupViewController agv = new SubGroupViewController (SelectedGroup, pview,DataSource);
			//btnback.BackgroundColor=UIColor.Blue;
			btnback.SetTitle("Done",UIControlState.Normal);
			btnback.SetBackgroundImage (UIImage.FromFile ("back.png"), UIControlState.Normal);
			btnback.SetTitleColor (UIColor.Blue, UIControlState.Normal);
			Selecteditems = pview.SelectedSubItems;

			btnback.TouchUpInside+= (object sender, EventArgs e) => {
				string selecteditem="{1} ({0})";
				string finaltext = string.Empty;
				if(agv.Selecteditems != null)
				{
					if(pview.TypeItemID==406)
					{
						foreach (var sitem in agv.Selecteditems) {
							finaltext=finaltext+sitem.ItemCode +",";
						}
					}
					else
					{
						foreach (var sitem in agv.Selecteditems) {
							finaltext=finaltext+sitem.ItemText +",";
						}
					}
				}
				string headertitle="";
				if(pview.TypeItemID==406)
				{
					headertitle="MAC  ";
					if(!string.IsNullOrEmpty(finaltext))
					{
						finaltext=" ("+finaltext.TrimEnd(',')+")";
					}
					selecteditem =headertitle+ finaltext.TrimEnd(',');//string.Format (selecteditem, finaltext.TrimEnd(','),headertitle);
				}
				if(!string.IsNullOrEmpty(finaltext))
				{
					
					if(pview.TypeItemID==608)
						headertitle="Serious adverse event  ";
					else if(pview.TypeItemID==686 && pview.TypeValue=="0582F")
						headertitle="NOT transferred directly to ICU  ";
					else if(pview.TypeItemID==685 && pview.TypeValue=="B")
						headertitle="Two or more risk factors for PONV  ";
					else if(pview.TypeItemID==687 && pview.TypeValue=="B")
						headertitle="NOT transferred directly to PACU  ";
					else
						headertitle="MAC  ";
					
					selecteditem = string.Format (selecteditem, finaltext.TrimEnd(','),headertitle);
					//selecteditem.  = selecteditem + " , " + itemName;
					CodePickerModel item=null;
					if(pview.TypeItemID==608)
					{
						item = mainrootview.RootData.Where (u => u.ItemID == 608 && u.ItemCode=="B").SingleOrDefault ();

					}
					else if(pview.TypeItemID==686 && pview.TypeValue=="0582F")
					{
						item = mainrootview.RootData.Where (u => u.ItemID == pview.TypeItemID && u.ItemCode ==pview.TypeValue ).SingleOrDefault ();
					}
					else if(pview.TypeItemID==685 && pview.TypeValue=="B")
					{
						item = mainrootview.RootData.Where (u => u.ItemID == pview.TypeItemID && u.ItemCode ==pview.TypeValue ).SingleOrDefault ();
					}
					else if(pview.TypeItemID==687 && pview.TypeValue=="B")
					{
						item = mainrootview.RootData.Where (u => u.ItemID == pview.TypeItemID && u.ItemCode ==pview.TypeValue ).SingleOrDefault ();
					}
					else 
					{
						item = mainrootview.RootData.Where (u => u.ItemID == pview.TypeItemID).SingleOrDefault ();
					}

					mainrootview.RootData.Remove (item);
					item.ItemText = selecteditem;
					mainrootview.RootData.Add (item);
				}
				else if(mainrootview.RootData.Count > 1 && pview.TypeItemID == mainrootview.RootData[1].ItemID && pview.TypeValue ==mainrootview.RootData[1].ItemCode)
				{
					var item = mainrootview.RootData.Where (u => u.ItemID == pview.TypeItemID && u.ItemCode == pview.TypeValue ).ToList();
					if(item != null && item.Count>0)
					{
						mainrootview.RootData.Remove (item[0]);
						if(pview.TypeItemID==686 && pview.TypeValue=="0582F")
							item[0].ItemText="NOT transferred directly to ICU";
						else if(pview.TypeItemID==685 && pview.TypeValue=="B")
							item[0].ItemText="Two or more risk factors for PONV";
						else if(pview.TypeItemID==687 && pview.TypeValue=="B")
							item[0].ItemText="NOT transferred directly to PACU";						

						mainrootview.RootData.Add (item[0]);
					}
				}
				else if( pview.TypeItemID !=686 && pview.TypeValue != "0582F")
				{
					var item = mainrootview.RootData.Where (u => u.ItemID == pview.TypeItemID ).ToList();
					if(item != null && item.Count>0)
					{
						mainrootview.RootData.Remove (item[0]);
						item[0].ItemCode = "MAC";
						mainrootview.RootData.Add (item[0]);
					}
				}



				//Serious adverse event
				if(pview.TypeItemID==608)
				{
					pview.SelectedItems.Clear();
					CodePickerModel msitem=new CodePickerModel();
					msitem.ItemCode="B";
					msitem.ItemID=608;
					msitem.ItemText=selecteditem;
					pview.SelectedItems.Add(msitem); 
				}
				if(pview.TypeItemID==406)
				{
					//pview.SelectedItems.Clear();
					CodePickerModel msitem=new CodePickerModel();
					msitem.ItemCode=null;
					msitem.ItemID=406;
					msitem.ItemText=selecteditem;
					pview.SelectedItems.Remove(pview.SelectedItems.Where(u=>u.ItemID==406).SingleOrDefault());
					pview.SelectedItems.Add(msitem); 
					mainrootview.RootData.Remove(mainrootview.RootData.Find(u=>u.ItemID ==406));
					mainrootview.RootData.Add(msitem);

				}
				mainrootview.RootData.Sort((x,y)=> x.ItemID.CompareTo(y.ItemID));
				mainrootview.TableView.ReloadData();		
				pview.sgv.View.Hidden=true;
				pview.rvc.View.Hidden=false;
			};

			//public List<string> SubGroupData = new List<string> { "Item1", "Item2", "Item3","Item4", "Item5", "Item6" };
			//agv.SubGroupData = DataSource;
			agv.Selecteditems = Selecteditems;
			pview.SelectedSubItems = Selecteditems;
			searchBar.TextChanged += (object sender, UISearchBarTextChangedEventArgs e) => 
			{
				//DataSource.Clear();
				agv.SubGroupData = DataSource.FindAll( u=>u.ItemText != null && u.ItemText.ToLower().Contains(searchBar.Text.ToLower()));
				agv.TableView.ReloadData();

			};
			agv.View.Frame = new CoreGraphics.CGRect (0, 0, pview.uvWidth, pview.uvheight);
			UIView uvconent = new UIView (new CoreGraphics.CGRect (0, 75, pview.uvWidth, pview.uvheight));
			uvconent.Add (agv.View);
			this.View.BackgroundColor = UIColor.White;
			this.View.Add (btnback);
			this.View.Add (uvconent);
		}
	}
}


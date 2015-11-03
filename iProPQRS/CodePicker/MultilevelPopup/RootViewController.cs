using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using Foundation;


namespace iProPQRS
{
	//[MonoTouch.Foundation.Register("RootViewController")]
	public partial class RootViewController : UITableViewController
	{
		public UITableViewCell previouscell = new UITableViewCell ();
		public UIButton prvbtn;
		public static mlsCodePicker pview;
		public RootViewController()
		{
		}

		public List<CodePickerModel>  RootData {
			get;
			set;
		}
		public RootViewController(List<CodePickerModel>  RootData )
		{
			this.RootData = RootData;

		}
		public RootViewController(List<CodePickerModel>  RootData ,mlsCodePicker popoverview)
		{
			
			this.RootData = RootData;
			pview = popoverview;
		}
		//mlsCodePicker
		public override void ViewWillAppear (bool animated)
		{			
			this.TableView.ReloadData ();
			base.ViewWillAppear (animated);
		}
			
		//public List<string> RootData;// = new List<string> { "Group1", "Group2" };
		
		class DataSource : UITableViewDataSource
		{
			SubGroupView sgvc;
			static NSString kCellIdentifier = new NSString ("MyIdentifier");
			RootViewController tvc;
			
			public DataSource (RootViewController tvc)
			{
				this.tvc = tvc;
			}		

			public override nint RowsInSection (UITableView tableView, nint section)
			{
				return tvc.RootData.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell (kCellIdentifier);
				
				if (cell == null)
				{
					cell = new UITableViewCell (UITableViewCellStyle.Default, kCellIdentifier);
				}

			    cell.TextLabel.Text = tvc.RootData.ElementAt(indexPath.Row).ItemText;
				CodePickerModel item= tvc.RootData.ElementAt(indexPath.Row);
				if(pview.TypeValue == item.ItemCode && pview.TypeItemID == item.ItemID )
				{
					UIButton btn = new UIButton (new CoreGraphics.CGRect (0, 0, 70, 37));
					btn.SetTitle ("CODE", UIControlState.Normal);

					List<CodePickerModel> checkbtnitem = null; // RootViewController.pview.SelectedItems.Where (u => u.ItemID == item.ItemID  && u.ItemCode == item.ItemCode).SingleOrDefault ();
					if(pview.SelectedItems.Count > 0 && pview.SelectedItems[0]!=null)
						checkbtnitem = pview.SelectedItems.Where (u => u.ItemID == item.ItemID  && u.ItemCode == item.ItemCode).ToList();
					
					if (checkbtnitem != null && checkbtnitem.Count > 0) {
						btn.SetTitleColor (UIColor.Blue, UIControlState.Normal);
						btn.Layer.BorderColor = UIColor.Blue.CGColor;
						btn.Layer.BorderWidth = 1;
					} else {
						btn.SetTitleColor (UIColor.Gray, UIControlState.Normal);
						btn.Layer.BorderColor = UIColor.Gray.CGColor;
						btn.Layer.BorderWidth = 1;
					}

					//btn.SetImage(UIImage.FromFile("info2.png"),UIControlState.Normal);
					//rightarrow
					btn.TouchUpInside+= (object sender, EventArgs e) => {					
						pview.sgv.View.Hidden=false;
						pview.rvc.View.Hidden=true;
					};
					tvc.prvbtn = btn;
					cell.AccessoryView = btn;// UITableViewCellAccessory.DetailDisclosureButton;
					cell.SetSelected(false,true);
				}

				List<CodePickerModel> checkitem = null;// = RootViewController.pview.SelectedItems.Where (u => u.ItemID == item.ItemID && u.ItemCode == item.ItemCode).SingleOrDefault ();
				if(pview.SelectedItems.Count > 0 && pview.SelectedItems[0] != null)
					checkitem = pview.SelectedItems.Where (u => u.ItemID == item.ItemID && u.ItemCode == item.ItemCode).ToList();
				if (checkitem != null && checkitem.Count > 0) {
					
					cell.Accessory = UITableViewCellAccessory.Checkmark;
					cell.SetSelected (true, false);
					cell.SelectionStyle = UITableViewCellSelectionStyle.Gray;
					cell.Selected = true;
					tvc.previouscell=cell;
				} else {
					//cell.SelectionStyle = UITableViewCellSelectionStyle.None;
					cell.Accessory = UITableViewCellAccessory.None;
					//cell.SetSelected (true, true);
				}
				
				return cell;
			}
		}
		class TableDelegate : UITableViewDelegate
		{
			RootViewController tvc;
			SubGroupViewController sgvc;
			
			public TableDelegate (RootViewController tvc)
			{
				this.tvc = tvc;
			}
			
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell selectedCell=tableView.CellAt (indexPath);
				string selectedGroup = tvc.RootData.ElementAt(indexPath.Row).ItemText;
				var item = tvc.RootData.ElementAt (indexPath.Row);
				if (!pview.isMainCatMultiSelect) {
					pview.SelectedItems.Clear ();
					pview.SelectedItems.Add(item);

					if(tvc.previouscell != null && tvc.previouscell != selectedCell)
						tvc.previouscell .Accessory = UITableViewCellAccessory.None;
					
					selectedCell.Accessory=UITableViewCellAccessory.Checkmark;

				}
				else {
					
					var checkitem = pview.SelectedItems.Where (s => s.ItemID == item.ItemID && s.ItemCode == item.ItemCode).SingleOrDefault ();
					if (checkitem == null) {
						pview.SelectedItems.Add (item);
						selectedCell.Accessory=UITableViewCellAccessory.Checkmark;
						selectedCell.SelectionStyle = UITableViewCellSelectionStyle.Gray;
					} else {
						selectedCell.Accessory = UITableViewCellAccessory.None;
						selectedCell.SetSelected (false, true);
						pview.SelectedItems.Remove (pview.SelectedItems.Where(r=>r.ItemID==checkitem.ItemID).SingleOrDefault());
						if (pview.TypeValue == item.ItemCode && pview.TypeItemID == item.ItemID) {

							if (tvc != null && tvc.prvbtn != null) {
								tvc.prvbtn.SetTitleColor (UIColor.Gray, UIControlState.Normal);
								tvc.prvbtn.Layer.BorderColor = UIColor.Gray.CGColor;
								tvc.prvbtn.Layer.BorderWidth = 1;
							}
						}
					}
				}

				//var checkitem = RootViewController.pview.SelectedItems.Where (s => s.ItemID == item.ItemID && s.ItemCode == item.ItemCode).SingleOrDefault ();
				//if(checkitem ==null)
				   //RootViewController.pview.SelectedItems.Add(item);
				//else
					//RootViewController.pview.SelectedItems.Remove(item);
				//"0581F"

			//	UITableViewCell selectedCell=tableView.CellAt (indexPath);

				//if(sgvc == null)
				//	sgvc = new SubGroupViewController(selectedGroup,tvc);
				
				//tvc.NavigationController.PushViewController(sgvc,true);
			}
			public override void RowDeselected (UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell selectedCell=tableView.CellAt (indexPath);

				var item = tvc.RootData.ElementAt (indexPath.Row);
				pview.SelectedSubItems.Clear ();
				var removeitem = pview.SelectedItems.Where (s => s.ItemID == item.ItemID && s.ItemCode == item.ItemCode).ToList();
				if (removeitem != null && removeitem.Count > 0) {
					pview.SelectedItems.Remove (removeitem [0]);
					selectedCell.Accessory=UITableViewCellAccessory.None;
					selectedCell.SetSelected (false, true);
				}
				if(!pview.isMultiSelect)
				  pview.SelectedSubItems.Clear ();
			}
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.TableView.Frame = new CoreGraphics.CGRect (0, 0, pview.uvWidth, 800);
			this.TableView.AllowsMultipleSelection = pview.isMainCatMultiSelect;
			TableView.Delegate = new TableDelegate (this);
			TableView.DataSource = new DataSource (this);
		}

			
	}
}
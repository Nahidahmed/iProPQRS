using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using Foundation;


namespace iProPQRS
{
	//[MonoTouch.Foundation.Register("SubGroupViewController")]
	public partial class SubGroupViewController : UITableViewController
	{
		public List<CodePickerModel> SubGroupData = new List<CodePickerModel>();
		public string SelectedGroup;
		UITableViewCell tempcell;
		public static RootViewController mainrootview;
		public static mlsCodePicker pview;
		public List<CodePickerModel> Selecteditems = new List<CodePickerModel> ();
		public UITableViewCell previouscell = new UITableViewCell ();
		public SubGroupViewController (string selectedGroup) : base ()
		{
			
			this.TableView.AllowsMultipleSelection = true;
			this.SelectedGroup = selectedGroup;	
		}

		public SubGroupViewController (string selectedGroup,RootViewController rootview,mlsCodePicker popoverview) : base ()
		{
			this.TableView.AllowsMultipleSelection = true;
		//	this.SelectedGroup = selectedGroup;	
			mainrootview = rootview;
			pview = popoverview;
		}
		public SubGroupViewController (string selectedGroup,mlsCodePicker popoverview,List<CodePickerModel> SubGroupData) : base ()
		{
			
			//this.SelectedGroup = selectedGroup;	
			this.SubGroupData=SubGroupData;
			//mainrootview = rootview;
			pview = popoverview;

		}
		public SubGroupViewController (string selectedGroup,RootViewController rootview) : base ()
		{
			this.TableView.AllowsMultipleSelection = true;
			this.SelectedGroup = selectedGroup;	
			//mainrootview = rootview;

		}
		class DataSource : UITableViewDataSource
		{
			SubGroupViewController tvc;

			static NSString kCellIdentifier = new NSString ("MyIdentifier");
	        
			public DataSource (SubGroupViewController tvc)
			{
				this.tvc = tvc;


			}			
			public override nint RowsInSection (UITableView tableView, nint section)
			{
				//return tvc.SubGroupData.Count;
				return tvc.indexedtableitems [tvc.keys [section]].Count;
			}
			public override string TitleForHeader (UITableView tableView, nint section)
			{
				return tvc.keys [section];
			}
			public override nint NumberOfSections (UITableView tableView)
			{
				// TODO: return the actual number of sections
				return tvc.keys.Length;

			}
			public override string[] SectionIndexTitles (UITableView tableView)
			{
				
				return tvc.indexedtableitems.Keys.OrderBy (o => o.ToString ()).ToArray ();

			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				
				var cell = tableView.DequeueReusableCell (kCellIdentifier);
				if (cell == null)
				{
					cell = new UITableViewCell (UITableViewCellStyle.Default, kCellIdentifier);
				}
				cell.Selected = false;
				//CodePickerModel item = tvc.SubGroupData.ElementAt (indexPath.Row);

				CodePickerModel item = null;
				try{
					item =  tvc.indexedtableitems [tvc.keys [indexPath.Section]] [indexPath.Row];
					cell.TextLabel.Text = tvc.SubGroupData.ElementAt(indexPath.Row).ItemText;
				}catch(Exception ex){
					Console.WriteLine ("SubgroupViewController GetCell: ex: "+ ex.Message);
				}


				if(cell.TextLabel.Text.Length > 64){
				cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
					cell.TextLabel.Lines = 2;
					cell.Frame = new CoreGraphics.CGRect (cell.Frame.X, cell.Frame.Y, cell.Frame.Width, cell.Frame.Height + (cell.Frame.Height));
				}
				List<CodePickerModel> checkitem = null;

				if (item != null) {
					if (tvc.Selecteditems != null && pview.TypeItemID == 406) {
						checkitem = tvc.Selecteditems.Where (u => u.ItemCode == item.ItemCode).ToList ();			
					} else if (tvc.Selecteditems != null) {
						checkitem = tvc.Selecteditems.Where (u => u.ItemID == item.ItemID).ToList ();
					}
				

					if (checkitem != null && checkitem.Count > 0 && tvc.Selecteditems.Contains (checkitem [0]) && checkitem [0].ItemCode == item.ItemCode) {
						cell.Accessory = UITableViewCellAccessory.Checkmark;
						cell.Selected = true;
						cell.SetHighlighted (true, true);
						cell.SetSelected (true, true);
						///cell.SelectionStyle = UITableViewCellSelectionStyle.Gray;
						tvc.previouscell = cell;
					} else {
						cell.Accessory = UITableViewCellAccessory.None;
					}
				} else {
					cell.Accessory = UITableViewCellAccessory.None;
				}

				return cell;
			}
		}
	
		public class TableDelegate : UITableViewDelegate
		{
			public List<string> RootData = new List<string> { "Group1", "Group2" };
			SubGroupViewController tvc;
			RootViewController idvc;
		
			public TableDelegate (SubGroupViewController tvc)
			{
				
				this.tvc = tvc;
			}
			public override void RowDeselected (UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell selectedCell=tableView.CellAt (indexPath);
				selectedCell.Accessory = UITableViewCellAccessory.None;
				//var itemName = tvc.SubGroupData.ElementAt(indexPath.Row);	
				CodePickerModel itemName =  tvc.indexedtableitems [tvc.keys [indexPath.Section]] [indexPath.Row];
				var ritem = tvc.Selecteditems.Where (r => r.ItemID == itemName.ItemID && r.ItemCode == itemName.ItemCode).ToList ();
				if(ritem.Count > 0)
					tvc.Selecteditems.Remove (ritem[0]);
			}
			public override nfloat GetHeightForHeader (UITableView tableView, nint section)
			{
				return 32.0f;
			} 
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell selectedCell=tableView.CellAt (indexPath);
				//selectedCell.Accessory = UITableViewCellAccessory.Checkmark;


				//var itemName = tvc.SubGroupData.ElementAt(indexPath.Row);	
				CodePickerModel itemName =  tvc.indexedtableitems [tvc.keys [indexPath.Section]] [indexPath.Row];
				if (pview.TypeItemID == 406) {
					if (tvc.Selecteditems != null) {
						var checkitem406 = tvc.Selecteditems.Where (u => u.ItemCode == itemName.ItemCode).ToList ();
						if (checkitem406.Count == 0) {
							tvc.Selecteditems.Add (itemName); 
							selectedCell.Accessory = UITableViewCellAccessory.Checkmark;
							selectedCell.SelectionStyle = UITableViewCellSelectionStyle.Gray;
						} else {
							selectedCell.Accessory = UITableViewCellAccessory.None;
							selectedCell.SetSelected (false, true);
							foreach (var existingitem in checkitem406) {
								tvc.Selecteditems.Remove (existingitem);
							}

						}
					}
				}
				else if (pview.TypeItemID == 608) {
					var checkitem608 = tvc.Selecteditems.Where (u => u.ItemID == itemName.ItemID && u.ItemCode == itemName.ItemCode).ToList ();
					if (checkitem608.Count == 0) {
						tvc.Selecteditems.Add (itemName); 

						selectedCell.Accessory = UITableViewCellAccessory.Checkmark;
					}
					else {
						selectedCell.Accessory = UITableViewCellAccessory.None;
						selectedCell.SetSelected (false, true);
						foreach (var existingitem in checkitem608) {
							tvc.Selecteditems.Remove (existingitem);
						}

					}
					
				}else {
					if (tvc.Selecteditems != null) {
						var checkitem = tvc.Selecteditems.Where (u => u.ItemID == itemName.ItemID && u.ItemCode == itemName.ItemCode).ToList ();
						if (checkitem.Count == 0) {
							tvc.Selecteditems.Add (itemName); 
							if(tvc.previouscell != null)
								tvc.previouscell .Accessory = UITableViewCellAccessory.None;

							selectedCell.Accessory = UITableViewCellAccessory.Checkmark;
							selectedCell.Selected = true;
						}
						else {
							selectedCell.Accessory = UITableViewCellAccessory.None;

							foreach (var existingitem in checkitem) {
								tvc.Selecteditems.Remove (existingitem);
							}
						}
					}
				}


			//	SubGroupViewController.pview.Add(tvc.View);
				//SubGroupViewController.mainrootview.RootData = RootData;
				//idvc = new RootViewController(RootData);
			//	tvc.NavigationController.PushViewController(idvc, true);
			}
		}
		public Dictionary<string, List<CodePickerModel>> indexedtableitems; 
		public string[] keys;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.TableView.AllowsMultipleSelection = pview.isMultiSelect;
			this.Title = SelectedGroup; //SelectedGroup set via ctor
			indexedtableitems = new Dictionary<string,List<CodePickerModel>> ();
			foreach (var t in SubGroupData) {
				if (indexedtableitems.ContainsKey (t.ItemText [0].ToString ().ToUpper())) {
					indexedtableitems [t.ItemText [0].ToString ().ToUpper()].Add (t);
				} else {
					indexedtableitems.Add (t.ItemText [0].ToString ().ToUpper(), new List<CodePickerModel> (){ t });
				}
			}

			keys = indexedtableitems.Keys.OrderBy (o=>o.ToString()).ToArray();
			TableView.Frame = new CoreGraphics.CGRect (0, 40, pview.uvWidth, pview.uvheight);
			TableView.Delegate = new TableDelegate (this);
			TableView.DataSource = new DataSource (this);
		}
			
	}
}
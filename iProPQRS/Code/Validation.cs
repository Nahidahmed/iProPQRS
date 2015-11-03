using System;
using UIKit;

namespace iProPQRS
{
	public class Validation
	{
		static bool CheckValidate;
		public Validation ()
		{
		}
	}

	public class ValidateTextFieldDelegate : UITextFieldDelegate
	{
		int Maxvalue;
		string uTitle;
		public ValidateTextFieldDelegate(int maxval,string title)
		{
			Maxvalue = maxval;
			uTitle = title;
		} 
		public override bool ShouldBeginEditing(UITextField textField)
		{
			textField.Placeholder = uTitle;
			return true;
		}
		public override bool ShouldReturn(UITextField textField)
		{
			textField.ResignFirstResponder();
			return true;
		}
		public override bool ShouldEndEditing(UITextField textField)
		{
			if (textField.Text.Length > Maxvalue)  {    				
				UIAlertView alert = new UIAlertView("Invalid "+uTitle, "Please enter below "+Maxvalue+" letter "+uTitle+" .", null, "OK", null);
				alert.Show();
				return false;
			}
			return true;
		}
	}
	public class KeyBoardHideTextFieldDelegate:UITextFieldDelegate
	{
		public override bool ShouldReturn(UITextField textField)
		{
			textField.ResignFirstResponder();
			return true;
		}
	}

}


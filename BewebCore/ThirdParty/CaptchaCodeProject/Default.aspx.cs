using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace CaptchaImage
{
	public class _Default : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox CodeNumberTextBox;
		protected System.Web.UI.WebControls.Button SubmitButton;
		protected System.Web.UI.WebControls.Label MessageLabel;

		// For generating random numbers.
		private Random random = new Random();
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.IsPostBack)

				// Create a random code and store it in the Session object.
				this.Session["CaptchaImageText"] = GenerateRandomCode();

			else
			{
				// On a postback, check the user input.
				if (this.CodeNumberTextBox.Text == this.Session["CaptchaImageText"].ToString())
				{
					// Display an informational message.
					this.MessageLabel.CssClass = "info";
					this.MessageLabel.Text = "Correct!";
				}
				else
				{
					// Display an error message.
					this.MessageLabel.CssClass = "error";
					this.MessageLabel.Text = "ERROR: Incorrect, try again.";

					// Clear the input and create a new random code.
					this.CodeNumberTextBox.Text = "";
					this.Session["CaptchaImageText"] = GenerateRandomCode();
				}
			}
		}

		//
		// Returns a string of six random digits.
		//
		private string GenerateRandomCode()
		{
			string s = "";
			for (int i = 0; i < 6; i++)
				s = String.Concat(s, this.random.Next(10).ToString());
			return s;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}

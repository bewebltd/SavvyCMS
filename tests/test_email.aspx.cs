using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class tests_test_email : System.Web.UI.Page {
	protected void Page_Load(object sender, EventArgs e) {
		string toAddress = "jeremy@beweb.co.nz"; // ConfigurationManager.AppSettings.Get("EmailAboutError")
		string fromAddress = ConfigurationManager.AppSettings.Get("EmailFromAddress" + Beweb.Util.ServerIs());
		string emailHost = ConfigurationManager.AppSettings.Get("EmailHost" + Beweb.Util.ServerIs());

		string message = String.Format(@"

This is a test email sent at: {0}

These are the normal error email settings on this server:

From: {1}
To: {2}
Server: {3}

If you still aren't getting normal emails, try checking yours and the to address's spam settings. That's caught me out a few times.

"
			, DateTime.Now.ToString("dd MMM yyyy HH:mm:ss")
			, fromAddress
			, toAddress
			, emailHost
			);

		ResultMessage.Text = message + "<br/><br/>Mail Send Result: [" + Beweb.Error.SendErrorEmail(toAddress, "Test Email", message) + "]";
	}
}
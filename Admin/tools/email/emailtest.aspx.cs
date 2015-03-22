using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class emailtest : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (IsPostBack && !String.IsNullOrEmpty(ToEmailAddress.Text))
		{
			RunTests(ToEmailAddress.Text);
			ShowResults.Visible = true;
		}

	}

	protected void RunTests(string toEmail)
	{
		runtime.Text = String.Format("{0:dd MMM yyyy HH:mm:ss}", DateTime.Now);

		// [Y] = it has worked at some point
		// [N] = it never has

		testResults.Text += SendTestMail(1, "127.0.0.1", 25, "etc@etcglobal.co.nz", toEmail, "", "", "[Y] openhost domain - email elsewhere" );

		testResults.Text += SendTestMail(2, "127.0.0.1", 25, "blah@dnserver.net.nz", toEmail, "", "", "[Y] openhost's domain");

		testResults.Text += SendTestMail(3, "127.0.0.1", 25, "blah@surtees.co.nz", toEmail, "", "", "[Y] openhost's domain - with openhost email - email address does not exist");

		testResults.Text += SendTestMail(4, "127.0.0.1", 25, "blah@microsoft.com", toEmail, "", "", "[Y] someone else's domain - email address probably does not exist");

		testResults.Text += SendTestMail(5, "127.0.0.1", 25, "blah@aksjdbfkjhkjhasdkfhasdkfhakhguibvhukanskudnfkansdf.co.nz", toEmail, "", "", "[N] non-existant domain");
		
		// credentials
		testResults.Text += SendTestMail(6, "127.0.0.1", 25, "blah@microsoft.com", toEmail, "test@surtees.co.nz", "jflem", "[Y] domain hosted elsewhere, using existing mail credentials (an existing domain name)");

		testResults.Text += SendTestMail(7, "127.0.0.1", 25, "test@surtees.co.nz", toEmail, "test@surtees.co.nz", "jflem", "[Y] domain hosted elsewhere, using existing mail credentials (an existing domain name)");
		// ---

		testResults.Text += SendTestMail(8, "119.47.118.31", 25, "blah@dnserver.net.nz", toEmail, "", "", "[N] openhost's domain - different server");

		testResults.Text += SendTestMail(9, "119.47.118.31", 25, "blah@microsoft.com", toEmail, "", "", "[N] machine IP address - domain hosted elsewhere");

		// credentials
		testResults.Text += SendTestMail(10, "119.47.118.31", 25, "blah@microsoft.com", toEmail, "blah@queenstdental.co.nz", "asdf1", "[N] machine IP address, domain hosted elsewhere, using existing mail credentials");

		testResults.Text += SendTestMail(11, "mail.surtees.co.nz", 25, "blah@microsoft.com", toEmail, "test@surtees.co.nz", "jflem", "[N] domain hosted elsewhere, using existing mail credentials (an existing domain name) (119.47.118.31 = mail.surtees.co.nz)");
		// ---

		testResults.Text += SendTestMail(12, "mail.aksjdbfkjhkjhasdkfhasdkfhakhguibvhukanskudnfkansdf.co.nz", 25, "blah@dnserver.net.nz", toEmail, "", "", "[N] openhost's domain - non-existant server");
		
	}


	protected string SendTestMail(int testNumber, string mailServer, int mailPort, string mailFrom, string mailTo, string mailUser, string mailPassword, string description)
	{
		string returnVal = String.Format("Test {0} has been run. {1}.<br />", testNumber, description);

		string mailTime = String.Format("{0:dd MMM yyyy HH:mm:ss}", DateTime.Now);

		MailMessage message = new MailMessage();
		message.From = new MailAddress(mailFrom);
		message.To.Add(new MailAddress(mailTo));
		message.Subject = "Email Test " + testNumber + ": " + mailTime;
		string mailDetails = String.Format(@"
Server: {0}
Port: {1}
From: {2}
To: {3}
"
			, mailServer
			, mailPort
			, mailFrom
			, mailTo
			);
		message.Body = mailDetails;
		returnVal += mailDetails;

		SmtpClient client = new SmtpClient(mailServer, mailPort);
		if(!String.IsNullOrEmpty(mailUser))
		{
			client.UseDefaultCredentials = false;
			System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(mailUser, mailPassword);
			client.Credentials = SMTPUserInfo;

			returnVal += " User: " + mailUser + " Password: " + mailPassword;
		}

		string errorMessage = "Success!";
		try
		{
			client.Send(message);
		}
		catch (Exception e)
		{
			errorMessage = " Error: " + e.Message;
		}
		return returnVal + "<br />" + errorMessage + "<br /><br />";
	}
}

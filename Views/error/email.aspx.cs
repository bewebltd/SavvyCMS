using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class error_email : System.Web.Mvc.ViewPage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string to = Request.QueryString["to"];
		if (String.IsNullOrEmpty(to)) to = ConfigurationManager.AppSettings.Get("WebmasterAddress");
		if (String.IsNullOrEmpty(to)) to = ConfigurationManager.AppSettings.Get("EmailAboutError");

		string emailResult = SendErrorEmail(
			to,
			"Email test page from " + Request.Url.Host,
			"This is a test email - emails are working properly if you get this.");
		if(String.IsNullOrEmpty(emailResult))
		{
			EmailResult.Text = "Email sending is working fine";
		}
		else
		{
			EmailResult.Text = String.Format("Email sending error: [{0}]", emailResult);
		}
	}
	
	public static string SendErrorEmail(string to, string subject, string body)
	{
		string returnValue = "";
		if (!String.IsNullOrEmpty(to)) {
			//defaults
			string serverType = Beweb.Util.ServerIs();
			string from = ConfigurationManager.AppSettings.Get("EmailFromAddress") ?? "website@beweb.co.nz";
			string host = ConfigurationManager.AppSettings.Get("EmailHost" + serverType) ?? "local"; // local will possibly not work either 
			int port = Convert.ToInt32(ConfigurationManager.AppSettings.Get("EmailPort" + serverType));
			port = (port == 0) ? 25 : port;

			MailMessage message = new MailMessage();
			message.From = new MailAddress(from);
			message.To.Add(new MailAddress(to));
			message.Subject = subject;
			message.Body = body;

			SmtpClient client = new SmtpClient(host, port);

			try {
				client.Send(message);
			}
			catch (SmtpException e) {
				if (e.InnerException != null)
				{
					returnValue = e.InnerException.Message;
				}
			}

		}
		return returnValue;
	}
}

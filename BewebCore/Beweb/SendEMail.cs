#define ActiveRecord
// Mail system
using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.ComponentModel;

namespace Beweb {
	/// <summary>
	/// Class to send emails
	/// </summary>
	public class SendEMail {
		protected bool mailSent = false;

		/// <summary>
		/// get the error text from the last failure of send
		/// </summary>
		public string errorResult = "";

		public static string EmailToAddress { get { return Util.GetSetting("EmailToAddress", "throw"); } }
		public static string EmailFromAddress { get { return Util.GetSetting("EmailFromAddress", "throw"); } }
		public static string EmailFromName { get { return Util.GetSetting("SiteName", "throw"); } }
		public static string EmailAboutError { get { return Util.GetSetting("EmailAboutError", "throw"); } }
		public static string EmailOverrideAddress { get { return Util.GetSetting("EmailOverrideAddress", ""); } }	  // override email address for dev server
		public static bool IsEmailOverride { get { return EmailOverrideAddress.IsNotBlank(); } }  // true if on dev server with email address override in place
		public static string EmailHost { get { return Util.GetSetting("EmailHost", "throw"); } }
		public static int EmailPort { get { return Convert.ToInt32(Util.GetSetting("EmailPort", "25")); } }

		public SendEMail() {
		}

		public static string GetButtonHtml(string buttonTextHtml, string buttonUrl, string buttonColor, string textColor, int fontSize) {
				string styleButton = "display:block;display:inline-block;padding:10px 20px;border-radius:4px;border:1px solid #666;color:"+textColor+";background:"+buttonColor+";font-size:"+fontSize+"px;text-decoration:none;font-weight:bold;margin:10px 0;text-align:center;box-shadow: 3px 3px 5px 0px rgba(0,0,0,0.2);";
			return "<div><a href=\""+buttonUrl+"\" target=\"_blank\" style=\""+styleButton+"\">"+buttonTextHtml+" &rarr;</a></div>";
		}

		/// <summary>
		/// a simple html emailing function that does throw an error if unsuccessful
		/// </summary>
		/// <param name="to">recipient</param>
		/// <param name="subject">subject of email</param>
		/// <param name="body">html body text</param>
		public static void SimpleSendHTMLEmail(string to, string subject, string body) {
			SimpleSendEmail(to, subject, body, true);
		}
		/// <summary>
		/// a simple plain text emailing function that does throw an error if unsuccessful
		/// </summary>
		/// <param name="to">recipient</param>
		/// <param name="subject">subject of email</param>
		/// <param name="body">plain text body text</param>
		public static void SimpleSendEmail(string to, string subject, string body) {
			SimpleSendEmail(to, subject, body, false);
		}
		/// <summary>
		/// a simple emailing function that does throw an error if unsuccessful
		/// </summary>
		/// <param name="to">recipient</param>
		/// <param name="subject">subject of email</param>
		/// <param name="body">body text</param>
		/// <param name="IsBodyHtml">if body has html, set to true</param>
		public static void SimpleSendEmail(string to, string subject, string body, bool IsBodyHtml) {
			SimpleSendEmail(to, subject, body, IsBodyHtml, new MailAddress(Util.GetNamedSetting("EmailFromAddress", "throw"), Util.GetNamedSetting("SiteName", "throw")));
		}
		/// <summary>
		/// a simple emailing function that does throw an error if unsuccessful
		/// </summary>
		/// <param name="to">recipient</param>
		/// <param name="subject">subject of email</param>
		/// <param name="body">body text</param>
		/// <param name="IsBodyHtml">if body has html, set to true</param>

		public static void SimpleSendEmail(string to, string subject, string body, bool IsBodyHtml, string from) {

			if (from.IsNotBlank() && from.DoesntContain("@")) { // AF20150128: It's probably just the From Name
				from = from + "<" + Util.GetSetting("EmailFromAddress", "") + ">";
			}

			SimpleSendEmail(to, subject, body, IsBodyHtml, new MailAddress(from));
		}

		public static void SimpleSendEmail(string to, string subject, string body, bool IsBodyHtml, MailAddress from) {
			if (!String.IsNullOrEmpty(to)) {
				MailMessage message = new MailMessage();
				if (from == null) {
					message.From = new MailAddress(Util.GetNamedSetting("EmailFromAddress", "throw"), Util.GetNamedSetting("SiteName", "throw"));
				} else {
					message.From = from;
				}
				to = to.Replace(",", ";");
				string[] toAddresses = to.Split(';');
				foreach (string address in toAddresses) {
					if (address.IsNotBlank()) {
						message.To.Add(new MailAddress(address));
					}
				}
				// override email address for dev server
				if (IsEmailOverride) {
					message.To.Clear();
					message.To.Add(new MailAddress(EmailOverrideAddress));
				}
				message.Subject = subject;
				message.Body = body;
				message.IsBodyHtml = IsBodyHtml;

				SmtpClient client = GetServerSmtpClient();

				try {
					client.Send(message);
					if (Util.GetSettingBool("WriteEmailSendToDLogFile", false)) {
						Logging.dlog("Sent mail subject [" + subject + "]: from [" + from + "] to [" + message.To.Join(",") + "]");
					}
				} catch (Exception e) {
					throw new Exception("Send mail failed. Host[" + client.Host + "] from[" + message.From + "]to[" + to + "] [" + e.Message + "]");
				}
			} else {
				throw new Exception("Send mail failed. Email to address blank");
			}
		}

		/// <summary>
		/// Send an html email from the default from address. 
		/// StripTags is run on html to get an alternative plain text version.
		/// If ok, return null
		/// </summary>
		/// <param name="to"></param>
		/// <param name="subject"></param>
		/// <param name="htmlBody"></param>
		public static string SimpleSendHtmlEmail(string to, string subject, string htmlBody) {
			return SimpleSendHtmlEmail(to, subject, htmlBody, null);
		}

		/// <summary>
		/// Send an html email from the default from address. 
		/// StripTags is run on html to get an alternative plain text version.
		/// If ok, return null
		/// </summary>
		/// <param name="to"></param>
		/// <param name="subject"></param>
		/// <param name="htmlBody"></param>
		/// <param name="attachments">files will full path, sep by ;</param>
		public static string SimpleSendHtmlEmail(string to, string subject, string htmlBody, string attachments) {
			string result = null;
			if (!String.IsNullOrEmpty(to)) {
				var envelope = new SendEMail();
				string rawbody = htmlBody;
				string body = "";
				if (!rawbody.Contains("<style")) {
					string styles = "<style type='text/css'>";
					styles += "p, ol li{margin:0;padding:0;font-family:Arial, Helvetica, sans-serif;font-size:12px;color:#333333;}";
					styles += "h2, h3, h4, h5, h6 {margin:0;padding:0;font-family:Arial, Helvetica, sans-serif;color:#1e3444;}";
					styles += "h2 {font-size:14px;}";
					styles += "h3 {font-size:13px;}";
					styles += "h4 {font-size:12px;}";
					styles += "h5 {font-size:15px;}";
					styles += "h6 {font-size:11px;color:#666;}";
					styles += "a {color:#0097d0;text-decoration:none;}";
					styles += "table td {font-family:Arial, Helvetica, sans-serif;font-size:12px;color:#333333;}";
					styles += "</style>";
					body = styles;
				}
				// fix relative links from tinyMCE - may be able to remove this later
				if (Util.ServerIs() == "LVE" || Util.ServerIs() == "STG") {
					// you must use assign to a new variable when using replace. else use Regex.Replace(string, searchText, replaceText)
					body += rawbody.Replace("../../..", Web.BaseUrl);
				} else {
					// it's DEV
					//to = Util.GetNamedSetting("EmailToAddress"); // MN 20110511 this overrides all emails sent from DEV server, which can be annoying - uncomment this temporarily if desired
					body += rawbody.Replace("../../../../", Web.BaseUrl);
				}
				if (!envelope.SendEmail(subject, body, Fmt.StripTags(body), "html", to, to, EmailFromAddress, EmailFromName, attachments)) {
					result = envelope.errorResult;
				}

			} else {
				throw new Exception("Send mail failed. Email 'to' address blank");
			}
			return result;
		}

		/// <summary>
		/// Returns a plain text version of the HTML body, suitable for sending in an email.
		/// Note: this is a bit slow if doing hundreds of times, eg sending a newsletter, so don't do it within the loop
		/// TODO - needs to add links etc (as per classic ASP version)
		/// </summary>
		/// <param name="htmlBody"></param>
		public static string GeneratePlainTextVersion(string htmlBody) {
			return Fmt.StripTagsComplete(htmlBody);
		}

		/// <summary>
		/// called by SimpleSendEmail to get client and auth login (from settings in web.config)
		/// </summary>
		/// <returns></returns>
		public static SmtpClient GetServerSmtpClient() {
			string serverType = Util.ServerIs();
			string host = ConfigurationManager.AppSettings.Get("EmailHost" + serverType);
			int port = Convert.ToInt32(ConfigurationManager.AppSettings.Get("EmailPort" + serverType));
			SmtpClient client = new SmtpClient(host, port);
			string authuser = ConfigurationManager.AppSettings.Get("EmailAuthUser" + serverType);
			if (authuser != "") {
				string authpass = ConfigurationManager.AppSettings.Get("EmailAuthPassword" + serverType);
				// Using authentication to connect to above smtp server to send email. 
				System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(authuser, authpass);
				client.UseDefaultCredentials = false;
				client.Credentials = SMTPUserInfo;
			}
			return client;
		}

		[Obsolete]
		public bool SendEmail(string subject, string msg, string messagePlainText, string options, string toEmail, string toName, string fromEmail, string fromName) {
			return SendEmail(subject, msg, messagePlainText, options, toEmail, toName, fromEmail, fromName, "");
		}

		/// <summary>
		/// Send a Message
		/// </summary>
		/// <param name="subject">subject</param>
		/// <param name="msg">message</param>
		/// <param name="messagePlainText"></param>
		/// <param name="options">send string with html in it for html message, else plain</param>
		/// <param name="toEmail"></param>
		/// <param name="toName"></param>
		/// <param name="fromEmail"></param>
		/// <param name="fromName"></param>
		/// <param name="attachments"></param>
		/// <returns></returns>
		[Obsolete]
		public bool SendEmail(string subject, string msg, string messagePlainText, string options, string toEmail, string toName, string fromEmail, string fromName, string attachments) {
			return SendEmail(subject, msg, messagePlainText, options, toEmail, toEmail, fromEmail, fromName, attachments, fromEmail);
		}
		[Obsolete]
		public bool SendEmail(string subject, string msg, string messagePlainText, string options, string toEmail, string toName, string fromEmail, string fromName, string attachments, string replyToEmail) {
			bool result = true;

			msg = VB.replace(msg, "@", "&#64;");// encode any email addresses to fool spiders - 20110404 JN Added

			MailMessage message = new System.Net.Mail.MailMessage();
			// Set destinations for the e-mail message.
			if (toEmail.Contains(";")) {
				string[] toAddresses = toEmail.Split(';');
				foreach (string address in toAddresses) {
					message.To.Add(new MailAddress(address));
				}
			} else {
				MailAddress to = new MailAddress(toEmail, toName);
				message.To.Add(to);
			}
			// override email address for dev server
			if (IsEmailOverride) {
				message.To.Clear();
				message.To.Add(new MailAddress(EmailOverrideAddress));
			}

			// Specify the e-mail sender.
			// Create a mailing address that includes a UTF8 character
			// in the display name.
			MailAddress from = new MailAddress(fromEmail, fromName, System.Text.Encoding.UTF8);
			message.From = from;
			// Specify the message content.
#if DOTNET4
			message.ReplyToList.Add(new MailAddress(replyToEmail));
#else
			message.ReplyTo = new MailAddress(replyToEmail);
#endif
			message.Body = msg; //"This is a test e-mail message sent by an application. ";
			// Include some non-ASCII characters in body and subject.
			string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
			//message.Body += Environment.NewLine + someArrows;
			message.BodyEncoding = System.Text.Encoding.UTF8;
			message.Subject = subject;//"test message 1" + someArrows;
			message.SubjectEncoding = System.Text.Encoding.UTF8;
			// Set the method that is called back when the send operation ends.
			//client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

			if (options.ToLower().IndexOf("html") != -1) {
				message.IsBodyHtml = true;
			}
			if (messagePlainText.IsNotBlank()) {
				//if same, create a non-html version of the msg by removing tags
				//messagePlainText=Fmt.StripTags(msg); //dont strip - too slow 20110411JN
				message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(messagePlainText, null, "text/plain"));
			}

			message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(msg, null, "text/html"));
			// The userState can be any object that allows your callback 
			// method to identify this send operation.
			// For this example, the userToken is a string constant.
			//string userState = "test message1";

			if (attachments.IsNotBlank()) {
				if (attachments.IndexOf(";") == -1)//20110404 JN Fixed
				{
					string file = attachments;
					Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
					// Add time stamp information for the file.
					ContentDisposition disposition = data.ContentDisposition;
					disposition.CreationDate = System.IO.File.GetCreationTime(file);
					disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
					disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
					// Add the file attachment to this e-mail message.
					message.Attachments.Add(data);
					//data.Dispose();
				} else {
					string[] files = attachments.Split(new Char[] { ';' });//20110404 JN Fixed
					foreach (string file in files) {
						Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
						// Add time stamp information for the file.
						ContentDisposition disposition = data.ContentDisposition;
						disposition.CreationDate = System.IO.File.GetCreationTime(file);
						disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
						disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
						// Add the file attachment to this e-mail message.
						message.Attachments.Add(data);

					}
				}
			}
			//string file = "";
			// Create	the file attachment for this e-mail message.
			//Send the message.
			//SmtpClient client = new SmtpClient(server);
			// Add credentials if the SMTP server requires them.
			//client.Credentials = CredentialCache.DefaultNetworkCredentials;
			SmtpClient client = GetServerSmtpClient();
			try {
				//Send the message.
				client.Send(message);
				// Display the values in the ContentDisposition for the attachment.
				//ContentDisposition cd = data.ContentDisposition;
				//Console.WriteLine("Content disposition");
				//Console.WriteLine(cd.ToString());
				//Console.WriteLine("File {0}", cd.FileName);
				//Console.WriteLine("Size {0}", cd.Size);
				//Console.WriteLine("Creation {0}", cd.CreationDate);
				//Console.WriteLine("Modification {0}", cd.ModificationDate);
				//Console.WriteLine("Read {0}", cd.ReadDate);
				//Console.WriteLine("Inline {0}", cd.Inline);
				//Console.WriteLine("Parameters: {0}", cd.Parameters.Count);
				//foreach(DictionaryEntry d in cd.Parameters)
				//{
				//	Console.WriteLine("{0} = {1}", d.Key, d.Value);
				//}


				//client.SendAsync(message, userState);			 // see client.SendCompleted above
				//client.Send(message);

				//client.SendAsyncCancel();	 	 // see client.SendCompleted above
				// Clean up.
			} catch (Exception e) {
				string serverType = Util.ServerIs();
				string authuser = ConfigurationManager.AppSettings.Get("EmailAuthUser" + serverType);
				string authpass = ConfigurationManager.AppSettings.Get("EmailAuthPassword" + serverType);
				this.errorResult = "Error -em2: [" + e.Message + "] usr[" + authuser + "] Host[" + client.Host + "] from[" + message.From + "]to[" + toEmail + "] [" + e.Message + "]";
				throw new Exception(errorResult);
			}
			message.Dispose();
			return result;
		}

		public void AttachmentExample(string server) {
			// Specify the file to be attached and sent.
			// This example assumes that a file named Data.xls exists in the
			// current working directory.
			string file = "data.xls";
			// Create a message and set up the recipients.
			MailMessage message = new MailMessage(
				 "jane@test.com",
				 "ben@test.com",
				 "Quarterly data report.",
				 "See the attached spreadsheet.");

			// Create	the file attachment for this e-mail message.
			Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
			// Add time stamp information for the file.
			ContentDisposition disposition = data.ContentDisposition;
			disposition.CreationDate = System.IO.File.GetCreationTime(file);
			disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
			disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
			// Add the file attachment to this e-mail message.
			message.Attachments.Add(data);
			//Send the message.
			SmtpClient client = new SmtpClient(server);
			// Add credentials if the SMTP server requires them.
			client.Credentials = CredentialCache.DefaultNetworkCredentials;
			client.Send(message);
			// Display the values in the ContentDisposition for the attachment.
			ContentDisposition cd = data.ContentDisposition;
			Console.WriteLine("Content disposition");
			Console.WriteLine(cd.ToString());
			Console.WriteLine("File {0}", cd.FileName);
			Console.WriteLine("Size {0}", cd.Size);
			Console.WriteLine("Creation {0}", cd.CreationDate);
			Console.WriteLine("Modification {0}", cd.ModificationDate);
			Console.WriteLine("Read {0}", cd.ReadDate);
			Console.WriteLine("Inline {0}", cd.Inline);
			Console.WriteLine("Parameters: {0}", cd.Parameters.Count);
			//foreach(DictionaryEntry d in cd.Parameters)
			//{
			//	Console.WriteLine("{0} = {1}", d.Key, d.Value);
			//}
			data.Dispose();
		}

		public void SendCompletedCallback(object sender, AsyncCompletedEventArgs e) {
			// Get the unique identifier for this asynchronous operation.
			String token = (string)e.UserState;

			if (e.Cancelled) {
				Console.WriteLine("[{0}] Send canceled.", token);
			}
			if (e.Error != null) {
				Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
			} else {
				Console.WriteLine("Message sent.");
			}
			mailSent = true;
		}


		public static string AddGoogleCampaignTrackingCodes(string body, string source, string campaign) {
			var regexPatternBaseUrl = Web.BaseUrl.RemoveSuffix("/").Replace(".", "\\.");
			//var regexPatternBaseUrl = "http://www.rishworthaviation.com".Replace(".","\\.");
			body = Regex.Replace(body, @"(href=""" + regexPatternBaseUrl + @".*?\?.*?)""", @"$1--BEWEBTOKEN--""");
			body = Regex.Replace(body, @"(href=""" + regexPatternBaseUrl + @".*?)""", @"$1?utm_source=" + source.UrlEncode() + @"&utm_campaign=" + campaign.UrlEncode() + @"&utm_medium=email""");
			body = body.Replace("--BEWEBTOKEN--?", "&");
			return body;
		}

	}


	/// <summary>
	/// Enables more advanced options than the SendEMail static methods.
	/// </summary>
	public class ElectronicMail {
		MailMessage message;
		public bool UseMailLog = Util.GetSettingBool("UseMailLog", false);
		public bool MailLogFullText = Util.GetSettingBool("MailLogFullText", false);
		private string trackingGuid = Fmt.CleanAlphaNumeric(Guid.NewGuid().ToString());

		/// <summary>
		/// example:
		/// 
		/// ElectronicMail em = new ElectronicMail(){
		/// 	ToAddress=record.Email,
		/// 	ToName = record.FullName,
		/// 	FromAddress = Util.GetSetting("EmailFromAddress"),
		/// 	FromName = Util.GetSetting("SiteName")};
		/// em.Subject = WhiteLabelSite.ReplaceWhiteLabelTextContent(tb.Title,whiteLabel);
		/// em.BodyHtml=emailBody;
		/// em.BodyPlain=plainBody;
		/// em.Send(false);

		/// 
		/// </summary>
		/*
		 more examples:
 		public static bool SendSummaryEmailsToAdmin() {
			var tb2 = (new Beweb.TextBlock("Email Summary To Admin", "Daily Summary",
			@"<p>Dear Administrator,</p> 
<p>Please find below the daily summary of activity.</p> 
<p></p> 
<p>Winners:<br> [--WINNERS--], Count([--WINNERS-COUNT--]).</p> 
<p>Losers:<br> [--LOSERS--], Count([--LOSERS-COUNT--]).</p> 
"));
			//SendEMail eMail = new SendEMail();
			string body = tb2.RawBody;	 // use raw body for email, not bodytexthtml which has links to the site removed from it / replaced with nothing
			//var link = @"<a href=""" + url + @""">Click here</a>";
			//body = body.Replace("[--WEBLINK--]", link);

			var losers= new DelimitedString();
			var winners = "";
			var allids= new DelimitedString();
			losers.CheckForNoDelimiter=false;
			var sql = new Sql("select * from gameslot where fpuniquecode is not null and VerificationSentToAdmin=0 order by gameslotid");
			var slots = Models.GameSlotList.Load(sql);

			var count = 0;
			foreach (var slot in slots) {
				if(slot.WinOrLose=="W")	{
					count++;
					string borderBottom = count == slots.Filter(s => s.WinOrLose == "W").Count? "border-bottom: 1px solid #e6e7e8;" : "";
					winners += "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style='width: 100%;font-size: 14px;line-height: 20px;border-top: 1px solid #e6e7e8; "+borderBottom+" padding: 10px 0 10px 0; '>";
					winners += "<tr><td width=\"153\" align=\"right\" style='color: #7c777c; padding-right: 20px;'>Name:</td><td>"+slot.FullName+"</td></tr>";
					winners += "<tr><td width=\"153\" align=\"right\" style='color: #7c777c; padding-right: 20px;'>Email:</td><td>"+slot.Email+"</td></tr>";
					winners += "<tr><td width=\"153\" align=\"right\" style='color: #7c777c; padding-right: 20px;'>Unique game code:</td><td>"+slot.FPUniqueCode+"/"+slot.UniqueGameCode+"</td></tr>";
					//if(slot.WinPrize!=null){winners += "<tr><td width=\"153\" align=\"right\" style='color: #7c777c; padding-right: 20px;'>Prize won:</td><td>"+slot.WinPrize.PrizeDescription+"</td></tr>";}
					winners += "<tr><td width=\"153\" align=\"right\" style='color: #7c777c; padding-right: 20px;'>Prize won:</td><td>"+slot.WinDescriptionOfPrize+"</td></tr>";
					winners += "<tr><td width=\"153\" align=\"right\" style='color: #7c777c; padding-right: 20px;'>Date of play:</td><td>"+slot.DateOfPlay.FmtDateTime()+"</td></tr>";
					winners += "</table>";
				}else if(slot.WinOrLose=="L")	{
					losers.AddItemToString("\n"+slot.FullName+" ("+slot.FPUniqueCode+"/"+slot.UniqueGameCode+", email: "+slot.Email+")");
				}
				allids.AddItemToString(slot.ID+"");
			}

			body = body.Replace("[--LOSERS--]", losers.ToString("None"));
			//body = body.Replace("[--LOSERS-COUNT--]", losers.Count+"");
			body = body.Replace("[--WINNERS--]", winners.DefaultValue("Nothing to report"));
			body = body.Replace("[--WINNERS-COUNT--]", count+"");
			
			string htmlTemplate = SiteCustom.SiteMain.GetTemplate();
			body = htmlTemplate.Replace("[--CONTENT--]", body);

			//attachments  = Server.MapPath(Web.Root+"attachments/")+pdfName;

			var em = new ElectronicMail() {
				Subject = tb2.Title,BodyHtml = body,FromAddress = Util.GetSetting("EmailFromAddress"),FromName = Util.GetSetting("SiteName"),ToAddress = Settings.All.CompAdminEmail.DefaultValue( Util.GetSetting("EmailFromAddress")),ToName = Settings.All.CompAdminName.DefaultValue(Util.GetSetting("SiteName"))};
			var emailWasOK = em.Send(false);
			if(emailWasOK && !Util.ServerIsDev && allids.IsNotBlank)
			{
				(new Sql("update gameslot set VerificationSentToAdmin=1 where gameslotid in (",allids.ToString().SqlizeNumberList(),")")).Execute();
			}

			return emailWasOK;			
		}

		 */

		public ElectronicMail() {
			message = new System.Net.Mail.MailMessage();
			message.BodyEncoding = System.Text.Encoding.UTF8;
			message.SubjectEncoding = System.Text.Encoding.UTF8;

		}

		public string Subject {
			get { return message.Subject; }
			set {
				if (value == null) {
					message.Subject = null;
				} else {
					string subject = value.Replace('\r', ' ').Replace('\n', ' ');
					subject = subject.Left(168); // If the subject line is over 168 characters, it will fail to send an email.
					try {
						message.Subject = subject;
					} catch (System.ArgumentException e) {
						message.Subject = "(No subject)";
					}
				}
			}
		}

		public string FromAddress { get; set; }
		public string FromName { get; set; }
		public string ToAddress { get; set; }
		public string ToName { get; set; }
		public string CC { get; set; }
		public string BCC { get; set; }
		public string ReplyTo { get; set; }

		public string BodyHtml { get; set; }
		public string BodyPlain { get; set; }

		public bool IsTracked = Util.GetSettingBool("MailTracking", false);
		public string TrackingCampaign = Util.GetSetting("MailTrackingCampaign", "Email");
		public string TrackingSource = Util.GetSetting("MailTrackingSource", "Savvy");


		public bool AddAttachment(string filename) {
			return AddAttachment(filename, null);
		}

		public bool AddAttachment(string filename, string niceFileName) {
			bool result = true;
			filename = Web.MapPath(filename);
			result = (File.Exists(filename));
			if (result) {
				Attachment data = new Attachment(filename, MediaTypeNames.Application.Octet);
				if (niceFileName != null) {
					if (niceFileName.DoesntContain(".")) {
						niceFileName += "." + filename.RightFrom(".");
					}
					data.Name = CleanNiceFileName(niceFileName);
				}
				// Add time stamp information for the file.
				ContentDisposition disposition = data.ContentDisposition;
				disposition.CreationDate = System.IO.File.GetCreationTime(filename);
				disposition.ModificationDate = System.IO.File.GetLastWriteTime(filename);
				disposition.ReadDate = System.IO.File.GetLastAccessTime(filename);
				message.Attachments.Add(data);
			}

			return result;
		}

		private string CleanNiceFileName(string niceFileName) {
			if (niceFileName == null) return null;
			return Fmt.CleanString(niceFileName, "[^a-zA-Z0-9 _\\-\\.]");
		}

		public bool AddAttachment(Stream contentStream) {
			return AddAttachment(contentStream, null);
		}

		public bool AddAttachment(Stream contentStream, string niceFileName) {
			bool result = true;
			Attachment data = new Attachment(contentStream, CleanNiceFileName(niceFileName), MediaTypeNames.Application.Octet);
			message.Attachments.Add(data);
			return result;
		}

		public bool AddAttachment(byte[] contentDataByteArray, string niceFileName) {
			MemoryStream stream = new MemoryStream(contentDataByteArray);
			Attachment data = new Attachment(stream, CleanNiceFileName(niceFileName), MediaTypeNames.Application.Octet);
			message.Attachments.Add(data);
			return true;
		}

		/// <summary>
		/// Send the message. Returns true if successful or false if error sending email.
		/// Pass in true as the parameter if you wish it to throw an error instead of returning true/false.
		/// You can check the property ErrorResult to see what the error message was.
		/// </summary>
		/// <param name="throwErrors">true to throw an error</param>
		/// <returns>true if ok or false if error</returns>
		public bool Send(bool throwErrors) {
			bool result = true;
			Sql.LogOtherEventStart("sending mail");
			PrepAddresses();
			PrepBody();
			SmtpClient client = SendEMail.GetServerSmtpClient();
			try {
				Logging.dlog(message.Subject + " Email should be sending to : " + message.To);
				client.Send(message);
				if (Util.GetSettingBool("WriteEmailSendToDLogFile", false)) {
					Logging.dlog("Sent mail subject [" + message.Subject + "]: from [" + message.From + "] to [" + message.To + "] replyto [" + message.ReplyToList + "]");
				}
			} catch (Exception e) {
				string serverType = Util.ServerIs();
				string authuser = ConfigurationManager.AppSettings.Get("EmailAuthUser" + serverType);
				string authpass = ConfigurationManager.AppSettings.Get("EmailAuthPassword" + serverType);
				this.ErrorResult = "Error -em3: [" + message.Subject + "] usr[" + authuser + "] Host[" + client.Host + "] from[" + message.From + "]to[" + message.To + "] [" + e.Message + "]";
				result = false;
				if (throwErrors) throw new BewebException(ErrorResult);

			} finally {
				message.Dispose();
				Sql.LogOtherEventEnd("sending mail");

				if (UseMailLog) {
					LogMessage();
				}
			}

			return result;
		}

		private void LogMessage() {
#if ActiveRecord
			if (!BewebData.TableExists("MailLog")) {
				new Sql("CREATE TABLE [dbo].[MailLog]([MailLogID] [int] IDENTITY(1,1) NOT NULL, [EmailTo] [nvarchar](150) NULL, [EmailSubject] [nvarchar](150) NULL, [Result] [nvarchar](250) NULL, [DateSent] [datetime] NULL, [EmailFrom] [nvarchar](150) NULL, CONSTRAINT [MailLog_PK] PRIMARY KEY NONCLUSTERED ([MailLogID] ASC))").Execute();
			}
			if (!BewebData.FieldExists("MailLog", "DateSent")) new Sql("ALTER TABLE [dbo].[MailLog] add  [DateSent] [datetime] NULL").Execute();
			if (!BewebData.FieldExists("MailLog", "EmailTo")) new Sql("ALTER TABLE [dbo].[MailLog] add  [EmailTo] [nvarchar](150) NULL").Execute();
			if (!BewebData.FieldExists("MailLog", "EmailFrom")) new Sql("ALTER TABLE [dbo].[MailLog] add  [EmailFrom] [nvarchar](150) NULL").Execute();
			if (!BewebData.FieldExists("MailLog", "EmailFromName")) new Sql("ALTER TABLE [dbo].[MailLog] add  [EmailFromName] [nvarchar](150) NULL").Execute();
			if (!BewebData.FieldExists("MailLog", "EmailToName")) new Sql("ALTER TABLE [dbo].[MailLog] add  [EmailToName] [nvarchar](150) NULL").Execute();
			if (!BewebData.FieldExists("MailLog", "EmailCC")) new Sql("ALTER TABLE [dbo].[MailLog] add  [EmailCC] [nvarchar](250) NULL").Execute();
			if (!BewebData.FieldExists("MailLog", "DateViewTracked")) new Sql("ALTER TABLE [dbo].[MailLog] add  [DateViewTracked] [datetime] NULL").Execute();
			if (!BewebData.FieldExists("MailLog", "TrackingGUID")) new Sql("ALTER TABLE [dbo].[MailLog] add  [TrackingGUID] [nvarchar](50) NULL").Execute();
			if (MailLogFullText) {
				if (!BewebData.FieldExists("MailLog", "EmailBodyPlain")) new Sql("ALTER TABLE [dbo].[MailLog] add  [EmailBodyPlain] [nvarchar](max) NULL").Execute();
				if (!BewebData.FieldExists("MailLog", "EmailBodyHtml")) new Sql("ALTER TABLE [dbo].[MailLog] add  [EmailBodyHtml] [nvarchar](max) NULL").Execute();
			}
			var maillog = new ActiveRecord("MailLog", "MailLogID");
			maillog["DateSent"].ValueObject = DateTime.Now;
			maillog["EmailFromName"].ValueObject = FromName.Left(150);
			maillog["EmailFrom"].ValueObject = FromAddress.Left(150);
			maillog["EmailToName"].ValueObject = ToName.Left(150);
			maillog["EmailTo"].ValueObject = ToAddress.Left(150);
			maillog["EmailSubject"].ValueObject = Subject.Left(150);
			maillog["EmailCC"].ValueObject = CC.Left(250);
			maillog["TrackingGUID"].ValueObject = trackingGuid;

			if (MailLogFullText) {
				maillog["EmailBodyPlain"].ValueObject = BodyPlain;
				maillog["EmailBodyHtml"].ValueObject = BodyHtml;
			}
			maillog["Result"].ValueObject = ErrorResult.Left(250) ?? "OK";
			maillog.Save();
#endif
		}

		private void PrepAddresses() {
			// sender
			if (FromAddress.IsBlank()) {
				FromAddress = SendEMail.EmailFromAddress;
				FromName = SendEMail.EmailFromName;
			}
			if (FromName.IsBlank()) {
				message.From = new MailAddress(FromAddress);
			} else {
				message.From = new MailAddress(FromAddress, FromName, System.Text.Encoding.UTF8);
			}
#if DOTNET4
			if (ReplyTo.IsBlank()) {																						 //20150205 jn added
				string replyTo = Util.GetSetting("EmailReplytoAddress", "");
				if (replyTo.IsNotBlank()) { ReplyTo = replyTo; }
			}
			message.ReplyToList.Add(new MailAddress(ReplyTo ?? FromAddress));    // .net 4 version
#else
					message.ReplyTo = new MailAddress(ReplyTo ?? FromAddress);           // .net 3.5 version
#endif


			// recipients
			FillRecipients(message.To, ToAddress, ToName, SendEMail.EmailToAddress, null, null);
			FillRecipients(message.CC, CC, null, null, null, null);
			FillRecipients(message.Bcc, BCC, null, null, null, null);

			// override email address for dev server
			if (SendEMail.IsEmailOverride) {
				message.To.Clear();
				FillRecipients(message.To, ToAddress, ToName, SendEMail.EmailToAddress, null, SendEMail.EmailOverrideAddress);
				message.CC.Clear();
				FillRecipients(message.CC, CC, null, null, null, SendEMail.EmailOverrideAddress);
				message.Bcc.Clear();
				FillRecipients(message.Bcc, BCC, null, null, null, SendEMail.EmailOverrideAddress);
			}
		}

		private void FillRecipients(MailAddressCollection recips, string address, string name, string defaultAddress, string defaultName, string emailOverrideAddress) {
			if (address.IsBlank()) {
				address = defaultAddress;
				name = defaultName;
			}
			if (address.IsBlank()) return;

			address += "";
			address = address.Replace(';', ',');
			address = address.Replace('|', ','); //JC ADDED 20140722
			if (address.Contains(",")) {
				// multiple email addresses
				string[] toAddresses = address.Split(',');
				foreach (string email in toAddresses) {
					var mailAddress = new MailAddress(CheckEmailAddressForOverride(email, emailOverrideAddress));
					if (!recips.Contains(mailAddress)) {
						recips.Add(mailAddress);
						if (Util.GetSettingBool("WriteEmailSendToDLogFile", false)) {
							Logging.dlog("1: add email recip [" + email + "]");
						}
					}
				}
			} else if (name.IsBlank()) {
				recips.Add(new MailAddress(CheckEmailAddressForOverride(address, emailOverrideAddress)));
				if (Util.GetSettingBool("WriteEmailSendToDLogFile", false)) {
					Logging.dlog("2: add email recip [" + address + "]");
				}
			} else {
				recips.Add(new MailAddress(CheckEmailAddressForOverride(address, emailOverrideAddress), name, System.Text.Encoding.UTF8));
				if (Util.GetSettingBool("WriteEmailSendToDLogFile", false)) {
					Logging.dlog("3: add email recip [" + address + "]");
				}
			}
		}

		private static string CheckEmailAddressForOverride(string email, string emailOverrideAddress) {
			string emailAddress = email.Trim();
			if (emailOverrideAddress.IsNotBlank()) {
				string flattenedOriginalEmailAddress = emailAddress.Replace("@", "_at_");
				emailAddress = emailOverrideAddress.Replace("@", "+" + flattenedOriginalEmailAddress + "@");
			}
			return emailAddress;
		}

		private void PrepBody() {
			if (this.IsTracked) {
				BodyHtml = SendEMail.AddGoogleCampaignTrackingCodes(BodyHtml, TrackingSource, TrackingCampaign);

				if (BodyHtml.IsNotBlank()) {
					BodyHtml += Environment.NewLine + "<img src='" + Web.BaseUrl + "track/" + trackingGuid + "' />";
				}
			}

			if (BodyHtml.IsNotBlank() && BodyPlain.IsNotBlank()) {
				message.IsBodyHtml = true;
				message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(BodyPlain, null, "text/plain"));
				message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(BodyHtml, null, "text/html"));
			} else if (BodyHtml.IsNotBlank()) {
				message.IsBodyHtml = true;
				message.Body = BodyHtml;
			} else {
				message.IsBodyHtml = false;
				message.Body = BodyPlain;
			}
		}

		/// <summary>
		/// Returns any error message or blank if no error
		/// </summary>
		public string ErrorResult {
			get;
			private set;
		}

		/// <summary>
		/// add a list of ; separated attachments
		/// </summary>
		/// <param name="list"></param>
		public void AddAttachments(string list) {
			foreach (var attachment in list.Split(';')) {
				AddAttachment(attachment);
			}
		}
		public void DebugDump() {
			string html = "";
			if (Subject.IsNotBlank()) html += "Subject: " + Subject.HtmlEncode() + "<br>";
			if (FromAddress.IsNotBlank()) html += "FromAddress: " + FromAddress.HtmlEncode() + "<br>";
			if (FromName.IsNotBlank()) html += "FromName: " + FromName.HtmlEncode() + "<br>";
			if (ToAddress.IsNotBlank()) html += "ToAddress: " + ToAddress.HtmlEncode() + "<br>";
			if (ToName.IsNotBlank()) html += "ToName: " + ToName.HtmlEncode() + "<br>";
			if (CC.IsNotBlank()) html += "CC: " + CC.HtmlEncode() + "<br>";
			if (BCC.IsNotBlank()) html += "BCC: " + BCC.HtmlEncode() + "<br>";
			if (BodyHtml.IsNotBlank()) html += "BodyHtml: " + BodyHtml + "<br>";
			if (BodyPlain.IsNotBlank()) html += "BodyPlain: " + BodyPlain.HtmlEncode() + "<br>";
			Web.Write(html);
		}

	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Beweb {
	/// <summary>
	/// These are the core standard emails.
	/// All custom site specific emails should be located in SiteCustom/Emails.cs the site sends in one place.
	/// </summary>
	public class Email {



		/// <summary>
		/// standard Security email
		/// </summary>
		/// <param name="emailAddress"></param>
		/// <param name="resetId"></param>
		public static void PasswordReset(string emailAddress, string resetId, string userName, string changePasswordURL) {
			string subject = Util.GetSiteName() + " password reset";
			var basePath = Util.GetSetting("WebsiteBaseUrl", "throw");
			if (basePath.IndexOf("|") != -1) {
				basePath = basePath.Split('|')[0];//get first if there are several
			}
			string message = String.Format(@"
A password reset for {0} has been requested.

If you did not request this, then please ignore this email, no further action is required.

To reset your password, please click this link:
{1}{4}?p={2}
(this link is valid once only and expires after one day)

Your username is: '{3}'


Thanks
"
					, Util.GetSiteName()
					, basePath
					, resetId
					, userName
					, changePasswordURL
					);

			Beweb.SendEMail.SimpleSendEmail(emailAddress, subject, message);
		}

		/// <summary>
		/// standard Security email
		/// </summary>
		public static void PasswordReminder(string emailAddress, string password, string userName, string loginURL) {
			string subject = Util.GetSiteName() + " password reminder";
			string message = String.Format(@"
A password reminder for {0} has been requested.

If you did not request this, then please ignore this email, no further action is required.

Your username is: {3}
Your password is: {2}

{1}{4}

Thanks
"
					, Util.GetSiteName()
					, Web.BaseUrl
					, password
					, userName
					, loginURL
					);

			Beweb.SendEMail.SimpleSendEmail(emailAddress, subject, message);
		}
	}
}


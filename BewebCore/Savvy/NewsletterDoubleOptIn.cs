using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Beweb;
using Models;
using TextBlock = Beweb.TextBlock;

/// <summary>
/// NewsletterDoubleOptIn
/// </summary>
public class NewsletterDoubleOptIn {
	/// <summary>
	/// When a new person subscribes, you need to add a Person record for them (using Models.Person) and complete all the fields you have data for, then pass it to this method.
	/// This method sends an email to the person asking them to click a link.
	/// The final part of double opt in is when the link is clicked.  The link takes them back to the Subscribe page with a URL parameter of "optin=[encryptedid]", and you need to call VerificationLinkClicked(Request["optin"]).
	/// </summary>
	/// <param name="person">A Models.Person object with email address and any other fields completed.</param>
	/// <param name="subscribePageFileName">File name of subscribe or optin page that calls VerificationLinkClicked (may include path from site root, may start with ~)</param>
	public static void SendVerificationEmail(ActiveRecord person, string subscribePageFileName) {
		// check person record is OK
		AssertFieldsExist(person);
		string email = person["Email"].ToString();

		// validate	
		if (email.IsBlank()) {
			throw new Exception("NewsletterDoubleOptIn: Email address was null or empty.");
		}
		if (!email.IsEmail()) {
			throw new Exception("NewsletterDoubleOptIn: Email address was not valid [" + email + "].");
		}

		// send double opt in email

		Models.TextBlock emailText = Models.TextBlock.LoadBySectionCode("Double_OptIn_Email");
		// create if not found
		if (emailText == null) {
			emailText = new Models.TextBlock() {
				SectionCode = "Double_OptIn_Email",
				IsBodyPlainText = true,
				IsTitleAvailable = true,
				BodyTextHtml = string.Format("Please confirm that you wish to subscribe to our email newsletter.{0}{0}Click the link below if you wish to subscribe:", Environment.NewLine),
				Title = "Email Newsletter - Please Confirm"
			};
			emailText.Save();
		}

		string emailBody = emailText.BodyTextHtml;
		emailBody += Environment.NewLine + Web.ResolveUrlFull(subscribePageFileName) + "?optin=" + Crypto.EncryptID((int)person["PersonID"].ValueObject).UrlEncode();
		SendEMail.SimpleSendEmail(email, emailText.Title, emailBody);

		person["DoubleOptInEmailSentDate"].ValueObject = DateTime.Now;
		person.Save();
	}

	public static void AssertFieldsExist(ActiveRecord person) {
		// check person record is OK
		if (!person.FieldExists("Email") || ! person.FieldExists("PersonID") || !person.FieldExists("DoubleOptInEmailSentDate") ||!person.FieldExists("SubscribeDate")||!person.FieldExists("UnsubscribeDate") ||!person.FieldExists("HasValidatedEmail")) {
			throw new Exception("NewsletterDoubleOptIn - fields missing from person record. The following fields must exists: Email, PersonID, DoubleOptInEmailSentDate, SubscribeDate, UnsubscribeDate, HasValidatedEmail");
		}
	}

	/// <summary>
	/// The Subscribe page must call this function if Request["optin"]!=null to provide the completion of double opt in.
	/// </summary>
	/// <example>if (Request["optin"]!=null) { Models.TextBlock myTextBlock = VerificationLinkClicked(Request["optin"]) }</example>
	/// <param name="optInCode">You need to pass in Request["optin"]</param>
	/// <returns>Returns a Models.TextBlock containing the title and body text to be displayed to the user</returns>
	public static Models.TextBlock VerificationLinkClicked(string optInCode) {
		Models.TextBlock textBlock;
		//Web.Write("optinCode: "+Crypto.DecryptID(optInCode));
		//Web.End();
		//var person = Person.LoadID(Crypto.DecryptID(optInCode));
		var person = new ActiveRecord<int>("Person", "PersonID");
		AssertFieldsExist(person);
		bool exists = person.LoadID(Crypto.DecryptID(optInCode));

		if (!exists) {
			// not on database, show error
			// get text to display on screen
			textBlock = Models.TextBlock.LoadBySectionCode("Double_OptIn_Error");
			// create if not found
			if (textBlock == null) {
				textBlock = new Models.TextBlock {
					SectionCode = "Double_OptIn_Error", 
					Title = "Problem with confirmation link", 
					IsTitleAvailable = true,
					IsBodyPlainText = false,
					BodyTextHtml = "Sorry, the link you clicked did not work. Your record was not found in our database.<br><br>If you would like to subscribe to our email newsletter, please use the subscribe panel on the right and try the process again." 
				};

				textBlock.Save();
			}
		} else {
			// opt in
			person["SubscribeDate"].ValueObject = DateTime.Now; 
			person["HasValidatedEmail"].ValueObject = true;
			person.Save();

			// get text to display on screen
			textBlock = Models.TextBlock.LoadBySectionCode("Double_OptIn_Confirmed");
			// create if not found
			if (textBlock == null) {
				textBlock = new Models.TextBlock {
					SectionCode = "Double_OptIn_Confirmed",
					IsBodyPlainText = false,
					IsTitleAvailable = true,
					Title = "Subscribe Confirmed",
					BodyTextHtml = "Thanks very much for confirming your subscription.<br><br>You are now subscribed to our email newsletter."
				};
				textBlock.Save();
			}
		}
		return textBlock;
	}



}

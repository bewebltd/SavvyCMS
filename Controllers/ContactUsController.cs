using System;
using System.Drawing.Imaging;
using System.Web.Mvc;
using Beweb;
//using BewebCore.ThirdParty.CaptchaCodeProject;
using Models;
using SavvyMVC.Helpers;
using Site.SiteCustom;

namespace Site.Controllers {
	public class ContactUsController : ApplicationController {



		public ActionResult Index() {
			var data = new ViewModel();

			data.ContentPage = Models.Page.LoadOrCreatePageCode("ContactUs");
			if (data.ContentPage == null) throw new Exception("Contact Us page not found");

			var companyDetail = Models.CompanyDetail.Load(new Sql("Select Top 1 * from CompanyDetail"));
			if (companyDetail == null) {
				companyDetail = new CompanyDetail();
				companyDetail.Address = "Lorem ipsum dolor sit amet";
				companyDetail.DateAdded = DateTime.Now;
				companyDetail.Email = "somecompany@company.com";
				companyDetail.Latitude = 41.4395;
				companyDetail.Longitude = 72.1936;
				companyDetail.Phone = "00-0000000-00";
				companyDetail.Title = "Lorem ipsum dolor sit amet";
				companyDetail.Save();
			}

			data.CompanyDetails = companyDetail;

			data.ContactUsTextBlock = TextBlockCache.GetRich("Contact Us Text", "");
			data.ContactDetails = new ContactUs();
			return View("ContactUs", data);
		}

		public ActionResult Thanks() {
			var page = new ViewModel();
			page.ContentPage = Models.Page.LoadPageContent("ContactUsThanks");

			return View("ContactUsThanks", page);
		}


		public class ViewModel : PageTemplateViewModel {
			public CompanyDetail CompanyDetails;
			public Models.TextBlock ContactUsTextBlock;
			public ContactUs ContactDetails;
		}

		[HttpPost]
		public ActionResult Submit(FormCollection collection) {

			if (!Crypto.CheckMinuteCypher(Request["cd"], 60)) {
				Web.InfoMessage = "Are you a robot? if not please try the form again.";
				return Redirect(Web.Root + "ContactUs?mode=token");
			}

			var data = new ViewModel();
			data.ContentPage = Models.Page.LoadOrCreatePageCode("ContactUs");
			if (data.ContentPage == null) throw new Exception("Contact Us page not found");
			var contactUs = new ContactUs();
			contactUs.UpdateFromRequest();
			contactUs.Save();
			data.ContactDetails = contactUs;
			SendContactEmail(contactUs);

			var thankYouMsg = new Beweb.TextBlock("Contact Us Thank You Msg");

			return View("ContactUsThanks", data);
		}

		private void SendContactEmail(ContactUs record) {
			//admin email
			var body = "";
			var template = (new Beweb.TextBlock("Contact Us Notification Email Text", Util.GetSiteName() + " Admin: Contact Request.",
 @"<p>Dear Admin,</p> 
<p>A member of the public has requested that you contact them. Here are the details:</p> 
<p>[--WEBLINK--]</p> 
<p>Name:[--Name--]</p>
<p>Email: [--Email--]</p>
<p>Subject: [--Subject--]</p>
<p>Message:<br>[--Message--]</p>"));

			body = template.RawBody;	  // use raw body for email, not bodytexthtml which has links to the site removed from it / replaced with nothing
			var url = Web.BaseUrl + "Admin/ContactUsAdmin/Edit/" + record.ID;
			var link = @"<a href=""" + url + @""">Click here</a>";
			body = body.Replace("[--WEBLINK--]", link);
			body = body.Replace("[--Name--]", record.Name);
			body = body.Replace("[--Email--]", record.Email);
			body = body.Replace("[--Subject--]", record.Subject);
			body = body.Replace("[--Message--]", record.Message);

			//send email to admin
			var email = new Beweb.ElectronicMail();
			email.ToAddress = Util.GetSetting("EmailToAddress");
			email.Subject = template.Title;
			email.BodyHtml = body;
			email.Send(true);

		}


	}
}

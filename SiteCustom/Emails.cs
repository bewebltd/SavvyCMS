using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Beweb;
using Models;
using Site.SiteCustom;

namespace Site.SiteCustom {
	public class Emails {

		public static void CommentApproveDeclineToPerson(Models.Comment comment) {
			// send the email to the person
			string address = comment.CommenterEmail;
			if (comment.PersonID != null) {
				// try and load the person - they may have been deleted
				var p = Models.Person.LoadByPersonID(comment.PersonID.Value);
				if (p != null) {
					address = p.Email;
				}
			}
			if (address.IsBlank()) return; // no email - cancel this
			string body = comment.DeclineReason.FmtPlainTextAsHtml();
			string subject = "";
			if (comment.Status == Models.Comment.CommentStatus.Approved.ToString()) {
				subject = "Your New World Auction comment has been approved";
			}
			if (comment.Status == Models.Comment.CommentStatus.Declined.ToString()) {
				subject = "Your New World Auction comment has been declined";
			}
			SendEmail(address, subject, subject, body);
		}

		public static void AuctionWinner(Auction auction, Person winnerPerson) {
			// send the email to the person
			string address = winnerPerson.Email;
			if (address.IsBlank()) return; // no email - cancel this
			string subject = "Prezzy Scored! You have won: " + auction.Title;
			var body = "Hi " + winnerPerson.FirstName + ",<br><br> Well done! You have won this Prezzy in the Great Prezzy Auction: " +
										"<a href=' " + Web.ResolveUrlFull(auction.GetUrl()) + "'>" + auction.Title + "</a> for "+auction.CurrentBidCredits+" credits"+
										"<br><br>We have your current address as:<br>" +
										winnerPerson.Street + "<br>" +
										winnerPerson.Suburb + "<br>" +
										winnerPerson.City + "<br>" +
										winnerPerson.PostCode + "<br><br>" +
										"If this is incorrect you have until " + DateTime.Now.AddDays(1) + " to change your address before Santa harnesses his reindeer.<br><br>" +
										"Want to tell your friends about your prezzy? <a href= 'http://www.facebook.com'>brag on Facebook</a>.<br><br>" +
										"Merry Christmas from The Team at New World.";
			SendEmail(address, subject, subject, body);
		}

		public static void BuyNowWinner(BuyNowItem buyNow, Person winnerPerson) {
			// send the email to the person
			string address = winnerPerson.Email;
			if (address.IsBlank()) return; // no email - cancel this
			string subject = "You won using buy now: " + buyNow.Title;
			var body = "Hi " + winnerPerson.FullName + ",<br><br> This email is to let you know that you won " +
										" the following prize using the buy now option:<br> <a href=' " + Web.ResolveUrlFull(buyNow.GetUrl()) + "'>" + buyNow.Title +
										"</a>" +
										"<br><br>We have your current address as:<br>" +
										winnerPerson.Street + "<br>" +
										winnerPerson.Suburb + "<br>" +
										winnerPerson.City + "<br>" +
										winnerPerson.PostCode + "<br><br>" +
										"If this is incorrect you have until " + DateTime.Now.AddDays(1) + " to update your details to confirm your address.<br><br>" +
										"Don't forget to <a href= 'http://www.facebook.com'>brag to your friends on Facebook</a> about the great prize you have won.<br><br>" +
										"Cheers,<br><br>" +
										"The Team at New World";

			SendEmail(address, subject, subject, body);
		}

		public static void Outbid(Auction auction, Person previousBidder) {
			if (!previousBidder.ReceiveOutBidEmail) return;

			string address = previousBidder.Email;
			if (address.IsBlank()) return; // no email - cancel this
			string subject = "Drat! You have been outbid on: " + auction.Title;
			string body = "Hi " + previousBidder.FirstName + "," +
										"<br><br>Somebody else has their eye on your prezzie! You’ve just been outbid on this Prezzy in the Great Prezzy Auction: <br> " +
										"<a href=' " + Web.ResolveUrlFull(auction.GetUrl()) + "'>" + auction.Title +"</a><br><br>" +
										"<a href=' " + Web.ResolveUrlFull(auction.GetUrl()) + "'>Don’t want to miss out? Bid Again</a> <a href= '" + Web.BaseUrl + "Home'>View all Prezzies</a><br><br>" +
										"Cheers,<br><br>" +
										"The Team at New World";
			SendEmail(address, subject, subject, body, true);
		}

		private static void SendEmail(string address, string subject, string heading, string htmlBody, bool showUnsubscribe = false) {
			var html = FileSystem.GetFileContents(Web.MapPath("~/emailtemplate/template.htm"), false);
			var footer = "";
			if(showUnsubscribe) {
				footer = TextBlockCache.Get("Email Footer Outbid","Sent to xxxemailxxx. You have been sent this email because you have registered for the New World Great Kiwi Christmas campaign. We will only send you these emails for auctions you are bidding on or watching, you can <a style=\"color:#ffffff;\" href=\"xxxunsubscribexxx\">unsubscribe</a> if you want to stop these. New World, Private Bag 4705, Christchurch.").BodyTextHtml;
			}else {
				footer = TextBlockCache.Get("Email Footer Normal","Sent to xxxemailxxx. You have been sent this email because you have registered for the New World Great Kiwi Christmas campaign. We will only send you these emails for auctions you are bidding on or watching. New World, Private Bag 4705, Christchurch.").BodyTextHtml;
			}
			html = html.Replace("xxxtitlexxx", heading);
			html = html.Replace("xxxcontentxxx", htmlBody);
			html = html.Replace("xxxfooterxxx", footer);
			if(showUnsubscribe) {
				var pid = Person.LoadByEmail(address).ID;
				html = html.Replace("xxxunsubscribexxx", Web.BaseUrl+"Page/Unsubscribe?uid="+Crypto.EncryptIDClassic(pid));
			}
			html = html.Replace("xxxemailxxx", address);
			html = html.Replace("images/", Web.BaseUrl+"emailtemplate/images/"); 
			try {
				Beweb.SendEMail.SimpleSendHTMLEmail(address, subject, html);
			} catch (System.FormatException formatException) {
				// The specified string is not in the form required for an e-mail address.
				// Send to webmaster instead

				html = "The following message was supposed to be sent to '" + address + "' but the email address is invalid.<br><br>" + htmlBody;
				Beweb.SendEMail.SimpleSendHTMLEmail(SendEMail.EmailToAddress, "Invalid email address for auction message", htmlBody);
			}
		}

	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Site.SiteCustom;

namespace Site.Controllers {
	public class CommentController : ApplicationController {

		public ActionResult RenderComments(int? auctionID, int? buyNowID) {
			var data = new ViewModel();
			data.AuctionID = auctionID;
			data.BuyNowID = buyNowID;
			data.CommentPostAuthToken = CreateAuthToken();
			/*if (auctionID != null) {
				var auction = Auction.LoadID(auctionID.Value);
				data.CommentList = auction.ApprovedComments;
			} else if (buyNowID != null) {
				var buyNow = BuyNowItem.LoadID(buyNowID.Value);
				data.CommentList = buyNow.ApprovedComments;
			}*/
			return View("CommentView", data);
		}


		public class ViewModel : PageTemplateViewModel {
			public int? AuctionID;
			public int? BuyNowID;
			public List<Comment> CommentList;
			public string CommentPostAuthToken;
		}

		public ActionResult SendComment(int? auctionID, int? buyNowID, string name, string email, string comment, string auth, int? parentID, string optin) {
			if (!ValidateAuthToken(auth)) {
				return Content("Sorry, your session has timed out. Please refresh the page and try again.");
			}
			var c = new Comment();
			//c.UpdateFromRequest(); //MK why UpdateFromRequest? all values are passed in as parameters right?

			c.CommentText = comment;
			c.CommentDate = DateTime.Now;
			c.Status = Comment.CommentStatus.Submitted.ToString();
		//	c.AuctionID = auctionID;
		//	c.BuyNowItemID = buyNowID;
			c.CommenterIP = Security.UserIpV4Address();
			c.ParentCommentID = parentID;
			c.PersonType = Comment.CommentPersonType.Member.ToString();
			if (Security.IsLoggedIn) {
				var p = Person.LoadByPersonID(Security.LoggedInUserID);

				/*if (p.Role == SecurityRoles.Roles.MODERATOR) {
					// a moderator
					c.PersonType = Comment.CommentPersonType.Moderator.ToString();
				}
*/
				//c.CommenterName = p.UserName;
				c.CommenterEmail = p.Email;
				c.PersonID = p.ID;
			} else {
				c.CommenterName = name.Trim();
				c.CommenterEmail = email;
			}
			c.Save();

			if (optin == "true") {
				//Beweb.SiteCustom.MailChimp.Subscribe(c.CommenterEmail, Person.GetFirstName(c.CommenterName), Person.GetLastName(c.CommenterName), "", "");
			}
			Models.TextBlock thanksTextBlock = TextBlockCache.GetRich("CommentThankYou");
			return View("CommentThankYou", thanksTextBlock);
		}

		private string CreateAuthToken() {
			var authToken = DateTime.Now.AddHours(4) + "%secret";
			return Crypto.Encrypt(authToken);
		}

		private bool ValidateAuthToken(string postedAuthToken) {
			try {
				string authToken = Crypto.Decrypt(postedAuthToken);
				if (authToken != null && authToken.EndsWith("%secret")) {
					var timestamp = authToken.RemoveSuffix("%secret");
					var time = DateTime.Parse(timestamp);
					if (time > DateTime.Now) {
						return true;
					}
				}
			} catch (Exception) {  // eg date parsing error
				// ignore and return false
			}
			return false;
		}


	}
}
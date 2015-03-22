using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;

using SavvyMVC;
using Site.SiteCustom;
using TextBlock = Beweb.TextBlock;
using System.Text;
using System.Text.RegularExpressions;

namespace Site.Controllers {

	//[Authorize(Roles = "administrators,onlinecataloguser")]
	public partial class ShoppingCartController : ApplicationController {

		public class ViewModel : PageTemplateViewModel {

		}
		public ActionResult Index() {
			var data = new ViewModel();
			//data.ContentPage=new Page();


			return View("Cart", data);
		}
		
		//public ActionResult CreateSampleData() {
		//  var cart = new ShoppingCart();
		//  cart.PartDescription="Thing";

		//  return Index();
		//}

		[HttpGet]
		public ActionResult CartRemoveLine(string encryptedShoppingCartID, string encryptedUserToken) {
			string result = "OK";
			var per = Crypto.DecryptID(encryptedUserToken);
			if (per == -1) throw new Exception("person invalid");
			var cartEntry = Models.ShoppingCart.Load(new Sql("select * from shoppingcart where ShoppingCartID=", Crypto.DecryptID(encryptedShoppingCartID), " and personid=", Crypto.DecryptID(encryptedUserToken), ""));
			if (cartEntry != null) {
				cartEntry.IsDeleted=true;
				cartEntry.Save();
			} else {
				result = "Fail";
			}
			return Content(result, "text/html");

		}

		/// <summary>
		/// add to cart 
		/// </summary>
		/// <param name="partNumber"></param>
		/// <param name="encryptedUserToken"></param>
		/// <param name="PartDescription"></param>
		/// <param name="VehID">use -1 if not known</param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult AddToCart(int partNumber, string encryptedUserToken, string PartDescription, int VehID) {
			string result = "OK";
			var per = Crypto.DecryptID(encryptedUserToken);
			if (per == -1) throw new Exception("person invalid");

			var cartEntry = new Models.ShoppingCart();
			cartEntry.DateAdded = DateTime.Now;
			cartEntry.PersonID = Crypto.DecryptID(encryptedUserToken);
			if (partNumber != -1) cartEntry.PartNumber = partNumber;
			cartEntry.PartDescription = PartDescription;
			cartEntry.Quantity = 1;
			cartEntry.Status = "Shopping";
			cartEntry.IsDeleted = false;
			
			cartEntry.Save();

			return Content(result, "text/html");
		}
		[HttpGet]
		public ActionResult PlaceOrder(string encryptedUserToken, string notes, string coref) {
			return PlaceOrder(encryptedUserToken, false, notes, coref);
		}


		public ActionResult PlaceOrder(string encryptedUserToken, bool isCostEnquiry, string Notes, string coref) {
			var PersonID = Crypto.DecryptID(encryptedUserToken);
			var per = Crypto.DecryptID(encryptedUserToken);
			if (per == -1) throw new Exception("person invalid");


			string basketContents = "";
			//result+="person["+PersonID+"]";

			//create order email

			var person = Models.Person.LoadID(PersonID);
			var order = new Models.ShoppingCartOrder();
			order.AddNew();
			order.DateOrdered = DateTime.Now;
			order.OrderRef = Models.ShoppingCart.GenerateRef();
			order.PersonID = PersonID;
			order.Email = person.Email;
			order.FirstName = person.FirstName;
			order.LastName = person.LastName;
			order.IsCostEnquiry = isCostEnquiry;
			order.CustomerOrderReference = coref;
			order.Notes = Notes;
			order.Save();
			//decimal erpID = -1;
			//string nimDataInSQLConn = BewebData.GetConnectionString("NimbleCartSQLConnection");
			//if (Util.GetSettingBool("SaveCartToERP") && Util.IsBewebOffice) {
			//  erpID = GetCartHeaderERPID(order, nimDataInSQLConn);

			//  SaveCartHeaderToERP(person, order, erpID, isCostEnquiry, nimDataInSQLConn);
			//}

			basketContents += "<table class=\"result\" style=\"width:100%\" cellspacing=\"0\" border=\"0\" cellspacing=\"0\">";
			basketContents += "<tr class=\"header\"><td>VehID</td><td>Part</td><td>QTY</td></tr>";
			var status = "Shopping";
			var itemsInCart = Models.ShoppingCartList.Load(new Sql("where personid=", PersonID, " and Status=", status.Sqlize_Text(), " and isnull(isdeleted,0)=0 order by dateadded"));
			if (itemsInCart.Count > 0) {
				foreach (var cartItem in itemsInCart) {
					string removelink = "<a href=\"\" onclick=\"return handleCartRemove(this,'" + Crypto.EncryptID(cartItem.ID) + "','" + encryptedUserToken.JsEncode() + "')\"><img width=\"32\" src=\"" + Web.Root + "images/cart-remove.gif\"></a>";
					basketContents += "<tr><td>" + cartItem.PartDescription + "</td><td>" + cartItem.Quantity + "</td></tr>";
					cartItem.Status = "Ordered";
					cartItem.ShoppingCartOrderID = order.ID;
					cartItem.Save();

					//SaveCartLineToERP(nimDataInSQLConn, erpID, cartItem);
				}
			} else {
				basketContents += "<tr><td colspan=\"4\">Nothing in your cart</td></tr>";
			}
			basketContents += "<tr><td>Notes</td><td colspan=\"3\">" + Notes.HtmlEncode() + "</td></tr>";
			basketContents += "</table>";
			//update the deleted items status that is currently set to shopping, change to archived
			new Sql("update ShoppingCart set status=", ("Archived").Sqlize_Text(), ", ShoppingCartOrderID=", order.ID, "where personid=", PersonID, " and Status=", status.Sqlize_Text(), " and isnull(isdeleted,0)=1 ").Execute();

			//send email to admin
			//basketContents = Savvy.Site.SendOrderEmail(order, person, isCostEnquiry, basketContents);


			return Content(basketContents, "text/html");
		}

		/// <summary>
		/// change qty of a cart item
		/// </summary>
		/// <param name="encryptedUserToken"></param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult ChangeQty(string encryptedUserToken, string newQty, string encryptedCartID, float rnd) {
			string result = "Error";
			var per = Crypto.DecryptID(encryptedUserToken);
			if (per == -1) throw new Exception("person invalid");
			var lineItem = Models.ShoppingCart.LoadID(Crypto.DecryptID(encryptedCartID));
			if (lineItem != null) {
				if (per != lineItem.PersonID) throw new Exception("person invalid");
				lineItem.Quantity = (int)Fmt.CleanNumber(newQty);
				lineItem.Save();
				result = "OK:" + lineItem.Quantity;
			}
			return Content(result, "text/html");

		}

		/// <summary>
		/// return the contents of the cart
		/// </summary>
		/// <param name="encryptedUserToken"></param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult GetCartContents(string encryptedUserToken) {
			var PersonID = Crypto.DecryptID(encryptedUserToken);
			var per = Crypto.DecryptID(encryptedUserToken);
			if (per == -1) throw new Exception("person invalid");

			string result = "";
			//result+="person["+PersonID+"]";
			result += "<table class=\"result\" style=\"width:100%\" cellspacing=\"0\" border=\"0\" cellspacing=\"0\">";
			result += "<tr class=\"header\"><td class=\"hide\">Json</td><td>Part</td><td>Qty</td><td>RRP</td><td width=\"80\">Remove</td></tr>";
			var status = "Shopping";
			var itemsInCart = Models.ShoppingCartList.Load(new Sql("where personid=", PersonID, " and Status=", status.Sqlize_Text(), " and isnull(isdeleted,0)=0 order by dateadded"));
			if (itemsInCart.Count > 0) {
				foreach (var cartItem in itemsInCart) {

					string removelink = "<a href=\"\" onclick=\"return handleCartRemove(this,'" + Crypto.EncryptID(cartItem.ID) + "','" + encryptedUserToken.JsEncode() + "')\"><img width=\"32\" src=\"" + Web.Root + "images/cart-remove.gif\"></a>";

					var json = cartItem.PartNumber.HasValue ? "" + cartItem.PartNumber.Value : "";
					result += "" +
						"<tr>" +
							"<td class=\"hide\">" + json + "</td>" +
							"<td>" + cartItem.PartDescription + "</td>" +
							"<td><input type=\"text\" value=\"" + cartItem.Quantity + "\" onkeyup=\"return handleQtyChange(this,'" + Crypto.EncryptID(cartItem.ID) + "','" + encryptedUserToken.JsEncode() + "')\"></td>" +
							//"<td>" + Models.Part.GetPrice(cartItem.PartDescription) + "</td>" +
							"<td>" + removelink + "</td>" +
						"</tr>";
				}
			} else {
				result += "<tr><td colspan=\"4\">Nothing in your list</td></tr>";
			}
			result += "</table>";

			var help = (new Beweb.TextBlock("Shopping Basket Buttons - Help", "Shopping Basket", "When you are done loading up your list, click order request or cost enquiry. We will contact you directly to discuss the order, and you won't need to read the part numbers to us over the phone. You can decide to proceed with your order at a later stage."));
			string helpHtml = @"<div style=""display:none""><div class=""HelpIconWindow" + help.ID + @""">" + help.BodyText + "</div></div>" +
				@"<a href="""" class=""helpIcon" + help.ID + @"""><img src=""" + Web.Root + @"images/help.gif"" alt=""click for help on " + help.Title.JsEncode() + @""" /></a>" +
				@"<script>$("".helpIcon" + help.ID + @""").colorbox({innerWidth: ""356px"",  inline: true, href: "".HelpIconWindow" + help.ID + @""", opacity: 0.7 });</script>";


			result += "When finished either: " + helpHtml + "<br>";

			result += "<input type=\"button\" value=\"Start parts order request\" class=\"cartCompleteBtn\" id=\"cartCompleteBtn\" onclick=\"handlePlaceOrder(this,'" + encryptedUserToken.JsEncode() + "')\"><br>";
			//result += "or:<br>";
			//result += "<input type=\"button\" value=\"Send cost enquiry email\" class=\"cartCompleteBtn\" id=\"cartPlaceCostEnquiryBtn\" onclick=\"handlePlaceCostEnquiryOrder(this,'" + encryptedUserToken.JsEncode() + "')\"><br>";
			result += "Order Ref/PO Number: <br/><input type=\"text\" maxlength=\"60\" id=\"CustomerOrderReference\"><br>";
			result += "Notes : <br/><textarea id=\"cartNotes\" cols=\"60\" rows=\"6\"></textarea><br>";
			return Content(result, "text/html");
		}

	}
}

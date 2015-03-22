using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Beweb;
using Models;
using Site.SiteCustom;

/*
 This is an example of how you can return create JSON objects and return them to the browser.
 There are 3 main ways of returning data to JS, any method is fine, just depends how you want to call it:
 1. literal js object using SCRIPT tag (can be any domain - simplest way to include a big array of data just once at the top, does not need jquery or xhr calls)
 2. JSON using for example $.getJSON (must be on same domain - normal way if you need to do several calls based on user interaction)
 3. JSONP using for example $.getJSON (can be any domain - standard for serving JSON to browsers on external sites)
 See sample code below.
*/

namespace Site.Controllers {
	public class JsDataController : ApplicationController {
		
		#region Sample code for returning Json, jsonp and javascript literals

		public ActionResult GetExampleJsLiteral() {
			// include this with a script tag
			var data = new Dictionary<string, object>();
			data["test"] = 42;
			var serializer = new JavaScriptSerializer();
			string script = "var myData = " + serializer.Serialize(data) + ";";
			return JavaScript(script);
		}

		public ActionResult GetExampleJson() {
			// call this with jquery
			var data = new Dictionary<string, object>();
			data["test"] = 42;
			return Json(data,JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetExampleJsonP(string callback) {
			// call this with jquery
			var data = new Dictionary<string, object>();
			data["test"] = 42;
			var serializer = new JavaScriptSerializer();
			string script = callback+"(" + serializer.Serialize(data) + ");";
			return JavaScript(script);
		}

		public ActionResult GetExampleComplexJson() {
			// call this with jquery
			var data = new Dictionary<string, object>();
			data["test"] = 42;
			data["anObjectLiteral"] = new {x=1,y=2};
			//foreach (var phone in ProductCache.Active) {
			//  List<object> plansForPhone = new List<object>();
			//  foreach (var pp in phone.VodafonePlanProducts.Active.OrderBy(prodprice=>prodprice.ProductPrice24)) {
			//    plansForPhone.Add(new {planID=pp.VodafonePlanID,price12=phone.PlanPrice12(pp),price24=phone.PlanPrice24(pp),paymentType=pp.VodafonePlan.PaymentType});
			//  }				
			//}
			//data["plansForPhone"] = plansForPhone;
			return Json(data,JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetExampleJsHtml() {
			// include this with a script tag
			// this gets around restrictions on bringing in html from another domain name 
			var html = Util.HttpGet(Web.BaseUrl+"SomeExampleUrl");  
			string script = "var theHtml = " + Fmt.JsEnquote(html) + ";";
			return JavaScript(script);
		}

		#endregion

	}
}
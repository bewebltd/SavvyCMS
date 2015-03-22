using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Beweb;
using Site.SiteCustom;

namespace System.Web.Mvc.Html {
	/// <summary>
	/// Extension methods for HTML Helpers in views
	/// </summary>
	public static class SavvyHtmlHelpers {
		
		/// <summary>
		/// Returns html for a standard admin menu item (TR).
		/// </summary>
		/// <param name="htmlHelper"></param>
		/// <param name="controllerName"></param>
		/// <param name="title"></param>
		/// <param name="descriptionText"></param>
		/// <returns></returns>
		public static MvcHtmlString AdminMenuItem(this HtmlHelper htmlHelper, string controllerName, string title=null, string descriptionText=null,string actionName=null) {
			if (SavvyMVC.Util.MvcUtil.ControllerExists(controllerName+"Admin")) {       // MN 20130131 - fixed bug
				controllerName+="Admin";
			}
			if (SavvyMVC.Util.MvcUtil.ControllerExists(controllerName) && SecurityRoles.Roles.IsAllowedAdminController(controllerName)) {
				if (title.IsBlank()) {
					string items = controllerName.RemoveSuffix("Admin").Plural().SplitTitleCase();
					if (descriptionText.IsBlank()) descriptionText = "View and edit " + items.ToLower();
					title = "Manage " + items;
				} else {
					if (descriptionText.IsBlank()) descriptionText = "View and edit " + title;
				}

				// add section title if needed
				var sectionHtml = "";
				string sectionTitle = Beweb.Web.PageGlobals["AdminMenu_SectionTitle"]+"";
				if (sectionTitle!="") {
					string sectionDescription = Beweb.Web.PageGlobals["AdminMenu_SectionDescription"]+"";
					sectionHtml += "<tr>";
					sectionHtml += "<td class=\"label section\"><strong>"+sectionTitle+"</strong></td>";
					sectionHtml += "<td class=\"section\">"+sectionDescription+"</td>";
					sectionHtml+="</tr>";
					// dont do it again
					Beweb.Web.PageGlobals["AdminMenu_SectionTitle"] = null;
					Beweb.Web.PageGlobals["AdminMenu_SectionDescription"] = null;
				}

				// construct TR
				var tr = new HtmlTag("tr");
				var td = new HtmlTag("td").Add("class", "label");
				var a = new HtmlTag("a").Add("href", controllerName + ((actionName!=null)?"/"+actionName:"")).SetInnerHtml(title);
				var td2 = new HtmlTag("td").SetInnerHtml(descriptionText);
				td.AddTag(a);
				tr.AddTag(td).AddTag(td2);

				return MvcHtmlString.Create(sectionHtml + tr);
			}
			return null;
		}
	
		public static MvcHtmlString AdminMenuLink(this HtmlHelper htmlHelper, string url, string title, string descriptionText=null,string actionName=null) {
			if (descriptionText.IsBlank()) descriptionText = "View and edit " + title;
				
			// add section title if needed
			var sectionHtml = "";
			string sectionTitle = Beweb.Web.PageGlobals["AdminMenu_SectionTitle"]+"";
			if (sectionTitle!="") {
				string sectionDescription = Beweb.Web.PageGlobals["AdminMenu_SectionDescription"]+"";
				sectionHtml += "<tr>";
				sectionHtml += "<td class=\"label section\"><strong>"+sectionTitle+"</strong></td>";
				sectionHtml += "<td class=\"section\">"+sectionDescription+"</td>";
				sectionHtml+="</tr>";
				// dont do it again
				Beweb.Web.PageGlobals["AdminMenu_SectionTitle"] = null;
				Beweb.Web.PageGlobals["AdminMenu_SectionDescription"] = null;
			}

			// construct TR
			var tr = new HtmlTag("tr");
			var td = new HtmlTag("td").Add("class", "label");
			var a = new HtmlTag("a").Add("href", url).SetInnerHtml(title);
			var td2 = new HtmlTag("td").SetInnerHtml(descriptionText);
			td.AddTag(a);
			tr.AddTag(td).AddTag(td2);

			return MvcHtmlString.Create(sectionHtml + tr);
			
			return null;
		}
	
		public static MvcHtmlString AdminMenuSectionTitle(this HtmlHelper htmlHelper, string title, string descriptionText=null) {
			Beweb.Web.PageGlobals["AdminMenu_SectionTitle"] = title;
			Beweb.Web.PageGlobals["AdminMenu_SectionDescription"] = descriptionText;
			return null;
		}

		public static MvcHtmlString BackLink(this HtmlHelper htmlHelper) {
			string html = "";
			if (Beweb.Web.Request["df_returnpage"]!=null) {
				html = "<a class=\"back-link\" href=\"" + Beweb.Web.Request["df_returnpage"] + "\">Back</a>";
			}else {
				html = "<a class=\"back-link\" href=\"#\" onclick=\"if (top.brandstrip && top.brandstrip.GoBack) top.brandstrip.GoBack(window);return false;\">Back</a>";
			}
			return MvcHtmlString.Create(html);
		}
	

	}
}
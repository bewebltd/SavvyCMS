//
using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Web;
using Beweb;
using Savvy;

namespace Site.SiteCustom {
	public partial class AdminNoFormMaster : System.Web.Mvc.ViewMasterPage {
		//public Models.SavvyAdmin styles =null;
		public Hashtable styles = null;
		public bool Exporting = false;
		protected void Page_Load(object sender, EventArgs e) {
			if ((Web.PageGlobals["Exporting"] + "").ToBool()) {
				Exporting = true;
			}
			if (!Exporting) {
				//used by thickbox, and generally useful
				Util.IncludeJavascript(this.Page, "websiteBaseUrl = '" + Web.BaseUrl + "'", true);
				// js includes
				Util.IncludejQuery(Page);
				Util.IncludeColorbox(Page);
				Util.IncludeJavascriptFile(Page, "~/areas/admin/help/helpwindow.js");

				Util.IncludeJQueryUI(Page);
				Util.IncludeSavvyValidate(Page);
				Util.IncludeBewebForms(Page);
				//css includes
				Util.IncludeStylesheetFile(Page, "~/areas/admin/help/helpwindow.css");
				Util.IncludeStylesheetFile(Page, "~/areas/admin/admin.css");
				//Util.IncludeStylesheetFile(Page,"~/warningbar.css"); 
				//Beweb.Util.IncludeStylesheetFile(Page, "~/js/ui.jquery/ui.jquery.themes/ui-darkness/jquery-ui-1.7.2.custom.css");		 //used by tabbed pageedit 
				Util.IncludeJavascriptFile(Page, "~/js/common.js");
				// include tiny MCE scripts for rich text editor
				if (Util.GetSetting("UseRichTextEditor")=="Redactor") {
					Util.InitRedactor(Page);
				} else {
					new Forms.RichTextEditor().InitMCE(Page);
				}

				var prefix = (!Beweb.Util.ServerIsLive) ? Util.ServerIs() + ": " : "";
				Page.Title = prefix + Page.Title + " - " + Util.GetSiteName() + " Content Management System";					 //append the cms name to the title
				Breadcrumbs.Current.SetBreadcrumb(1, "Admin Menu", "~/admin/adminmenu");
			}
		}
	}
}

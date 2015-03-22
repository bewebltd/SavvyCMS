//
using System;
using System.Web;
using System.Web.SessionState;

using System.Collections;
using System.Data;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Beweb;

namespace Savvy {
	/// <summary>
	/// BREADCRUMBS SYSTEM.
	/// By default this is configured for use in the CMS admin system.
	/// To use it on the main part of the site you probably need to subclass this class (by convention we have SiteCustom/TrackingBreadcrumb which you can modify to use for this).
	/// You can configure the properties IsAdminSection and DefaultUrl if needed.
	/// </summary>
	public class Breadcrumbs {
		private int _currentLevel = -1;//not set
		public string WebsiteBaseCodeName = "";
		public HttpRequest CurrentRequestObject = null;
		public HttpSessionState CurrentSessionObject = null;
		private string _defaultUrl;

		public bool IncludeHome {
			get { return Util.GetSettingBool("AdminBreadcrumbIncludeHome", true); }
		}

		public bool ShowCurrentBreadcrumbAsLink {
			get { return Util.GetSettingBool("ShowCurrentBreadcrumbAsLink", false); }
		}

		public int CurrentLevel {
			get { return (CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_currentPageLevel"] + "").ToInt(0); }
		}

		/// <summary>
		/// Set this to the URL of the homepage or admin main menu. (If you don't set this, it will use either site root or admin/adminmenu depending on setting of IsAdminSection.)
		/// </summary>
		public string DefaultUrl {
			get {
				if (_defaultUrl.IsBlank()) {
					// nobody's set it, we better figure it out ourselves
					_defaultUrl = DetermineDefaultUrl();
				}
				return _defaultUrl;
			}
			set {
				// allow people to set it if they like
				_defaultUrl = value;
			}
		}

		/// <summary>
		/// By default this is true, which puts a Home link to the site root before the Admin link. If you set this to false, you can use the breadcrumbs system for the main part of the site. You probably need to subclass this class (by convention we have SiteCustom/TrackingBreadcrumb which you can modify to use for this).
		/// </summary>
		public bool IsAdminSection { get; set; }

		public Breadcrumbs() : this(Web.Request, Web.Session) { }

		public Breadcrumbs(HttpRequest r, HttpSessionState s) {
			WebsiteBaseCodeName = ConfigurationManager.AppSettings["SiteName"];
			CurrentRequestObject = r;
			CurrentSessionObject = s;
			IsAdminSection = true;
		}

		/// <summary>
		/// Returns a reference to the current breadcrumbs object so you can call its methods without having to use "new Breadcrumbs()"
		/// </summary>
		public static Breadcrumbs Current { get { return new Breadcrumbs(); } }

		/// <summary>
		/// Override this if you want to add breadcrumb
		/// </summary>
		public virtual void InitBread() {
			if (IsAdminSection) {
				if (IncludeHome) {
					AddBreadcrumb(0, "Home");
					SetBreadcrumb(0, "Home", Web.Root);
				}
				AddBreadcrumb(1, "Admin");
				SetBreadcrumb(1, "Admin", DefaultUrl);
				if (HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToLower().Contains("admin/edit")||Web.PageFileName.ToLower().Contains("edit.aspx")) {
					//lost during edit, assume level 3, create level 2
#if MVC
					var tableName = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].Substring(0, HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToLower().IndexOf("admin/edit/"));
					tableName = tableName.Substring(tableName.LastIndexOf("/")+1);
#else
					var tableName = Web.PageFileName.Substring(0, Web.PageFileName.ToLower().IndexOf("edit.aspx"));
#endif
					var newlabel = Fmt.SplitTitleCase(tableName).Plural();
					AddBreadcrumb(2, newlabel );
#if MVC
					SetBreadcrumb(2, newlabel, Web.ResolveUrl("~/admin/") + tableName + "Admin/");
#else
					SetBreadcrumb(2, newlabel, Web.ResolveUrl("~/admin/") + tableName + "List.aspx");
#endif

				}
			} else {
				AddBreadcrumb(0, "Home");
				SetBreadcrumb(0, "Home", DefaultUrl);
			}
		}

		public void AddBreadcrumb(int currentPageLevel, string currentPageName) {
			//' set the current page level, then adds a breadcrumb with the current url and the specified name
			if (Web.FullUrl.Contains("ExportButton")) {
				return;
			}
			int i;

			//check for lost session

			if (_currentLevel < currentPageLevel) {

				int test = (CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_currentPageLevel"] + "").ToInt(0);
				if (test == 0) {
					_currentLevel = currentPageLevel;
					InitBread();
				}


				_currentLevel = currentPageLevel;
				string pageURL, query;
				CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_currentPageLevel"] = currentPageLevel;
				CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_pageName_level" + currentPageLevel] = currentPageName;
				pageURL = CurrentRequestObject.ServerVariables["SCRIPT_NAME"];
				query = CurrentRequestObject.ServerVariables["QUERY_STRING"];
				if (query != "") pageURL = pageURL + "?" + query;
				CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_pageURL_level" + currentPageLevel] = pageURL;

				//' erase any lower level pages from trail
				i = currentPageLevel + 1;
				for (; ; ) {
					if (CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_pageURL_level" + i] + "" != "") {
						CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_pageURL_level" + i] = "";
						CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_pageName_level" + i] = "";
						i = i + 1;
					} else {
						break;
					}
				}
			}//else add bread at higher
		}
		public string GetBreadcrumb(int level) {
			string str = "";
			for (int i = level; i >= 0; i--)       // i not level
			{
				str = CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_pageURL_level" + i] + "";
				if (str != "") break;
			}
			if (str == "") {
				// MN 20100707 - this is often wrong for MVC apps - not sure if needed for WebForms though
				//str = "default.aspx";
			}
			return str;
		}

		public void SetBreadcrumb(int pageLevel, string pageName, string pageURL) {
			//' sets a breadcrumb at the specified page level without changing the current page level
			//' useful for setting top-level title and url
			//CurrentSessionObject[WebsiteBaseCodeName+"breadcumbs_currentPageLevel"] = pageLevel;
			CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_pageName_level" + pageLevel] = pageName;
			CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_pageURL_level" + pageLevel] = Web.ResolveUrlFull(pageURL);
		}

		/// <summary>
		/// Returns breadcrumb links separated by >
		/// </summary>
		public string GetBreadcrumbLinks() {
			return GetBreadcrumbLinks(" &gt; ");
		}

		/// <summary>
		/// Returns breadcrumb links separated by given separator
		/// </summary>
		public string GetBreadcrumbLinks(string separatorHtml) {
			return GetBreadcrumbLinks(separatorHtml, false);
		}

		/// <summary>
		/// Returns breadcrumb links separated by given separator and if outputAsListItems=true then wrap in LIs
		/// </summary>
		public string GetBreadcrumbLinks(string separatorHtml, bool outputAsListItems) {
			//' separatorHtml is usually " &gt; "
			string str;
			int i, currentPageLevel;
			str = "";
			currentPageLevel = (CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_currentPageLevel"] + "").ToInt(0);
			_currentLevel = currentPageLevel;
			for (i = 0; i <= currentPageLevel - 1; i++) {
				if (CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_pageURL_level" + i] + "" != "") {
					if (outputAsListItems) str += "<li>";
					str += "<a";
					if (IsAdminSection && i == 0 && CurrentSessionObject[WebsiteBaseCodeName + "admin" + WebsiteBaseCodeName] + "" != "") {
						//' special case to make main site always open in new window from admin section
						str += " target=\"_blank\"";
					}
					str += " href=\"" + CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_PageURL_level" + i] + "\">" + CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_PageName_level" + i] + "</a>" + separatorHtml;
					if (outputAsListItems) str += "</li>";
				}
			}
			if (outputAsListItems) str += "<li>";

			if (ShowCurrentBreadcrumbAsLink) {
				string label = CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_pageName_level" + currentPageLevel] + "";
				label = "<a href=\"" + CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_PageURL_level" + currentPageLevel] + "\">" + label + "</a>";						//20130820JN made current bread clickable FFS
				str += "<span>" + label + "</span>";
			} else {
				str += "<span>" + CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_pageName_level" + currentPageLevel] + "</span>";
			}

			if (outputAsListItems) str += "</li>";
			return str;
		}

		/*
		 * function GetBreadcrumbText(separatorHtml)
			' separatorHtml is usually " | "
			dim str, i, currentPageLevel
			str = ""
			currentPageLevel = session("breadcumbs_currentPageLevel")
			for i = 1 to currentPageLevel-1
				if session("breadcumbs_pageURL_level" & i) <> "" then
					str = str & session("breadcumbs_PageName_level" & i) & separatorHtml
				end if
			next
			GetBreadcrumbText = str & session("breadcumbs_pageName_level" & currentPageLevel)
		end function

		function GetBreadcrumbCurrentPageTitle()
			dim result
			result = session("breadcumbs_pageName_level" & session("breadcumbs_currentPageLevel"))
			GetBreadcrumbCurrentPageTitle = result
		end function 
			 */


		public string GetReturnPage() {
			string returnpage = Web.Request["df_returnpage"];
			if (returnpage.IsBlank()) {
				// try standard asp.net param returnUrl
				returnpage = Web.Request["returnUrl"];
			}
			if (returnpage.IsBlank()) {
				// try param used in delete urls
				returnpage = Web.Request["returnPage"];
			}
			if (returnpage.IsBlank()) {
				//' determine calling page to return back to
				returnpage = Web.Request.ServerVariables["http_referer"];
				if (returnpage.ContainsInsensitive("security/login") || returnpage.ContainsInsensitive("/login.aspx")) {
					returnpage = "";
				}
			}
			if (returnpage.IsBlank()) {
				returnpage = GetBreadcrumb(2);
			}
			if (returnpage.IsBlank()) {
				// find default page
				//' blank breadcrumb - locate default page
				returnpage = DefaultUrl;
			}
			return returnpage;
		}



		/// <summary>
		/// Determines the URL of the admin main menu. 
		/// If you want to change the default URL, you have two options:
		/// 1. override this method (useful if you need to do some logic)
		/// 2. simply set the property DefaultUrl (eg on startup such as in AdminAreaRegistration or in global.asax)
		/// </summary>
		/// <returns></returns>
		protected virtual string DetermineDefaultUrl() {
			string defaultUrl;
			if (IsAdminSection) {
				defaultUrl = Util.GetSetting("Beweb_Admin_Folder", "");
				if (defaultUrl.IsBlank()) {
					if (FileSystem.FileExists(Web.MapPath("~/admin/default.aspx"))) {
						// use admin/default.aspx
						defaultUrl = Web.ResolveUrl("~/admin/default.aspx");
					} else if (FileSystem.FileExists(Web.MapPath("~/areas/admin/Controllers/AdminMenuController.cs"))) {
						defaultUrl = Web.ResolveUrl("~/admin/AdminMenu");
					} else {
						// return to web root
						defaultUrl = Web.Root;
					}
				}
			} else {
				// not admin section, so assume homepage
				defaultUrl = Web.Root;
			}
			return defaultUrl;
		}

		public string GetCurrentPageName() {
			int currentPageLevel = (CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_currentPageLevel"] + "").ToInt(0);
			return CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_pageName_level" + currentPageLevel] + "";
		}

		public string GetParentPageName() {
			int currentPageLevel = (CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_currentPageLevel"] + "").ToInt(0);
			return CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_pageName_level" + (currentPageLevel - 1)] + "";
		}

		public string GetCurrentPageUrl() {
			int currentPageLevel = (CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_currentPageLevel"] + "").ToInt(0);
			return CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_PageURL_level" + currentPageLevel] + "";
		}

		public string GetParentPageUrl() {
			int currentPageLevel = (CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_currentPageLevel"] + "").ToInt(0);
			return CurrentSessionObject[WebsiteBaseCodeName + "breadcumbs_PageURL_level" + (currentPageLevel - 1)] + "";
		}
	}
}

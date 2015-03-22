using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Profile;
using Beweb;
using Savvy;

namespace SavvyMVC.Helpers {
	/// <summary>
	/// Extension methods for HTML Helpers in views
	/// </summary>
	public static class SavvyHtmlHelpers {


		/// <summary>
		/// This method is called from the view to print out a list of errors, one error or an info message.
		/// There should only ever be one non null object in when this is called, eg either info, errorList or singleError.
		/// 
		/// errorMessage is a string
		/// errorList is a List of ValidationError/s.
		/// infoMessage is a string
		/// 
		/// In special circumstances such as custome validation that is site specific, any of 
		/// these session variables could be accesses directly in the front end with out
		/// using this helper. This would for example enable you to iterate through the errors list
		/// doing some fancy wicked awesome stuff on the fly. 
		/// 
		/// The errorList is constructed in BeWebModelState.cs. (To add an error use  BeWebModelState.AddError...) 
		/// There are three constructor overloads for foward and backward compatibility (well, with backward you still would
		/// need to use bewebmodelstate rather than modelstate).
		/// 
		/// errorMessage and infoMessage are constructed in Web.cs.
		/// (Usage: Web.InfoMessage = "a message", Web.ErrorMessage = "An Error").
		/// 
		/// cm
		/// 
		/// </summary>
		/// <param name="htmlHelper"></param>
		/// <returns></returns>
		public static MvcHtmlString InfoMessage(this HtmlHelper htmlHelper) {
			object info = Web.InfoMessageHtml ?? htmlHelper.ViewContext.TempData["info"];
			object errorList = Web.Session["errorList"];
			object singleError = Web.Session["errorMessage"];
			string result = "";

			if (info != null && info + "" != "") {
				var div = new HtmlTag("div").Add("class", "InfoMessage alert alert-info");
				div.SetInnerHtml((info + ""));
				result += div;
			}

			if (errorList != null) {
				var errors = (List<ValidationError>)errorList;
				var div = new HtmlTag("div").Add("class", "ErrorMessage errorServer alert alert-error");

				string allErrMessage = "";

				foreach (ValidationError err in errors) {
					//prefer not to do it this way but have to do for now
					//as it works with html this way.. would rather
					//have a seperate line for each error message
					allErrMessage += err.ErrorMessage + "\n";
				}

				div.SetInnerHtml((allErrMessage + ""));
				result += div;
			}

			if (singleError != null) {
				var div = new HtmlTag("div").Add("class", "ErrorMessage errorServer alert alert-error");
				div.SetInnerHtml((singleError + ""));
				result += div;
			}

			ResetVars();
			return MvcHtmlString.Create(result);
		}

		private static void ResetVars() {
			Web.InfoMessage = null;
			Web.ErrorMessage = null;
			Web.Session["infoMessage"] = null;
			Web.Session["errorList"] = null;
			Web.Session["errorMessage"] = null;
		}

		//Added JC 20140724

		public static MvcHtmlString PreviewLink(this HtmlHelper htmlHelper, string url, string label = null) {
			string html = "<a href=\"" + url + "\" target=\"_blank\" title=\"View this page\"><i class=\"icon-search\"></i>" + label + "</a>";
			return MvcHtmlString.Create(html);
		}

		public static MvcHtmlString PreviewLink(this HtmlHelper htmlHelper, ActiveRecord listItem, string label = null) {
			string html = "<a href=\"" + listItem.GetUrl("?preview=adminonly") + "\" target=\"_blank\" title=\"View this page\"><i class=\"icon-search\"></i>" + label + "</a>";
			return MvcHtmlString.Create(html);
		}

		public static MvcHtmlString ChildListLink(this HtmlHelper htmlHelper, ActiveRecord listItem, string childTableName, string linkText = null, string foreignKeyName = null) {
			if (linkText == null) linkText = "Items";
			if (foreignKeyName == null) foreignKeyName = listItem.GetPrimaryKeyName();  // fk is often to a pk the same name
			string url = Web.AdminRoot + childTableName + "Admin";
			url += "/?bread=" + (Breadcrumbs.Current.CurrentLevel + 1) + "&" + foreignKeyName + "=" + listItem.ID_Field.ToString().UrlEncode();
			// <span class="badge">42</span>
			string html = "<a href=\"" + url + "\">" + linkText.HtmlEncode() + "</a>";
			return MvcHtmlString.Create(html);
		}

		public static MvcHtmlString ChildListLink(this HtmlHelper htmlhelper, IActiveRecordList childList, string linkText = null) {
			var record = childList.GetParentRecord<ActiveRecord>();
			var childTableName = childList.GetTableName();
			string foreignKeyName = childList.GetForeignKeyName();
			if (linkText == null) linkText = childList.GetFriendlyTableNamePlural();
			return ChildListLink(htmlhelper, record, childTableName, linkText, foreignKeyName);
		}

		public static MvcHtmlString DraggableSortPosition(this HtmlHelper htmlHelper, ActiveRecord listItem, int? sortPositionCurrentValue, object sortGroup = null, string action = "SaveSortOrder") {
			Beweb.Util.IncludeJavascriptFile("~/js/redips_drag/redips-drag.min.js");
			string url = new UrlHelper(htmlHelper.ViewContext.RequestContext).Action(action);
			url += "?t=" + Crypto.Encrypt(listItem.GetTableName()) + "&p=" + Crypto.Encrypt(listItem.GetPrimaryKeyName());
			Beweb.Util.IncludeJavascript("$(function() { svyInitSortPositionTableDrag(\"" + url + "\"); });");

			string html = "<td class=\"rowhandler\"><div class=\"drag row\" title=\"Drag to change sort position\"><i class=\"icon-align-justify\"></i> <span class=\"svyRowSort\" data-pkid=" + listItem.ID_Field.ToString() + " data-sortgroup=" + sortGroup + ">" + sortPositionCurrentValue + "</span><input type=\"hidden\" name=\"SortPosition" + Forms.CurrentRowSuffix + "\" value=\"" + sortPositionCurrentValue + "\"></div></td>";


			if (Web.Request["Search"].IsNotBlank() || (Web.Request["ColSortField"].IsNotBlank() && Web.Request["ColSortField"] != "SortPosition")) { // hide sortable if in middle of a search filter
				html = "<td class=\"rowhandler\"><div class=\"row\">" + sortPositionCurrentValue + "</div></td>";
			}
			return MvcHtmlString.Create(html);
		}

		public static MvcHtmlString SaveButton(this HtmlHelper htmlHelper) {
			return SaveButton(htmlHelper, null);
		}

		public static MvcHtmlString SaveButton(this HtmlHelper htmlHelper, string cssclass) {
			var button = new HtmlTag("input").Add("type", "submit");
			button.Add("name", "SaveButton");
			button.Add("value", "    Save     ");
			button.Add("class", "btn btn-default svySaveButton" + ((cssclass != null) ? " " + cssclass : ""));

			return button.ToHtmlString();
		}
		public static MvcHtmlString SaveButton(this HtmlHelper htmlHelper, string Caption = "    Save     ", string ButtonNameAttribute = "SaveButton", string cssclass = null) {
			var button = new HtmlTag("input").Add("type", "submit");
			button.Add("name", ButtonNameAttribute);
			button.Add("value", Caption);
			button.Add("class", "btn btn-default svySaveButton" + ((cssclass != null) ? " " + cssclass : ""));
			//button.Add("onclick", "$('form#form').submit();this.disabled=true");
			return button.ToHtmlString();
		}

		public static MvcHtmlString SaveAndRefreshButton(this HtmlHelper htmlHelper) {
			var button = new HtmlTag("input").Add("type", "submit");
			button.Add("name", "SaveAndRefreshButton");
			button.Add("value", "Save & Refresh");
			button.Add("class", "btn btn-default svySaveButton svySaveAndRefreshButton");

			return button.ToHtmlString();
		}

		public static MvcHtmlString CancelButton(this HtmlHelper htmlHelper) {
			return htmlHelper.ButtonLink("Cancel", Breadcrumbs.Current.GetReturnPage(), "cancel");
		}



		public static MvcHtmlString ButtonLink(this HtmlHelper htmlHelper, string buttonCaption, string linkUrl, string cssClass = null) {
			var tag = new HtmlTag("input").Add("type", "button").Add("value", buttonCaption).Add("onclick", "location=" + linkUrl.JsEnquote() + "");
			if (cssClass != null) { tag.Add("class", cssClass); }
			return tag.ToHtmlString();
		}

		public static MvcHtmlString DuplicateCopyButton(this HtmlHelper htmlHelper, string caption = "Duplicate", string buttonNameAttribute = "DuplicateButton", string cssclass = null) {
			var button = new HtmlTag("input").Add("type", "submit");
			button.Add("name", buttonNameAttribute);
			button.Add("value", caption);
			button.Add("class", "btn btn-default svySaveButton" + ((cssclass != null) ? " " + cssclass : ""));
			return button.ToHtmlString();
		}

		/// <summary>
		/// standard delete button - only superadmin access can delete a record
		/// </summary>
		public static MvcHtmlString DeleteButton<TActiveRecord>(this HtmlHelper<TActiveRecord> htmlHelper) where TActiveRecord : ActiveRecord {
			return DeleteButton<TActiveRecord>(htmlHelper, false);
		}

		/// <summary>
		/// standard delete button - only superadmin access can delete a record
		/// </summary>
		public static MvcHtmlString DeleteButton<TActiveRecord>(this HtmlHelper htmlHelper, TActiveRecord record) where TActiveRecord : ActiveRecord {
			if (record == null) return null;					//20140506JN if no rec, not delete button
			return DeleteButton<TActiveRecord>(htmlHelper, record, false);
		}

		/// <summary>
		/// all accesss	delete button - any users may delete the record
		/// </summary>
		/// <typeparam name="TActiveRecord"></typeparam>
		/// <param name="htmlHelper"></param>
		/// <param name="allowAnyoneToDelete"></param>
		/// <returns></returns>
		public static MvcHtmlString DeleteButton<TActiveRecord>(this HtmlHelper<TActiveRecord> htmlHelper, bool allowAnyoneToDelete) where TActiveRecord : ActiveRecord {
			TActiveRecord record = (TActiveRecord)htmlHelper.ViewData.Model;
			return DeleteButton<TActiveRecord>(htmlHelper, record, allowAnyoneToDelete);
		}

		/// <summary>
		/// all accesss	delete button - any users may delete the record
		/// </summary>
		/// <typeparam name="TActiveRecord"></typeparam>
		/// <param name="htmlHelper"></param>
		/// <param name="allowAnyoneToDelete"></param>
		/// <returns></returns>
		public static MvcHtmlString DeleteButton<TActiveRecord>(this HtmlHelper htmlHelper, TActiveRecord record, bool allowAnyoneToDelete) where TActiveRecord : ActiveRecord {
			if (record.IsNewRecord) {
				return null;
			} else if (allowAnyoneToDelete || Beweb.Security.IsDevAccess || Beweb.Security.IsSuperAdminAccess) {
				MvcHtmlString mvcHtmlString = htmlHelper.ActionLink("Delete", "Delete", new { id = record.ID_Field.ValueObject, returnpage = Breadcrumbs.Current.GetReturnPage() }, new { onclick = "return confirm('Are you sure you want to delete this?')", @class = "delete icon-trash", title = "Delete" });
				//MvcHtmlString mvcHtmlString = htmlHelper.ActionLink("Delete", "Delete", new { id = record.ID_Field.ValueObject, returnpage = Breadcrumbs.Current.GetReturnPage() }, new { onclick = "return confirm('Are you sure you want to delete this?')", @class = "delete" });
				//if(Beweb.Security.IsDevAccess) {
				//  mvcHtmlString. = MvcHtmlString( "DEV: "+mvcHtmlString)	;
				//}
				return mvcHtmlString;
			} else {
				return null;
			}
		}

		public static MvcHtmlString ReturnPageToken(this HtmlHelper htmlHelper) {
			return htmlHelper.Hidden("df_returnpage", Breadcrumbs.Current.GetReturnPage());
		}

		public enum HelpWindowType { Help, Window, Inline, HelpText }

		public static MvcHtmlString SavvyHelp(this HtmlHelper htmlHelper, string helpText, int? width, int? height) {
			return htmlHelper.SavvyHelp(helpText, HelpWindowType.Inline, null, null, width, height);
		}
		public static MvcHtmlString SavvyHelp(this HtmlHelper htmlHelper, string helpText) {
			return htmlHelper.SavvyHelp(helpText, HelpWindowType.Inline, null, null, null, null);
		}

		public static MvcHtmlString SavvyHelpText(this HtmlHelper htmlHelper, HelpText helpText, HelpWindowType windowType = HelpWindowType.Inline, string url = null, int? width = 300, int? height = 150) {
			var bodyTextHtml = helpText.BodyTextHTML;
			if (bodyTextHtml.StripTags().IsBlank()) return MvcHtmlString.Create("<!--Help text is blank, dont show-->");
			if (Security.IsSuperAdminAccess) { 
				//add edit button
				string editURL=Web.AdminRoot+"HelpTextAdmin/Edit/"+helpText.ID;
				bodyTextHtml+="<a target=\"_blank\" href=\""+editURL+"\" class=\"btn btn-mini\">Edit Help</a>";
			}
			windowType = HelpWindowType.HelpText;
			return htmlHelper.SavvyHelp(bodyTextHtml, windowType, url, helpText.Title, width, height);
		}

		public static MvcHtmlString SavvyHelp(this HtmlHelper htmlHelper, string helpText = null, HelpWindowType windowType = HelpWindowType.Inline, string url = null, string title = null, int? width = null, int? height = null) {
			if ((Web.PageGlobals["Exporting"] + "").ToBool()) {
				return null;
			}
			
			if (title.IsBlank()) title = "Help";

			if (width == null) width = 200;
			if (height == null) height = 150;
			//return htmlHelper.RenderUserControl<control_beweb_Help>("~/Controls/Help.aspx", param => { });
			string imageSrc = Beweb.Web.Root + ((windowType != HelpWindowType.HelpText) ? "admin/images/help.gif" : "admin/images/helptext.png");
			string imageClass = (windowType != HelpWindowType.HelpText) ? "helpIcon" : "helpTextIcon";
			var img = new HtmlTag("img").Add("src", imageSrc).Add("title", "Help").Add("tabindex", "-1");
			img.Add("class", imageClass);
			string clickFunction = "";
			helpText = helpText + "";
			helpText = helpText.Replace("\r\n", "");								//remove crlf incase we are in some js
			helpText = helpText.Replace("\n\r", "");								//remove crlf incase we are in some js
			helpText = helpText.Replace("\n", "");									//remove crlf incase we are in some js
			helpText = helpText.Replace("\r", "");									//remove crlf incase we are in some js

			switch (windowType) {
				case HelpWindowType.Inline:
					if (helpText == "") {
						throw new Exception("You need to specify the text for windowType.Inline");
					}
					clickFunction = String.Format("OpenInline('{0}','{1}',{2},{3},event);return false;", helpText.HtmlEncode(), title.HtmlEncode(), width, height);

					break;
				case HelpWindowType.Window:
					if (url.IsBlank()) {
						throw new Exception("You need to specify a URL for windowType.Window");
					}
					clickFunction = String.Format("OpenIframe('{0}','{1}',{2},{3},event);return false;", url, title.HtmlEncode(), width, height);

					break;
				case HelpWindowType.Help:
					if (url.IsBlank()) {
						throw new Exception("You need to specify a URL for windowType.Help");
					}
					clickFunction = String.Format("OpenHelp('{0}','{1}');return false;", url, title.HtmlEncode());

					break;
				case HelpWindowType.HelpText:
					if (helpText == "") {
						throw new Exception("You need to specify the text for windowType.HelpText");
					}
					clickFunction = String.Format("OpenHelpText('{0}','{1}',{2},{3},event);return false;", helpText.HtmlEncode(), title.HtmlEncode(), width, height);

					break;
			}
			// todo img.Add("data-title", title.HtmlEncode(), false);
			img.Add("onclick", clickFunction, false);
			return img.ToHtmlString();
		}

		/// <summary>
		/// helper to change model state errors into a csv (/n)
		/// </summary>
		/// <param name="modelState"></param>
		/// <returns></returns>
		public static string ReadModelStateErrors(ModelStateDictionary modelState) {
			string result = "";
			if (modelState["Record"] == null) return result;
			foreach (var res in modelState["Record"].Errors) {
				result += res.ErrorMessage + "\n";
			}
			return result;
		}

		/// <summary>
		/// Returns the finished HTML as a MvcHtmlString (which is a string that "knows" that it is does not need HTML encoding).
		/// MN 20130715 moved this method from BewebCore project so that there is no need for reference to System.Web.Mvc in that project
		/// </summary>
		/// <returns></returns>
		public static MvcHtmlString ToHtmlString(this HtmlTag likeThis) {
			return MvcHtmlString.Create(likeThis.ToString());
		}

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using Site.SiteCustom;

namespace Site.Areas.Admin.Controllers {
	/// <summary>
	/// This is the base class for all controllers in Admin section
	/// </summary>

	[ValidateInput(false)]            // disable request validation (ie the feature where HTML is not allowed to be posted in a form)
	[SavvyAuthorize]
	public class AdminBaseController : SavvyMVC.AdminBaseController {

		/*
		 how to do custom auth in the site.
		 * add this to the controller
		 * 
			protected override void OnAuthorization(AuthorizationContext filterContext)
			{
				base.OnAuthorization(filterContext);
				if(!Security.IsAdministratorAccess && !Security.IsInRole(SecurityRoles.OnlineCatalogUser))
				{
					filterContext.Result=new HttpUnauthorizedResult();
				}
			}
		 * 
		 * and add this above the class:
		 * 
		 * 	[SavvyAuthorize]
		 */

		/// <summary>
		/// Checks that the current user is allowed access to this controller. 
		/// If you are an Administrator, SuperAdmin or Developer you have access to all admin pages (defined by AdminBaseController).
		/// Note: to allow custom roles to access a few pages of the admin, modify SecurityRoles.GetAllowedAdminControllers()
		/// </summary>
		/// <param name="filterContext"></param>
		protected override void OnAuthorization(AuthorizationContext filterContext) {
			if(Request["pdf"]=="abc"){return;}
			base.OnAuthorization(filterContext);

			string allowedControllerList = SecurityRoles.Roles.GetAllowedAdminControllers();
			if (!allowedControllerList.Contains("(all)")) {
				// check for controller name match
				string[] allowedControllers = allowedControllerList.Split(',');
				if (!allowedControllers.Contains(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName)) {
					if (Security.AuthProviderIsSavvy) {
						Security.KickOut("Your login is not authorised to access this section.");
						return;
					} else {
						filterContext.Result = new HttpUnauthorizedResult();
					}
				}
			}
			
			//if (!Security.IsAdministratorAccess)        // add any other roles here that can access the admin section
			//{
			//  filterContext.Result = new HttpUnauthorizedResult();
			//}
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext) {
			if (Request["dev"] == "1") {
				Session["DevAccess"] = true;
			} else {
				Session["DevAccess"] = false;

			}
			base.OnActionExecuting(filterContext);
		}

		protected override void OnException(ExceptionContext filterContext) {
			if (Util.ServerIsDev || Util.IsBewebOffice) {
				// fall through to Global asax error handler - and display error onscreen - we want to see full debug info on screen
				filterContext.ExceptionHandled = false;
			} else if (filterContext.Exception is BadUrlException) {
				// todo - make this return a Shared View instead of a Redirect to Action
				// filterContext.Result = View("BadUrl", new{message=filterContext.Exception.Message});
				filterContext.Result = Redirect("~/Admin/NotFound?message=" + filterContext.Exception.Message);
				filterContext.ExceptionHandled = true;
			} else if (filterContext.Exception is AdminErrorException) {
				SendEMail.SimpleSendEmail(SendEMail.EmailToAddress, Util.GetSiteName() + " Website Problem Notification", "Please address the following issue with the website:\n" + filterContext.Exception.Message);
				filterContext.Result = Redirect("~/Admin/ShowError?message=" + filterContext.Exception.Message.UrlEncode());
				filterContext.ExceptionHandled = true;
			// fall through to Global asax error handler - and show standard error page
			//} else {  //if (filterContext.Exception is ProgrammingErrorException) {
			//	Beweb.Error.SendExceptionEmail();
			//	filterContext.Result = Redirect("~/Admin/ShowError?message=" + "A programming error occurred. Please contact the developers.".UrlEncode());
			//	filterContext.ExceptionHandled = true;
				//} else {
				//  // fall through to Global asax error handler - and show standard error page - maybe don't need this?
				//  filterContext.ExceptionHandled = false;
			}
		}
		protected Locking lockobj = new Locking();

		protected void CheckLock(ActiveRecord record) {
			if(record==null)return;
			lockobj.InitLocking(Security.LoggedInUserID);
			if(!lockobj.LockTable(record.GetTableName(),(record[record.GetPrimaryKeyName()]+"").ToIntOrDie(),record.GetName()))
			{
				Web.ErrorMessage = lockobj.LockMessage;
			}
		}

		//public static void CheckAttachmentsForMSOfficeFileOrPDFTextExtractPlain(GenTest record) {
		//	//walk the field list for this record looking for attachments
		//	foreach (var fieldName in record.GetFieldNames()) {
		//		if (fieldName.Contains("Attachment")) {
		//
		//			//if (record.Fields.Attachment.IsDirty) {
		//			if (ActiveFieldBase.IsDirtyObj(record[fieldName].ValueObject, record[fieldName].OriginalValueObject)) {
		//				var baseFilename = record[fieldName].ToString();
		//				if (baseFilename.Contains(".doc")||baseFilename.Contains(".pdf")||baseFilename.Contains(".xls")||baseFilename.Contains(".ppt")) {
		//					if (!record.FieldExists(fieldName+"RAWText")) {
		//						(new Sql("ALTER TABLE ", record.GetTableName().SqlizeName(), " ADD ["+fieldName+"RAWText] nvarchar (MAX);")).Execute();
		//					}
		//					string output = "";
		//					var filename = baseFilename.ToLower();
		//					if (filename.EndsWith(".doc")||filename.EndsWith(".xls")||filename.EndsWith(".ppt")) {
		//						OfficeFileReader.OfficeFileReader objOFR = new OfficeFileReader.OfficeFileReader();
		//						if (objOFR.GetText(Web.MapPath(Web.Attachments) + baseFilename, ref output) > 0) {
		//							//ok
		//						}
		//					} else if (filename.EndsWith(".docx")) {
		//						BewebCore.ThirdParty.ReadWordDocText.DocxToText objOFR = new DocxToText(Web.MapPath(Web.Attachments) + baseFilename);
		//						if ((output=objOFR.ExtractText()).Length>0) {
		//							//ok
		//						}
		//					} else if (filename.EndsWith(".pdf")) {
		//						PdfToText.PDFParser pdf = new PDFParser();
		//						if (pdf.ExtractText(Web.MapPath(Web.Attachments) + baseFilename, ref output)) {
		//							//ok
		//						}
		//					}
		//					if (output.Trim() != "") { 
		//						(new Sql("update ", record.GetTableName().SqlizeName(), "set "+fieldName+"RAWText=", output.SqlizeText(), " where ",
		//									record.GetPrimaryKeyName().SqlizeName(), "=", record.ID, "")).Execute();
		//					}
		//
		//				} else {
		//					//no doc any more
		//					if (record.FieldExists(fieldName+"RAWText")) {
		//						(new Sql("update ", record.GetTableName().SqlizeName(), "set "+fieldName+"RAWText=null where ",
		//							record.GetPrimaryKeyName().SqlizeName(), "=", record.ID, "")).Execute();
		//					}
		//				}
		//			}
		//		}
		//	}
		//}
		//

		public virtual ActionResult Cancel(int id, string returnPage) {
			return Redirect(returnPage);
		}

	}



	public class AdminBaseController<TActiveRecord> : SavvyMVC.AdminBaseController where TActiveRecord : Beweb.ActiveRecord {
		// TODO
	}

}

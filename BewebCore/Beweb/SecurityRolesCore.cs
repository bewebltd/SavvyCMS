using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using Beweb;

namespace Beweb {
	/// <summary>
	/// Summary description for Security
	/// </summary>
	public class SecurityRolesCore {
		public static SecurityRolesCore Roles {
			get { return new SecurityRolesCore(); }
		}

		public string ADMINISTRATOR = "administrators";
		public string SUPERADMIN = "superadmins";		 //override this in globalasax.cs app start
		public string DEVELOPER = "developers";
		// do not add site-custom roles here - instead, add them in Site.SiteCustom.SecurityRoles

		public static Forms.SelectOptions GetRoleDropDownOptions() {
			var options = new Forms.SelectOptions();
			options.Add(Roles.ADMINISTRATOR, "Administrator");
			if (Security.IsSuperAdminAccess) {
				options.Add(Roles.ADMINISTRATOR + "," + Roles.SUPERADMIN, "SuperAdmin");
			}
			if (Security.IsDevAccess) {
				options.Add(Roles.ADMINISTRATOR + "," + Roles.SUPERADMIN + "," + Roles.DEVELOPER, "Developer");
			}
			// do not add site-custom roles here - instead, add them in Site.SiteCustom.SecurityRoles
			return options;
		}

		/// <summary>
		/// Don't call this directly - call it in Site.SiteCustom.SecurityRolesCustom
		/// This method is called by AdminBaseController to check that the current user is allowed access to this controller. 
		/// If you are an Administrator, SuperAdmin or Developer you have access to all admin pages.
		/// Note: to allow custom roles to access a few pages of the admin, modify SecurityRoles.GetAllowedAdminControllers()
		/// </summary>
		/// <returns></returns>
		public virtual string GetAllowedAdminControllers() {
			// do not add site-custom roles here - instead, add them in Site.SiteCustom.SecurityRoles
			if (Security.IsAdministratorAccess) {
				return "(all)";
			} else {
				// anyone else has no access
				return "";
			}
		}

		public bool IsAllowedAdminController(string controllerName) {
			var allowedControllerList = GetAllowedAdminControllers();
			return (allowedControllerList.Contains("(all)") || allowedControllerList.ContainsCommaSeparated(controllerName));
		}

		/// <summary>
		/// Don't call this directly - call it in Site.SiteCustom.SecurityRolesCustom
		/// </summary>
		public virtual string[] GetAllRoles() {
			return new string[] { Roles.ADMINISTRATOR, Roles.SUPERADMIN, Roles.DEVELOPER };
		}

		public virtual bool IsExludedLoginRole(Hashtable authenticatedPersonDetails) {
				return false;
		}

	}

}


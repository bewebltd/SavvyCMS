using System.Collections;
using Beweb;
using Models;

namespace Site.SiteCustom {
	/// <summary>
	/// Summary description for Security
	/// </summary>
	public class SecurityRoles : Beweb.SecurityRolesCore {
		public static new SecurityRoles Roles {
			get { return new SecurityRoles(); }
		}

		// by default "administrators" and "developers" are defined in the base class;
		// custom roles should be added here...

		// TODO - add any custom roles here 
		//public string TEACHER = "teachers";
		//public string EXAMINER = "examiner";
#if PageRevisions
		public string EDITOR = "editors";
#endif

		// note: the method we use above is not compatible with Authorise Attributes. If you use them, you may be better to use const instead (as example below), as this is accessable from within authorise attribute - it depends if you are using Authorise Attributes or not
		//public const string OnlineCatalogUser = "onlinecataloguser";
		
		public new static Forms.SelectOptions GetRoleDropDownOptions() {
			var options = new Forms.SelectOptions();
			// TODO - add any custom roles here 
			// some role options should include multiple roles, eg Teacher might also include Administator, whereas Student might not
			//options.Add(Roles.ADMINISTRATOR + "," + Roles.TEACHER, "Teacher");
			//options.Add(Roles.STUDENT, "Student");
#if PageRevisions
			if (Settings.All.EnableWorkflow) {
				options.Add(Roles.EDITOR, "Editor");
			}
#endif
			//options.Add(Roles.ADMINISTRATOR + "," + Roles.APPROVALSSUPERADMINISTRATOR, "Approvals Super Administrator");
			options.Add(SecurityRolesCore.GetRoleDropDownOptions());
			return options;
		}

		public override string[] GetAllRoles() {
			return base.GetAllRoles();
//			string[] result = base.GetAllRoles();
//			string[] locals = new string[]{TEACHER};
//			return locals.AppendArray(result);
		}

		public override bool IsExludedLoginRole(Hashtable authenticatedPersonDetails) {
			/* add roles to exclude here:
			if (authenticatedPersonDetails["StaffType"].ToString() == SecurityRoles.Roles.NEWSLETTERONLY) {
				return true;
			}
			*/
			return false;
		}

		/// <summary>
		/// Checks that the current user is allowed access to this controller. 
		/// If you are an Administrator, SuperAdmin or Developer you have access to all admin pages (defined by AdminBaseController).
		/// Note: to allow custom roles to access a few pages of the admin, modify SecurityRoles.GetAllowedAdminControllers()
		/// </summary>
		/// <returns></returns>
		public override string GetAllowedAdminControllers() {
#if PageRevisions
			if (Security.IsUserInRoleOnly(UserSession.Role, Roles.EDITOR)) {
				return "AdminMenu,PageAdmin,CommonAdmin";
			}else
#endif
			
			if(false){ //remove this line if you uncomment the ones below
			//} else if (Security.IsInRole(Roles.TEACHER)) {
			//	return "AdminMenu,CommonAdmin,Course,Student"; // Note: Always include CommonAdmin
			//} else if (Security.IsInRole(Roles.STUDENT)) {
			//	return "AdminMenu,MyDetails";
			} else if (Security.IsAdministratorAccess) {
				return "(all)";
			} else {
				// anyone else has no access
				return "";
			}
		}
		
		public string GetAllowedSiteControllers() {
			return "(all)";// if not frontend login show all content
			if (Security.IsAdministratorAccess) {
				return "(all)";
			} else if (Security.IsInRole(Roles.DEVELOPER)) {
				return "(all)";
			//} else if (Security.IsInRole(Roles.STUDENT)) {
			//	return "AdminMenu,MyDetails";
			} else {
				// anyone else has no access
				return "(none)";
			}

		}

	}

}




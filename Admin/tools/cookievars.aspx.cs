//
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Beweb;

namespace Beweb.Admin {
	/// <summary>
	/// Summary description for servervars.
	/// </summary>
	public partial class CookieVars : System.Web.UI.Page {
		protected void Page_Load(object sender, System.EventArgs e) {
			Security.RequireLogin(SecurityRolesCore.Roles.DEVELOPER);
		}
	}
}

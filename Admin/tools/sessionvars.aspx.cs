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

namespace Beweb.Admin
{
	/// <summary>
	/// Summary description for servervars.
	/// </summary>
	public partial class SessionVars : System.Web.UI.Page
	{
		protected  void Page_Load(object sender, EventArgs e)
		{
			Security.RequireLogin(SecurityRolesCore.Roles.DEVELOPER);

			if(Request["ses_action"]=="save")
			{
				foreach(string req in Request.Form.Keys)
				{
					if(req!="__VIEWSTATE" && req.IndexOf("ses_")==-1)
					{
						Session[req] = Request[req];
					}else if(req=="ses_keyname")
					{
						if(Request[req]!=null &&Request[req]!="")Session[Request[req]] = Request["ses_newdata"];
					}
				}
			}else if(Request["ses_action"]=="abandon")
			{
				Session.Abandon();
			}else
			{
				if(Request["ses_action"]!=null && Request["ses_action"]!="" && Request["ses_action"].IndexOf("del:")==0)
				{
					Session.Remove(Request["ses_action"].Substring(4));
				}
			}
		}
	}
}

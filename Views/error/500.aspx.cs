using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class error_500 : System.Web.Mvc.ViewPage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		// real status - important to have this!! 
		Response.Status = "500 Internal Server Error";

		bool emailNak = (Request["emailNAK"] == "1");
		if (Request["g"] == "1")
		{
			// g = 1 is GOOD
			// redirect from global.asax - email already sent
		}
		else
		{
			// Note: could send an email from here?
		}

		if(emailNak)
		{
			EmailMessage.Visible = true;
			ErrorMessage.Text = String.Format("<p>Error message: {0}</p>", Server.HtmlEncode(Request.QueryString["em"]));
			ErrorMessage.Visible = true;
			ServerIs.Text = String.Format("<p>Server is: {0}</p>", Beweb.Util.ServerIs());
			ServerIs.Visible = true;
		}
	}
}

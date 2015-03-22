using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class error_404 : System.Web.Mvc.ViewPage 
{
	protected void Page_Load(object sender, EventArgs e)
	{
		// real status - important to have this!! 
		Response.Status = "404 Not Found";
		
		bool emailNak = (Request["emailNAK"] == "1");
		if(Request["g"] == "1")
		{
			// redirect from global.asax - don't send email
		}
		else
		{
			
		}
		
		EmailMessage.Visible = emailNak;

	}
}

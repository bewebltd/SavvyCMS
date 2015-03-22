using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class error_502 : System.Web.Mvc.ViewPage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		// real status - important to have this!! 
		Response.Status = "502 Bad Gateway";
	}
}

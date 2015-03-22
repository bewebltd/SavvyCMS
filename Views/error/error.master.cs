using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class error_errormaster : System.Web.Mvc.ViewMasterPage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		Response.TrySkipIisCustomErrors = true;

		//ErrorTimeStamp.Text = DateTime.Now.ToString("dd MMM yyyy HH:mm:ss"); -- MN 20131107 this is old
	}
}

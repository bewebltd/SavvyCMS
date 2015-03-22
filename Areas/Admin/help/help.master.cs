using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class admin_help_help : System.Web.UI.MasterPage
{
	public int x = 0;
	public int y = 0;
	protected void Page_Load(object sender, EventArgs e)
	{
		if(Request["x"]+"" != "" && Request["y"]+"" != "")
		{
			x = Convert.ToInt32(Request["x"]);
			y = Convert.ToInt32(Request["y"]);
		}
	}
}

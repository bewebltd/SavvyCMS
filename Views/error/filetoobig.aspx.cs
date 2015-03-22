using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class filetoobig : System.Web.Mvc.ViewPage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		ThisFileSize.Text = Beweb.Fmt.FileSize(Request.QueryString["this"], 2);
		MaxFileSize.Text = Beweb.Fmt.FileSize(Request.QueryString["max"], 2);
	}
}

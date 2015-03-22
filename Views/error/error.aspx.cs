using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class error_error : System.Web.Mvc.ViewPage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		// clean up the code - it can only contain numbers
		Regex r1 = new Regex("[^0-9]", RegexOptions.Multiline);
		string code = r1.Replace(Request["code"], "");

		// real status - important to have this!! 
		try
		{
			Response.Status = code;
		}	catch(Exception exc)
		{
			Message.Text += "Failed to set Response.Status to code["+code+"]" ;
			Trace.Write(exc.Message);
			Trace.Write("stack:"+exc.StackTrace);
		}
		
		Message.Text += "Error " + code;
		if (!String.IsNullOrEmpty(Request["emailNAK"]) && Request["emailNAK"] == "1")
		{
			Message.Text +=
				@" There has been an error, please configure the mail host correctly to view the details.<br>
									This issue will be resolved shortly, please check back again later.";
		}
		else
		{
			Message.Text +=
				@" There has been an error, details of what has just happened have been sent to an administrator.<br>
									This issue will be resolved shortly, please check back again later.";
		}
	}
}

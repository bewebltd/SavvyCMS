using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Beweb;

public partial class admin_tools_InitialUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
			Security.RequireLogin(SecurityRolesCore.Roles.DEVELOPER);
		// only show the form if there are no users in the system!
    	int countUsers = Convert.ToInt32(BewebData.GetValue(new Sql("SELECT COUNT(PersonId) FROM Person")));
		if(countUsers == 0)
		{
			ShowForm.Visible = true;
			DoneMessage.Visible = false;
		}
		else
		{
			ShowForm.Visible = false;
			DoneMessage.Visible = true;
		}
    }

	protected void AddInitialUser(object sender, EventArgs e)
	{
		if(IsPostBack)
		{
			Beweb.Security.CreateInitialUser(username.Text, password.Text);
		}
	}
}
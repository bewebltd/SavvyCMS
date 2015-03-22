using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Beweb;

namespace Savvy
{
	/// <summary>
	/// Summary description for ListPage
	/// </summary>
	public class ListPage : System.Web.UI.Page
	{		
		protected virtual void Page_Load(object sender, EventArgs e)
		{
			//BewebData.StructureCheck();
		}
		
		protected virtual void Page_PreRender(object sender, EventArgs e)
		{
		}
	}
}
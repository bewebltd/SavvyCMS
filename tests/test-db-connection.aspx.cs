using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Beweb;

public partial class tests_test_db_connection : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		msg.Text+="Start<br/>";
		string cs = BewebData.GetConnectionString();
		//msg.Text+="cs ["+cs+"]<br/>";
		Beweb.DataBlock db = new Beweb.DataBlock(cs);
		db.OpenDB();
		msg.Text += "select count(*) as cnt from Person: [" + db.FetchValue("select count(*) as cnt from Person") + "]<br/>";

		db.CloseDB();
		msg.Text+="Done<br/>";
		
	}
}

using System;
using System.Data;
using Beweb;

public partial class admin_tools_PasswordGenerator : System.Web.UI.Page {
	protected void Page_Load(object sender, EventArgs e) {
			Security.RequireLogin(SecurityRolesCore.Roles.DEVELOPER);

	}

	protected string findnocrypt() {
		string result = "";
		if(Request["mode"]=="findnocrypt")
		{
			var data = (new Sql("select CustomerID,custentity_webpassword, email from customer ")).GetDataTable();
			Log("Start "+Fmt.DateTime(DateTime.Now)+"<br>");
			for(int scan=0;scan<data.Rows.Count;scan++)
			{
			  DataRow item = data.Rows[scan];
				string ps;
				string plainPassword = item["custentity_webpassword"]+"";
				Log(" ["+scan+"]----------- Found ["+item["email"]+"] - plainPassword ["+plainPassword+"]");
				if((ps = Security.DecryptPassword(plainPassword+""))==""){
					Log("- insecure ["+plainPassword+"]<br>");
					if(Request["ac"]=="encrypt"){
						var newps=Security.CreateSecuredPassword(plainPassword);
						Log("--change to ["+newps+"]<br>");
						var sql = new Sql("update customer set custentity_webpassword=",newps.Sqlize_Text()," where customerid=",(item["CustomerID"]).ToString().Sqlize_Text(),"");
						Log("--sqlis ["+sql+"]<br>");
						sql.Execute();
					}

				}else{
					Log("- secure ["+ps+"]<br>");
				}
				if(false&&scan>2215){
					
					Log("Stop after 15..<br>");
					break;
				}
			}
			Log("End "+Fmt.DateTime(DateTime.Now)+"<br>");

		}
		return result;
	}

	protected string findnocryptadmin() {
		string result = "";
		if(Request["mode"]=="findnocryptadmin")
		{
			var data = (new Sql("select personID,password, email from person ")).GetDataTable();
			Log("Start "+Fmt.DateTime(DateTime.Now)+"<br>");
			for(int scan=0;scan<data.Rows.Count;scan++)
			{
			  DataRow item = data.Rows[scan];
				string ps;
				string plainPassword = item["password"]+"";
				Log(" ["+scan+"]----------- Found ["+item["email"]+"] - plainPassword ["+plainPassword+"]");
				if((ps = Security.DecryptPassword(plainPassword+""))==""){
					Log("- insecure ["+plainPassword+"]<br>");
					if(Request["ac"]=="encrypt"){
						var newps=Security.CreateSecuredPassword(plainPassword);
						Log("--change to ["+newps+"]<br>");
						var sql = new Sql("update person set password=",newps.Sqlize_Text()," where personid=",(item["PersonID"]).ToString().Sqlize_Text(),"");
						Log("--sqlis ["+sql+"]<br>");
						sql.Execute();
					}

				}else{
					Log("- secure ["+ps+"]<br>");
				}
				if(false&&scan>2215){
					
					Log("Stop after 15..<br>");
					break;
				}
			}
			Log("End "+Fmt.DateTime(DateTime.Now)+"<br>");

		}
		return result;
	}

	private int numlog=0;
	private void Log(string msg) {
		Response.Write(msg);
		numlog++;
		if(numlog>100){Web.Flush();numlog=0;}
	}
}

using System;
using System.Text;

using Beweb;
public partial class ClearTables : System.Web.UI.Page {
	// common properties
	private StringBuilder resultMessage = new StringBuilder("");
	private string conn;
	protected void Page_Load(object sender, EventArgs e) {
			Security.RequireLogin(SecurityRolesCore.Roles.DEVELOPER);

		if (Util.ServerIsDev || Util.ServerIsStaging) {
			var tables = Util.GetSetting("SavvyActiveRecord_ClearData", "");

			foreach (string table in tables.Split("|")) {
				new Sql("delete from " + table + "").Execute();
				resultMessage.AppendLine(table + " table has been cleared successfully");
			}
			resultMessage.AppendLine();
			resultMessage.AppendLine("Specified Tables have all been cleared");
		} else {
			resultMessage.AppendLine("Too dodgy to run this on a live server. Do it manually.");
		}
		Response.Write(resultMessage.ToString().FmtPlainTextAsHtml()); // use this one for BC file compares
	}
}

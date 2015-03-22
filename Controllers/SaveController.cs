using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Site.Controllers;
using Site.SiteCustom;

namespace Site.Controllers {
	public class SaveController : ApplicationController {
		/// <summary>
		/// ajax call to save a value from a dynamic edit control on a list page
		/// </summary>
		/// <param name="tn"></param>
		/// <param name="rid"></param>
		/// <param name="cn"></param>
		/// <param name="vl"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Index(
				string tn//tablename		 encrypted
			, string rid//record id		 encrypted
			, string cn //col name		 encrypted
			, string vl//value					un-encrypted value
			) {
			var tablename = Crypto.Decrypt(tn);//tablename		
			var recordid = Crypto.DecryptID(rid);//record id		
			var colname = Crypto.Decrypt(cn);//col name		
			var value = vl;

			var data = new ActiveRecord(tablename, tablename + "ID");

			data.LoadData(new Sql("select * from ", tablename.SqlizeName(), " where " + tablename + "id=", recordid, ""));
			if (data != null) {							//rec exists
				var field = data.GetFieldByName(colname);
				SqlizedValue dbvalue;
				if (field.ColumnType.Equals("int",StringComparison.CurrentCultureIgnoreCase)) {
					dbvalue = value.ToIntOrDie();
				}else if (field.ColumnType.Equals("nvarchar",StringComparison.CurrentCultureIgnoreCase)) {
					dbvalue = value.SqlizeText();
				} else if (field.ColumnType.Equals("datetime", StringComparison.CurrentCultureIgnoreCase)) {
					dbvalue = value.SqlizeDate();
				} else {
					dbvalue = value.SqlizeText(); //not allowed, unrecognised type?
				}
				(new Sql("update ", tablename.SqlizeName(), "set ", colname.SqlizeName(), "=",dbvalue, " where " + tablename + "id=", recordid, "")).Execute();
			}
			return Content("OK");
		}
	}
}
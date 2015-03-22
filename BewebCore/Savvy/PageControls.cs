using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Beweb;


namespace Savvy
	{
	/// <summary>
	/// Summary description for PageControls
	/// </summary>
	public class PageControls
	{

		/// <summary>
		/// pageNum is current page (with 1 being the first page)
		/// 
		/// </summary>
		/// <param name="pageNum"></param>
		/// <param name="numPages"></param>
		/// <returns></returns>
		public static string PagingNav(int pageNum, int numPages) {
			string html = "";
			if (numPages > 1) {
				html += "<span class=pagingnav>";
				if (pageNum > 1) {
					// we are on the second (or later) page, display Back
					html += "<span class=pagingnav>";
					html += "<a class=pagingnav href=\"javascript:GoPageNum(" + (pageNum - 1) + ")\">&lt; Previous Page</a> | ";
				}

				if (pageNum < numPages) {
					// there are more items, display Next
					html += " | <a class=pagingnav href=\"javascript:GoPageNum(" + (pageNum + 1) + ")\">Next Page &gt;</a>";
				}
				html += "</span>";
			}
			return html;
		}

		public static int checkBoxCounter = 0;   // todo; this is probably not thread safe

		public PageControls()
		{
		}
			/// <summary>
			/// Write a checkbox and hidden to handle processing of the checkbox
			/// </summary>
			/// <param name="fieldName"></param>
			/// <param name="initialValue"></param>
			/// <param name="label"></param>
			/// <returns></returns>
		public static string WriteCheckBox(string fieldName, string initialValue, string label)
		{
			string result = "";

			int actualValue;
			string source = "" +
				"<script language=\"JavaScript\">" +
				"function CheckBoxClick" + checkBoxCounter + "(cb) " +
				"{" +
				"	var hfld = document.form[" + fieldName + "];" +
				"	if (cb.checked)" +
				"	{" +
				"		hfld.value = \"1\";" +
				"	} else" +
				"	{" +
				"		hfld.value = \"0\";" +
				"	}" +
				"}" +
				"</script>" +
				"" +
				"";

			result += source;

			if (VB.cbool(initialValue) || VB.IsNull(initialValue))
			{
				actualValue = 0;
			} else
			{
				actualValue = 1;
			}

			result += "<input type=hidden name='" + fieldName + "' value=" + actualValue + ">" + VB.crlf;
			result += "<input type=checkbox name='chbox_" + fieldName + "' id='chbox_" + fieldName + "'";
			if (actualValue == 1) result += " checked";
			result += " onclick='CheckBoxClick" + checkBoxCounter + "(this)'>";
			result += "<label for='chbox_" + fieldName + "'>" + label + "</label>";
			checkBoxCounter++;
			return result;
		}


		/// <summary>
		/// Given a sql statement where the first column is the data field and the secodn is the description, draw a set of option tags (call this within a select tag)
		/// </summary>
		/// <param name="cma">Savvy Page containing database connection to work with</param>
		/// <param name="sql">SQL statement where the first column is the data field and the secodn is the description</param>
		/// <param name="selectedValue">value to select if in the list</param>
		/// <returns></returns>
		
		public static string WriteDropDownOptions(string sql, string selectedValue)
		{
			string result = "";
			DataView rs;
			string value = "";
			string description = "";
			selectedValue = selectedValue + "";		// convert to string

			DataBlock dataObject = new DataBlock(BewebData.GetConnectionString());
			dataObject.OpenDB();
			rs = dataObject.CreateDataSource(sql);

			for (int scan = 0; scan < dataObject.RecordCount; scan++)
			{
				value = dataObject.GetValue(scan, 0) + "";				// convert to string
				description = dataObject.GetValue(scan, 1);
				result += "<option value=\"" + value + "\"";
				if (value == selectedValue)
				{
					result += " selected";
				}
				result += ">" + description + "</option>" + VB.crlf;
			}
			dataObject.CloseDB();
			return result;
		}

		public static string WriteOptionWithDescription(string value, int selectedValue, string description)
		{
			return WriteOptionWithDescription(value, ""+selectedValue, description);
		}

		public static string WriteOptionWithDescription(string value, string selectedValue, string description)
		{
			string result = "";
			result += "<option value=\"" + value + "\"";
			if (value == selectedValue)
			{
				result += " selected";
			}
			result += ">" + description + "</option>" + VB.crlf;
			return result;	
		}


		//public static string FmtSqlString(string dataValue)
		//{
		//	return (dataValue==null)?"0":dataValue.Replace("'", "''");
		//}
		public static string WriteOption(string optionValue, string selectedValue, string description)
		{
			string result = "";
			result += "<option ";
			if (selectedValue == optionValue) result += "selected ";
			result += "value=\"" + optionValue.HtmlEncode() + "\">" + description.HtmlEncode() + "</option>";

			return result;
		}

		//public static string WriteDropDownOptions(ActiveRecordList activeRecordList, string selectedValue) {
		//  string result = "";
		//  foreach (ActiveRecord record in activeRecordList) {
		//    string id = record.ID_Field.ValueObject.ToString();
		//    string description = record.GetName();
		//    result += WriteOption(id, selectedValue, description);
				
		//  }
		//}
	}
}
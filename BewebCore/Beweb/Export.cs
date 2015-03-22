using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Text;

namespace Beweb {

	public class Export {
		public static void ExportToExcel(Sql sql) {
			var exportFileName = Web.PageFileNameNoExtn;
			if (exportFileName.IsBlank()) exportFileName = "Export-"+Fmt.DateTimeCompressed(DateTime.Now);
			exportFileName += ".xls";
			ExportToExcel(sql, exportFileName);
		}

		public static void ExportToExcel(Sql sql, string filename) {
			StringBuilder allData = new StringBuilder();

			var reader = sql.GetReader();
			if (!reader.HasRows) {
				allData.AppendLine("No records");
			} else {
				// write field titles
				string str = "";
				for (int scan = 0; scan < reader.VisibleFieldCount; scan++) {
					str += "\t" + Fmt.SplitTitleCase(reader.GetName(scan));
				}
				allData.AppendLine(str.Trim());

				foreach (DbDataRecord row in reader) {
					// write field values
					str = "";
					for (int scan = 0; scan < reader.VisibleFieldCount; scan++) {
						string val = "";
						var datatype = reader.GetDataTypeName(scan).ToLower();
						if (datatype == "datetime") {
							if (row[scan].ToString().IsNotBlank()) {
								val = Fmt.DateTime((DateTime)row[scan]);
							}
						} else if (datatype == "bit") {
							if (row[scan].ToString().IsNotBlank()) {
								val = Fmt.YesNo(("" + row[scan]).ToLower());
							}
						} else {
							val = row[scan].ToString();
						}
						str += "\t" + TsvCleanup(val);
					}
					allData.AppendLine(str.Trim());
				}
			}
			reader.Close();
			reader.Dispose();
			ExportToExcel(allData.ToString(), filename);
		}

		public static void ExportToExcel(ActiveRecordList<ActiveRecord> list, string filename) {
			ExportToExcel(list.innerList, filename);
		}
		/// <summary>
		/// <![CDATA[example: Export.ExportToExcel(pl.Cast<ActiveRecord>(), filename); where pl is a PolicyList]]>
		/// </summary>
		/// <param name="list"></param>
		/// <param name="filename"></param>
		public static void ExportToExcel(IEnumerable<ActiveRecord> list, string filename) {
			ExportToExcel(list.ToList(), filename);
		}

		public static void ExportToExcel(List<ActiveRecord> list, string filename) {
			StringBuilder allData = new StringBuilder();
			if (list.Count == 0) {
				allData.AppendLine("No records");
			} else {
				// write field titles
				string str = "";
				foreach (var field in list[0].GetFields()) {
					str += "\t" + field.FriendlyName;
				}
				allData.AppendLine(str.Trim());

				foreach (var record in list) {
					// write field values
					str = "";
					foreach (var field in record.GetFields()) {
						str += "\t" + TsvCleanup(field.ToString());
					}
					allData.AppendLine(str.Trim());
				}
			}
			ExportToExcel(allData.ToString(), filename);
		}

		/// <summary>
		/// pass in 
		/// </summary>
		/// <param name="data"></param>
		/// <param name="filename"></param>
		public static void ExportToExcel(string data, string filename) {
			Web.Response.BufferOutput = true;
			Web.Response.ClearHeaders();
			Web.Response.ClearContent();
			Web.SetHeadersForExcel(filename);
			// TODO - write in chunks for fast downloading
			Web.Write(data);
			Web.End();
		}

		public static string TsvCleanup(string val) {
			if (val != "") {
				val = val.Replace("\r\n", ", ").Trim(); // replace line feed with comma
				val = val.Replace("\n", ", ").Trim(); // replace line feed with comma
				val = val.Replace("\t", " ").Trim(); // replace tabs with space
			}
			return val.Trim();
		}

	}
}

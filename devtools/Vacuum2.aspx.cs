using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Beweb;

namespace Site.devtools {
	public partial class Vacuum2 : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {

			return;
			var excludedExtensions = new[] { ".config", ".txt" };
			var directory = new DirectoryInfo(Web.MapPath(Web.Attachments));
			var diskAttachments = directory.GetFiles("*.*", SearchOption.AllDirectories).Where(f => (f.Attributes & FileAttributes.Hidden) == 0 && !excludedExtensions.Contains(f.Extension)).OrderByDescending(f => f.Length).Select(file => file.FullName.RightFrom(Web.MapPath(Web.Attachments)).ToLower().Replace("\\", "/")).ToList();

			var dbAttachments = new HashSet<string>();

			foreach (var tableName in BewebData.GetTableNames()) {

				var sql = new Sql("select top 1 * from ", tableName.SqlizeName());
				var sb = new StringBuilder("");
				using (var reader = sql.GetReader()) {
					if (reader.HasRows) {
						int visibleFieldCount = reader.VisibleFieldCount;
						for (int i = 0; i < visibleFieldCount; i++) {	// for each column
							string dataType = reader.GetDataTypeName(i).ToLower();
							string fieldName = reader.GetName(i).ToLower();
							bool isAttach = (dataType.Contains("varchar") && (fieldName.Contains("attachment") || fieldName.Contains("picture")));
							bool isRich = ((dataType.Contains("varchar") || dataType.Contains("text")) && (fieldName.Contains("html") || fieldName.Contains("body") || fieldName.Contains("text")));
							if (isAttach || isRich) {
								sb.Append(fieldName).Append(",");
							}
						}
					}
				}
				var fields = sb.ToString();
				fields = fields.IsNotBlank() ? fields.Substring(0, fields.Length - 1) : ""; // remove last comma

				if (fields.IsNotBlank()) {
					sql = new Sql("select " + fields + " from ", tableName.SqlizeName());
					using (var reader = sql.GetReader()) {
						foreach (DbDataRecord record in reader) {
							for (int i = 0; i < reader.VisibleFieldCount; i++) {	// for each column
								string fieldName = record.GetName(i).ToLower();
								bool isAttach = ((fieldName.Contains("attachment") || fieldName.Contains("picture")));
								if (!record.IsDBNull(i)) {
									string fieldValue = record.GetString(i);
									if (fieldValue.IsNotBlank()) {
										if (isAttach) {
											dbAttachments.Add(fieldValue);
										} else {
											// @todo: Regex against the value to extract the images
										}
									}
								}
							}
						}
					}
				}
			}

			diskAttachments.RemoveAll(dbAttachments.Contains);

			foreach(var att in diskAttachments) {
				Web.WriteLine(att);
			}
		}
	}
}
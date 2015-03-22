using System;
using System.Collections.Generic;
using System.Web.UI;
using Beweb;

namespace Site.devtools {
	public partial class DBManager : Page {

		protected void Page_Load(object sender, EventArgs e) {

		}

		public static Dictionary<string, DBSchema> GetDatabaseStructure() {
			
			var rows = new Sql("SELECT ROW_NUMBER() OVER (ORDER BY table_type, table_schema, table_name) AS ID, table_type as [Type], table_schema as [Schema], table_name as [Table] FROM information_schema.tables").LoadPooList<DBStructurePOO>();

			var structure = new Dictionary<string, DBSchema>();

			foreach(var row in rows) {
				if(!structure.ContainsKey(row.Schema)) {
					structure[row.Schema] = new DBSchema();
				}

				if(row.Type == "VIEW") {
					structure[row.Schema].Views.Add(row.Table);
				} else {
					structure[row.Schema].Tables.Add(row.Table);
				}
			}

			return structure;
		}

		public class DBStructurePOO {
			public int ID { get; set; }
			public string Schema { get; set; }
			public string Type { get; set; }
			public string Table { get; set; }
		}

		public class DBSchema {
			public List<string> Tables = new List<string>();
			public List<string> Views  = new List<string>();
		}
	}
}
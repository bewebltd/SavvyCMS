using System;
using System.Collections.Generic;
using System.Web.UI;
using Beweb;

namespace Site.devtools {
	public partial class DBUsage : Page {

		protected void Page_Load(object sender, EventArgs e) {

		}

		public static List<DBStructurePOO> GetTablesSize() {
			var tables = new Sql().AddRawSqlString("SELECT t.object_id as ID, t.NAME AS TableName, s.Name AS SchemaName, p.rows AS Rows, (SUM(a.total_pages) * 8) * 1024 AS Size FROM sys.tables t INNER JOIN sys.indexes i ON t.OBJECT_ID = i.object_id INNER JOIN sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id LEFT OUTER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE t.NAME NOT LIKE 'dt%' AND t.is_ms_shipped = 0 AND i.OBJECT_ID > 255 GROUP BY t.object_id, t.Name, s.Name, p.Rows ORDER BY Size DESC").LoadPooList<DBStructurePOO>();
			return tables;
		}

		public class DBStructurePOO {
			public int ID { get; set; }
			public string TableName { get; set; }
			public string SchemaName { get; set; }
			public int Rows { get; set; }
			public int Size { get; set; }
		}

	}
}
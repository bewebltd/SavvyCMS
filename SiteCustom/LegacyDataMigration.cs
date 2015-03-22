using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Beweb;

namespace Site.SiteCustom {
	public class LegacyDataMigration {

	/// <summary>
	/// Example of how to create migration scripts from an old legacy databse
	/// </summary>
	/// <param name="newServerDbName"></param>
	/// <param name="doCreate"></param>
	/// <returns></returns>
		public static List<Sql> GenerateDataMigrationScripts(string newServerDbName, bool doCreate) {
			// generate models off old database with underscorized naming (connstr = guru)
			// newServerDbName = thatcher.rinnai
			var result = new List<Sql>();
			var models = ActiveRecordDatabaseGenerator.GetModelInstances();
			foreach (ActiveRecord model in models) {
				var tableName = model.GetTableName().SqlizeName().value;
				if (!tableName.Contains("rn_")) continue;
				if (tableName.Contains("homebanner")) continue;
				if (tableName.Contains("filetracking")) continue;
				if (tableName.Contains("rn_tracking")) continue;
				if (tableName.Contains("rn_emailtemplate")) continue;
				if (tableName.Contains("rn_categorybanner")) continue;
				if (tableName.Contains("rn_pagecontent")) continue;
				if (tableName.Contains("rn_promo")) continue;
				if (tableName.Contains("rn_pagesection")) continue;
				if (tableName.Contains("banner")) continue;

				string sourceTable, destTable;
				sourceTable = tableName;
				destTable = model.GetType().Name;
				destTable = destTable.Replace("tradesmart", "_tradesmart_");
				destTable = destTable.Replace("centre", "_centre_");
				destTable = destTable.Replace("subcategory", "_subcategory_");
				destTable = destTable.Replace("training", "_training_");
				destTable = destTable.Replace("file", "_file_");
				destTable = destTable.Replace("association", "_association_");
				destTable = destTable.Replace("group", "_group_");
				destTable = destTable.Replace("manuals", "_manuals_");
				destTable = destTable.Replace("archive", "_archive_");
				destTable = destTable.Replace("rn_tn", "rn_trade_");

				destTable = destTable.Remove("rn_").PascalCase();

				bool hasID = false;

				var fields = new DelimitedString(",");
				var destfields = new DelimitedString(",");
				var creates = "" + destTable + "ID [int] IDENTITY(1,1) NOT NULL, ";
				foreach (var field in model.GetFields()) {
					fields += field.Name.SqlizeName().value;
					var newFieldName = field.Name;
					foreach (var repl in "meta,centre,areas,covered,subcategory,training,retailer,tradesmart,consumer,accessory,group,price,email,size,section,path,name,manuals,product,banner,studies,performance,compare,text,study,archive,video,type,manuals,content,outlet".Split(',')) {
						newFieldName = newFieldName.Replace(repl, "_" + repl + "_");
					}
					newFieldName = newFieldName.Replace("dateandtime", "date_and_time");
					newFieldName = Fmt.PascalCase(newFieldName);
					newFieldName = newFieldName.Replace("Id", "ID");
					if (newFieldName == "ID" && destTable=="Country") {
						newFieldName = "Code";
					} else if (newFieldName == "ID") {
						newFieldName = destTable + "ID";
						hasID = true;
					}
					if (newFieldName == "Priority") {
						newFieldName = "SortPosition";
					}
					if (newFieldName == "Image") {
						newFieldName = "Picture";
					}
					if (newFieldName == "Created") {
						newFieldName = "DateAdded";
					}
					destfields += newFieldName;

					if (field.Name != "id" || !hasID) {
						creates += newFieldName + " " + field.GetSqlDataTypeDeclaration().Replace("tinyint","bit").Replace("smallint","int").Replace("char","varchar").Replace("varchar","nvarchar").Replace("varnvarchar","nvarchar").Replace("text","nvarchar(max)").Replace("NOT NULL","") + ", ";
					}
				}

				if (doCreate) {
					var sql2 = new Sql("create table " + destTable + " (<b>" + creates + "</b> CONSTRAINT [pk_" + destTable + "] PRIMARY KEY CLUSTERED (" + destTable + "ID ASC));");
					result.Add(sql2);
				}
				var sql = new Sql("delete from " + destTable + ";");   // maybe use trunc table
				
				if (hasID) sql.Add("set identity_insert " + destTable + " on;");
				sql.Add("insert into " + destTable + " (" + destfields + ") select " + fields + " from RinnaiOld.dbo." + sourceTable + ";");
				if (hasID) sql.Add("set identity_insert " + destTable + " off;");
				result.Add(sql);
			}

			foreach (var migration in result) {
					Web.WriteLine(migration.ToString());
			}

			return result;
		}
	}
}
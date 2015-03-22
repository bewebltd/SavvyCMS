using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Beweb;


namespace Savvy {

	public class SubformOptions {
		public string SubformName = null;
		public bool DisplayAsTableRows = true;
		public bool ShowDeleteButtons = true;
		public bool UseCssButtons = true;
		public string DeleteButtonCaption = "x";
		public string DeleteCallback = "";
	}

	public interface ISubform { }

	/// <summary>
	/// SavvyDataForm is misnamed. It is actually a Subform, not a form. See SavvyDataEdit for the master form manager.
	/// </summary>
	public class SavvyDataForm<TActiveRecordList, TActiveRecord> : ISubform
		where TActiveRecord : ActiveRecord, new()
		where TActiveRecordList : ActiveRecordList<TActiveRecord>, IActiveRecordList, new() {

		// properties
		public ActiveRecordList<TActiveRecord> recordList;
		private Action<TActiveRecord> fieldsCodeBlock;
		private SubformOptions options = new SubformOptions();

		// methods
		/// <summary>
		/// Default constructor: Pass in an ActiveRecordList that will be used in the subform.
		/// </summary>
		/// <param name="list">The ActiveRecordList </param>
		public SavvyDataForm(TActiveRecordList list) {
			this.recordList = list;
		}
		public SavvyDataForm(TActiveRecordList list, SubformOptions options) {
			this.options = options;
			this.recordList = list;
		}
		public SavvyDataForm(ActiveRecordList<TActiveRecord> list, SubformOptions options) {
			this.options = options;
			this.recordList = list;
		}

		public SavvyDataForm(Sql sql, SubformOptions options) {
			this.options = options;
			this.recordList = new TActiveRecordList();
			this.recordList.LoadRecords(sql);
		}

		public string GetSubFormName() {
			if (options.SubformName.IsNotBlank()) {
				return options.SubformName;
			}

			return recordList.GetTableName();
		}

		public string GetSuffix() {
			if (options.SubformName.IsNotBlank()) {
				return "__" + options.SubformName + "__" + recordList.LoopIndex;
			}

			return "__" + recordList.GetTableName() + "__" + recordList.LoopIndex;
		}

		public string GetNewItemSuffix() {
			if (options.SubformName.IsNotBlank()) {
				return "__" + options.SubformName + "__newindex";
			}

			return "__" + recordList.GetTableName() + "__newindex";
		}

		private string GetMaxItemIndex() {
			if (options.SubformName.IsNotBlank()) {
				return "df_MaxRow__" + options.SubformName;
			}

			return "df_MaxRow__" + recordList.GetTableName();
		}

		public void Render(Action<TActiveRecord> aspxCodeBlock) {
			RenderMaxItemsRow();
			this.fieldsCodeBlock = aspxCodeBlock;

			// render rows of existing child records
			foreach (TActiveRecord record in recordList) {
				RenderRow(record, GetSuffix());
			}

			// render an adding row
			var blankRecord = new TActiveRecord();
			RenderRow(blankRecord, GetNewItemSuffix());
		}

		private void RenderMaxItemsRow() {
			string itemIndexName = GetMaxItemIndex();
			string html;
			if (options.DisplayAsTableRows) {
				html = "<tr id=\"" + itemIndexName + "IndexRow\" style=\"display: none;\"><td>";
			} else {
				html = "<div id=\"" + itemIndexName + "IndexRow\" style=\"display: none;\">";
			}

			html += "<input type=\"hidden\" name=\"" + GetMaxItemIndex() + "\" id=\"" + GetMaxItemIndex() + "\" value=\"" + (recordList.Count - 1) + "\" />";

			if (options.DisplayAsTableRows) {
				html += "</td>";
			} else {
				html += "</div>";
			}

			Web.Write(html);
		}

		private void RenderRow(TActiveRecord record, string suffix) {
			Forms.CurrentRowSuffix = suffix;

			// check status for row hide
			string style = "";
			if (suffix.Contains("newindex")) {  // MN 20110705 - fixed
				style = " style='display: none;'";
			}

			// row begin
			string html = "";
			if (options.DisplayAsTableRows) {
				html = "<tr id=\"df_SubformRow" + suffix + "\"" + style + " class=\"svySubformRow row-"+ Beweb.Html.OddEven+"\">";
			} else {
				html = "<div id=\"df_SubformRow" + suffix + "\" class=\"svySubformRow\">";
			}
			Web.Write(html);

			// write out fields
			fieldsCodeBlock.Invoke(record);

			// write row end
			html = "";
			if (!options.ShowDeleteButtons && options.DisplayAsTableRows) {
				html += "<td class=\"remove\" style=\"width:0\">";
			
			}else
			if (options.DisplayAsTableRows) {
				html += "<td class=\"remove\">";
			}

			if (!record.IsNewRecord) {
				html += new HtmlTag("input").Add("type", "hidden").Add("name", "df_recordId" + suffix).Add("value", record.ID_Field.ToString()).ToString();
			}

			string recordStatus = record.IsNewRecord ? "new" : "existing";
			html += new HtmlTag("input").Add("type", "hidden").Add("name", "df_status" + suffix).Add("value", recordStatus).ToString();

			if (options.ShowDeleteButtons) {
				HtmlTag deleteButton = new HtmlTag("");
				if (options.UseCssButtons) {
					deleteButton = new HtmlTag("a").Add("href", "#").SetInnerHtml(options.DeleteButtonCaption);
				} else {
					deleteButton = new HtmlTag("input").Add("type", "button").Add("value", options.DeleteButtonCaption);
				}
				deleteButton.Add("onclick", "df_DeleteRow(this, '" + suffix + "', '" + GetSubFormName() + "', false); " + options.DeleteCallback + " return false;", false);
				deleteButton.Add("title", "Click to delete this row");
				html += deleteButton;
			}
			if (options.DisplayAsTableRows) {
				html += "</td></tr>";
			} else {
				html += "</div>";
			}
			Web.Write(html);

			// clear prefix
			Forms.CurrentRowSuffix = null;
		}
	}
}

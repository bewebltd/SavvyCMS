using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Beweb;

namespace Savvy {
	/// <summary>
	/// Provides standard admin section LIST page functionality.
	/// </summary>
	/// <typeparam name="TActiveRecord"></typeparam>
	public abstract class SavvyDataList<TActiveRecord>
		where TActiveRecord : ActiveRecord, new() {
		public const string SearchFieldInputName = "Search";

		//ActiveRecordList<TActiveRecord> list = new ActiveRecordList<TActiveRecord>();
		private int recordCount = -1;  // not initialised yet

		public string SearchText = Web.Request["Search"] + "";
		public string SearchField = Web.Request["SearchField"] + "";

		// MN 2014 - changed both these to read/write
		public string SortBy = Web.Request["ColSortField"];
		public bool SortDesc = Web.Request["ColSortDesc"] + "" == "1";
			
		public int PageNum { get { return Web.Request["PageNum"].ToInt(1); } }

		/// <summary>
		/// Should be full order by sql string including "order by"
		/// </summary>
		public string DefaultSortBy = "";

		public bool DefaultSortDesc = true;    // NOT USED - what should be used instead?

		public int ItemsPerPage = 50;

		public bool ShowSearch = true;
		public bool ShowExport = true;

		public string FormStyle = "float:right;";
		public string SubTitle;
		public string Title;

		public void PageLoad(System.Web.UI.Page page) {
			//Util.IncludeJavascriptFile(page, "~/js/bewebcore/forms.js");
			PageLoad();
		}
		public void PageLoad() {
			if (Web.Request["ExportButton"] == "Export") {
				ExportToExcel();
			}
		}

		private void ExportToExcel() {
			Beweb.Export.ExportToExcel(GetExportSql());
			Web.Response.End();
		}

		public string AddNewButton(string caption, string url) {
			string html = "";
			html += "<div style='float:left;'><input type=button onclick=\"location='" + url + "'\" value=\"" + caption + "\"></div>";
			return html;
		}

		/// <summary>
		/// Write out the html for the form. This contains pagenum and sortcol hidden fields as well as search/filter fields.
		/// </summary>
		public void Form() {
			Form(null);
		}

		/// <summary>
		/// Write out the html for the form. This contains pagenum and sortcol hidden fields as well as search/filter fields.
		/// Pass in a lambda action which is like a code block to be executed.
		/// </summary>
		public void Form(Action htmlChunk) {
			// build html and write out in two parts so lambda can be executed in between
			string html = "";
			html += "<form name=\"form\" id=\"form\" class=\"svyForm AutoValidate\" method=\"get\" action=\"" + Web.PageFileName + "\" style='" + FormStyle + "'>";
			html += Html.HiddenPageNumField();
			html += Html.HiddenSortColFields();

			//charles:Commented out
			//if (this.ShowSearch) {
			//    html += GetSearchField();
			//}
			Web.Write(html);

			// exec lambda to write some custom html
			if (htmlChunk != null) {
				htmlChunk.Invoke();
			}

			// build second part of html 
			html = "";
			if (this.ShowSearch) {
				html += GetSearchField() + "	<input type=\"submit\" value=\" GO \">";
			}
			if (this.ShowExport) {
				//html += "	<input type=\"submit\" name=\"ExportButton\" value=\"Export\" class=\"svyExport\" onclick=\"//setTimeout(function(){window.location.reload()},500)\" >";						//20140508 jn removed timeout - was no good	as it doesnt allow enough time to export before request is cancelled after half a second.
				// MN 20141204 - finally fixed issue with export button not being able to be clicked twice, by using hidden field and js instead of submit
				html += "	<input type=\"hidden\" name=\"ExportButton\" value=\"\">";
				html += "	<input type=\"button\" onclick=\"this.form.ExportButton.value='Export';this.form.submit();this.form.ExportButton.value='';\" value=\"Export\" class=\"svyExport\">";
			}
			html += "	</form>";
			Web.Write(html);
		}

		protected virtual string GetSearchField() {
			var result = "";
			if (Web.Request[SearchFieldInputName].IsNotBlank()) {					 //20140318 jn fixed filter clear
				var url = Web.PageUrl;

				result += "<a class='svyCancelFilter' href=" + url + ">cancel filter</a> | ";
			}
			result += "Find " + new Forms.TextField(SearchFieldInputName, false) { maxlength = "25" };
			return result;
		}

		public virtual string TitleRow(string title) {
			string html = "<tr><td class=\"dataheading\" colspan=99><div style=\"float:left\">" + title.HtmlEncode() + " <small>"+SubTitle.HtmlEncode()+"</small></div>";
			html += "<div style=\"float:right\"><span class=\"svyAjaxStatus dontprint\"> " + this.PagingNav() + "</span></div>";
			html += "</td></tr>";
			this.Title = title;
			return html;
		}

		public string FilterValue(ActiveField<int?> field) {
			return FilterValue(field,field.ValueObject+"");
		}
		public string FilterAlias;//prefix

		public string FilterValue(ActiveField<int?> field, string displayValue) {
			var url = "?filter="+Web.Server.UrlEncode(field.ValueObject+"")+"&filterCol="+FilterAlias+"["+field.Name+"]";
			string html = "<a href=\""+url+"\" title=\"Filter: show only this value\">" + displayValue.HtmlEncode() + "</a>";
			return html;
		}
		public string FilterValue(ActiveField<string> field) {
			return FilterValue(field,field.ValueObject+"");
		}

		public string FilterValue(ActiveField<string> field, string displayValue) {
			var url = "?filter="+Web.Server.UrlEncode(field.ValueObject+"")+"&filterCol="+FilterAlias+"["+field.Name+"]";
			string html = "<a href=\""+url+"\" title=\"show only this value\">" + displayValue.HtmlEncode() + "</a>";
			return html;
		}
		public string FilterValue(ActiveField<decimal?> field) {
			return FilterValue(field,field.ValueObject+"");
		}

		public string FilterValue(ActiveField<decimal?> field, string displayValue) {
			var url = "?filter="+Web.Server.UrlEncode(field.ValueObject+"")+"&filterCol="+FilterAlias+"["+field.Name+"]";
			string html = "<a href=\""+url+"\" title=\"show only this value\">" + displayValue.HtmlEncode() + "</a>";
			return html;
		}
		public string ItemCountRow() {
			return "<tr class=\"row-last\"><td colspan=99><div>" + ItemCountText + "</div><div>" + PagingNav() + "</div><div class=\"width-expander\"></div></td></tr>";
		}

		public virtual string PagingNav() {
			return "" + Html.PagingNav(Html.CalcPageCount(RecordCount, ItemsPerPage), PageNum) + "";
		}

		public virtual int RecordCount {
			get {
				if (recordCount == -1) {
					var sql = GetSql();
					if (sql != null) {
						recordCount = sql.GetCountSql().FetchIntOrZero();
					}
				}
				return recordCount;
			}
		}

		public string ItemCountText {
			get {
				return "Displaying page " + PageNum + " of "+((RecordCount>0)?"total " + RecordCount + " items":"1");
			}
		}

		public int BreadcrumbLevel = Web.RequestEx["bread"].ToInt(2);
		public Sql FilterSql = new Sql();

		public virtual string RowClass(ActiveRecord record) {
			if(record==null)return "row-"+Html.OddEven;
			return "row-" + Html.OddEven + (record.GetIsActive() ? "" : " inactive");
		}

		public virtual string RowClass() {
			return "row-" + Html.OddEven ;
		}

		public string ColSort(string fieldName) {
			return Html.ColSort(fieldName, SortBy, SortDesc);
		}

		public string ColSort(string fieldName, string title) {
			return Html.ColSort(fieldName, title, SortBy, SortDesc);
		}

		public string ColHead(string title) {
			return ColHead(null,title);
		}

		public string ColHead(string notused,string title) {
			return Html.ColHead(title);
		}

		public Sql GetOrderBySql() {
			Sql sql = new Sql();
			if (SortBy.IsBlank() || SortBy=="SortPosition") {  // if person sorts by sortposition, we assume they really want to sort by default sort order
				if (DefaultSortBy.ToLower().Contains("order by")) {
					sql.AddRawSqlString(DefaultSortBy);
				} else {
					// old behaviour for backwards compatibility with generated code
					sql.Add("order by ", DefaultSortBy.SqlizeName(), (DefaultSortDesc ? "desc" : ""));
				}
			} else {
				sql.Add("order by ", SortBy.SqlizeName(), (SortDesc ? "desc" : ""));
			}
			return sql;
		}

		public virtual Sql GetExportSql() {
			return GetSql();
		}

		/// <summary>
		/// must be overridden as it's abstract, but may return null
		/// </summary>
		/// <returns></returns>
		public abstract Sql GetSql();

		public virtual ActiveRecordList<TActiveRecord> GetResults() {
			return GetResults(false);
		}

		public ActiveRecordList<TActiveRecord> GetResults(bool useDefaultOrderBy) {
			var sql = GetSql();
			sql.Paging(ItemsPerPage, PageNum);
			var list = new ActiveRecordList<TActiveRecord>();
			ActiveRecordLoader.LoadRecords<TActiveRecord>(list, sql);
			return list;
		}

		protected void GetFiltersFromQueryString() {
			string parentFieldName = Web.Request.QueryString["parent"];
			var dummy = new TActiveRecord();
			foreach (var field in dummy.GetFields()) {
				string filterParam = Web.Request.QueryString[field.Name];
				if (filterParam.IsNotBlank()) {
					field.FromString(filterParam);
					FilterSql.Add("and", field.Name.SqlizeName(), "=", field.Sqlize());
					//SubTitle += " - " + field.ToString();   // this will display an ID as a number, upgrade to ToStringNice to do the lookup and show the title
					SubTitle += " - " + field.ToStringNice();  // MN2014: if you aren't using this and don't want to upgrade ActiveField just replace this with the line above
				}
				//if (parentFieldName==filterParam) {
				//	// also set subtitle and go down a breadcrumb
				//	BreadcrumbLevel++;
				//}
			}
		}


	}

}
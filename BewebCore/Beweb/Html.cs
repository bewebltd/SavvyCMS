using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
namespace Beweb {


	/// <summary>
	/// Helper functions for writing HTML.
	/// </summary>
	public class Html {

		/// <summary>
		/// Sortable column header in a list/grid
		/// </summary>
		public static string ColSort(string fieldName) {
			return ColSort(fieldName, null);
		}

		/// <summary>
		/// Sortable column header in a list/grid
		/// </summary>
		public static string ColSort(string fieldName, string currentSortField, bool isDescending) {
			return ColSort(fieldName, null, Web.Request["ColSortField"], (Web.Request["ColSortDesc"] == "1"));
		}

		/// <summary>
		/// Sortable column header in a list/grid
		/// </summary>
		public static string ColSort(string fieldName, string title) {
			return ColSort(fieldName, title, Web.Request["ColSortField"], (Web.Request["ColSortDesc"] == "1"));
		}

		/// <summary>
		/// Sortable column header in a list/grid
		/// </summary>
		public static string ColSort(string fieldName, string title, string currentSortField, bool isDescending) {
			if (title == null) {
				title = Fmt.SplitTitleCase(fieldName);
				if (title.StartsWith("Is "))
					title = title.Substring(3);
			}

			if (fieldName == "SortPosition") {
				isDescending = false;
			}

			HtmlTag html = new HtmlTag("a");
			html.Add("href", "javascript:ColSortBy(" + fieldName.JsEnquote() + ")");
			html.Add("title", "sort by " + title.JsEnquote() + "");
			string css = "colhead colsort";
			if (currentSortField == fieldName) {
				css += " selected";
				if (isDescending) {
					css += " descending";
				} else {
					css += " ascending";
				}
			}
			html.Add("class", css);

			html.SetInnerHtml(title);
			return html.ToString();
		}

		/// <summary>
		/// Column header in a list/grid which is not sortable
		/// </summary>
		public static string ColHead(string title) {
			HtmlTag html = new HtmlTag("div");
			html.Add("class", "colhead");
			html.SetInnerHtml(title);
			return html.ToString();
		}

		/// <summary>
		/// Resets a html helper session, if it's called more than once on a single pageload from different files
		/// </summary>
		/// <param name="name">Name of the Session</param>
		private static void ResetHtmlSession(string name) {
			System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
			var declaringType = stackTrace.GetFrame(2).GetMethod().DeclaringType;
			if (declaringType != null) {
				var callingMethodName = declaringType.Name; // + stackTrace.GetFrame(2).GetILOffset();
				if (Web.PageGlobals["Html." + name + ".CallingMethodName"] + "" != callingMethodName) {
					Web.PageGlobals["Html." + name] = "";
					Web.PageGlobals["Html." + name + ".CallingMethodName"] = callingMethodName;
				}
			}
		}

		/// <summary>
		/// Returns "odd" or "even" to be used as css class names.
		/// </summary>
		public static string OddEven {
			get {
				ResetHtmlSession("Stripe");

				if (Web.PageGlobals["Html.Stripe"] + "" != "odd") {
					Web.PageGlobals["Html.Stripe"] = "odd";
				} else {
					Web.PageGlobals["Html.Stripe"] = "even";
				}
				return Web.PageGlobals["Html.Stripe"].ToString();
			}
		}

		public static void ResetOddEven() {
			Web.PageGlobals["Html.Stripe"] = "";
		}

		/// <summary>
		/// Returns "column1", "column2", "column3" etc up to the number of columns and then starts again at "column1".
		/// To be used as css class names (for example an image gallery grid where you can put clear:both on "column1").
		/// </summary>
		public static string Column(int numColumns) {
			ResetHtmlSession("Column");

			if (Web.PageGlobals["Html.Column"] + "" == "") {
				Web.PageGlobals["Html.Column"] = 1;
			} else {
				Web.PageGlobals["Html.Column"] = (int)Web.PageGlobals["Html.Column"] + 1;
				if ((int)Web.PageGlobals["Html.Column"] > numColumns) {
					Web.PageGlobals["Html.Column"] = 1;
				}
			}
			return "column" + Web.PageGlobals["Html.Column"];
		}

		public static void ResetColumnCount() {
			Web.PageGlobals["Html.Column"] = "";
		}

		public static string Row(int numColumns) {
			return Row(numColumns, 0);
		}

		public static string Row(int numColumns, int totalCount) {
			ResetHtmlSession("Row_ItemCount");

			string row = "";
			int numRows = Numbers.Ceiling((totalCount + 0.0) / (numColumns + 0.0));
			int itemCount = Web.PageGlobals["Html.Row_ItemCount"].ToInt(0);
			itemCount++;
			Web.PageGlobals["Html.Row_ItemCount"] = itemCount;
			var rowNum = Numbers.Ceiling((itemCount + 0.0) / (numColumns + 0.0));
			row = "row" + rowNum;
			if (rowNum == numRows) {
				row += " last-row";
			}
			return row;
		}


		public static string LocalRow(int numColumns, int totalCount) {
			string row = "";
			int numRows = Numbers.Ceiling((totalCount + 0.0) / (numColumns + 0.0));

			int itemCount = Web.PageGlobals["Html.Row_ItemCount"].ToInt(0);
			itemCount++;
			Web.PageGlobals["Html.Row_ItemCount"] = itemCount;


			var rowNum = Numbers.Ceiling((itemCount + 0.0) / (numColumns + 0.0));
			row = "row" + rowNum;
			if (rowNum == numRows) {
				row += " last-row";
			}
			return row;
		}

		public static void ResetRowCount() {
			Web.PageGlobals["Html.Row_ItemCount"] = "";
		}

		public static bool IsLastRow(int numColumns, int totalCount) {
			ResetHtmlSession("IsLastRow_ItemCount");

			bool isLastRow = false;
			int numRows = Numbers.Ceiling((totalCount + 0.0) / (numColumns + 0.0));
			int itemCount = Web.PageGlobals["Html.IsLastRow_ItemCount"].ToInt(0);
			itemCount++;
			Web.PageGlobals["Html.IsLastRow_ItemCount"] = itemCount;
			var rowNum = Numbers.Ceiling((itemCount + 0.0) / (numColumns + 0.0));
			if (rowNum == numRows) {
				isLastRow = true;
			}
			return isLastRow;
		}

		public static void ResetIsLastRowCount() {
			Web.PageGlobals["Html.IsLastRow_ItemCount"] = "";
		}

		//this is useful when you have more than one loop and need the Row method to work eg: tabs - call this method before you render out the next loop!
		public static void ClearAllRowGlobalCounter() {
			ResetRowCount();
			ResetColumnCount();
			ResetIsLastRowCount();
		}


		/// <summary>
		/// Returns "first", "last" or "" depending on the loop index within the given active record list.
		/// To be used as css class names.
		/// </summary>
		/// <param name="activeRecordList"></param>
		/// <returns></returns>
		public static string FirstLast(IActiveRecordList activeRecordList) {
			if (activeRecordList.LoopIndex == 0 && activeRecordList.LoopIndex == activeRecordList.Count - 1) {
				return "first last";
			} else if (activeRecordList.LoopIndex == 0) {
				return "first";
			} else if (activeRecordList.LoopIndex == activeRecordList.Count - 1) {
				return "last";
			}
			return "";
		}

		public static int CalcPageCount(int numResults, int itemsPerPage) {
			return VB.fix((numResults - 1) / itemsPerPage) + 1;
		}

		public static string HiddenPageNumField(int firstPage) {
			//return new Forms.HiddenField("PageNum", "1") + ""; // need to override this as the search starts on page 0 not the usual 1
			return new Forms.HiddenField("PageNum", firstPage.ToString()) + "";
		}

		public static string HiddenPageNumField() {
			return HiddenPageNumField(1);
		}

		public static string HiddenSortColFields() {
			string html = new Forms.HiddenField("ColSortField") + "";
			html += new Forms.HiddenField("ColSortDesc");

			//JC Added 20150319 Losing params when filtering.
			var keys = Web.Request.QueryString.AllKeys;
			string[] exclusions = { "PageNum", "ColSortField", "ColSortDesc", "Search" }; // dont know if this needs tweaking.
			foreach (var key in keys) {
				if (exclusions.Contains(key)) {
					//skip over normal keys
				} else {
					html += new Forms.HiddenField(key, Web.Request[key]);
				}
			}
			return html;
		}

		/// <summary>
		/// Returns HTML for a next/prev paging control, with jump to page dropdown if more than 5 pages.
		/// pageNum is current page (with 1 being the first page).
		/// Calls the js function GoPageNum(pageNum) which you need to include if not already (in js/bewebcore/forms.js).
		/// ItemsPerPage will be taken from the ItemsPerPage property of the sql.
		/// </summary>
		/// <param name="sql">Instance of Sql class that will be converted to a COUNT statement and run to calculate the number of pages.</param>
		/// <returns></returns>
		public static string PagingNav(Sql sql) {
			return PagingNav(sql, sql.ItemsPerPage);
		}

		/// <summary>
		/// Returns HTML for a next/prev paging control, with jump to page dropdown if more than 5 pages.
		/// pageNum is current page (with 1 being the first page).
		/// Calls the js function GoPageNum(pageNum) which you need to include if not already (in js/bewebcore/forms.js).
		/// </summary>
		/// <param name="sql">Instance of Sql class that will be converted to a COUNT statement and run to calculate the number of pages.</param>
		/// <returns></returns>
		public static string PagingNav(Sql sql, int itemsPerPage) {
			int numResults = sql.FetchCount();
			int numPages = CalcPageCount(numResults, itemsPerPage);
			return PagingNav(numPages);
		}

		/// <summary>
		/// Returns HTML for a next/prev paging control, with jump to page dropdown if more than 5 pages.
		/// pageNum is current page (with 1 being the first page).
		/// Calls the js function GoPageNum(pageNum) which you need to include if not already (in js/bewebcore/forms.js).
		/// </summary>
		/// <param name="pageNum">Current page number</param>
		/// <param name="numPages">Total number of pages available</param>
		/// <returns></returns>
		public static string PagingNav(int numPages) {
			return PagingNav(numPages, Web.Request["PageNum"].ToInt(1));
		}

		/// <summary>
		/// Returns HTML for a next/prev paging control, with jump to page dropdown if more than 5 pages.
		/// pageNum is current page (with 1 being the first page).
		/// Calls the js function GoPageNum(pageNum) which you need to include if not already (in js/bewebcore/forms.js).
		/// </summary>
		/// <param name="pageNum">Current page number</param>
		/// <param name="numPages">Total number of pages available</param>
		/// <returns></returns>
		public static string PagingNav(int numPages, int pageNum) {
			//Util.IncludeJavascriptFile("~/js/bewebcore/forms.js", Util.IncludeRenderMode.Auto); // too late?
			//string html = "";
			StringBuilder html = new StringBuilder();
			if (numPages > 1) {
				html.Append("<span class=\"pagingnav-wrapper\">");
				if (pageNum > 1) {
					// we are on the second (or later) page, display Back
					html.Append("<span class=\"pagingnav\">");
					html.Append("<a class=\"pagingnav\" href=\"javascript:GoPageNum(" + (pageNum - 1) + ")\">&lt; Previous Page</a> | ");
				}

				if (numPages > 5) {
					// use dropbox
					html.Append("Page <select class=\"pagingnav\" id=\"PageSelect\" onchange=\"GoPageNum(this.options[this.selectedIndex].value)\">");
					for (int p = 1; p <= numPages; p++) {
						//html.Append("<option value='" + p + "'");
						html.Append("<option value='");
						html.Append(p);
						html.Append("'");
						if (p == pageNum)
							html.Append(" selected");
						//html.Append(">" + p + "</option>");
						html.Append(">");
						html.Append(p);
						html.Append("</option>");
					}
					html.Append("</select>");
				} else {
					// write page numbers
					for (int p = 1; p <= numPages; p++) {
						if (p == pageNum) {
							html.Append("<span class=\"pagingnavcurrent\">" + p + "</span>");
						} else {
							html.Append("<a class=\"pagingnav\" href=\"javascript:GoPageNum(" + p + ")\">" + p + "</a>");
						}
						if (p < numPages) {
							html.Append(" | ");
						}
					}
				}

				if (pageNum < numPages) {
					// there are more items, display Next
					html.Append(" | <a class=\"pagingnav\" href=\"javascript:GoPageNum(" + (pageNum + 1) + ")\">Next Page &gt;</a>");
				}
				html.Append("</span>");
			}

			return html.ToString();
		}

		/// <summary>
		/// AF: Returns HTML for frontend pagination.
		/// pageNum is current page (with 1 being the first page).
		/// url will be the current page url + ?pageNum=1
		/// Usage example:
		///
		/// <div class="pagination-wrapper pagingnav-light-theme">
		///    <%=Beweb.Html.PagingNavFrontEnd(Model.pageCount, Model.pageNum) %>
		/// </div>
		/// 
		/// Available themes: light, dark and compact
		///
		/// </summary>
		/// <param name="pageNum">Current page number</param>
		/// <param name="numPages">Total number of pages available</param>
		/// <returns></returns>
		public static string PagingNavFrontEnd(int numPages, int pageNum) {
			return PagingNavFrontEnd(numPages, pageNum, null);
		}

		/// <summary>
		/// AF: Returns HTML for frontend pagination.
		/// pageNum is current page (with 1 being the first page).
		/// url will be replaced using string.format. Eg: /News/{0}/optparam1/optparam2....
		/// Usage example:
		///
		/// <div class="pagination-wrapper pagingnav-light-theme">
		///    <%=Beweb.Html.PagingNavFrontEnd(Model.pageCount, Model.pageNum, Web.BaseUrl + "News/{0}") %>
		/// </div>
		/// 
		/// Available themes: light, dark and compact
		///
		/// </summary>
		/// <param name="pageNum">Current page number</param>
		/// <param name="numPages">Total number of pages available</param>
		/// <param name="url">The URL which contains the pageNum</param>
		/// <returns></returns>
		public static string PagingNavFrontEnd(int numPages, int pageNum, string url) {

			if (String.IsNullOrEmpty(url)) {
				// Remove the current numPage from the query string
				string queryString = Regex.Replace(Web.QueryString, @"(&|\?)pageNum=\d", "");
				url = Web.PageUrl + queryString + (queryString.Contains("?") ? "&" : "?") + "pageNum={0}";
			}

			if (numPages <= 1) {
				return "";
			}

			int start = 0;
			int end = numPages;

			if (numPages > 10) {
				start = pageNum - 4 < 0 ? 0 : pageNum - 4;
				end = pageNum + 3 > numPages ? numPages : pageNum + 3;

				if (end - start < 7) {
					if (start < Math.Floor(Convert.ToDouble(numPages / 2))) {
						end = end + (7 - (end - start));
					} else {
						start = start - (7 - (end - start));
					}
				}
			}

			StringBuilder html = new StringBuilder("<div class=\"pagingnav-wrapper\">");

			if (pageNum > 1) {
				html.AppendLine("<a href=\"" + string.Format(url, pageNum - 1) + "\" class=\"pagingnav-previous\">Previous</a>");
			} else {
				html.AppendLine("<span class=\"pagingnav-previous\">Previous</span>");
			}

			if (numPages > 10 && pageNum > 4) {
				html.AppendLine("<a class=\"paging-number\" href=\"" + string.Format(url, 1) + "\">" + 1 + "</a>");
				html.AppendLine("<span class=\"paging-ellipse\">...</span>");
			}

			for (int i = (start + 1); i <= end; i++) {
				if (i == pageNum) {
					html.AppendLine("<span class=\"pagingnav-current paging-number\">" + i + "</span>");
				} else {
					html.AppendLine("<a class=\"paging-number\" href=\"" + string.Format(url, i) + "\">" + i + "</a>");
				}
			}

			if (numPages > 10 && numPages - 4 >= pageNum) {
				html.AppendLine("<span class=\"paging-ellipse\">...</span>");
				html.AppendLine("<a class=\"paging-number\" href=\"" + string.Format(url, numPages) + "\">" + numPages + "</a>");
			}

			// Only for small screens. Will be hidden on desktop screen
			html.AppendLine("<i class=\"paging-pagestatus\">" + pageNum + " of " + numPages + "</i>");

			if (pageNum < numPages) {
				html.AppendLine("<a href=\"" + string.Format(url, pageNum + 1) + "\" class=\"pagingnav-next\">Next</a>");
			} else {
				html.AppendLine("<span class=\"pagingnav-next\">Next</span>");
			}

			html.AppendLine("<br/></div>");
			return html.ToString();
		}


		public static string PictureThumb(PictureActiveField picture) {
			return PictureThumb(picture, "", "", false);
			// MN 20101207 - changed default alt text to blank (was displaying filename as alt text)
			//return PictureThumb(picture, (picture.FileName.IsNotBlank())?picture.FileName.RemoveCharsFromEnd(4):"Picture", "", true);
		}

		public static string PictureThumb(PictureActiveField picture, string altText) {
			return PictureThumb(picture, altText, "", false);
		}

		public static string PictureThumb(PictureActiveField picture, string altText, string cssClass) {
			return PictureThumb(picture, altText, cssClass, false);
			// MN 20100919 - default change to false ie no enlargement - defaulting to enlargement is annoying as it just opens a new window if you don't have colorbox
			//return (picture!=null)?PictureThumb(picture, (altText.IsNotBlank())?altText:(picture.FileName.IsNotBlank())?picture.FileName.RemoveCharsFromEnd(4):"Picture", "", false):"";   // MN 20100919 - default change to false ie no enlargement - defaulting to enlargement is annoying as it just opens a new window if you don't have colorbox
			// MN 20101207 - changed default alt text to blank (was displaying filename as alt text)
		}

		public static string PictureThumb(PictureActiveField picture, string altText, string cssClass, bool showEnlargement) {
			if (picture == null) {
				return "";
			} else {
				if (picture.FileName != null && showEnlargement) {
					return "<a href=\"" + picture.ImagePath + "\" class=\"popup\" title=\"View this image larger\">" + Html.Image(picture.ImageThumbPath, altText, cssClass) + "</a>";
				}
				return Html.Image(picture.ImageThumbPath, altText, cssClass);
			}
		}

		/// <summary>
		/// Outputs html image tag for preview size image. Does not show a popup enlargement by default. 
		/// </summary>
		/// <param name="picture"></param>
		/// <param name="altText"></param>
		/// <param name="cssClass"></param>
		/// <returns></returns>
		public static string PicturePreview(PictureActiveField picture, string altText, string cssClass) {
			return PicturePreview(picture, altText, cssClass, false);
		}		
		
		public static string PicturePreview(PictureActiveField picture) {
			return PicturePreview(picture, "", "", false);
		}

		public static string PicturePreview(PictureActiveField picture, string altText, string cssClass, bool showEnlargement) {
			if (picture.FileName != null && showEnlargement) {
				return "<a href=\"" + picture.ImagePath + "\" class=\"popup\" alt=\"View this image larger [" + picture.ImagePath.JsEncode() + "]\">" + Html.Image(picture.ImagePreviewPath, altText, cssClass) + "</a>";
			}
			return Html.Image(picture.ImagePreviewPath, altText, cssClass);
		}

		/// <summary>
		/// Outputs html image tag for medium size image. Does not show a popup enlargement by default. 
		/// </summary>
		/// <param name="picture"></param>
		/// <param name="altText"></param>
		/// <param name="cssClass"></param>
		/// <returns></returns>
		/// 
		public static string PictureMedium(PictureActiveField picture, string altText, string cssClass) {
			return PictureMedium(picture, altText, cssClass, false);
		}

		public static string PictureMedium(PictureActiveField picture, string altText, string cssClass, bool showEnlargement) {
			if (picture.FileName != null && showEnlargement) {
				return "<a href=\"" + picture.ImagePath + "\" class=\"popup\" alt=\"View this image larger [" + picture.ImagePath.JsEncode() + "]\">" + Html.Image(picture.ImageMediumPath, altText, cssClass) + "</a>";
			}
			return Html.Image(picture.ImageMediumPath, altText, cssClass);
		}


		/// <summary>
		/// Outputs html image tag for sma;; size image. Does not show a popup enlargement by default. 
		/// </summary>
		/// <param name="picture"></param>
		/// <param name="altText"></param>
		/// <param name="cssClass"></param>
		/// <returns></returns>
		public static string PictureSmall(PictureActiveField picture, string altText, string cssClass) {
			return PictureSmall(picture, altText, cssClass, false);
		}

		public static string PictureSmall(PictureActiveField picture, string altText, string cssClass, bool showEnlargement) {
			if (picture.FileName != null && showEnlargement) {
				return "<a href=\"" + picture.ImagePath + "\" class=\"popup\" alt=\"View this image larger [" + picture.ImagePath.JsEncode() + "]\">" + Html.Image(picture.ImageSmallPath, altText, cssClass) + "</a>";
			}
			return Html.Image(picture.ImageSmallPath, altText, cssClass);
		}

		public static string PictureCustomSize(string pictureSizeCodeSuffix, PictureActiveField picture) {
			return Html.Image(picture.ImageCustomSizePath(pictureSizeCodeSuffix), "", "");
		}

		/// <summary>
		/// Returns a string containing an HTML IMG tag, given a picture field and alt text.
		/// Returns empty string (ie no tag) if picture field is null.
		/// </summary>
		/// <param name="picture"></param>
		/// <param name="altText"></param>
		/// <param name="cssClass"></param>
		/// <returns></returns>
		public static string Picture(PictureActiveField picture, string altText, string cssClass) {
			return Html.Image(picture.ImagePath, altText, cssClass);
		}

		/// <summary>
		/// Returns a string containing an HTML IMG tag, given a file name (assumed to be in attachments folder) and alt text.
		/// Returns empty string (ie no tag) if imageFileName is null.
		/// </summary>
		/// <param name="imageFileName">File name in attachments folder</param>
		/// <param name="altText"></param>
		/// <param name="cssClass"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static string Picture(string imageFileName, string altText, string cssClass, int width, int height) {
			return Html.Image(ImageProcessing.ImagePath(imageFileName), altText, cssClass, width, height);
		}

		/// <summary>
		/// Returns a string containing an HTML IMG tag, given a file name (assumed to be in attachments folder) and alt text.
		/// Returns empty string (ie no tag) if imageFileName is null.
		/// </summary>
		/// <param name="imageFileName">File name in attachments folder</param>
		/// <param name="altText"></param>
		/// <param name="cssClass"></param>
		/// <returns></returns>
		public static string Picture(string imageFileName, string altText, string cssClass) {
			return Html.Image(ImageProcessing.ImagePath(imageFileName), altText, cssClass);
		}

		/// <summary>
		/// Returns a string containing an HTML IMG tag, given a SRC attribute (ie relative URL to image file), alt text and css class.
		/// Returns empty string (ie no tag) if imageSrc is null.
		/// </summary>
		/// <param name="imageSrc">this should include the attachments folder if required</param>
		/// <param name="altText"></param>
		/// <param name="cssClass"></param>
		/// <returns></returns>
		public static string Image(string imageSrc, string altText, string cssClass) {
			return Image(imageSrc, altText, cssClass, null, null);
		}

		/// <summary>
		/// Returns a string containing an HTML IMG tag, given a SRC attribute (ie relative URL to image file), alt text and css class.
		/// Returns empty string (ie no tag) if imageSrc is null.
		/// </summary>
		/// <param name="imageSrc">this should include the attachments folder if required</param>
		/// <param name="altText"></param>
		/// <param name="cssClass"></param>
		/// <returns></returns>
		public static string Image(string imageSrc, string altText, string cssClass, int? width, int? height) {
			string result = "";
			if (imageSrc.IsNotBlank()) {
				var html = new HtmlTag("img");
				html.Add("src", imageSrc, false);
				html.AddIfNotBlank("alt", altText);
				html.AddIfNotBlank("class", cssClass);
				if (width != null) html.Add("width", width.ToString());
				if (height != null) html.Add("height", height.ToString());
				result = html.ToString();
			}
			return result;
		}

		//public class Image {
		//  protected PictureActiveField picture;
		//  protected string altText;
		//  public Image(PictureActiveField picture, string altText) {
		//    this.picture = picture;
		//    this.altText = altText;
		//  }
		//  public new string ToString() {
		//    return Html.Image(picture, altText);
		//  }
		//}

		public static string If(bool condition, string resultIfTrue) {
			if (condition) {
				return resultIfTrue;
			}
			return "";
		}

		public static string If(bool condition, string resultIfTrue, string resultIfFalse) {
			if (condition) {
				return resultIfTrue;
			}
			return resultIfFalse;
		}


		public static string Picture(PictureActiveField imageFileName, string altText) {
			return Picture(imageFileName, altText, "");
		}

		public static string PicturePreview(PictureActiveField picture, string altText) {
			return PicturePreview(picture, altText, "");
		}
		public static string PictureMedium(PictureActiveField picture, string altText) {
			return PictureMedium(picture, altText, "");
		}

		public static string PictureSmall(PictureActiveField picture, string altText) {
			return PictureSmall(picture, altText, "");
		}

#if DOTNET4
		/// <summary>
		/// You can supply any of the parameters you like. You must supply either 'percentage' (as a whole number out of 100) or 'amount' and 'outOfTotal' parameters.
		/// </summary>
		/// <example>
		/// 	<%=Beweb.Html.BarGraph(percentage:project.PercentOfBudget.ToInt(0), amount:actualHours, showAmount:true, decimalPlaces:1) %>
		/// 	<%=Beweb.Html.BarGraph(amount:totalHoursUsed, outOfTotal:totalHoursPurchased, showPercentage:true) %>
		/// </example>
		public static string BarGraph(decimal? amount = null, decimal? outOfTotal = null, bool showPercentage = false, bool showAmount = false, bool showOutOf = false, decimal? percentage = null, string label = null, int decimalPlaces = 0) {
			/* You need to add the following CSS to your stylesheet.
			 * .svyBarGraph { width: 150px; }
			 * .svyBarGraphLabel { position: absolute;color:white;margin-left: 5px; }
			 * .svyBarGraphOuter { width:50px;background:#333;float:left;margin-right:10px;height:18px;border-radius: 5px; }
			 * .svyBarGraphInner { background:green;height:14px;margin-top:2px;margin-left:2px;border-radius: 4px; }
			 */
			if (percentage == null && amount != null && outOfTotal != null) {
				percentage = Numbers.Floor(Numbers.SafeDivide(amount.Value, outOfTotal.Value) * 100);
			} else if (percentage == null) {
				percentage = 0;
			}
			if (showAmount && amount == null) {
				throw new BewebException("Html.BarGraph: You need to supply 'amount' parameter if you set 'showAmount' to true.");
			}
			if (showOutOf && outOfTotal == null) {
				throw new BewebException("Html.BarGraph: You need to supply 'outOfTotal' parameter if you set 'showOutOf' to true.");
			}

			if (label.IsBlank()) {
				if (showAmount) {
					label = Fmt.Number(amount, decimalPlaces);
					if (showOutOf) {
						label += " of " + Fmt.Number(outOfTotal, decimalPlaces);
					}
					if (showPercentage) {
						label += " (" + Fmt.Percent(percentage, decimalPlaces) + ")";
					}
				} else if (showPercentage) {
					label = Fmt.Percent(percentage, decimalPlaces);
				}
			}

			var html = new HtmlTag("div").Add("class", "svyBarGraph");
			var labelSpan = new HtmlTag("span").Add("class", "svyBarGraphLabel").SetInnerText(label);
			var barOuter = new HtmlTag("div").Add("class", "svyBarGraphOuter");
			var barInner = new HtmlTag("div").Add("class", "svyBarGraphInner").Add("style", "width:" + Fmt.Number(percentage, 0, false) + "%");

			barOuter.AddTag(barInner);
			html.AddTag(labelSpan);
			html.AddTag(barOuter);

			return html.ToString();
		}

#endif

	}

	/// <summary>
	/// simple class for appending attributes together to output an html element with proper htmlencoding of values
	/// </summary>
	public class HtmlFragment {
		protected List<HtmlFragment> _contents = new List<HtmlFragment>();

		protected HtmlFragment() {
		}

		public virtual HtmlFragment AddRawHtml(string html) {
			_contents.Add(new RawHtml(html));
			return this;
		}

		public HtmlFragment AddFrag(HtmlFragment html) {
			_contents.Add(html);
			return this;
		}

		public override string ToString() {
			return _contents.Join("");
		}

		protected class RawHtml : HtmlFragment {
			private string _raw = null;
			public RawHtml(string html) {
				_raw = html;
			}
			public override string ToString() {
				return _raw;
			}
		}
	}

	/// <summary>
	/// appends attributes to output an html element with proper htmlencoding of values
	/// </summary>
	public class HtmlTag : HtmlFragment {
		private string _html = "";
		//private string _innerHtml = "";
		private string _tagName = "";
		public bool AddEndTag { get; set; }

		public HtmlTag(string tagName) {
			//_innerHtml = "";
			_tagName = tagName;
			_html = "<" + tagName;
			AddEndTag = true;
			string lowerTagName = tagName.ToLower();
			string selfClosingTags = "area,base,br,col,command,embed,hr,img,input,keygen,link,meta,param,source,track,wbr";
			if (selfClosingTags.Split(',').Contains(lowerTagName)) {
				AddEndTag = false;    // MN/JN 20120724 - fixed
			}

			// alternatively we could use a hashtable or something similar eg var d = new System.Collections.Hashtable { { "1", "hello" }, { 2, "goodbye" } };
		}

		public HtmlTag Add(string attributeName) {
			_html += " " + attributeName;
			return this;
		}

		public HtmlTag Add(string attributeName, string attributeValue) {
			var val = HttpContext.Current.Server.HtmlEncode(attributeValue);
			_html += " " + attributeName + "=\"" + val + "\"";
			return this;
		}

		public HtmlTag Add(string attributeName, string attributeValue, bool encode) {
			var val = attributeValue;
			if (encode) val = HttpContext.Current.Server.HtmlEncode(attributeValue);
			_html += " " + attributeName + "=\"" + val + "\"";
			return this;
		}

		public HtmlTag AddIfNotBlank(string attributeName, string attributeValue) {
			AddIfNotBlank(attributeName, attributeValue, true);
			return this;
		}

		public HtmlTag AddIfNotBlank(string attributeName, string attributeValue, bool encode) {
			if (!String.IsNullOrEmpty(attributeValue)) {
				Add(attributeName, attributeValue, encode);
			}
			return this;
		}

		public HtmlTag SetInnerHtml(string innerHtml) {
			_contents = new List<HtmlFragment>();
			AddRawHtml(innerHtml);
			return this;
		}

		public HtmlTag SetInnerText(string innerText) {
			SetInnerHtml(HttpContext.Current.Server.HtmlEncode(innerText));
			return this;
		}
		public HtmlTag SetInnerTextCdata(string rawInnerText) {
			SetInnerHtml("<![CDATA[" + rawInnerText + "]]>");
			return this;
		}

		public new HtmlTag AddRawHtml(string divText) {
			base.AddRawHtml(divText);
			return this;
		}

		public HtmlTag AddTag(HtmlTag tag) {
			AddFrag(tag);
			return this;
		}

		public HtmlTag AddTag(string tagName, string textContent) {
			var tag = new HtmlTag(tagName);
			tag.SetInnerText(textContent);
			AddFrag(tag);
			return this;
		}

		/// <summary>
		/// Returns the finished HTML as a string.
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			if (_contents.Count > 0 || AddEndTag) {
				return _html + ">" + base.ToString() + "</" + _tagName + ">";
			} else {
				return _html + "/>";
			}
		}

		//MN 20130715 - moved ToHtmlString() to an extension method in SavvyMVC project to remove System.Web.Mvc dependency

		public HtmlTag AppendToInnerHTML(string p) {
			AddRawHtml(p);
			return this;
		}

	}
}
using System.Web.Mvc;
using Beweb;
using Savvy;
using Site.SiteCustom;

namespace Site.Controllers {

	public class QuestionsController : ApplicationController {

		public class ViewModel : PageTemplateViewModel {
			public ListHelper Questions;
		}

		[HttpGet]
		public ActionResult Index() {
			var data = new ViewModel();
			data.ContentPage = Models.Page.LoadPageContent("Questions");
			data.Questions = new ListHelper();
			return View("Questions", data);
		}

		public ActionResult CreateSampleQuestions() {
			for (var i = 1; i < Util.GetRandomInt(3,6); i++) {
				var sec = new Models.FAQSection();
				sec.SectionName = "Sample section " + i;
				sec.IsPublished=true;
				sec.Save();
				for (var j = 1; j < Util.GetRandomInt(3,6); j++) {
					var qn = new Models.FAQItem();
					qn.FAQTitle = "Sample question " + j;
					qn.FAQSectionID = sec.ID;
					qn.IsPublished=true;
					qn.BodyTextHTML="Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zril delenit au gue duis dolore tLorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zril delenit au gue duis dolore t";

					qn.Save();

				}
			}
			return Index();
		}
		public class ListHelper : SavvyDataList<Models.FAQSection> {
			public ListHelper() {

			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM FAQSection where 1=1");
				if (SearchText != "") {
					sql.AddKeywordSearch(SearchText, new Models.FAQSection().GetNameField().Name, true);
				}
				if (SortBy.IsBlank()) {
					sql.AddRawSqlString(new Models.FAQSection().GetDefaultOrderBy());
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}










	}
}

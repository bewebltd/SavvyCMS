using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Site.SiteCustom;
using TextBlock = Beweb.TextBlock;

namespace Site.Controllers {
	public class EventsController : ApplicationController {
		
		public ActionResult Index() {
			// this is the main news & events page
			var data = new ViewModel();

			data.ContentPage = Models.Page.LoadOrCreatePageCode("Events");
			if (data.ContentPage == null) throw new Exception("Events page not found");
			//data.newsList = Models.NewsList.Load(new Sql("Select Top 10 * from News where IsPublished=1 order by NewsDate desc"));
			data.eventList = Models.EventList.Load(new Sql("Select * from Event where IsPublished=1 and StartDate >=", DateTime.Now.AddDays(-7).SqlizeDate(), " order by StartDate"));
			// events list to do
//      string sql = "SELECT EventCategory.ColourKey, Event.* FROM Event INNER JOIN EventCategory ON Event.EventCategoryID = EventCategory.EventCategoryID INNER JOIN SiteEventCalenderHasEvent ON Event.EventID = SiteEventCalenderHasEvent.EventID INNER JOIN SiteEventCalender ON SiteEventCalenderHasEvent.SiteEventCalenderID = SiteEventCalender.SiteEventCalenderID"
//sql = sql & " WHERE ISNULL(Event.IsPublished, 0) = 1 AND Event.EventDate >= " & FmtSqlDate(DateAdd("d",-7,date()))
//sql = sql & " "
//sql = sql & " AND EventCategory.ShowOnExtranetOnly = 0"
//if (eventCategoryID <> "" and eventCategoryID <> "0") then
//  sql = sql & " AND EventCategory.EventCategoryID=" & FmtSqlNumber(eventCategoryID)
//end if 
//sql = sql & " AND (SiteEventCalender.SiteCode = '"& FmtSqlString(sitecode)&"')"
//sql = sql & " ORDER BY Event.EventDate, Event.StartTimeAMPM, Event.StartTimeHours, Event.StartTimeminutes, Event.FinishTimeAMPM, Event.FinishTimeHours, Event.FinishTimeMinutes"
//set rs = db.execute(sql)


			return View("Events", data);
		}


		public class ViewModel : PageTemplateViewModel {
			//public NewsList newsList;
			public EventList eventList;

			public bool showEventFilter =true;

			// NOTE: this should be a method or property on the News model
			// DUPLICATED CODE
			public string FormatNewsDate(Object date) {
				return FormatNewsDate(date, false);
			}

			public string FormatNewsDate(Object date, bool isShort) {
				string result = "";
				string[] longdate;
				if (isShort) {
					longdate = Fmt.Date(date.ToString()).Split('-');
				} else {
					longdate = Fmt.LongDate(date).Split('-');
				}
				string day = longdate[0];
				string month = longdate[1];
				string year = longdate[2];
				if (day.StartsWith("0")) {
					day = day.Remove(0, 1);
				}
				int intDay = day.ToInt();
				if (intDay < 11 || intDay > 13) {
					if (day.EndsWith("1")) {
						day += "st";
					} else if (day.EndsWith("2")) {
						day += "nd";
					} else if (day.EndsWith("3")) {
						day += "rd";
					} else {
						day += "th";
					}
				} else {
					day += "th";
				}
				result = month + " " + day + ", " + year;
				return result;
			}

		}
	}
}

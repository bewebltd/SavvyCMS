#define TestExtensions
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Beweb;

namespace Beweb {
	public static class Dates {
		/// <summary>
		/// given a date, return the date for the first day of the month of the source
		/// </summary>
		/// <param name="dt">date to convert for first day of </param>
		/// <returns></returns>
		public static DateTime FirstDayOfMonth(DateTime dt) {
			var dtfi = new DateTimeFormatInfo();
			return DateTime.Parse("1-" + dtfi.AbbreviatedMonthNames[dt.Month - 1] + "-" + dt.Year);
		}

		public static DateTime GetMonthBegin(DateTime date) {
			return new DateTime(date.Year, date.Month, 1);
		}

		/// <summary>
		/// Determines if the given string represenataion of a date is a 'real' parsable date
		/// </summary>
		/// <seealso cref="ActiveFieldBase.IsDateField"/>
		/// <remarks>
		/// Useful on server side validation of form submissions, to check if the submitted string can successfully be bound to a Date
		/// </remarks>
		/// <param name="strDate">a date, expected to be in some kind of date format (hopefully!)</param>
		/// <returns>true if date, false if not</returns>
		public static bool IsDate(string strDate) {
			bool isDate = true;
			try {
				DateTime dt = DateTime.Parse(strDate);
			} catch {
				isDate = false;
			}
			return isDate;
		}

		/// <summary>
		/// Determines if the given string represenataion of a time is a 'real' time. E.g. '7:00am'
		/// MN: fixed bug, need to test
		/// </summary>
		/// <param name="strTime">a time, expected to be in some kind of time format (hopefully!)</param>
		/// <returns>true if is a time, false if not</returns>
		public static bool IsTime(string strTime) {
			var now = DateTime.Now;
			var strDate = "1 Apr 2014 " + strTime; // MN: fixed bug, need to test
			return Dates.IsDate(strDate);
		}

		public static DateTime GetMonthEnd(DateTime date) {
			return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
		}

		public static DateTime GetPreviousMonthBegin(DateTime date) {
			return GetMonthBegin(date).AddMonths(-1);
		}

		/// <summary>
		/// Returns latest monday (including today if its monday)
		/// </summary>
		public static DateTime GetFinancialYearBegin(DateTime date, string monthAbbr) {
			if (monthAbbr == null) {
				monthAbbr = "Apr";
			}
			date = GetMonthBegin(date);
			while (date.GetMonthName().Left(3).ToLower() != monthAbbr.Left(3).ToLower()) {
				date = date.AddMonths(-1);
			}
			return date;
		}

		public static DateTime GetPreviousMonthEnd(DateTime date) {
			return GetMonthBegin(date).AddDays(-1);
		}

		public static DateTime GetFollowingMonthBegin(DateTime date) {
			return GetMonthBegin(date).AddMonths(1);
		}

		public static DateTime GetFollowingMonthEnd(DateTime date) {
			return GetMonthEnd(GetFollowingMonthBegin(date));
		}

		public static DateTime GetFollowingMonth20th(DateTime date) {
			return new DateTime(date.Year, date.Month, 20).AddMonths(1);
		}
		///<summary>Gets the first week day following a date. StackOverflow 3284452</summary>
		///<param name="date">The date.</param>
		///<param name="dayOfWeek">The day of week to return.</param>
		///<returns>The first dayOfWeek day following date, or date if it is on dayOfWeek.</returns
		public static DateTime Next(this DateTime date, DayOfWeek dayOfWeek) {
			return date.AddDays((dayOfWeek < date.DayOfWeek ? 7 : 0) + dayOfWeek - date.DayOfWeek);
		}

		public static int GetPersonAge(DateTime birthDate, DateTime now) {
			int age = now.Year - birthDate.Year;
			if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day)) age--;
			return age;
		}

		public static int DaysBetween(DateTime fromDate, DateTime toDate) {
			return (int)Math.Abs(Math.Floor((toDate - fromDate).TotalDays)) + 1;
		}

		public static bool WithinStartAndEnd(DateTime? startDateTime, DateTime? endDateTime) {
			// neither date specified
			if (startDateTime == null && endDateTime == null) return true; // is ok
			// end date - no start date
			if (startDateTime == null && DateTime.UtcNow < endDateTime.Value.ToUniversalTime()) return true;
			// start date - no end date
			if (endDateTime == null && DateTime.UtcNow > startDateTime.Value.ToUniversalTime()) return true;
			// both dates specified
			if (DateTime.UtcNow < endDateTime.Value.ToUniversalTime() && DateTime.UtcNow > startDateTime.Value.ToUniversalTime()) return true;
			return false; // didn't fit any of the above
		}

		/// <summary>
		/// Returns latest monday (including today if its monday)
		/// </summary>
		public static DateTime GetWeekBegin(DateTime date) {
			while (date.DayOfWeek != DayOfWeek.Monday) {
				date = date.AddDays(-1);
			}
			return date;
		}

		/// <summary>
		/// Given a server time (eg 11:00 server time = NZT = +13) and a timezone code (eg "Eastern Australian Time"), return the time with correct offset (eg 9:00 +11).
		/// dateTime = time in local time (server time)
		/// timeZone = windows timezone code eg "New Zealand Time"
		/// You can get timezones and put them in a dropbox using Forms.GetTimeZoneOptions()
		/// </summary>
		public static DateTimeOffset? ConvertToTimeZone(DateTime? dateTime, string timeZone) {
			if (dateTime == null) {
				return null;
			}
			var localTime = new DateTimeOffset(dateTime.Value);
			var otherTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(localTime,timeZone);
			return otherTime;
		}

		/// <summary>
		/// Returns latest monday (including today if its monday)	 regardless of annual public holidays
		/// </summary>

		public static DateTime GetFirstBusinessDay(int Year, int Month) {
			var firstOfMonth = default(DateTime);
			var firstBusinessDay = default(DateTime);
			firstOfMonth = new DateTime(Year, Month, 1);
			if (firstOfMonth.DayOfWeek == DayOfWeek.Sunday) {
				firstBusinessDay = firstOfMonth.AddDays(1);
			} else if (firstOfMonth.DayOfWeek == DayOfWeek.Saturday) {
				firstBusinessDay = firstOfMonth.AddDays(2);
			} else {
				firstBusinessDay = firstOfMonth;
			}
			return firstBusinessDay;
		}
		public static DateTime GetLastBusinessDay(int Year, int Month) {
			var lastOfMonth = default(DateTime);
			var lastBusinessDay = default(DateTime);
			lastOfMonth = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month));
			if (lastOfMonth.DayOfWeek == DayOfWeek.Sunday) {
				lastBusinessDay = lastOfMonth.AddDays(-2);
			} else if (lastOfMonth.DayOfWeek == DayOfWeek.Saturday) {
				lastBusinessDay = lastOfMonth.AddDays(-1);
			} else {
				lastBusinessDay = lastOfMonth;
			}
			return lastBusinessDay;
		}

		private static bool? _hasCalendarHolidayTable = null;
		// http://www.dol.govt.nz/er/holidaysandleave/publicholidays/publicholidaydates/ical/public-holidays-all.ics
		// http://www.dol.govt.nz/er/holidaysandleave/publicholidays/publicholidaydates/current.asp

		public static DateTime GetWorkingDay(DateTime date, int holidayRegionID) {
			if (_hasCalendarHolidayTable==null) {
				_hasCalendarHolidayTable = BewebData.TableExists("CalendarHoliday");
			}
			DateTime nextWorkDate = new DateTime(date.Ticks);
			bool isHoliday;
			do {
				isHoliday = nextWorkDate.DayOfWeek== DayOfWeek.Sunday || nextWorkDate.DayOfWeek== DayOfWeek.Saturday;
				if (_hasCalendarHolidayTable.Value && !isHoliday) {
					Sql sql = new Sql("select * from CalendarHoliday where HolidayDate=", nextWorkDate);
					if (holidayRegionID > 0) {
						// return National and Regional Holidays 
						sql.Add("and (HolidayRegionID is null or HolidayRegionID = ", holidayRegionID.SqlizeNumber(), ")");
					}else if (BewebData.FieldExists("CalendarHoliday","HolidayRegionID")) {
						// return Holidays without regions if regions exist (e.g. National Holidays)
						sql.Add("and HolidayRegionID is null");
					}
					isHoliday = sql.RecordExists();
				}
				if (isHoliday) {
					nextWorkDate = nextWorkDate.AddDays(1);
				}
			} while (isHoliday);
			return nextWorkDate;
		}

		public static bool IsWorkingDay(DateTime date, int holidayRegionID) {
			DateTime nextWorkDay = GetWorkingDay(date, holidayRegionID);
			return nextWorkDay == date;
		}


		public static List<DateTime> GetBusinessDays(List<DateTime> days,  int holidayRegionID) {
			List<DateTime> businessDays = new List<DateTime>();
			foreach (var day in days) {
				if (IsWorkingDay(day,holidayRegionID)) {
					businessDays.Add(day);
				}
			}
			return businessDays;
		}

		public static List<DateTime> GetBusinessDays(List<DateTime> days) {
			return GetBusinessDays(days, 0);
		}
		/// <summary>
		/// return number of months (absolute) between lvalue and rvalue
		/// </summary>
		/// <param name="lValue"></param>
		/// <param name="rValue"></param>
		/// <returns></returns>
		public static int MonthDifference(this DateTime lValue, DateTime rValue) {
			return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
		}
		/// <summary>
		/// 20150205jn
		///given a date in this format, yyyyMMdd convert to date
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime? ConvertCompressedDate(string date) {
			DateTime? result = null;

			try {
				 	result= DateTime.ParseExact(date,
				"yyyyMMdd",
				CultureInfo.InvariantCulture,
				DateTimeStyles.None);
			} catch (Exception) {
				
			}
		
			return result;
		}

		/// <summary>
		/// Returns true if both dates are on the same day of the year (eg both 18 Feb).
		/// </summary>
		public static bool IsAnniversary(DateTime? date1, DateTime? date2) {
			if (date1 == null || date2 == null) return false;
			if (date1.Value.Month != date2.Value.Month) {
				return false;
			}
			if (date1.Value.Date == date2.Value.Date) {
				return true;
			}
			if (date1.Value == GetMonthEnd(date1.Value) && date2.Value == GetMonthEnd(date2.Value)) {
				// feb 28 (non leap year) = feb 29 (leap year)
				return true;
			}
			return false;
		}
		/// <summary>
		///  Returns true if dateTime is Business Time (Wednesday Evening) https://www.youtube.com/watch?v=AqZcYPEszN8
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static bool IsBusinessTime(DateTime dateTime) {
			return (dateTime.DayOfWeek == DayOfWeek.Wednesday && dateTime.TimeOfDay >= DateTime.Today.AddHours(18).TimeOfDay);
		}


		public static DateTime UnixTimeStampToDateTime(double unixTimeStamp) {
			// Unix timestamp is seconds past epoch
			System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
			return dtDateTime;
		}

		public static double DateTimeToUnixTimestamp(DateTime dateTime) {
			return (dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
		}

		public static Int64 MillisecondsSince1970 {
			get { return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds, CultureInfo.CurrentCulture); }
		}

		public static Int64 SecondsSince1970 {
			get { return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds, CultureInfo.CurrentCulture); }
		}

	}

	public static class DateExtensionMethods {
		public static DateTime GetMonthBegin(this DateTime date) {
			return new DateTime(date.Year, date.Month, 1);
		}
		public static DateTime GetFirstBusinessDayOfMonth(this DateTime date) {
			return Dates.GetFirstBusinessDay(date.Year, date.Month);
		}
		public static DateTime GetLastBusinessDayOfMonth(this DateTime date) {
			return Dates.GetLastBusinessDay(date.Year, date.Month);
		}

		public static DateTime GetMonthEnd(this DateTime date) {
			return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
		}

		public static DateTime GetPreviousMonthBegin(this DateTime date) {
			return GetMonthBegin(date).AddMonths(-1);
		}

		public static DateTime GetPreviousMonthEnd(this DateTime date) {
			return GetMonthBegin(date).AddDays(-1);
		}

		public static DateTime GetFollowingMonthBegin(this DateTime date) {
			return GetMonthBegin(date).AddMonths(1);
		}

		public static DateTime GetFollowingMonthEnd(this DateTime date) {
			return GetMonthEnd(GetFollowingMonthBegin(date));
		}

		public static DateTime GetFollowingMonth20th(this DateTime date) {
			return new DateTime(date.Year, date.Month, 20).AddMonths(1);
		}

		public static int GetPersonAge(this DateTime birthDate) {
			return Dates.GetPersonAge(birthDate, DateTime.Now);
		}

		/// <summary>
		/// Return nicely formatting string of number of milliseconds since this DateTime. Useful for outputting timing information.
		/// </summary>
		/// <param name="startDateTime"></param>
		/// <returns></returns>
		public static string FmtMillisecondsElapsed(this DateTime startDateTime) {
			double elapse = 0;
			int result = 0;
			try {
				elapse = Math.Floor((DateTime.Now - startDateTime).TotalMilliseconds);
				result = Convert.ToInt32(elapse);
			} catch (Exception e) { }
			return result + "ms";
		}
		/// <summary>
		/// Return nicely formatting string of number of milliseconds if under 10 seconds, otherwise seconds/minutes/hours/etc since this DateTime. Useful for outputting timing information.
		/// </summary>
		/// <param name="startDateTime"></param>
		/// <returns></returns>
		public static string FmtTimeElapsed(this DateTime startDateTime) {
			var millis = Convert.ToInt32(Math.Floor((DateTime.Now - startDateTime).TotalMilliseconds));
			if (millis < 10000) {
				return millis + " milliseconds";
			}
			return Fmt.TimeDiffText(startDateTime, DateTime.Now).Remove("ago");
		}
	}
}

#if TestExtensions
namespace BewebTest {


	/// <summary>
	///This is a test class for FmtTest and is intended
	///to contain all FmtTest Unit Tests
	///</summary>
	[TestClass()]
	public class DatesTest {
		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for FmtDateTime
		///</summary>
		[TestMethod()]
		public void GetNextTest() {
			DateTime date = new DateTime(2014, 08, 2); // 2 aug 2014
			string actual = Fmt.Date(date.Next(DayOfWeek.Monday)); //get the next monday
			string expected = Fmt.Date(new DateTime(2014, 08, 4)); //4th is a monday 
			Web.WriteLine("");
			Assert.AreEqual(expected, actual);
			Web.WriteLine("Value is: "+actual);
			
			//rollover date
			date = new DateTime(2014, 08, 30); // 2 aug 2014
			actual = Fmt.Date(date.Next(DayOfWeek.Wednesday)); //get the next monday should rollover into next month

			expected = Fmt.Date(new DateTime(2014, 09, 3)); //3rd is a wednesday in sept 
			Web.WriteLine("");
			Assert.AreEqual(expected, actual);
			Web.WriteLine("Value is: "+actual);

		}
		[TestMethod()]
		public void GetWeekBegin() {
			DateTime expected = "16-Dec-2013 4:50pm".ConvertToDate(DateTime.Now);
			DateTime actual = Dates.GetWeekBegin("16 dec 2013 16:50".ConvertToDate(DateTime.Now));

			Assert.AreEqual(expected, actual);																	 //same day is monday


			Assert.AreEqual("16-Dec-2013 4:50pm".ConvertToDate(DateTime.Now), Dates.GetWeekBegin("19 dec 2013 16:50".ConvertToDate(DateTime.Now))); // start with thursday, get monday
			Assert.AreEqual("9-Dec-2013 4:50pm".ConvertToDate(DateTime.Now), Dates.GetWeekBegin("15 dec 2013 16:50".ConvertToDate(DateTime.Now))); // start with sunday, get monday

		}
		[TestMethod()]
		public void GetFirstBusinessDay() {
			DateTime expected = "2-Dec-2013".ConvertToDate(DateTime.Now);
			var startdate = "16 dec 2013".ConvertToDate(DateTime.Now);
			DateTime actual = Dates.GetFirstBusinessDay(startdate.Year, startdate.Month);			 //start at middle of month
			Assert.AreEqual(expected, actual);

			expected = "2-Dec-2013".ConvertToDate(DateTime.Now);
			startdate = "1 dec 2013".ConvertToDate(DateTime.Now);															 //try for 1st day (sunday), but should be monday (2)
			actual = Dates.GetFirstBusinessDay(startdate.Year, startdate.Month);

			Assert.AreEqual(expected, actual);
			Assert.AreEqual(expected, startdate.GetFirstBusinessDayOfMonth());
		}
		[TestMethod()]
		public void GetLastBusinessDay() {
			DateTime expected = "31-Dec-2013".ConvertToDate(DateTime.Now);
			var startdate = "16 dec 2013".ConvertToDate(DateTime.Now);
			DateTime actual = Dates.GetLastBusinessDay(startdate.Year, startdate.Month);

			Assert.AreEqual(expected, actual);
		}
		[TestMethod()]
		public void GetFollowingMonth20th() {
			DateTime expected = "20-jan-2014".ConvertToDate(DateTime.Now);
			var startdate = "16 dec 2013".ConvertToDate(DateTime.Now);
			DateTime actual = Dates.GetFollowingMonth20th(startdate);

			Assert.AreEqual(expected, actual);
		}
		[TestMethod()]
		public void MonthDifference() {
			DateTime enddate = "20-jan-2014".ConvertToDate(DateTime.Now);
			var startdate = "16 dec 2013".ConvertToDate(DateTime.Now);
			int actual = Dates.MonthDifference(startdate, enddate);
			int expected = 1;
			Assert.AreEqual(expected, actual);

			enddate = "20-jan-2014".ConvertToDate(DateTime.Now);
			startdate = "16 mar 2022".ConvertToDate(DateTime.Now);
			actual = Dates.MonthDifference(startdate, enddate);
			expected = 98;



			Assert.AreEqual(expected, actual);

			enddate = "1-jan-1970".ConvertToDate(DateTime.Now);
			startdate = "16 mar 2022".ConvertToDate(DateTime.Now);
			actual = Dates.MonthDifference(startdate, enddate);
			expected = 626;

			Assert.AreEqual(expected, actual);
		}
	}
}


#endif
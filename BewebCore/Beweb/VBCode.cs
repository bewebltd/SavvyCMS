using System;

namespace Beweb {
	/// <summary>
	/// ASP VB Port
	/// </summary>
	public partial class VB //SavvyPage : System.Web.UI.Page //class CodeLib
	{
		public const string cr = "\r";
		public const string lf = "\n";
		public const string crlf = "\r\n";
		public const string tab = "\t";

		public static DateTime DateNull = new DateTime(1001, 1, 1);
		/// <summary>
		/// Return the left numChars of the source string (from VB)
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public static char chr(int code) {
			char result = (char)code;
			return result;
		}
		public static string left(string source, int numChars) {
			if (source == null) return null;
			if (numChars > source.Length) return source;

			return (numChars < 1) ? "" : source.Substring(0, numChars);
		}


		public static string right(string source, int numChars) {
			int endPoint = source.Length - numChars;
			endPoint = (endPoint >= 0) ? endPoint : 0;
			string str = source.Substring(endPoint);
			return str;
		}


		/// <summary>
		/// find posn of a string in another string (zero based)
		/// </summary>
		/// <param name="source">long str</param>
		/// <param name="findStr">find str</param>
		/// <returns>0 if not found</returns>
		public static int instr(string source, string findStr) {
			if (source.ToLower().IndexOf(findStr) == -1) return 0;
			return source.ToLower().IndexOf(findStr) + 1;
		}

		/// <summary>
		/// get last index of a string
		/// </summary>
		/// <param name="source"></param>
		/// <param name="findStr"></param>
		/// <returns></returns>
		public static int instrrev(string source, string findStr) {
			return source.ToLower().LastIndexOf(findStr) + 1;
		}
		public static string lcase(string source) {
			return source.ToLower();
		}

		/// <summary>
		/// Uppercase a source
		/// </summary>
		/// <param name="source">uppercase version of the string</param>
		/// <returns></returns>
		public static string ucase(string source) {
			return source.ToUpper();
		}
		public static int len(string source) {
			return source.Length;
		}
		public static DateTime DateAdd(string seg, int delta, DateTime startdate) {
			DateTime dt;
			switch (seg) {
				case "d": {
						dt = startdate.AddDays(delta);
						break;

					}
				default: {
						throw new Exception("dateadd seg [" + seg + "] not supported");
					}
			}

			return dt;
		}

		public static string WeekDayName(int dayNumber) {
			string result;
			switch (dayNumber) {
				case 1: {
						result = "sunday";
						break;

					}
				case 2: {
						result = "monday";
						break;
					}
				case 3: {
						result = "tuesday";
						break;
					}
				case 4: {
						result = "wednesday";
						break;
					}
				case 5: {
						result = "thursday";
						break;

					}
				case 6: {
						result = "friday";
						break;

					}
				case 7: {
						result = "saturday";
						break;

					}
				default: {
						throw new Exception("WeekDay unknown [" + dayNumber + "]");
					}
			}

			return result;

		}

		public static int WeekDay(DateTime dt) {
			int result = 1;
			switch (dt.DayOfWeek.ToString().ToLower()) {
				case "sunday": {
						result = 1;
						break;

					}
				case "monday": {
						result = 2;
						break;
					}
				case "tuesday": {
						result = 3;
						break;
					}
				case "wednesday": {
						result = 4;
						break;
					}
				case "thursday": {
						result = 5;
						break;

					}
				case "friday": {
						result = 6;
						break;

					}
				case "saturday": {
						result = 7;
						break;

					}
				default: {
						throw new Exception("WeekDay unknown [" + dt.DayOfWeek.ToString().ToLower() + "]");
					}
			}

			return result;
		}

		public static string Day(string dateText) {
			if (dateText == "") return "";
			DateTime dt = Convert.ToDateTime(dateText);
			string result = dt.Day.ToString();
			return result;
		}
		public static int Day(DateTime dt) {
			int result = dt.Day;
			return result;
		}
		public static int Month(string dateText) {
			//if (dateText == "") return "";
			DateTime dt = Convert.ToDateTime(dateText);
			int result = dt.Month;
			return result;
		}
		public static int Hour(string dateText) {
			if (dateText == "") return 0;
			DateTime dt = Convert.ToDateTime(dateText);
			return dt.Hour;
		}
		public static int Minute(string dateText) {
			if (dateText == "") return 0;
			DateTime dt = Convert.ToDateTime(dateText);
			return dt.Minute;
		}

		/// <summary>
		/// return the current year as a string
		/// </summary>
		/// <param name="dateText"></param>
		/// <returns></returns>
		public static string Year(string dateText) {
			if (dateText == "") return "";
			DateTime dt = Convert.ToDateTime(dateText);
			string result = dt.Year.ToString();
			return result;
		}
		public static string Now() {
			string result = DateTime.Now.ToLongDateString(); //Fmt.FmtDate(DateTime.Now.ToLongDateString());
			return result;
		}
		public static string MonthName(int month, bool isAbbreviated) {
			return left(MonthName(month), 3);
		}

		public static string MonthName(int month) {
			int? newMonth = month;
			return MonthName(newMonth);
		}
		/// <summary>
		/// pass in a 1->12 number to get the month name
		/// </summary>
		/// <param name="month"></param>
		/// <returns></returns>
		public static string MonthNameAbbrev(int month) {
			int? newMonth = month;
			return MonthNameAbbrev(newMonth);
		}

		public static string MonthNameAbbrev(int? month) {
			return (System.Globalization.DateTimeFormatInfo.CurrentInfo != null && month.HasValue && month > 0) ? System.Globalization.DateTimeFormatInfo.CurrentInfo.AbbreviatedMonthNames[month.Value - 1] : "";
		}
		public static string MonthName(int? month) {
			return (System.Globalization.DateTimeFormatInfo.CurrentInfo != null && month.HasValue && month > 0) ? System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames[month.Value - 1] : "";
		}
		public static string MonthName(string dateText) {
			if (dateText == "") return "";
			DateTime dt = Convert.ToDateTime(dateText);
			return MonthName(dt.Month);
		}
		public static string MonthName(DateTime? date) {
			if (date == null) return "";
			return MonthName(date.Value.Month);
		}
		public static string trim(string source) {
			if (source == null) return "";
			return source.Trim();
		}

		static Random randomGenerator = new Random();

		/// <summary>
		/// get a number between 0 and 1
		/// </summary>
		/// <returns>return number less than 1</returns>
		public static float rnd() {
			float res = randomGenerator.Next(200);
			res = (res / 200);
			return res;//return number less than 1
		}

		public static bool isNumeric(string source) {
			try {
				int rs = (Convert.ToInt32(source));
			} catch (Exception)//ex
			{
				//dout("isNumeric ex[" + ex.Message + "]");
				return false;
			}
			return true;

		}
		public static string replace(string source, string find, string replace) {
			if (source == null) return source;
			if (find == null) return source;
			if (replace == null) return source;
			if (find.Length == 0) return source;
			if (replace.Length == 0) return source;
			return source.Replace(find, replace);
		}

		/// <summary>
		/// return right side of string starting st 1 based start
		/// </summary>
		/// <param name="source">string to return right end of</param>
		/// <param name="start">1 based start</param>
		/// <returns>right end of string from start</returns>
		public static string mid(string source, int start) {
			return (start - 1 > source.Length) ? "" : source.Substring(start - 1);
		}
		public static string mid(string source, int start, int numChars) {
			numChars = (source.Length < numChars) ? source.Length - 1 : numChars;
			return (start < source.Length) ? source.Substring(start, numChars) : "";
		}

		/// <summary>
		/// safe convert to int, returns 0 if bad
		/// </summary>
		/// <param name="source">string to convert to number</param>
		/// <returns></returns>
		public static int cint(string source) {
			int result = 0;
			try { result = Convert.ToInt32(source); } catch (Exception e) { Console.WriteLine(e.Message); }
			return result;
		}

		//public static bool cbool(string source)
		//{
		//	int result = 0;
		//	try { result = Convert.ToBoolean(source); }
		//	catch (Exception e) { Console.WriteLine(e.Message); }
		//	return result;
		//}
		/// <summary>
		/// Convert the source to a bool, or return n/a if blank. This includes values: true, yes, on, 1
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static bool cbool(string source) {
			bool result = false;
			result = (source.ToLower() == "true" || source.ToLower() == "on" || source.ToLower() == "1" || source.ToLower() == "yes") ? true : false;
			return result;
		}
		/// <summary>
		/// Return true of source is empty
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static bool IsNull(string source) {
			bool result = false;
			result = (source.Trim() == "") ? true : (source.ToString() == null) ? true : false;
			return result;
		}
		public static bool IsNull(DateTime source) {
			bool result = false;
			result = (source.Equals(DateNull));
			return result;
		}
		public static int ubound(string[] ar) {
			return ar.Length;
		}

		/// <summary>
		/// asp vb mod function
		/// </summary>
		/// <param name="v1">divisor</param>
		/// <param name="v2">quotiant</param>
		/// <returns>remainder</returns>
		public static int mod(int v1, int v2) {
			return v1 % v2;
		}

		public static int fix(double src) {
			return Convert.ToInt32(Math.Floor(src));
		}

		public static string[] split(string txt, string p) {
			return txt.Split(p[0]);
		}
		public static string[] split(string txt, string p, int ignore1, int ignore2) {
			return txt.Split(p[0]);
		}

		public static DateTime cdate(string d) {
			if (d == "") return DateNull;
			return Convert.ToDateTime(d);
		}
		public static bool isdate(string d) {
			bool result = true;
			try {
				DateTime tempdate = Convert.ToDateTime(d);
			} catch (Exception) {
				result = false;
			}
			return result;
		}

		public static int hour(string dateTime) {
			int result = 0;
			if (dateTime != "") { result = cdate(dateTime).Hour; }
			return result;
		}
		public static int minute(string dateTime) {
			int result = 0;
			if (dateTime != "") { result = cdate(dateTime).Minute; }
			return result;
		}

		public static int DatePart(string seg, string dateTime) {
			DateTime dt = cdate(dateTime);
			int result = 0;
			switch (seg) {
				case "h": {
						result = dt.Hour;
						break;
					}
				case "n": {
						result = dt.Minute;
						break;
					}
				default: {
						throw new Exception("date part seg [" + seg + "] not supported");
					}
			}

			return result;
		}

		public static long clng(string source) {
			long result = 0;
			try { result = Convert.ToInt64(source); } catch (Exception e) { Console.WriteLine(e.Message); }
			return result;
		}

		public static string round(int p, int prec) {
			return "" + Math.Round((decimal)p, prec);
		}

	}
}

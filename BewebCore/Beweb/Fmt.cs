#define MVC
#define Fmt
#define BaseTypeExtensions
#define TestExtensions
#define ActiveRecord
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web;
using System.Text;
using System.Globalization;
using System.Configuration;
using System.Linq;
using Beweb;
using JetBrains.Annotations;

namespace Beweb {
	/// <summary>
	/// Formatting helper functions
	/// </summary>
	public class Fmt {  // 20130702 MN changed to non-partial		
		public static bool DefaultDateFormatHasDashes = true;

		#region OutputImage
		/// <summary>
		/// takes a filename of a file uploaded to the system and adds the correct filepath
		/// </summary>
		/// <param name="FileName"></param>
		/// <returns>filename with path prefix</returns>
		public static string OutputImage(string FileName) {
			return OutputImage(FileName, "blank.gif");
		}
		public static string OutputImage(string FileName, string defaultImage) {
			if (FileName == "" || FileName == null) {
				return "attachments/" + defaultImage;
			} else {
				return "attachments/" + FileName;
			}
		}
		#endregion

		#region CleanString
		/// <summary>
		/// Cleans a string - only allows alpha numeric and a couple of other characters ( ,.-)
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string CleanString(string s) {
			// DO NOT change this default to include apostrophes or quotes - or it will open up SQL injection holes everywhere
			return CleanString(s, @"[^a-z A-Z0-9,.-]");
		}
		/// <summary>
		/// Remove all characters other than alphanumerics. Removes any spaces, dots and any other punctuation.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string CleanAlphaNumeric(string s) {
			if (s == null) return null;
			var sb = new StringBuilder(s.Length);
			foreach (var c in s) {
				if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9')) {
					sb.Append(c);
				}
			}
			return sb.ToString();
		}



		/// <summary>
		/// Remove all characters other than digits. Removes any spaces, dots and any other punctuation.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string CleanDigits(string s) {
			if (s == null) return null;
			var sb = new StringBuilder(s.Length);
			foreach (var c in s) {
				if (c >= '0' && c <= '9') {
					sb.Append(c);
				}
			}
			return sb.ToString();
			//return CleanString(s, @"[^0-9]");
		}

		/// <summary>
		/// Returns a string from a with leading zeros added.
		/// </summary>
		/// <param name="i"></param>
		/// <param name="totalLength">The length of the string returned</param>
		/// <returns> A converted string with leading zeros added.</returns>
		public static string PadZeros(int? i, int totalLength) {
			return (i ?? 0).ToString().PadLeft(totalLength, '0');
		}

		public static decimal CleanNumber(string s) {
			// to be super safe - characters all have to be added here
			//return (CleanString(s, @"[^0-9.-]")).ToDecimal(0);  // 0 added for clarity (no change in functionality)					 //20120105JN added - to regex to allow negatives
			if (s == null) return 0;
			var sb = new StringBuilder(s.Length);
			bool minusAllowed = true;
			bool doneDot = false;
			foreach (var c in s) {
				if (c >= '0' && c <= '9') {
					minusAllowed = false;
					sb.Append(c);
				} else if (c == '.' && !doneDot) {
					sb.Append('.');
					minusAllowed = false;
					doneDot = true;
				} else if (c == '-' && minusAllowed) {
					sb.Append('-');
					minusAllowed = false;
				} else {
					// skip it
				}
			}
			s = sb.ToString();
			return s.ToDecimal(0);
		}

		/// <summary>
		/// Takes a string and returns an int. Given a string with extra characters, it will strip out any other characters.
		/// If the string contains a dot, only numbers before the dot will be returned (eg $5.99 returns 5).
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static int CleanInt(string s) {
			// to be super safe - characters all have to be added here
			// 20110323 MN - breaking change - if supply $5.00 it used to return 500, now it returns 5
			//if (s.Contains(".")) s = s.Split('.')[0];
			//return (CleanString(s, @"[^0-9-]")).ToInt(0);  // 0 added for clarity (no change in functionality)		 //20120105JN added - to regex to allow negatives
			// 20121029 - MN/JN - fixed issue with dash causing all characters to slip through and therefore return zero
			if (s == null) return 0;
			var sb = new StringBuilder(s.Length);
			bool minusAllowed = true;
			foreach (var c in s) {
				if (c >= '0' && c <= '9') {
					sb.Append(c);
					minusAllowed = false;
				} else if (c == '.') {
					// strip off anything after decimal point
					break;
				} else if (c == '-' && minusAllowed) {
					sb.Append('-');
					minusAllowed = false;
				} else {
					// skip it
				}
			}
			int i = sb.ToString().ToInt(0);

			//var num = CleanNumber(s);
			//int i = Numbers.Floor(num);
			return i;
		}

		/// <summary>
		/// Cleans a string - using a custom expression
		/// </summary>
		/// <param name="s">the string to clean</param>
		/// <param name="expression">a character class of allowed characters - all others are removed. i.e. [^a-z] keeps lower case alpha only </param>
		/// <returns></returns>
		//public static string CleanString(string s, string expression)
		//{
		//  string returnValue = "";
		//  if (!String.IsNullOrEmpty(s))
		//  {
		//	Regex r1 = new Regex(expression, RegexOptions.Multiline);
		//	returnValue = r1.Replace(s, "");
		//  }
		//}
		/// <summary>
		/// Cleans a string - using a custom expression
		/// </summary>
		/// <param name="s">the string to clean</param>
		/// <param name="expression">a character class of allowed characters - all others are removed. i.e. [^a-z] keeps lower case alpha only </param>
		/// <returns></returns>
		public static string CleanString(string s, string expression) {
			string returnValue = "";
			if (!String.IsNullOrEmpty(s)) {
				Regex r1 = new Regex(expression, RegexOptions.Multiline);
				returnValue = r1.Replace(s, "");
			}
			return returnValue;
		}

		public static string CleanStringChars(string s, string onlyTheseChars) {
			if (s == null) return null;
			var sb = new StringBuilder(s.Length);
			foreach (var c in s) {
				if (onlyTheseChars.Contains(c)) {
					sb.Append(c);
				}
			}
			return sb.ToString();
		}

		#endregion

		/// <summary>
		/// crunch string into format for url
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string Crunch(string s) {
			string returnValue = "";

			if (!String.IsNullOrEmpty(s)) {
				returnValue = s.ToLower().Trim();

				//	returnValue = s.ToLower().Trim();
				//	returnValue = returnValue.Replace("&", "and");
				//	returnValue = returnValue.Replace(".", "-");
				//	returnValue = returnValue.Replace(" ", "-");
				//	returnValue = returnValue.Replace("_", "-");
				//	returnValue = Fmt.CleanString(returnValue, "[^a-z0-9-]");
				//	while (returnValue.Contains("--")) returnValue = returnValue.Replace("--", "-");
				//	returnValue = returnValue.TrimEnd('-');

				var sb = new StringBuilder(s.Length);
				bool prevDash = false;
				foreach (var c in returnValue) {
					if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9')) {
						sb.Append(c);
						prevDash = false;
					} else if ((c == '.' || c == ' ' || c == '_' || c == '-') && !prevDash) {
						sb.Append('-');
						prevDash = true;
					} else if (c == '&') {
						sb.Append("and-");
						prevDash = true;
					}
				}
				returnValue = sb.ToString().TrimEnd('-');
			}

			return returnValue;
		}

		/// <summary>
		/// Remove characterrs that arent + or digits. useful for nz phone numbers (note brackets are also removed)
		/// </summary>
		/// <param name="phone"></param>
		/// <returns></returns>
		public static string CleanMobile(string phone) {
			var countryCode = "+64";
			phone = Regex.Replace(phone, @"[^\d]", "");
			if (phone.StartsWith("0")) {
				phone = phone.RemoveCharsFromStart(1);
				phone = countryCode + phone;
			} else {
				phone = "+" + phone;
			}
			return phone;
		}

		#region Json
#if BaseTypeExtensions
		/// <summary>
		/// Correctly escapes value for jquery 1.4 using jsenquote. Wraps the string in apostrophes and escapes any characters that would break the value
		/// </summary>
		/// <param name="Value"></param>
		/// <returns></returns>
		public static string Json(string Value) {
			//return String.Format("'{0}'", Value.Replace("\\", "\\\\").Replace("\r\n", "").Replace("'", "\\'"));	  -- not compatible with JSON standard used in jQuery 1.4
			return Value.JsEnquote();
		}
#endif
		public static string Json(object Value) {
			if (Value == null) return "";

			return Json(Value.ToString());
		}
		#endregion

		#region FmtSql
		// MN 20130702 - moved all FmtSql functions back from SqlStringBuilder partial class to this file

		/// <summary>
		/// Enquote a GUID properly to include in a SQL string
		/// </summary>
		/// <param name="guid">A GUID of type System.Guid</param>
		/// <returns></returns>
		public static string SqlGuid(System.Guid guid) {
			// on sql server it is the same as a string (ie quoted) - if using Access we need to extend this
			return Fmt.SqlGuid("{" + guid.ToString().ToUpperInvariant() + "}");
		}

		/// <summary>
		/// Enquote a GUID properly to include in a SQL string
		/// </summary>
		/// <param name="guid">A string that should contain a GUID</param>
		/// <returns></returns>
		public static string SqlGuid(string guid) {
			// on sql server it is the same as a string (ie quoted) - if using Access we need to extend this
			if (guid.Contains("'")) throw new BewebDataException("An apostrophe was detected in a GUID by Beweb.Fmt.SqlGuid(). This is invalid and may be SQL injection. GUIDs cannot contain an apostrophe.");
			if (!guid.Trim().IsGuid()) throw new BewebDataException("An invalid GUID was detected by Beweb.Fmt.SqlGuid(). The invalid GUID was " + guid + ".");

			return "'" + guid.Trim() + "'";
		}

		/// <summary>
		/// convert datetime to '22-Jan-2008 10:57 am' format
		/// </summary>
		/// <param name="d">source date</param>
		/// <returns>standard format</returns>
		public static string SqlDateTime(string d) {
			return Fmt.SqlDateTime(System.DateTime.Parse(d));
		}

		/// <summary>
		///	convert a date to a common format string that sql server can read
		/// </summary>
		/// <param name="d">1 apr 2010</param>
		/// <returns>01 Apr 2009 12:00:00.000 am</returns>
		public static string SqlDateTimeLong(DateTime d) {
			return "'" + d.ToString("dd MMM yyyy hh:mm:ss.fff t") + "m'";
			//return "'"+Fmt.DateTime(d)+"'";
		}

		/// <summary>
		///	convert a date to a common format string that sql server can read
		/// </summary>
		/// <param name="d">1 apr 2010 5:19pm</param>
		/// <returns>01-Apr-2010 05:19 pm</returns>
		public static string SqlDateTime(DateTime d) {
			//return "'" +d.ToString("dd-MMM-yyyy hh:mm t") + "m" + "'"; //add am/pm
			return "'" + Fmt.DateTime(d, Fmt.DateTimePrecision.Millisecond) + "'";
		}

		public static bool SqlDateIncludesTime = false;   // MN 20130702 added Fmt.SqlDateIncludesTime for backwards compatibility - you must set this true for projects prior to 2012 to keep existing behaviour, or find all references

		/// <summary>
		///	convert a date to a common format string that sql server can read
		/// </summary>
		/// <param name="d">1 apr 2010</param>
		/// <returns>01-Apr-2009</returns>
		public static string SqlDate(DateTime d) {
			if (SqlDateIncludesTime) {
				return "'" + Fmt.DateTime(d) + "'";
			} else {
				return "'" + d.ToString("dd-MMM-yyyy") + "'";
			}
		}
		#endregion

		/// <summary>
		/// Beweb standard date format - eg 01-Apr-2009 11:59pm.
		/// Does not display time if datetime has no time (ie time is 0:00 midnight).
		/// Does not depend on server settings or culture.
		/// Good for user display, able to be used in URLs and form submitted.
		/// However: not able to be parsed by Javascript - use Fmt.JsDate instead.
		/// </summary>
		/// <param name="date">A nullable date type</param>
		/// <returns>Returns a string version of the date</returns>
		public static string DateTime(DateTime? date) {
			return Fmt.DateTime(date, DateTimePrecision.Minute);
		}

		/// <summary>
		/// Beweb standard date format - eg 01-Apr-2009 11:59pm.
		/// Does not display time if datetime has no time (ie time is 0:00 midnight).
		/// Does not depend on server settings or culture.
		/// Good for user display, able to be used in URLs and form submitted.
		/// However: not able to be parsed by Javascript - use Fmt.JsDate instead.
		/// </summary>
		/// <param name="date">A nullable date type</param>
		/// <param name="displayPrecision">An enum value from Fmt.DateTimePrecision (eg whether to display minutes, seconds or milliseconds)</param>
		/// <returns>Returns a string version of the date</returns>
		public static string DateTime(DateTime? date, DateTimePrecision displayPrecision) {
			string result;
			if (date == null) {
				result = "";
			} else {
				DateTime dt = date.Value; // this gets the non-nullable value out of nullable date
				DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
				if (displayPrecision == DateTimePrecision.Year) {
					// just year
					result = dt.Year.ToString();
				} else if (displayPrecision == DateTimePrecision.Month) {
					// just month and year		  
					result = dtfi.AbbreviatedMonthNames[dt.Month - 1] + " " + dt.Year;
				} else {
					if (Fmt.DefaultDateFormatHasDashes) {
						result = "" + dt.Day.ToString().PadLeft(2, '0') + "-" + dtfi.AbbreviatedMonthNames[dt.Month - 1] + "-" + dt.Year;
					} else {
						result = "" + dt.Day.ToString() + " " + dtfi.AbbreviatedMonthNames[dt.Month - 1] + " " + dt.Year;
					}
				}
				if (displayPrecision >= DateTimePrecision.Hour && (dt.Hour > 0 || dt.Minute > 0 || dt.Second > 0)) {
					result += " " + Fmt.Time(dt, displayPrecision);        // MN 20120202 - added space here instead of in Fmt.Time
				}
			}

			return result;
		}

		public static string DateTime() {
			return DateTime(System.DateTime.Now);
		}

		public static string DateTime(string date) {
			string result;
			if (String.IsNullOrEmpty(date)) {
				result = "";
			} else {
				DateTime dt = Convert.ToDateTime(date);
				result = Fmt.DateTime(dt);
			}
			return result;
		}

		/// <summary>
		/// Formats a list of dates that are concatenated by a separator.
		/// </summary>
		/// <param name="datelist">The list of dates in string format</param>
		/// <param name="separator">The separator char to split by</param>
		/// <returns>A list of formatted dates separated by the same separator.</returns>
		public static string DateTimeList(string datelist, char separator) {
			List<string> datestrings = datelist.Split(separator).ToList();
			string result = "";
			foreach (string date in datestrings) {
				result += DateTime(date) + separator;
			}

			return result.Substring(0, result.Length - 1);	// takes care of the last separator
		}

		public static string Date(object date) {
			if (date is DateTime || date is Nullable<DateTime>) {
				return Fmt.Date((DateTime?)date);
			} else {
				return Fmt.Date(date + "");
			}
		}

		/// <summary>
		/// Beweb standard date format - eg 01-Apr-2008.
		/// Does not depend on server settings or culture.
		/// Good for user display, able to be used in URLs and form submitted.
		/// However: not able to be parsed by Javascript - use Fmt.JsDate instead.
		/// Use fmt.DateTime if you want the time.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string Date(DateTime? date) {
			return Fmt.Date(date, DateTimePrecision.Day);
		}

		public static string Date(DateTime date) {
			return Fmt.Date(date, DateTimePrecision.Day);
		}

		//public static string MonthYear(DateTime? date) {  //20140911 jn changed to simpler version
		//	return Fmt.Date(date, DateTimePrecision.Month);
		//}
		public static string DayMonth(DateTime? date) {
			return String.Format("{0:dd MMM}", date);
		}

		public static string MonthYear(DateTime? date) {
			return String.Format("{0:MMM yyyy}", date);
		}
		public static string YearMonth() {
			//DateTime today = new DateTime();
			return String.Format("{0:yyyyMMM}", System.DateTime.Now);
		}

		public static string YearMonth(DateTime? date) {
			return String.Format("{0:yyyyMMM}", date);
		}
		/// Beweb standard date format - eg 01-Apr-2008.
		/// Does not depend on server settings or culture.
		/// Good for user display, able to be used in URLs and form submitted.
		/// However: not able to be parsed by Javascript - use Fmt.JsDate instead.
		/// Use fmt.DateTime if you want the time.
		/// </summary>
		/// <param name="date"></param>
		/// <param name="displayPrecision">An enum value from Fmt.DateTimePrecision (eg whether to display minutes, seconds or milliseconds)</param>
		/// <returns></returns>
		public static string Date(DateTime? date, DateTimePrecision displayPrecision) {
			return Fmt.DateTime(date, displayPrecision);
		}

		/// <summary>
		/// Returns datetime as per ISO 8601 standard, used by W3C and compatible with RFC 3339. (note: has no timezone)
		/// eg 1994-11-05T13:15:30
		/// </summary>
		public static string DateTimeISO(DateTime? date) {
			string result;
			if (date == null) {
				result = "";
			} else {
				result = date.Value.ToString("s");
			}
			return result;

			// note this may be better: return date.ToString("yyyy-MM-ddTHH:mm:ss");
		}

		/// <summary>
		/// Returns datetime as per ISO 8601 standard, used by W3C and compatible with RFC 3339.
		/// Converts a local datetime to UTC and includes "Z" suffix (zulu time = UTC).
		/// eg 1994-11-05T13:15:30Z
		/// </summary>
		public static string DateTimeISOZ(DateTime? date) {
			string result;
			if (date == null) {
				result = "";
			} else {
				// maybe add this? -- date = new DateTime(date.Value.Ticks, DateTimeKind.Local);
				result = date.Value.ToUniversalTime().ToString("s") + "Z";
				// or this: return date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
			}
			return result;
		}

		/// <summary>
		/// Returns date as YYYY-MM-DD (without time) as per ISO 8601 standard, used by W3C and compatible with RFC 3339.
		/// eg 1994-11-05
		/// </summary>
		public static string DateISO(DateTime? date) {
			string result;
			if (date == null) {
				result = "";
			} else {
				result = date.Value.ToString("yyyy-MM-dd");
			}
			return result;
		}

		public static string DateForJS(DateTime? date) {
			return DateForJS(date, DateTimePrecision.Day);
		}

		public static string DateForJS(DateTime? date, DateTimePrecision displayPrecision) {
			return Fmt.Date(date, displayPrecision).Replace("-", " ");
		}

		public static string DateScriptForJS(DateTime? date) {
			return DateScriptForJS(date, DateTimePrecision.Day);
		}

		/// <summary>
		/// Returns date javascript eg new Date(2013,5,10,12,45,500)
		/// </summary>
		/// <param name="date"></param>
		/// <param name="displayPrecision"></param>
		/// <returns></returns>
		public static string DateScriptForJS(DateTime? date, DateTimePrecision displayPrecision) {
			string result = "";
			if (date != null) {
				DateTime dt = date.Value; // this gets the non-nullable value out of nullable date
				//DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
				//result = "" + dt.Day.ToString().PadLeft(2, '0') + " " + dtfi.AbbreviatedMonthNames[dt.Month - 1] + ", " + dt.Year;
				//if (displayPrecision >= DateTimePrecision.Hour && (dt.Hour > 0 || dt.Minute > 0 || dt.Second > 0)) {
				//  result += Fmt.Time(dt, displayPrecision);
				//}
				//TimeSpan timeSpanSince1970 = System.DateTime.UtcNow - new System.DateTime(1970, 1, 1);
				//const int millisecondsPerJavascriptDay = 86400000;
				//Int64 millisecondsSince1970 = Convert.ToInt64(timeSpanSince1970.TotalDays) * millisecondsPerJavascriptDay);
				//result = "new Date("+millisecondsSince1970+")";

				result = "new Date(";
				if (displayPrecision >= DateTimePrecision.Year) result += dt.Year;
				if (displayPrecision >= DateTimePrecision.Month) result += "," + (dt.Month - 1);
				if (displayPrecision >= DateTimePrecision.Day) result += "," + dt.Day;
				if (displayPrecision >= DateTimePrecision.Hour) result += "," + dt.Hour;
				if (displayPrecision >= DateTimePrecision.Minute) result += "," + dt.Minute;
				if (displayPrecision >= DateTimePrecision.Second) result += "," + dt.Second;
				if (displayPrecision >= DateTimePrecision.Millisecond) result += "," + dt.Millisecond;
				result += ")";
			}

			return result;
		}
		public static string Date(string date) {
			string result;
			if (date == null || date.Trim() == "") {
				result = "";
			} else {
				DateTime dt = Convert.ToDateTime(date);
				result = Fmt.Date(dt);
			}
			return result;
		}

		public enum DateTimePrecision {
			Year, Month, Day, Hour, Minute, Second, Millisecond
		}

		/// <summary>
		/// Beweb standard time format optionally including seconds & milliseconds - eg 12:52pm.
		/// Does not depend on server settings or culture.
		/// Good for user display.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string Time(DateTime? date, DateTimePrecision displayPrecision) {
			return Time(date, displayPrecision, false);
		}

		/// <summary>
		/// Beweb standard time format optionally including seconds & milliseconds - eg 12:52pm.
		/// Does not depend on server settings or culture.
		/// Good for user display.
		/// If use24HourClock=true this can be used as HTML5 input value format.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string Time(DateTime? date, DateTimePrecision displayPrecision, bool use24HourClock) {
			string result = "";
			if (date != null) {
				DateTime dt = date.Value; // this gets the non-nullable value out of nullable date
				int h = dt.Hour;
				string ampm = "";
				if (!use24HourClock) {
					ampm = "am";
					if (h > 12) {
						h = h - 12;
						ampm = "pm";
					} else if (h == 12) {
						ampm = "pm";
					} else if (h == 0) {
						h = 12;
						ampm = "am";
					}
				}
				if (displayPrecision >= DateTimePrecision.Hour) {
					result += h;       // MN 20120202 - moved the leading space into Fmt.DateTime
				}
				if (displayPrecision >= DateTimePrecision.Minute) {
					result += ":" + VB.right("0" + dt.Minute, 2);
				}
				if (displayPrecision >= DateTimePrecision.Second) {
					result += ":" + VB.right("0" + dt.Second, 2);
				}
				if (displayPrecision >= DateTimePrecision.Millisecond) {
					result += "." + VB.right("00" + dt.Millisecond, 3);
				}
				result += ampm;
			}

			return result;
		}

		/// <summary>
		/// Beweb standard time format - eg 12:52pm.
		/// Does not depend on server settings or culture.
		/// Good for user display.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string Time(DateTime? date) {
			return Fmt.Time(date, DateTimePrecision.Minute);
		}
		public static string Time(string date) {
			string result;
			if (date == null) {
				result = "";
			} else {
				DateTime dt = Convert.ToDateTime(date);
				result = Fmt.Time(dt);
			}
			return result;
		}

		/// <summary>
		/// Beweb standard time format - eg 12:52pm.
		/// Does not depend on server settings or culture.
		/// Good for user display.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string Time(DateTime? date, bool use24HourClock) {
			return Fmt.Time(date, DateTimePrecision.Minute, use24HourClock);
		}
		public static string Time(string date, bool use24HourClock) {
			string result;
			if (date == null) {
				result = "";
			} else {
				DateTime dt = Convert.ToDateTime(date);
				result = Fmt.Time(dt, use24HourClock);
			}
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		[Obsolete("This has been renamed to Fmt.DateTime, use Fmt.DateTime instead")]
		public static string bwbDateTime(DateTime d) {
			//	' date in 18-Feb-2000 4:50pm format
#pragma warning disable 618,612
			string str = bwbDate(d.ToLongDateString());
#pragma warning restore 618,612
			string dt = d.ToLongTimeString();
			if (VB.Hour(dt) > 0 || VB.Minute(dt) > 0) {
				string ampm;
				int h;
				ampm = "am";
				h = VB.Hour(dt);
				if (h > 12) {
					h = h - 12;
					ampm = "pm";
				} else if (h == 12) {
					ampm = "pm";
				} else if (h == 0) {
					h = 12;
					ampm = "am";
				}
				str = str + " " + h + ":" + VB.right("0" + VB.Minute(dt), 2) + ampm;
			}

			return str;
		}


		/// <summary>
		/// Format a date into dd-mmm-yyyy given a string
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		[Obsolete("This has been renamed to Fmt.Date, use Fmt.Date instead")]
		public static string bwbDate(string date) {
			string result = "";

			// date in unambiguous 18-Feb-2000 format
			if (date == "") {
				result = "";
			} else {
				DateTime dt = Convert.ToDateTime(date);

				DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
				result = "" + dt.Day.ToString().PadLeft(2, '0') + "-" + dtfi.AbbreviatedMonthNames[dt.Month - 1] + "-" + dt.Year;
			}

			return result;
		}

		public static string LongDate(Object date) {
			return LongDate(date, false);
		}

		public static string LongDate(Object date, bool includeDayOfWeek) {
			return LongDate((DateTime)date, includeDayOfWeek);
		}

		public static string LongDate(DateTime dt) {
			return LongDate(dt, false);
		}

		public static string LongDate(DateTime dt, bool includeDayOfWeek) {
			DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
			string str = "";
			if (includeDayOfWeek) {
				str += Fmt.DayOfWeek(dt) + ", ";
			}
			if (DefaultDateFormatHasDashes) {
				str += dt.Day.ToString().PadLeft(2, '0') + "-" + dtfi.MonthNames[dt.Month - 1] + "-" + dt.Year;
			} else {
				str += dt.Day.ToString() + " " + dtfi.MonthNames[dt.Month - 1] + " " + dt.Year;
			}
			return str;
		}

		public static string DayOfWeek(DateTime dt) {
			return dt.DayOfWeek.ToString();
		}

		public static string DayAbbrev(DateTime dt) {
			return dt.DayOfWeek.ToString().Left(3);
		}

		public static string LongDate(string date) {
			string result = "";

			// date in unambiguous 18-February-2000 format
			if (date == "") {
				result = "";
			} else {
				DateTime dt = Convert.ToDateTime(date);
				result = LongDate(dt);
			}

			return result;
		}

		public static string ShortDateTime(DateTime? dateTime) {
			return Fmt.ShortDate(dateTime) + " " + Fmt.Time(dateTime);
		}

		public static string DayDateTime24(DateTime? dateTime) {
			return DayDateTime(dateTime, true);
		}

		public static string DayDateTime(DateTime? dateTime) {
			return DayDateTime(dateTime, false);
		}

		public static string DayDateTime(DateTime? dateTime, bool use24HourClock) {
			if (dateTime == null) return "";
			var dt = dateTime.Value;
			string result = Fmt.DayAbbrev(dateTime.Value) + " " + Fmt.ShortDate(dateTime);
			if (dt.Hour > 0 || dt.Minute > 0 || dt.Second > 0 || dt.Millisecond > 0) {
				result += " " + Fmt.Time(dateTime, use24HourClock);
			}
			return result;
		}

		/// <summary>
		/// Formats the date in 20071128 format - good for sorting
		/// </summary>
		/// <param name="date">date to be parsed</param>
		/// <returns>20071128</returns>
		public static string DateCompressed(string date) {
			string result = "";

			if (date == "") {
				result = "";
			} else {
				DateTime dt = Convert.ToDateTime(date);

				result = DateCompressed(dt);//dt.Year + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0');
			}

			return result;
		}
		/// <summary>
		/// Formats the date in 20071128 format - good for sorting
		/// </summary>
		/// <param name="dt">date to be parsed</param>
		/// <returns>20071128</returns>
		public static string DateCompressed(DateTime dt) {
			return dt.Year + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0'); ;
		}

		/// <summary>
		/// Return date in format 200908071302
		/// </summary>
		/// <param name="dt"></param>
		/// <returns>200908071302</returns>
		public static string DateTimeCompressed(DateTime dt) {
			string result = "";
			int hour = dt.Hour;
			result = dt.Year.ToString().PadLeft(4, '0') + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0');
			result += hour.ToString().PadLeft(2, '0') + dt.Minute.ToString().PadLeft(2, '0');
			return result;
		}


		/// <summary>
		/// Return date in format 200908071302
		/// </summary>
		/// <param name="dt"></param>
		/// <returns>200908071302</returns>
		public static string DateTimeCompressed(string datetime) {
			string result = "";

			if (datetime == "") {
				result = "";
			} else {
				DateTime dt = Convert.ToDateTime(datetime);
				result = DateTimeCompressed(dt);
			}

			return result;
		}



		[Obsolete("This has been renamed to Fmt.Date")]
		public static string FmtDate(DateTime dt) {
			DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
			return "" + dt.Day.ToString().PadLeft(2, '0') + "-" + dtfi.AbbreviatedMonthNames[dt.Month - 1] + "-" + dt.Year;
		}
		[Obsolete("This has been renamed to Fmt.Date")]
		public static string FmtDate(string date) {
			string result = "";

			// date in unambiguous 18-Feb-2000 format
			if (date == "") {
				result = "";
			} else {
				DateTime dt = Convert.ToDateTime(date);

				DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
				result = "" + dt.Day.ToString().PadLeft(2, '0') + "-" + dtfi.AbbreviatedMonthNames[dt.Month - 1] + "-" + dt.Year;
			}

			return result;
		}

		#region SplitTitleCase
		/// <summary>
		/// takes a string and separates words where the case changes or where it turns to a number
		/// e.g. ThisIsTitle9Case becomes This Is Title 9 Case
		/// </summary>
		/// <param name="s">text to format</param>
		/// <returns></returns>
		public static string SplitTitleCase(string s) {
			string returnValue = s;
			if (s != null) {
				// find e3C in Picture3Caption
				Regex r1 = new Regex(@"([a-z])([0-9]+)([A-Z])", RegexOptions.Multiline);
				returnValue = r1.Replace(returnValue, "$1 $2 $3");
				// find e3, 2C, nL in Picture32CaptionLong
				Regex r2 = new Regex(@"([a-z0-9])([A-Z0-9])", RegexOptions.Multiline);
				returnValue = r2.Replace(returnValue, "$1 $2");
			}
			return returnValue;
		}
		#endregion


		[Obsolete("This has been renamed to Fmt.DateTime")]
		public static string FmtDateTime(DateTime d) {
			//return FmtDateTime(d.ToLongDateString()+" "+d.ToLongTimeString());
			return VB.right("0" + d.Day, 2) + "-" + VB.MonthName(d.Month, true) + "-" + d.Year + " " + Fmt.FmtTime(d.ToLongTimeString());
		}
		//public static string FmtDateTime(DateTime d)
		//{ 

		//}

		[Obsolete("This has been renamed to Fmt.DateTime")]
		public static string FmtDateTime() {
			return FmtDateTime(System.DateTime.Now);
		}
		[Obsolete("This has been renamed to Fmt.DateTime")]
		public static string FmtDateTime(string d) {
			//	' date in 18-Feb-2000 4:50pm format
			string str = FmtDate(d) + " " + FmtTime(d);
			return str;
		}
		[Obsolete("This has been renamed to Fmt.Time")]
		public static string FmtTime(string d) {
			string str = "";
			if (VB.Hour(d) > 0 || VB.Minute(d) > 0) {
				string ampm;
				int h;
				ampm = "am";
				h = VB.Hour(d);
				if (h > 12) {
					h = h - 12;
					ampm = "pm";
				} else if (h == 12) {
					ampm = "pm";
				} else if (h == 0) {
					h = 12;
					ampm = "am";
				}
				str = str + h + ":" + VB.right("0" + VB.Minute(d), 2) + ampm;
			}

			return str;
		}


		public static string ShortDate(DateTime? dt) {
			string result = "";
			if (VB.DateNull == dt || dt == null) {
				result = "";
			} else {
				string d = Fmt.Date(dt);
				result = VB.right(VB.Day(d), 2) + " " + VB.MonthName(VB.Month(d), true) + " " + VB.right(VB.Year(d), 2);
			}
			return result;
		}

		public static string ShortDate(string d) {
			//	' date in nice short 1 Feb 04 format
			string result = "";
			if (VB.IsNull(d)) {
				result = "";
			} else if (d == "") {
				result = "";
			} else {
				result = VB.right(VB.Day(d), 2) + " " + VB.MonthName(VB.Month(d), true) + " " + VB.right(VB.Year(d), 2);
			}
			return result;
		}

		/// <summary>
		/// Given a long string, return a string truncated to 50 chars ellipsis (...)
		/// (Note that was changed - it used to add a link tag)
		/// </summary>
		/// <param name="src">A string</param>
		/// <returns></returns>
		public static string Ellipsis(string src) {
			return Ellipsis(src, 50);
		}
		/// <summary>
		/// Given a long string, return a string truncated to given number of chars plus ellipsis (...)
		/// (Note that this was changed - it used to add a link tag)
		/// </summary>
		/// <param name="src">A string</param>
		/// <returns></returns>
		public static string Ellipsis(string src, int len) {
			string result = src + "";
			if (result.Length > len) {
				// try and split on a word if there is an obvious point to break on
				int spacePos = result.LastIndexOf(' ', len - 3);
				if (spacePos > len * 0.5) {
					result = result.Substring(0, spacePos);
				}
				// make sure never longer than max len
				if (result.Length > len - 3) {
					result = result.Substring(0, len - 3);
				}
				// add ... because to indicate there was more
				result = result + "...";
			}
			return result;
		}

		/// <summary>
		/// given an attachment in the database, return an a-tag containing that address after adding http:// etc
		/// </summary>
		/// <param name="dbRecordValue">name of attachment file relative to attachments</param>
		/// <param name="defaultText">text to write in place of link if no data in dbRecordValue</param>
		/// <returns></returns>
#if DOTNET4
		public static string AttachmentDownloadLink(string dbRecordValue, string defaultText = "No document", string caption = "Download") {
			return (dbRecordValue.IsNotBlank()) ? WebAddress(Web.BaseUrlNoSSL + "attachments/" + dbRecordValue, "Click to download " + dbRecordValue, caption) : defaultText;
		}
#endif

		/// <summary>
		/// given a basic web address, return an a-tag containing that address after adding http:// etc
		/// </summary>
		/// <param name="url">what to link to, also display this</param>
		/// <returns></returns>
		public static string WebAddress(string url) {
			return WebAddress(url, url);
		}

		/// <summary>
		/// given a basic web address, return an a-tag containing that address after adding http:// etc
		/// </summary>
		/// <param name="url">what to link to</param>
		/// <param name="urldesc">what to display</param>
		/// <returns></returns>
		public static string WebAddress(string url, string urldesc) {
			return WebAddress(url, urldesc, urldesc);
		}

		/// <summary>
		/// given a basic web address, return an a-tag containing that address after adding http:// etc
		/// </summary>
		/// <param name="url">what to link to</param>
		/// <param name="urldesc">what to display</param>
		/// <returns></returns>
		public static string WebAddress(string url, string urldesc, string caption) {
			string result = "";
			if (VB.left(urldesc, 7) == "http://") urldesc = VB.mid(urldesc, 8); //remove http from start if exists
			if (VB.left(caption, 7) == "http://") urldesc = VB.mid(caption, 8); //remove http from start if exists
			if (VB.left(url, 7) != "http://") {
				url = "http://" + url;
			}
			result = "<a target=\"_blank\" href=\"" + url + "\" title=\"" + urldesc.JsEncode() + "\">" + caption + "</a>";

			return result;
		}
		///<summary>
		/// given an email address, return an href with the at symbol replaced with the html code for it to fool web spiders
		/// </summary>
		/// <param name="emailAddress"></param>
		/// <returns></returns>
		public static string EmailAddress(string emailAddress) {
			return EmailAddress(emailAddress, emailAddress);
		}
		public static string PhoneNumber(string phoneNumber) {
			string result = "<img src=\"" + Web.BaseUrl + "images/telephone.png\" alt=\"phone\"> " + phoneNumber + "";
			return result;
		}
		public static string FaxNumber(string faxNumber) {
			string result = "<img src=\"" + Web.BaseUrl + "images/telephone_edit.png\" alt=\"fax\"> " + faxNumber + "";
			return result;
		}

		public static string EmailAddress(string emailAddress, string caption) {
			return EmailAddress(emailAddress, caption, false);
		}

		public static string EmailAddress(string emailAddress, string caption, bool showIcon) {
			string result = "";

			if (emailAddress != "") {
				emailAddress = VB.replace(emailAddress, "@", "&#64;");	// encode to fool spiders
				result = "";
				if (showIcon) result += "<img src=\"" + Web.BaseUrl + "images/email.png\" alt=\"email\"> ";
				result += "<a href=\"mailto:" + emailAddress + "\">" + caption + "</a>";
			} else {
				result = "";
			}

			return result;
		}

		public const string MatchEmailPattern = "[a-z0-9!#$%&'*+/=?^_`{|}~-\u00E0-\u00FC]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-\u00E0-\u00FC]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+(?:[a-z]{2,64})";

		// another regex to try from microsoft if we have trouble with our one  http://msdn.microsoft.com/en-us/library/ff650303.aspx
		// ^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$

		/// <summary>
		/// return true if a valid email address is given.
		/// if email is blank returns false.
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		[ContractAnnotation("null => false")]
		public static bool IsValidEmailAddress(string email) {
			string sEmail = email;
			//const string matchEmailPattern = "^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+(?:[a-z]{2}" +  20140911jn old pattern?
			//																 "|aero|asia|biz|cat|com|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|pro|travel|tel)\\b$";


			// another regex to try from microsoft if we have trouble with our one  http://msdn.microsoft.com/en-us/library/ff650303.aspx
			// ^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$

			if (string.IsNullOrEmpty(sEmail)) {
				return false;
			}

			int nFirstAT = sEmail.IndexOf('@');
			int nLastAT = sEmail.LastIndexOf('@');

			if ((nFirstAT > 0) && (nLastAT == nFirstAT) && (nFirstAT < (sEmail.Length - 1))) {
				// address is ok regarding the single @ sign
				return (Regex.IsMatch(sEmail, "^" + MatchEmailPattern + "\\b$", RegexOptions.IgnoreCase));
			} else {
				return false;
			}
		}

		/// <summary>
		/// if a url doesnt start with http, add the http:// to it
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static string MakeValidUrl(string url) {
			return MakeValidUrl(url, false);
		}

		/// <summary>
		/// if a url doesnt start with http, add the http:// to it
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static string MakeValidUrl(string url, bool fixProtocolRelative) {
			if(url.StartsWith("//")) {
				if (fixProtocolRelative) {
					return "http:"+url;
				}
				return url;
			}

			if (VB.left(url, 4) != "http") {
				url = "http://" + url;
			}
			return url;
		}

		/// <summary>
		/// Formats a number commas separating the thousands and auto-detect number of decimal places. If null result is empty string.
		/// </summary>
		public static string Number(object amt) {
			return Number(amt, -1, true);
		}

		/// <summary>
		/// Formats a number with the specified number of digits and commas separating the thousands. If null result is empty string.
		/// </summary>
		public static string Number(object amt, int decimalPlaces) {
			return Number(amt, decimalPlaces, true);
		}

		/// <summary>
		/// Formats a number with the specified number of digits and will group thousands with commas or not. If null result is empty string.
		/// Decimalplaces of -1 means auto.
		/// Decimalplaces of -2 means auto but up to 2 decimal places at most.
		/// Decimalplaces of -n means auto but up to n decimal places at most.
		/// </summary>
		public static string Number(object amt, int decimalPlaces, bool groupDigits) {
			string result = "";

			if (amt is string) {
				// eg amt of "US$1,556.789 " will be converted to 1556.789 before formatting
				amt = CleanNumber((string)amt);
			}

			string decimalPlacesForPattern = decimalPlaces.ToString();
			if (decimalPlaces == -1) decimalPlacesForPattern = "10";   // 10 digits should avoid rounding bugs like 2.000000000001 or 1.999999999995;
			if (decimalPlaces <= -2) decimalPlacesForPattern = (decimalPlaces * -1).ToString();

			if (amt != null) {
				string pattern;
				if (groupDigits) {
					pattern = "{0:N" + decimalPlacesForPattern + "}";
				} else {
					pattern = "{0:F" + decimalPlacesForPattern + "}";
				}
				result = String.Format(pattern, amt);
			}

			if (decimalPlaces <= -1) {
				result = result.TrimEnd('0');  // trim all zeros off end before dot
				result = result.RemoveSuffix(".");  // remove final dot
			}


			return result;

			//return Microsoft.VisualBasic.Strings.FormatNumber(amt, decimalPlaces, TriState.True, TriState.True, groupDigits?TriState.True:TriState.False);
		}


		/// <summary>
		/// format a string as a currency by convert to decimal, then use src.tostring("C"). If blank, return blank
		/// </summary>
		/// <param name="amt"></param>
		/// <returns></returns>
		public static string Currency(string amt) {
			string result = "";
			if (amt + "" == "") {
				result = "";
			} else {
				//result = Microsoft.VisualBasic.Strings.FormatCurrency(amt);
				decimal c = Convert.ToDecimal(amt);
				result = c.ToString("C");

			}
			return result;
		}

		/// <summary>
		/// format a decimal as a currency by convert to decimal, then use src.tostring("C"). If blank, return blank
		/// </summary>
		/// <param name="amt"></param>
		/// <returns></returns>
		public static string Currency(decimal? amt) {
			return Currency(amt, 2);
		}
		public static string Currency(decimal? amt, int decimalPlaces) {
			return (amt.HasValue) ? "$" + Fmt.Number(amt, decimalPlaces) : "";
		}

		/// <summary>
		/// format a nullable decimal as a currency by convert to decimal, then use src.tostring("C"). If blank, return blank
		/// </summary>
		/// <param name="amt"></param>
		/// <returns></returns>
		public static string Currency(decimal amt) {
			return amt.ToString("C");
		}

		public static string Percent(decimal? percentageNumber, int decimalPlaces) {
			string result = Fmt.Number(percentageNumber, decimalPlaces);
			if (result.IsNotBlank()) result += "%";
			return result;
		}
		/// <summary>
		/// return green for 100%, red for 0%, or a colour in between
		/// </summary>
		/// <param name="percentage"></param>
		/// <returns></returns>
		//public static string PercentAsColour(decimal percentage) {
		//	var result = Colour.PercentageAsColour(percentage);
		//	return result;
		//}

		#region FileSize

		/// <summary>
		/// given a file size in bytes, return a formatted version
		/// MinimumDigits is if the whole portion of the result, is less than MinimumDigits, we will start using decimal places
		/// </summary>
		public static string FileSize(int fileSizeBytes, int minimumDigits) {
			return FileSize(fileSizeBytes.ToString(), minimumDigits);
		}

		/// <summary>
		/// given a file size in bytes, return a formatted version
		/// MinimumDigits is if the whole portion of the result, is less than MinimumDigits, we will start using decimal places
		/// </summary>
		/// <param name="fileSizeBytes"></param>
		/// <param name="minimumDigits"></param>
		/// <returns></returns>
		public static string FileSize(string fileSizeBytes, int minimumDigits) {
			double CalculatedSize = 0;
			string result = "";
			string SizeName = "";

			// in the checks the last divisor is always 1000 - we don't want 1000 KB - make it 0.9 MB
			// the actual calculator always uses 1024 though
			if (Convert.ToDouble(fileSizeBytes) / 1024 / 1024 / 1024 / 1000 >= 1) {
				// tera bytes
				CalculatedSize = Math.Round((double)(Convert.ToDouble(fileSizeBytes) / 1024 / 1024 / 1024 / 1024), minimumDigits);
				SizeName = "TB";
			} else if (Convert.ToDouble(fileSizeBytes) / 1024 / 1024 / 1000 >= 1) {
				// giga bytes
				CalculatedSize = Math.Round((double)(Convert.ToDouble(fileSizeBytes) / 1024 / 1024 / 1024), minimumDigits);
				SizeName = "GB";
			} else if (Convert.ToDouble(fileSizeBytes) / 1024 / 1000 >= 1) {
				// mega bytes
				CalculatedSize = Math.Round((double)(Convert.ToDouble(fileSizeBytes) / 1024 / 1024), minimumDigits);
				SizeName = "MB";
			} else if (Convert.ToDouble(fileSizeBytes) / 1000 >= 1) {
				// kilo bytes
				CalculatedSize = Math.Round((double)(Convert.ToDouble(fileSizeBytes) / 1024), minimumDigits);
				SizeName = "KB";
			} else {
				// bytes
				CalculatedSize = Math.Round((double)Convert.ToDouble(fileSizeBytes), minimumDigits);
				SizeName = "B";
			}
			result = CalculatedSize.ToString();

			// make sure there is a decimal point
			if (result.IndexOf(".") > 0) {
				// remove numbers after decimal point - but how many?
				string WholePortion;
				WholePortion = result.Substring(0, result.IndexOf("."));
				//future change?: round these numbers rather than just chopping them off?
				if (WholePortion.Length < minimumDigits) {
					// leave a certain number of decimal places
					result = result.Substring(0, minimumDigits + 1);
				} else {
					// we have enough characters - show a whole number
					// chop off the .## on the end
					result = WholePortion;
				}
			}

			result = string.Format("{0} {1}", result, SizeName);

			return result;
		}
		#endregion

		/// <summary>
		/// Given a boolean, return Yes or No as text. Nullable booleans are allowed, and null returns n/a (not answered), or No if SavvyActiveRecord_AllowNullableBooleans is false.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string YesNo(bool? value) {
			string result = "";
			if (value == null) {
				result = "n/a"; //not answered
				if (!Util.GetSettingBool("SavvyActiveRecord_AllowNullableBooleans", false)) {
					result = "No";
				}
			} else if (value.Value) {
				result = "Yes";
			} else {
				result = "No";
			}
			return result;
		}
		/// <summary>
		/// Given a string containing some type of bool, return yes or no as text
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string YesNo(bool value) {
			string result = "";
			if (value) {
				result = "Yes";
			} else {
				result = "No";
			}
			return result;
		}

		public static string YesNo(int? value) {
			string result = "";
			if (value == null) {
				result = "n/a";
			} else if (value == 1) {
				result = "Yes";
			} else {
				result = "No";
			}
			return result;
		}
		/// <summary>
		/// Given a string consisting of "true", "yes" or "1", return "Yes". Given null or "" return "n/a". Otherwise return "No".
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string YesNo(string value) {
			string result = "";

			if (value == null || value == "") {
				result = "n/a";
			} else {
				result = Fmt.YesNo(value.ToLower() == "yes" || value.ToLower() == "true" || value == "1");

			}

			return result;
		}

		/// <summary>
		/// Formats plain text for output. Converts plain text to HTML, by replacing \n with BR tags
		/// </summary>
		/// <param name="txt"></param>
		/// <returns></returns>
		public static string FmtText(string txt) {
			txt = ReplaceWebsiteBaseUrlPaths(txt);
			string result = (txt + "").TrimEnd(' ', '\t', '\n').Replace("\n", "<br>");
			return result;
		}
		public static string FmtTextWithLinks(string txt) {
			string result = txt;//"(txt+"").TrimEnd(' ','\t','\n').Replace("\n", "<br>");

			result = ReplaceWebsiteBaseUrlPaths(result);
			//link email addresses
			result = Regex.Replace(result, @"\b(\S+@\S+)\b", @"<a href=""mailto:$1"">$1</a>");
			// link "http://" and "www" addresses
			//result = Regex.Replace(result, @"(\s)(http:\/\/){0,1}(www\S+)\s", @"<a href=""http://$3"" target=""_blank"">$3</a>");
			result = Regex.Replace(result, @"\b(?:http://|www\.)\S+\b", @"<a href=""$0"" target=""_blank"">$0</a>");
			return result;
		}

		/// <summary>
		/// Formats plain text for output. Converts plain text to HTML, by replacing \n with BR tags. Also calls ReplaceWebsiteBaseUrlPaths to remove references to the current, dev, or staging servers
		/// IMPORTANT: this method HtmlEncodes the output. Be careful not to HtmlEncode it twice by calling HtmlEncode yourself as well.
		/// </summary>
		/// <param name="txt"></param>
		/// <returns></returns>
		public static string Text(string txt) {
			return Text(txt, false);
		}

		/// <summary>
		/// Formats plain text for output. Converts plain text to HTML, by replacing \n with BR tags. Also calls ReplaceWebsiteBaseUrlPaths to remove references to the current, dev, or staging servers
		/// IMPORTANT: this method HtmlEncodes the output. Be careful not to HtmlEncode it twice by calling HtmlEncode yourself as well.
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="alsoFmtLinks">if true, fmt the links and mailto in the plain text by adding html tags</param>
		/// <returns></returns>
		public static string Text(string txt, bool alsoFmtLinks) {

			if (!alsoFmtLinks) {
				txt = ReplaceWebsiteBaseUrlPaths(txt); //20120228 JN removed if  alsoFmtLinks is true, as this is called in 	FmtTextWithLinks
				txt = HttpContext.Current.Server.HtmlEncode(txt);
			} else {
				txt = HttpContext.Current.Server.HtmlEncode(txt);
				txt = Fmt.FmtTextWithLinks(txt); //20120228 JN added this, as it must go before the htmlencode to preserve the word breaks needed to format the mailto and hrefs using regex. 
			}

			string result = (txt + "").TrimEnd(' ', '\t', '\n');
			result = result.Replace("\r\n", "<br>");  //crlf > break
			result = result.Replace("\n", "<br>"); //cr > break
			result = result.Replace("\r", ""); //remove orphaned cr
			return result;
		}

		#region FmtSql
		/// <summary>
		/// Escape a numeric value properly for including in a SQL string. Makes sure this is a number (anti hack).
		/// </summary>
		/// <param name="value">String that should be a number</param>
		/// <returns></returns>
		public static string SqlNumber(string value) {
			double result = 0;
			if (double.TryParse(value, out result)) {
				return result.ToString();
			} else {
				// failed 
				throw new Exception("Failed to convert value [" + value + "] to a number");
			}
		}
		public static string SqlNumber(int value) {
			return Fmt.SqlNumber(value + "");
		}
		public static string SqlNumber(double value) {
			return Fmt.SqlNumber(value + "");
		}
		public static string SqlNumber(decimal value) {
			return Fmt.SqlNumber(value + "");
		}
		public static string SqlNumber(short value) {
			return Fmt.SqlNumber(value + "");
		}
		public static string SqlNumber(long value) {
			return Fmt.SqlNumber(value + "");
		}
		public static string SqlNumber(float value) {
			return Fmt.SqlNumber(value + "");
		}

		[Obsolete("Deprecated - use Fmt.SqlNumber instead - this method returns an Int but it should return a String")]
		public static int SQLNumber(string source) {
			int result = 0;
			try {
				if (source + "" != "") result = Convert.ToInt32(source);
			} catch (Exception ex) {
				// failed 
				Console.WriteLine("Exception: [{0}]", ex.Message);
				throw new Exception("Failed to convert value to a number");
			}

			return result;
		}

		/// <summary>
		/// Encode a boolean value properly for including in a SQL string. Prevents SQL injection hacks.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string SqlBoolean(string value) {
			bool result = false;
			if (value == "") value = "false";
			if (value == "1") value = "true";
			if (bool.TryParse(value, out result)) {
				return SqlBoolean(result);
			} else {
				// failed 
				throw new Exception("Failed to convert value [" + value + "] to a boolean");
			}
		}
		/// <summary>
		/// Encode a boolean value properly for including in a SQL string. Prevents SQL injection hacks.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string SqlBoolean(bool value) {
			return value ? "1" : "0";
		}
		#endregion


		/// <summary>
		/// Chop a length of text to a given length, but dont chop the word. 
		/// Dont break html tags (within reason). Can be used on HTML or plain text. Best to call striptags first to get it to plain text.
		/// 
		/// also see Ellipsis
		/// </summary>
		/// <param name="source">text to chop the left side of</param>
		/// <param name="intendedLength">length to get back, perhaps less if there is a tag or word near the end</param>
		/// <returns></returns>
		public static string TruncHTML(string source, int intendedLength) {
			string txt = source;
			string result_TruncHTML = txt + "";
			if (result_TruncHTML.Length <= intendedLength) return result_TruncHTML;

			//result_TruncHTML = left("" + source, intendedLength);
			//return result_TruncHTML;	// TODO	 TruncHTML remove this

			int scan;
			bool lookForSpace;
			int spacePosn, closeTagPosn;
			spacePosn = 0;
			closeTagPosn = 0;
			lookForSpace = false;
			for (scan = intendedLength - 1; scan >= 1; scan += -1) {
				if (result_TruncHTML.Length > 0 && VB.mid(result_TruncHTML, scan, 1) == " ") {
					spacePosn = scan;
					break;

				}
				if (result_TruncHTML.Length > 0 && VB.mid(result_TruncHTML, scan, 1) == "<" || VB.mid(result_TruncHTML, scan, 1) == ">") {
					if (VB.mid(result_TruncHTML, scan, 1) == ">") {
						closeTagPosn = scan;
						lookForSpace = true;

					} else {
						result_TruncHTML = VB.left("" + source, scan - 1);

					}
					break;
				}
			}
			if (lookForSpace) {
				for (scan = intendedLength; scan >= 1; scan += -1) {
					if (VB.mid(result_TruncHTML, scan, 1) == " ") {
						spacePosn = scan;
						break;
					}
				}
			}
			if (closeTagPosn > spacePosn) {
				result_TruncHTML = VB.left("" + source, closeTagPosn);
			} else {
				if (spacePosn != 0) {
					result_TruncHTML = VB.left("" + source, spacePosn);
				}

			}


			//int resultLength = len(result_TruncHTML);

			return result_TruncHTML;
		}

		/// <summary>
		/// Given a series of words, Make First Letter of Each Capitalised (except short words like a and of - todo)
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string TitleCase(string str) {
			return TitleCase(str, ' ');
		}
		public static string TitleCase(string str, char splitItem) {
			// todo: add exceptions for a, and, of etc -- see classic ASP version
			string[] words;
			str = str + "";

			string newstr = "";
			words = str.Split(splitItem);
			foreach (string word in words) {
				var newWord = word.ToLower().UpperCaseFirstLetter();
				if (newWord.StartsWith("Mc") && word.Length > 2) {
					string thirdLetter = word[2] + "";
					newWord = newWord.Substring(0, 2) + "" + thirdLetter.ToUpper() + "" + newWord.Substring(3);
				}
				if (splitItem == ' ' && newWord.Contains('-')) {
					newWord = TitleCase(newWord.Trim(), '-');
				}
				newstr += newWord + splitItem;
			}
			str = newstr;
			//}
			if (str.Substring(str.Length - 1) == splitItem + "") str = str.Substring(0, str.Length - 1); //shorten str if ends with splitter
			return str.Trim();
		}

		/// <summary>
		/// Converts string to PascalCase (eg this is a test -> ThisIsATest)
		/// </summary>
		/// <param name="str">string with words separated by spaces or underscores</param>
		/// <returns></returns>
		public static string PascalCase(string str) {
			string result = str + "";
			result = result.Replace("_", " ");   // in case it is underscoreized
			result = Fmt.TitleCase(result);
			result = result.Replace(" ", "");
			return result;
		}

		/// <summary>
		/// Converts string to camelCase (eg this is a test -> thisIsATest)
		/// </summary>
		/// <param name="str">string with words separated by spaces or underscores</param>
		/// <returns></returns>
		public static string CamelCase(string str) {
			string result = Fmt.PascalCase(str);
			// lower case first letter
			result = VB.left(result, 1).ToLower() + VB.mid(result, 2);
			return result;
		}

		/// <summary>
		/// Converts string to first letter capital and rest lower case (eg this is a Test -> This is a test)
		/// </summary>
		/// <param name="str">string with words separated by spaces or underscores</param>
		/// <returns></returns>	
		public static string SentenceCase(string str) {
			string result = str + "";
			result = result.Replace("_", "");   // in case it is underscoreized
			result = result.ToLower();
			// upper case first letter
			result = VB.left(result, 1).ToUpper() + VB.mid(result, 2);
			return result;
		}

		/// <summary>
		/// Returns the supplied number and word, making the word plural if the number is not one.
		/// eg "You have "+Fmt.Plural(count, "box")
		/// <param name="word">A string ending in an English word</param>
		/// <param name="number">The number of things eg 2 boxes</param>
		/// </summary>
		/// <returns></returns>	
		public static string Plural(int number, string str) {
			if (number == 1) {
				return number + " " + str;
			} else {
				return number + " " + Plural(str);
			}
		}

		/// <summary>
		/// Returns the supplied number and word, making the word plural if the number is not one.
		/// eg "You have "+Fmt.Plural(count, "box")
		/// <param name="word">A string ending in an English word</param>
		/// <param name="number">The number of things eg 2 boxes</param>
		/// </summary>
		/// <returns></returns>	
		public static string Plural(decimal number, string str) {
			if (number == 1) {
				return Fmt.Number(number, -1) + " " + str;
			} else {
				return Fmt.Number(number, -1) + " " + Plural(str);
			}
		}

		/// <summary>
		/// Returns the supplied number and word, making the word plural if the number is not one.
		/// eg "You have "+Fmt.Plural(count, "box")
		/// <param name="word">A string ending in an English word</param>
		/// <param name="number">The number of things eg 2 boxes</param>
		/// </summary>
		/// <returns></returns>	
		public static string Plural(double number, string str) {
			if (number == 1.0) {
				return Fmt.Number(number, -1) + " " + str;
			} else {
				return Fmt.Number(number, -1) + " " + Plural(str);
			}
		}

		/// <summary>
		/// Returns plural form of a word (eg "box".Plural() -> "boxes")
		/// </summary>
		/// <param name="word">A string ending in an English word - eg "You have "+count+" "+Fmt.Plural("box")</param>
		/// <returns></returns>
		public static string Plural(string word) {
			if (word == null) throw new NullReferenceException("Fmt.Plural() called on null string.");
			return Inflector.Pluralize(word);
		}

		#region FmtSql
		/// <summary>
		/// Formats a string ready for a SQL statement - puts appropriate quotes around value. Note that N converts the string to a ntext / nvarchar string
		/// </summary>
		/// <param name="dataValue"></param>
		/// <returns>string</returns>
		public static string SqlText(string dataValue) {
			return "N'" + SqlString(dataValue) + "'";
		}

		[Obsolete("Use Fmt.SqlText")]
		public static string SQLText(string dataValue) {
			return SqlText(dataValue);
		}

		/// <summary>
		/// Replace single quotes in a given string with '' quotes so it can be put in a sql string that has reserved single quotes in it
		/// Does not put quotes around the string. This is useful for Access databases or partial strings.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string SqlString(string str) {
			if (false) {				//add this mike?  // not sure, i dont think its a problem and maybe someone validly includes this like 'i do declare'
				if (str.IsNotBlank()
					&& (str.ToLower().Contains("exec")
					|| str.ToLower().Contains("declare")
					//||str.ToLower().Contains("cast(")
					)) {
					throw new UserErrorException("SQL contains EXEC or DECLARE command [" + str + "]");
				}
			}
			return (str == null) ? "" : str.Replace("'", "''");
		}
		#endregion

		public static string ReplaceSQLChars(string fieldval) {
			fieldval = fieldval.Replace("'", "'+char(39)+'");				   //single quote
			fieldval = fieldval.Replace("\r\n", "'+char(10)+char(13)+'");  //cr  with lf  :)
			fieldval = fieldval.Replace("\n", "'+char(10)+char(13)+'");  //lf on it's own :(
			fieldval = fieldval.Replace("\t", "'+char(9)+'");					   //tabs
			fieldval = fieldval.Replace(";", "'+char(59)+'");				   //semicol;
			fieldval = fieldval.Replace("?", "'+char(63)+'");				   //questionmark
			fieldval = fieldval.Replace(((char)147).ToString(), "'+char(147)+'");   //double qute left
			fieldval = fieldval.Replace(((char)148).ToString(), "'+char(148)+'");   //double qute left
			//fieldval = fieldval.Replace(fieldval,chr(39),"'+char(39)+'");   //pling!
			fieldval = fieldval.Replace(((char)149).ToString(), "'+char(149)+'");   //long dash -
			fieldval = fieldval.Replace(((char)150).ToString(), "'+char(150)+'");   //long dash -
			fieldval = fieldval.Replace(((char)133).ToString(), "'+char(133)+'");
			return fieldval;
		}

		#region FmtSql
		[Obsolete("Use Fmt.SqlString")]
		public static string SQLString(string src) {
			return SqlString(src);
		}
		#endregion

		/// <summary>
		/// Remove tags from HTML text. This is the fast version. Essentially converts HTML to plain text, typically for display as an intro or for searching.
		/// (Does not add line breaks and spacing. Does not change HTML entities to plain text equivalents. If you need these, use the much slower Fmt.StripTagsComplete)
		/// </summary>
		/// <param name="htmlText"></param>
		/// <returns></returns>
		public static string StripTags(string htmlText) {
			//return Regex.Replace(htmlText, "<\\S.*?>", string.Empty); -- below regex is a bit faster, same accuracy
			string result = htmlText + "";
			if (result.IsNotBlank()) {
				result = Regex.Replace(result, @"<[^>]*>", string.Empty);
				result = result.Replace("&nbsp;", " ");
				result = result.Replace("\r\n", " ");
				result = result.Replace("  ", " ");
				result = result.Replace("  ", " ");
				result = result.Replace("  ", " ");
				result = result.Trim();
			}
			return result;
		}

		/// <summary>
		/// Remove tags from HTML text and truncate to a certain number of characters. This is the fast version. Essentially converts HTML to plain text, typically for display as an intro or for searching.
		/// </summary>
		public static string StripTags(string htmlText, int maxChars) {
			return TruncHTML(StripTags(htmlText), maxChars);
		}

		/// <summary>
		/// Converts HTML to plain text (SLOW - for a faster job use Fmt.StripTags).
		/// Removes all the html tags from a string using a fancy number of regex that should also clean up the code. Adds line breaks and changes HTML entities to plain text equivalents. 
		/// For an even more thorough version, use Beweb.HtmlTagStripper.HtmlStripTags.
		/// </summary>
		/// <param name="str">html string</param>
		/// <returns>plain text string</returns>
		public static string StripTagsComplete(string str) {
			// from http://www.codeproject.com/useritems/HTML_to_Plain_Text.asp
			string result;

			try {
				// Remove HTML Development formatting
				// Replace line breaks with space
				// because browsers inserts space
				result = str.Replace("\r", " ");
				// Replace line breaks with space
				// because browsers inserts space
				result = result.Replace("\n", " ");
				// Remove step-formatting
				result = result.Replace("\t", string.Empty);

				// Remove the header (prepare first by clearing attributes)
				result = Regex.Replace(result,
								 @"<( )*head([^>])*>", "<head>",
								 RegexOptions.IgnoreCase);
				result = Regex.Replace(result,
								 @"(<( )*(/)( )*head( )*>)", "</head>",
								 RegexOptions.IgnoreCase);
				result = Regex.Replace(result,
								 "(<head>).*(</head>)", string.Empty,
								 RegexOptions.IgnoreCase);

				// remove all scripts (prepare first by clearing attributes)
				result = Regex.Replace(result,
								 @"<( )*script([^>])*>", "<script>",
								 RegexOptions.IgnoreCase);
				result = Regex.Replace(result,
								 @"(<( )*(/)( )*script( )*>)", "</script>",
								 RegexOptions.IgnoreCase);
				//result = Regex.Replace(result, 
				//				 @"(<script>)([^(<script>\.</script>)])*(</script>)",
				//				 string.Empty, 
				//				 RegexOptions.IgnoreCase);
				result = Regex.Replace(result,
								 @"(<script>).*(</script>)", string.Empty,
								 RegexOptions.IgnoreCase);

				// remove all styles (prepare first by clearing attributes)
				result = Regex.Replace(result,
								 @"<( )*style([^>])*>", "<style>",
								 RegexOptions.IgnoreCase);
				result = Regex.Replace(result,
								 @"(<( )*(/)( )*style( )*>)", "</style>",
								 RegexOptions.IgnoreCase);
				result = Regex.Replace(result,
								 "(<style>).*(</style>)", string.Empty,
								 RegexOptions.IgnoreCase);

				// insert tabs in spaces of <td> tags
				result = Regex.Replace(result,
								 @"<( )*td([^>])*>", "\t",
								 RegexOptions.IgnoreCase);

				// insert line breaks in places of <BR> and <LI> tags
				result = Regex.Replace(result,
								 @"<( )*br( )*>", "\r",
								 RegexOptions.IgnoreCase);
				result = Regex.Replace(result,
								 @"<( )*li( )*>", "\r",
								 RegexOptions.IgnoreCase);

				// insert line paragraphs (double line breaks) in place
				// if <P>, <DIV> and <TR> tags
				result = Regex.Replace(result,
								 @"<( )*div([^>])*>", "\r\r",
								 RegexOptions.IgnoreCase);
				result = Regex.Replace(result,
								 @"<( )*tr([^>])*>", "\r\r",
								 RegexOptions.IgnoreCase);
				result = Regex.Replace(result,
								 @"<( )*p([^>])*>", "\r\r",
								 RegexOptions.IgnoreCase);

				//replace H1, h2, etc w space
				result = Regex.Replace(result,
								 @"<(/| )*h.([^>])*>", " ",
								 RegexOptions.IgnoreCase);


				// Remove remaining tags like <a>, links, images,
				// comments etc - anything thats enclosed inside < >
				result = Regex.Replace(result,
								 @"<[^>]*>", string.Empty,
								 RegexOptions.IgnoreCase);

				// replace special characters:
				result = Regex.Replace(result,
								 @"&nbsp;", " ",
								 RegexOptions.IgnoreCase);

				result = Regex.Replace(result,
								 @"&bull;", " * ",
								 RegexOptions.IgnoreCase);
				result = Regex.Replace(result,
								 @"&lsaquo;", "<",
								 RegexOptions.IgnoreCase);
				result = Regex.Replace(result,
								 @"&rsaquo;", ">",
								 RegexOptions.IgnoreCase);
				result = Regex.Replace(result,
								 @"&trade;", "(tm)",
								 RegexOptions.IgnoreCase);
				result = Regex.Replace(result,
								 @"&frasl;", "/",
								 RegexOptions.IgnoreCase);
				result = Regex.Replace(result,
								 @"<", "<",
								 RegexOptions.IgnoreCase);
				result = Regex.Replace(result,
								 @">", ">",
								 RegexOptions.IgnoreCase);
				result = Regex.Replace(result,
								 @"&copy;", "(c)",
								 RegexOptions.IgnoreCase);
				result = Regex.Replace(result,
								 @"&reg;", "(r)",
								 RegexOptions.IgnoreCase);
				result = Regex.Replace(result,  // 20110329 MN added
								 @"&amp;", "&",
								 RegexOptions.IgnoreCase);
				// Remove all others. More can be added, see
				// http://hotwired.lycos.com/webmonkey/reference/special_characters/
				result = Regex.Replace(result,
								 @"&(.{2,6});", string.Empty,
								 RegexOptions.IgnoreCase);

				// for testng
				//Regex.Replace(result, 
				//			 this.txtRegex.Text,string.Empty, 
				//			 RegexOptions.IgnoreCase);

				// make line breaking consistent
				result = result.Replace("\n", "\r");

				/*				// Remove extra line breaks and tabs:
								// replace over 2 breaks with 2 and over 4 tabs with 4. 
								// Prepare first to remove any whitespaces inbetween
								// the escaped characters and remove redundant tabs inbetween linebreaks
								result = Regex.Replace(result,
												 "(\r)( )+(\r)", "\r\r",
												 RegexOptions.IgnoreCase);
								result = Regex.Replace(result,
												 "(\t)( )+(\t)", "\t\t",
												 RegexOptions.IgnoreCase);
								result = Regex.Replace(result,
												 "(\t)( )+(\r)", "\t\r",
												 RegexOptions.IgnoreCase);
								result = Regex.Replace(result,
												 "(\r)( )+(\t)", "\r\t",
												 RegexOptions.IgnoreCase);
								// Remove redundant tabs
								result = Regex.Replace(result,
												 "(\r)(\t)+(\r)", "\r\r",
												 RegexOptions.IgnoreCase);
								// Remove multible tabs followind a linebreak with just one tab
								result = Regex.Replace(result,
												 "(\r)(\t)+", "\r\t",
												 RegexOptions.IgnoreCase);
								// Initial replacement target string for linebreaks
								string breaks = "\r\r\r";
								// Initial replacement target string for tabs
								string tabs = "\t\t\t\t\t";
								for (int index = 0; index < result.Length; index++)
								{
									result = result.Replace(breaks, "\r\r");
									result = result.Replace(tabs, "\t\t\t\t");
									breaks = breaks + "\r";
									tabs = tabs + "\t";
								}
				*/

				// Remove repeating speces becuase browsers ignore them
				result = Regex.Replace(result, @"( )+", " ");

				// Thats it.
				return result;

			} catch {
				throw new Exception("regex error [" + str.Substring(0, 50) + "...]");
			}
		}

		/// <summary>
		/// Insert ~WebRoot markers in the string, ready to save into database.
		/// </summary>
		/// <param name="txt"></param>
		/// <returns></returns>
		public static string InsertWebsiteBaseUrlPathMarkers(string stringValue) {
			if (ActiveFieldBase.AutoFixBaseUrl && stringValue != null)
				if (stringValue.Contains(Web.BaseUrl)) {
					if (stringValue.StartsWith(Web.BaseUrl)) {
						// it is at the start, so probably a URL field, just replace
						stringValue = stringValue.Replace(Web.BaseUrl, "~WebRoot/");
					} else {
						// not at the start, so possibly an HTML content field, only replace if it is in quotes eg href="http://localhost/site/"
						stringValue = stringValue.Replace("'" + Web.BaseUrl, "'~WebRoot/").Replace("\"" + Web.BaseUrl, "\"~WebRoot/");
					}
				}
			return stringValue;
		}

		/// <summary>
		/// Replace out any links to the dev or test, with absolute current base URL. This is for links within html content blocks and URL entry fields in the CMS.
		/// You should call this when outputting any URLs that are stored in the CMS database. 
		/// It is called automatically by Fmt.HtmlText() to fix up any URLs in the rich text editor.
		/// </summary>
		/// <param name="txt"></param>
		/// <returns></returns>
		public static string ReplaceWebsiteBaseUrlPaths(string txt) {
			if(txt.IsBlank())return txt;          //20150218 jn  updated
			txt = "" + txt;// make it not null
			if (txt.StartsWith("~/")) {
				return Web.ResolveUrlFull(txt);
			}
			string searchText = "~WebRoot/";              //20150218 jn added contains if ~WebRoot not there
			if (ActiveFieldBase.AutoFixBaseUrl&&txt.Contains(searchText)) {
				// don't need to do anything here because ActiveField is replacing it automatically
				txt = txt.Replace(searchText, Web.BaseUrl);
				return txt;
			}

			//txt = VB.replace(txt, GetSetting("WebsiteBaseUrlLVE") + "", "");
			string listCSV = ConfigurationManager.AppSettings["ServerStages"];//"STG,LVE";
			if (listCSV == null) throw new Exception("ServerStages missing from web.config:  e.g.: DEV,STG,LVE");
			string[] list;
			list = listCSV.Split(new Char[] { ',' }, 10);
			for (int sc = 0; sc < list.Length; sc++) {
				string url = ConfigurationManager.AppSettings["WebsiteBaseUrl" + list[sc]];
				if (!String.IsNullOrEmpty(url)) {
					if (url.IndexOf("|") > -1) {
						string[] urls;
						urls = url.Split(new Char[] { '|' }, 100);
						for (int sc2 = 0; sc2 < urls.Length; sc2++) {
							txt = txt.Replace(urls[sc2], Web.BaseUrl);

						}
					} else {
						txt = txt.Replace(url, Web.BaseUrl);
					}
				}
			}
			return txt;
		}

		/// <summary>
		/// Trims off any whitespace including html whitespace (eg <br>, <p>, &nbsp;).
		/// Note this may need some more cases added.
		/// </summary>
		/// <param name="txt"></param>
		/// <returns></returns>
		public static string TrimHtmlText(string txt) {
			string result = txt;
			// remove trailing blanks lines
			txt = txt + "";
			txt = VB.replace(txt, "<B></B>", "");
			for (; ; ) {
				txt = txt.Trim();
				if (VB.right(txt, 2) == VB.crlf) {
					txt = VB.left(txt, VB.len(txt) - 2);
				} else if (VB.right(txt, 1) == VB.cr || VB.right(txt, 1) == VB.lf || VB.right(txt, 1) == VB.tab) {
					txt = VB.left(txt, VB.len(txt) - 1);
				} else if (VB.right(txt, 4) == "<br>") {
					txt = VB.left(txt, VB.len(txt) - 4);
				} else if (VB.right(txt, 5) == "<br/>") {
					txt = VB.left(txt, VB.len(txt) - 5);
				} else if (VB.right(txt, 6) == "<br />") {
					txt = VB.left(txt, VB.len(txt) - 6);
				} else if (VB.right(txt, 7) == "<p></p>") {
					txt = VB.left(txt, VB.len(txt) - 7);
				} else if (VB.ucase(VB.right(txt, 11)) == "<p><br></p>") {
					txt = VB.left(txt, VB.len(txt) - 11);
				} else if (VB.ucase(VB.right(txt, 17)) == "<p><br>&nbsp;</p>") {
					txt = VB.left(txt, VB.len(txt) - 17);
				} else if (VB.right(txt, 8) == "<p> </p>") {
					txt = VB.left(txt, VB.len(txt) - 8);
				} else if (VB.right(txt, 13) == "<p>&nbsp;</p>") {
					txt = VB.left(txt, VB.len(txt) - 13);
				} else if (VB.ucase(VB.right(txt, 12)) == "<br><br></p>") {
					txt = VB.left(txt, VB.len(txt) - 12) + "</p>";
				} else if (VB.ucase(VB.right(txt, 13)) == "<p>&nbsp;</p>") {
					txt = VB.left(txt, VB.len(txt) - 13);
				} else {
					break;

				};
			}//loop
			//txt = HtmlTextReplaceMarkers(txt)//todo this
			return txt;
		}

		/// <summary>
		/// Prepare to print out database html from the cms. Remove trailing 
		/// blanks lines, ensure in paragraph tag, etc, glossarise if available
		/// </summary>
		/// <param name="txt"></param>
		/// <returns></returns>
		public static string HTMLText(string txt) {
			return HTMLText(txt, Web.Root);
		}
		public static string HTMLText(string txt, string pathToRoot) {
			txt = TrimHtmlText(txt);
			if (VB.instr(txt, "<p>") < 1) {
				txt = "<p>" + txt + "</p>";
			}
			txt = VB.replace(txt, "@", "&#64;");// encode any email addresses to fool spiders

			// replace out any links to the current site with blank
			txt = ReplaceWebsiteBaseUrlPaths(txt);

			/* //no longer necessary since mce saves full urls
			string attachmentVPath = pathToRoot+"attachments/";
			txt = txt.Replace("href=\"../../../", "href=\""); //fix up mce path save problem

			txt = txt.Replace("../../../attachments/", attachmentVPath); //fix up mce path save problem
			txt = txt.Replace("../../attachments/", attachmentVPath); //fix up mce path save problem

			txt = VB.replace(txt, "VPATHMARKER", attachmentVPath);
			txt = VB.replace(txt, "../attachments/", attachmentVPath);
			*/
			/*
						if(BewebData.TableExists("Glossary"))
						{
							txt = BewebData.FmtGlossarize(txt); 
						}
			*/

			txt = "<div class=\"normal\">" + txt + "</div>";

			//txt = VB.replace(txt, "../_attachments/", attachmentVPath);
			//txt = FmtHtmlText(txt);

			return txt;
		}

		/// <summary>
		/// Encodes Text to UTF-8 for writing to the screen/xml
		/// </summary>
		/// <param name="txt"></param>
		/// <returns></returns>
		public static string UTF8Encode(string txt) {
			txt = TrimHtmlText(txt);

			txt = VB.replace(txt, "&", "&amp;");
			txt = VB.replace(txt, "'", "&apos;");
			txt = VB.replace(txt, "\"", "&quot;");
			txt = VB.replace(txt, ">", "&gt;");
			txt = VB.replace(txt, "<", "&lt;");
			return txt;
		}
		/// <summary>
		/// Converts a string value to an enumeration of a given enum type.
		/// </summary>
		/// <typeparam name="TEnum">The enum type</typeparam>
		/// <param name="value">The string value</param>
		/// <returns>The enum value</returns>
		public static TEnum StringToEnum<TEnum>(string value) {
			string[] validEnumNames = Enum.GetNames(typeof(TEnum));
			if (validEnumNames.Contains(value)) {
				return (TEnum)Enum.Parse(typeof(TEnum), value);
			}
			throw new Exception("Beweb.StringToEnum(): The value is not in the valid " + typeof(TEnum).Name + " list. The invalid value is: " + value + ". Valid values are: " + validEnumNames.Join(", "));
		}

		// MN: deprecated - this is dumb
		//public static bool Bool(object obj)
		//{
		//  if(String.IsNullOrEmpty(obj.ToString()))
		//  {
		//    return false;
		//  }
		//  return true;
		//}

		/// <summary>
		/// Returns how long since the first date assuming the second date is now (eg 5 minutes ago)
		/// </summary>
		public static string TimeDiffText(DateTime? source, DateTime compareDate) {
			return TimeDiffText(source, compareDate, " ago");
		}
		public static string TimeDiffText(DateTime? source, DateTime compareDate, string suffix) {
			string result = "";
			if (!source.HasValue) return result;
			TimeSpan ts = compareDate.Subtract(source.Value);
			if (ts.Days >= 30 && ts.Days < 60) {
				result = "1 month";
			} else if (ts.Days > 30) {
				result = ts.Days / 30 + " months";
			} else if (ts.Days >= 7 && ts.Days < 14) {
				result = ts.Days / 7 + " week";
			} else if (ts.Days >= 14) {
				result = ts.Days / 7 + " weeks";
			} else if (ts.Days == 1) {
				result = ts.Days + " day";
			} else if (ts.Days > 1) {
				result = ts.Days + " days";
			} else if (ts.Hours == 1) {
				result = ts.Hours + " hour";
			} else if (ts.Hours > 0) {
				result = ts.Hours + " hours";
			} else if (ts.Minutes == 1) {
				result = ts.Minutes + " minute";
			} else if (ts.Minutes > 0) {
				result = ts.Minutes + " minutes";
			} else if (ts.Seconds == 1) {
				result = ts.Seconds + " second";
			} else if (ts.Seconds > 0) {
				result = ts.Seconds + " seconds";
			} else if (ts.Seconds <= 1) {
				result = "a second";
			}
			return result + suffix;   //20140618 jn added back optional suffix
		}

		/// <summary>
		/// Returns how long ago (eg 5 minutes ago)
		/// </summary>
		public static string TimeDiffText(DateTime? source) {
			return TimeDiffText(source, System.DateTime.Now);
		}
		public static string TimeDiffTextFuture(DateTime? source, DateTime compareDate) {
			if (!source.HasValue) return "";
			return TimeDiffTextFutureBase(source.Value, compareDate, true);
		}
		public static string TimeDiffTextFutureBase(DateTime source, DateTime compareDate, bool includeHoursMinsAndSeconds) {
			string result = "";
			TimeSpan ts = compareDate.Subtract(source);

			if (ts.Days > 365 && ts.Days < (365 * 2)) {
				result += (result.IsBlank() ? "" : ", ");
				result += "1 year";
			} else if (ts.Days > 365) {
				result += (result.IsBlank() ? "" : ", ");
				result += ts.Days / 365 + " years";
			}

			var numMonths = ts.Days / 30;
			if (ts.Days > 30 && ts.Days < 60) {
				result += (result.IsBlank() ? "" : ", ");
				result += "1 month";
			} else if (ts.Days > 30) {
				result += (result.IsBlank() ? "" : ", ");
				result += numMonths + " months";
			}
			var numWeeks = ts.Days / 7;
			var weeksRemaining = numWeeks - (numMonths * 4);
			if (ts.Days > 7 && ts.Days < 14) {
				result += (result.IsBlank() ? "" : ", ");
				result += "1 week";
			} else if (ts.Days > 14 && weeksRemaining > 0) {
				result += (result.IsBlank() ? "" : ", ");

				result += weeksRemaining + " weeks";
			}
			if (ts.Days == 1) {
				result += (result.IsBlank() ? "" : ", ");
				result += ts.Days + " day";
			} else if (ts.Days > 1) {
				result += (result.IsBlank() ? "" : ", ");
				result += (ts.Days - (numWeeks * 7)) + " days";
			}
			if (includeHoursMinsAndSeconds) {
				if (ts.Hours > 0) {
					result += (result.IsBlank() ? "" : ", ");
					result += ts.Hours + " hours";
				}
				if (ts.Minutes == 1) {
					result += (result.IsBlank() ? "" : ", ");
					result += ts.Minutes + " minute";
				} else if (ts.Minutes > 1) {
					result += (result.IsBlank() ? "" : ", ");
					result += ts.Minutes + " minutes";
				}
				if (ts.Seconds == 1) {
					result += (result.IsBlank() ? "" : ", ");
					result += ts.Seconds + " second";
				} else if (ts.Seconds > 1) {
					result += (result.IsBlank() ? "" : ", ");
					result += ts.Seconds + " seconds";
				}
			}
			return result;
		}

		public static string PublishStatus(DateTime? publishDate, DateTime? expiryDate) {
			return PublishStatus(publishDate, expiryDate, true);
		}

		public static string PublishStatus(DateTime? publishDate, DateTime? expiryDate, bool assumeExpiryDateEndOfDay) {
			string result = "";
			if (publishDate == null) {
				result = "Unpublished";
			} else if (publishDate >= System.DateTime.Now) {
				result = "Scheduled";
			} else if (expiryDate == null) {
				result = "Live";
			} else if (assumeExpiryDateEndOfDay && expiryDate == System.DateTime.Today) {  // midnight this morning - therefore use whole day logic ie expire at end of day
				result = "Live";
			} else if (expiryDate < System.DateTime.Now) {
				result = "Expired";
			} else {
				result = "Live";
			}
			return result;
		}

#if ActiveRecord
		public static string PublishStatus(ActiveRecord record) {
			string result;
			if (record.Advanced.HasPublishDates) {
				result = PublishStatus(record.Advanced.PublishDate, record.Advanced.ExpiryDate, !record.Advanced.ExpiryDatesHaveTimes);
			} else {
				if (record.GetIsActive()) {
					result = "Inactive";
				} else {
					result = "Active";
					if (record.Advanced.IsActiveFields.Count == 1) {
						result = record.Advanced.IsActiveFields[0].FriendlyName.RemovePrefix("Is ");
					}
				}
			}
			return result;
		}

#endif

		public static string PublishStatusHtml(DateTime? publishDate, DateTime? expiryDate) {
			string result = PublishStatus(publishDate, expiryDate);
			result = "<span class=\"publish-status-" + result + "\">" + result + "</span>";
			return result;
		}

#if ActiveRecord
		public static string PublishStatusHtml(ActiveRecord record) {
			string result;
			if (record.Advanced.HasPublishDates) {
				result = PublishStatus(record.Advanced.PublishDate, record.Advanced.ExpiryDate);
				result = "<span class=\"publish-status-" + result + "\">" + result + "</span>";
			} else {
				if (record.GetIsActive()) {
					result = "<span class=\"publish-status-Unpublished\">Inactive</span>";
				} else {
					result = "Active";
					if (record.Advanced.IsActiveFields.Count == 1) {
						result = record.Advanced.IsActiveFields[0].FriendlyName.RemovePrefix("Is ");
					}
					result = "<span class=\"publish-status-Live\">" + result + "</span>";
				}
			}
			return result;
		}
#endif

		public static string YesNoHtml(bool? value) {
			string result = Fmt.YesNo(value);
			result = "<span class=\"yesno-" + result + "\">" + result + "</span>";
			return result;
		}

		/// <summary>
		/// Create a descriptive version of a complex looking filename. Remove - and _ and extension, and titlecase the result
		/// </summary>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static string CleanupFileNameAsDescriptiveName(string defaultValue) {
			string result = defaultValue;
			result = result.RightFrom("/");    // remove any subdirectories and just get file name
			result = result.Replace("-", " ");
			result = Fmt.TitleCase(result.Replace("_", " "));
			var posn = result.LastIndexOf(".");

			if (posn != -1) result = result.Substring(0, posn);
			return result;
		}

		/// <summary>
		/// Provides a helper with correct encoding for putting together mailto: links such as send to friend links
		/// </summary>
		/// <param name="toEmail"></param>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		/// <returns></returns>
		public static string MailToLink(string toEmail, string subject, string body) {
			string link = "mailto:" + toEmail;
			if (subject.IsNotBlank() || body.IsNotBlank()) {
				link += "?subject=" + subject.HtmlEncode() + "&body=" + body.HtmlEncode();
			}
			return link;
		}

		/// <summary>
		/// Surrounds with double quotes and escapes javascript special characters (eg quotes, line breaks, etc) for output as a javascript string. Produces a string in double quotes with backslash sequences in all the right places.
		/// </summary>
		/// <param name="s"></param>
		/// <returns>Encoded string surrounded by double quotes</returns>
		public static string JsEnquote(string s) {
			///  FUNCTION Enquote Public Domain 2002 JSON.org 
			///  @author JSON.org 
			///  @version 0.1 
			///  Ported to C# by Are Bjolseth, teleplan.no 
			if (s == null || s.Length == 0) {
				return "\"\"";
			}
			char c;
			int i;
			int len = s.Length;
			StringBuilder sb = new StringBuilder(len + 4);
			string t;

			sb.Append('"');
			for (i = 0; i < len; i += 1) {
				c = s[i];
				if ((c == '\\') || (c == '"') /*|| (c == '>')*/) {  // MN 20100908 - pretty sure this was commented out on purpose - Jeremy please comment better!
					sb.Append('\\');
					sb.Append(c);
				} else if (c == '\b')
					sb.Append("\\b");
				else if (c == '\t')
					sb.Append("\\t");
				else if (c == '\n')
					sb.Append("\\n");
				else if (c == '\f')
					sb.Append("\\f");
				else if (c == '\r')
					sb.Append("\\r");
				else {
					if (c < ' ') {
						//t = "000" + Integer.toHexString(c); 
						string tmp = new string(c, 1);
						try {
							// AF20140902 It will fail to parse weird chars and will throw exception. Put this try/catch to ignore these chars
							t = "000" + int.Parse(tmp, System.Globalization.NumberStyles.HexNumber);
							sb.Append("\\u" + t.Substring(t.Length - 4));
						} catch { }
					} else {
						sb.Append(c);
					}
				}
			}
			sb.Append('"');
			//sb.Replace("'", "\\'"); //20141205 jn put this back, as i need to replace single quote with encoded single quote in this case:
			sb.Replace("'", "\u0027"); //MN 20150214 uncommented this, not sure why it was commented out.... //20141208 jn from 	http://stackoverflow.com/a/7446361
															//onclick="handlePaid(this,'<%=ReportController.ViewModel.FABMode.OrderInPastInvoiceInPresent%>',
																	//'<%=lastDealerCode.JsEncode() %>','<%=lastDealerName.JsEncode()%>','mkToPayBefore_<%=sc %>'); return false;"
																	// where last dealer name has a single quote in it
															// -- MN 20140512 - andre reckons this is not valid JSON (although it may be valid javascript literal)
			return sb.ToString();
		}

		/// <summary>
		/// Escapes javascript special characters (eg quotes, line breaks, etc) for output as a javascript string. Does not include the quotes around the javascript string.
		/// </summary>
		/// <param name="str"></param>
		/// <returns>Encoded string</returns>
		public static string JsEncode(string str) {
			string newStr = JsEnquote(str);
			// remove quotes
			newStr = newStr.RemoveCharsFromStart(1);
			newStr = newStr.RemoveCharsFromEnd(1);
			return newStr;
		}

		public static string SkypeAddress(string skypeAddress) {
			return @"
				<a href=""" + skypeAddress + @"?call"" class=""skypeaddress"">" + skypeAddress + @"</a>
				<a href=""skype:" + skypeAddress + @"?call"" class=""skypeaddress""><img src=""http://mystatus.skype.com/smallicon/" + skypeAddress + @""" width=""16"" height=""16"" alt=""Skype status for " + skypeAddress + @""" /></a>";

		}

		public static string RemoveYearFromDate(string fmtDate) {
			return fmtDate.Substring(0, fmtDate.LastIndexOf("-"));
		}

		/// <summary>
		/// apply escape characters to plain text to enable it to be part of a regex pattern string
		/// </summary>
		/// <returns></returns>
		public static string RegExEscape(string txt) {
			txt = txt.Replace(@"[", @"\[");
			txt = txt.Replace(@"\", @"\\");
			txt = txt.Replace(@"^", @"\^");
			txt = txt.Replace(@"$", @"\$");
			txt = txt.Replace(@".", @"\.");
			txt = txt.Replace(@"|", @"\|");
			txt = txt.Replace(@"?", @"\?");
			txt = txt.Replace(@"*", @"\*");
			txt = txt.Replace(@"+", @"\+");
			txt = txt.Replace(@"(", @"\(");
			txt = txt.Replace(@")", @"\)");
			return txt;
		}

		public static string Repeat(string s, int depth) {
			return s.Repeat(depth);
		}

		public static string ReverseString(string str) {
			char[] array = str.ToCharArray();
			Array.Reverse(array);
			return new string(array);
		}

		public static string Url(string linkUrl) {
			linkUrl = MakeValidUrl(linkUrl);
			return "<a href=\"" + linkUrl + "\">" + linkUrl + "</a>";
		}

		public static string WrapWords(string truncHtml, string tagName, int limit) {
			var words = truncHtml.Split(" ");
			var wrapper = "<" + tagName + ">";
			var normalWords = "";
			var count = 1;

			foreach (var word in words) {
				if (count > limit) {
					normalWords += word + " ";
				} else {
					wrapper += word + " ";
				}
				count++;
			}

			wrapper += "</" + tagName + ">";


			return wrapper + normalWords;
		}

		//public static string BoldAsterisk(string title) {
		//  // AF20141008 The regex is expecting at least 2 '*'. Don't proceed if the string has less than 2 asterisks
		//  if (title.IsBlank() || title.Count(x => x == '*') < 2) {
		//    return title + "";
		//  }
			
		//  return Regex.Replace(title, @"\*+([^*]+)\*", match => "<b>" + match.Groups[1].Value + "</b>");
		//  //return Regex.Replace(title, "\\*+([^.*?$]+)+\\*", match => "<b>" + match.Groups[1].Value + "</b>");   // this dies in an infinite loop depending on input eg "*Return $99.99* Economy"
		//}

		public static string BoldAsterisk(string title) {
			// AF20141008 The regex is expecting at least 2 '*'. Don't proceed if the string has less than 2 asterisks
			if (title.IsBlank()) {
				return title + "";
			}
			var source = title.Split("\n");
			//var lines = new List<string>();
			StringBuilder newLine = new StringBuilder((int) (title.Length * 1.2));
			int lineNum = 0;
			int lineCount = source.Length;
			foreach (var line in source) {
				bool isInsideBoldTag = false;
				int lastAsteriskPos = line.LastIndexOf("*", System.StringComparison.Ordinal);
				if (lastAsteriskPos < 3) {       // ie *a* is the min
					newLine.Append(line);
				} else {
					int pos = 0;
					foreach (char c in line) {
						if (c == '*') {
							if (isInsideBoldTag) {
								newLine.Append("</b>");
								isInsideBoldTag = false;
							} else if (pos == lastAsteriskPos) {
								newLine.Append("*");
							} else {
								newLine.Append("<b>");
								isInsideBoldTag = true;
							}
						} else {
							// not an *
							newLine.Append(c);
						}
						//if (pos > lastAsteriskPos) {
						//  newLine.Append(line.Substring();
						//  break;
						//}
						pos++;
					}
				}
				if (lineNum < lineCount-1) {
					newLine.Append("\n");
				}
				lineNum++;
			}
			return newLine.ToString();
		}

		/// <summary>
		/// Wraps in span with class 'good' if value matches pipe separated values, otherwise 'bad'.
		/// </summary>
		/// <returns>Returns HTML span tag</returns>
		public static string GoodBad(string currentValue, string goodValues) {
			return GoodBad(currentValue, goodValues, null);
		}

		/// <summary>
		/// Wraps in span with class 'good' or 'bad' if value matches pipe separated values. Otherwise 'neutral'.
		/// </summary>
		/// <returns>Returns HTML span tag</returns>
		public static string GoodBad(string currentValue, string goodValues, string badValues) {
			string className = "neutral";
			if (goodValues.ContainsPipeSeparated(currentValue)) {
				className = "good";
			} else if (badValues == null || badValues.ContainsPipeSeparated(currentValue)) {
				className = "bad";
			}
			return "<span class=" + className + ">" + currentValue.HtmlEncode() + "</span>";
		}

		public static string DayDateShort(DateTime? dateTime) {
			if (dateTime == null) return null;
			return Fmt.DayAbbrev(dateTime.Value.Date) + " " + dateTime.Value.Day + " " + VB.MonthNameAbbrev(dateTime.Value.Month);
		}

		public static string CommaDelimit(params string[] items) {
			var str = new DelimitedString(", ");
			foreach (var item in items) {
				if (item.IsNotBlank()) {
					str += item;
				}
			}
			return str.ToString();
		}

		public static string CSVToHtmlTable(string csv) {
			string result = "";
			int i = 0;
			foreach (var rec in csv.Split('\n')) {
				var row = "";	
				foreach (var rec2 in rec.Split(',')) {
					row += "<td>"+rec2+"</td>";
					i++;
				}
				result += "<tr>"+row+"</tr>";
				i++;
			}
			return "<table>"+result + "</table>";
		}

	}


	#region "Inflector - for pluralizing"
	// source code from Inflector.Net
	public static class Inflector {
		#region Default Rules

		static Inflector() {
			AddPlural("$", "s");
			AddPlural("s$", "s");
			AddPlural("(ax|test)is$", "$1es");
			AddPlural("(octop|vir)us$", "$1i");
			AddPlural("(alias|status)$", "$1es");
			AddPlural("(bu)s$", "$1ses");
			AddPlural("(buffal|tomat)o$", "$1oes");
			AddPlural("([ti])um$", "$1a");
			AddPlural("sis$", "ses");
			AddPlural("(?:([^f])fe|([lr])f)$", "$1$2ves");
			AddPlural("(hive)$", "$1s");
			AddPlural("([^aeiouy]|qu)y$", "$1ies");
			AddPlural("(x|ch|ss|sh)$", "$1es");
			AddPlural("(matr|vert|ind)ix|ex$", "$1ices");
			AddPlural("([m|l])ouse$", "$1ice");
			AddPlural("^(ox)$", "$1en");
			AddPlural("(quiz)$", "$1zes");

			AddSingular("s$", "");
			AddSingular("(n)ews$", "$1ews");
			AddSingular("([ti])a$", "$1um");
			AddSingular("((a)naly|(b)a|(d)iagno|(p)arenthe|(p)rogno|(s)ynop|(t)he)ses$", "$1$2sis");
			AddSingular("(^analy)ses$", "$1sis");
			AddSingular("([^f])ves$", "$1fe");
			AddSingular("(hive)s$", "$1");
			AddSingular("(tive)s$", "$1");
			AddSingular("([lr])ves$", "$1f");
			AddSingular("([^aeiouy]|qu)ies$", "$1y");
			AddSingular("(s)eries$", "$1eries");
			AddSingular("(m)ovies$", "$1ovie");
			AddSingular("(x|ch|ss|sh)es$", "$1");
			AddSingular("([m|l])ice$", "$1ouse");
			AddSingular("(bus)es$", "$1");
			AddSingular("(o)es$", "$1");
			AddSingular("(shoe)s$", "$1");
			AddSingular("(cris|ax|test)es$", "$1is");
			AddSingular("(octop|vir)i$", "$1us");
			AddSingular("(alias|status)es$", "$1");
			AddSingular("^(ox)en", "$1");
			AddSingular("(vert|ind)ices$", "$1ex");
			AddSingular("(matr)ices$", "$1ix");
			AddSingular("(quiz)zes$", "$1");

			AddIrregular("person", "people");
			AddIrregular("man", "men");
			AddIrregular("child", "children");
			AddIrregular("sex", "sexes");
			AddIrregular("move", "moves");
			AddIrregular("fish", "fishes"); //different types of fish

			AddUncountable("equipment");
			AddUncountable("information");
			AddUncountable("rice");
			AddUncountable("money");
			AddUncountable("species");
			AddUncountable("series");

			AddUncountable("bass");
			AddUncountable("sheep");
			AddUncountable("deer");
			AddUncountable("cattle");
			AddUncountable("moose");
			AddUncountable("data");

			AddUncountable("staff");
			AddUncountable("accommodation");
			AddUncountable("advice");
			AddUncountable("aircraft");
			AddUncountable("baggage");
			AddUncountable("bread");
			AddUncountable("bravery");
			AddUncountable("chaos");
			AddUncountable("clarity");
			AddUncountable("courage");
			AddUncountable("cowardice");
			AddUncountable("equipment");
			AddUncountable("education");
			AddUncountable("evidence");
			AddUncountable("furniture");
			AddUncountable("garbage");
			AddUncountable("greed");
			AddUncountable("homework");
			AddUncountable("honesty");
			AddUncountable("information");
			AddUncountable("jewelry");
			AddUncountable("knowledge");
			AddUncountable("livestock");
			AddUncountable("luggage");
			AddUncountable("marketing");
			AddUncountable("money");
			AddUncountable("insurance");
			AddUncountable("mud");
			AddUncountable("news");
			AddUncountable("pasta");
			AddUncountable("patriotism");
			AddUncountable("progress");
			AddUncountable("racism");
			AddUncountable("research");
			AddUncountable("sexism");
			AddUncountable("travel");
			AddUncountable("weather");
			AddUncountable("work");
			AddUncountable("milk");
			AddUncountable("sugar");
			AddUncountable("water");
			AddUncountable("salt");
			AddUncountable("distance");
			AddUncountable("art");
			AddUncountable("blame");
			AddUncountable("freshness");
			AddUncountable("speed");
			AddUncountable("scissors");


		}

		#endregion

		private class Rule {
			private readonly Regex _regex;
			private readonly string _replacement;

			public Rule(string pattern, string replacement) {
				_regex = new Regex(pattern, RegexOptions.IgnoreCase);
				_replacement = replacement;
			}

			public string Apply(string word) {
				if (!_regex.IsMatch(word)) {
					return null;
				}

				return _regex.Replace(word, _replacement);
			}
		}

		private static void AddIrregular(string singular, string plural) {
			AddPlural("(" + singular[0] + ")" + singular.Substring(1) + "$", "$1" + plural.Substring(1));
			AddSingular("(" + plural[0] + ")" + plural.Substring(1) + "$", "$1" + singular.Substring(1));
		}

		private static void AddUncountable(string word) {
			_uncountables.Add(word.ToLower());
		}

		private static void AddPlural(string rule, string replacement) {
			_plurals.Add(new Rule(rule, replacement));
		}

		private static void AddSingular(string rule, string replacement) {
			_singulars.Add(new Rule(rule, replacement));
		}

		private static readonly List<Rule> _plurals = new List<Rule>();
		private static readonly List<Rule> _singulars = new List<Rule>();
		private static readonly List<string> _uncountables = new List<string>();

		public static string Pluralize(string word) {
			return ApplyRules(_plurals, word);
		}

		public static string Singularize(string word) {
			return ApplyRules(_singulars, word);
		}

		private static string ApplyRules(List<Rule> rules, string word) {
			string result = word;

			if (!_uncountables.Contains(word.ToLower())) {
				for (int i = rules.Count - 1; i >= 0; i--) {
					if ((result = rules[i].Apply(word)) != null) {
						break;
					}
				}
			}

			return result;
		}

		public static string Titleize(string word) {
			return Regex.Replace(Humanize(Underscore(word)), @"\b([a-z])",
								 delegate(Match match) {
									 return match.Captures[0].Value.ToUpper();
								 });
		}

		public static string Humanize(string lowercaseAndUnderscoredWord) {
			return Capitalize(Regex.Replace(lowercaseAndUnderscoredWord, @"_", " "));
		}

		public static string Pascalize(string lowercaseAndUnderscoredWord) {
			return Regex.Replace(lowercaseAndUnderscoredWord, "(?:^|_)(.)",
								 delegate(Match match) {
									 return match.Groups[1].Value.ToUpper();
								 });
		}

		public static string Camelize(string lowercaseAndUnderscoredWord) {
			return Uncapitalize(Pascalize(lowercaseAndUnderscoredWord));
		}

		public static string Underscore(string pascalCasedWord) {
			return Regex.Replace(
				Regex.Replace(
				Regex.Replace(pascalCasedWord, @"([A-Z]+)([A-Z][a-z])", "$1_$2"), @"([a-z\d])([A-Z])",
				"$1_$2"), @"[-\s]", "_").ToLower();
		}

		public static string Capitalize(string word) {
			return word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
		}

		public static string Uncapitalize(string word) {
			return word.Substring(0, 1).ToLower() + word.Substring(1);
		}

		public static string Ordinalize(string number) {
			int n = int.Parse(number);
			return Ordinalize(n);
		}

		public static string Ordinalize(int number) {
			int nMod100 = number % 100;

			if (nMod100 >= 11 && nMod100 <= 13) {
				return number + "th";
			}

			switch (number % 10) {
				case 1:
					return number + "st";
				case 2:
					return number + "nd";
				case 3:
					return number + "rd";
				default:
					return number + "th";
			}
		}

		public static string Dasherize(string underscoredWord) {
			return underscoredWord.Replace('_', '-');
		}
	}
	#endregion

}

#if TestExtensions
namespace BewebTest {


	/// <summary>
	///This is a test class for FmtTest and is intended
	///to contain all FmtTest Unit Tests
	///</summary>
	[TestClass()]
	public class FmtTestSpeed {
		[SlowTestMethod()]
		public void TestStripTagsSpeed() {
			string str = Util.HttpGet("http://nzherald.co.nz/");

			var now = DateTime.Now;
			for (var i = 0; i < 100; i++) {
				Fmt.StripTags(str);
			}
			Web.Write(" - StripTags:" + (DateTime.Now - now).TotalMilliseconds);


			now = DateTime.Now;
			for (var i = 0; i < 100; i++) {
				Fmt.StripTagsComplete(str);
			}
			Web.Write(" - StripTagsComplete: " + (DateTime.Now - now).TotalMilliseconds);

			now = DateTime.Now;
			for (var i = 0; i < 100; i++) {
				//			Beweb.HtmlTagStripper.HtmlStripTags(str, true, false);
			}
			Web.Write(" - ThirdPartyHtmlStripTags: " + (DateTime.Now - now).TotalMilliseconds);
		}

	[TestMethod]
		public void CrunchSpeed() {
			var now = DateTime.Now;
			for (var i = 0; i < 1000000; i++) {
				var x = Fmt.Crunch("24/7 Full Roadside Assistance for Breakdown and Accident Emergency");
			}
			Web.Write("CrunchSpeed:" + now.FmtMillisecondsElapsed());
		}


	}

	[TestClass()]
	public class FmtTest {


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
		/// A Test for FmtCleanMobile
		/// </summary>
		[TestMethod()]
		public void FmtCleanMobile() {
			string valueToTest;
			string expected;
			string actual;

			valueToTest = "(021) 1885 232";
			expected = "+64211885232";
			actual = Fmt.CleanMobile(valueToTest);
			Assert.AreEqual(expected, actual);
			//retest.
			actual = Fmt.CleanMobile(actual);
			Assert.AreEqual(expected, actual);

			valueToTest = "+6421 1885     232";
			expected = "+64211885232";
			actual = Fmt.CleanMobile(valueToTest);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void FmtCrunch() {
			Assert.AreEqual("mike-hello", Fmt.Crunch("Mike'--hel*lo!-"));
			Assert.AreEqual("mike-hello", Fmt.Crunch("  Mike'--hel*lo!-"));
			Assert.AreEqual("mike-hel-lo", Fmt.Crunch(" Mike hel lo - "));
			Assert.AreEqual("", Fmt.Crunch(" * "));
			Assert.AreEqual("", Fmt.Crunch("  "));
			Assert.AreEqual("", Fmt.Crunch(null));
			Assert.AreEqual("247-full-roadside-assistance-for-breakdown-and-accident-emergency", Fmt.Crunch("24/7 Full Roadside Assistance for Breakdown and Accident Emergency"));
			Assert.AreEqual("247-full-androadside-assistance-for-andandandandandandandandandandandandandandandandandandandandandandandandandandandandandandandandandandandandbreakdown-andaccident-emergency", Fmt.Crunch("24/7 Full & Roadside Assistance for &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&& Breakdown & Accident Emergency"));
		}

		[TestMethod()]
		public void FmtCleanDigits() {
			string valueToTest;
			string expected;
			string actual;

			valueToTest = "(021) 1885 232";
			expected = "0211885232";
			actual = Fmt.CleanDigits(valueToTest);
			Assert.AreEqual(expected, actual);

			Assert.AreEqual("3545", Fmt.CleanDigits("-35.45"));
			Assert.AreEqual("545", Fmt.CleanDigits("-acd5.45"));
			Assert.AreEqual("", Fmt.CleanDigits("-"));
			Assert.AreEqual(null, Fmt.CleanDigits(null));
		}

		[TestMethod()]
		public void FmtCleanInt() {
			Assert.AreEqual(-35, Fmt.CleanInt("-35.45"));
			Assert.AreEqual(-35004, Fmt.CleanInt(" dsfs$-35,004.45"));
			Assert.AreEqual(35, Fmt.CleanInt("@3-5.45"));
			Assert.AreEqual(0, Fmt.CleanInt("@"));
			Assert.AreEqual(0, Fmt.CleanInt(""));
			Assert.AreEqual(0, Fmt.CleanInt(null));
		}

		/// <summary>
		///A test for FmtDateTime
		///</summary>
		[TestMethod()]
		public void FmtDateTimeTest() {
			string d = "18 feb 2000 16:50";
			string expected = "18-Feb-2000 4:50pm";
			string actual;
			if (!Fmt.DefaultDateFormatHasDashes) {
				expected = expected.Replace("-", " ");
			}
			actual = Fmt.DateTime(d);
			Assert.AreEqual(expected, actual);
			//Assert.Inconclusive("Verify the correctness of this test method.");
		}

		[TestMethod()]
		public void TestFmtPlural() {
			Assert.AreEqual("dogs", Fmt.Plural("dog"));
			Assert.AreEqual("doggies", Fmt.Plural("doggy"));
			Assert.AreEqual("information", Fmt.Plural("information"));
			Assert.AreEqual("5 people", Fmt.Plural("5 person"));
			Assert.AreEqual("foxes", Fmt.Plural("fox"));
			Assert.AreEqual("bosses", Fmt.Plural("boss"));
		}

		[TestMethod()]
		public void TestFmtSqlString() {
			Assert.AreEqual("o''brien", Fmt.SqlString("o'brien"));
			Assert.AreEqual("o''brien@o''malleys.com", Fmt.SqlString("o'brien@o'malleys.com"));
			Assert.AreEqual("<whatever>!@#$%^&*()-+", Fmt.SqlString("<whatever>!@#$%^&*()-+"));
			Assert.AreEqual("information", Fmt.SqlString("information"));
			Assert.AreEqual("double''''apos", Fmt.SqlString("double''apos"));
		}

		[TestMethod()]
		public void TestStripTagComplete() {
			Assert.AreEqual("b", Fmt.StripTagsComplete("<b>b"));
			Assert.AreEqual("blue colour's", Fmt.StripTagsComplete("<b style='color:green and blue'>blue colour's</b>"));
			Assert.AreEqual(" Next> ", Fmt.StripTagsComplete("<h1>Next></h1>"));
			Assert.AreEqual(" mike jelly beans ", Fmt.StripTagsComplete("<h1>mike</h1> <h1 style=\"color:red;\">jelly beans</h1>"));
			Assert.AreEqual("and &", Fmt.StripTagsComplete("and &amp;"));
		}
		[TestMethod()]
		public void TestStripTags() {
			Assert.AreEqual("b", Fmt.StripTags("<b>b"));
			Assert.AreEqual("blue colour's", Fmt.StripTags("<b style='color:green and blue'>blue colour's</b>"));
			Assert.AreEqual("Next>", Fmt.StripTags("<h1>Next></h1>"));
			Assert.AreEqual("mike jelly beans", Fmt.StripTags("<h1>mike</h1> <h1 style=\"color:red;\">jelly beans</h1>"));
			Assert.AreEqual("and &amp;", Fmt.StripTags("and &amp;"));
		}

		public void TestStripTagsSpeed() {
			string str = Util.HttpGet("http://nzherald.co.nz/");

			var now = DateTime.Now;
			for (var i = 0; i < 100; i++) {
				Fmt.StripTags(str);
			}
			Web.Write(" - StripTags:" + (DateTime.Now - now).TotalMilliseconds);


			now = DateTime.Now;
			for (var i = 0; i < 100; i++) {
				Fmt.StripTagsComplete(str);
			}
			Web.Write(" - StripTagsComplete: " + (DateTime.Now - now).TotalMilliseconds);

			now = DateTime.Now;
			for (var i = 0; i < 100; i++) {
				//			Beweb.HtmlTagStripper.HtmlStripTags(str, true, false);
			}
			Web.Write(" - ThirdPartyHtmlStripTags: " + (DateTime.Now - now).TotalMilliseconds);




		}

		[TestMethod()]
		public void TestTruncHtml() {
			string str = Util.HttpGet("http://nzherald.co.nz/");

			var now = DateTime.Now;
			for (var i = 0; i < 100; i++) {
				Fmt.TruncHTML(str, 100);
			}
			Web.Write(" - 1:" + (DateTime.Now - now).TotalMilliseconds);
		}

		[TestMethod()]
		public void TestFmtSqlBoolean() {
			Assert.AreEqual("1", Fmt.SqlBoolean("true"));
			Assert.AreEqual("1", Fmt.SqlBoolean("True"));
			Assert.AreEqual("1", Fmt.SqlBoolean(true));
			Assert.AreEqual("0", Fmt.SqlBoolean(false));
			Assert.AreEqual("0", Fmt.SqlBoolean("false"));
			Assert.AreEqual("0", Fmt.SqlBoolean("False"));
		}

		[TestMethod()]
		public void TestFmtTitleCase() {
			Assert.AreEqual("Mike Nelson", Fmt.TitleCase("MIKE NELSON"));
			Assert.AreEqual("Mike Nelson", Fmt.TitleCase("Mike Nelson"));
			Assert.AreEqual("Mike Nelson", Fmt.TitleCase("mike nelson"));
			Assert.AreEqual("McKay Shipping Ltd", Fmt.TitleCase("McKay Shipping Ltd"));
			Assert.AreEqual("McKay Shipping Ltd", Fmt.TitleCase("MCKAY SHIPPING LTD"));
			Assert.AreEqual("McKay Shipping Ltd", Fmt.TitleCase("mckay shipping ltd"));
			Assert.AreEqual("Mike Nelson-Smith", Fmt.TitleCase("MIKE NELSON-SMITH"));
			Assert.AreEqual("Mike Nelson-Smith", Fmt.TitleCase("Mike Nelson-Smith"));
			Assert.AreEqual("Mike Nelson-Smith", Fmt.TitleCase("mike nelson-smith"));
			Assert.AreEqual("Pil McKay Shipping Ltd", Fmt.TitleCase("PIL MCKAY SHIPPING LTD")); 
			//Assert.AreEqual("PIL (McKAY SHIPPING LTD)", Fmt.TitleCase("PIL (McKAY SHIPPING LTD)"));  20131223jn removed, not titlecase example
		}

		[TestMethod()]
		public void TestTimeDiffTextFuture() {
			Assert.AreEqual("1 day", Fmt.TimeDiffTextFuture(DateTime.Now, DateTime.Now.AddDays(1)));
			Assert.AreEqual("1 day, 1 minute", Fmt.TimeDiffTextFuture(DateTime.Now, DateTime.Now.AddDays(1).AddMinutes(1)));
		}

		[TestMethod()]
		public void TestFmtYesNo() {
			Assert.AreEqual("Yes", Fmt.YesNo("yes"));
			Assert.AreEqual("Yes", Fmt.YesNo("true"));
			Assert.AreEqual("Yes", Fmt.YesNo("True"));
			Assert.AreEqual("Yes", Fmt.YesNo("1"));
			Assert.AreEqual("Yes", Fmt.YesNo("Yes"));
			Assert.AreEqual("Yes", Fmt.YesNo(true));
			Assert.AreEqual("No", Fmt.YesNo(false));
			Assert.AreEqual("No", Fmt.YesNo("false"));
			Assert.AreEqual("No", Fmt.YesNo("no"));
			Assert.AreEqual("No", Fmt.YesNo("NO"));
			Assert.AreEqual("No", Fmt.YesNo("0"));
			Assert.AreEqual("No", Fmt.YesNo((bool?)null));
			Assert.AreEqual("n/a", Fmt.YesNo((string)null));
			Assert.AreEqual("n/a", Fmt.YesNo(""));
		}

		[TestMethod()]
		public void TestTimeDiffText() {
			Assert.AreEqual("1 month ago", Fmt.TimeDiffText(DateTime.Now.AddDays(-30)));
			Assert.AreEqual("3 months ago", Fmt.TimeDiffText(DateTime.Now.AddMonths(-3)));
			Assert.AreEqual("1 week ago", Fmt.TimeDiffText(DateTime.Now.AddDays(-7)));
			Assert.AreEqual("2 weeks ago", Fmt.TimeDiffText(DateTime.Now.AddDays(-14)));
			Assert.AreEqual("1 day ago", Fmt.TimeDiffText(DateTime.Now.AddDays(-1)));
			Assert.AreEqual("6 days ago", Fmt.TimeDiffText(DateTime.Now.AddDays(-6)));
			Assert.AreEqual("1 hour ago", Fmt.TimeDiffText(DateTime.Now.AddHours(-1)));
			Assert.AreEqual("1 minute ago", Fmt.TimeDiffText(DateTime.Now.AddMinutes(-1)));
			Assert.AreEqual("5 seconds ago", Fmt.TimeDiffText(DateTime.Now.AddSeconds(-5)));
		}

		[TestMethod()]
		public void TestPublishStatus() {
			DateTime yesterday = DateTime.Today.AddDays(-1);
			DateTime today = DateTime.Today;
			DateTime tomorrow = DateTime.Today.AddDays(1);
			DateTime laterToday = DateTime.Now.AddHours(1);
			DateTime earlierToday = DateTime.Now.AddHours(-1);
			DateTime fortyHoursAgo = DateTime.Now.AddHours(-40);
			DateTime twentyHoursAgo = DateTime.Now.AddHours(-20);
			DateTime fortyHoursTime = DateTime.Now.AddHours(40);
			DateTime twentyHoursTime = DateTime.Now.AddHours(20);
			Assert.AreEqual("Live", Fmt.PublishStatus(yesterday, tomorrow));
			Assert.AreEqual("Live", Fmt.PublishStatus(yesterday, today));
			Assert.AreEqual("Live", Fmt.PublishStatus(yesterday, laterToday));
			Assert.AreEqual("Expired", Fmt.PublishStatus(yesterday, earlierToday));
			Assert.AreEqual("Expired", Fmt.PublishStatus(yesterday, fortyHoursAgo));
			Assert.AreEqual("Expired", Fmt.PublishStatus(yesterday, twentyHoursAgo));
			Assert.AreEqual("Live", Fmt.PublishStatus(yesterday, fortyHoursTime));
			Assert.AreEqual("Live", Fmt.PublishStatus(yesterday, twentyHoursTime));

			Assert.AreEqual("Live", Fmt.PublishStatus(today, tomorrow));
			Assert.AreEqual("Live", Fmt.PublishStatus(today, today));
			Assert.AreEqual("Live", Fmt.PublishStatus(today, laterToday));
			Assert.AreEqual("Expired", Fmt.PublishStatus(today, earlierToday));
			Assert.AreEqual("Expired", Fmt.PublishStatus(today, fortyHoursAgo));
			Assert.AreEqual("Expired", Fmt.PublishStatus(today, twentyHoursAgo));
			Assert.AreEqual("Live", Fmt.PublishStatus(today, fortyHoursTime));
			Assert.AreEqual("Live", Fmt.PublishStatus(today, twentyHoursTime));

			Assert.AreEqual("Scheduled", Fmt.PublishStatus(tomorrow, tomorrow));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(tomorrow, today));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(tomorrow, laterToday));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(tomorrow, earlierToday));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(tomorrow, fortyHoursAgo));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(tomorrow, twentyHoursAgo));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(tomorrow, fortyHoursTime));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(tomorrow, twentyHoursTime));

			Assert.AreEqual("Scheduled", Fmt.PublishStatus(laterToday, tomorrow));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(laterToday, today));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(laterToday, laterToday));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(laterToday, earlierToday));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(laterToday, fortyHoursAgo));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(laterToday, twentyHoursAgo));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(laterToday, fortyHoursTime));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(laterToday, twentyHoursTime));

			Assert.AreEqual("Live", Fmt.PublishStatus(earlierToday, tomorrow));
			Assert.AreEqual("Live", Fmt.PublishStatus(earlierToday, today));
			Assert.AreEqual("Live", Fmt.PublishStatus(earlierToday, laterToday));
			Assert.AreEqual("Expired", Fmt.PublishStatus(earlierToday, earlierToday));
			Assert.AreEqual("Expired", Fmt.PublishStatus(earlierToday, fortyHoursAgo));
			Assert.AreEqual("Expired", Fmt.PublishStatus(earlierToday, twentyHoursAgo));
			Assert.AreEqual("Live", Fmt.PublishStatus(earlierToday, fortyHoursTime));
			Assert.AreEqual("Live", Fmt.PublishStatus(earlierToday, twentyHoursTime));

			Assert.AreEqual("Live", Fmt.PublishStatus(fortyHoursAgo, tomorrow));
			Assert.AreEqual("Live", Fmt.PublishStatus(fortyHoursAgo, today));
			Assert.AreEqual("Live", Fmt.PublishStatus(fortyHoursAgo, laterToday));
			Assert.AreEqual("Expired", Fmt.PublishStatus(fortyHoursAgo, earlierToday));
			Assert.AreEqual("Expired", Fmt.PublishStatus(fortyHoursAgo, fortyHoursAgo));
			Assert.AreEqual("Expired", Fmt.PublishStatus(fortyHoursAgo, twentyHoursAgo));
			Assert.AreEqual("Live", Fmt.PublishStatus(fortyHoursAgo, fortyHoursTime));
			Assert.AreEqual("Live", Fmt.PublishStatus(fortyHoursAgo, twentyHoursTime));

			Assert.AreEqual("Live", Fmt.PublishStatus(twentyHoursAgo, tomorrow));
			Assert.AreEqual("Live", Fmt.PublishStatus(twentyHoursAgo, today));
			Assert.AreEqual("Live", Fmt.PublishStatus(twentyHoursAgo, laterToday));
			Assert.AreEqual("Expired", Fmt.PublishStatus(twentyHoursAgo, earlierToday));
			Assert.AreEqual("Expired", Fmt.PublishStatus(twentyHoursAgo, fortyHoursAgo));
			Assert.AreEqual("Expired", Fmt.PublishStatus(twentyHoursAgo, twentyHoursAgo));
			Assert.AreEqual("Live", Fmt.PublishStatus(twentyHoursAgo, fortyHoursTime));
			Assert.AreEqual("Live", Fmt.PublishStatus(twentyHoursAgo, twentyHoursTime));

			Assert.AreEqual("Scheduled", Fmt.PublishStatus(fortyHoursTime, tomorrow));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(fortyHoursTime, today));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(fortyHoursTime, laterToday));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(fortyHoursTime, earlierToday));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(fortyHoursTime, fortyHoursAgo));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(fortyHoursTime, twentyHoursAgo));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(fortyHoursTime, fortyHoursTime));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(fortyHoursTime, twentyHoursTime));

			Assert.AreEqual("Scheduled", Fmt.PublishStatus(twentyHoursTime, tomorrow));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(twentyHoursTime, today));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(twentyHoursTime, laterToday));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(twentyHoursTime, earlierToday));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(twentyHoursTime, fortyHoursAgo));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(twentyHoursTime, twentyHoursAgo));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(twentyHoursTime, fortyHoursTime));
			Assert.AreEqual("Scheduled", Fmt.PublishStatus(twentyHoursTime, twentyHoursTime));

		}

		[TestMethod]
		public static void TestDate() {
			Assert.AreEqual("1970-12-20T11:00:00Z", Fmt.DateTimeISOZ(new DateTime(1970, 12, 21)));
		}

		
		[TestMethod]
		public static void TestBoldAsterisk() {
			Assert.AreEqual("Return Economy class airfares from Auckland <b>Tahiti Nui</b>.", Fmt.BoldAsterisk("Return Economy class airfares from Auckland *Tahiti Nui*."));
			Assert.AreEqual("Return Economy class airfares from Auckland <b>Tahiti Nui</b>", Fmt.BoldAsterisk("Return Economy class airfares from Auckland *Tahiti Nui*"));
			Assert.AreEqual("Return Economy class airfares from <b>Auckland </b>Tahiti Nui*.", Fmt.BoldAsterisk("Return Economy class airfares from *Auckland *Tahiti Nui*."));
			Assert.AreEqual("Return Economy class airfares from Auckland <b>Tahiti Nui</b>*.", Fmt.BoldAsterisk("Return Economy class airfares from Auckland *Tahiti Nui**."));
			Assert.AreEqual("Return Economy class airfares from Auckland Tahiti Nui*.", Fmt.BoldAsterisk("Return Economy class airfares from Auckland Tahiti Nui*."));
			Assert.AreEqual("Return Economy class airfares from Auckland *Tahiti Nui.", Fmt.BoldAsterisk("Return Economy class airfares from Auckland *Tahiti Nui."));
			Assert.AreEqual("*Return Economy class airfares from Auckland Tahiti Nui.", Fmt.BoldAsterisk("*Return Economy class airfares from Auckland Tahiti Nui."));
			Assert.AreEqual("<b>Return</b> Economy class airfares from Auckland Tahiti Nui.", Fmt.BoldAsterisk("*Return* Economy class airfares from Auckland Tahiti Nui."));
			Assert.AreEqual("<b>Return $99.99</b> Economy class airfares from Auckland Tahiti Nui.", Fmt.BoldAsterisk("*Return $99.99* Economy class airfares from Auckland Tahiti Nui."));
			
			Assert.AreEqual("Nui<b>Private  Resort</b>", Fmt.BoldAsterisk("Nui*Private  Resort*"));
			Assert.AreEqual(" Nui <b>Private  Resort</b>,  fees *Business.", Fmt.BoldAsterisk(" Nui *Private  Resort*,  fees *Business."));
			Assert.AreEqual(" Nui<b>Private  Resort</b>,  fees *Business.", Fmt.BoldAsterisk(" Nui*Private  Resort*,  fees *Business."));
			Assert.AreEqual(" Nui<b>.Private  Resort</b>,  fees *Business.", Fmt.BoldAsterisk(" Nui*.Private  Resort*,  fees *Business."));

			Assert.AreEqual(" Nui*.\r\nPrivate  Resort*,  fees.\r\n\r\n*Business.", Fmt.BoldAsterisk(" Nui*.\r\nPrivate  Resort*,  fees.\r\n\r\n*Business."));
			Assert.AreEqual(" Nui*.\r\nPrivate  <b>Resort</b>,  fees.\r\n\r\n*Business.", Fmt.BoldAsterisk(" Nui*.\r\nPrivate  *Resort*,  fees.\r\n\r\n*Business."));
			
			Assert.AreEqual("Return  Nui*.\r\nPrivate return.\r\n1-Night pre & post Resort*, including breakfast.\r\n7-Night  Spirit.\r\nSuperb dining, with  cruise.\r\nOnboard entertainment.\r\nPort   fees.\r\n\r\n*Business  on request.", Fmt.BoldAsterisk("Return  Nui*.\r\nPrivate return.\r\n1-Night pre & post Resort*, including breakfast.\r\n7-Night  Spirit.\r\nSuperb dining, with  cruise.\r\nOnboard entertainment.\r\nPort   fees.\r\n\r\n*Business  on request."));

			Assert.AreEqual("Return Economy class airfares from Auckland to Papeete flying Air Tahiti Nui*.\r\nPrivate transfers from airport to hotel to yacht and return.\r\n1-Night pre & post cruise hotel accommodation at the Radisson Plaza Resort*, including full American buffet breakfast.\r\n7-Night yacht cruise aboard Wind Spirit.\r\nSuperb dining, with all main meals included during your cruise.\r\nOnboard entertainment.\r\nPort taxes and government fees.\r\n\r\n*Business Class upgrades and additional night rates available on request.", Fmt.BoldAsterisk("Return Economy class airfares from Auckland to Papeete flying Air Tahiti Nui*.\r\nPrivate transfers from airport to hotel to yacht and return.\r\n1-Night pre & post cruise hotel accommodation at the Radisson Plaza Resort*, including full American buffet breakfast.\r\n7-Night yacht cruise aboard Wind Spirit.\r\nSuperb dining, with all main meals included during your cruise.\r\nOnboard entertainment.\r\nPort taxes and government fees.\r\n\r\n*Business Class upgrades and additional night rates available on request."));

			var now = DateTime.Now;
			for (int i = 0;i<1000; i++) {
				var x = Fmt.BoldAsterisk("Return Economy bold");
				var y = Fmt.BoldAsterisk("Return Economy bold\n*mike");
				var z = Fmt.BoldAsterisk("Return Economy *bold*");
				var q = Fmt.BoldAsterisk("Return Economy class airfares from Auckland to Papeete flying Air Tahiti Nui*.\r\nPrivate transfers froairfares from Auckland to Papeete flying Air Tahiti Nui*.\r\nPrivate transfers froairfares from Auckland to Papeete flying Air Tahiti Nui*.\r\nPrivate transfers froairfares from Auckland to Papeete flying Air Tahiti Nui*.\r\nPrivate transfers froairfares from Auckland to Papeete flying Air Tahiti Nui*.\r\nPrivate transfers froairfares from Auckland to Papeete flying Air Tahiti Nui*.\r\nPrivate transfers froairfares from Auckland to Papeete flying Air Tahiti Nui*.\r\nPrivate transfers from airport to hotel to yacht and return.\r\n1-Night pre & eturn.\r\n1-Night pre & eturn.\r\n1-Night pre & eturn.\r\n1-Night pre & eturn.\r\n1-Night pre & eturn.\r\n1-Night pre & eturn.\r\n1-Night pre & post cruise hotel accommodation at the Radisson Plaza Resort*, including full American buffet breakfast.\r\n7-Night yacht cruise aboard Wind Spirit.\r\nSuperb dining, with all main meals included during your cruise.\r\nOnboard entertainment.\r\nPort taxes and government fees.\r\n\r\n*Business Class upgrades and additional night rates available on request.");
			}
			Web.Write(now.FmtMillisecondsElapsed());

		}


		[TestMethod]
		public void TestIsValidEmailAddress() {
			Assert.AreEqual(true, Fmt.IsValidEmailAddress("mike@man.com"));
			Assert.AreEqual(true, Fmt.IsValidEmailAddress("mike@man.kiwi"));
			Assert.AreEqual(true, Fmt.IsValidEmailAddress("mike@beweb.co.nz"));
			Assert.AreEqual(true, Fmt.IsValidEmailAddress("Phil.o'neil@potatoes.ie"));
			Assert.AreEqual(true, Fmt.IsValidEmailAddress("André+55@beweb.co.nz"));
			Assert.AreEqual(true, Fmt.IsValidEmailAddress("André+55@test.co.nz"));
			Assert.AreEqual(true, Fmt.IsValidEmailAddress("A ndré+55@test.co.nz"));
			Assert.AreEqual(false, Fmt.IsValidEmailAddress("mike%20is%26.the%25%3d%20man"));
		}
	}

}
#endif
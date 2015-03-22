#define TestExtensions
#define BaseTypeExtensions
#define Fmt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using System.Web.UI.WebControls;
using System.Reflection;
using System.ComponentModel;
using Beweb;
using Assert = Beweb.Assert;

namespace Beweb {


	/// <summary>
	/// extension methods to add features to built in types eg string, int, object etc
	/// </summary>
	public static class BaseTypeExtensions {
		//---------------------------------------------------------
		// EXTENSION METHODS TO ADD FEATURES TO TYPE
		//---------------------------------------------------------

		/// <summary>
		/// Return true if a nullable type or a strign type
		/// </summary>
		/// <param name="theType"></param>
		/// <returns></returns>
		public static bool IsNullableTypeOrString(this Type theType) {
			if (theType == null) return false;
			return ((theType == typeof(string)) || (theType.IsGenericType && theType.GetGenericTypeDefinition().Equals(typeof(Nullable<>))));
		}

		/// <summary>
		/// Return true if a nullable type 
		/// </summary>
		/// <param name="theType"></param>
		/// <returns></returns>
		public static bool IsNullableType(this Type theType) {
			if (theType == null) return false;
			return ((theType.IsGenericType && theType.GetGenericTypeDefinition().Equals(typeof(Nullable<>))));
		}

		/// <summary>
		/// Returns a nullable version of the given type.
		/// eg fieldType.MakeNullableType() -- say fieldType is DateTime, this would return the type Nullable DateTime
		/// </summary>
		public static Type MakeNullableType(this Type theType) {
			if (theType == null) return null;						 //mike says this doent make sense, but it better than trying to run the next line
			if (theType == typeof(System.DBNull)) return theType;
			if (theType == typeof(System.String)) return theType;
			if (theType.IsNullableType()) return theType;
			return typeof(Nullable<>).MakeGenericType(theType);
		}

		//---------------------------------------------------------
		// EXTENSION METHODS TO ADD FEATURES TO DATAREADER
		//---------------------------------------------------------


		public static DbDataReader MoveToPage(this DbDataReader dataReader, int pageNum, int itemsPerPage) {
			// pageNum is one-based
			int pos = itemsPerPage * (pageNum - 1);
			for (int i = 1; i <= pos; i++) {
				if (!dataReader.Read()) break;
			}
			return dataReader;
		}

		public static List<string> GetFieldNames(this DbDataReader dataReader) {
			var result = new List<string>();
			for (int i = 0; i < dataReader.VisibleFieldCount; i++) {
				// for each column
				result.Add(dataReader.GetName(i));
			}
			return result;
		}

		public static bool FieldExists(this DbDataReader dataReader, string fieldName) {
			return BewebData.FieldExists(dataReader, fieldName);
		}

		//---------------------------------------------------------
		// EXTENSION METHODS TO ADD FEATURES TO NUMBERS
		//---------------------------------------------------------

		/// <summary>
		/// Returns the supplied number and word, making the word plural if the number is not one.
		/// eg "You have "+count.Plural("box") -> 
		/// <param name="number">The number of things eg 2 boxes</param>
		/// <param name="word">A string ending in an English word</param>
		/// </summary>
		/// <returns></returns>	
		public static string Plural(this int number, string word) {
			return Fmt.Plural(number, word);
		}

		/// <summary>
		/// Formats a number commas separating the thousands and auto-detect number of decimal places. If null result is empty string.
		/// </summary>
		public static string FmtNumber(this decimal amount) {
			return Fmt.Number(amount);
		}
		public static string FmtNumber(this decimal amount, int decimalPlaces) {
			return Fmt.Number(amount, decimalPlaces);
		}
		/// <summary>
		/// Formats a number commas separating the thousands. If null result is empty string.
		/// </summary>
		public static string FmtNumber(this int amount) {
			return Fmt.Number(amount);
		}
		public static string FmtCurrency(this int amount) {
			return Fmt.Currency(amount.ToInt(), 0);
		}
		public static string FmtCurrency(this int? amount) {
			return Fmt.Currency(amount.ToInt(0), 0);
		}

		public static string FmtNumber(this int amount, int decimalPlaces) {
			return Fmt.Number(amount, decimalPlaces);
		}
		/// <summary>
		/// Formats a number commas separating the thousands and auto-detect number of decimal places. If null result is empty string.
		/// </summary>
		public static string FmtNumber(this double amount) {
			return Fmt.Number(amount);
		}
		public static string FmtNumber(this double amount, int decimalPlaces) {
			return Fmt.Number(amount, decimalPlaces);
		}

		public static string FmtCurrency(this decimal amount) {
			return Fmt.Currency(amount);
		}
		public static string FmtCurrency(this decimal amount, bool noDollarSymbol) {
			return Fmt.Currency(amount).Replace("$", "");
		}
		public static string FmtCurrency(this decimal? amount) {
			return Fmt.Currency(amount);
		}
		public static string FmtCurrency(this decimal? amount, bool noDollarSymbol) {
			return Fmt.Currency(amount).Replace("$", "");
		}
		public static string FmtCurrencyWithOutCents(this decimal amount) {
			return Fmt.Currency(amount, 0);
		}
		public static string FmtCurrencyWithOutCents(this decimal amount, bool noDollarSymbol) {
			return Fmt.Currency(amount, 0).Replace("$", "");
		}
		public static string FmtCurrencyWithOutCents(this decimal? amount) {
			return Fmt.Currency(amount, 0);
		}
		public static string FmtCurrencyWithOutCents(this decimal? amount, bool noDollarSymbol) {
			return Fmt.Currency(amount, 0).Replace("$", "");
		}

		//---------------------------------------------------------
		// EXTENSION METHODS TO ADD FEATURES TO BOOLEANS
		//---------------------------------------------------------

		public static string FmtYesNo(this bool value) {
			return Fmt.YesNo(value);
		}
		public static string FmtYesNo(this bool? value) {
			return Fmt.YesNo(value);
		}

		public static string ToStringLower(this bool value) {
			return value ? "true" : "false";
		}
		public static string ToStringLower(this bool? value) {
			return value ?? false ? "true" : "false";
		}
		public static string FmtJs(this bool value) {
			return value ? "true" : "false";
		}
		public static string FmtJs(this bool? value) {
			return value ?? false ? "true" : "false";
		}

		//---------------------------------------------------------
		// EXTENSION METHODS TO ADD FEATURES TO DATES
		//---------------------------------------------------------

		public static string FmtDate(this DateTime value) {
			return Fmt.Date(value);
		}

		public static string FmtDateTime(this DateTime value) {
			return Fmt.DateTime(value);
		}

		public static string FmtShortDate(this DateTime value) {
			return Fmt.ShortDate(value);
		}

		public static string FmtLongDate(this DateTime value) {
			return Fmt.LongDate(value);
		}

		public static string FmtDate(this DateTime? value) {
			return Fmt.Date(value);
		}

		public static string FmtDateTime(this DateTime? value) {
			return Fmt.DateTime(value);
		}

		public static string FmtShortDate(this DateTime? value) {
			return Fmt.ShortDate(value);
		}

		public static string FmtLongDate(this DateTime? value) {
			return Fmt.LongDate(value);
		}
		public static string FmtMonthYear(this DateTime? value) {
			return Fmt.MonthYear(value);
		}
		public static string FmtMonthYear(this DateTime value) {
			return Fmt.MonthYear(value);
		}

		public static string FmtDayMonth(this DateTime? value) {
			if (!value.HasValue) return "";
			return Fmt.DayMonth(value.Value);
		}

		public static string FmtDayMonth(this DateTime value) {
			return Fmt.DayMonth(value);
		}
		public static string FmtYearMonth(this DateTime? value) {
			return Fmt.YearMonth(value);
		}
		public static string FmtYearMonth(this DateTime value) {
			return Fmt.YearMonth(value);
		}

		public static string GetMonthName(this DateTime value) {
			return VB.MonthName(value);
		}

		public static string GetMonthName(this DateTime? value) {
			return VB.MonthName(value);
		}

		public static string TimeDiffText(this DateTime? value) {
			return Fmt.TimeDiffText(value, DateTime.Now);
		}

		public static string TimeDiffText(this DateTime value) {
			return Fmt.TimeDiffText(value, DateTime.Now);
		}

		//---------------------------------------------------------
		// EXTENSION METHODS TO ADD FEATURES TO COLLECTIONS
		//---------------------------------------------------------

		public static string Join(this string[] value, string delimiter) {
			return String.Join(delimiter, value);
		}
		public static string[] AppendArray(this string[] value, string[] appendSource) {
			string result = value.Join(",") + "," + appendSource.Join(",");
			return result.Split(',');
		}

		/// <summary>
		/// Calls ToString() on each element in the list and concatenates the strings together with a delimiter
		/// </summary>
		public static string Join(this IEnumerable value, string delimiter) {
			var result = new StringBuilder();
			foreach (var i in value) {
				if (result.Length != 0) result.Append(delimiter);
				result.Append(i.ToString());
			}
			return result.ToString();
		}
		/// <summary>
		/// Returns a single string version of this List of Strings (same as Join extension method) 
		/// </summary>
		public static string Join(this IEnumerable<string> value, string delimiter) {
			//return String.Join(delimiter, value.ToArray());
			return String.Join(delimiter, value);               // slightly faster? if this does not compile in old version of .net replace with value.ToArray()
		}
		/// <summary>
		/// Returns a single string version of this List of Strings (same as Join extension method) 
		/// </summary>
		public static string Join(this ArrayList value, string delimiter) {
			string[] array = new string[value.Count];
			value.ToArray(typeof(string)).CopyTo(array, 0);
			return String.Join(delimiter, array);
		}

		/// <summary>
		/// Calls ToString() on each element in the list and concatenates the strings together with a delimiter
		/// </summary>
		public static string ToString(this IEnumerable value, string delimiter) {
			return value.Join(delimiter);
		}

		/// <summary>
		/// Calls the supplied lambda selector function for each element in the list. Your lambda should return a string for each element. Then the strings are concatenated together with a delimiter.
		/// eg myPageList.ToString(p=>p.Title, ", ")      -- returns a comma separated list of page titles of all pages in myPageList
		/// </summary>
		public static string ToString<TSource>(this IEnumerable<TSource> value, Func<TSource, object> lambdaReturningString, string delimiter) {
			return value.Select(lambdaReturningString).ToString(delimiter);
		}

		/// <summary>
		/// Given a list, returns a list of another type. This is simply a shortcut to value.Select(getter).ToList()
		/// eg myPageList.ToList(p=>p.PageID)           -- returns a list of pageIDs of all pages in myPageList
		/// you can also use myPageList.Pluck(p=>p.PageID)           -- returns a list of pageIDs of all pages in myPageList
		/// </summary>
		public static List<TResult> ToList<TSource, TResult>(this IEnumerable<TSource> value, Func<TSource, TResult> getter) {
			return value.Select(getter).ToList();
		}

		/// <summary>
		/// Given a list, returns a list of another type. This is simply a shortcut to value.Select(getter).ToList()
		/// eg myPageList.Pluck(p=>p.PageID)           -- returns a list of pageIDs of all pages in myPageList
		/// you can also use myPageList.ToList(p=>p.PageID)           -- returns a list of pageIDs of all pages in myPageList
		/// </summary>
		///<example>
		/// Models.PheasantList birds = source.Pluck(p=>p.IsPleasant);
		/// do while(birds.KeepOnPluckingPheasants){
		///   if(birds.PheasantPlucking==Status.Done)break;
		/// }
		///</example>
		public static List<TResult> Pluck<TSource, TResult>(this IEnumerable<TSource> value, Func<TSource, TResult> getter) {
			return value.ToList(getter);
		}

		/// <summary>
		/// Returns groups within the list. For example, finding all the destinations which have active groups:
		/// 	var deal = DealList.LoadActive();
		///		var dealTypes = deal.GetGroups(d=>d.Destination);
		///		foreach (var t in dealTypes) {
		///			Web.Write(t.Title);
		///		}
		/// </summary>
		public static List<TResult> GetGroups<TSource, TResult>(this IEnumerable<TSource> value, Func<TSource, TResult> getter) {
			return value.GroupBy(getter).Select(g => g.Key).ToList();
		}

		/// <summary>
		/// A null safe version of mylist.AddRange(arrayOrList).
		/// </summary>
		public static void AddItems<T>(this List<T> someList, IEnumerable<T> collection) {
			if (collection == null) return;
			someList.AddRange(collection);
		}

		/// <summary>
		/// A null safe version of mylist.AddRange(arrayOrList).
		/// But only adds items if they are not already in the list.
		/// </summary>
		public static void AddItemsUnique<T>(this List<T> someList, IEnumerable<T> collection) {
			if (collection == null) return;
			foreach (var item in collection) {
				if (someList.DoesntContain(item)) {
					someList.Add(item);
				}
			}
		}

		/// <summary>
		/// A null safe version of mylist.AddRange(arrayOrList).
		/// But only adds items if they are not already in the list.
		/// </summary>
		public static void AddUnique<T>(this List<T> someList, T item) {
			if (someList.DoesntContain(item)) {
				someList.Add(item);
			}
		}

		//public static string JsEncode(this System.Collections.Generic.ICollection collection) {
		//  string result = "";
		//  foreach (var item in collection) {
		//    result += result.JsEncode();
		//  }
		//  return result;
		//}

		//public static string JsEncode(this System.Collections.Hashtable hash) {
		//  string result = "";
		//  foreach (var key in hash.Keys) {
		//    result += key + ":" + hash[key].JsEncode();
		//  }
		//  return result;
		//}


		//---------------------------------------------------------
		// EXTENSION METHODS TO ADD FEATURES TO STRING
		//---------------------------------------------------------

		// string extensions

		/// <summary>
		/// Useful for text processing. Returns the text in between the given precedingText and succeedingText. Searches from the beginning of the string (left to right) and does not include the precedingText or succeedingText in the result. Throws a ExtractTextException if not found. Pass false as last param if you don't want it to throw exceptions.
		/// </summary>
		/// <param name="sourceText"></param>
		/// <param name="precedingText"></param>
		/// <param name="succeedingText"></param>
		/// <returns></returns>
		public static string ExtractTextBetween(this string sourceText, string precedingText, string succeedingText) {
			return sourceText.ExtractTextBetween(precedingText, succeedingText, true, 0);
		}

		public static string ExtractTextBetween(this string sourceText, string precedingText, string succeedingText, bool throwIfNotFound) {
			return sourceText.ExtractTextBetween(precedingText, succeedingText, throwIfNotFound, 0);
		}
		/// <summary>
		/// Useful for text processing. Returns the text in between the given precedingText and succeedingText if it doesn't exceed the given max chars limit (note: line breaks DO count as a character). Searches from the beginning of the string (left to right) and does not include the precedingText or succeedingText in the result. Throws a ExtractTextException if not found. 
		/// </summary>
		/// <param name="sourceText"></param>
		/// <param name="precedingText"></param>
		/// <param name="succeedingText"></param>
		/// <param name="throwIfNotFound"></param>
		/// <param name="maxChars"></param>
		/// <returns></returns>
		public static string ExtractTextBetween(this string sourceText, string precedingText, string succeedingText, int maxChars) {
			return sourceText.ExtractTextBetween(precedingText, succeedingText, true, maxChars);
		}

		/// <summary>
		/// Useful for text processing. Returns the text in between the given precedingText and succeedingText if it doesn't exceed the given max chars limit (note: line breaks DO count as a character). Searches from the beginning of the string (left to right) and does not include the precedingText or succeedingText in the result. Throws a ExtractTextException if not found. Pass false as last param if you don't want it to throw exceptions.
		/// </summary>
		/// <param name="sourceText"></param>
		/// <param name="precedingText"></param>
		/// <param name="succeedingText"></param>
		/// <param name="throwIfNotFound"></param>
		/// <param name="maxChars"></param>
		/// <returns></returns>
		public static string ExtractTextBetween(this string sourceText, string precedingText, string succeedingText, bool throwIfNotFound, int maxChars) {
			string result = "";
			if (sourceText != null && sourceText.Contains(precedingText)) {
				result = sourceText.Substring(sourceText.IndexOf(precedingText) + precedingText.Length);
				int endPos = result.IndexOf(succeedingText);
				if (endPos != -1) {
					if (maxChars > 0 && endPos - 1 > maxChars) { // AF20150313 exceeds the limit, so return the original text
						result = sourceText;
					} else { // All good
						result = result.Substring(0, endPos);
					}
				} else {
					result = "";
					if (throwIfNotFound) throw new ExtractTextException("ExtractTextBetween: could not find succeedingText. precedingText[" + precedingText + "] succeedingText[" + succeedingText + "]");
				}
			} else {
				if (throwIfNotFound) throw new ExtractTextException("ExtractTextBetween: could not find precedingText. precedingText[" + precedingText + "] succeedingText[" + succeedingText + "]");
			}
			return result;
		}

		/// <summary>
		/// Converts this string to a memory stream. This is useful when you want to call a .net method which takes a stream but you only have a string. Default is to use ASCII encoding, or you can specify Encoding.Unicode as the second param to use Unicode instead.
		/// </summary>
		/// <param name="value"></param>
		/// <returns>A memory stream</returns>
		public static MemoryStream ToStream(this string value) {
			return new MemoryStream(Encoding.ASCII.GetBytes(value));
		}

		/// <summary>
		/// Converts this string to a memory stream. This is useful when you want to call a .net method which takes a stream but you only have a string. Default is to use ASCII encoding, or you can specify Encoding.Unicode as the second param to use Unicode instead.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="encoding">A System.Text.Encoding (eg Encoding.Unicode, Encoding.ASCII or Encoding.UTF8)</param>
		/// <returns>A memory stream</returns>
		public static MemoryStream ToStream(this string value, Encoding encoding) {
			MemoryStream stream = new MemoryStream(encoding.GetBytes(value));
			return stream;
		}


		public static string SaveStreamToFile(this Stream value, string filename) {
			return FileSystem.SaveStreamToFile(value, filename);
		}


		/// <summary>
		/// Converts the string to an enum type. Throws an exception if no enum value exactly matches the string.
		/// </summary>
		/// <typeparam name="TEnum">The enum type</typeparam>
		/// <param name="value"></param>
		/// <returns>An enum value from the given enumeration.</returns>
		public static TEnum ToEnum<TEnum>(this string value) {
			return Fmt.StringToEnum<TEnum>(value);
		}

		/// <summary>
		/// Returns true if the string is a valid email address.
		/// </summary>
		/// <example>"mike@beweb.co.nz".IsValidEmailAddress() - returns true</example>
		/// <example>"mike".IsValidEmailAddress() - returns false</example>
		/// <example>"".IsValidEmailAddress() - returns false</example>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsEmail(this string value) {
			return Fmt.IsValidEmailAddress(value);
		}

		public static bool IsGuid(this string value) {
			if (value != null) {
				Regex guidRegEx = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");

				return guidRegEx.IsMatch(value);
			}
			return false;
		}

		/// <summary>
		/// Returns true if the string is null or empty string. 
		/// Also returns true if whitespace.
		/// Similar to String.IsNullOrEmpty(myString) but also checks for whitespace.
		/// </summary>
		/// <example>"mike".IsBlank() - returns false</example>
		/// <example>"".IsBlank() - returns true</example>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsBlank(this string value) {
			return String.IsNullOrEmpty(value) || value.Trim() == "";
		}

		/// <summary>
		/// Returns true if the string is not null and not empty string and not whitespace.
		/// Similar to !String.IsNullOrEmpty(myString) but also checks for whitespace.
		/// </summary>
		/// <example>"mike".IsNotBlank() - returns true</example>
		/// <example>"".IsNotBlank() - returns false</example>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsNotBlank(this string value) {
			return !IsBlank(value);
		}

		/// <summary>
		/// Converts the string to an integer. If it can't be converted it will return zero.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static int ToInt(this string value) {
			return ConvertToInt(value, 0);
		}

		/// <summary>
		/// Converts the string to an integer. If it can't be converted it returns the supplied default value.
		/// </summary>
		/// <example>"454".ToInt(0) returns 454</example>
		/// <example>"tertre".ToInt(0) returns 0</example>
		/// <example>"".ToInt(0) returns 0</example>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static int ToInt(this string value, int defaultValue) {
			return ConvertToInt(value, defaultValue);
		}
		public static int ToInt(this object value, int defaultValue) {
			if (value == null) return defaultValue;
			return ConvertToInt(value + "", defaultValue);
		}

		/// <summary>
		/// Converts the value to an integer. If it can't be converted it will die.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static int ToInt(this ValueType value) {
			return Convert.ToInt32(value);
		}

		public static short ToShort(this ValueType value) {
			return Convert.ToInt16(value);
		}

		/// <summary>
		/// Converts the value to an double. If it can't be converted it will die.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static double ToDouble(this ValueType value) {
			return Convert.ToDouble(value);
		}

		/// <summary>
		/// Converts the value to a decimal. If it can't be converted it will die.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static decimal? ToDecimal(this double? value) {
			return Math.Round(Convert.ToDecimal(value), 9); // round to 9 decimal places to fix errors with float rounding
		}

		/// <summary>
		/// Converts the value to a decimal. If it can't be converted it will die.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static decimal ToDecimal(this ValueType value) {
			if (value is float || value is double) {
				return Math.Round(Convert.ToDecimal(value), 9); // round to 9 decimal places to fix errors with float rounding
			}
			return Convert.ToDecimal(value);
		}
		public static decimal ToDecimal(this object value, decimal defaultValue) {
			if (value == null) return defaultValue;
			if (value is ValueType) {
				return ToDecimal((ValueType)value);
			}
			return ConvertToDecimal(value.ToString(), defaultValue);
		}
		public static decimal ConvertToDecimal(this string value, decimal defaultValue) {
			decimal result = defaultValue;
			if (value != null) {
				try {
					result = Convert.ToDecimal(value);
				} catch (Exception) {
					//
				}

			}
			return result;
		}
		/// <summary>
		/// convert string to long (int64) or return defaultValue
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static long ToLong(this string value, int defaultValue) {
			return ConvertToLong(value, defaultValue);
		}

		/// <summary>
		/// convert string to long (int64) or return 0
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static long ToLong(this string value) {
			return ConvertToLong(value, 0);
		}
		/// <summary>
		/// Converts the string to an integer. If it can't be converted it throws an exception.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static int ConvertToInt(this string value) {
			return ConvertToInt(value, 0);
		}
		/// <summary>
		/// convert string to bool, or return false
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool ToBool(this string value) {
			return ConvertToBool(value);
		}
		public static bool ToBool(this object value) {
			if (value == null) return false;
			return ConvertToBool(value.ToString());
		}
		/// <summary>
		/// convert string to bool, or return false
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool ConvertToBool(this string value) {
			bool result = false;
			if (value != null) {
				try {
					result = VB.cbool(value);
				} catch (Exception) {
				}
			}
			return result;
		}

		public static long ConvertToLong(this string value) {
			return ConvertToLong(value, 0);
		}

		public static long ConvertToLong(this string value, long defaultValue) {
			long result = defaultValue;
			if (value != null) {
				try {
					result = Convert.ToInt64(value);
				} catch (Exception) {
					//
				}

			}
			return result;
		}
		/// <summary>
		/// Converts the string to an integer. If it can't be converted it returns the supplied default value.
		/// </summary>
		/// <example>"454".ToInt(0) returns 454</example>
		/// <example>"tertre".ToInt(0) returns 0</example>
		/// <example>"".ToInt(0) returns 0</example>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>		
		public static int ConvertToInt(this string value, int defaultValue) {
			int result = defaultValue;
			if (!String.IsNullOrEmpty(value)) {
				try {
					//result = Convert.ToInt32(value);
					//value = Fmt.CleanString(value, @"[^0-9]");

					if (value.Contains(".")) value = value.LeftUntil(".");						 //JN+MN 20121130 this threw execption when passed 30000.0000
					result = (int)Convert.ChangeType(value, typeof(int));
				} catch (Exception) {
					//
				}

			}
			return result;
		}

		/// <summary>
		/// will throw exception if value cannot be converted to an int
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static int ToIntOrDie(this string value) {
			int result;
			//if (!int.TryParse(value, out result)) {  //JN 20121017 this thre execption when passed 30000.0000
			int? i = ToInt(value, null);
			if (i == null) {
				throw new ProgrammingErrorException("Failed to convert [" + value + "] to integer.");
			}
			result = i.Value;
			return result;
		}

		/// <summary>
		/// Converts the string to an integer. If it can't be converted it returns null.
		/// </summary>
		/// <example>"454".ToInt(null) returns 454</example>
		/// <example>"tertre".ToInt(null) returns null</example>
		/// <example>"".ToInt(null) returns null</example>
		/// <param name="value"></param>
		/// <param name="defaultValue">defaultValue is null</param>
		/// <returns></returns>		
		public static int? ToInt(this string value, Null defaultValue) {
			if (value.IsBlank()) return null;

			int? result = null;

			try {
				result = ConvertToInt(value);
			} catch (Exception) {
				result = null;
			}

			return result;
		}
		/// <summary>
		/// Converts the object to an integer. If it can't be converted it returns null.
		/// </summary>
		/// <example>"454".ToInt(null) returns 454</example>
		/// <example>"tertre".ToInt(null) returns null</example>
		/// <example>"".ToInt(null) returns null</example>
		/// <param name="value"></param>
		/// <param name="defaultValue">defaultValue is null</param>
		/// <returns></returns>		
		public static int? ToInt(this object value, Null defaultValue) {
			if (value + "" == "") return null;

			int? result = null;

			try {
				result = ConvertToInt(value + "");
			} catch (Exception) {
				result = null;
			}

			return result;
		}

		/// <summary>
		/// Converts the string to a decimal. If it can't be converted it throws an exception.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static decimal ToDecimal(this string value) {
			//value = Fmt.CleanString(value, @"[^0-9\.]"); todo add this and test
			decimal result = 0;

			try {
				result = Convert.ToDecimal(value);
			} catch (Exception) {
				throw new Exception("ToDecimal failed: [" + value + "]");
			}

			return result;
			//return Convert.ToDecimal(value);
		}

		/// <summary>
		/// Converts the string to a double. If it can't be converted it throws an exception.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static double ToDouble(this string value) {
			double result = 0;

			try {
				result = Convert.ToDouble(value);
			} catch (Exception) {
				throw new Exception("ToDouble failed: [" + value + "]");
			}

			return result;
		}

		public static double ToDouble(this string value, double defaultValue) {
			double result = defaultValue;
			if (value.IsNotBlank()) {
				try {
					result = Convert.ToDouble(value);
				} catch (Exception) {
					//
				}

			}
			return result;
		}

		public static double? ToDouble(this string value, Null defaultValue) {
			double? result = null;
			if (value.IsNotBlank()) {
				try {
					result = Convert.ToDouble(value);
				} catch (Exception) {
					//
				}

			}
			return result;
		}

		/// <summary>
		/// Converts the string to a nullable DateTime. If it can't be converted it returns null.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue">defaultValue is null</param>
		/// <returns></returns>		
		public static DateTime? ConvertToDate(this string value, Null defaultValue) {
			if (value.IsBlank()) return null;

			DateTime result;

			bool isValid = DateTime.TryParse(value, out result);
			if (!isValid) {
				isValid = DateTime.TryParse(value.Replace(".", ":"), out result);
			}

			//try {
			//  result = Convert.ToDateTime(value);
			//} catch (Exception) {
			//  result = null;
			//}

			if (!isValid) {
				return null;
			}
			return result;
		}

		/// <summary>
		/// Converts the string to a nullable DateTime. If it can't be converted it returns null.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue">defaultValue is a default date eg today</param>
		/// <returns></returns>		
		public static DateTime ConvertToDate(this string value, DateTime defaultValue) {
			return ConvertToDate(value, null) ?? defaultValue;
		}

		/// <summary>
		/// return the source, unless it's null or blank, then return the default value
		/// </summary>
		/// <param name="source">string to return, unless null</param>
		/// <param name="defaultValue"></param>
		/// <returns>the source, unless it's null or blank, then return the default value</returns>
		public static string DefaultValue(this string source, string defaultValue) {
			return Util.DefaultValue(source, defaultValue);
		}

		/// <summary>
		/// Converts the string to a decimal. If it can't be converted it returns the supplied default value.
		/// </summary>
		/// <example>"454".ToInt(0) returns 454</example>
		/// <example>"tertre".ToInt(0) returns 0</example>
		/// <example>"".ToInt(0) returns 0</example>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>		
		public static decimal ToDecimal(this string value, int defaultValue) {
			decimal result = defaultValue;

			try {
				result = Convert.ToDecimal(value);
			} catch (Exception) {
				//
			}

			return result;
		}

		/// <summary>
		/// Converts the string to a decimal. If it can't be converted it returns null.
		/// </summary>
		/// <example>"454.45".ToDecimal(null) returns 454.45</example>
		/// <example>"tertre".ToDecimal(null) returns null</example>
		/// <example>"".ToDecimal(null) returns null</example>
		/// <param name="value"></param>
		/// <param name="defaultValue">defaultValue is null</param>
		/// <returns></returns>		
		public static decimal? ToDecimal(this string value, Null defaultValue) {
			if (value.IsBlank()) return null;
			decimal? result = null;

			try {
				result = Convert.ToDecimal(value);
			} catch (Exception) {
				//
			}

			return result;
		}

		public class Null {
			private Null() { }
		}

		/// <summary>
		/// Returns the leftmost n characters of the string.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="numChars"></param>
		/// <returns></returns>
		public static string Left(this string str, int numChars) {
			if (str == null) return null;
			return VB.left(str, numChars);
		}
		public static string Right(this string str, int numChars) {
			if (str == null) return null;
			return VB.right(str, numChars);
		}

		/// <summary>
		/// Get all characters after the given 'splitter' string (ie rightmost characters starting from the last occurrence of the given string).
		/// </summary>
		/// <param name="str">this</param>
		/// <param name="splitter">String to look for and split on</param>
		/// <returns>Rightmost part of string</returns>
		public static string RightFrom(this string str, string splitter) {
			if (str == null) return null;
			if (str.Contains(splitter)) {
				return str.Substring(str.LastIndexOf(splitter) + splitter.Length);
			} else {
				return str;
			}
		}

		/// <summary>
		/// Get all characters after the given 'splitter' string (ie rightmost characters starting from the FIRST occurrence of the given string).
		/// MN 20140822 - made this case insensitive
		/// </summary>
		public static string RightFromFirst(this string str, string splitter) {
			if (str == null) return null;
			if (str.ContainsInsensitive(splitter)) {
				return str.Substring(str.IndexOf(splitter, StringComparison.InvariantCultureIgnoreCase) + splitter.Length);
			} else {
				return str;
			}
		}

		/// <summary>
		/// Get all characters before the given 'splitter' string (ie leftmost characters up until the first occurrence of the given string). Does not include the splitter string.
		/// </summary>
		/// <param name="str">this</param>
		/// <param name="splitter">String to look for and split on</param>
		/// <returns>Leftmost part of string</returns>
		public static string LeftUntil(this string str, string splitter) {
			if (str == null) return null;
			if (str.Contains(splitter)) {
				return str.Substring(0, str.IndexOf(splitter));
			} else {
				return str;
			}
		}

		public static string LeftUntilLast(this string str, string splitter) {
			if (str == null) return null;
			if (str.Contains(splitter)) {
				return str.Substring(0, str.LastIndexOf(splitter));
			} else {
				return str;
			}
		}

		/// <summary>
		/// Returns an array of substrings that are delimited by a splitter string.
		/// If string is null returns null.
		/// </summary>
		public static string[] Split(this string str, string splitter) {
			if (str == null) return null;
			return str.Split(new string[] { splitter }, StringSplitOptions.None);
		}

		/// <summary>
		/// Splits and then trims the results. Removes any results that are blank.
		/// If string is null returns null.
		/// </summary>
		public static string[] SplitTrim(this string str, string splitter) {
			if (str == null) return null;
			return str.Split(new string[] { splitter }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Where(x => x.IsNotBlank()).ToArray();
		}

		//public static string Split(this string str, params string[] splitOnAnyOfTheseStrings) {
		//  return str.Split(splitOnAnyOfTheseStrings, StringSplitOptions.None);
		//}

		/// <summary>
		/// Returns a string with the specified number of characters removed from the start.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="numCharsToRemove"></param>
		/// <returns></returns>
		public static string RemoveCharsFromStart(this string str, int numCharsToRemove) {
			return str.Substring(numCharsToRemove);
		}

		/// <summary>
		/// Returns a string with the specified number of characters removed from the end.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="numCharsToRemove"></param>
		/// <returns></returns>
		public static string RemoveCharsFromEnd(this string str, int numCharsToRemove) {
			return VB.left(str, VB.len(str) - numCharsToRemove);
		}

		/// <summary>
		/// Removes all instances of the substring from the string (ignores case).
		/// </summary>
		public static string Remove(this string str, string substringToRemove) {
			if (substringToRemove == null || str == null) return str;
			return str.ReplaceInsensitive(substringToRemove, "");
		}

		public static string RemoveSuffix(this string str, string substringToRemoveFromEnd) {
			if (substringToRemoveFromEnd == null || str == null) return str;
			if (str.EndsWith(substringToRemoveFromEnd, true)) {
				return str.Substring(0, str.Length - substringToRemoveFromEnd.Length);
			}
			return str;
		}

		public static string RemovePrefix(this string str, string substringToRemoveFromStart) {
			if (substringToRemoveFromStart == null || str == null) return str;
			if (str.StartsWith(substringToRemoveFromStart, true)) {
				return str.Substring(substringToRemoveFromStart.Length);
			}
			return str;
		}

		//public static string RemoveFirstLine(this string str) {
		//	if (str == null) return null;
		//	return str.RightFromFirst("\n").TrimPlainText();
		//}

		//public static string FirstLine(this string str) {
		//	if (str == null) return null;
		//	return str.LeftUntilAny("\r", "\n", Environment.NewLine);
		//}

		public static string LeftUntilAny(this string str, params string[] findStrings) {
			if (str == null) return null;
			var pos = str.IndexOfAny(findStrings);
			if (pos != -1) {
				return str.Substring(0, pos);
			}
			return null;
		}

		public static int IndexOfAny(this string str, params string[] findStrings) {
			if (str == null) return -1;
			int result = Int32.MaxValue;
			foreach (var findString in findStrings) {
				if (str.IndexOf(findString) > -1) {
					result = Math.Min(result, str.IndexOf(findString));
				}
			}
			if (result == Int32.MaxValue) {
				result = -1;
			}
			return result;
		}

		public static bool EndsWith(this string str, string value, bool ignoreCase) {
			return str.EndsWith(value, ignoreCase, null);
		}
		public static bool StartsWith(this string str, string value, bool ignoreCase) {
			return str.StartsWith(value, ignoreCase, null);
		}

		/// <summary>
		/// Same as Replace() but not case sensitive.
		/// From http://www.codeproject.com/KB/string/fastestcscaseinsstringrep.aspx
		/// </summary>
		/// <param name="str"></param>
		/// <param name="find"></param>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public static string ReplaceInsensitive(this string str, string find, string replacement) {
			var comparisonType = StringComparison.InvariantCultureIgnoreCase;
			int stringBuilderInitialSize = -1;
			if (str == null) { return null; }
			if (String.IsNullOrEmpty(find)) { return str; }
			int posCurrent = 0;
			int lenPattern = find.Length;
			int idxNext = str.IndexOf(find, comparisonType);
			StringBuilder result = new StringBuilder(stringBuilderInitialSize < 0 ? Math.Min(4096, str.Length) : stringBuilderInitialSize);
			while (idxNext >= 0) {
				result.Append(str, posCurrent, idxNext - posCurrent);
				result.Append(replacement);
				posCurrent = idxNext + lenPattern;
				idxNext = str.IndexOf(find, posCurrent, comparisonType);
			}
			result.Append(str, posCurrent, str.Length - posCurrent);
			return result.ToString();
		}

		/// <summary>
		/// Replaces the last occurrence of the specified string oldValue with newValue.
		/// </summary>
		/// <example>"red, white, blue".ReplaceLast(",", " and") returns "red, white and blue"</example>
		/// <param name="str"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		/// <returns></returns>
		public static string ReplaceLast(this string str, string oldValue, string newValue) {
			int pos = str.LastIndexOf(oldValue);
			string result = str;
			if (pos != -1) {
				result = str.Substring(0, pos) + newValue + str.Substring(pos + oldValue.Length);
			}
			return result;
		}

		/// <summary>
		/// Replaces the first occurrence of the specified string oldValue with newValue.
		/// This is not case-sensitive.
		/// </summary>
		/// <example>"deals $99, $55, $88".ReplaceFirst("$", "from $") returns "deals from $99, $55, $88"</example>
		/// <param name="str"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		/// <returns></returns>
		public static string ReplaceFirst(this string str, string oldValue, string newValue) {
			int pos = str.IndexOf(oldValue, StringComparison.InvariantCultureIgnoreCase);
			string result = str;
			if (pos != -1) {
				result = str.Substring(0, pos) + newValue + str.Substring(pos + oldValue.Length);
			}
			return result;
		}

		public static string ReplaceRegex(this string str, string pattern, string replacement) {
			if (str == null) return null;
			return Regex.Replace(str, pattern, replacement);
		}

		public static string ReplaceRegexInsensitive(this string str, string pattern, string replacement) {
			if (str == null) return null;
			return Regex.Replace(str, pattern, replacement, RegexOptions.IgnoreCase);
		}

		public static string ReplaceRegex(this string str, string pattern, string replacement, RegexOptions options) {
			if (str == null) return null;
			return Regex.Replace(str, pattern, replacement, options);
		}

		public static string ReplaceRegex(this string str, string pattern, MatchEvaluator replacementFunction) {
			return ReplaceRegex(str, pattern, replacementFunction, RegexOptions.None);
		}

		public static string ReplaceRegex(this string str, string pattern, MatchEvaluator replacementFunction, RegexOptions options) {
			if (str == null) return null;
			return Regex.Replace(str, pattern, replacementFunction, options);
		}

		/// <summary>
		/// Trims off any whitespace including html whitespace (eg <br>, <p>, &nbsp;)
		/// </summary>
		public static string TrimHtml(this string str) {
			return Fmt.TrimHtmlText(str);
		}

		/// <summary>
		/// Trims off any whitespace and linebreaks from start or end of string
		/// </summary>
		public static string TrimPlainText(this string str) {
			if (str == null) return null;
			return str.Trim().Trim('\r', '\n', '\t', ' ').Trim();
		}

		public static string TrimEnd(this string str, string anyOfTheseChars) {
			return str.TrimEnd(anyOfTheseChars.ToCharArray());
		}

		public static int IndexOfAny(this string str, string anyOfTheseChars) {
			return str.IndexOfAny(anyOfTheseChars.ToCharArray());
		}

		/// <summary>
		/// Returns true if this string contains any of the characters in the anyOfTheseChars string
		/// </summary>
		/// <param name="str"></param>
		/// <param name="anyOfTheseChars"></param>
		/// <returns></returns>
		public static bool ContainsAny(this string str, string anyOfTheseChars) {
			return (str.IndexOfAny(anyOfTheseChars.ToCharArray()) > -1);
		}

		/// <summary>
		/// Returns true if this string contains only characters in the supplied allowedChars string
		/// </summary>
		/// <example>isNumber = str.ContainsOnly("0123456789.-")</example>
		/// <param name="str"></param>
		/// <param name="allowedChars"></param>
		/// <returns></returns>
		public static bool ContainsOnly(this string str, string allowedChars) {
			if (str == null) {
				throw new Exception("string.ContainsOnly() called with a null string");
			}
			foreach (var c in str) {
				if (!allowedChars.Contains(c)) return false;
			}
			return true;
		}

		/// <summary>
		/// A case insensitive version of str.Contains(substring).
		/// If str is null, this returns false. 
		/// This is a shortcut for: (str+"").ToLowerInvariant().Contains(substring)
		/// </summary>
		public static bool ContainsInsensitive(this string str, string substring) {
			return (str + "").ToLowerInvariant().Contains(substring.ToLowerInvariant());
		}

		/// <summary>
		/// For maximum readability use this instead of !str.ToLower().Contains(substring).
		/// This is case-insensitive.
		/// </summary>
		public static bool DoesntContain(this string str, string substring) {
			return !str.ContainsInsensitive(substring);
		}

		/// <summary>
		/// A case insensitive version of mylist.Contains(stringElement).
		/// </summary>
		public static bool ContainsInsensitive(this IEnumerable<string> someList, string someStringElement) {
			return someList.Contains(someStringElement, StringComparer.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// For maximum readability use this instead of !someList.Contains(someStringElement)
		/// </summary>
		public static bool DoesntContain(this IEnumerable<string> someList, string someStringElement) {
			return !someList.ContainsInsensitive(someStringElement);
		}

		/// <summary>
		/// For maximum readability use this instead of !someList.Contains(someElement)
		/// </summary>
		public static bool DoesntContain<T>(this IEnumerable<T> someList, T someElement) {
			return !someList.Contains(someElement);
		}

		/// <summary>
		/// Given a comma delimited string, see if the supplied string is one of the elements after splitting on comma. This avoids partial string matches when using the standard Contains() method when you are trying to check for a match in a comma separated string. String matching is not case sensitive. Also trims any spaces around commas.
		/// </summary>
		public static bool ContainsCommaSeparated(this string commaSeparatedString, string stringToFind) {
			if (commaSeparatedString.IsBlank()) return false;
			stringToFind = stringToFind.ToLower();
			foreach (string element in commaSeparatedString.Split(',')) {
				if (element.Trim().ToLower() == stringToFind) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Given a pipe delimited string, see if the supplied string is one of the elements after splitting on comma. This avoids partial string matches when using the standard Contains() method when you are trying to check for a match in a comma separated string. String matching is not case sensitive. Also trims any spaces around commas.
		/// </summary>
		public static bool ContainsPipeSeparated(this string pipeSeparatedString, string stringToFind) {
			if (stringToFind.IsBlank()) return false;
			stringToFind = stringToFind.ToLower();
			foreach (string element in pipeSeparatedString.Split('|')) {
				if (element.Trim().ToLower() == stringToFind) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Repeats a character the given number of times.
		/// </summary>
		/// <param name="stringToRepeat"></param>
		/// <param name="repeat"></param>
		/// <returns></returns>
		public static string Repeat(this char charToRepeat, int repeat) {
			return new string(charToRepeat, repeat);
		}

		/// <summary>
		/// Repeats a string the given number of times.
		/// </summary>
		/// <param name="stringToRepeat"></param>
		/// <param name="repeat"></param>
		/// <returns></returns>
		public static string Repeat(this string stringToRepeat, int repeat) {
			var builder = new StringBuilder(repeat);
			for (int i = 0; i < repeat; i++) {
				builder.Append(stringToRepeat);
			}
			return builder.ToString();
		}

		public static string ReverseString(this string str) {
			char[] array = str.ToCharArray();
			Array.Reverse(array);
			return new string(array);
		}

		/// <summary>
		/// Returns a copy of the string with the first character made lower case.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string LowerCaseFirstLetter(this string str) {
			return str[0].ToString().ToLower() + str.RemoveCharsFromStart(1);
		}

		/// <summary>
		/// Returns a copy of the string with the first character captialised.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string UpperCaseFirstLetter(this string str) {
			string result = str;
			if (str != null && str.Length > 0) {
				// ignore any punctuation and find first actual letter
				var pos = str.IndexOfAny("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
				if (pos > -1) {
					result = str.Left(pos) + str[pos].ToString().ToUpper() + str.RemoveCharsFromStart(pos + 1);
				}
			}
			return result;
		}

		public static string ToUpperIgnoreNull(this string value) {
			if (value != null) {
				value = value.ToUpper();
			}
			return value;
		}

		public static string ToLowerIgnoreNull(this string value) {
			if (value != null) {
				value = value.ToLower();
			}
			return value;
		}


		/// <summary>
		/// Returns the plural form of a word (eg Bike -> Bikes, Fox -> Foxes)
		/// </summary>
		/// <param name="str">an english word</param>
		/// <returns></returns>
		//public static string Plural(this string str) {
		//    // please add more special cases here as you think of them
		//    if (str.EndsWith("s") || str.EndsWith("x")) {
		//        return str + "es";
		//    } else if (str == "Person") {
		//        return "People";
		//    } else {
		//        return str + "s"; 
		//    } 
		//}

		/// <summary>
		/// Returns plural form of a word (eg "box".Plural() -> "boxes")
		/// </summary>
		/// <param name="word">A string ending in an English word</param>
		/// <returns></returns>
		public static string Plural(this string word) {
			return Fmt.Plural(word);
		}

		/// <summary>
		/// Splits apart a camelCaseName or PascalCaseName into separate words
		/// </summary>
		public static string SplitTitleCase(this string str) {
			return Fmt.SplitTitleCase(str);
		}

		/// <summary>
		/// Capitalises first letter of each word
		/// </summary>
		public static string TitleCase(this string str) {
			return Fmt.TitleCase(str);
		}

		public static string CamelCase(this string str) {
			return Fmt.CamelCase(str);
		}
		public static string PascalCase(this string str) {
			return Fmt.PascalCase(str);
		}
		public static string SentenceCase(this string str) {
			return Fmt.SentenceCase(str);
		}

		/// <summary>
		/// Converts HTML to plain text.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string StripTags(this string value) {
			return Fmt.StripTags(value);
		}

		/// <summary>
		/// Gets HTML ready for display. 
		/// (Fixes relative path issues, adds class=normal, applies Glossarize if available)
		/// </summary>
		/// <seealso cref="HtmlEncode">Use HtmlEncode() or FmtPlainTextAsHtml() instead if your source data is plain text.</seealso>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string FmtHtmlText(this string value) {
			return Fmt.HTMLText(value);
		}

		/// <summary>
		/// Gets plain text ready for HTML display. This is the same as calling Fmt.Text(str)
		/// (Calls HtmlEncode then replaces line breaks with BRs)
		/// </summary>
		/// <seealso cref="HtmlEncode">Use FmtHtmlText() instead if your source data is HTML already.</seealso>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string FmtPlainTextAsHtml(this string value) {
			//return Fmt.Text(value.HtmlEncode()); MN 17-Apr-2010 Fmt.Text now does HTML encoding
			return Fmt.Text(value);
		}

		public static string FmtPlainTextAsHtml(this string value, bool alsoFmtLinks) {
			//return Fmt.Text(value.HtmlEncode()); MN 17-Apr-2010 Fmt.Text now does HTML encoding
			return Fmt.Text(value, alsoFmtLinks);
		}

		/// <summary>
		/// Escapes url special characters (ie pretty much everything except letters, digits and dashes) for passing as a value in a URL string. Each variable in the querystring must be encoded separately.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string UrlEncode(this string str) {
			// DONT use UrlPathEncode - it is WRONG
			str = HttpUtility.UrlEncode(str + "") + "";
			str = str.Replace("+", "%20");       // otherwise it won't work properly in a mailto link
			return str;
		}

		/// <summary>
		/// Makes plain text safe for display in an HTML page.
		/// Escapes html special characters (eg angle brackets, etc) for output as a html body content. 
		/// This should be used inside attribute values as well as in html text content.
		/// (Use FmtHtmlText() instead if your source data is plain text.)
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string HtmlEncode(this string str) {
			if (str == null) return "";
			return HttpUtility.HtmlEncode(str);
		}

		/// <summary>
		/// Escapes javascript special characters (eg quotes, line breaks, etc) for output as a javascript string. Does not include the quotes for the javascript string.
		/// </summary>
		/// <param name="str"></param>
		/// <returns>Encoded string</returns>
		public static string JsEncode(this string str) {
			return Fmt.JsEncode(str);
		}

		/// <summary>
		/// Surrounds with double quotes and escapes javascript special characters (eg quotes, line breaks, etc) for output as a javascript string. Produces a string in double quotes with backslash sequences in all the right places.
		/// </summary>
		/// <param name="s"></param>
		/// <returns>Encoded string surrounded by double quotes</returns>
		public static string JsEnquote(this string s) {
			return Fmt.JsEnquote(s);
		}

		/// <summary>
		/// Converts to Base 64 String
		/// </summary>
		/// <param name="s"></param>
		/// <returns>Encoded string</returns>
		public static string Base64Encode(this string s) {
			var bytes = Encoding.UTF8.GetBytes(s);
			return Convert.ToBase64String(bytes);
		}

		/// <summary>
		/// Converts from Base 64 String
		/// </summary>
		/// <param name="s"></param>
		/// <returns>Decoded string</returns>
		public static string Base64Decode(this string s) {
			var bytes = Convert.FromBase64String(s);
			return Encoding.UTF8.GetString(bytes);
		}


		/// <summary>
		/// Removes all special characters (e.g. accents, non-utf8 chars).
		/// </summary>
		/// <param name="text"></param>
		/// <returns>The clean string</returns>
		public static string RemoveSpecialCharacters(this string text) {
			if (string.IsNullOrEmpty(text))
				return String.Empty;

			var normalizedString = text.Normalize(NormalizationForm.FormD);
			var stringBuilder = new StringBuilder();

			foreach (var c in normalizedString) {
				var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
				if (unicodeCategory != UnicodeCategory.NonSpacingMark) {
					stringBuilder.Append(c);
				}
			}

			return Regex.Replace(stringBuilder.ToString().Normalize(NormalizationForm.FormC), @"[^\u0000-\u007F]", string.Empty);
		}



		//---------------------------------------------------------
		// EXTENSION METHODS TO ADD FEATURES TO DICTIONARY
		//---------------------------------------------------------
		public static string ToUrlString(this Dictionary<string, string> data) {
			string dataString = null;
			if (data != null) {
				foreach (var param in data as Dictionary<string, string>) {
					if (dataString != null) dataString = dataString + "&";
					dataString += param.Key.UrlEncode() + "=" + param.Value.UrlEncode();
				}
			}
			return dataString;
		}


		//---------------------------------------------------------
		// EXTENSION METHODS TO ADD FEATURES TO OBJECT
		//---------------------------------------------------------

		public static string ToStringIgnoreNull(this object value) {
			if (value != null) {
				return value.ToString();
			}
			return null;
		}

		public static string JsonStringify(this object value) {
			if (value == null) {
				return "null";
			} else if (value is string || value is char) {
				return value.ToString().JsEnquote();
			} else if (value is DateTime) {
				return "\"" + ((DateTime)value).ToUniversalTime().ToString("s") + "Z\"";
			} else if (value.GetType().IsEnum) {
				return value.ToString().JsEnquote();
			} else if (value is ValueType) {
				return value.ToString().ToLowerInvariant();
			} else if (value is IDictionary) {
				var result = new StringBuilder();
				result.Append("{");
				var dic = value as IDictionary;
				foreach (var key in dic.Keys) {
					if (result.Length > 1) result.Append(",");
					result.Append("\"").Append(key).Append("\":");
					result.Append(dic[key].JsonStringify());
				}
				result.Append("}");
				return result.ToString();
			} else if (value is IEnumerable) {
				// stringify each element
				var result = new StringBuilder();
				result.Append("[");
				foreach (var i in value as IEnumerable) {
					if (result.Length > 1) result.Append(",");
					result.Append(i.JsonStringify());
				}
				result.Append("]");
				return result.ToString();
			}
			return value.ToString().JsEnquote();
		}

		/// <summary>
		/// Given the property name as a string, call the property getter and return the value of the property.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public static object GetPropertyValue(this object obj, string propertyName) {
			return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
		}

		/// <summary>
		/// Given the property name as a string, call the property setter to set the value of the property.
		/// If you want to update an ActiveRecord field value from a string, use myrecord["MyFieldName"].FromString(mystring) instead.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="propertyName"></param>
		/// <param name="newValue"></param>
		/// <returns>false if property not found to update</returns>
		public static bool SetPropertyValue(this object obj, string propertyName, object newValue) {
			var prop = obj.GetType().GetProperty(propertyName);
			try {
				if (prop != null) prop.SetValue(obj, newValue, null);
			} catch (Exception) { }//dont care!
			return prop != null;
		}

		/// <summary>
		/// like update from request, but reads the source object (instead of request) and updates 'this' (obj) with
		/// any matching properties
		/// </summary>
		/// <param name="obj">this</param>
		/// <param name="source">object to find matching properties in.</param>
		/// <returns>number of matches - throw an error if you get zero maybe?</returns>
		public static int UpdateFromObject(this object obj, object source) {
			return obj.UpdateFromObject(source, null);
		}
		//public static int UpdateFromDataRow(this object obj, DataRow source) {
		//  for (int col = 0; col < obj.Columns.Count; col++) {
		//    var colName = obj.Columns[col].ColumnName;
		//    if (this.FieldExists(colName)) {
		//      rateCalcLine[colName].ValueObject = source[colName];
		//    }
		//  }
		//  return -1;
		//}
		public static int UpdateFromObject(this object obj, object source, string excludelist) {
			PropertyInfo[] props = null;
			try {
				props = source.GetType().GetProperties();
			} catch (Exception) {
				//no props?
			}
			excludelist = "," + excludelist + ",";
			int numItems = 0;
			if (props != null) {
				foreach (var prop in props) //walk the source
				{
					string attribName = prop.Name;
					//store the source value
					object attribValue = null;
					try {
						attribValue = prop.GetValue(source, null);
					} catch (Exception) { }

					PropertyInfo destProp = null;
					try {
						destProp = obj.GetType().GetProperty(attribName);
					} catch (Exception) { }
					if (destProp != null)//see if the propertyname exists on this (obj)
					{
						bool skipItem = false;
						if (excludelist != null && excludelist.ToLower().Contains("," + attribName.ToLower() + ",")) skipItem = true;
						//var destVal = destProp.GetValue(obj,null);
						//check for 'crazy null date' from netsuite!
						if (attribValue != null && destProp.ToString().Contains("System.DateTime") && Fmt.DateTimeCompressed(attribValue.ToString()) == "000101010000") skipItem = true; // ="1/01/0001 12:00:00 a.m."
						if (!skipItem) {
							//overwrite the value
							obj.SetPropertyValue(attribName, attribValue);
							numItems++;
						}
					} else {
						//prop doesnt exist in obj (this)
					}
				}
			}
			return numItems;
		}

		/*
		Create the enum
		public enum ProductTypes {
			[Description("1 Country Passes")]
			OneCountry,
			
			[Description("2 Countries Passes")]
			TwoCountry
		}
		
		Usage: ProductTypes.OneCountry.GetDescription();
		*/
		public static string GetDescription(this Enum value) {
			FieldInfo fi = value.GetType().GetField(value.ToString());
			var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (attributes.Length > 0) {
				return attributes[0].Description;
			}
			return value.ToString();
		}

		///// <summary>
		///// Copy values of request variables to matching properties of this object
		///// Takes values from submitted form fields or querystring params which have the same names as properties in the objects
		///// </summary>
		///// <param name="value"></param>
		//public static void UpdateFromRequest(this object value)
		//{
		//  UpdateFrom(value, HttpContext.Current.Request, "", "");
		//}



		//public static void UpdateFromRequest(this object value, string objectPrefix, string objectSuffix)     // extension method new 3.5
		//{
		//  UpdateFrom(value, HttpContext.Current.Request, objectPrefix, objectSuffix);
		//}

		////public static void UpdateFrom(object value, NameValueCollection values, string objectSuffix) {
		//public static void UpdateFrom(this object value, HttpRequest values, string objectPrefix, string objectSuffix) {
		//  Type objType = value.GetType();
		//  string objName = objType.Name;

		//  // TODO: Use ComponentModel instead of Reflection to get/set the properties
		//  PropertyInfo[] fields = objType.GetProperties();

		//  //						 PopulateTypeException ex = null;
		//  Exception ex = null;

		//  foreach (PropertyInfo property in fields) {
		//    //check the key
		//    //going to be forgiving here, allowing for full declaration 13                   //or just propname
		//    string httpKey = objectPrefix + property.Name + objectSuffix;

		//    //if (!String.IsNullOrEmpty(objectSuffix))
		//    //	httpKey = httpKey + objectSuffix;

		//    if (values[httpKey] == null) {
		//      httpKey = objName + "." + property.Name;
		//    }

		//    if (values[httpKey] == null)
		//    {
		//      httpKey = objName + "_" + property.Name;
		//    }

		//    if (values[httpKey] == null)
		//    {
		//      httpKey = objName + "__" + property.Name;
		//    }

		//    if (values[httpKey] != null) {
		//      TypeConverter conv = TypeDescriptor.GetConverter(property.PropertyType);
		//      object thisValue = values[httpKey];

		//			if(property.PropertyType.FullName=="System.Boolean")
		//			{
		//      if (thisValue + "" == "on") thisValue = "True";
		//				if (thisValue + "" == "1") thisValue = "True";
		//				if (thisValue + "" == "0") thisValue = "False";
		//				if (thisValue + "" == "yes") thisValue = "True";
		//			}
		//      if (conv.CanConvertFrom(typeof (string))) {
		//        try {
		//          thisValue = conv.ConvertFrom(thisValue);
		//          property.SetValue(value, thisValue, null);

		//        } catch (Exception e) {
		//          // MN changed from: catch (FormatException e) {
		//          string message = "updatefrom:" + property.Name + " is not a valid " + property.PropertyType.Name + "; " +
		//                           e.Message;
		//          if (ex == null)
		//            throw new Exception(message);
		//          //    ex = new PopulateTypeException("Errors occurred during object binding - review the LoadExceptions property of this exception for more details");

		//          //ExceptionInfo info = new ExceptionInfo();
		//          //info.AttemptedValue = thisValue;
		//          //info.PropertyName = property.Name;
		//          //info.ErrorMessage = message;

		//          //ex.LoadExceptions.Add(info);
		//        }
		//      } else {
		//        // TODO: Why do we throw an exception here instead of setting "ex"?
		//        throw new FormatException("No type converter available for type: " + property.PropertyType);
		//      }
		//    //} else if (values.Files[objectPrefix + property.Name + objectSuffix].ContentLength > 0) {
		//    //  // found a file upload
		//    //  var file = values.Files[objectPrefix + property.Name + objectSuffix];
		//    //  // set file name
		//    //  property.SetValue(value, file.FileName, null);
		//    }
		//  }
		//  // TODO: Why does this code only throw the last exception that happened? Typically the first exception is the most important one.
		//  if (ex != null)
		//    throw ex;
		//}

	}


	[Obsolete("Deprecated")]
	public class DeprecatedAttribute : System.Attribute { }

	[Obsolete("Experimental - try out but use with caution - feedback to Mike or whoever wrote this code")]
	public class ExperimentalAttribute : System.Attribute { }

	[Obsolete("Incomplete feature - do not use or use with caution")]
	public class IncompleteAttribute : System.Attribute { }

	public class ExtractTextException : BewebException {
		public ExtractTextException(string message) : base(message) { }
		public ExtractTextException(string message, Exception innerException) : base(message, innerException) { }
	}


	public static class LoopHelper {
		public static int Index() {
			return (int)HttpContext.Current.Items["LoopHelper_Index"];
		}
	}

	/// <summary>
	/// Example usage:
	/// 	<%foreach (var x in myArrayOrList.WithIndex()){ %>
	///			<%=LoopHelper.Index() %> = <%=x %><br>
	///		<%} %>
	/// </summary>
	public static class LoopHelperExtensions {
		public static IEnumerable<T> WithIndex<T>(this IEnumerable<T> that) {
			return new EnumerableWithIndex<T>(that);
		}

		public class EnumerableWithIndex<T> : IEnumerable<T> {
			public IEnumerable<T> Enumerable;

			public EnumerableWithIndex(IEnumerable<T> enumerable) {
				Enumerable = enumerable;
			}

			public IEnumerator<T> GetEnumerator() {
				for (int i = 0; i < Enumerable.Count(); i++) {
					HttpContext.Current.Items["LoopHelper_Index"] = i;
					yield return Enumerable.ElementAt(i);
				}
			}

			IEnumerator IEnumerable.GetEnumerator() {
				return GetEnumerator();
			}
		}
	}
}

#if TestExtensions
namespace BewebTest {
	[TestClass]
	public class TestBaseTypeExtensions {
		public class testobj {
			protected string _FieldName;
			public string FieldName {
				get { return _FieldName; }
				set { _FieldName = value; }
			}


		}

		[Beweb.TestMethod]
		public static void TestEndsWith() {
			Assert.AreEqual(true, "MikE".EndsWith("e", true));
			Assert.AreEqual(true, "MikE".EndsWith("E", true));
			Assert.AreEqual(false, "MikE".EndsWith("e", false));
			Assert.AreEqual(true, "Mike".EndsWith("e", false));
			Assert.AreEqual(true, "Mike".EndsWith("e", true));
			Assert.AreEqual(false, "Mike".EndsWith("E", false));
			Assert.AreEqual(true, "Mike".EndsWith("e", false));
		}
		[Beweb.TestMethod]
		public static void TestStartsWith() {
			Assert.AreEqual(true, "MikE".StartsWith("m", true));
			Assert.AreEqual(true, "MikE".StartsWith("M", true));
			Assert.AreEqual(false, "MikE".StartsWith("m", false));
			Assert.AreEqual(true, "mike".StartsWith("m", false));
			Assert.AreEqual(true, "mike".StartsWith("m", true));
			Assert.AreEqual(false, "mike".StartsWith("M", false));
			Assert.AreEqual(true, "mike".StartsWith("m", false));
		}


		[TestMethod]
		public static void TestReplaceLast() {
			var expectedValue = "red, white and blue";
			var actualValue = "red, white, blue".ReplaceLast(",", " and");
			Assert.AreEqual(expectedValue, actualValue);

			Assert.AreEqual("this is a test", "this is a test".ReplaceLast(",", "and"));
		}

		[TestMethod]
		public static void TestReplaceFirst() {
			var expectedValue = "red, white and blue";
			var actualValue = "red and white and blue".ReplaceFirst(" and", ",");
			Assert.AreEqual(expectedValue, actualValue);

			expectedValue = "deals from $99, $55, $88";
			actualValue = "deals $99, $55, $88".ReplaceFirst("$", "from $");
			Assert.AreEqual(expectedValue, actualValue);

			Assert.AreEqual("this is a test", "this is a test".ReplaceFirst(",", "and"));
		}

		[TestMethod]
		public static void TestToIntOrDie() {

			Assert.AreEqual(true, "35000.00".ToIntOrDie() == 35000);
			Assert.AreEqual(true, "35000".ToIntOrDie() == 35000);
			Assert.AreEqual(true, "35000.002".ToInt() == 35000);
			Assert.AreEqual(true, "35000".ToInt() == 35000);
		}

		[TestMethod]
		public static void TestUpperCaseFirstLetter() {

			Assert.AreEqual(true, "mike".UpperCaseFirstLetter() == "Mike");
			Assert.AreEqual(true, "".UpperCaseFirstLetter() == "");
		}


		[TestMethod]
		public static void TestRightFrom() {
			Assert.AreEqual("man", "mike.is.the.man".RightFrom("."));
			Assert.AreEqual(".man", "mike.is.the.man".RightFrom("he"));
			Assert.AreEqual("mike.is.the.man", "mike.is.the.man".RightFrom("q"));
		}

		[TestMethod]
		public void TestUrlEncode() {
			Assert.AreEqual("mike%20is%26.the%25%3d%20man", "mike is&.the%= man".UrlEncode());
			Assert.AreEqual("charles%20lee", "charles lee".UrlEncode());
		}


		//[TestMethod]
		//public void RemoveFirstLine() {
		//	Assert.AreEqual("lee", "charles \rlee".RemoveFirstLine());
		//	Assert.AreEqual("lee\r", "charles \rlee\r".RemoveFirstLine());
		//	Assert.AreEqual("lee", "charles \nlee".RemoveFirstLine());
		//	Assert.AreEqual("lee", "charles " + Environment.NewLine + "lee".RemoveFirstLine());
		//}

		//[TestMethod]
		//public void FirstLine() {
		//	Assert.AreEqual("charles ", "charles \nlee".FirstLine());
		//	Assert.AreEqual("charles ", "charles " + Environment.NewLine + "lee".FirstLine());
		//	Assert.AreEqual("", "\ncharles ".FirstLine());
		//}

		[TestMethod]
		public void LeftUntilAny() {
			Assert.AreEqual("char", "charles lee".LeftUntilAny("les", "ee"));
		}

		[TestMethod]
		public void IndexOfAny() {
			Assert.AreEqual(4, "charles lee".IndexOfAny("les", "ee"));
		}


		[TestMethod]
		public void TestConvertToDate() {
			Assert.AreEqual(null, "mike is&.the%= man".ConvertToDate(null));
			Assert.AreEqual(null, "".ConvertToDate(null));
			Assert.AreEqual(DateTime.Today, "mike is&.the%= man".ConvertToDate(DateTime.Today));
			Assert.AreEqual(DateTime.Today, "".ConvertToDate(DateTime.Today));
			Assert.AreEqual(new DateTime(1973, 1, 15), "15 Jan 1973".ConvertToDate(DateTime.Today));
			Assert.AreEqual(new DateTime(1973, 1, 15), "15 Jan 1973".ConvertToDate(null));
			Assert.AreEqual(new DateTime(1973, 1, 15), "15 Jan 1973".ConvertToDate(null));
			Assert.AreEqual(new DateTime(2013, 10, 10, 8, 30, 0), "2013-10-10 08:30".ConvertToDate(null));
			Assert.AreEqual(new DateTime(2013, 10, 10, 8, 30, 45, 500), "2013-10-10 08:30:45.500".ConvertToDate(null));
			Assert.AreEqual(new DateTime(2013, 10, 10, 8, 30, 0), "2013-10-10 08.30".ConvertToDate(null));
			Assert.AreEqual(new DateTime(2013, 10, 10, 8, 30, 45), "2013-10-10 08.30.45".ConvertToDate(DateTime.Today));
			//Assert.AreEqual(new DateTime(2013,10,7,9,0,0),"2013-10-07 0900".ConvertToDate(DateTime.Today));
			//Assert.AreEqual(new DateTime(2013,10,10,8,30,45,500),"2013-10-10 08.30.45.500".ConvertToDate(DateTime.Today));
		}
	}
}
#endif

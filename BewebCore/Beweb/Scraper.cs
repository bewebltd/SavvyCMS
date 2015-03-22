using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Beweb;

namespace Beweb {
	/// <summary>
	/// Helper methods for HTML screen scraping, text extraction, web services, email
	/// </summary>
	public static class Scraper {

		public static DateTime? GetDateFromJson(string json, string property) {
			return GetStringFromJson(json, property).Replace(@"\/", "/").ConvertToDate(null);
		}
		public static string GetDateStringFromJson(string json, string property) {
			return GetStringFromJson(json, property).Replace(@"\/", "/");
		}

		public static string GetStringFromJson(string json, string property) {
			return json.ExtractTextBetween("\"" + property + "\":", ",").RemovePrefix("\"").RemoveSuffix("\"").Replace("&nbsp;", " ");
		}

		public static string ExtractNextTableCell(string html, string label) {
			return html.ExtractTextBetween(label, "/tr").ExtractTextBetween("<td", "/td>").ExtractTextBetween(">", "<").Replace("&nbsp;", " ").Trim();
		}

		public static string ExtractTableCellBelow(string html, string label) {
			html = html.Replace("<TABLE", "<table");
			html = html.Replace("</TABLE", "</table");
			html = html.Replace("<TR", "<tr");
			html = html.Replace("</TR", "</tr");
			html = html.Replace("<TD", "<td");
			html = html.Replace("</TD", "</td");
			html = "<table" + html.RightFromFirst("<table");
			int startOfTable = html.LeftUntil(label).LastIndexOf("<table");
			if (startOfTable == -1) {
				throw new ProgrammingErrorException("ExtractTableCellBelow must be given HTML including TABLE tag.\nFirst part of HTML is [" + html.Left(50) + "]");
			}
			html = html.Substring(startOfTable);
			html = html.LeftUntil("/table>");
			var rows = html.Split("<tr");
			int foundColIndex = 0;
			foreach (var row in rows) {
				if (foundColIndex > 0) {
					var cols = row.Split("<td");
					if (cols.Count() > foundColIndex) {
						var result = cols[foundColIndex].ExtractTextBetween(">", "</td>").Replace("&nbsp;", " ").Trim();
						return result;
					}
				} else if (row.Contains(label)) {
					var cols = row.Split("<td");
					int colIndex = 0;
					foreach (var col in cols) {
						if (col.Contains(label)) {
							foundColIndex = colIndex;
						}
						colIndex++;
					}
				}
			}
			return null;
		}

		public static MatchCollection ExtractEmailAddresses(string text) {
			var m = Regex.Matches(text.ToLower(), Fmt.MatchEmailPattern);
			return m;
		}

		public static List<NameAndEmailAddress> ExtractNameAndEmailAddresses(string text) {
			var m = Regex.Matches(text, @"""?(?<firstname>\w+) (?<lastname>\w+)""? <(?<email>[\w\+\-\._]+@[\w\+\-\._]+)>");
			var result = new List<NameAndEmailAddress>();
			foreach (Match match in m) {
				if (result.Exists(e => e.Email == match.Groups["email"].ToString())) {
					result.AddUnique(new NameAndEmailAddress() { FirstName = match.Groups["firstname"].ToString(), LastName = match.Groups["lastname"].ToString(), Email = match.Groups["email"].ToString(), Position = match.Index, Length = match.Length });
				}
			}
			return result;
		}

		public class NameAndEmailAddress {
			public string FirstName;
			public string LastName;
			public string Email;
			public int Position;
			public int Length;
		}

		public static string ExtractEmailAddress(string text) {
			//Regex rx = new Regex(Fmt.MatchEmailPattern, RegexOptions.IgnoreCase);
			// Find matches.
			var m = Regex.Match(text.ToLower(), Fmt.MatchEmailPattern).ToString();
			// Report on each match.
			return m;
		}


	}

	namespace BewebTest {
		public class ScraperTest {
			[TestMethod]
			public static void TestExtractEmailAddresses() {
				var result = Scraper.ExtractEmailAddresses("miuke@beweb.co.nz,jeremy@beweb.co.nz 34 j3f awonmcokawmc982 hj2d,kwk  jim@mytruckshop.trucks");
				Assert.AreEqual("miuke@beweb.co.nz", result.ToString());
				result = Scraper.ExtractEmailAddresses("miukfds899  s::e.@beweb.co.nz,jeremy@@beweb.co.nz 34 j3f awonmcokawmc982 hj2d,kwk  jim@mytruckshop.trucks");
				Assert.AreEqual("jim@mytruckshop.trucks", result.ToString());
			}



		}
	}
}

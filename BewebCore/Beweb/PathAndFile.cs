#define PathAndFile
#define TestExtensions
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;
using Beweb;

namespace Beweb {

	/// <summary>
	/// Summary description for PathAndFile
	/// </summary>
	public class PathAndFile {
		#region RawUrl
		public static string RawUrl() {
			return RawUrl(false);
		}
		public static string RawUrl(bool includeQueryString) {
			string returnValue = HttpContext.Current.Request.RawUrl;
			// remove the host
			string appPath = HttpContext.Current.Request.ApplicationPath;
			if (appPath != "/" && returnValue.StartsWith(appPath)) {
				returnValue = returnValue.Substring(appPath.Length);
			}
			// add a tilde
			returnValue = "~" + returnValue;
			// add the querystring
			if (includeQueryString) returnValue += HttpContext.Current.Request.Url.Query;

			return returnValue;
		}
		#endregion

		#region InsertSuffix
		/// <summary>
		/// Inserts suffix such as "_tn" into the filename for getting a thumbnail etc.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="suffix">suffix such as "_tn"</param>
		/// <returns></returns>
		public static string InsertSuffix(string filename, string suffix) {
			string returnValue = "";

			if (filename.LastIndexOf(".") > 0) {
				returnValue = filename.Insert(filename.LastIndexOf("."), suffix + "");
			}

			return returnValue;
		}
		#endregion

		#region FileSize
		/// <summary>
		/// given a file size in bytes, return a formatted version
		/// MinimumDigits is if the whole portion of the result, is less than MinimumDigits, we will start using decimal places
		/// </summary>
		/// <param name="FileSizeBytes"></param>
		/// <param name="MinimumDigits"></param>
		/// <returns></returns>
		public static string FileSize(string FileSizeBytes, int MinimumDigits) {
			double CalculatedSize = 0;
			string result = "";
			string SizeName = "";

			// in the checks the last divisor is always 1000 - we don't want 1000 KB - make it 0.9 MB
			// the actual calculator always uses 1024 though
			if (Convert.ToDouble(FileSizeBytes) / 1024 / 1024 / 1024 / 1000 >= 1) {
				// tera bytes
				CalculatedSize = Math.Round((double)(Convert.ToDouble(FileSizeBytes) / 1024 / 1024 / 1024 / 1024), MinimumDigits);
				SizeName = "TB";
			} else if (Convert.ToDouble(FileSizeBytes) / 1024 / 1024 / 1000 >= 1) {
				// giga bytes
				CalculatedSize = Math.Round((double)(Convert.ToDouble(FileSizeBytes) / 1024 / 1024 / 1024), MinimumDigits);
				SizeName = "GB";
			} else if (Convert.ToDouble(FileSizeBytes) / 1024 / 1000 >= 1) {
				// mega bytes
				CalculatedSize = Math.Round((double)(Convert.ToDouble(FileSizeBytes) / 1024 / 1024), MinimumDigits);
				SizeName = "MB";
			} else if (Convert.ToDouble(FileSizeBytes) / 1000 >= 1) {
				// kilo bytes
				CalculatedSize = Math.Round((double)(Convert.ToDouble(FileSizeBytes) / 1024), MinimumDigits);
				SizeName = "KB";
			} else {
				// bytes
				CalculatedSize = Math.Round((double)Convert.ToDouble(FileSizeBytes), MinimumDigits);
				SizeName = "B";
			}
			result = CalculatedSize.ToString();

			// make sure there is a decimal point
			if (result.IndexOf(".") > 0) {
				// remove numbers after decimal point - but how many?
				string WholePortion;
				WholePortion = result.Substring(0, result.IndexOf("."));
				//TODO: round these numbers rather than just chopping them off?
				if (WholePortion.Length < MinimumDigits) {
					if (result.Length < MinimumDigits + 1) {
						char zeroes = '0';
						result = result.PadRight(MinimumDigits + 1, zeroes);
					}

					// leave a certain number of decimal places
					result = result.Substring(0, MinimumDigits + 1);
				} else {
					// we have enough characters - show a whole number
					// chop off the .## on the end
					result = result.Substring(0, result.Length - MinimumDigits - 1); // extra -1 is to account for the decimal point
				}
			}

			result = string.Format("{0} {1}", result, SizeName);

			return result;
		}
		#endregion

		#region FolderSize
		/// <summary>
		/// returns the size of a directory or folder
		/// </summary>
		/// <param name="directoryPath"></param>
		/// <returns></returns>
		public static long FolderSize(string directoryPath) {
			return FolderSize(new DirectoryInfo(directoryPath));
		}
		/// <summary>
		/// returns the size of a directory or folder
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public static long FolderSize(DirectoryInfo d) {
			long Size = 0;
			// Add file sizes.
			FileInfo[] fis = d.GetFiles();
			foreach (FileInfo fi in fis) {
				Size += fi.Length;
			}
			// Add subdirectory sizes.
			DirectoryInfo[] dis = d.GetDirectories();
			foreach (DirectoryInfo di in dis) {
				Size += FolderSize(di);
			}
			return (Size);
		}
		#endregion

		#region CrunchPageName
		/// <summary>
		/// Turns a title string into one usable as a URL (ie lower case separated with dashes). Includes ".aspx" on the end.
		/// </summary>
		/// <param name="crunchVal"></param>
		/// <returns></returns>
		public static string CrunchPageName(string crunchVal) {
			string returnValue = "";
			if (!String.IsNullOrEmpty(crunchVal)) {
				returnValue = CrunchFileName(crunchVal);
				returnValue = String.Format("{0}.aspx", returnValue);
			}
			return returnValue;
		}

		/// <summary>
		/// Turns a string such as a title into one usable as a file name or URL (ie lower case separated with dashes).
		/// </summary>
		/// <param name="titleText"></param>
		/// <returns></returns>
		public static string CrunchFileName(string titleText) {
			//string returnValue = "";
			//if (!String.IsNullOrEmpty(titleText)) {
			//	returnValue = titleText.ToLower().Trim();
			//	returnValue = returnValue.Replace("&", "and");
			//	returnValue = returnValue.Replace(".", "-");
			//	returnValue = returnValue.Replace(" ", "-");
			//	returnValue = returnValue.Replace("_", "-");
			//	//returnValue = Fmt.CleanString(returnValue, "[^a-z0-9-]");
			//	returnValue = Fmt.CleanAlphaNumeric(returnValue);
			//	while (returnValue.Contains("--")) returnValue = returnValue.Replace("--", "-");
			//	returnValue = returnValue.TrimEnd('-');
			//}
			//return returnValue;
			return Fmt.Crunch(titleText);
		}
		#endregion

		#region GetExtension
		/// <summary>
		/// return the file extension without the .
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static string GetExtension(string filename) {
			if (filename.Contains(".")) {
				return filename.RightFrom(".");
			}
			throw new Exception("PathAndFile.GetExtension: the filename did not contain an extension [" + filename + "]");
		}

		public static string GetExtension(string filename, string defaultValue) {
			if (filename.Contains(".")) {
				return filename.RightFrom(".");
			}
			return defaultValue;
		}
		#endregion

	}
}

#if TestExtensions
namespace BewebTest {
	/// <summary>
	///This is a test class for PathAndFileTest and is intended
	///to contain all PathAndFileTest Unit Tests
	///</summary>
	[TestClass()]
	public class PathAndFileTest {
		[TestMethod()]
		public void TestCrunchFileName() {
			var sw = new Stopwatch();
			sw.Start();
			for (var i=0;i<9999;i++){
				var s = PathAndFile.CrunchFileName("this is agreat TitleFormat!");
			}
			sw.Stop();
			Web.Write("Execution time: "+sw.ElapsedMilliseconds + "ms");
			
		}


	}
}
#endif

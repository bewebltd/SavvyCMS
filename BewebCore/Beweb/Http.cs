using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Beweb;

namespace Beweb {
	public class Http {
		public static Encoding DefaultEncoding = Encoding.ASCII;  // keep ASCII for backwards compatability - this is overridden to UTF8 in BewebCoreSettings.cs for projects going forward

		public class HttpVerbConst : StringConst {
			public HttpVerbConst(string str) : base(str) {
			}
		}

		public static HttpVerbConst GET = new HttpVerbConst("GET");
		public static HttpVerbConst POST = new HttpVerbConst("POST");
		public static HttpVerbConst DELETE = new HttpVerbConst("DELETE");
		public static HttpVerbConst PUT = new HttpVerbConst("PUT");

		public static string Get(string url) {
			return Get(url, Util.GetSettingInt("GetRequestsDefaultTimeout", 60));
		}

		/// <summary>
		/// Timeout in seconds
		/// </summary>
		/// <param name="url"></param>
		/// <param name="timeout"></param>
		/// <returns></returns>
		public static string Get(string url, int timeout) {
			// used to build entire input
			StringBuilder sb = new StringBuilder();

			// used on each read operation
			byte[] buf = new byte[8192];

			HttpWebRequest webReq;

			try {
				webReq = WebRequest.Create(url) as HttpWebRequest;
			} catch (UriFormatException exception) {
				throw new ProgrammingErrorException("Incorrect URL format, you must use a absolute full URL. URL is: " + url, exception);
			}

			// Fake Chrome 
			webReq.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.117 Safari/537.36";

			webReq.Timeout = timeout * 1000; // Convert to miliseconds

			HttpWebResponse response = null;
			// execute the request
			try {
				response = (HttpWebResponse)webReq.GetResponse();
			} catch (Exception e) {
				throw new ProgrammingErrorException("Failed to get url[" + url + "]: " + e.Message);
			}
			if (response != null) {
				// we will read data via the response stream
				Stream resStream = response.GetResponseStream();

				string tempString = null;
				int count = 0;

				do {
					// fill the buffer with data
					count = resStream.Read(buf, 0, buf.Length);

					// make sure we read some data
					if (count != 0) {
						// translate from bytes to ASCII text
						tempString = DefaultEncoding.GetString(buf, 0, count);

						// continue building the string
						sb.Append(tempString);
					}
				}
				while (count > 0); // any more data to read?
			}
			return sb.ToString();
		}

		/// <summary>
		/// Call a URL with POST method. 
		/// The data can be URL encoded or JSON or XML.
		/// We auto detect and send correct content-type header eg
		/// eg containerText=NYKU3012540&isImport=true will go as application/x-www-form-urlencoded
		/// or {"containerText":"NYKU3012540","isImport":true} will go as application/json
		/// Returns a string.
		/// </summary>
		public static string Post(string url, string data) {
			return Post(url, data, null);
		}

		public static string Post(string url, string data, string headerName, string headerData) {
			var coll = new WebHeaderCollection();
			coll.Add(headerName, headerData);
			return Post(url, data, coll);
		}

		public static string Post(string url, string data, WebHeaderCollection headers) {
			return Request(Http.POST, url, data, headers);
		}

		public static string Request(HttpVerbConst method, string url, Dictionary<string, string> data, WebHeaderCollection headers) {
			string str = "";
			if (data == null) {
				str = null;
			} else {
				foreach (var param in data) {
					if (str != "") str = str + "&";
					str += param.Key.UrlEncode() + "=" + param.Value.UrlEncode();
				}
			}
			return Request(method, url, str, headers);
		}

		/// <summary>
		/// Call a URL with any method (eg GET, PUT, POST, DELETE). 
		/// Pass in any extra headers you want. 
		/// To set UserAgent and Accept headers, set them in the headers collection and then we automatically pull these off and put them in the correct places.
		/// The data can be URL encoded or JSON or XML.
		/// We auto detect and send correct content-type header eg
		/// eg containerText=NYKU3012540&isImport=true will go as application/x-www-form-urlencoded
		/// or {"containerText":"NYKU3012540","isImport":true} will go as application/json
		/// Returns a string.
		/// </summary>
		public static string Request(HttpVerbConst method, string url, object data, WebHeaderCollection headers) {
			var ErrorFormat = "throw";

			string dataString = null;
			if (data != null) {
				if (data is string) {
					dataString = (string)data;
				} else if (data is Dictionary<string, string>) {
					foreach (var param in data as Dictionary<string, string>) {
						if (dataString != null) dataString = dataString + "&";
						dataString += param.Key.UrlEncode() + "=" + param.Value.UrlEncode();
					}
				} else {
					throw new ProgrammingErrorException("Http.Request - Invalid data type. Use a string or a dictionary of strings.");
				}
			}

			if (method == Http.GET && dataString != null) {
				if (url.Contains("?")) {
					url += "&" + dataString;
				} else {
					url += "?" + dataString;
				}
				dataString = null;
			}

			HttpWebRequest webReq;
			try {
				webReq = WebRequest.Create(url) as HttpWebRequest;
			} catch (UriFormatException exception) {
				throw new ProgrammingErrorException("Incorrect URL format, you must use a absolute full URL. URL is: " + url, exception);
			}

			if (Util.GetSettingBool("SendRemoteRequestsViaFiddler", false)) {
				// ignore ssl cert errors - important for fiddler to capture https traffic
				webReq.Proxy = new WebProxy("http://127.0.0.1:8888", false);
				ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
			}

			if (headers != null) {
				// To set UserAgent and Accept headers, set them in the headers collection and then we automatically pull these off and put them in the correct places
				// there could be other headers like this, so if you see it saying you can't set it, use this same approach
				if (headers.AllKeys.Contains("Accept")) {
					var accept = headers["Accept"];
					headers.Remove("Accept");
					webReq.Accept = accept;
				}
				if (headers.AllKeys.Contains("User-Agent")) {
					var userAgent = headers["User-Agent"];
					headers.Remove("User-Agent");
					webReq.UserAgent = userAgent;
				}
				if (headers.AllKeys.Contains("Referer")) {  //20140611 jn added
					var referer = headers["Referer"];
					headers.Remove("Referer");
					webReq.Referer = referer;
				}
				if (headers.AllKeys.Contains("Connection")) { //20140611 jn added
					var connection = headers["Connection"];
					headers.Remove("Connection");
					webReq.Connection = connection;
				}
				if (headers.AllKeys.Contains("Content-Type")) {  //20140611 jn added
					var contentType = headers["Content-Type"];
					headers.Remove("Content-Type");
					webReq.ContentType = contentType;
				}
				foreach (var header in headers.AllKeys) {
					try {
						webReq.Headers[header] = headers[header]; //20140611 jn put in catch
					} catch (ArgumentException exception) {
						throw new ProgrammingErrorException("header failed[" + header + "]", exception);
					}

				}
				//webReq.Headers = headers;
				headers.Remove("Host");
			}

			webReq.Method = method.ToString().ToUpper();

			if (dataString != null) {  // data will be null for a GET, otherwise should not be null
				byte[] reqBytes = null;
				reqBytes = DefaultEncoding.GetBytes(dataString);
				if (webReq.ContentType == null) {							 //leave this if already set by headers collection
					if (dataString != null && dataString.StartsWith("{")) {
						webReq.ContentType = "application/json; charset=utf-8";
					} else if (dataString != null && dataString.StartsWith("<?xml")) {
						webReq.ContentType = "application/xml; charset=utf-8";
					} else {
						webReq.ContentType = "application/x-www-form-urlencoded";
					}
				}
				webReq.ContentLength = reqBytes.Length;
				try {
					using (Stream requestStream = webReq.GetRequestStream()) {
						requestStream.Write(reqBytes, 0, reqBytes.Length);
						requestStream.Close();
					}
				} catch (Exception e) {
					if (e.InnerException != null && e.InnerException.Message == "No connection could be made because the target machine actively refused it 127.0.0.1:8888") {
						throw new ProgrammingErrorException("Trying to proxy via Fiddler but Fiddler is not running. Use appsetting SendRemoteRequestsViaFiddlerDEV to turn on or off sending to Fiddler proxy.", e);
					} else {
						throw;
					}
				}
			}
			string result = null;
			HttpWebResponse webResponse = null;
			//try {
			webResponse = (HttpWebResponse)webReq.GetResponse();
			// AF20140909: Removed notify because we want to handle Http Status codes in the calling method
			// MN breaking change but should be fine as it should fall through to notify anyway
			//} catch (Exception e) {
			//	// try and replace the URL if we find it - if not, just return the user's Error String
			//	Error.Notify(false, "", e);
			//}

			if (webResponse != null) {
			using (Stream responseStream = webResponse.GetResponseStream()) {
				if (responseStream != null) {
					using (StreamReader streamReader = new StreamReader(responseStream, DefaultEncoding)) {
						result = streamReader.ReadToEnd();
						streamReader.Close();
					}
				}
			}
      }
			return result;
		}

		// server side site suck / spidery thing to grab a remote image and save it locally
		public static string DownloadImageFromRemoteSiteAndSaveToDisk(string fullImageUrl) {
			var folderName = "Attachments\\RemoteImages\\" + Fmt.YearMonth(DateTime.Today) + "\\";
			var path = folderName +
			           FileSystem.GetUniqueFilename(folderName,
				           fullImageUrl.Replace("/", "\\").Substring(fullImageUrl.LastIndexOf("/")));
			FileSystem.CreateFolder(Web.MapPath("~\\" + folderName));

			byte[] imageBytes;
			HttpWebRequest imageRequest = (HttpWebRequest) WebRequest.Create(fullImageUrl);
			WebResponse imageResponse = imageRequest.GetResponse();

			Stream responseStream = imageResponse.GetResponseStream();

			using (BinaryReader br = new BinaryReader(responseStream)) {
				//imageBytes = br.ReadBytes(500000);					//jn replaced this
				const int bufferSize = 4096;
				using (var ms = new MemoryStream()) {
					byte[] buffer = new byte[bufferSize];
					int count;
					while ((count = br.Read(buffer, 0, buffer.Length)) != 0) {
						ms.Write(buffer, 0, count);
					}
					imageBytes = ms.ToArray();
				}
				br.Close();
			}
			responseStream.Close();
			imageResponse.Close();

			FileStream fs = new FileStream(Web.MapPath("~\\" + path), FileMode.Create);
			BinaryWriter bw = new BinaryWriter(fs);
			try {
				bw.Write(imageBytes);
			} finally {
				fs.Close();
				bw.Close();
			}
			var result = Web.BaseUrl + path.Replace("\\", "/");
			return result;
		}

	}

}


namespace BewebTest {
	[TestClass]
	public class HttpTests {
		[SlowTestMethod]
		public void TestAsyncGet() {
			string url = "http://www.adobe.com";
			for (var i = 0; i < 100; i++) {
				//Http.DownloadDataInBackground(url);
				Util.HttpGetAsync(url, "http://drunk/RAL/admin/");
				//Util.HttpGetAsync("http://www.adobe.com/page?"+i, "");
				//Util.HttpGetAsync("http://www.test.com/page?"+i, "");
				//Util.HttpGetAsync("http://www.test.com/?"+i, "");
				//Util.HttpGetAsync("http://www.tempuri.com/?"+i, "");
				//Util.HttpGetAsync("http://www.loadtest.com/?"+i, "");
				//Util.HttpGetAsync("http://www.dummy.com/?"+i, "");
				//Util.HttpGetAsync("http://www.asdf.com/?"+i, "");
				//Util.HttpGetAsync("http://www.sorry.com/?"+i, "");
				//Util.HttpGetAsync("http://www.thisisatest.com/?"+i, "");
				//Util.HttpGetAsync("http://www.mike.com/?"+i, "");
				//Util.HttpGetAsync("http://www.flash.com/?"+i, "");
				//Util.HttpGetAsync("http://www.yahoo.com/?"+i, "");
				//Util.HttpGetAsync("http://www.msn.com/?"+i, "");
				//Util.HttpGetAsync("http://www.live.com/?"+i, "");
				//Util.HttpGetAsync("http://www.loadtestingtool.com/?"+i, "");
				//Util.HttpGetAsync("http://www.iwebtool.com/?"+i, "");
				//Util.HttpGetAsync("http://www.media-alerts.com/?"+i, "");
				//Util.HttpGetAsync("http://www.soapui.org/?"+i, "");
				//Util.HttpGetAsync("http://spaces.live.com/?"+i, "");
				//Util.HttpGetAsync("http://geocities.com/?"+i, "");
				if (i % 10 == 0) {
					Web.Write(i + " ");
					Web.Flush();
				}
				Thread.Sleep(10);
			}
			Assert.Pass();
		}

	}
}
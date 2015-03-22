using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Site.SiteCustom;


namespace Site.Controllers {
	public class TwitterController : ApplicationController {

		public ActionResult Index() {
			return Content("<script>window.opener.passTwitterUserToHerepin('" + Request["oauth_token"] + "','" + Request["oauth_verifier"] + "');window.close();</script>");
		}

	/* this code is for doing our own twitter auth
	 * API takes care of this for us now, so not needed
	 * keep it in case we need it later (or on another project)
	 
		private string oauth_consumer_key = "OWiXeQ2bjfIy0fLe9Z3w";
		string oauth_signature_method = "HMAC-SHA1";
		string oauth_version = "1.0";
		private string oauth_consumer_secret = "5fWsfGjI1A2utQhwfe51rvBGjbQgY1rIylIn2FRkA";


		private string GetNonce() {
			return new Random().Next(123400, int.MaxValue).ToString("X", CultureInfo.InvariantCulture);
		}
		private string GetTimestamp() {
			return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds, CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
		}


		public ActionResult GetScreenName(string oauth_token, string oauth_verifier) {
			var headers = new SortedDictionary<string, string>();
			headers.Add("oauth_token", oauth_token);
			var payload = new SortedDictionary<string, string>();
			payload.Add("oauth_verifier", oauth_verifier);
			var q = CallTwitterAPI("GET", "https://api.twitter.com/oauth/access_token", null, headers, payload);
	
			headers = new SortedDictionary<string, string>();
			headers.Add("oauth_token", q["oauth_token"]);
			q = CallTwitterAPI("GET", "https://api.twitter.com/1.1/account/verify_credentials.json", q["oauth_token_secret"], headers, null);

		//	headers = new SortedDictionary<string, string>();
		//	headers.Add("oauth_token", q["oauth_token"]);
		//	payload = new SortedDictionary<string, string>();
		//	//payload.Add("oauth_verifier", oauth_verifier);
		//	payload.Add("user_id", q["user_id"]);
		//	payload.Add("screen_name", q["screen_name"]);
		//	q = CallTwitterAPI("GET", "https://api.twitter.com/1.1/users/show.json", q["oauth_token_secret"], headers, payload);

			//string data = "oauth_verifier=" + oauth_verifier;
			//var response = Http.Post("https://api.twitter.com/oauth/access_token", data);
			//var q = HttpUtility.ParseQueryString(response);

			return Content("<script>window.opener.passTwitterUserToHerepin('" + q["oauth_token"] + "','" + q["oauth_token_secret"] + "','" + q["user_id"] + "','" + q["screen_name"] + "');window.close();</script>");
		}

		public ActionResult TwitterAuthUrl() {
			string oauth_request_token_url = "https://api.twitter.com/oauth/request_token";
			//string oauth_callback = Web.BaseUrl + "Twitter/GetScreenName";
			string oauth_callback = Web.BaseUrl + "Twitter";

			string oauth_nonce = GetNonce();
			string oauth_timestamp = GetTimestamp();

			string oauth_signature =
					"oauth_callback=" + UrlEncode(oauth_callback) + "&" +
					"oauth_consumer_key=" + UrlEncode(oauth_consumer_key) + "&" +
					"oauth_nonce=" + UrlEncode(oauth_nonce) + "&" +
					"oauth_signature_method=" + UrlEncode(oauth_signature_method) + "&" +
					"oauth_timestamp=" + UrlEncode(oauth_timestamp) + "&" +
					"oauth_version=" + UrlEncode(oauth_version);
			oauth_signature = "GET" + "&" + UrlEncode(oauth_request_token_url) + "&" + UrlEncode(oauth_signature);
			oauth_signature = Convert.ToBase64String((new HMACSHA1(Encoding.ASCII.GetBytes(UrlEncode(oauth_consumer_secret) + "&"))).ComputeHash(Encoding.ASCII.GetBytes(oauth_signature)));

			string header =
					"OAuth" + " " +
					"realm=\"" + "Twitter API" + "\"," +
					"oauth_consumer_key=\"" + UrlEncode(oauth_consumer_key) + "\"," +
					"oauth_nonce=\"" + UrlEncode(oauth_nonce) + "\"," +
					"oauth_signature_method=\"" + UrlEncode(oauth_signature_method) + "\"," +
					"oauth_timestamp=\"" + UrlEncode(oauth_timestamp) + "\"," +
					"oauth_version=\"" + UrlEncode(oauth_version) + "\"," +
					"oauth_signature=\"" + UrlEncode(oauth_signature) + "\"";

			HttpWebRequest r = (HttpWebRequest)WebRequest.Create(oauth_request_token_url + "?" + "oauth_callback" + "=" + oauth_callback);
			r.Method = "GET";
			r.ContentLength = 0;
			r.ServicePoint.Expect100Continue = false;
			r.Headers.Add("Authorization", header);

			HttpWebResponse s = (HttpWebResponse)r.GetResponse();
			Stream stream = s.GetResponseStream();
			var result = new StreamReader(stream).ReadToEnd();

			var queryPrams = HttpUtility.ParseQueryString(result);

			return Redirect("https://api.twitter.com/oauth/authorize?oauth_token=" + queryPrams["oauth_token"]);
			//return Redirect("https://api.twitter.com/oauth/authenticate?oauth_token=" + queryPrams["oauth_token"]);//+"&oauth_token_secret="+queryPrams["oauth_token_secret"]);
		}

		private NameValueCollection CallTwitterAPI(string method, string twitterApiUrl, string oauthTokenSecret, SortedDictionary<string, string> extraHeaderParams, SortedDictionary<string, string> payloadParams) {
			string oauth_nonce = GetNonce();
			string oauth_timestamp = GetTimestamp();

			if (extraHeaderParams == null) {
				extraHeaderParams = new SortedDictionary<string, string>();
			}
			extraHeaderParams["oauth_consumer_key"] = oauth_consumer_key;
			extraHeaderParams["oauth_nonce"] = oauth_nonce;
			extraHeaderParams["oauth_signature_method"] = oauth_signature_method;
			extraHeaderParams["oauth_timestamp"] = oauth_timestamp;
			extraHeaderParams["oauth_version"] = oauth_version;

			string oauth_signature = "";
			foreach (string key in extraHeaderParams.Keys) {
				if (key.StartsWith("oauth_")) {
					oauth_signature += "&" + UrlEncode(key + "") + "=" + UrlEncode(extraHeaderParams[key] + "");
				}
			}
			if (payloadParams != null) {
				foreach (string key in payloadParams.Keys) {
					oauth_signature += "&" + UrlEncode(key) + "=" + UrlEncode(payloadParams[key] + "");
				}
			}
			oauth_signature = method + "&" + UrlEncode(twitterApiUrl) + "&" + UrlEncode(oauth_signature);
			string signingKey = UrlEncode(oauth_consumer_secret) + "&";
			if (oauthTokenSecret!=null) signingKey+= UrlEncode(oauthTokenSecret);  // note oauthTokenSecret may be null if not known yet
			oauth_signature = Convert.ToBase64String((new HMACSHA1(Encoding.ASCII.GetBytes(signingKey))).ComputeHash(Encoding.ASCII.GetBytes(oauth_signature)));
			extraHeaderParams.Add("oauth_signature", oauth_signature);

			string header = "OAuth ";
			foreach (string key in extraHeaderParams.Keys) {
				header += UrlEncode(key) + "=\"" + UrlEncode(extraHeaderParams[key] + "") + "\", ";
			}
			header = header.Trim().RemoveSuffix(",");

			// eg Authorization: OAuth oauth_consumer_key="fo4IjyKDvvAa2M8btBLYxQ", oauth_nonce="8082830e56b1d10c2303c6978971f7ea", oauth_signature="8EEwDqeeTQaZfF40RPjrEwoGOdw%3D", oauth_signature_method="HMAC-SHA1", oauth_timestamp="1383647486", oauth_token="27609133-lNhCPb3QLr23pbo7JgJmo5hVjGBaBvO08bhdAmdz4", oauth_version="1.0"

			// form post payload or get url query
			var data = "";
			if (payloadParams != null) {
				foreach (string key in payloadParams.Keys) {
					data += "&" + UrlEncode(key) + "=" + UrlEncode(payloadParams[key] + "");
				}
			}
			data = data.RemovePrefix("&");
			if (method == "GET") {
				// for GET payload is added to the URL
				twitterApiUrl += "?" + data;
			}
			HttpWebRequest r = (HttpWebRequest)WebRequest.Create(twitterApiUrl);
			r.Method = method; //"GET";
			r.ContentLength = 0;
			r.ServicePoint.Expect100Continue = false;
			if (oauthTokenSecret!=null) {
				//header = "OAuth oauth_consumer_key=\"fo4IjyKDvvAa2M8btBLYxQ\", oauth_nonce=\"46754e94151913a9b234ab8ab07493ca\", oauth_signature=\"weYlNVh6yffVWa9dmJqQu2jI5PI%3D\", oauth_signature_method=\"HMAC-SHA1\", oauth_timestamp=\"1383649882\", oauth_token=\"27609133-EE3NPVklok5WXCAzmM5QdA7qU5u1p1BBRKUwyriNT\", oauth_version=\"1.0""
			}
			r.Headers.Add("Authorization", header);
			if (method == "POST") {
				// for GET payload is the body
				ASCIIEncoding encoding = new ASCIIEncoding();
				byte[] byte1 = encoding.GetBytes(data);
				Stream newStream = r.GetRequestStream();
				newStream.Write(byte1, 0, byte1.Length);
			}

			HttpWebResponse s = (HttpWebResponse)r.GetResponse();
			Stream stream = s.GetResponseStream();
			var result = new StreamReader(stream).ReadToEnd();

			return HttpUtility.ParseQueryString(result);
		}

		public string UrlEncode(string value) {
			value = Uri.EscapeDataString(value);
			value = Regex.Replace(value, "(%[0-9a-f][0-9a-f])", c => c.Value.ToUpper());
			return value;
		}

		//public static bool ValidateSignedRequest(string VALID_SIGNED_REQUEST) {
		//	string[] signedRequest = VALID_SIGNED_REQUEST.Split('.');            
		//	string expectedSignature = signedRequest[0];
		//	string payload = signedRequest[1];

		//	// Attempt to get same hash
		//	var Hmac = SignWithHmac(UTF8Encoding.UTF8.GetBytes(payload), UTF8Encoding.UTF8.GetBytes(oauth_consumer_secret));
		//	var HmacBase64 = ToUrlBase64String(Hmac);

		//	return (HmacBase64 == expectedSignature);
		//}

		//private static string ToUrlBase64String(byte[] Input){
		//	return Convert.ToBase64String(Input).Replace("=", String.Empty).Replace('+', '-').Replace('/', '_');
		//}

		//private static byte[] SignWithHmac(byte[] dataToSign, byte[] keyBody){
		//	using (var hmacAlgorithm = new HMACSHA256(keyBody)){
		//		hmacAlgorithm.ComputeHash(dataToSign);
		//		return hmacAlgorithm.Hash;
		//	}
		//}


		public class ViewModel : PageTemplateViewModel {

		}
		*/
	}
}
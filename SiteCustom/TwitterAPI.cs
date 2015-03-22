using System;
using System.IO;
using System.Net;
using System.Text;

/*
	Twitter API - Created by André Feijó
 */
using Beweb;

namespace Site.SiteCustom {
	
	public class TwitterAPI {

		private string oAuthConsumerKey;
		private string oAuthConsumerSecret;
		private const string oAuthUrl = "https://api.twitter.com/oauth2/token";

		// It will be filled up by the authentication (Auth method)
		private string tokenType;
		private string accessToken;

		public TwitterAPI(string consumerKey, string consumerSecret) {
			oAuthConsumerKey = consumerKey;
			oAuthConsumerSecret = consumerSecret;
		}

		// This Auth enables us to do requests to Twitter API
		private void Auth() {
			var authHeaderFormat = "Basic {0}";

			var authHeader = string.Format(authHeaderFormat,
					Convert.ToBase64String(Encoding.UTF8.GetBytes(Uri.EscapeDataString(oAuthConsumerKey) + ":" +
					Uri.EscapeDataString((oAuthConsumerSecret)))
			));

			var postBody = "grant_type=client_credentials";

			HttpWebRequest authRequest = (HttpWebRequest)WebRequest.Create(oAuthUrl);
			authRequest.Headers.Add("Authorization", authHeader);
			authRequest.Method = "POST";
			authRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
			authRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

			using (Stream stream = authRequest.GetRequestStream()) {
					byte[] content = ASCIIEncoding.ASCII.GetBytes(postBody);
					stream.Write(content, 0, content.Length);
			}

			authRequest.Headers.Add("Accept-Encoding", "gzip");

			WebResponse authResponse = authRequest.GetResponse();

			// deserialize into an object
			using (authResponse) {
					using (var reader = new StreamReader(authResponse.GetResponseStream())) {
							var objectText = reader.ReadToEnd();
							dynamic authResponseObj = JsonHelper.Parse(objectText);
							tokenType = authResponseObj.token_type;
							accessToken = authResponseObj.access_token;
					}
			}
		}

		public string GetJson(string url) {

			if (string.IsNullOrEmpty(accessToken)) {
				Auth();
			}

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			var format = "{0} {1}";
			request.Headers.Add("Authorization", string.Format(format, tokenType, accessToken));
			request.Method = "Get";
			WebResponse timeLineResponse = request.GetResponse();
			var json = string.Empty;
			using (timeLineResponse) {
					using (var reader = new StreamReader(timeLineResponse.GetResponseStream())) {
							 json = reader.ReadToEnd();
					}
			}
			return json;
		}

	}
}
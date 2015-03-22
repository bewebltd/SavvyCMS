using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Beweb;
using Models;
using Newtonsoft.Json.Linq;

namespace Site.SiteCustom {
	public class FacebookBeweb {


		public static JObject ExtractSignedRequestJson() {
			string facebook_signed_request = "";
			facebook_signed_request = Web.Request.Form["signed_request"];

			if (facebook_signed_request.IsBlank() || !ValidateSignedRequest(facebook_signed_request)) {
				return null;
			}

			string json = DecodePayload(facebook_signed_request);

			return JObject.Parse(json);
		}

		public static string DecodePayload(string fullReturnedString) {
			if (fullReturnedString.IsBlank()) return ""; // no payload
			var payload = GetPayload(fullReturnedString);
			var encoding = new UTF8Encoding();
			var decodedJson = payload.Replace("=", string.Empty).Replace('-', '+').Replace('_', '/');
			var base64JsonArray = Convert.FromBase64String(decodedJson.PadRight(decodedJson.Length + (4 - decodedJson.Length % 4) % 4, '='));
			var json = encoding.GetString(base64JsonArray);
			return json;
		}
		public static string GetPayload(string fullReturnedString) {
			// get the bit after the "." the bit before is a hash check
			return fullReturnedString.Substring(fullReturnedString.IndexOf(".") + 1);
		}

		public static bool ValidateSignedRequest(string VALID_SIGNED_REQUEST) {
			string[] signedRequest = VALID_SIGNED_REQUEST.Split('.');
			string expectedSignature = signedRequest[0];
			string payload = signedRequest[1];

			// Attempt to get same hash
			string applicationSecret = Beweb.Util.GetSetting("FacebookApiSecret");
			var Hmac = SignWithHmac(UTF8Encoding.UTF8.GetBytes(payload), UTF8Encoding.UTF8.GetBytes(applicationSecret));
			var HmacBase64 = ToUrlBase64String(Hmac);

			return (HmacBase64 == expectedSignature);
		}
		private static string ToUrlBase64String(byte[] Input) {
			return Convert.ToBase64String(Input).Replace("=", String.Empty)
												.Replace('+', '-')
												.Replace('/', '_');
		}
		private static byte[] SignWithHmac(byte[] dataToSign, byte[] keyBody) {
			using (var hmacAlgorithm = new HMACSHA256(keyBody)) {
				hmacAlgorithm.ComputeHash(dataToSign);
				return hmacAlgorithm.Hash;
			}
		}

	}

	public class FacebookAuth {
		public FacebookAuth() { }
		public FacebookAuth(JObject jObject) {
			UserID = jObject["user_id"] + "";
			AccessToken = jObject["oauth_token"] + "";
			if (jObject["page"] != null && jObject["page"]["liked"] != null) {
				YouLikeIt = (bool)jObject["page"]["liked"];
			}
		}

		public string UserID;
		public string AccessToken;
		public bool YouLikeIt;
	}
}
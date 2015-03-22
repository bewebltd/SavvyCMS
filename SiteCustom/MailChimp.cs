using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Beweb;

namespace Site.SiteCustom
{
	public class MailChimp
	{
		public static bool Subscribe(string emailAddress, string firstName, string lastName, int entryID) {
			// uses these keys:
/*
	<add key="MailChimpUrlDEV" value="http://us2.api.mailchimp.com/1.3/"/>
	<add key="MailChimpUrlSTG" value="http://us2.api.mailchimp.com/1.3/"/>
	<add key="MailChimpUrlLVE" value="http://us1.api.mailchimp.com/1.3/"/>
	<add key="MailChimpApiKeyDEV" value="5e155dd11d0813b8996e84ebf008783d-us2"/>
	<add key="MailChimpApiKeySTG" value="5e155dd11d0813b8996e84ebf008783d-us2"/>
	<add key="MailChimpApiKeyLVE" value="b4b006786dde2230b8b460e7cde616a4-us1"/>
	<add key="MailChimpListIdDEV" value="fe974029d9"/>
	<add key="MailChimpListIdSTG" value="fe974029d9"/>
	<add key="MailChimpListIdLVE" value="f9696666b3"/>
*/
			
			// http://apidocs.mailchimp.com/api/1.3/
			// http://apidocs.mailchimp.com/api/1.3/listsubscribe.func.php

			// to find a list Id - either look it up on the mailchimp console or use this:
			// put this in your browser: http://us2.api.mailchimp.com/1.3/?method=lists&apikey=53985383f8824facf94578802844d1e1-us2

			string url = Util.GetSetting("MailChimpUrl");
			string apiKey = Util.GetSetting("MailChimpApiKey");
			string listId = Util.GetSetting("MailChimpListId");
			string source = "Paddock Prizes App";

			url += "?method=listSubscribe";
			url += "&apikey=" + apiKey;
			url += "&id=" + listId;
			url += "&double_optin=false";
			url += "&send_welcome=false";
			url += "&email_address=" + emailAddress;
			url += "&merge_vars[FNAME]=" + firstName;
			url += "&merge_vars[LNAME]=" + lastName;
			url += "&merge_vars[MMERGE5]=" + source; // Source
			url += "&merge_vars[MMERGE6]=" + entryID; // EntryID

			var returnVal = Http.Get(url);

			return returnVal == "true" || returnVal.ToLower().Contains("already subscribed");
		}
	}
}
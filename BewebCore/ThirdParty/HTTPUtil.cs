using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Beweb
{
	class HTTPUtil
	{
		/*
		 from rick strahl
		  Html and Uri String Encoding without System.Web
18 comments
February 05, 2009 @ 5:17 am-  from Maui, Hawaii


I’ve been revisiting and refactoring some of my old utility libraries recently. One of my classes is WebUtils which is – duh - Web specific and really didn’t need to be in this general utility library and so I moved it off into my Web support library that contains the brunt of Web specific behavior like custom controls and general ASP.NET helpers. The Westwind.Web project naturally contains a reference to System.Web because it’s a Web project.

After removing the Web functionality from the Westwind.Utilities project I thought I could safely ditch the System.Web reference from the project, but alas I hit a snag – there was still a dependency for the HttpUtility class to provide UrlEncoding/Decoding and HtmlEncoding. Part of the Utilities project includes communication classes that use Http access and interact with Web servers so UrlEncoding at the very least is still a requirment and HtmlEncoding and UrlDecoding comes up in a few places as well.

So the problem is that the only comprehensive set of of UrlEncoding/Decoding and HtmlEncoding/Decoding features available in the .NET framework live in System.Web. Which is a bad design choice – these are general features that probably should live in System.Net. It turns out System.Uri contains Url encoding/decoding functionality but HtmlEncoding is not to be found outside of System.Web.

Now I could add a System.Web dependency to my Utility library – but that just doesn’t sit well with me. It forces System.Web into the loaded assembly  list of any application consuming the library. Now normally I’m not a stickler for including a an assembly here or there, but System.Web is quite a honker and loading it into your app will add a good 2.5 megs to the memory footprint just for loading it. And for just the privilege of Url and Html Encoding/Decoding that’s quite a bit of overhead. It also slows down load time as the assembly is read on system startup etc.

Long story short – I’d like to avoid including System.Web into a non-Web application – it just doesn’t feel right.
Url Encoding and Decoding

It turns out System.Net that when .NET 2.0 rolled around it did get some Url Encoding and Decoding functions. Unfortunately it looks like these functions don’t provide the full range of functionality that the HttpUtility functions provided. The System.Uri class contains a few static helper methods to escape data. The following code (in LinqPad) demonstrates the functionality:

string test = "This is a value & I don't care for it.\t\"quoted\" 'single quoted',<% alligator %>#";
string encoded = System.Web.HttpUtility.UrlEncode(test);
encoded.Dump();
System.Uri.EscapeDataString(test).Dump();
System.Uri.EscapeUriString(test).Dump();
Westwind.Utilities.StringUtils.UrlEncode(test).Dump();

System.Uri.UnescapeDataString(encoded).Dump();
System.Web.HttpUtility.UrlDecode(encoded).Dump();
Westwind.Utilities.StringUtils.UrlDecode(encoded).Dump();

The result of this diverse bunch is:

This+is+a+value+%26+I+don't+care+for+it.%09%22quoted%22+'single+quoted'%2c%3c%25+alligator+%25%3e%23
This%20is%20a%20value%20%26%20I%20don't%20care%20for%20it.%09%22quoted%22%20'single%20quoted'%2C%3C%25%20alligator%20%25%3E%23
This%20is%20a%20value%20&%20I%20don't%20care%20for%20it.%09%22quoted%22%20'single%20quoted',%3C%25%20alligator%20%25%3E#
This+is+a+value+%26+I+don%27t+care+for+it%2E%09%22quoted%22+%27single+quoted%27%2C%3C%25+alligator+%25%3E%23


This+is+a+value+&+I+don't+care+for+it.    "quoted"+'single+quoted',<%+alligator+%>#
This is a value & I don't care for it.    "quoted" 'single quoted',<% alligator %>#
This is a value & I don't care for it.    "quoted" 'single quoted',<% alligator %>#

Oh my what a fucking mess. Every single version (including my own) generates something different. All of them are actually valid, but the output generated from HttpUtility.UrlEncode is NOT parsed properly by the System.Uri methods. Ouch!

Notice that HttpUtility.UrlEncode and the System.Uri equivalents output different kinds of formatting. And worse that they are not compatible with each other – System.Uri.UnescapeDataString() cannot properly decode output created with System.Web.HttpUtility.UrlEncode() if it contains the + sign for spaces. That’s a bummer since the + sign syntax is certainly legal and well HttpUtility itself outputs spaces in that format. Only HttpUtility.UrlDecode() seems to work with both the + and %20 and does the right thing in all places, but the System.Uri equivalents fail to restore the string.

If we start UrlEncoding with EscapeDataString():

string encoded = System.Uri.EscapeDataString(test);

then all of the decoders work in returning the same result at least.

Sooo… to avoid the System.Web Reference and get around the confusion I needed to replace the calls to  HttpUtility without System.Web dependencies. Luckily a long time ago when I used these functions I had the good sense to create wrapper utility functions for the HttpUtility calls because it bugged me even in my early .NET days to have to include a reference to System.Web in a non-Web app, so the following function signatures were already part of my StringUtils class. The following are the UrlEncoding and Decoding related static methods of that class:
		 
		 */
			/// <summary>
		/// UrlEncodes a string without the requirement for System.Web
		/// </summary>
		/// <param name="String"></param>
		/// <returns></returns>
		// [Obsolete("Use System.Uri.EscapeDataString instead")]
		public static string UrlEncode(string text)
		{
				// Sytem.Uri provides reliable parsing
				return System.Uri.EscapeDataString(text);
		}

		/// <summary>
		/// UrlDecodes a string without requiring System.Web
		/// </summary>
		/// <param name="text">String to decode.</param>
		/// <returns>decoded string</returns>
		public static string UrlDecode(string text)
		{
				// pre-process for + sign space formatting since System.Uri doesn't handle it
				// plus literals are encoded as %2b normally so this should be safe
				text = text.Replace("+", " ");
				return System.Uri.UnescapeDataString(text);
		}

		/// <summary>
		/// Retrieves a value by key from a UrlEncoded string.
		/// </summary>
		/// <param name="urlEncoded">UrlEncoded String</param>
		/// <param name="key">Key to retrieve value for</param>
		/// <returns>returns the value or "" if the key is not found or the value is blank</returns>
		public static string GetUrlEncodedKey(string urlEncoded, string key)
		{
				urlEncoded = "&" + urlEncoded + "&";

				int Index = urlEncoded.IndexOf("&" + key + "=",StringComparison.OrdinalIgnoreCase);
				if (Index < 0)
						return "";

				int lnStart = Index + 2 + key.Length;

				int Index2 = urlEncoded.IndexOf("&", lnStart);
				if (Index2 < 0)
						return "";

				return UrlDecode(urlEncoded.Substring(lnStart, Index2 - lnStart));
		}

		//The UrlEncode method is just a passthrough to System.Uri.EscapeDataString() because it actually does the right thing. Initially I was going to Obsolete this method, but I decided against it – for consistency with the other wrappers it makes sense to use a common API to make the calls.

		//Decoding then pre-processes the input string for + signs since UnescapeDataString() doesn’t handle them by converting them into spaces. This makes for an invalid UrlEncoded string but UnescapeString leaves the embedded spaces alone, so it actually works as expected converting plus signs to spaces.

		//The final method is GetUrlEncodedKey which is basically a quick and dirty query string parser to return a single query string value. This is quite useful if a client app needs to look at intercepted URLs – for example in Web Browser Navigate events. Passing values as UrlEncoded strings can also be useful in interapplication communication for small chunks of message data in some situations.
		//Html Encoding

		//Html Encoding and Decoding has no equivalent to the HttpUtility functions so this is left up to the developer. HtmlEncoding I need to do quite frequently in client  applications that use HTML content (I do this alot using the Web Browser control to display certain content). But HtmlDecoding I have never really had a need for. Decoding is also quite a bit more complex than encoding so I never bothered with that. However encoding is more common and also straight forward to implement:

				/// <summary>
				/// HTML-encodes a string and returns the encoded string.
				/// </summary>
				/// <param name="text">The text string to encode. </param>
				/// <returns>The HTML-encoded text.</returns>
				public static string HtmlEncode(string text)
				{
						if (text == null)
								return null;

						StringBuilder sb = new StringBuilder(text.Length);

						int len = text.Length;
						for (int i = 0; i < len; i++)
						{
								switch (text[i])
								{

										case '<':
												sb.Append("&lt;");
												break;
										case '>':
												sb.Append("&gt;");
												break;
										case '"':
												sb.Append("&quot;");
												break;
										case '&':
												sb.Append("&amp;");
												break;
										default:
												if (text[i] > 159)
												{
														// decimal numeric entity
														sb.Append("&#");
														sb.Append(((int)text[i]).ToString(CultureInfo.InvariantCulture));
														sb.Append(";");
												}
												else
														sb.Append(text[i]);
												break;
								}
						}
						return sb.ToString();
				}

		}
	}


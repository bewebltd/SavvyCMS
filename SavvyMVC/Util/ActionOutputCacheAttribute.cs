using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO.Compression;
using System.Web;
using System.Reflection;
using System.Web.Mvc;
using System.IO;
using System.Web.UI;
using System.Web.Caching;
using System.Text;
using Beweb;

namespace SavvyMVC {

	/// <summary>
	/// Code originally from Steve Sanderson.
	/// Enhancements to support contenttype and UTF8 by Miha Markic.
	/// Mike@Beweb added full URL to cache by unique URLs, similar to VaryByParam, and Donut Hole Replacement
	/// http://blog.stevensanderson.com/2008/10/15/partial-output-caching-in-aspnet-mvc/
	/// http://blog.rthand.com/post/2009/03/21/Partial-Output-Caching-in-ASPNET-MVC-updated.aspx
	/// </summary>
	public class ActionOutputCacheAttribute : ActionFilterAttribute {
		// This hack is optional; I'll explain it later in the blog post
		private static MethodInfo _switchWriterMethod = typeof(HttpResponse).GetMethod("SwitchWriter", BindingFlags.Instance | BindingFlags.NonPublic);

		public ActionOutputCacheAttribute(int cacheDuration) {
			_cacheDuration = Beweb.Util.GetSettingInt("SiteOutputCacheDurationSeconds", cacheDuration);
		}

		private int _cacheDuration;
		private TextWriter _originalWriter;
		private string _cacheKey;
		private DateTime _startTime = DateTime.Now;

		/// <summary>
		/// Overridable method to fill in any "donut holes" or template codes that were created. In this method you can make any text replacements each time a response is fetched from cache and output.
		/// </summary>
		/// <param name="cachedOutput">The output just after coming out of the cache.</param>
		/// <returns>The final output, after any custom post processing.</returns>
		protected virtual string ReplaceDonutHoles(string cachedOutput) {
			return cachedOutput;
		}

		/// <summary>
		/// Overridable method to add in any "donut holes" or template codes which will be replaced out each time a response is fetched from cache and output.
		/// </summary>
		/// <param name="originalOutput">The original output before it is stored in the cache.</param>
		/// <returns>The output to be stored in the cache.</returns>
		protected virtual string CreateDonutHoles(string originalOutput) {
			return originalOutput;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext) {
			_originalWriter = null;
			if (_cacheDuration == 0) return;

			_startTime = DateTime.Now;
			_cacheKey = ComputeCacheKey(filterContext);
			CacheContainer cachedOutput = (CacheContainer)filterContext.HttpContext.Cache[_cacheKey];
			var hardReload = filterContext.RequestContext.HttpContext.Request.Headers["Cache-Control"] == "no-cache";
			if (cachedOutput != null && !hardReload) {
				// cache hit
				foreach (string header in cachedOutput.Headers) {
					// AF20141028 Don't include GZIP header
					if (header != "Content-Encoding") {
						filterContext.HttpContext.Response.Headers[header] = cachedOutput.Headers[header];
					}
				}

				filterContext.HttpContext.Response.Status = cachedOutput.Status;
				filterContext.HttpContext.Response.StatusCode = cachedOutput.StatusCode;
				filterContext.HttpContext.Response.SubStatusCode = cachedOutput.SubStatusCode;
				filterContext.HttpContext.Response.ContentEncoding = cachedOutput.ContentEncoding;
				filterContext.HttpContext.Response.HeaderEncoding = cachedOutput.HeaderEncoding;
				filterContext.HttpContext.Response.Charset = cachedOutput.Charset;
				filterContext.HttpContext.Response.ContentType = cachedOutput.ContentType;

				filterContext.Result = new ContentResult { Content = ReplaceDonutHoles(cachedOutput.Output) + "<!-- cache hit: " + _startTime.FmtMillisecondsElapsed() + " -->" };

			} else {
				// new request to cache
				StringWriter stringWriter = new StringWriterWithEncoding(filterContext.HttpContext.Response.ContentEncoding);
				HtmlTextWriter newWriter = new HtmlTextWriter(stringWriter);
				_originalWriter = (TextWriter)_switchWriterMethod.Invoke(HttpContext.Current.Response, new object[] { newWriter });
			}
		}

		public override void OnResultExecuted(ResultExecutedContext filterContext) {
			if (_originalWriter != null) {  // Must complete the caching 
				// new request to cache - add it to the cache
				HtmlTextWriter cacheWriter = (HtmlTextWriter)_switchWriterMethod.Invoke(HttpContext.Current.Response, new object[] { _originalWriter });
				string textWritten = ((StringWriter)cacheWriter.InnerWriter).ToString();
				textWritten = CreateDonutHoles(textWritten);
				CacheContainer container = new CacheContainer(textWritten, filterContext.HttpContext.Response);
				filterContext.HttpContext.Cache.Add(_cacheKey, container, null, DateTime.Now.AddSeconds(_cacheDuration), Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
				filterContext.HttpContext.Response.Write(ReplaceDonutHoles(textWritten));
				var hardReload = filterContext.RequestContext.HttpContext.Request.Headers["Cache-Control"] == "no-cache";
				filterContext.HttpContext.Response.Write("<!-- cache prep: " + _startTime.FmtMillisecondsElapsed() + " " + (hardReload ? "Hard Reload" : "") + " -->");
			}
		}

		/// <summary>
		/// Returns a unique string for each version of a page/action that should be output. This generally means a unique URL and/or unique action method call (in the case of a subcontroller call). We also consider logged in vs not logged in to be a unique version (but not each user - we expect to use Donut Caching to resolve displaying the username and don't use caching at all if pages contain complex user-dependent content).
		/// </summary>
		/// <param name="filterContext"></param>
		/// <returns></returns>
		protected string ComputeCacheKey(ActionExecutingContext filterContext) {
			var keyBuilder = new StringBuilder();

			// maybe just return the URL?
			keyBuilder.Append("BewebOutputCache:").Append(Beweb.Web.FullUrl).Append("::" + Security.IsLoggedIn);

			//// maybe not do this if should not vary by page params? (i.e. if caching common snippet that is same on all pages like a footer)
			foreach (var pair in filterContext.RouteData.Values) {
				keyBuilder.AppendFormat("rd{0}_{1}_", pair.Key.GetHashCode(), pair.Value == null ? -99999 : pair.Value.GetHashCode());
			}
			foreach (var pair in filterContext.ActionParameters) {
				keyBuilder.AppendFormat("ap{0}_{1}_", pair.Key.GetHashCode(), pair.Value == null ? -99999 : pair.Value.GetHashCode());
			}
			return keyBuilder.ToString();
		}


	}

	class CacheContainer {
		public string Output;
		public string ContentType;
		public int StatusCode;
		public int SubStatusCode;
		public string Status;
		public Encoding ContentEncoding;
		public Encoding HeaderEncoding;
		public string Charset;
		public NameValueCollection Headers;

		/// <summary>
		/// Initializes a new instance of the CacheContainer class.
		/// </summary>
		public CacheContainer(string data, HttpResponseBase response) {
			Output = data;
			ContentType = response.ContentType;
			Status = response.Status;
			StatusCode = response.StatusCode;
			SubStatusCode = response.SubStatusCode;
			ContentEncoding = response.ContentEncoding;
			HeaderEncoding = response.HeaderEncoding;
			Charset = response.Charset;
			Headers = response.Headers;
		}

	}

	public class StringWriterWithEncoding : StringWriter {
		Encoding encoding;
		public StringWriterWithEncoding(Encoding encoding) {
			this.encoding = encoding;
		}
		public override Encoding Encoding {
			get { return encoding; }
		}
	}


}

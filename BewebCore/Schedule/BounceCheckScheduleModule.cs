using System;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Threading;
using System.Net;

using Beweb;

/// <summary>
/// Summary description for BounceCheckScheduleModule
/// </summary>
public class BounceCheckScheduleModule : IHttpModule
{
	#region IHttpModule Members

	public void Dispose() {
		BounceCheckerRunner.Instance.Stop();
	}

	public void Init(HttpApplication context) {
		context.BeginRequest += new EventHandler(StartBounceChecker);
	}

	private void StartBounceChecker(object sender, EventArgs e) {
		BounceCheckerRunner.Instance.Init();
	}

	#endregion
}

public sealed class BounceCheckerRunner {

	private static readonly BounceCheckerRunner instance = new BounceCheckerRunner();
	public static BounceCheckerRunner Instance { get { return instance; } }

	private BounceCheckerRunner() {
		// singleton
	}

	private const int WaitTime = 15 * 60 * 1000;		// 15 minutes
	private static string BounceEmailPattern = ConfigurationManager.AppSettings["BounceEmailAddress"];

	private bool hasStarted;
	private string websiteBaseUrl, serverIs;
	private object synclock = new object();
	private Thread thread;

	public void Init() {
		if (!hasStarted) {
			lock (synclock) {
				hasStarted = true;
				websiteBaseUrl = GetWebUrl();
				serverIs = Util.ServerIs();
				ThreadStart start = new ThreadStart(RunBounceChecker);
				thread = new Thread(start);
				thread.Start();
			}
		}
	}

	public void Stop() {
		thread.Abort();
		hasStarted = false;
	}

	private string GetWebUrl() {
		string host = (HttpContext.Current.Request.Url.IsDefaultPort) ? HttpContext.Current.Request.Url.Host : HttpContext.Current.Request.Url.Authority;
		return String.Format("{0}://{1}", HttpContext.Current.Request.Url.Scheme, host);
	}

	private void RunBounceChecker() {

		while (true) {
			DateTime start = DateTime.Now;
			using (WebClient wc = new WebClient()) {
				try {
					string response = wc.DownloadString(websiteBaseUrl + "/newsletter/ProcessMailboxEntries.asp?pattern=" + BounceEmailPattern.UrlEncode());
				} catch (Exception ex) {
					LogException(start, ex);
				} finally {
					TimeSpan ts = DateTime.Now.Subtract(start);
					Thread.Sleep(WaitTime - (int)ts.TotalMilliseconds);
				}
			}
		}
	}

	private void LogException(DateTime start, Exception ex) {
		StringBuilder msg = new StringBuilder();
		msg.AppendLine("Exception caught while attempting to run the bounce checker.")
			.AppendLine("Details:");
		PrintStackTrace(msg, ex);
		msg.AppendLine("Other Details:")
			.Append("URL: ").AppendLine(websiteBaseUrl)
			.Append("Process Start Time: ").AppendLine(start.FmtDateTime())
			.Append("Exception Time: ").AppendLine(DateTime.Now.FmtDateTime());

		string emailTo = ConfigurationManager.AppSettings.Get("EmailAboutError") ?? "matt@beweb.co.nz";
		string subject = string.Format("ERROR on: {0}", websiteBaseUrl);
		Error.SendErrorEmail(emailTo, subject, msg.ToString(), serverIs);
	}

	private void PrintStackTrace(StringBuilder msg, Exception ex) {
		msg.AppendLine("Exception caught while attempting to run the bounce checker")
			.AppendLine("Details:")
			.AppendLine(ex.Message)
			.Append(ex.StackTrace)
			.AppendLine();

		if (ex.InnerException != null) {
			PrintStackTrace(msg, ex.InnerException);
		}
	}
}
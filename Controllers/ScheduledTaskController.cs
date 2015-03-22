using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Site.SiteCustom;


namespace Site.Controllers {
	public class ScheduledTaskController : Controller {

		public static DateTime GetNextDailyRunTime() {
			//Settings.RebuildCache();//clear incase changed in background // but this means it will reload from db every time... should only be changed via settings edit page
			var scheduledTaskLastDailyRunTime = Settings.All.ScheduledTaskLastDailyRunTime;
			if (scheduledTaskLastDailyRunTime == null) {
				return DateTime.Now.AddMinutes(10);
			} else {
				return scheduledTaskLastDailyRunTime.Value.Date.AddDays(1).AddHours(9);  // 9am tomorrow
			}
		}

		// update from last settings date soon after app pool restart 
		public static DateTime nextHourlyRunTime = DateTime.Now.AddMinutes(5);  // update soon after app pool restart 

		// update from last settings date soon after app pool restart 
		public static DateTime nextFrequentRunTime = DateTime.Now.AddMinutes(1);  // update soon after app pool restart 

		/// <summary>
		/// Note: you should add this line to site.master
		/// 
		/// 	<% ScheduledTaskController.Check(); %>
		/// 	
		/// or add an actual scheduled task	to call RunDailyTasks and/or RunHourlyTasks using curl, wget, some sort of vb script
		/// </summary>
		public static void Check() {
			Logging.dlog("ScheduledTask Check");
			var nextDailyRunTime = GetNextDailyRunTime();

			if (Web.Request["daily"] == "past") {
				Logging.dlog("daily in past");
				nextDailyRunTime = DateTime.Now.AddHours(-2);
			}
			if (Web.Request["hourly"] == "past") {
				Logging.dlog("hourly in past");
				nextHourlyRunTime = DateTime.Now.AddHours(-2);
			}

			if (Util.ServerIsLive || Web.Request["force"] == "allowcheck") {
				Logging.dlog("checking");
				if (DateTime.Now > nextDailyRunTime) {
					Logging.dlog("run daily async");
					Logging.dlog("next time set to nextDailyRunTime["+nextDailyRunTime+"]");
					Util.HttpGetAsync(Web.BaseUrl + "ScheduledTask/RunDailyTasks");
				}
				if (DateTime.Now > nextHourlyRunTime) {
					Logging.dlog("run hourly async");
					nextHourlyRunTime = DateTime.Now.AddHours(1);
					Logging.dlog("next time set to nextHourlyRunTime["+nextHourlyRunTime+"]");
					//Util.HttpGetAsync(Web.BaseUrl + "ScheduledTask/RunHourlyTasks");
				}
				if (DateTime.Now > nextFrequentRunTime && Util.ServerIsLive) {
					//Util.HttpGetAsync(Web.BaseUrl + "ScheduledTask/RunFrequentTasks");
					nextFrequentRunTime = DateTime.Now.AddSeconds(60);
				}
				
			} else {
				Logging.dlog("skip check");
			}
			Logging.dlog("ScheduledTask Check DONE");
		}
		
		public ActionResult Index() {
			Check();
			return Content("Check done. next hourly["+nextHourlyRunTime.FmtDateTime()+"] last daily["+Settings.All.ScheduledTaskLastDailyRunTime.FmtDateTime()+"] next daily["+GetNextDailyRunTime().FmtDateTime()+"]");
		}

		public ActionResult RunDailyTasks() {
			Logging.dlog("RunDailyTasks");

			Web.CacheClearAll();

			// if there is anything to do every day, do it here
#if AutocompletePhrase
				// re-do autocomplete phrases for expired content (assumes content tends to expire daily)
				AutocompletePhrase.AutocompletePhraseCleanup();
#endif

#if ModificationLog
			//daily remove old mod logs
			if (BewebData.GetValueInt(new Sql("select count(*) from sys.objects where name like ", "ModificationLogTemp".SqlizeLike()), 0) > 0) {
				// remove the temp table if it exists
				new Sql("drop table ModificationLogTemp").Execute();
			}
			var s = @"
select * into ModificationLogTemp from ModificationLog where UpdateDate > getdate()-90;
truncate table ModificationLog;
set identity_insert ModificationLog ON;
insert into ModificationLog ([ModificationLogID],[UpdateDate],[PersonID],[TableName],[RecordID],[ActionType],[UserName],[ChangeDescription],[RecordIDChar]) select [ModificationLogID],[UpdateDate],[PersonID],[TableName],[RecordID],[ActionType],[UserName],[ChangeDescription],[RecordIDChar] from ModificationLogTemp;
set identity_insert ModificationLog OFF;
drop table ModificationLogTemp";
			new Sql(s).Execute();
		
#endif
			Logging.dlog("RunDailyTasks start");
			//TASKS here
			//update the run time in the database
			Settings.All.ScheduledTaskLastDailyRunTime = DateTime.Now;
			Settings.All.Save();
			
			//SendEMail.SendDeadLetter("RunDailyTasks",30);
			Logging.dlog("RunDailyTasks done");
			return Content("OK");
		}

		public ActionResult RunHourlyTasks() {
			// if there is anything to do every hour, do it here
			Logging.dlog("RunHourlyTasks start");
			
			//tasks here
			
			//SendEMail.SendDeadLetter("RunHourlyTasks",4);
			Logging.dlog("RunHourlyTasks done");
			
			return Content("OK");
		}
		
		public ActionResult RunFrequentTasks() {
			
			//each minute, run this
			//Logging.dlog("RunFrequentTasks");  //commented out - too much logging!
			
			//tasks here
			
			//Logging.dlog("RunFrequentTasks done");
			return Content("OK");
		}

	}
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Site.SiteCustom;

namespace Site.Controllers {
	public class BlogController : ApplicationController {

		public ActionResult Index() {
			var data = new ViewModel();
			data.Init();
			return View("Blog", data);
		}
		public ActionResult BlogDetail(string page, string id) {
			var data = new DetailViewModel();
			data.Init();
			return View("BlogDetail", data);
		}
		public class DetailViewModel : PageTemplateViewModel {
			public string BlogTitle;
			public string BlogBody;
			public string BlogDate;
			public DataTable blogCommentList;
			public string Url;

			public void Init() {
				int blogid = Crypto.DecryptID(Web.Request["id"]);
				var blogSql = new Sql("select * from blog where blogid=", blogid, " and ispublished=1");
				var rec = blogSql.GetHashtable();
				BlogTitle = rec["Title"] + "";
				BlogBody = rec["BodyText"] + "";
				BlogDate = rec["DateAdded"] + "";
				Sql mainlist = new Sql("select top 25 * from blogcomment where blogid=", blogid, " and ispublished=1 order by dateadded desc");
				blogCommentList = mainlist.GetDataTable();

				Url = Web.Server.UrlEncode(Web.ResolveUrlFull("~") + "blogdetail.aspx?page=" + Web.Request["page"] + "&id=" + Web.Request["id"]);
				bool autopublish = false;
				if (Web.Request["go"] != null) {
					int id = (Web.Session["CurrentUserID"] + "").ToInt(-1);

					if (id > 0) { autopublish = true; }
					var sql = new Sql(@"insert into blogcomment(title,bodytext
				,[Company]
				,[FirstName]
				,[LastName]
				,[Email],CommentByPersonID, blogid, dateadded, ispublished) values(");
					sql.Add("",
						(Web.Request["title"] + "").SqlizeText(), ",",
						(Web.Request["body"] + "").SqlizeText(), ",");

					sql.Add("",
						(Web.Request["company"] + "").SqlizeText(), ",",
						(Web.Request["firstname"] + "").SqlizeText(), ",",
						(Web.Request["lastname"] + "").SqlizeText(), ",",
						(Web.Request["email"] + "").SqlizeText(), ",");

					sql.Add("",
						(id + "").SqlizeNumber(), ",",
						blogid.SqlizeNumber(), ", getdate(),", autopublish.SqlizeBool(), ");select @@identity;");

					decimal savedNewID = (sql).FetchDecimalOrZero();

					//send email

					//Response.Write("ok");
					string adminurl = Web.ResolveUrlFull("~/admin/") + "BlogCommentAdmin/EditEnc?encID=" + Crypto.EncryptID(savedNewID.ToInt());
					//string EmailToAddress = Util.GetSetting("BlogEmailToAddress");
					string EmailToAddress = Util.GetSetting("EmailToAddress");
					//fromemail = "website@dnserver.net.nz";
					//EmailToAddress = "suella@beweb.co.nz";
					string msg =
						"" +
						"Site Admin,\n" +
						"\n" +
						"	A new comment has been posted to your blog." +
						"\n" +
						"	Link to admin: " + adminurl + "" +
						"\n";

					if (!autopublish) {
						msg +=
							"	Note that you will to review the data and publish it if it has appropriate content." +
							"\n";
					} else {
						msg +=
							"	This was auto-published, as it was created by a logged in user." +
							"\n";
					}
					SendEMail.SimpleSendEmail(EmailToAddress, Util.GetSiteName()+" : New Blog Comment", msg);


					//reload page
					string url = Web.Request.RawUrl.ToString();
					//Response.Redirect(url + (url.Contains("&post") ? "" : "&post=1"));
					
				}
			}

		}
		public class ViewModel : PageTemplateViewModel {
			public DataTable blogList;
			public DataTable blogArchiveList;
			public DataTable NumBlogs;
			public void Init() {
				Sql mainlist = new Sql("select top 25 * from blog where ispublished=1 order by dateadded desc");
				if (Web.Request["d"] != null) {

					mainlist = new Sql();
					mainlist.SuppressQuoteChecking = true;
					var startDate = Convert.ToDateTime(Web.Request["d"] + "-1");
					var endDate = startDate.AddMonths(1).AddDays(-1);
					mainlist.Add("select top 25 * from blog where ispublished=1 AND DateAdded >= ", startDate, " AND DateAdded <= ", endDate, " order by dateadded desc");
					//Logging.dout(mainlist.ToString());
				}
				blogList = mainlist.GetDataTable();
				Sql archivelist = new Sql(@"SELECT TOP (100) PERCENT CONVERT(CHAR(5), DateAdded, 120) + CONVERT(CHAR(4), DateAdded, 100) AS ArchiveDate, COUNT(BlogID) AS NumBlogs FROM dbo.Blog WHERE (IsPublished = 1) and dateadded<getdate()-30");

				//var d = Web.Request["d"] + "";

				/*
				if(d!=""){
					archivelist.Add("AND ArchiveDate = ", d.SqlizeText);
				}*/

				archivelist.Add("GROUP BY CONVERT(CHAR(5), DateAdded, 120) + CONVERT(CHAR(4), DateAdded, 100) ORDER BY ArchiveDate DESC");

				blogArchiveList = archivelist.GetDataTable();

			}
		}
	}
}
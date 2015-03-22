using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Beweb;
using Models;
using Site.SiteCustom;

namespace Site.Controllers {
	public class XMLController : Controller {
		//
		// GET: /XML/

		[OutputCache(Duration = 60, VaryByParam = "*")]
		public ActionResult GetSitemapXML()
		{
			var sql = new Sql();
			var topTenSql = new Sql();
			var topTenRegionSql = new Sql();
			var xmlPrefix = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
			var xml = new HtmlTag("urlset");
			xml.Add("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
			xml.Add("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");
			xml.Add("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");

			// get pages
			topTenSql = new Sql("select top 10 * from Page where 1=1");
			topTenSql.AndIsActive<Models.Page>();
			var topTenDeals = PageList.Load(topTenSql);
		
//			var cats = CategoryList.LoadActive();
//			foreach (var cat in cats) {
//				AddSiteMapXmlLink(cat.GetUrl(), xml);
//			}
//
//			var attractions = AttractionList.LoadActive();
//			foreach (var attraction in attractions) {
//				AddSiteMapXmlLink(attraction.GetUrl(), xml);
//			}
			return Content(xmlPrefix + xml.ToString(), "text/xml");

		}

		private static void AddSiteMapXmlLink(string url, HtmlTag xml) {
			var urlTag = new HtmlTag("url");
			var locTag = new HtmlTag("loc");
			locTag.SetInnerText(Fmt.UTF8Encode(Web.ResolveUrlFull(url)));
			urlTag.AddTag(locTag);
			var changefreqTag = new HtmlTag("changefreq");
			changefreqTag.SetInnerText("daily");
			urlTag.AddTag(changefreqTag);
			var priorityTag = new HtmlTag("priority");
			priorityTag.SetInnerText("0.5");
			urlTag.AddTag(priorityTag);
			xml.AddTag(urlTag);
		}
	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Site.SiteCustom;

namespace Site.Controllers {
	public class MapController : ApplicationController {

		public ActionResult Index() {
			var data = new ViewModel();
			return View("Map", data);
		}

		public class ViewModel : PageTemplateViewModel {
		} 

		public ActionResult GetData() {
			var data = new Data();
			var mapLocations = MapLocationList.LoadActive();
			foreach (var loc in mapLocations) {
				Location location = new Location() { hoverText = loc.Title+" ("+loc.Dates+")",title = loc.Title.HtmlEncode(), location = loc.LocationName.HtmlEncode(), address = loc.LocationAddress.HtmlEncode(), latitude = loc.Latitude.Value, longitude = loc.Longitude.Value, dates=loc.Dates.HtmlEncode(), startTime=Fmt.Time(loc.StartTime), link=loc.LinkUrl, html = loc.MoreInfoTextHtml.FmtHtmlText(), regionID =loc.MapRegionID ?? 0, eventType=loc.EventType};
				data.locations.Add(location);
			}
			var regions = MapRegionList.Load(new Sql("where exists (select * from maplocation where maplocation.mapregionid=mapregion.mapregionid) order by mapregionID"));
			foreach (var reg in regions) {
				data.regions.Add(new Region(){regionID=reg.MapRegionID, title=reg.Title.HtmlEncode()});
			}
			var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
			string script = "var data = " + serializer.Serialize(data);
			return JavaScript(script);
		}

		public class Data {
			public List<Region> regions = new List<Region>();
			public List<Location> locations = new List<Location>();
		}

		public class Location {
			public string hoverText, title, location, address, dates, startTime;
			public double latitude, longitude;
			public string html, link;
			public int regionID;
			public string eventType;
		}

		public class Region {
			public int regionID;
			public string title;
		}

	}


}
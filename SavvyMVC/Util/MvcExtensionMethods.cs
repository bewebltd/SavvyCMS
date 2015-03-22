using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;

namespace SavvyMVC.Util {
	public static class MvcExtensionMethods {
		public static void RemoveRouteByName(this RouteCollection routes, string routeName) {
			if (routes[routeName] != null) {
				routes.Remove(RouteTable.Routes[routeName]);
			}
		}

		public static void InsertRouteAfter(this RouteCollection routes, string existingRouteName, RouteBase newRoute) {
			if (routes[existingRouteName] != null) {
				int index = routes.IndexOf(routes[existingRouteName]);
				routes.Insert(index, newRoute);
			} else {
				throw new Exception("InsertRouteAfter: Route not found [" + existingRouteName + "]");
			}
		}

		/// <summary>The render partial to string.</summary>
		/// <param name="controlName">The control name.</param>
		/// <param name="viewData">The view data.</param>
		/// <returns>The <see cref="string"/>.</returns>
		private static string RenderPartialToString(string controlName, object viewData) {
			var viewPage = new ViewPage { ViewContext = new ViewContext(), ViewData = new ViewDataDictionary(viewData) };

			viewPage.Controls.Add(viewPage.LoadControl(controlName));

			var sb = new StringBuilder();
			using (var sw = new StringWriter(sb)) {
				using (var tw = new HtmlTextWriter(sw)) {
					viewPage.RenderControl(tw);
				}
			}

			return sb.ToString();
		}


	}
}

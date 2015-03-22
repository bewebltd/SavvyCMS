using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Beweb;
using SavvyMVC.Util;

namespace SavvyMVC.Util {
	/// <summary>
	/// Useful functions for working with MVC projects.
	/// </summary>
	public static class MvcUtil {
		private static List<Type> _controllerClasses;
		private static List<Type> _siteControllerClasses;

		private static List<Type> GetAllControllerClasses() {
			if (_controllerClasses == null) {
				_controllerClasses = new List<Type>();
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
					// this is making the assumption that all assemblies we need are already loaded.
					try {
						foreach (Type type in assembly.GetTypes()) {
							if (type.IsSubclassOf(typeof(ControllerBase))) {
								// found a controller
								_controllerClasses.Add(type);
							}
						}
					} catch (System.Reflection.ReflectionTypeLoadException) {
						// dont care if it isnt loaded
					}
				}
			}
			return _controllerClasses;
		}

		public static bool ControllerExists(string controllerName) {
			foreach (Type controllerClass in GetAllControllerClasses()) {
				if (controllerClass.Name == controllerName || controllerClass.Name == controllerName + "Controller") {
					return true;
				}
			}
			return false;
		}

		private static List<Type> GetSiteControllerClasses() {
			if (_siteControllerClasses == null) {
				_siteControllerClasses = new List<Type>();
				foreach (Type controllerClass in GetAllControllerClasses()) {
					if (controllerClass.Namespace == SiteControllerNamespace) {
						_siteControllerClasses.Add(controllerClass);
					}
				}
			}
			return _siteControllerClasses;
		}
		/// <summary>
		/// use reflection to get routes to all controllers. This would be slow, but it's only called once 
		/// </summary>
		/// <returns></returns>
		private static List<string> GetSiteControllerNames() {
			List<string> result = new List<string>();
			foreach (Type controllerClass in GetSiteControllerClasses()) {
				result.Add(controllerClass.Name.RemoveSuffix("Controller"));
			}
			return result;
		}

		private static string SiteControllerNamespace {
			get { return "Site.Controllers"; }
		}

		public static string[] SiteControllerNamespaceArray {
			get { return new string[] { "Site.Controllers" }; }
		}

		public static RouteCollection Routes {
			get { return RouteTable.Routes; }
		}

		public static string GetSiteControllerNamesPattern() {

			return "(" + GetSiteControllerNames().Join("|") + ")";
		}

		///// <summary>
		///// Usage:
		///// result = MvcUtil.RenderViewToString<ViewModel>(this, "~/Views/Common/CellNotesSearchResults.ascx", model);
		///// </summary>
		///// <typeparam name="T"></typeparam>
		///// <param name="controller"></param>
		///// <param name="viewPath"></param>
		///// <param name="model"></param>
		///// <returns></returns>
		//public static string RenderViewToString<T>(Controller controller, string viewPath, T model) {
		//	controller.ViewData.Model = model;
		//	using (var writer = new StringWriter()) {
		//		var view = new WebFormView(viewPath);
		//		var vdd = new ViewDataDictionary<T>(model);
		//		var viewCxt = new ViewContext(controller.ControllerContext, view, vdd, new TempDataDictionary(), writer);
		//		viewCxt.View.Render(viewCxt, writer);
		//		return writer.ToString();
		//	}
		//}

		/// <summary>
		/// Usage:
		/// string html = MvcUtil.RenderViewToString(this, "~/Views/ShoppingCart/CustomFields.ascx", data);
		/// </summary>
		/// <param name="thisController"></param>
		/// <param name="viewName"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		public static string RenderViewToString(Controller thisController, string viewName, object model) {
			// assign the model of the controller from which this method was called to the instance of the passed controller (a new instance, by the way)
			thisController.ViewData.Model = model;

			// initialize a string builder
			using (StringWriter sw = new StringWriter()) {
				// find and load the view or partial view, pass it through the controller factory
				ControllerContext controllerContext = thisController.ControllerContext;

				if (controllerContext == null) {
					controllerContext = new ControllerContext(Web.Request.RequestContext, thisController);
				}

				ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
				ViewContext viewContext = new ViewContext(controllerContext, viewResult.View, thisController.ViewData, thisController.TempData, sw);

				// render it
				viewResult.View.Render(viewContext, sw);

				//return the razorized view/partial-view as a string
				return sw.ToString();
			}
		}

	}


}


namespace BewebTest {
	[TestClass]
	public class MvcUtilTest {
		[TestMethod]
		public void TestGetSiteControllerNamesPattern() {
			Web.Write(MvcUtil.GetSiteControllerNamesPattern());
			Assert.Pass();
		}
	}
}
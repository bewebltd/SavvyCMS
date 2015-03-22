using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using Beweb;

namespace SavvyMVC.Helpers {
	public static class UserControlHelpers {
		/// <summary>
		/// WebForms UserControl renderer Delegate
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="ControlToUse"></param>
		public delegate void InitializeControlDelegate<T>(T ControlToUse);

		/// <summary>
		/// WebForms UserControl renderer
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="ControlToUse"></param>		
		public static MvcHtmlString RenderUserControl<T>(this HtmlHelper htmlHelper, string ControlPath, InitializeControlDelegate<T> InitControlCallback) where T : UserControl {
			System.Web.UI.Page pageHolder = new Page();
			T ControlToRender = (T) pageHolder.LoadControl(ControlPath);
			pageHolder.Controls.Add(ControlToRender);
			InitControlCallback.Invoke(ControlToRender);

			StringWriter result = new StringWriter();
			System.Web.HttpContext.Current.Server.Execute(pageHolder, result, false);
			return MvcHtmlString.Create(result.ToString());
		}




	}
}

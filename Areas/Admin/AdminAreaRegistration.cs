using System.Web.Mvc;

namespace Site.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
			context.MapRoute("admin", "admin", new {controller = "AdminMenu", action = "RedirectToMenu"});
			context.MapRoute("admin_slash", "admin/", new {controller = "AdminMenu", action = "RedirectToMenu"});
            context.MapRoute("Admin_menu", "Admin/Menu", new { controller="AdminMenu", action = "Index" } );
            context.MapRoute("Admin_notfound", "Admin/NotFound", new { controller="AdminMenu", action = "NotFound" } );
            context.MapRoute("Admin_error", "Admin/ShowError", new { controller="AdminMenu", action = "ShowError" } );
         
			context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
		}
    }
}

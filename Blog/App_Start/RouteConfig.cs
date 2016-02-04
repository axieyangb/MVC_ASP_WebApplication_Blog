using System.Web.Mvc;
using System.Web.Routing;

namespace Blog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "Blog",
               url: "Article/{action}/{ArticleID}",
               defaults: new { controller = "Article", action = "Index", ArticleID = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "GalleryDefault",
               url: "Gallery/{pageIndex}/{pageSize}",
               defaults: new { controller = "Gallery", action = "Index", pageIndex = "0", pageSize = "12" }
            );
            routes.MapRoute(
                name: "DashBoard",
                url: "Admin/Dashboard/{action}/{parameters}",
                defaults: new { controller = "DashBoard", action = "Index", parameters = UrlParameter.Optional }
                );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
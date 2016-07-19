using System.Web.Mvc;
using System.Web.Routing;

namespace Poetry.Web
{
    /// <summary>
    /// MVC注册路由
    /// </summary>
    public class RouteConfig
    {

        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(name: "Default", url: "{controller}/{action}/{id}", defaults: new { controller = "WeChat", action = "Index", id = UrlParameter.Optional });
        }
    }
}
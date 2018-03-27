// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="zjzx">
//   ©2016 中教在线 版权所有
// </copyright>
// <summary>
//   Defines the HomeController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ICusCRM.Web.Portal
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// The route config.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// The register routes.
        /// </summary>
        /// <param name="routes">
        /// The routes. 
        /// </param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            // 注册后台访问路由
            //routes.MapRoute(
            //    name: "admin",
            //    url: "admin/",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //   namespaces: new[] { "ICusCRM.Web.Portal.Controllers" });

            // 默认路由注册
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "ICusCRM.Web.Portal.Controllers" });
        }
    }
}

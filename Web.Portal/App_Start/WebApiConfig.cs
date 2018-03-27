// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebApiConfig.cs" company="zjzx">
//   ©2016 中教在线 版权所有
// </copyright>
// <summary>
//   Defines the HomeController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ICusCRM.Web.Portal
{
    using System.Web.Http;

    /// <summary>
    /// The web api config.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi", 
                routeTemplate: "api/{controller}/{action}/{id}", 
                defaults: new { id = RouteParameter.Optional });
        }
    }
}

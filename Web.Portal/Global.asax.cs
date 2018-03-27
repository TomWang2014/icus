// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="zjzx">
//   2016-11-30 13:50:27
// </copyright>
// <summary>
//   Defines the HomeController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ICusCRM.Web.Portal
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using ICusCRM.Application;
    using ICusCRM.Infrastructure.Mvc;
    using ICusCRM.Infrastructure.Unity.Ioc;
    //using ICusCRM.Web.Portal.Toolkits;
    using System.Web.Http;
    using System.IO;
    using ICusCRM.Web.Portal.Toolkits;

    /// <summary>
    /// The mvc application.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// The application_ start.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);     

             //实现自定义的依赖注入控制器
            var container = IocManager.Instance.GetContainer();
            var factory = new UnityControllerFactory(container);
            ControllerBuilder.Current.SetControllerFactory(factory);

            // api 依赖注入
            var depentdeencyrFactory = new UnityControllerApiFactory(container);
            GlobalConfiguration.Configuration.DependencyResolver = depentdeencyrFactory;

            // 运行应用程序初始化操作
            AppInit.Run();
            ToolkitsHelper.InitAllFunc();

            // 启用EF性能调试工具
            HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();

            // 启用Log4Net 日志
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Server.MapPath("~" + @"/config/Log4Net.config")));
        }
    }
}
namespace ICusCRM.Web.Portal.Controllers
{
    using System.Web.Mvc;

    using ICusCRM.Application.SystemMgtServices;
    using ICusCRM.Web.Portal.Toolkits;

    /// <summary>
    /// The home controller.
    /// </summary>
    public class HomeController : AppAuthorizeController
    {
        /// <summary>
        /// 系统服务
        /// </summary>
        private readonly SystemServices systemServices;

        /// <summary>
        /// 够着函数
        /// </summary>
        /// <param name="systemServices">系统服务</param>
        public HomeController(SystemServices systemServices)
        {
            this.systemServices = systemServices;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns>系统首页</returns>
        public ActionResult Index()
        {
            ToolkitsHelper.ClientRouteInit(HttpContext, RouteData);
            ViewBag.userName = UserIdentity.CurrentUser.RealName;
            ViewBag.roleName = UserIdentity.CurrentUser.ToString();
            return View(UserIdentity.CurrentUser.FuncItems);
        }

        /// <summary>
        /// 管理员首页面板展示
        /// </summary>
        /// <returns>view</returns>
        public ActionResult MainPanel()
        {
            return this.View();
        }

        /// <summary>
        /// 租户首页面板展示
        /// </summary>
        /// <returns></returns>
        public ActionResult TenantMainPanel()
        {
            return View();
        }

    }
}
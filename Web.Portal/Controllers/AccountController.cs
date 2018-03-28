// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="zjzx">
//   ©2016 中教在线 版权所有
// </copyright>
// <summary>
//   Defines the HomeController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ICusCRM.Web.Portal.Controllers
{
    using System;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using ICusCRM.Application.SystemMgtServices;
    using ICusCRM.Infrastructure;
    using ICusCRM.Infrastructure.Entity;
    using ICusCRM.Infrastructure.Toolkit;
    using ICusCRM.Web.Portal.Toolkits;

    /// <summary>
    /// 账户相关控制器
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// The system services.
        /// </summary>
        private readonly SystemServices systemServices;



        /// <summary>
        /// 系统服务
        /// </summary>
        /// <param name="systemServices">系统管理访问</param>
        public AccountController(SystemServices systemServices)
        {
            this.systemServices = systemServices;
        }

        /// <summary>
        /// 系统用户登录
        /// </summary>
        /// <returns>返回系统用户登录页面</returns>
        public ActionResult Login()
        {
            ToolkitsHelper.ClientRouteInit(HttpContext, RouteData);
            return this.View();
        }

        /// <summary>
        /// 处理用户登录提交的数据
        /// </summary>
        /// <param name="account"> 用户账号</param>
        /// <param name="password"> 用户密码 </param>
        /// <param name="verCode">验证码</param>
        /// <returns> 返回登录成功后的页面 </returns>
        [HttpPost]
        public ActionResult Login(string account, string password, string verCode)
        {
            string msg;

            if (string.IsNullOrWhiteSpace(verCode))
            {
                this.ViewBag.msg = "请输入正确的验证码！";
                return this.View();
            }

            var code = this.TempData["vcode"] as string;
            if (!verCode.Equals(code))
            {
                this.ViewBag.msg = "请输入正确的验证码！";
                return this.View();
            }

            var user = this.systemServices.UserLogin(account, password, out msg);
            this.ViewBag.msg = msg;
            if (user == null)
            {
                return this.View();
            }


            try
            {
                var ip = HttpContext.Request.UserHostAddress;
                var browser = HttpContext.Request.Browser.Browser;
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex);
            }


            // 计算可访问的权限
            foreach (var sysFuncItem in user.FuncItems)
            {
                var roleName = ToolkitsHelper.InitAllFunc().FirstOrDefault(a => a.RoleName == sysFuncItem.ToString());
                if (roleName != null)
                {
                    user.AuthenticationUrl.AddRange(roleName.IncludeUrl);
                }

                if (sysFuncItem.SubSysFunc != null)
                {
                    foreach (var funcSmall in sysFuncItem.SubSysFunc)
                    {
                        var smallRoleName = ToolkitsHelper.InitAllFunc().FirstOrDefault(a => a.RoleName == funcSmall.ToString());
                        if (smallRoleName != null)
                        {
                            user.AuthenticationUrl.AddRange(smallRoleName.IncludeUrl);
                        }
                    }
                }
                
            }

            UserIdentity.CurrentUser = user;
            return this.Redirect("/home/index");
        }

        /// <summary>
        /// 用户退出登录
        /// </summary>
        /// <returns>返回登录页面</returns>
        public ActionResult LoginOut()
        {
            var limit = Request.Cookies.Count;
            for (var i = 0; i < limit; i++)
            {
                var httpCookie = this.Request.Cookies[i];
                if (httpCookie == null)
                {
                    continue;
                }

                var cookieName = httpCookie.Name;
                var aCookie = new HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(-1) };
                this.Response.Cookies.Add(aCookie);
            }

            Session.Abandon();
            return this.Redirect("/home/index");
        }

        /// <summary>
        /// 代理登录页面
        /// <remarks>为即将iframe嵌套登录bug，将需要登录的请求转发到此</remarks>
        /// </summary>
        /// <returns>返回登录页</returns>
        public ActionResult Index()
        {
            return View();
        }

    //    /// <summary>
    //    /// 查看当前系统配置文件中的权限列表
    //    /// </summary>
    //    /// <returns>
    //    /// The <see cref="ActionResult"/>.
    //    /// </returns>
    //    public ActionResult GetFileResult()
    //    {
    //        return Json(ToolkitsHelper.InitAllFunc(), JsonRequestBehavior.AllowGet);
    //    }

        /// <summary>
        /// 获得图片验证码
        /// </summary>
        public void GetImgVerificationCode()
        {
            // 生成随机验证码图
            string vcode;
            var basemap = RandomNumberHelper.GetValidateCodeMap(out vcode, 4);

            // 存储在tempdata并且返回图片格式
            this.TempData["vcode"] = vcode;
            basemap.Save(this.Response.OutputStream, ImageFormat.Gif);
            this.Response.End();
        }
    }
}
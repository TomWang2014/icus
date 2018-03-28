// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemController.cs" company="zjzx">
//   ©2015 zjzx 李天赐 版权所有
// </copyright>
// <summary>
//   系统设置
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ICusCRM.Web.Portal.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;


    using ICusCRM.Application.SystemMgtServices;
    using ICusCRM.Infrastructure.Dto;
    using ICusCRM.Infrastructure.Exceptions;
    using ICusCRM.Infrastructure.Mvc.Authorization;
    using ICusCRM.Infrastructure.Mvc.ModelBinder;
    using ICusCRM.Infrastructure.Toolkit;
    using ICusCRM.Web.Portal.Toolkits;

    /// <summary>
    /// 系统管理
    /// </summary>
    public class SystemController : AppAuthorizeController
    {
        /// <summary>
        /// 系统服务
        /// </summary>
        private readonly SystemServices systemServices;



        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="systemServices">
        /// 系统服务
        /// </param>
        /// <param name="orgMgtService">机构设置</param>
        public SystemController(SystemServices systemServices)
        {
            this.systemServices = systemServices;
        }

        //#region 用户管理相关

        /// <summary>
        /// 系统用户管理
        /// </summary>
        /// <returns>用户管理界面</returns>
        public ActionResult UserMgtResult()
        {
            this.ViewBag.UserType = UserIdentity.CurrentUser.ToString();
            return View();
        }

        ///// <summary>
        ///// 编辑用户信息
        ///// </summary>
        ///// <param name="model">
        ///// model
        ///// </param>
        //public void ModifyUserInfo([FromJson] UserItem model)
        //{
        //    if (model.Id > 0)
        //    {
        //        this.systemServices.UpdateUserInfo(model);
        //    }
        //    else
        //    {
        //        this.systemServices.AddUserInfo(model);
        //    }
        //}

        ///// <summary>
        ///// 删除用户信息
        ///// </summary>
        ///// <param name="id">
        ///// 用户Id
        ///// </param>
        //public void DeleteUser(int id)
        //{
        //    this.systemServices.DeleteUser(id);
        //}

        ///// <summary>
        ///// 批量删除
        ///// </summary>
        ///// <param name="ids">
        ///// ids
        ///// </param>
        //public void DeleteBatch([FromJson] List<int> ids)
        //{
        //    this.systemServices.DeleteUserBatch(ids);
        //}

        ///// <summary>
        ///// 获得用户列表
        ///// </summary>
        ///// <param name="model">
        ///// The model. 
        ///// </param>
        ///// <returns>
        ///// 返回所有的用户信息 
        ///// </returns>
        //public JsonResult GetALlUserList([FromJson] UserSearch model)
        //{
        //    var list = this.systemServices.GetALlUserList(model);
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 重置用户密码
        ///// </summary>
        ///// <param name="userId">用户Id</param>
        //public void ResetPassword(int userId)
        //{
        //    this.systemServices.ResetPassword(userId);
        //}

        //#endregion

        //#region 角色管理相关

        ///// <summary>
        ///// 系统角色管理
        ///// </summary>
        ///// <returns>角色管理界面</returns>
        //public ActionResult SysRoleMgtResult()
        //{
        //    return View();
        //}

        ///// <summary>
        ///// 获得角色列表
        ///// </summary>
        ///// <param name="roleName"> The role Name. </param>
        ///// <returns> 角色列表 </returns>
        //public JsonResult GetRoleList(string roleName = "")
        //{
        //    var list = this.systemServices.GetRoleList(roleName);
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 获得角色键值对列表
        ///// </summary>
        ///// <returns>返回key/val</returns>
        //public JsonResult GetRoleItemList()
        //{
        //    var list = this.systemServices.GetRoleItemList();
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 获得角色键值对列表
        ///// </summary>
        ///// <returns>返回key/val</returns>
        //public JsonResult GetRoleSelect2List()
        //{
        //    var list = this.systemServices.GetRoleItemList().Select(a => new EntitySelectDto { id = a.Value, text = a.DisplayText });
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 编辑角色信息
        ///// </summary>
        ///// <param name="model">
        ///// 模型
        ///// </param>
        ////public void ModifyRoleInfo([FromJson] AddRole model)
        ////{
        ////    if (model.Id == 0)
        ////    {
        ////        this.systemServices.AddRoleInfo(model);
        ////    }
        ////    else
        ////    {
        ////        this.systemServices.UpdateRoleInfo(model);
        ////    }
        ////}

        ///// <summary>
        ///// 删除用户信息
        ///// </summary>
        ///// <param name="id">
        ///// 角色Id
        ///// </param>
        ////public void DeleteRole(int id)
        ////{
        ////    this.systemServices.DeleteRole(id);
        ////}

        //#endregion

        //#region 权限管理相关

        ///// <summary>
        ///// 系统权限管理
        ///// </summary>
        ///// <returns>权限管理界面</returns>
        //public ActionResult SysFuncMgtResult()
        //{
        //    return View();
        //}

        ///// <summary>
        ///// 获得所有系统权限
        ///// </summary>
        ///// <returns>返回权限列表</returns>
        //public JsonResult GetAllFuncs()
        //{
        //    var list = this.systemServices.GetAllSysFuncs();
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 获得用户权限列表
        ///// </summary>
        ///// <returns>返回权限</returns>
        //public JsonResult GetFuncByUser()
        //{
        //    var list = this.systemServices.GetFuncByUser(UserIdentity.CurrentUser.Account);
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 获得系统所有权限树
        ///// </summary>
        ///// <returns>返回jstree</returns>
        //public JsonResult GetFuncTree()
        //{
        //    var result = new List<JsTreeNode>();
        //    var funcList = this.systemServices.GetAllSysFuncs();

        //    foreach (var sysFunc in funcList)
        //    {
        //        var node = new JsTreeNode
        //        {
        //            text = sysFunc.Name,
        //            id = sysFunc.Id.ToString(CultureInfo.InvariantCulture)
        //        };

        //        var children = sysFunc.NetSysFunc1.Select(subFunc => new JsTreeNode
        //        {
        //            text = subFunc.Name,
        //            id = subFunc.Id.ToString(CultureInfo.InvariantCulture)
        //        });

        //        node.children = children;
        //        result.Add(node);
        //    }

        //    return this.Json(result, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 获得系统所有非租户菜单的权限树
        ///// </summary>
        ///// <returns>返回jstree</returns>
        //public JsonResult GetAllNotTenantFuncsTree()
        //{
        //    var result = new List<JsTreeNode>();
        //    var funcList = this.systemServices.GetAllNotTenantFuncs();

        //    foreach (var sysFunc in funcList)
        //    {
        //        var node = new JsTreeNode
        //        {
        //            text = sysFunc.Name,
        //            id = sysFunc.Id.ToString(CultureInfo.InvariantCulture)
        //        };

        //        var children = sysFunc.NetSysFunc1.Where(a => a.IsTenantFunc == false).Select(subFunc => new JsTreeNode
        //        {
        //            text = subFunc.Name,
        //            id = subFunc.Id.ToString(CultureInfo.InvariantCulture)
        //        });

        //        node.children = children;
        //        result.Add(node);
        //    }

        //    return this.Json(result, JsonRequestBehavior.AllowGet);
        //}

        //#endregion

        //#region 租户管理
        ///// <summary>
        ///// 租户管理
        ///// </summary>
        ///// <returns>租户管理页面</returns>
        //public ActionResult SysTenantMgtResult()
        //{
        //    return this.View();
        //}

        ///// <summary>
        ///// 获得租户信息
        ///// </summary>
        ///// <param name="model">
        ///// The model. 
        ///// </param>
        ///// <returns>
        ///// 返回租户列表 
        ///// </returns>
        //public JsonResult GetTenantList([FromJson] TenantSearch model)
        //{
        //    var list = this.systemServices.GetTenantList(model);
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 编辑租户信息
        ///// </summary>
        ///// <param name="model">
        ///// 住户信息
        ///// </param>
        //public void ModifyTenantInfo([FromJson] TenantItem model)
        //{
        //    if (model.Id == 0)
        //    {
        //        this.systemServices.AddTenantInfo(model);
        //    }
        //    else
        //    {
        //        this.systemServices.UpdateTenantInfo(model);
        //    }
        //}

        ///// <summary>
        ///// 删除租户信息
        ///// </summary>
        ///// <param name="id">
        ///// 租户主键
        ///// </param>
        //public void DeleteTenantInfo(int id)
        //{
        //    this.systemServices.DeleteTenantInfo(id);
        //}

        ///// <summary>
        ///// 设置租户logo
        ///// </summary>
        ///// <returns>返回页面</returns>
        //public JsonResult SetTenantLogoOrIoc()
        //{
        //    Image image = null;
        //    try
        //    {
        //        var requst = this.HttpContext.Request;
        //        var imgFile = requst.Files["avatar_file"];
        //        var type = Convert.ToInt32(requst["type"]);
        //        var id = Convert.ToInt32(requst["id"]);

        //        if (requst == null || imgFile == null || imgFile.ContentLength == 0)
        //        {
        //            return Json(new { state = 201, message = "当前请求无效，请重试。" }, JsonRequestBehavior.AllowGet);
        //        }

        //        // 获取转码后的文件参数
        //        var item = JsonHelper.JsonStr2Obj<ImageNote>(requst["avatar_data"]);

        //        // 裁剪后的图片
        //        image = AvatarImgHelper.CropImage(Image.FromStream(imgFile.InputStream), (int)item.height, (int)item.width, (int)item.x, (int)item.y);

        //        // 保存图片
        //        const string Path = "/UploadFiles/UserPhoto/";

        //        var serverPath = this.Server.MapPath(Path);
        //        if (!Directory.Exists(serverPath))
        //        {
        //            Directory.CreateDirectory(serverPath);
        //        }

        //        var fileName = Guid.NewGuid() + "." + imgFile.FileName.Substring(imgFile.FileName.LastIndexOf(".", StringComparison.Ordinal) + 1);

        //        // io操作
        //        image.Save(serverPath + fileName, ImageFormat.Png);

        //        systemServices.SetTenantLogoOrIoc(id, type, Path + fileName);

        //        return Json(new { state = 200, message = "设置机构信息成功，刷新页面后生效。" }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        Infrastructure.Log.WriteLine(ex);
        //        return Json(new { state = 500, message = "上传失败：" + ex }, JsonRequestBehavior.AllowGet);
        //    }
        //    finally
        //    {
        //        if (image != null)
        //        {
        //            image.Dispose();
        //        }
        //    }
        //}

        //#endregion

        ///// <summary>
        ///// 项目配置
        ///// </summary>
        ///// <returns>返回view</returns>
        //public ActionResult ProjectSetting()
        //{
        //    return this.View();
        //}

        ///// <summary>
        ///// 获得项目设置的数据列表
        ///// </summary>
        ///// <param name="model">查询的model</param>
        ///// <returns>返回数据列表</returns>
        //public JsonResult GetAllProject([FromJson] SelectDto model)
        //{
        //    var list = this.orgMgtService.GetAllProjectSettingList(model);
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 取消推荐项目
        ///// </summary>
        ///// <param name="id">id</param>
        //public void Cancel(int id)
        //{
        //    this.orgMgtService.Cancel(id);
        //}

        ///// <summary>
        ///// 设置项目推荐封面图
        ///// </summary>
        ///// <param name="projectId"> 项目Id </param>
        //public void SetProjetHost(int? projectId)
        //{
        //    // 保存图片
        //    const string Path = "/UploadFiles/ProjectImg/";

        //    var serverPath = this.Server.MapPath(Path);
        //    if (!Directory.Exists(serverPath))
        //    {
        //        Directory.CreateDirectory(serverPath);
        //    }

        //    var file = System.Web.HttpContext.Current.Request.Files["fileImg"];

        //    // 项目封面必填
        //    if (file == null)
        //    {
        //        throw new UserFriendlyException("项目封面不能为空！");
        //    }

        //    if (!projectId.HasValue)
        //    {
        //        throw new UserFriendlyException("操作失败，请选择项目！");
        //    }

        //    var fileName = Guid.NewGuid() + "." + file.FileName.Substring(file.FileName.LastIndexOf(".", StringComparison.Ordinal) + 1);
        //    file.SaveAs(serverPath + fileName);

        //    this.orgMgtService.SetProjetHost(projectId, Path + fileName);
        //}

        ///// <summary>
        ///// 修改密码
        ///// </summary>
        ///// <returns>view</returns>
        //[Anonymous]
        //public ActionResult UserInfo()
        //{
        //    return this.View();
        //}


        ///// <summary>
        ///// xiugai
        ///// </summary>
        ///// <param name="oldPwd">旧密码</param>
        ///// <param name="newPwd">新密码</param>
        ///// <param name="newPwd2">重新输入密码</param>
        //[Anonymous]
        //public void UpdataPassword(string oldPwd, string newPwd, string newPwd2)
        //{
        //    if (string.IsNullOrEmpty(oldPwd) || string.IsNullOrEmpty(newPwd) || string.IsNullOrEmpty(newPwd2))
        //    {
        //        throw new UserFriendlyException("请输入完整的信息后提交！");
        //    }

        //    if (!newPwd.Equals(newPwd2))
        //    {
        //        throw new UserFriendlyException("两次密码输入不一致！");
        //    }

        //    this.systemServices.UpdatePassword(oldPwd, newPwd);
        //}


        /// <summary>
        /// 刷新session
        /// </summary>
        [AnonymousAttribute]
        public void RefreshSession()
        {
            var obj = UserIdentity.CurrentUser.Account;
        }
    }
}
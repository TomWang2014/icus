// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemServices.cs" company="zjzx">
//   2016-11-30 15:45:14
// </copyright>
// <author>李天赐</author>
// <date>2016/11/8 16:46:55</date>
// <summary>
//   主要功能有：
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ICusCRM.Application.SystemMgtServices
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;

    using ICusCRM.Domain;
    using ICusCRM.Domain.Systems;
    using ICusCRM.Infrastructure.AutoMapper;
    using ICusCRM.Infrastructure.Data;
    using ICusCRM.Infrastructure.Data.Specification;
    using ICusCRM.Infrastructure.Entity;
    using ICusCRM.Infrastructure.Exceptions;
    using ICusCRM.Infrastructure.Mvc.Authorization;
    using ICusCRM.Infrastructure.Runtime.Session;
    using ICusCRM.Infrastructure.Toolkit;
    using ICusCRM.Infrastructure.Unity.Aop;
    using ICusCRM.Application.SystemMgtServices.Dtos;

    /// <summary>
    /// 系统设置服务
    /// </summary>
    public class SystemServices : InterceptiveObject
    {
        /// <summary>
        /// 权限仓储
        /// </summary>
        private readonly IFuncsRepository funcsRepository;

        /// <summary>
        /// 角色仓储
        /// </summary>
        private readonly IRolesRepository rolesRepository;

        /// <summary>
        /// 用户仓储
        /// </summary>
        private readonly IUsersRepository usersRepository;



        /// <summary>
        /// 数据库工作单元
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// 课程够着函数
        /// </summary>
        /// <param name="funcsRepository"> 权限仓促 </param>
        /// <param name="unitOfWork"> 数据仓促工作单元 </param>
        /// <param name="rolesRepository"> 角色仓储 </param>
        /// <param name="usersRepository"> 用户仓储 </param>
        /// <param name="sysTenantRepository"> 租户仓储 </param>
        /// <param name="sysTenantIpWhiteRepository"> 租户白名单 </param>
        /// <param name="sysTenantSettingRepository"> The sys Tenant Setting Repository. </param>
        public SystemServices(
            IFuncsRepository funcsRepository,
            IUnitOfWork unitOfWork,
            IRolesRepository rolesRepository,
            IUsersRepository usersRepository)
        {
            this.funcsRepository = funcsRepository;
            this.unitOfWork = unitOfWork;
            this.rolesRepository = rolesRepository;
            this.usersRepository = usersRepository;

        }

        #region 用户相关操作

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="account">
        /// 账号 
        /// </param>
        /// <param name="password">
        /// 密码 
        /// </param>
        /// <param name="errorMsg">
        /// 消息 
        /// </param>
        /// <returns>
        /// 结果 
        /// </returns>
        public CurrentUserDto UserLogin(string account, string password, out string errorMsg)
        {
            errorMsg = string.Empty;
            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(password))
            {
                errorMsg = "登录账号和密码不能为空。";
                return null;
            }

            var users = this.usersRepository.GetFilteredElements(a => a.Account == account && a.IsDelete == 0).ToList();
            if (users.Count == 0)
            {
                errorMsg = "登录失败，不存在该用户。";
                return null;
            }

            foreach (var sysUser in users)
            {
                var md5Pwd = Hasher.GetMd5Hash(password).ToUpper();
                if (sysUser.Password.ToUpper() == md5Pwd)
                {
                    // 登录成功
                    var temp = sysUser.MapTo<CurrentUserDto>();
                    temp.FuncItems = this.GetFuncByUser(sysUser.Account);

                    // admin账号特殊处理，拥有全部权限
                    // if (sysUser.Account.ToLower() == "admin")
                    // {
                    // temp.FuncItems = this.GetAllNotTenantFuncs();
                    // }
                    // temp.IsTenant = CheckUserIsTenant(temp.Id);
                    return temp;
                }
            }

            errorMsg = "登录失败，账号或者密码不匹配。";
            return null;
        }

        /// <summary>
        /// 新增用户信息
        /// </summary>
        /// <param name="model">
        /// 模型
        /// </param>
        //public void AddUserInfo(UserItem model)
        //{
        //    var exitUser = this.usersRepository.GetFilteredElements(a => a.Account == model.Account && a.IsDeleted == false)
        //            .ToList();
        //    if (exitUser.Count > 0)
        //    {
        //        throw new UserFriendlyException("该账号已经存在，请重试！");
        //    }

        //    // 补全信息
        //    model.CreationTime = DateTime.Now.ToDateTimeString();
        //    model.Number = "US" + RandomNumberHelper.GetRandomNumber();

        //    var sysuser = model.MapTo<NetSysUser>();
        //    sysuser.Password = Hasher.GetMd5Hash("123456");
        //    sysuser.Type = (int)SysUserType.SysUser;

        //    this.usersRepository.Add(sysuser);
        //    this.unitOfWork.Commit();
        //}

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="userId">用户Id</param>
        public void ResetPassword(int userId)
        {
            // TODO 参数的安全性
            var model = this.usersRepository.GetModel(userId);
            model.Password = Hasher.GetMd5Hash("123456");

            this.usersRepository.Modify(model);
            this.unitOfWork.Commit();
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="model">
        /// 模型
        /// </param>
        //public void UpdateUserInfo(UserItem model)
        //{
        //    var exitUser =
        //        this.usersRepository.GetFilteredElements(
        //            a => a.Account == model.Account && a.IsDeleted == false && a.Id != model.Id).ToList();
        //    if (exitUser.Count > 0)
        //    {
        //        throw new UserFriendlyException("该账号已经存在，请重试！");
        //    }

        //    var user = this.usersRepository.GetModel(model.Id);

        //    model.MapTo(user);

        //    this.usersRepository.Modify(user);

        //    this.unitOfWork.Commit();
        //}

        /// <summary>
        /// 获得用户列表
        /// </summary>
        /// <param name="searchDto">
        /// The search Dto. 
        /// </param>
        /// <returns>
        /// 用户列表  
        /// </returns>
        //public PagedResult<UserItem> GetALlUserList(UserSearch searchDto)
        //{
        //    Expression<Func<NetSysUser, bool>> filter = p => p.IsDeleted == false && p.Type == (int)SysUserType.SysUser;

        //    if (!string.IsNullOrEmpty(searchDto.Number))
        //    {
        //        filter = filter.And(p => p.Number.Contains(searchDto.Number.Trim()));
        //    }

        //    if (!string.IsNullOrEmpty(searchDto.Name))
        //    {
        //        filter = filter.And(p => p.Name.Contains(searchDto.Name.Trim()));
        //    }

        //    if (!string.IsNullOrEmpty(searchDto.Account))
        //    {
        //        filter = filter.And(p => p.Account.Contains(searchDto.Account.Trim()));
        //    }

        //    var list = this.usersRepository.GetFilteredPageResult<NetSysUser>(
        //        filter,
        //        searchDto.PageIndex,
        //        searchDto.PageSize,
        //        searchDto.Orderby,
        //        searchDto.Desc);

        //    return list.MapTo<PagedResult<UserItem>>();
        //}

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">
        /// ids
        /// </param>
        public void DeleteUserBatch(List<int> ids)
        {
            foreach (var id in ids)
            {
                DeleteUser(id);
            }
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id">
        /// 用户Id
        /// </param>
        public void DeleteUser(int id)
        {
            var model = this.usersRepository.GetModel(id);
            this.usersRepository.Delete(model);

            this.unitOfWork.Commit();
        }

        #endregion

        #region 角色操作相关

        /// <summary>
        /// 获得角色列表
        /// </summary>
        /// <param name="roleName"> The role Name. </param>
        /// <returns> 角色列表 </returns>
        //public List<RoleItem> GetRoleList(string roleName)
        //{
        //    Expression<Func<Roles, bool>> filter = p => p.IsDelete == 0;

        //    if (!string.IsNullOrEmpty(roleName))
        //    {
        //        filter = filter.And(p => p.Name.Contains(roleName.Trim()));
        //    }

        //    var list =
        //        this.rolesRepository.GetFilteredElements(filter).OrderByDescending(a => a.CreateTime).ToList();
        //    return list.MapTo<List<RoleItem>>();
        //}

        ///// <summary>
        ///// 新增角色
        ///// </summary>
        ///// <param name="model">
        ///// 模型
        ///// </param>
        //public void AddRoleInfo(AddRole model)
        //{
        //    // 检查是否同名
        //    var exitRole =
        //        this.rolesRepository.GetFilteredElements(a => a.Name == model.Name && a.IsDeleted == false).ToList();
        //    if (exitRole.Count > 0)
        //    {
        //        throw new UserFriendlyException("该角色名称已经存在，请重试！");
        //    }

        //    var role = new Roles
        //                   {
        //                       CreationTime = DateTime.Now,
        //                       IsDeleted = false,
        //                       Name = model.Name,
        //                       Type = (int)SysRoleType.SysCommon,
        //                       Number = "RS" + RandomNumberHelper.GetRandomNumber()
        //                   };

        //    // 角色赋值
        //    foreach (var sysFunc in model.SysFuncIds.Select(func => this.funcsRepository.GetModel(func)))
        //    {
        //        role.NetSysFuncs.Add(sysFunc);
        //    }

        //    this.rolesRepository.Add(role);
        //    this.unitOfWork.Commit();
        //}

        ///// <summary>
        ///// 编辑角色
        ///// </summary>
        ///// <param name="model">
        ///// 模型
        ///// </param>
        //public void UpdateRoleInfo(AddRole model)
        //{
        //    var exitRole =
        //        this.rolesRepository.GetFilteredElements(
        //            a => a.Name == model.Name && a.IsDeleted == false && a.Id != model.Id).ToList();
        //    if (exitRole.Count > 0)
        //    {
        //        throw new UserFriendlyException("该角色名称已经存在，请重试！");
        //    }

        //    var role = this.rolesRepository.GetModel(model.Id);
        //    role.Name = model.Name;

        //    role.NetSysFuncs.Clear();
        //    foreach (var sysFunc in model.SysFuncIds.Select(func => this.funcsRepository.GetModel(func)))
        //    {
        //        role.NetSysFuncs.Add(sysFunc);
        //    }

        //    role.Type = (int)SysRoleType.SysCommon;
        //    this.rolesRepository.Modify(role);
        //    this.unitOfWork.Commit();
        //}

        ///// <summary>
        ///// 删除角色
        ///// </summary>
        ///// <param name="id">
        ///// id
        ///// </param>
        //public void DeleteRole(int id)
        //{
        //    var model = this.rolesRepository.GetModel(id);

        //    if (model.NetSysUsers.Count > 0)
        //    {
        //        throw new UserFriendlyException("该角色下有用户，不能删除！");
        //    }

        //    this.rolesRepository.Delete(model);

        //    this.unitOfWork.Commit();
        //}

        ///// <summary>
        ///// 获得角色键值对列表
        ///// </summary>
        ///// <returns>返回key/val</returns>
        //public List<ComboboxItemDto> GetRoleItemList()
        //{
        //    var list = this.rolesRepository.GetFilteredElements(a => a.IsDeleted == false).Where(a => a.Type == (int)SysRoleType.SysCommon)
        //            .Select(c => new ComboboxItemDto { DisplayText = c.Name, Value = c.Id.ToString(CultureInfo.InvariantCulture) })
        //            .ToList();

        //    return list;
        //}

        //#endregion

        //#region 权限操作相关

        ///// <summary>
        ///// 获得所有权限列表
        ///// </summary>
        ///// <returns>权限集合</returns>
        //public List<SysFuncItem> GetAllSysFuncs()
        //{
        //    var list = this.funcsRepository.GetAll().Where(a => a.FuncType == 1).OrderBy(a => a.OrderNumber).ToList();
        //    return list.MapTo<List<SysFuncItem>>();
        //}

        ///// <summary>
        ///// 获得所有非租户菜单权限列表
        ///// </summary>
        ///// <returns>返回所有非租户的权限列表集合</returns>
        //public List<SysFuncItem> GetAllNotTenantFuncs()
        //{
        //    var list = this.funcsRepository.GetAll().Where(a => a.FuncType == 1 && a.IsTenantFunc == false).OrderBy(a => a.OrderNumber).ToList();
        //    return list.MapTo<List<SysFuncItem>>();
        //}

        /// <summary>
        /// 获得用户权限列表
        /// </summary>
        /// <param name="account">
        /// 用户账号
        /// </param>
        /// <returns>
        /// 返回权限 
        /// </returns>
        public List<FuncsItem> GetFuncByUser(string account)
        {
            var list = new List<FuncsItem>();
            var user = this.usersRepository.GetFilteredElements(a => a.Account == account).FirstOrDefault();

            if (user == null)
            {
                throw new UserFriendlyException(account + "账号未能查找到用户信息。");
            }

            // 用户没有任何角色
            if (user.Roles == null)
            {
                return list;
            }

            var allFunc = user.Roles.Funcs.MapTo<List<FuncsItem>>();

            // 该用户实际拥有的权限idj集合
            var allFuncIds = allFunc.Select(a => a.Id).ToList();

            // 只过滤一级菜单
            foreach (var f in allFunc.Where(a => a.ParentFuncId == 0 && a.IsDisplay == true))
            {
                var model = f;
                if (f.SysFunc1 != null)
                {
                    var sonFuns = f.SysFunc1.ToList();

                    model.SysFunc1.Clear();
                    foreach (var func in sonFuns.Where(func => allFuncIds.Contains(func.Id) && func.IsDisplay == true))
                    {
                        model.SysFunc1.Add(func);
                    }
                }
                

                list.Add(model);
            }

            return list;
        }

        //#endregion

        //#region 租户管理相关

        ///// <summary>
        ///// 获取前端合作机构列表
        ///// </summary>
        ///// <returns>TenantItem</returns>
        //public List<TenantItem> GetMainTenantList()
        //{
        //    Expression<Func<NetTenant, bool>> filter = p => p.IsDelete == false;
        //    var list = this.sysTenantRepository.GetFilteredElements(filter).OrderBy(a => a.CreateTime).ToList();
        //    return list.MapTo<List<TenantItem>>();
        //}

        ///// <summary>
        ///// 获取前端合作机构详情
        ///// </summary>
        ///// <param name="id">Id</param>
        ///// <returns>TenantItem</returns>
        //public TenantItem GetMainTenantInfo(int id)
        //{
        //    var model = this.sysTenantRepository.GetModel(id);
        //    return model.MapTo<TenantItem>();
        //}

        ///// <summary>
        ///// 新增租户信息
        ///// </summary>
        ///// <param name="addTenant">
        ///// 住户信息
        ///// </param>
        //public void AddTenantInfo(TenantItem addTenant)
        //{
        //    var tenantRole =
        //        this.rolesRepository.GetFilteredElements(a => a.Type == (int)SysRoleType.SysTenant).FirstOrDefault();
        //    if (tenantRole == null)
        //    {
        //        throw new UserFriendlyException("新增租户失败，系统中没有租户对应的角色信息！");
        //    }

        //    if (this.usersRepository.GetFilteredElements(a => a.IsDeleted == false && a.Name == addTenant.Name).FirstOrDefault() != null)
        //    {
        //        throw new UserFriendlyException("新增租户失败，系统已经存在同名的用户信息！");
        //    }

        //    // 新增租户
        //    var userModle = new NetSysUser
        //      {
        //          Account = addTenant.Account.Trim(),
        //          CreationTime = DateTime.Now,
        //          Number = "US" + RandomNumberHelper.GetRandomNumber(),
        //          Password = Hasher.GetMd5Hash("123456"),
        //          SysRoleId = tenantRole.Id,
        //          Type = (int)SysUserType.SysTenant,
        //          IsDeleted = false,
        //          Name = addTenant.Name
        //      };

        //    // 新增租户
        //    var tenantModel = new NetTenant
        //       {
        //           IsDelete = false,
        //           CreateTime = DateTime.Now,
        //           CreateUser = 1,
        //           AesKey = Guid.NewGuid().ToString().Replace("-","").Substring(0,16),//addTenant.AesKey,
        //           Gateway = addTenant.Gateway,
        //           UserId = userModle.Id,
        //           Number = "TS" + RandomNumberHelper.GetRandomNumber(),
        //           NetSysUser = userModle,
        //           TokenUrl = addTenant.TokenUrl
        //       };

        //    sysTenantRepository.Add(tenantModel);
        //    this.unitOfWork.Commit();
        //}

        ///// <summary>
        ///// 获得租户列表
        ///// </summary>
        ///// <param name="model">
        ///// The model. 
        ///// </param>
        ///// <returns>
        ///// 获得列表 
        ///// </returns>
        //public PagedResult<TenantItem> GetTenantList(TenantSearch model)
        //{
        //    Expression<Func<NetTenant, bool>> filter = p => p.IsDelete == false;

        //    if (!string.IsNullOrEmpty(model.Number))
        //    {
        //        filter = filter.And(p => p.Number.Contains(model.Number.Trim()));
        //    }

        //    if (!string.IsNullOrEmpty(model.Name))
        //    {
        //        filter = filter.And(p => p.NetSysUser.Name.Contains(model.Name.Trim()));
        //    }

        //    if (!string.IsNullOrEmpty(model.Account))
        //    {
        //        filter = filter.And(p => p.NetSysUser.Account.Contains(model.Account.Trim()));
        //    }

        //    var list = this.sysTenantRepository.GetFilteredPageResult(
        //        filter,
        //        model.PageIndex,
        //        model.PageSize,
        //        a => a.CreateTime,
        //        false);
        //    return list.MapTo<PagedResult<TenantItem>>();
        //}

        ///// <summary>
        ///// 获得租户信息
        ///// </summary>
        ///// <param name="id"> The id. </param>
        ///// <returns> 租户模型 </returns>
        //public TenantItem GetTenantByUserId(int id)
        //{
        //    var model = this.sysTenantRepository.GetFilteredElements(a => a.UserId == id).FirstOrDefault();
        //    if (model == null)
        //    {
        //        throw new UserFriendlyException("当前用户Id为" + id + "的租户信息无效！");
        //    }

        //    return model.MapTo<TenantItem>();
        //}

        ///// <summary>
        ///// 获得租户信息
        ///// </summary>
        ///// <param name="id"> The id. </param>
        ///// <returns> 租户模型 </returns>
        //public TenantItem GetTenantById(int id)
        //{
        //    var model = this.sysTenantRepository.GetModel(id);
        //    if (model == null)
        //    {
        //        throw new UserFriendlyException("当前Id为" + id + "的租户信息无效！");
        //    }

        //    return model.MapTo<TenantItem>();
        //}

        ///// <summary>
        ///// 新增租户信息
        ///// </summary>
        ///// <param name="model">
        ///// 住户信息
        ///// </param>
        //public void UpdateTenantInfo(TenantItem model)
        //{
        //    var tenantRole =
        //    this.rolesRepository.GetFilteredElements(a => a.Type == (int)SysRoleType.SysTenant).FirstOrDefault();
        //    if (tenantRole == null)
        //    {
        //        throw new UserFriendlyException("新增租户失败，系统中没有租户对应的角色信息！");
        //    }

        //    var sysTenant = this.sysTenantRepository.GetModel(model.Id);

        //    sysTenant.OperTime = DateTime.Now;
        //    sysTenant.Gateway = model.Gateway;
        //    sysTenant.Descriptions = model.Descriptions;
        //    sysTenant.TokenUrl = model.TokenUrl;

        //    var sysuser = sysTenant.NetSysUser;
        //    sysuser.Name = model.Name;
        //    sysuser.SysRoleId = tenantRole.Id;
        //    sysuser.Type = (int)SysUserType.SysTenant;

        //    this.sysTenantRepository.Modify(sysTenant);
        //    this.unitOfWork.Commit();
        //}

        ///// <summary>
        ///// 更新用户log和ioc
        ///// </summary>
        ///// <param name="id">
        ///// 租户id
        ///// </param>
        ///// <param name="type">
        ///// 更新类型1设置logo 2设置图标
        ///// </param>
        ///// <param name="path">
        ///// 文件路径
        ///// </param>
        //public void SetTenantLogoOrIoc(int id, int type, string path)
        //{
        //    var model = this.sysTenantRepository.GetModel(id);

        //    if (type == 1)
        //    {
        //        model.Logo = path;
        //    }
        //    else
        //    {
        //        model.FrontImage = path;
        //    }

        //    this.unitOfWork.Commit();
        //}

        ///// <summary>
        ///// 判定当前登录用户是否是租户类型的用户
        ///// </summary>
        ///// <param name="userId">用户uId</param>
        ///// <returns>是否是租户</returns>
        //public bool CheckUserIsTenant(int userId)
        //{
        //    var tenant = this.sysTenantRepository.GetFilteredElements(a => a.UserId == userId).FirstOrDefault();
        //    return tenant != null;
        //}

        //#endregion

        //#region 机构设置

        ///// <summary>
        ///// 删除租户信息
        ///// </summary>
        ///// <param name="id">
        ///// id
        ///// </param>
        //public void DeleteTenantInfo(int id)
        //{
        //    var model = this.sysTenantRepository.GetModel(id);

        //    // 删除租户的账号
        //    var user = this.usersRepository.GetModel(model.UserId);
        //    this.usersRepository.Remove(user);

        //    // 删除租户信息
        //    this.sysTenantRepository.Remove(model);

        //    this.unitOfWork.Commit();
        //}

        ///// <summary>
        ///// 修改密码
        ///// </summary>
        ///// <param name="oldPwd">
        ///// 旧密码
        ///// </param>
        ///// <param name="newPwd">
        ///// 新密码
        ///// </param>
        //public void UpdatePassword(string oldPwd, string newPwd)
        //{
        //    var model = this.usersRepository.GetModel(AppUser.Id);

        //    if (!Hasher.GetMd5Hash(oldPwd).Equals(model.Password))
        //    {
        //        throw new UserFriendlyException("修改失败，旧密码不正确！");
        //    }

        //    model.Password = Hasher.GetMd5Hash(newPwd);

        //    this.usersRepository.Modify(model);
        //    this.unitOfWork.Commit();
        //}

        ///// <summary>
        ///// 设置白名单
        ///// </summary>
        ///// <param name="one">
        ///// 第一个
        ///// </param>
        ///// <param name="two">
        ///// 第二个
        ///// </param>
        ///// <param name="three">
        ///// 第三个
        ///// </param>
        ///// <param name="tokenUrl">
        ///// 回调地址
        ///// </param>
        //public void SaveIpWhite(string one, string two, string three, string tokenUrl)
        //{
        //    IPAddress ip;

        //    var model = this.sysTenantRepository.GetFilteredElements(a => a.UserId == AppUser.Id).FirstOrDefault();
        //    if (model == null)
        //    {
        //        throw new UserFriendlyException("当前用户Id为" + AppUser.Id + "的租户信息无效！");
        //    }

        //    foreach (var item in model.NetTenantIpWhites.ToList())
        //    {
        //        this.sysTenantIpWhiteRepository.Remove(item);
        //    }

        //    model.NetTenantIpWhites.Clear();

        //    var iplist = new List<NetTenantIpWhite>();

        //    if (!string.IsNullOrEmpty(one))
        //    {
        //        if (!StringUrlExtension.IsValidIp(one))
        //        {
        //            throw new UserFriendlyException("新增失败，ip地址" + one + "无效！");
        //        }

        //        iplist.Add(new NetTenantIpWhite { IpAddress = one, NetTenant = model });
        //    }

        //    if (!string.IsNullOrEmpty(two))
        //    {
        //        if (!StringUrlExtension.IsValidIp(two))
        //        {
        //            throw new UserFriendlyException("新增失败，ip地址" + one + "无效！");
        //        }

        //        iplist.Add(new NetTenantIpWhite { IpAddress = two, NetTenant = model });
        //    }

        //    if (!string.IsNullOrEmpty(three))
        //    {
        //        if (!StringUrlExtension.IsValidIp(three))
        //        {
        //            throw new UserFriendlyException("新增失败，ip地址" + one + "无效！");
        //        }

        //        iplist.Add(new NetTenantIpWhite { IpAddress = three, NetTenant = model });
        //    }

        //    model.NetTenantIpWhites = iplist;
        //    model.TokenUrl = tokenUrl;
        //    this.sysTenantRepository.Modify(model);

        //    this.unitOfWork.Commit();
        //}

        #endregion




    }

}

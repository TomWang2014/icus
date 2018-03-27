//------------------------------------------------------------
// <copyright file="TenantItem.cs" company="zjzx">
//    ©2015 中教在线 版权所有
// </copyright>
// <author>李天赐</author>
// <date>2016/11/16 17:16:00</date>
// <summary>
//  主要功能有：
//  
// </summary>
//------------------------------------------------------------

namespace ICusCRM.Application.SystemMgtServices.Dtos
{
    using System.Collections.Generic;

    using ICusCRM.Domain;
    using ICusCRM.Infrastructure.AutoMapper;
    using ICusCRM.Infrastructure.Dto;

    /// <summary>
    /// TenantItem
    /// </summary>
    [AutoMap(typeof(NetTenant))]
    public class TenantItem : EntityDto
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TenantItem()
        {
            this.NetTenantIpWhites = new List<IpNode> { new IpNode(), new IpNode(), new IpNode() };
        }

        /// <summary>
        /// 账号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string NetSysUserAccount { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string NetSysUserNetSysRoleName { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string NetSysUserName { get; set; }

        /// <summary>
        /// 租户图标
        /// </summary>
        public string FrontImage { get; set; }

        /// <summary>
        /// 租户logo
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Descriptions { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public int NetSysUserNetSysRoleId { get; set; }

        /// <summary>
        /// 租户对应UserId
        /// </summary>
        public int NetSysUserId { get; set; }

        /// <summary>
        /// Gets or sets the aes key.
        /// </summary>
        public string AesKey { get; set; }

        /// <summary>
        /// Gets or sets the gateway.
        /// </summary>
        public string Gateway { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 白名单
        /// </summary>
        public List<IpNode> NetTenantIpWhites { get; set; }

        /// <summary>
        /// 回调地址
        /// </summary>
        public string TokenUrl { get; set; }
    }
}

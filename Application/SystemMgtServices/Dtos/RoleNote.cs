//------------------------------------------------------------
// <copyright file="RoleNote.cs" company="zjzx">
//    ©2015 中教在线 版权所有
// </copyright>
// <author>李天赐</author>
// <date>2016/11/15 17:44:10</date>
// <summary>
//  主要功能有：
//  
// </summary>
//------------------------------------------------------------

namespace ICusCRM.Application.SystemMgtServices.Dtos
{
    using System;

    using ICusCRM.Domain;
    using ICusCRM.Infrastructure.AutoMapper;
    using ICusCRM.Infrastructure.Dto;

    /// <summary>
    /// RoleNote
    /// </summary>
    [Serializable]
    [AutoMap(typeof(Roles))]
    public class RoleNote : EntityDto
    {
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }
    }
}

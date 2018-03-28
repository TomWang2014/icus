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

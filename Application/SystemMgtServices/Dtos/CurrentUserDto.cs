namespace ICusCRM.Application.SystemMgtServices.Dtos
{
    using System;
    using System.Collections.Generic;

    using ICusCRM.Domain;
    using ICusCRM.Infrastructure.AutoMapper;
    using ICusCRM.Infrastructure.Dto;

    /// <summary>
    /// 当前登录用户
    /// </summary>
    [Serializable]
    [AutoMap(typeof(Users))]
    public class CurrentUserDto : EntityDto<int>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CurrentUserDto()
        {
            this.FuncItems = new List<FuncsItem>();
            this.AuthenticationUrl = new List<string>();
        }

        public string Account { get; set; }
        public string Phone { get; set; }

        public string RealName { get; set; }
        public Nullable<long> CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string IdentityCard { get; set; }
        public string BankCard { get; set; }
        public string BankName { get; set; }
        public Nullable<long> UsedTimes { get; set; }
        public Nullable<long> LastTimes { get; set; }

        /// <summary>
        /// 用户所属权限
        /// </summary>
        public RoleNote role { get; set; }

        /// <summary>
        /// 用户权限
        /// </summary>
        public List<FuncsItem> FuncItems { get; set; }

        /// <summary>
        /// 认证通过的Url
        /// </summary>
        public List<string> AuthenticationUrl { get; set; }


        /// <summary>
        /// 获得角色名称
        /// </summary>
        /// <returns>角色名称</returns>
        public override string ToString()
        {
            return this.role == null ? string.Empty : this.role.Name;
        }
    }
}

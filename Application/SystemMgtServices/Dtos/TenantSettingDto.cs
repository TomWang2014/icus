using ICusCRM.Domain;
using ICusCRM.Infrastructure.AutoMapper;
using ICusCRM.Infrastructure.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICusCRM.Application.SystemMgtServices.Dtos
{
    [AutoMap(typeof(NetTenantSetting))]
    public class TenantSettingDto : EntityDto
    {
        /// <summary>
        /// 租户Logo
        /// </summary>
        public string Logo { get; set; }

        public int TenantId { get; set; }
    }
}

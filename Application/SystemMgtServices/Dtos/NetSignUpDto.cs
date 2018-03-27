using ICusCRM.Domain;
using ICusCRM.Infrastructure.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICusCRM.Application.SystemMgtServices.Dtos
{
    [AutoMap(typeof(NetStudentProject))]
    public class NetSignUpDto
    {
        public int Id { get; set; }
        public Nullable<int> StuId { get; set; }
        public Nullable<int> ProjectId { get; set; }

        /// <summary>
        /// 合作机构名称
        /// </summary>
        public string NetStudentNetTenantNetSysUserName { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 学员报名人数
        /// </summary>
        public int SignUpNumbers { get; set; }
    }
}

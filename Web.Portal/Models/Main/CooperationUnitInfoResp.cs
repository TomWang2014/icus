using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICusCRM.Web.Portal.Models.Main
{
    /// <summary>
    /// 合作机构详情
    /// </summary>
    public class CooperationUnitInfoResp
    {
        /// <summary>
        /// 机构详情
        /// </summary>
        public TenantItem TenanetInfo { get; set; }

        /// <summary>
        /// 项目集合
        /// </summary>
        public List<ProjectList> ProjectList { get; set; }
    }
}
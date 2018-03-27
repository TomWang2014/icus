using ICusCRM.Application.OrgMgtServices.Dtos;
using ICusCRM.Infrastructure.Entity.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICusCRM.Web.Portal.Models.Main
{
    /// <summary>
    /// 首页数据
    /// </summary>
    public class IndexDataResp
    {
        /// <summary>
        /// 热门推荐的项目
        /// </summary>
        public List<ProjectHotDto> ProjectHotList { get; set; }

        /// <summary>
        /// 网络培训动态类别名称
        /// </summary>
        public string WLPXDtypeName { get; set; }

        /// <summary>
        /// 网络培训动态
        /// </summary>
        public List<ToNewsInfo> WLPXDTNewList { get; set; }

        /// <summary>
        /// 网络培训政策类别名称
        /// </summary>
        public string WLPXZCypeName { get; set; }

        /// <summary>
        /// 网络培训政策
        /// </summary>
        public List<ToNewsInfo> WLPXZCNewList { get; set; }

        /// <summary>
        /// 干部培训推荐课程
        /// </summary>
        public List<MainProject> OfficialProjects { get; set; }

        /// <summary>
        /// 干部培训最新课程
        /// </summary>
        public List<ProjectList> OfficialNewProjects { get; set; }

        /// <summary>
        /// 技能培训推荐课程
        /// </summary>
        public List<MainProject> TechnologyProjects { get; set; }

        /// <summary>
        /// 技能培训最新课程
        /// </summary>
        public List<ProjectList> TechnologyNewProjects { get; set; }

        /// <summary>
        /// 企业培训推荐课程
        /// </summary>
        public List<ProjectList> CompanyProjects { get; set; }

        /// <summary>
        /// 企业培训最新课程
        /// </summary>
        public List<ProjectList> CompanyNewProjects { get; set; }

    }

    public class ToNewsInfo
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string CreateTime { get; set; }

        public string newsTypeName { get; set; }
    }

}
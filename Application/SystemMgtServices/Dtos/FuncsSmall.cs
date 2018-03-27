// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuncSmall.cs" company="zjzx">
//   2016-11-10 09:37:44
// </copyright>
// <summary>
//   The func small.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ICusCRM.Application.SystemMgtServices.Dtos
{
    using System;

    using ICusCRM.Domain;
    using ICusCRM.Infrastructure.AutoMapper;
    using ICusCRM.Infrastructure.Toolkit;

    /// <summary>
    /// The func small.
    /// </summary>
    [Serializable]
    [AutoMap(typeof(Funcs))]
    public class FuncsSmall
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 访问Action名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 访问Controller名称
        /// </summary>
        public string ControllerName { get; set; }


        /// <summary>
        /// 获得完整路径
        /// </summary>
        /// <returns>返回url</returns>
        public override string ToString()
        {
            return StringUrlExtension.GetRequestUrlByParameter(this.AreaName, this.ControllerName, this.ActionName);
        }

        public bool IsDisplay { get; set; }
    }
}
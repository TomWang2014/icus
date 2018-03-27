// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IpNode.cs" company="zjzx">
//   2016-11-30 10:24:11
// </copyright>
// <author>李天赐</author>
// <date>2016/11/30 10:15:04</date>
// <summary>
//   主要功能有：
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ICusCRM.Application.SystemMgtServices.Dtos
{
    using ICusCRM.Domain;
    using ICusCRM.Infrastructure.AutoMapper;
    using ICusCRM.Infrastructure.Dto;

    /// <summary>
    /// IpNode
    /// </summary>
    [AutoMap(typeof(NetTenantIpWhite))]
    public class IpNode : EntityDto
    {
        /// <summary>
        /// Gets or sets the tenant id.
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        public string IpAddress { get; set; }
    }
}

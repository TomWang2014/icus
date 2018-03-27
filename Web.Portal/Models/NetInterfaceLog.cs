using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICusCRM.Web.Portal.Models
{
    /// <summary>
    /// 日志类
    /// </summary>
    public class NetInterfaceLog
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int _id { get; set; }
        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectCode { get; set; }
        /// <summary>
        /// 解密串
        /// </summary>
        public string Decryption { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Explain { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
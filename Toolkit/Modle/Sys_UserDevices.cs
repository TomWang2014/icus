/*****************************************************************************************************
 * 命名空间：YangMei.Toolkit.Modle
 * 版权归zjzx所有
 * 类名称：Sys_UserDevices
 * 文件名：Sys_UserDevices
 * 创建时间：2016/7/11 15:42:06
 * 创建人：zw
 * 创建说明：用户设备信息表
*****************************************************************************************************/

using System;

namespace YangMei.Toolkit.Modle
{
    public class Sys_UserDevices
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 平台(Android or Ios)
        /// </summary>
        public string Platform { get; set; }
        /// <summary>
        /// 机型
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// 屏幕大小
        /// </summary>
        public string ScreenSize { get; set; }
        /// <summary>
        /// 设备唯一值
        /// </summary>
        public string IMEI { get; set; }
        /// <summary>
        /// IOS设备位置值
        /// </summary>
        public string Mac { get; set; }
        /// <summary>
        /// 客户端连接ID(推送时用)
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }
    }
}

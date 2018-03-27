/*****************************************************************************************************
 * 命名空间：YangMei.Toolkit.Modle
 * 版权归zjzx所有
 * 类名称：Sys_Parameter
 * 文件名：Sys_Parameter
 * 创建时间：2016/7/11 15:58:15
 * 创建人：zw
 * 创建说明：配置表
*****************************************************************************************************/

namespace YangMei.Toolkit.Modle
{
    public class Sys_Parameter
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 租户ID
        /// </summary>
        public int TenantId { get; set; }
        /// <summary>
        /// 组
        /// </summary>
        public string Itemgroup { get; set; }
        /// <summary>
        /// 项
        /// </summary>
        public string Item { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

    }
}

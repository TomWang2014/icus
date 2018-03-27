/*****************************************************************************************************
 * 命名空间：YangMei.Toolkit.Modle
 * 版权归zjzx所有
 * 类名称：Res_GroupConfig
 * 文件名：Res_GroupConfig
 * 创建时间：2016/7/11 15:49:36
 * 创建人：zw
 * 创建说明：首页课程分类表
*****************************************************************************************************/

namespace YangMei.Toolkit.Modle
{
    public class Res_GroupConfig
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
        /// 课程ID
        /// </summary>
        public int ItemId { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Img { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNum { get; set; }

    }
}

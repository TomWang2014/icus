﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace YangMei.Toolkit.Modle
{
    using System;

    public partial class Res_CoursePlan
    {
        public int Id { get; set; }
        public int FromId { get; set; }
        public int UserId { get; set; }
        public string CourseIds { get; set; }
        public string BookIds { get; set; }
        public System.DateTime CreateTime { get; set; }
        public int CreateUser { get; set; }
        public int Status { get; set; }
        public int IsDelete { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public int ComeFrom { get; set; }
    }
}

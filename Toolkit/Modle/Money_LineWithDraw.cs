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

    public partial class Money_LineWithDraw
    {
        public int ID { get; set; }
        public Nullable<int> DeptId { get; set; }
        public Nullable<decimal> SumMoney { get; set; }
        public string WithDrawName { get; set; }
        public Nullable<System.DateTime> WithDrawTime { get; set; }
        public Nullable<int> Status { get; set; }
        public string RemitCode { get; set; }
        public string RemitName { get; set; }
        public Nullable<System.DateTime> RemitTime { get; set; }
        public Nullable<int> CreateUserID { get; set; }
        public string Bank { get; set; }
        public Nullable<int> TenantId { get; set; }
        public string Mark { get; set; }
        public string WithDrawUserName { get; set; }
        public Nullable<int> ApproverId { get; set; }
        public Nullable<System.DateTime> ApproveTime { get; set; }
    }
}

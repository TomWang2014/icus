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

    public partial class Dept_DealRecord
    {
        public int ID { get; set; }
        public int DeptId { get; set; }
        public int TenantId { get; set; }
        public int DealType { get; set; }
        public int DealWay { get; set; }
        public Nullable<decimal> DealMoney { get; set; }
        public Nullable<int> OperatorId { get; set; }
        public string OperatorRealName { get; set; }
        public string OperatorUserName { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> OperateTime { get; set; }
        public string Mark { get; set; }
        public Nullable<int> TableId { get; set; }
        public string AlipayTradeNo { get; set; }
    }
}
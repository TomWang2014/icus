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

    public partial class Res_BookBuy
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string BookBuyCode { get; set; }
        public Nullable<int> CreateUserID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> ComeFrom { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<System.DateTime> SendDate { get; set; }
        public string OrderNumber { get; set; }
        public string Company { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ZIP { get; set; }
        public string Memo { get; set; }
        public int BuyUserId { get; set; }
        public Nullable<int> DeptId { get; set; }
        public int ComeFromId { get; set; }
        public Nullable<int> IsInvoice { get; set; }
        public string InvoiceTitle { get; set; }
    }
}

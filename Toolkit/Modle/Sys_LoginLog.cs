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

    public partial class Sys_LoginLog
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public int TenantId { get; set; }
        public System.DateTime LoginTime { get; set; }
        public Nullable<System.DateTime> LogoutTime { get; set; }
        public string ClientIp { get; set; }
    }
}
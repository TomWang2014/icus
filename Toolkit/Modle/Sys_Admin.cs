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

    public partial class Sys_Admin
    {
        public int AdminId { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string Realname { get; set; }
        public int IsDelete { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
        public string LastLoginIp { get; set; }
        public int LoginFailureCount { get; set; }
        public string Icon { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> Sex { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Photo { get; set; }
    }
}

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

    public partial class Sys_Permission
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string AreaName { get; set; }
        public int ParentId { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public int Sort { get; set; }
        public int PermissionType { get; set; }
        public int Status { get; set; }
        public Nullable<int> UserType { get; set; }
    }
}
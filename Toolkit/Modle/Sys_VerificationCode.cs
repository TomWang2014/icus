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

    public partial class Sys_VerificationCode
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> IsVerification { get; set; }
        public string VerificationCode { get; set; }
        public Nullable<System.DateTime> VerificationTime { get; set; }
        public Nullable<int> Type { get; set; }
    }
}

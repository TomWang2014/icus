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
    public partial class Sys_Log
    {
        public int LogId { get; set; }
        public System.DateTime LogTime { get; set; }
        public int Opener { get; set; }
        public string RequestUrl { get; set; }
        public string RequestData { get; set; }
        public int OperatorType { get; set; }
        public string Description { get; set; }
        public string ClientIp { get; set; }
        public string ClientType { get; set; }
        public string TableDesc { get; set; }
        public string DataId { get; set; }
        public string TableName { get; set; }
        public string ModuleName { get; set; }
        public int TenantId { get; set; }
    }
}
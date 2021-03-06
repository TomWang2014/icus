//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ICusCRM.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            this.BalanceLog = new HashSet<BalanceLog>();
            this.CallBatchToCustomer = new HashSet<CallBatchToCustomer>();
            this.Orders = new HashSet<Orders>();
            this.Orders1 = new HashSet<Orders>();
            this.BusinessToCustomers = new HashSet<BusinessToCustomers>();
            this.Commission = new HashSet<Commission>();
        }
    
        public long Id { get; set; }
        public string Account { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int type { get; set; }
        public string RealName { get; set; }
        public Nullable<long> CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string IdentityCard { get; set; }
        public string BankCard { get; set; }
        public string BankName { get; set; }
        public Nullable<long> UsedTimes { get; set; }
        public Nullable<long> LastTimes { get; set; }
        public Nullable<int> RoleId { get; set; }
        public int IsDelete { get; set; }
        public Nullable<long> CreatorId { get; set; }
        public System.DateTime CreateTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BalanceLog> BalanceLog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CallBatchToCustomer> CallBatchToCustomer { get; set; }
        public virtual Company Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders> Orders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders> Orders1 { get; set; }
        public virtual Roles Roles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BusinessToCustomers> BusinessToCustomers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Commission> Commission { get; set; }
    }
}

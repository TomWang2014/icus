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
    
    public partial class Orders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Orders()
        {
            this.BalanceLog = new HashSet<BalanceLog>();
            this.Commission = new HashSet<Commission>();
        }
    
        public long Id { get; set; }
        public long BId { get; set; }
        public Nullable<long> CompanyId { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> BuyNumbers { get; set; }
        public Nullable<long> SellerId { get; set; }
        public Nullable<double> Percentage { get; set; }
        public Nullable<System.DateTime> PayTime { get; set; }
        public Nullable<int> IsPay { get; set; }
        public Nullable<int> Status { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string DealImg { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BalanceLog> BalanceLog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Commission> Commission { get; set; }
        public virtual Users Users { get; set; }
        public virtual Users Users1 { get; set; }
    }
}

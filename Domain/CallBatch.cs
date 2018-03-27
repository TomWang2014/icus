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
    
    public partial class CallBatch
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CallBatch()
        {
            this.CallBatchToCustomer = new HashSet<CallBatchToCustomer>();
        }
    
        public long BatchId { get; set; }
        public string BatchName { get; set; }
        public Nullable<int> CustomerNumbers { get; set; }
        public Nullable<int> CallLeader { get; set; }
        public Nullable<int> CalledNumbers { get; set; }
        public Nullable<int> Creator { get; set; }
        public System.DateTime CreateTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CallBatchToCustomer> CallBatchToCustomer { get; set; }
    }
}

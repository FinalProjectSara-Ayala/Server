//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dal
{
    using System;
    using System.Collections.Generic;
    
    public partial class workHour
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public workHour()
        {
            this.schedulingTbls = new HashSet<schedulingTbl>();
        }
    
        public int id { get; set; }
        public int inspector_id { get; set; }
        public int dayWork { get; set; }
        public System.TimeSpan start_shift { get; set; }
        public System.TimeSpan stop_shift { get; set; }
    
        public virtual inspectorTbl inspectorTbl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<schedulingTbl> schedulingTbls { get; set; }
    }
}

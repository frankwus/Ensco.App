//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ensco.Moc.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Task
    {
        public int Id { get; set; }
        public int WorkOrderId { get; set; }
        public string TaskNumber { get; set; }
        public int AdmGroup { get; set; }
        public int AdmSystem { get; set; }
        public int AdmSubSystem { get; set; }
        public bool EamsCriticalitySC { get; set; }
        public bool EamsCriticalityCC { get; set; }
        public bool EamsCriticalityEC { get; set; }
        public bool EamsCriticalityNC { get; set; }
        public bool EamsCriticalityOC { get; set; }
    
        public virtual WorkOrder WorkOrder { get; set; }
    }
}

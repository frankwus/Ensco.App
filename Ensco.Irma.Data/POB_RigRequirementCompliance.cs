//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ensco.Irma.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class POB_RigRequirementCompliance
    {
        public int Id { get; set; }
        public int PobId { get; set; }
        public int RequirementId { get; set; }
        public Nullable<int> SubRequirementId { get; set; }
        public Nullable<bool> Required { get; set; }
        public Nullable<bool> EvidenceRequired { get; set; }
        public Nullable<int> RecordId { get; set; }
        public Nullable<int> BypassedBy { get; set; }
        public Nullable<System.DateTime> BypassedDate { get; set; }
        public string SiteId { get; set; }
    }
}

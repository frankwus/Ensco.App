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
    
    public partial class Vw_OApChecklists
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ControlNumber { get; set; }
        public bool Randomize { get; set; }
        public int OapTypeId { get; set; }
        public Nullable<int> OapSubTypeId { get; set; }
        public string OapType { get; set; }
        public string OapSubType { get; set; }
        public System.DateTime StartDateTime { get; set; }
        public System.DateTime EndDateTime { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public System.DateTime UpdatedDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public string SiteId { get; set; }
    }
}

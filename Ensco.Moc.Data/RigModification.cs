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
    
    public partial class RigModification
    {
        public int Id { get; set; }
        public Nullable<int> MocId { get; set; }
        public string ReaName { get; set; }
        public string ReaHyperLink { get; set; }
        public bool MinorModification { get; set; }
        public int ReasonForModification { get; set; }
        public int DisciplineLead { get; set; }
        public Nullable<int> AttachmentId { get; set; }
    
        public virtual Moc Moc { get; set; }
    }
}
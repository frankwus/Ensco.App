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
    
    public partial class DeviationFromStandard
    {
        public int Id { get; set; }
        public int MocId { get; set; }
        public string Name { get; set; }
        public string ControlNumber { get; set; }
        public int Deviation { get; set; }
        public string SubSection { get; set; }
    
        public virtual Moc Moc { get; set; }
    }
}

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
    
    public partial class SWA
    {
        public int id { get; set; }
        public Nullable<int> Drill { get; set; }
        public Nullable<int> LocationId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> JobId { get; set; }
        public Nullable<System.DateTime> DateDone { get; set; }
        public string JobTitle { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Electronic { get; set; }
        public string Comment { get; set; }
    }
}

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
    
    public partial class vw_JobLookup
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string JobCriticality { get; set; }
        public string JobCategory { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string Location { get; set; }
        public string Department { get; set; }
    }
}
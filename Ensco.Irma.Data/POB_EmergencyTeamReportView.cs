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
    
    public partial class POB_EmergencyTeamReportView
    {
        public int TeamId { get; set; }
        public int MemberId { get; set; }
        public string Team { get; set; }
        public string Position { get; set; }
        public int Required { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public Nullable<int> Status { get; set; }
        public System.DateTime LastUpdated { get; set; }
    }
}
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
    
    public partial class vw_OpenJob
    {
        public int Id { get; set; }
        public Nullable<int> JobId { get; set; }
        public int PermitId { get; set; }
        public Nullable<int> PassportId { get; set; }
        public string Title { get; set; }
        public string CW { get; set; }
        public string HW { get; set; }
        public string CSE { get; set; }
        public string EI { get; set; }
        public string GT { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string Location { get; set; }
        public string PermitAuthority { get; set; }
        public string Status { get; set; }
    }
}

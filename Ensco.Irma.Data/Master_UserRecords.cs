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
    
    public partial class Master_UserRecords
    {
        public int Id { get; set; }
        public int PassportId { get; set; }
        public string Name { get; set; }
        public int GovtIdType { get; set; }
        public int GovtIdSubTypeId { get; set; }
        public string Number { get; set; }
        public string IssuedBy { get; set; }
        public Nullable<int> Country { get; set; }
        public string DocumentFile { get; set; }
        public System.DateTime DateIssued { get; set; }
        public System.DateTime DateExpires { get; set; }
        public System.DateTime DateCreated { get; set; }
        public int Status { get; set; }
        public Nullable<int> ValidatedBy { get; set; }
        public Nullable<System.DateTime> ValidatedDate { get; set; }
        public string Notes { get; set; }
        public string SiteId { get; set; }
    }
}

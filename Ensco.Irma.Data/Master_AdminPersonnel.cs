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
    
    public partial class Master_AdminPersonnel
    {
        public int Id { get; set; }
        public Nullable<int> PassportId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public Nullable<int> BuId { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public string SiteId { get; set; }
    }
}
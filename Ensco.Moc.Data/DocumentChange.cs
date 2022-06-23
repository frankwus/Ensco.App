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
    
    public partial class DocumentChange
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DocumentChange()
        {
            this.Documents = new HashSet<Document>();
        }
    
        public int Id { get; set; }
        public int MocId { get; set; }
        public Nullable<int> ChangeLevel { get; set; }
        public Nullable<int> Target { get; set; }
        public Nullable<int> Criticality { get; set; }
    
        public virtual Moc Moc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Document> Documents { get; set; }
    }
}
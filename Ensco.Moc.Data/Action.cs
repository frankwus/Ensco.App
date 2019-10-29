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
    
    public partial class Action
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Action()
        {
            this.ActionNotifications = new HashSet<ActionNotification>();
            this.ActionReviews = new HashSet<ActionReview>();
        }
    
        public int Id { get; set; }
        public int MocId { get; set; }
        public int Type { get; set; }
        public int Sequence { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public Nullable<int> Assignor { get; set; }
        public int Assignee { get; set; }
        public System.DateTime DateAssigned { get; set; }
        public System.DateTime DateDue { get; set; }
        public Nullable<System.DateTime> DateCompleted { get; set; }
        public int Criticality { get; set; }
        public int ControlHeirarchy { get; set; }
        public bool RequireReview { get; set; }
        public string ActionDescription { get; set; }
        public string CompletionNotes { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ActionNotification> ActionNotifications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ActionReview> ActionReviews { get; set; }
        public virtual Moc Moc { get; set; }
    }
}

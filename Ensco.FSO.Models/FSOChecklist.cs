using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ensco.Irma.Services.JobConstants;
using Ensco.Irma.Services;
using Ensco.Utilities;

namespace Ensco.FSO.Models
{
    public class FSOChecklist
    {

        public int ChecklistId { get; set; }
        [Utilities.Column(LookupList = "Location")]
        public Nullable<int> LocationId { get; set; }
        public Nullable<System.DateTime> DateTimeObserved { get; set; }        
        public string Status { get; set; }
        public Nullable<int> JobId { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public string SafeActsObserved { get; set; }
        public string UnsafeActsObserved { get; set; }
        public string TimeSpentOnObservation { get; set; }
        public string NumberOfPeopleContacted { get; set; }
        public string NumberOfPeopleObserved { get; set; }
        public string LeadObserverPassport { get; set; }
        public Nullable<System.DateTime> LeadObserverSignDate { get; set; }
        public string LeadObserverComments { get; set; }
        public string OIMPassport { get; set; }
        public Nullable<System.DateTime> OIMSignDate { get; set; }
        public string OIMComments { get; set; }

        public List<FSOChecklistObserver> Observers { get; set; }


        [NotMapped]
        public bool IsNew { get { return this.ChecklistId == 0; } }

    }
}

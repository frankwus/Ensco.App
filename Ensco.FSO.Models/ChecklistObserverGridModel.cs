using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ensco.Utilities;

namespace Ensco.FSO.Models
{
    public class ChecklistObserverGridModel
    {
        public List<ChecklistObserverGridRowModel> Observers { get; set; }

        public ChecklistObserverGridModel()
        {
            Observers = new List<ChecklistObserverGridRowModel>();
        }
    }

    public class ChecklistObserverGridRowModel
    {
        [Column(Key = true, Visible = false)]
        public int Id { get; set; }

        [Column(LookupList = "Passport")]
        public int PassportId { get; set; }        

        [Column(DisplayName = "Lead Observer")]
        public Nullable<bool> LeadObserver { get; set; }

        [Column(LookupList = "Company")]
        public Nullable<int> Company { get; set; }

        [Column(LookupList = "Position")]
        public Nullable<int> Position { get; set; }

        [Column(LookupList = "Tour")]
        public Nullable<int> Tour { get; set; }

        public ChecklistObserverGridRowModel()
        {
            LeadObserver = false;
        }
    }
    
}

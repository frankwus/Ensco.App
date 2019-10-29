using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.FSO.Models
{
    public class FSOChecklistObserver
    {
        public int Id { get; set; }
        public string OberserverPassport { get; set; }
        public string ObserverPosition { get; set; }
        public Nullable<int> ObserverCompanyId { get; set; }
        public Nullable<int> TourId { get; set; }
    }
}

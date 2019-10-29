using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.FSO.Models
{
    public class TrainedObserversViewModel
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public int DaysOnboard { get; set; }
        [Column(Visible = false)]
        public bool? IsInCompliance { get; set; }
        public int Observations { get; set; }

        [Column(DisplayName = "Last Observation")]
        public DateTime? LastObservation { get; set; }
    }
}

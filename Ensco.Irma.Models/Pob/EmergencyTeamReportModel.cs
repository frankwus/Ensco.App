using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class EmergencyTeamReportModel
    {
        public int TeamId { get; set; }
        public int MemberId { get; set; }
        public string Team { get; set; }
        public string Position { get; set; }
        public int Required { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public System.DateTime LastUpdated { get; set; }
        public Nullable<int> Status { get; set; }
        public byte[] ImageUrlYes { get; set; }
        public byte[] ImageUrlNo { get; set; }
        public EmergencyTeamReportModel()
        {

        }
    }
}

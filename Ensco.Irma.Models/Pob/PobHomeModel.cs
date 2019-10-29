using Ensco.Models;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class PobHomeModel
    {
        public int RigId { get; set; }
        public List<PobSummaryModel> PobSummary { get; set; }
        public DataTableModel PobCurrent { get; set; }
        public PobHomeModel()
        {
            RigId = UtilitySystem.Settings.RigId;
            PobSummary = new List<PobSummaryModel>();
        }
    }
}

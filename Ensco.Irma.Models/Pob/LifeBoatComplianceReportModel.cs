using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class LifeBoatComplianceReportModel
    {
        [Column(Visible = false, Key =true)]
        public Nullable<long> Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> Maximum { get; set; }
        public Nullable<int> ActualAssigned { get; set; }
        public Nullable<int> Available { get; set; }
    }
}

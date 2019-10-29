using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Moc.Models
{
    public class MocType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool RiskAssessment { get; set; }
    }
}

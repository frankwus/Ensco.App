using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class RigSubRequirementsModel
    {
        [Column(Visible = false, Key = true)]
        public int Id { get; set; }
        [Column(Visible = false)]
        public int Requirement { get; set; }
        [Column(LookupList ="UserGovtIdSubType")]
        public int SubRequirement { get; set; }
        public bool EvidenceRequired { get; set; }
        public RigSubRequirementsModel()
        {

        }
    }
}

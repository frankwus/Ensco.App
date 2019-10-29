using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class RigRequirementsComplianceModel
    {
        [Column(Visible =false, Key =true)]
        public int Id { get; set; }
        [Column(LookupList = "RigPersonnel")]
        public int PobId { get; set; }
        [Column(LookupList = "UserGovtIdType")]
        public int RequirementId { get; set; }
        [Column(LookupList = "UserGovtIdSubType")]
        public Nullable<int> SubRequirementId { get; set; }
        public Nullable<bool> Required { get; set; }
        public Nullable<bool> EvidenceRequired { get; set; }
        public Nullable<int> RecordId { get; set; }
        [Column(LookupList = "Passport")]
        public Nullable<int> BypassedBy { get; set; }
        public Nullable<System.DateTime> BypassedDate { get; set; }
    }
}

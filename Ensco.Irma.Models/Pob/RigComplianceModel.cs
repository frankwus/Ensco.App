using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class RigComplianceModel
    {
        [Column(Visible = false, Key = true)]
        public int Id { get; set; }
        [Column(Visible = false)]
        public int DocId { get; set; }
        [Column(LookupList= "UserGovtIdType")]
        public int Requirement { get; set; }
        [Column(LookupList = "UserGovtIdSubType")]
        public int Type { get; set; }
        public bool Required { get; set; }
        public bool EvidenceRequired { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> Expires { get; set; }

        [Column(Visible = false)]
        public Nullable<int> RecordId { get; set; }
        public bool RequirementMet {
            get
            {
                // Check to see if document expired
                DateTime curDate = DateTime.Now;
                if (Expires == null || Expires > curDate)
                    return false;

                // Check to see if document verified

                // Check to see if document uploaded
                if (RecordId == null)
                    return false;

                return true;
            }
        }
    }
}

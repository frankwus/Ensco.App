using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class RigRequirementDocsModel
    {
        [Column(Visible =false, Key =true)]
        public int Id { get; set; }
        public int Requirement { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public bool EvidenceRequired { get; set; }
        public string Description { get; set; }
        public Nullable<int> UserRecordId { get; set; }
    }
}

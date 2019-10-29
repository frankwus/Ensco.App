using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class RigRequirementsModel
    {
        [Column(Visible = false, Key = true)]
        public int Id { get; set; }
        [Column(LookupList ="UserGovtIdType")]
        public int Requirement { get; set; }
        public bool Required { get; set; }
        [Column(Visible =false)]
        public List<RigSubRequirementsModel> SubRequirements { get; set; }
        public RigRequirementsModel()
        {

        }
    }
}

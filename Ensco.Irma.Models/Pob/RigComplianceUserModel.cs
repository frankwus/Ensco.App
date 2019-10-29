using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class RigComplianceUserModel
    {
        [Column(Visible = false, Key =true)]
        public int Id { get; set; }
        [Column(DisplayName="Name")]
        public string DisplayName { get; set; }
        [Column(LookupList = "Nationality")]
        public int Nationality { get; set; }
        [Column(LookupList = "Company")]
        public int Company { get; set; }
        [Column(LookupList = "Department")]
        public int Department { get; set; }
        [Column(LookupList = "Position")]
        public int Position { get; set; }
        public bool RequirementsMet {
            get
            {
                // Check the requirements here
                return false;
            }
        }
        public RigComplianceUserModel()
        {

        }
    }
}

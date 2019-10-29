using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class TeamMemberModel
    {
        [Column(Visible =false, Key =true)]
        public int Id { get; set; }
        [Column(Visible = false)]
        public int TeamId { get; set; }
        [Column(LookupList = "Passport")]
        public int Passport { get; set; }
        public System.DateTime LastUpdated { get; set; }
        [Column(LookupList = "Company")]
        public int Company { get; set; }
        [Column(LookupList ="Position")]
        public int Position { get; set; }
        [Column(Visible = false)]
        public int Onboard { get; set; }
        public bool OnboardNow { get { return (Onboard == 1 ? true : false); } set { Onboard = (value ? 1 : 0); } }
        public TeamMemberModel()
        {
            LastUpdated = DateTime.Now;
        }
    }
}

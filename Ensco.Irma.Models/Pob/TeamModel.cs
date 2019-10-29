using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class TeamModel
    {
        [Column(Visible = false, Key = true)]
        public int Id { get; set; }
        [Column(LookupList = "TeamType")]
        public int Team { get; set; }
        public int Required { get; set; }
        public int Actual { get; set; } // Get this from count of onboard personnel in the team
        public bool Compliance {
            get {
                if(UtilitySystem.GetDataSet("select * from POB_EmergencyTeamMembers t join vw_currentPob_RigPersonnel p on t.Passport=p.passportId and p.status =1 and t.TeamId=" + Team.ToString()).Tables[0].Rows.Count >= Required)
                    return true;
                return false;
                //    return ((Required <= Members.Where(x=>x.Onboard==1).Count()) ? true : false);
            }
        }
        [Column(Visible = false)]
        public List<TeamMemberModel> Members { get; set; }

        public TeamModel() {
            Members = new List<TeamMemberModel>();
        }
    }
}

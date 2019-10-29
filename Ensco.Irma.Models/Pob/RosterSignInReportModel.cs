using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class RosterItem
    {
        public string Name { get; set; }
        public string RigName { get; set; }       
        public RosterItem(string name)
        {
            Name = name;
            RigName = Utilities.UtilitySystem.Settings.RigName;
        }
    }
    public class RosterSignInReportModel : List<RosterItem>
    {        
        public RosterSignInReportModel()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class RosterSignInSheetModel
    {
        public string RigName { get; set; }

        public List<RosterUserModel> Persons { get; set; }

        public RosterSignInSheetModel()
        {
            Persons = new List<RosterUserModel>();
            RigName = Utilities.UtilitySystem.Settings.RigName;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class RosterUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GroupItem { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public string Tour { get; set; }
        public string RigName { get; set; }
        public RosterUserModel()
        {
            Name = "";
            GroupItem = "";
            Position = "";
            Company = "";
            Tour = "";
            RigName = Utilities.UtilitySystem.Settings.RigName;
        }
    }
}

using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class LifeBoatRosterListModel
    {
        [Column(Visible = false, Key =true)]
        public int Id { get; set; }
        public string PersonnelType { get; set; }
        public string PrimaryLifeBoat { get; set; }
        public string Company { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Nationality { get; set; }
        public string Tour { get; set; }
        public string Cabin { get; set; }
    }
}

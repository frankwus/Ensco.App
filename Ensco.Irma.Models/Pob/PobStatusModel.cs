using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class PobStatusModel
    {
        public enum PobStatus { PlannedOnboard, PlannedOffboard, Active, Inactive, Disabled, Onboard, Offboard };

        [Column(Visible =false, Key =true)]
        public int Id { get; set; }
        public string Name { get; set; }
        [Column(Visible=false)]
        public PobStatus Status { get; set; }
    }
}

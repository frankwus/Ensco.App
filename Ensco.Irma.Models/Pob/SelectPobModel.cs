using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class SelectPobModel
    {
        public enum PobType{ None, CrewChangePob, PassportPob };
        public Nullable<int> SubmitAction { get; set; }
        [Column(LookupList = "CrewPobArriving")]
        public Nullable<int> PobCrewChange { get; set; }
        [Column(LookupList ="PassportNotOnboard")]
        public Nullable<int> PobPassport { get; set; }
        public SelectPobModel()
        {
            SubmitAction = (int)PobType.None;
        }
    }
}

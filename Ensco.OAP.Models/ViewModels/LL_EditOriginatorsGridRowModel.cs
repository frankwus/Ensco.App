using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.OAP.Models.ViewModels
{
    public class LL_EditOriginatorsGridRowModel
    {
        [Column(Key = true)]
        public int Id { get; set; }

        [Column(LookupList = "Passport", DisplayName = "Name")]
        public int PassportId { get; set; }
        
        [Column(LookupList = "Position")]
        public int Position { get; set; }

        [Column(DisplayName = "Rig / Facility", LookupList = "Rig")]
        public int RigFacility { get; set; }
    }
}

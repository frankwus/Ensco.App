using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class PobSummaryCountsModel
    {
        [Column(Visible =false, Key =true)]
        public int Id { get; set; }
        public int BedsOccupied { get; set; }
        public int EsstentialYes { get; set; }
        public int EsstentialNo { get; set; }
        public int ShortServiceYes { get; set; }
        public int ShortServiceNo { get; set; }
    }
}

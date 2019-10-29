using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class TourChangeModel
    {
        [Column(Visible =false, Key =true)]
        public int Id { get; set; }
        public int PobId { get; set; }
        public int TourFrom { get; set; }
        public int TourTo { get; set; }
        public bool ShortChange { get; set; }
        public System.DateTime DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
    }
}

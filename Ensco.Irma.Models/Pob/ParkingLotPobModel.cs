using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class ParkingLotPobModel
    {
        public Nullable<int> Arriving { get; set; }
        public Nullable<int> Departing { get; set; }

        public int Status { get; set; }
    }
}

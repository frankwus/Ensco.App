using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class ParkingLotModel
    {
        [Column(Visible = false)]
        public int Id { get; set; }
        [Column(LookupList = "Passport")]
        public int PassportId { get; set; }
        public Nullable<double> BodyWeight { get; set; }
        public Nullable<double> BagWeight { get; set; }
        public Nullable<short> Bags { get; set; }
        public string AirlineFlight { get; set; }
        public string HomeAirport { get; set; }
        public string Terminal { get; set; }
        public string Location { get; set; }
        public Nullable<System.DateTime> ArrivalDepartureTime { get; set; }
        public Nullable<int> Status { get; set; }
        public DateTime ModifiedDate { get; set; }
        [Column(LookupList = "Passport")]
        public int ModifiedBy { get; set; }
    }
}

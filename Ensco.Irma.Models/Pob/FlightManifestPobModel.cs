using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class FlightManifestPobModel
    {
        [Column(Visible =false)]
        public int CrewFlightId { get; set; }
        [Column(Visible = false)]
        public int PobId { get; set; }
        [Column(Visible = false, Key =true)]
        public int Id { get; set; }
        [Column(LookupList = "Passport", DisplayName = "Name")]
        public int PassportId { get; set; }
        public string Passport { get; set; }
        [Column(LookupList = "Position",Editable = false)]
        public Nullable<int> Position { get; set; }
        [Column(LookupList = "Company", Editable = false)]
        public Nullable<int> Company { get; set; }
        [Column(LookupList = "Nationality", Editable = false)]
        public Nullable<int> Nationality { get; set; }
        public string Location { get; set; }
        [Column(Visible = false)]
        public double BodyWeight { get; set; }
        [Column(Visible = false)]
        public double BagWeight { get; set; }
        public short Bags { get; set; }
        public string HomeAirport { get; set; }
        public string AirlineFlight { get; set; }
        public string Terminal { get; set; }
        [Column(DateFormatType = ColumnAttribute.DateTimeDisplayType.DateTime)]
        public Nullable<DateTime> Time { get; set; }

        [Column(LookupList = "PobPlanningStatus", DisplayName = "PoB Status")]
        public int Status { get; set; }

        [Column(Visible = false)]
        public bool Selected { get; set; }
        public FlightManifestPobModel()
        {
            Time = DateTime.Now;
        }
    }
}

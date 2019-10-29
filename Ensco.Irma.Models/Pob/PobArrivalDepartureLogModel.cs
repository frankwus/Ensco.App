using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class PobArrivalDepartureLogModel
    {
        [Column(Visible = false, Key = true)]
        public int Id { get; set; }
        public int CrewChangeId { get; set; }
        public int CrewFlightId { get; set; }
        public string Flight { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string CompanyType { get; set; }
        public string Company { get; set; }
        public string Nationality { get; set; }
        public Nullable<System.DateTime> On { get; set; }
        public string Status { get; set; }
        [Column(Visible = false)]
        public int StatusId { get; set; }
    }
}

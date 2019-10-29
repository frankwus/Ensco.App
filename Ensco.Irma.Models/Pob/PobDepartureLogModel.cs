using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class PobDepartureLogModel
    {
        [Column(Visible = false, Key =true)]
        public int Id { get; set; }
        [Column(Visible = false)]
        public int CrewChangeId { get; set; }
        [Column(Visible = false)]
        public int CrewFlightId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Nationality { get; set; }
        public System.DateTime On { get; set; }

        [Column(Visible = false)]
        public int StatusId { get; set; }
    }
}

using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class OffboardPobModel
    {
        [Column(Visible = false, Key = true)]
        public int Id { get; set; }
        [Column(Visible = false)]
        public int CrewChangeId { get; set; }
        [Column(Visible = false)]
        public int CrewFlightId { get; set; }
        [Column(Visible = false)]
        public int CrewPobId { get; set; }
        [Column(Visible = false)]
        public int PassportId { get; set; }
        public string DisplayName { get; set; }
        [Column(LookupList = "Position")]
        public int Position { get; set; }
        [Column(LookupList = "Company")]
        public int Company { get; set; }
        [Column(LookupList = "Nationality")]
        public int Nationality { get; set; }
        [Column(LookupList = "Tour")]
        public int Tour { get; set; }
        [Column(LookupList = "RigCrew")]
        public int Crew { get; set; }
        [Column(LookupList = "Room")]
        public int Room { get; set; }
        [Column(LookupList = "RoomBed")]
        public int Bed { get; set; }
        [Column(LookupList = "MusterStation")]
        public DateTime DateStart { get; set; }

        [Column(Visible = false)]
        public bool Selected { get; set; }
        public OffboardPobModel()
        {

        }

    }
}

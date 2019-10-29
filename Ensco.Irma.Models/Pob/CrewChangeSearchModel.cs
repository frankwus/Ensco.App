using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class CrewChangeSearchModel
    {
        [Column(Visible = false, Key =true)]
        public int Id { get; set; }
        [Column(Visible = false)]
        public Nullable<int> CrewChangeId { get; set; }
        [Column(Visible = false)]
        public int CrewFlightId { get; set; }
        public string CrewChange { get; set; }
        public string Flight { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public string Name { get; set; }
        [Column(LookupList = "Company")]
        public Nullable<int> Company { get; set; }
        [Column(LookupList = "Position")]
        public Nullable<int> Position { get; set; }
        [Column(LookupList = "Nationality")]
        public Nullable<int> Nationality { get; set; }
        [Column(LookupList = "PobStatus")]
        public Nullable<int> Status { get; set; }
    }
}

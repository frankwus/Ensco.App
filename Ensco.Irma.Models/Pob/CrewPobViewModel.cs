using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class CrewPobViewModel
    {
        [Column(Visible = false)]
        public int Id { get; set; }
        [Column(Visible = false)]
        public int CrewChangeId { get; set; }
        public string Flight { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public string Nationality { get; set; }
        public string Status { get; set; }
        [Column(Visible = false)]
        public int CrewFlightId { get; set; }
        [Column(Visible = false)]
        public int StatusId { get; set; }
        public int PassportId { get; set; }
        public CrewPobViewModel()
        {

        }
    }
}

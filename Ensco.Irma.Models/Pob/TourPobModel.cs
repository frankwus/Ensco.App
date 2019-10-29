using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class TourPobModel
    {
        [Column(Visible = false, Key =true)]
        public int Id { get; set; }
        public string Name { get; set; }
        [Column(LookupList = "Company")]
        public Nullable<int> Company { get; set; }
        [Column(LookupList = "Department")]
        public Nullable<int> Department { get; set; }
        [Column(LookupList = "Position")]
        public Nullable<int> Position { get; set; }
        [Column(LookupList = "RigCrew")]
        public Nullable<int> Crew { get; set; }
        [Column(LookupList = "Tour")]
        public Nullable<int> Tour { get; set; }

        [Column(Visible = false)]
        public bool Selected { get; set; }
    }
}

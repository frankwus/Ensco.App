using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class PobLookupModel
    {
        [Column(Visible = false, Key =true)]
        public int Id { get; set; }
        public string Name { get; set; }        
        public string Passport { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public string Manager { get; set; }
        public string Nationality { get; set; }
        public Nullable<int> Status { get; set; }
    }
}

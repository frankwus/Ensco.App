using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class PersonnelOnOffBoardModel
    {
        [Column(Visible = false, Key =true)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public string CompanyType { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<System.DateTime> On { get; set; }
        public Nullable<System.DateTime> Off { get; set; }
    }
}

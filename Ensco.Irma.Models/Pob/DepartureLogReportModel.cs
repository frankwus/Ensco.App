using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class DepartureLogReportModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string CompanyType { get; set; }
        public string Company { get; set; }
        public Nullable<System.DateTime> On { get; set; }
        public int StatusId { get; set; }
    }
}

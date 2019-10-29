using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models.Irma
{
    public class IrmaHomeTasksCapModel
    {
        [Column(Visible=true, Key=true)]
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string Approver { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
    }
}

using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models.Irma
{
    public class IrmaHomeTasksIsolationModel
    {
        [Column(Visible=true, Key=true)]
        public Nullable<int> id { get; set; }
        public string Source { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        [Column(Visible = false)]
        public int id1 { get; set; }
    }
}

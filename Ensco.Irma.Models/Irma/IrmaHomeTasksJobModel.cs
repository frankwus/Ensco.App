using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models.Irma
{
    public class IrmaHomeTasksJobModel
    {
        [Column(Visible=true, Key=true)]
        public int id { get; set; }
        public string Job_Title { get; set; }
        public string Status { get; set; }
        [Column(Visible = false)]
        public string userId { get; set; }
    }
}

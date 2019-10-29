using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.OAP.Models.Protocol
{
    public class OpenProtocolGridViewModel
    {
        public int ID { get; set; }
        public DateTime DateStarted { get; set; }
        public string AssessorPosition { get; set; }
        public string AssessorName { get; set; }
        public bool AuthoScheduled { get; set; }
        public string Status { get; set; }
    }
}

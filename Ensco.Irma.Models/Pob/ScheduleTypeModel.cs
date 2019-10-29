using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class ScheduleTypeModel
    {
        [Column(Visible=false, Key=true)]
        public int Id { get; set; }

        public string Name { get; set; }

        public ScheduleTypeModel()
        {

        }
    }
}

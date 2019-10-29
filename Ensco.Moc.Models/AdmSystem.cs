using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Moc.Models
{
    public class AdmSystem
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public List<AdmSubSystem> SubSystems { get; set; }
    }
}

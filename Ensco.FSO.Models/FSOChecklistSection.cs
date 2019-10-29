using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.FSO.Models
{
    public class FSOChecklistSection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string Type { get; set; }

    }
}

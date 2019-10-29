using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Services
{
    public class CrewChangeApproverModel
    {
        [Column(Visible = false, Key =true)]
        public int Id { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}

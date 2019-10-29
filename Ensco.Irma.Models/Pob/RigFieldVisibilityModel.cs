using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class RigFieldVisibilityModel
    {
        [Column(Visible=false, Key =false)]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Visible { get; set; }
        public RigFieldVisibilityModel()
        {

        }
    }
}

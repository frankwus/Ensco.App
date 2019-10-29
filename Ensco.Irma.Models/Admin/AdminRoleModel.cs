using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models.Admin
{
    public class AdminRoleModel
    {
        [Column(Visible=false, Key=true)]
        public int id { get; set; }
        [Display(Name = "Ensco.Property.Name")]
        public string Name { get; set; }
    }
}

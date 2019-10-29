using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.OAP.Models.ViewModels
{
    public class LL_ControlGridViewRowModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string ImpactLevel { get; set; }
        public string Originator { get; set; }
        public string Title { get; set; }
        public string eMocId { get; set; }
        public string Status { get; set; }
    }
}

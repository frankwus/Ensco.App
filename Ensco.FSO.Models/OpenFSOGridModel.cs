using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.FSO.Models
{
    public class OpenFSOGridModel
    {
        [Column(Visible = false)]
        public Guid Id { get; set; }

        [Column(DisplayName = "Id")]
        public int RigChecklistUniqueId { get; set; }

        [Required]
        public string Title { get; set; }

        [Column(DisplayName = "Checklist Date")]
        public DateTime ChecklistDateTime { get; set; }

        public string Observer { get; set; }

        public string Position { get; set; }

        [Column(LookupList = "Company")]
        public int? Company { get; set; }

        [Column(LookupList = "Rig")]
        public int? Location { get; set; }
    }
}

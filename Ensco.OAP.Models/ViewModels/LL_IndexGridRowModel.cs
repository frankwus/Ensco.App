using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.OAP.Models.ViewModels
{
    public class LL_IndexGridRowModel
    {
        [Column(HyperLinkField = "Id", HyperLinkType = ColumnAttribute.HyperLinkFieldType.LessonLearned)]
        public int Id { get; set; }
        [Column(DisplayName = "Type")]
        public int? TypeId { get; set; } 
        [Required]
        public string Title { get; set; }
        [Column(DisplayName = "Source Rig / Facility", LookupList = "Rig")]
        public int SourceRigFacility { get; set; }
        [Column(DisplayName = "Source Form", Visible = false)]
        public string SourceForm { get; set; }
        [Column(DisplayName = "Impact Level"), Required]
        public string ImpactLevel { get; set; }
        [Column(DisplayName = "eMoc Id")]
        public string eMocId { get; set; }
        public string Status { get; set; }

        [Column(DisplayName = "Date Created")]
        public DateTimeOffset DateStarted { get; set; }
    }
}

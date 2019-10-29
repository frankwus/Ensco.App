using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.OAP.Models
{
    public class LessonLearnedModel
    {
        public LessonLearnedModel()
        {
            Approvals = new List<LessonLearnedApprovalModel>();
            Attachments = new List<LessonLearnedAttachmentModel>();
            Originators = new List<LessonLearnedOriginatorModel>();
        }
        [Column(HyperLinkField = "Id", HyperLinkType = ColumnAttribute.HyperLinkFieldType.LessonLearned)]
        public int Id { get; set; }
        public string OapType { get; set; }
        public DateTimeOffset? DateStarted { get; set; }
        public DateTimeOffset? DateCompleted { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SourceBU { get; set; }
        public string SourceRigFacility { get; set; }
        public string SourceForm { get; set; }
        public int? SourceFormId { get; set; }
        public string ImpactLevel { get; set; }
        public string eMocId { get; set; }
        public string eMocStatus { get; set; }

        public virtual List<LessonLearnedApprovalModel> Approvals { get; set; }
        public virtual List<LessonLearnedAttachmentModel> Attachments { get; set; }
        public virtual List<LessonLearnedOriginatorModel> Originators { get; set; }


    }
}

using Ensco.Irma.Models;
using Ensco.Models;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.OAP.Models
{
    public class LessonLearnedModel
    {
        public LessonLearnedModel()
        {
            Approvals = new List<ApprovalModel>();
            Attachments = new List<AttachmentModel>();
            Originators = new List<LessonLearnedOriginatorModel>();
        }
        [Column(HyperLinkField = "Id", HyperLinkType = ColumnAttribute.HyperLinkFieldType.LessonLearned)]
        public int Id { get; set; }
        public string OapType { get; set; }
        public Nullable<DateTimeOffset> DateStarted { get; set; }
        public Nullable<DateTimeOffset> DateCompleted { get; set; }
        public string Status { get; set; }
        public Nullable<int> TypeId { get; set; }

        [Required]
        public string Title { get; set; }

        [Column(LookupList = "Topic")]
        public Nullable<int> Topic { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Column(LookupList = "BusinessUnit")]
        public Nullable<int> SourceBU { get; set; }

        [Required]
        [Column(LookupList = "Rig")]
        public Nullable<int> SourceRigFacility { get; set; }
        [Column(DisplayName = "Source Form", Visible = false)]
        public string SourceForm { get; set; }
        public Nullable<int> SourceFormId { get; set; }

        public string SourceFormURL { get; set; }

        [Required(ErrorMessage = "Please select an Impact Level")]
        public string ImpactLevel { get; set; }

        public string eMocId { get; set; }

        public string eMocStatus { get; set; }

        public bool IsEditable {
            get { return (Status == "Open" || Status == "Final Review"); }
        }

        public IEnumerable<ApprovalModel> GetPreApprovals()
        {
            return Approvals.Where(a => a.Type == (int)ApprovalModel.ApprovalType.LessonsLearnedPreApproval);
        }

        public IEnumerable<ApprovalModel> GetApprovals()
        {
            return Approvals.Where(a => a.Type == (int)ApprovalModel.ApprovalType.LessonsLearnedApproval);
        }

        public virtual List<ApprovalModel> Approvals { get; set; }
        public virtual IEnumerable<AttachmentModel> Attachments { get; set; }
        public virtual List<LessonLearnedOriginatorModel> Originators { get; set; }
        public virtual LessonLearnedType Type { get; set; }

    }
}

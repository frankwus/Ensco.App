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
    public class ReviewModel
    {
        [Column(Visible = false)]
        public int Id { get; set; }
        [Column(DisplayName = "Source Rig", LookupList ="Rig")]
        public int? RigId { get; set; }

        [Column(Visible = false)]
        public string SourceForm { get; set; }

        [Column(Visible = false)]
        public string SourceFormId { get; set; }

        [Column(Visible = false)]
        public string SourceFormURL { get; set; }

        [Column(Visible = false)]
        public DateTimeOffset? ReviewDate { get; set; }

        [Column(Visible = false)]
        public int? CAPAId { get; set; }

        [Column(Visible = false)]
        public string Comment { get; set; }

        [Column(LookupList ="Passport", DisplayName = "Reviewer")]
        public int ReviewerPassportId { get; set; }

        [Column(LookupList = "BusinessUnit", Visible = false)]
        public int? SourceBU { get; set; }
        [Column(LookupList ="Passport", Visible = false)]
        public int RequestedBy { get; set; }

        [Column(Visible = false)]
        public DateTimeOffset? DateCreated { get; set; }

        [Column(Visible = false, DisplayName = "ReviewerDisplayName")]
        public string Reviewer { get; set; }

        [Column(Visible = false)]
        public IEnumerable<AttachmentModel> Attachments { get; set; }

        [Column(Visible = false, DateFormatType = ColumnAttribute.DateTimeDisplayType.DateTime)]
        public DateTime? DueDate { get; set; }

        [Column(Visible = false)]
        public bool IsSigned => ReviewDate != null;

        public string Status => ReviewDate != null ? "Completed" : "Pending";

        public ReviewModel()
        {
            Attachments = new List<AttachmentModel>();
        }
        
    }
}

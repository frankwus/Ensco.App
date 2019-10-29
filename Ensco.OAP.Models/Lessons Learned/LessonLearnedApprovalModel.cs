using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.OAP.Models
{
    public class LessonLearnedApprovalModel
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public string ApprovalSection { get; set; }
        [Column(LookupList = "Passport")]
        public int ApproverPassportId { get; set; }
        [Column(LookupList = "Position")]
        public int Position { get; set; }
        public string OriginatorComments { get; set; }
        public string Status { get; set; }
        public string ApproverComments { get; set; }
        public DateTimeOffset DateCompleted { get; set; }
        [Column(DateFormatType = ColumnAttribute.DateTimeDisplayType.DateTime)]
        public DateTime? DueDate { get; set; }

        public virtual LessonLearnedModel LessonLearned { get; set; }

    }
}

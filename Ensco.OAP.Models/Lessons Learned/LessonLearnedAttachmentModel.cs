using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.OAP.Models
{
    public class LessonLearnedAttachmentModel
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public string Link { get; set; }

        public virtual List<LessonLearnedModel> LessonLearned { get; set; }
    }
}

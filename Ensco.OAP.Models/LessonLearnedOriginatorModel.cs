using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.OAP.Models
{
    public class LessonLearnedOriginatorModel
    {
        [Column(Visible = false)]
        public int Id { get; set; }
        [Column(LookupList = "Rig")]
        public int RigFacility { get; set; }
        [Column(LookupList = "Passport")]
        public int PassportId { get; set; }
        public int LessonId { get; set; }

        [Column(LookupList = "Position")]
        public int Position { get; set; }

        public virtual LessonLearnedModel LessonLearned { get; set; }
    }
}

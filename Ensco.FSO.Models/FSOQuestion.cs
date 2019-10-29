using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.FSO.Models
{
    public abstract class FSOQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public Nullable<bool> Safe { get; set; }
        public string Description { get; set; }
        public string QuestionTopic { get; set; }
        public int ChecklistSectionId { get; set; }
        public virtual FSOChecklistSection Section { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ensco.App.Areas.Oap.Models
{
    public class OapChecklistAnswerPostbackModel
    {        
        public Guid QuestionId { get; set; }
        public int AnswerOrdinal { get; set; }
        public string Value { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ensco.App.Areas.Oap.Models
{
    public class ChecklistAssessorGridViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public string Tour { get; set; }
        public bool IsLead { get; set; }
    }
}
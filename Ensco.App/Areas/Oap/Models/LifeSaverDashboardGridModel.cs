using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ensco.App.Areas.Oap.Models
{
    public class LifeSaverDashboardGridModel
    {
        public int RigChecklistUniqueId { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string LeadAssessor { get; set; }
        public string Position { get; set; }
        public string Status { get; set; }
    }
}
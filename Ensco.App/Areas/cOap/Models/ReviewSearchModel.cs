using System;
using System.ComponentModel.DataAnnotations;

namespace Ensco.App.Areas.cOap.Models
{
    public class ReviewSearchModel
    {
        [Required]
        public int SearchBy { get; set; }

        [Required]
        public int ChecklistId { get; set; }
                 
        public DateTime FromDate { get; set; }
      
        public DateTime ToDate { get; set; } 
    }
}
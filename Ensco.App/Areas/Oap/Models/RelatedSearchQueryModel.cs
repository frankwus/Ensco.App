using System;
using System.ComponentModel.DataAnnotations;

namespace Ensco.App.Areas.Oap.Models
{
    public class RelatedSearchQueryModel
    {
        [Required]
        public string SearchBy { get; set; }

        public int QuestionId { get; set; }

        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }        

       

    }

}
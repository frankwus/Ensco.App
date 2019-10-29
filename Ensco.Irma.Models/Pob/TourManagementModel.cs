using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class TourManagementModel
    {
        public List<TourPobModel> CurrentPob { get; set; }
        [Column(LookupList ="Tour")]
        [Required]
        public Nullable<int> TourChange { get; set; }
        public DateTime TourChangeDate { get; set; }
        public DateTime ShortChangeBeginDate { get; set; }
        public DateTime ShortChangeEndDate { get; set; }

        public bool TourChangeNow { get; set; }
        public bool TourChangeFrom { get; set; }
        public bool ShortChange { get; set; }

        public TourManagementModel()
        {
            TourChangeDate = DateTime.Now;
            ShortChangeBeginDate = DateTime.Now;
            ShortChangeEndDate = DateTime.Now;
            TourChangeNow = true;
            TourChangeFrom = false;
            ShortChange = false;
        }
    }
}

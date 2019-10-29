using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class ScheduleCrewChangeModel
    {
        public enum ScheduleTypeEnum { Daily = 1, Weekly, BiWeekly, EveryThreeWeeks, Monthly, Quarterly, EverySixMonths, Yearly };
        [Required]
        public string Title { get; set; }
        [Column(LookupList = "RigCrew")]
        [Required]
        public Nullable<int> Crew { get; set; }
        [Column(DateFormatType = ColumnAttribute.DateTimeDisplayType.DateOnly)]
        [Required]
        public DateTime DateStart { get; set; }
        [Column(DateFormatType = ColumnAttribute.DateTimeDisplayType.DateOnly)]
        [Required]
        public DateTime DateEnd { get; set; }
        //[Column(LookupList ="ScheduleType")]
        [Required]
        public Nullable<int> ScheduleType { get; set; }
        public ScheduleCrewChangeModel() {

        }
    }
}

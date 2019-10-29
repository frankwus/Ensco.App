using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class PobSummaryModel
    {
        [Column(Visible=false, Key=true)]
        public int Id { get; set; }
        [Column(DisplayName = "Ensco Per Contract")]
        public int EnscoPerContract { get; set; }
        [Column(DisplayName = "Ensco Standard")]
        public int EnscoStandard { get; set; }
        [Column(DisplayName = "Ensco Other")]
        public int EnscoOther { get; set; }
        [Column(DisplayName = "Ensco Service")]
        public int EnscoService { get; set; }
        [Column(DisplayName = "Ensco Catering")]
        public int EnscoCatering { get; set; }
        public int Client { get; set; }
        [Column(DisplayName = "Client Service")]
        public int ClientService { get; set; }
        [Column(Visible=false)]
        public int TotalPOB { get; set; }
        [Column(DisplayName = "Total PoB")]
        public string TotalPoB { get { return string.Format("{0} ({1} {2})", TotalPOB, (EnscoPerContract-TotalPOB > 0 ? "-" : "+" ), Math.Abs(EnscoPerContract - TotalPOB)); } }
        public PobSummaryModel()
        {
        }
    }
}

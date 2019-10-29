using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models.Jobs
{
    public class JobSummaryModel
    {
        public int OpenPermits { get; set; }
        public int PendingAuthorization { get; set; }
        public int Authorized { get; set; }
        public int PendingVerification { get; set; }
        public int Expired { get; set; }
    }
}

using Ensco.Irma.Data.Contexts.OAP.OIMOversightDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Data.Repositories.SWA
{
    public class SWARepository : EnscoIrmaRepository<Irma.Data.SWA>
    {
        private readonly OIMOversightDashboardContext context;

        public SWARepository(OIMOversightDashboardContext context)
        {
            this.context = context;
        }
    }
}

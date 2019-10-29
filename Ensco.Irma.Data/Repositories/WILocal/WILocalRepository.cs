using Ensco.Irma.Data.Contexts.OAP.OIMOversightDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Data.Repositories
{
    public class WILocalRepository : EnscoIrmaRepository<WiLocal>
    {        
        public WILocalRepository(OIMOversightDashboardContext context)
        {
            Entities = context;
        }
    }
}

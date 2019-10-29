using Ensco.Data;
using Ensco.Irma.Data.Contexts.OAP.OIMOversightDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Data.Repositories.CAPA
{
    public class CAPARepository : EnscoRepository<IrmaCapa>
    {
        public CAPARepository(OIMOversightDashboardContext context)
        {
            Entities = context;
        }
    }
}

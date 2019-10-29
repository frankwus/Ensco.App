using Ensco.Irma.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Data.Services.WILocal
{
    public class WILocalService
    {
        private readonly WILocalRepository repository;
        public WILocalService(WILocalRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<WiLocal> GetNewOrPendingWiLocals()
        {
            return repository.Filter(w => w.Status.ToLower().Equals("new") || w.Status.ToLower().Equals("pending approval from oim")).ToList();
        }
    }
}

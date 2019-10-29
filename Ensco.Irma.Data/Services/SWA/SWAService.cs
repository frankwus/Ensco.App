using Ensco.Irma.Data.Repositories.SWA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Data.Services.SWA
{
    public class SWAService
    {
        private readonly SWARepository repository;

        public SWAService(SWARepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Irma.Data.SWA> GetByStatus(string status)
        {
            string statusToLower = status.ToLower();
            return repository.Filter(s => s.Status.ToLower().Equals(statusToLower)).ToList();
        }
    }
}

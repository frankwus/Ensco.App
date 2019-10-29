using Ensco.Irma.Data.Enums;
using Ensco.Irma.Data.Repositories.CAPA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Data.Services.CAPA
{
    public class CAPAService
    {
        private readonly CAPARepository repository;

        public CAPAService(CAPARepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<IrmaCapa> GetUserCAPAs(int userId)
        {
            return repository.Filter(c => c.AssignedTo == userId.ToString() && c.Status != "Closed").ToList();
        }
    }
}

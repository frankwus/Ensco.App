using Ensco.Data;

namespace Ensco.Irma.Data
{
    public class EnscoIrmaRepository<T> : EnscoRepository<T> where T : class
    {
        EnscoIrmaEntities _entities = new EnscoIrmaEntities();
        
        public EnscoIrmaRepository()
        {
            Entities = _entities;
        }
    }
}

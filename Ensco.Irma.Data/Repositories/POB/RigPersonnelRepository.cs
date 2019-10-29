using Ensco.Data;
using Ensco.Irma.Data.Contexts.POB;

namespace Ensco.Irma.Data.Repositories.POB
{
    public class RigPersonnelRepository : EnscoRepository<POB_RigPersonnel>
    {
        public RigPersonnelRepository()
        {
            Entities = new RigPersonnelContext();            
        }
    }
}

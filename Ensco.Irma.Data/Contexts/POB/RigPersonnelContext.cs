using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Data.Contexts.POB
{
    public class RigPersonnelContext : DbContext
    {
        public RigPersonnelContext() : base("name=EnscoIrmaEntities")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<POB_RigPersonnel> POB_RigPersonnel { get; set; }
    }
}

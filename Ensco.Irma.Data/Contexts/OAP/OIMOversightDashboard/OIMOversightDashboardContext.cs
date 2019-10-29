using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Data.Contexts.OAP.OIMOversightDashboard
{
    public class OIMOversightDashboardContext : DbContext
    {
        public OIMOversightDashboardContext() : base("name=EnscoIrmaEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<WiLocal> WILocals { get; set; }
        public virtual DbSet<IrmaCapa> CAPAs { get; set; }
        public virtual DbSet<Irma.Data.SWA> SWAs { get; set; }
        public virtual DbSet<vw_Job> Jobs { get; set; }
    }
}

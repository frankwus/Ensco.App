using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Storage;

[assembly: OwinStartup(typeof(Ensco.App.App_Start.Startup))]
namespace Ensco.App.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            GlobalConfiguration.Configuration.UseSqlServerStorage("EnscoHangfireIrmaConnection", new SqlServerStorageOptions { QueuePollInterval = TimeSpan.FromSeconds(10) });

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            // setup any schedule jobs for background processing
            using (JobStorageConnection connection = Hangfire.JobStorage.Current.GetConnection() as JobStorageConnection)
            {
                EnscoSchedulerService.ScheduleJobs(connection);
                OAPScheduler.ScheduleBarrierAuthorityTasks(connection);
                OAPScheduler.ScheduleOIMOversightReminderEmails(connection);
            }

        }
    }
}
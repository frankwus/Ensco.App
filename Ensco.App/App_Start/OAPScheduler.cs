using Ensco.Irma.Models;
using Ensco.Irma.Oap.WebClient.Rig;
using Ensco.Irma.Services;
using Hangfire;
using Hangfire.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace Ensco.App.App_Start
{
    public class OAPScheduler
    {   
        internal static void ScheduleBarrierAuthorityTasks(IStorageConnection connection)
        {
            //IEnumerable<RigPersonnelModel> rigPersonnel = IrmaServiceSystem.GetOnboardedByDate(DateTime.Parse("2018-11-29 14:31:00"));
        }

        internal static void ScheduleOIMOversightReminderEmails(IStorageConnection connection)
        {
            // Deferred

            //string apiUrl = ConfigurationManager.AppSettings["WebApiUrl"]?.ToString();            
            //CookieContainer cookieContainer = new CookieContainer();
            //var Handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            //HttpClient httpClient = new HttpClient(Handler);

            //RigOapChecklistClient rigChecklistClient = new RigOapChecklistClient(apiUrl, httpClient);

            //RecurringJob.AddOrUpdate(() =>
            //{



            //}, Cron.Weekly(DayOfWeek.Sunday));
        }
    }
}
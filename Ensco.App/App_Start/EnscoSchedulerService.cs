using Ensco.Irma.Models;
using Ensco.Irma.Services;
using Ensco.Models;
using Ensco.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Ensco.Utilities;
using System.Reflection;
using Hangfire;
using System.DirectoryServices;
using Ensco.Scheduler;
using Hangfire.Storage;

namespace Ensco.App.App_Start
{
    public static class EnscoSchedulerService
    {
        private static bool scheduled = false;

        public static void ScheduleJobs(IStorageConnection connection)
        {
            var storageConnection = connection as JobStorageConnection;

            if (storageConnection != null)
            {
                var recurringJob = storageConnection.GetRecurringJobs();

                foreach (var job in recurringJob)
                {
                    RecurringJob.RemoveIfExists(job.Id);
                }
            }

            SchedulerService.ProcessActiveDirectoryUsersJob(UtilitySystem.Settings.ConfigSettings["AD"]);
            RecurringJob.AddOrUpdate(() => ProcessPobSummaryEmalJob(), Cron.Daily, TimeZoneInfo.Local);

            BackgroundJob.Enqueue(() => ProcessActiveDirectoryUsersJob());
        }

        public static void ProcessJob(int jobid)
        {
            Logger.Info(new LogInfo(MethodBase.GetCurrentMethod(), string.Format("Processing scheduled job with jobId={0}", jobid)));

            IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.Scheduler);
            ScheduleJobModel job = dataModel.GetItem(string.Format("Id={0}", jobid), "Id");
            if (job == null)
                return;

            switch ((ScheduleJobModel.ScheduledJobType)job.JobType)
            {
                case ScheduleJobModel.ScheduledJobType.PobSummaryEmail:
                    ProcessPobSummaryEmailJob(job);
                    break;
                case ScheduleJobModel.ScheduledJobType.ActiveDirUserInfo:
                    ProcessActiveDirectoryUsersJob(job);
                    break;
            }
        }

        public static void ProcessPobSummaryEmalJob()
        {
            IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.Scheduler);
            ScheduleJobModel job = dataModel.GetItem(string.Format("Id={0}", 1), "Id"); // PobSummaryReport job id
            if (job == null)
                return;

            ProcessPobSummaryEmailJob(job);
        }
        public static void ProcessActiveDirectoryUsersJob()
        {
            //IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.Scheduler);
            //ScheduleJobModel job = dataModel.GetItem(string.Format("Id={0}", 1), "Id"); // ActiveDirectory Service job id
            //if (job == null)
            //    return;

            //SchedulerService.ProcessActiveDirectoryUsersJob("ddr-ad04z");
        }
        public static void ProcessPobSummaryEmailJob(ScheduleJobModel job)
        {
            Logger.Info(new LogInfo(MethodBase.GetCurrentMethod(), string.Format("Processing jobId={0}", job.Id)));

            PobSummaryReport report = new PobSummaryReport();
            report.RigName.Value = Ensco.Utilities.UtilitySystem.Settings.RigName;
            report.LogoFile.Value = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/ensco.png");
            report.IrmaFile.Value = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/irma.png");

            // Show/Hide Essential and Vantage
            IIrmaServiceDataModel reqModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.RigFieldVisible);
            RigFieldVisibilityModel req = (RigFieldVisibilityModel)reqModel.GetItem(string.Format("Id=1"), "Id");
            report.ShowVantage.Value = (req != null) ? req.Visible : true;
            req = (RigFieldVisibilityModel)reqModel.GetItem(string.Format("Id=3"), "Id");
            report.ShowEssential.Value = (req != null) ? req.Visible : true;

            List<PobSummaryReportModel> list = new List<PobSummaryReportModel>();
            list.Add(IrmaServiceSystem.GetSummaryReportData());

            report.DataSource = list;

            IIrmaServiceDataModel emailDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Emails);
            PobEmailModel emailModel = emailDataModel.GetItem(string.Format("Name=\"PobSummaryReport\""), "Name");
            char[] sep = { ';' };
            string[] recipients = (emailModel != null && emailModel.Recipients != null) ? emailModel.Recipients.Split(sep) : null;
            IServiceDataModel pobDataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.User);

            try
            {
                using (SmtpClient client = new SmtpClient("smtp.ensco.ws"))
                {
                    MemoryStream memStream = new MemoryStream();
                    report.ExportToPdf(memStream);

                    memStream.Seek(0, System.IO.SeekOrigin.Begin);
                    Attachment att = new Attachment(memStream, "PobSummayReport.pdf", "application/pdf");

                    MailMessage message = new MailMessage();
                    message.Attachments.Add(att);
                    message.From = new MailAddress("irmareport@enscoplc.com");
                    message.Subject = emailModel.Subject;

                    // Get recepients       
                    foreach (string id in recipients)
                    {
                        UserModel user = pobDataModel.GetItem(string.Format("Id={0}", id), "Id");
                        if (user != null && user.Email != null)
                            message.To.Add(new MailAddress(user.Email));
                    }

                    // This line can be used to embed HTML into the email itself
                    // MailMessage message = currentReport.ExportToMail("irmareport@enscoplc.com", emailModel.Recipients, emailModel.Subject);

                    // Get correct credentials for irma profile
                    client.Credentials = new System.Net.NetworkCredential("Ensco\\023627", "");
                    client.Send(message);

                    memStream.Close();
                    memStream.Flush();
                }
            }
            catch (Exception ex)
            {

            }

        }

        public static void ProcessActiveDirectoryUsersJob(ScheduleJobModel job)
        {
            string AD = "ddr-ad04z";

            SchedulerService.ProcessActiveDirectoryUsersJob("ddr-ad04z");
        }
    }
}
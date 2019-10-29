using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Ensco.App.App_Start;
using Ensco.Irma.Models;
using Ensco.Irma.Services;
using Ensco.Models;
using Ensco.Services;
using Ensco.TLC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Ensco.TLC.Services;
using Ensco.Utilities;

namespace Ensco.App.Areas.IRMA.Controllers
{
    public class ReportController : Controller
    {
        // GET: IRMA/Report
         
        public ActionResult RosterSignInByMusterStationReport()
        {
            RosterSignInByMusterStation report = new RosterSignInByMusterStation();
            report.RigName.Value = Ensco.Utilities.UtilitySystem.Settings.RigName;
            report.LogoFile.Value = HttpContext.Server.MapPath("~/Images/ensco.png");
            report.IrmaFile.Value = HttpContext.Server.MapPath("~/Images/irma.png");

            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.RosterByMusterStation);
            List<RosterUserModel> list = dataModel.GetAllItems().Cast<RosterUserModel>().ToList();

            report.DataSource = list;

            Session["currentReport"] = report;

            //PopupModel popupModel = new PopupModel();
            //popupModel.Name = "RosterSignInByMusterStationReport";
            //popupModel.Title = "Roster SignIn By Muster Station";
            //popupModel.AllowResize = true;
            //popupModel.MaximizeBox = true;
            //popupModel.ContentUrl = Url.Action("ShowReportPartial", "Report", new { Area = "IRMA" });
            //popupModel.Data = report;
            //Session["PopupModel"] = popupModel;

            //return RedirectToAction("EnscoPopup", "Common", new { Area = "Common" });

            return RedirectToAction("ShowReport", "Report");
        }
         
        public ActionResult RosterSignInByLifeBoatReport()
        {
            RosterSignInByLifeBoat report = new RosterSignInByLifeBoat();
            report.RigName.Value = Ensco.Utilities.UtilitySystem.Settings.RigName;
            report.LogoFile.Value = HttpContext.Server.MapPath("~/Images/ensco.png");
            report.IrmaFile.Value = HttpContext.Server.MapPath("~/Images/irma.png");

            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.RosterByLifeBoat);
            List<RosterUserModel> list = dataModel.GetAllItems().Cast<RosterUserModel>().ToList();

            report.DataSource = list;
            Session["currentReport"] = report;

            return RedirectToAction("ShowReport", "Report");
        }
         
        public ActionResult RosterSignInReport()
        {
            RosterSignInSheet report = new RosterSignInSheet();
            report.RigName.Value = Ensco.Utilities.UtilitySystem.Settings.RigName;
            report.LogoFile.Value = HttpContext.Server.MapPath("~/Images/ensco.png");
            report.IrmaFile.Value = HttpContext.Server.MapPath("~/Images/irma.png");

            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.RosterFull);
            List<RosterUserModel> list = dataModel.GetAllItems().Cast<RosterUserModel>().ToList();

            report.DataSource = list;
            Session["currentReport"] = report;

            return RedirectToAction("ShowReport", "Report");
        }
         
        public ActionResult PobSummaryReport()
        {
            PobSummaryReport report = new PobSummaryReport();
            report.RigName.Value = Ensco.Utilities.UtilitySystem.Settings.RigName;
            report.LogoFile.Value = HttpContext.Server.MapPath("~/Images/ensco.png");
            report.IrmaFile.Value = HttpContext.Server.MapPath("~/Images/irma.png");

            // Show/Hide Essential and Vantage
            IIrmaServiceDataModel reqModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.RigFieldVisible);
            RigFieldVisibilityModel req = (RigFieldVisibilityModel)reqModel.GetItem(string.Format("Id=1"), "Id");
            report.ShowVantage.Value = (req != null) ? req.Visible : true;
            req = (RigFieldVisibilityModel)reqModel.GetItem(string.Format("Id=3"), "Id");
            report.ShowEssential.Value = (req != null) ? req.Visible : true;

            List<PobSummaryReportModel> list = new List<PobSummaryReportModel>();
            list.Add(IrmaServiceSystem.GetSummaryReportData());

            report.DataSource = list;

            Session["currentReport"] = report;

            return RedirectToAction("ShowReport", "Report");
        }
         
        public ActionResult LifeBoatComplianceReport()
        {
            LifeBoatComplianceReport report = new LifeBoatComplianceReport();
            report.RigName.Value = Ensco.Utilities.UtilitySystem.Settings.RigName;
            report.LogoFile.Value = HttpContext.Server.MapPath("~/Images/ensco.png");
            report.IrmaFile.Value = HttpContext.Server.MapPath("~/Images/irma.png");

            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.LifeBoatCompliance);
            report.DataSource = dataModel.GetAllItems();

            Session["currentReport"] = report;

            return RedirectToAction("ShowReport", "Report");
        }
         
        public ActionResult LifeBoatRosterListReport()
        {
            LifeBoatRosterListReport report = new LifeBoatRosterListReport();
            report.RigName.Value = Ensco.Utilities.UtilitySystem.Settings.RigName;
            report.LogoFile.Value = HttpContext.Server.MapPath("~/Images/ensco.png");
            report.IrmaFile.Value = HttpContext.Server.MapPath("~/Images/irma.png");

            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.LifeBoatRosterList);
            report.DataSource = dataModel.GetAllItems();

            Session["currentReport"] = report;

            return RedirectToAction("ShowReport", "Report");
        }
         
        public ActionResult RoomBedSummaryReport()
        {
            RoomBedSummaryReport report = new RoomBedSummaryReport();
            report.RigName.Value = Ensco.Utilities.UtilitySystem.Settings.RigName;
            report.LogoFile.Value = HttpContext.Server.MapPath("~/Images/ensco.png");
            report.IrmaFile.Value = HttpContext.Server.MapPath("~/Images/irma.png");

            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.RoomBedSummary);
            report.DataSource = dataModel.GetAllItems();

            Session["currentReport"] = report;

            return RedirectToAction("ShowReport", "Report");
        }
         
        public ActionResult FlightArrivalLogReport(int Id)
        {
            PobArrivalLogReport report = new PobArrivalLogReport();
            report.RigName.Value = Ensco.Utilities.UtilitySystem.Settings.RigName;
            report.LogoFile.Value = HttpContext.Server.MapPath("~/Images/ensco.png");
            report.IrmaFile.Value = HttpContext.Server.MapPath("~/Images/irma.png");

            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.FlightArrivalLog);

            report.DataSource = dataModel.GetItems(string.Format("CrewFlightId={0} AND StatusId=3", Id), "Id");

            Session["currentReport"] = report;

            return RedirectToAction("ShowReport", "Report");
        }
         
        public ActionResult FlightDepartureLogReport(int Id)
        {
            PobDepartureLogReport report = new PobDepartureLogReport();
            report.RigName.Value = Ensco.Utilities.UtilitySystem.Settings.RigName;
            report.LogoFile.Value = HttpContext.Server.MapPath("~/Images/ensco.png");
            report.IrmaFile.Value = HttpContext.Server.MapPath("~/Images/irma.png");

            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.FlightDepartureLog);

            report.DataSource = dataModel.GetItems(string.Format("CrewFlightId={0} AND StatusId=4", Id), "Id");

            Session["currentReport"] = report;

            return RedirectToAction("ShowReport", "Report");
        }
         
        public ActionResult FlightArrivalDepartureLogReport(int Id)
        {
            PobArrivalDepartureLogReport report = new PobArrivalDepartureLogReport();
            report.RigName.Value = Ensco.Utilities.UtilitySystem.Settings.RigName;
            report.LogoFile.Value = HttpContext.Server.MapPath("~/Images/ensco.png");
            report.IrmaFile.Value = HttpContext.Server.MapPath("~/Images/irma.png");

            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.FlightArrivalDepartureLog);

            report.DataSource = dataModel.GetItems(string.Format("CrewFlightId={0}", Id), "Id");

            Session["currentReport"] = report;

            return RedirectToAction("ShowReport", "Report");
        }
         
        public ActionResult CrewArrivalLogReport(int Id)
        {
            PobArrivalLogReport report = new PobArrivalLogReport();
            report.RigName.Value = Ensco.Utilities.UtilitySystem.Settings.RigName;
            report.LogoFile.Value = HttpContext.Server.MapPath("~/Images/ensco.png");
            report.IrmaFile.Value = HttpContext.Server.MapPath("~/Images/irma.png");

            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewArrivalLog);

            report.DataSource = dataModel.GetItems(string.Format("CrewChangeId={0} AND StatusId=3", Id), "Id");

            Session["currentReport"] = report;

            return RedirectToAction("ShowReport", "Report");
        }
         
        public ActionResult CrewDepartureLogReport(int Id)
        {
            PobArrivalLogReport report = new PobArrivalLogReport();
            report.RigName.Value = Ensco.Utilities.UtilitySystem.Settings.RigName;
            report.LogoFile.Value = HttpContext.Server.MapPath("~/Images/ensco.png");
            report.IrmaFile.Value = HttpContext.Server.MapPath("~/Images/irma.png");

            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewDepartureLog);

            report.DataSource = dataModel.GetItems(string.Format("CrewChangeId={0} AND StatusId=4", Id), "Id");

            Session["currentReport"] = report;

            return RedirectToAction("ShowReport", "Report");
        }
         
        public ActionResult CrewArrivalDepartureLogReport(int Id)
        {
            PobArrivalDepartureLogReport report = new PobArrivalDepartureLogReport();
            report.RigName.Value = Ensco.Utilities.UtilitySystem.Settings.RigName;
            report.LogoFile.Value = HttpContext.Server.MapPath("~/Images/ensco.png");
            report.IrmaFile.Value = HttpContext.Server.MapPath("~/Images/irma.png");

            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewArrivalDepartureLog);

            report.DataSource = dataModel.GetItems(string.Format("CrewChangeId={0}", Id), "Id");

            Session["currentReport"] = report;

            return RedirectToAction("ShowReport", "Report");
        }

         
        public ActionResult EmergencyTeams()
        {
            EmergencyTeamReport report = new EmergencyTeamReport();
            report.RigName.Value = Ensco.Utilities.UtilitySystem.Settings.RigName;
            report.LogoFile.Value = HttpContext.Server.MapPath("~/Images/ensco.png");
            report.IrmaFile.Value = HttpContext.Server.MapPath("~/Images/irma.png");

            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.EmergencyTeamReportView);

            List<dynamic> list = dataModel.GetAllItems();

            string yesUrl= HttpContext.Server.MapPath("~/Images/Yes.png");
            string noUrl = HttpContext.Server.MapPath("~/Images/No.png");

            foreach(EmergencyTeamReportModel item in list)
            {
                item.ImageUrlNo = System.IO.File.ReadAllBytes(HttpContext.Server.MapPath("~/Images/No.png"));
                item.ImageUrlYes = System.IO.File.ReadAllBytes(HttpContext.Server.MapPath("~/Images/Yes.png"));
            }
            report.DataSource = list;

            Session["currentReport"] = report;

            return RedirectToAction("ShowReport", "Report");
        }

        public ActionResult CapBookReport(string passportId)
        {
            CAP_BookReportModel model = new CAP_BookReportModel();
            model.CapBookEnscoLogo = HttpContext.Server.MapPath("~/Images/CapEnscoLogo.png");
            model.CapBookImage1 = HttpContext.Server.MapPath("~/Images/CapBook1.png");
            model.CapBookImage2 = HttpContext.Server.MapPath("~/Images/CapBook2.png");
            model.CapBookTlcLogo = HttpContext.Server.MapPath("~/Images/TlcLogo.png");

            model.CapBookPage1 = HttpContext.Server.MapPath("~/Images/CapBookPage1.png");
            model.CapBookPage2 = HttpContext.Server.MapPath("~/Images/CapBookPage2.png");
            model.CapBookPage3 = HttpContext.Server.MapPath("~/Images/CapBookPage3.png");
            model.CapBookPage4 = HttpContext.Server.MapPath("~/Images/CapBookPage4.png");
            model.CapBookPage5 = HttpContext.Server.MapPath("~/Images/CapBookPage5.png");
            model.CapBookPage6 = HttpContext.Server.MapPath("~/Images/CapBookPage6.png");
            model.CapBookPage7 = HttpContext.Server.MapPath("~/Images/CapBookPage7.png");
            model.CapBookPage8 = HttpContext.Server.MapPath("~/Images/CapBookPage8.png");
            model.CapBookPage9 = HttpContext.Server.MapPath("~/Images/CapBookPage9.png");
            model.CapBookPage10 = HttpContext.Server.MapPath("~/Images/CapBookPage10.png");

            // Get User Information
            LookupListModel<dynamic> lkpPosition = LookupListSystem.GetLookupList("Position");
            LookupListModel<dynamic> lkpDept = LookupListSystem.GetLookupList("Department");
            LookupListModel<dynamic> lkpBU = LookupListSystem.GetLookupList("BusinessUnit");

            UserModel user = ServiceSystem.GetUserFromPassport(passportId);
            model.Passport = passportId;
            model.Name = user.DisplayName;
            model.Position = (string)lkpPosition.GetDisplayValue(user.Position);
            model.Department = (string)lkpPosition.GetDisplayValue(user.Department);
            model.BusinessUnit = (string)lkpPosition.GetDisplayValue(user.BusinessUnit);
            model.RigFacility = UtilitySystem.Settings.RigName;
            if(user.Manager != null)
            {
                UserModel manager = ServiceSystem.GetUser((int)user.Manager);
                if(manager != null)
                {
                    model.Supervisor = manager.DisplayName;
                }
            }
            model.OIM = IrmaServiceSystem.GetAdminCustomValue("OIMName");

            CapBookReport report = new CapBookReport();

            RigCapService RigCapService = new RigCapService();
            IQueryable<CAP_BookModel> capBookItems = RigCapService.GetCAPBookQueryable().Where(x => x.EnscoPassportNo == passportId).OrderBy(x=>x.CompId);
            int compNo = 1;
            int ksaNo = 1;
            CAP_BookModel lastItem = null;
            foreach (CAP_BookModel item in capBookItems)
            {
                if(lastItem != null)
                {
                    if(lastItem.CompId != item.CompId)
                    {
                        compNo++;
                        ksaNo = 0;
                    }
                    ksaNo++;
                }
                item.CompetencyNumber = string.Format("{0}.0", compNo);
                item.KSANumber = string.Format("{0}.{1}", compNo, ksaNo);

                model.Items.Add(item);

                lastItem = (lastItem == null || lastItem.CompId != item.CompId) ? item : lastItem;
            }
            List<CAP_BookReportModel> list = new List<CAP_BookReportModel>();
            list.Add(model);
            report.DataSource = list;

            Session["currentReport"] = report;

            return RedirectToAction("ShowReport", "Report");
        }

        public ActionResult ShowReport()
        {
            DevExpress.XtraReports.UI.XtraReport currentReport = (DevExpress.XtraReports.UI.XtraReport)Session["currentReport"];
            return View(currentReport);
        }

        public ActionResult ShowReportPartial()
        {
            DevExpress.XtraReports.UI.XtraReport currentReport = (DevExpress.XtraReports.UI.XtraReport)Session["currentReport"];

            return PartialView("ShowReportPartial", currentReport);
        }

        public ActionResult ExportDocumentViewer(object model)
        {
            DevExpress.XtraReports.UI.XtraReport currentReport = (DevExpress.XtraReports.UI.XtraReport)Session["currentReport"];

            return DocumentViewerExtension.ExportTo(currentReport);
        }

        public ActionResult EmailReport()
        {
            DevExpress.XtraReports.UI.XtraReport currentReport = (DevExpress.XtraReports.UI.XtraReport)Session["currentReport"];

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
                    currentReport.ExportToPdf(memStream);

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
            catch(Exception ex)
            {

            }

            return View("ShowReportPartial", currentReport);
        }
    }
}
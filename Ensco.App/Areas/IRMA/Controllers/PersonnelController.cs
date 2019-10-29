using Ensco.App.App_Start;
using mUtilities.Data;
using System.Data;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Ensco.Irma.Models;
using Ensco.Irma.Services;
using Ensco.Models;
using Ensco.Services;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.DataExtensions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Ensco.App.Areas.IRMA.Controllers
{
    public class PersonnelController : Ensco.App.Areas.Common.Controllers.BaseController
    {
        // GET: IRMA/Personnel

        #region PobLanding
         
        public ActionResult Index() {
            PobHomeModel pobHomeModel = new PobHomeModel();

            Session["pobHomeModel"] = pobHomeModel;

            return View(pobHomeModel);
        }

        public ActionResult PobRigHomePartial() {
            PobHomeModel pobHomeModel = (PobHomeModel)Session["pobHomeModel"];

            return PartialView("PobRigHomePartial", pobHomeModel);
        }

        public ActionResult PobOverviewPartial() {
            PobHomeModel pobHomeModel = (PobHomeModel)Session["pobHomeModel"];

            IIrmaServiceDataModel summary = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.PobSummaryView);

            List<dynamic> items = summary.GetAllItems();
            int totalPob = 0;
            PobSummaryModel model = new PobSummaryModel();
            foreach (PobSummaryDataModel item in items) {
                totalPob += item.Count;
                switch (item.PersonnelType) {
                    case 165:
                        model.EnscoStandard = item.Count;
                        break;
                    case 166:
                        model.EnscoOther = item.Count;
                        break;
                    case 167:
                        model.EnscoService = item.Count;
                        break;
                    case 168:
                        model.EnscoCatering = item.Count;
                        break;
                    case 169:
                        model.Client = item.Count;
                        break;
                    case 170:
                        model.ClientService = item.Count;
                        break;
                }
            }
            model.EnscoPerContract = IrmaServiceSystem.GetMaxPOB();
            model.TotalPOB = totalPob;
            pobHomeModel.PobSummary.Add(model);

            return PartialView("PobOverviewPartial", pobHomeModel);
        }

        public ActionResult PobCurrentPartial() {
            PobHomeModel pobHomeModel = (PobHomeModel)Session["pobHomeModel"];

            pobHomeModel.PobCurrent = new DataTableModel(1, IrmaServiceSystem.GetQueryable(IrmaConstants.IrmaPobModels.CurrentPobView, "Id"));

            return PartialView("PobCurrentPartial", pobHomeModel);
        }
        #endregion
        #region CrewChange
         
        public ActionResult CrewChange() {
            ManageCrewChangeModel model = new ManageCrewChangeModel();

            Session["ManageCrewChangeModel"] = model;

            model.CrewChanges = new DataTableModel(1, IrmaServiceSystem.GetQueryable(IrmaConstants.IrmaPobModels.CrewChange, string.Format("Status!=11"), "Id"));

            return View(model);
        }

        public ActionResult CrewChangePartial() {
            ManageCrewChangeModel model = (ManageCrewChangeModel)Session["ManageCrewChangeModel"];

            return PartialView("CrewChangePartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CrewChangeAdd(CrewChangeModel model) {
            ManageCrewChangeModel manageModel = (ManageCrewChangeModel)Session["ManageCrewChangeModel"];

            if (ModelState.IsValid) {
                model.RigId = UtilitySystem.Settings.RigId;
                model = IrmaServiceSystem.Add(IrmaConstants.IrmaPobModels.CrewChange, model, true);
            } else {
                manageModel.SelectedCrewChange = model;
                ViewData["UpdateError"] = true;
            }
            return PartialView("CrewChangePartial", manageModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CrewChangeUpdate(CrewChangeModel model) {
            ManageCrewChangeModel manageModel = (ManageCrewChangeModel)Session["ManageCrewChangeModel"];

            if (ModelState.IsValid) {
                CrewChangeModel entityModel = manageModel.CrewChanges.GetItem(model.Id);
                entityModel.Title = model.Title;
                entityModel.InboundCrew = model.InboundCrew;
                entityModel.DateCrewChange = model.DateCrewChange;
                model = entityModel;
                bool result = IrmaServiceSystem.Save(IrmaConstants.IrmaPobModels.CrewChange, entityModel, true);
            } else {
                ViewData["UpdateError"] = true;
            }
            manageModel.SelectedCrewChange = model;
            return PartialView("CrewChangePartial", manageModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CrewChangeDelete(CrewChangeModel model) {
            ManageCrewChangeModel manageModel = (ManageCrewChangeModel)Session["ManageCrewChangeModel"];

            if (ModelState.IsValid) {
                CrewChangeModel entityModel = manageModel.CrewChanges.GetItem(model.Id);
                bool result = IrmaServiceSystem.Delete(IrmaConstants.IrmaPobModels.CrewChange, entityModel, true);
            } else {
                ViewData["UpdateError"] = true;
            }
            manageModel.SelectedCrewChange = null;

            return PartialView("CrewChangePartial", manageModel);
        }

        public ActionResult CrewChangeManifestsPartial(Nullable<int> CrewChangeId) {
            ManageCrewChangeModel manageModel = (ManageCrewChangeModel)Session["ManageCrewChangeModel"];

            CrewChangeModel crewChange = manageModel.CrewChanges.GetItem(CrewChangeId);
            if (crewChange != null) {
                IIrmaServiceDataModel flightDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewFlightManifestView);
                manageModel.SelectedCrewChange = crewChange;
                crewChange.CrewFlightInfo = new DataTableModel(crewChange.Id, flightDataModel.GetQueryable(string.Format("CrewChangeId={0}", crewChange.Id), "Id"));
            }

            return PartialView("CrewChangeManifestsPartial", manageModel.SelectedCrewChange);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ManifestAdd(CrewFlightModel model) {
            ManageCrewChangeModel manageModel = (ManageCrewChangeModel)Session["ManageCrewChangeModel"];

            if (ModelState.IsValid) {
                model.CrewChangeId = manageModel.SelectedCrewChange.Id;
                model.RigId = UtilitySystem.Settings.RigId;
                model = IrmaServiceSystem.Add(IrmaConstants.IrmaPobModels.CrewFlightManifest, model, true);
            } else {
                ManageCrewChangeModel.SelectedFlight = model;
                ViewData["UpdateError"] = true;
            }
            return PartialView("CrewChangeManifestsPartial", manageModel.SelectedCrewChange);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ManifestUpdate(CrewFlightModel model) {
            ManageCrewChangeModel manageModel = (ManageCrewChangeModel)Session["ManageCrewChangeModel"];

            if (ModelState.IsValid) {
                CrewFlightModel entityModel = manageModel.SelectedCrewChange.CrewFlightInfo.GetItem(model.Id);
                entityModel.Assign(model);
                entityModel.CrewChangeId = manageModel.SelectedCrewChange.Id;
                model.RigId = UtilitySystem.Settings.RigId;
                bool result = IrmaServiceSystem.Save(IrmaConstants.IrmaPobModels.CrewFlightManifest, entityModel, true);
            } else {
                ManageCrewChangeModel.SelectedFlight = model;
                ViewData["UpdateError"] = true;
            }
            return PartialView("CrewChangeManifestsPartial", manageModel.SelectedCrewChange);
        }

        public ActionResult ManifestDelete(CrewFlightModel model) {
            ManageCrewChangeModel manageModel = (ManageCrewChangeModel)Session["ManageCrewChangeModel"];

            if (ModelState.IsValid) {
                CrewFlightModel entityModel = manageModel.SelectedCrewChange.CrewFlightInfo.GetItem(model.Id);
                bool result = IrmaServiceSystem.Delete(IrmaConstants.IrmaPobModels.CrewFlightManifest, entityModel, true);
            } else {
                ManageCrewChangeModel.SelectedFlight = model;
                ViewData["UpdateError"] = true;
            }
            return PartialView("CrewChangeManifestsPartial", manageModel.SelectedCrewChange);
        }
        #endregion CrewChange

        public ActionResult CrewSearchReport(int Id) {
            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewChangeSearch);
            CrewChangeSearchModel model = dataModel.GetItem(string.Format("Id={0}", Id), "Id");

            return RedirectToAction("CrewChangeReport", "Personnel", new { Id = model.CrewChangeId });
        }

         
        public ActionResult CrewChangeReport2(Nullable<int> Id, string status = null) {
            if (status != null) {
                this.GetDataSet("exec usp_CrewChangeReportSubmit " + Id.ToString() + ", " + Utilities.UtilitySystem.CurrentUserId.ToString());
                return RedirectToAction("CrewSearchReport/" + Id.ToString());
            }
            ManageCrewChangeModel manageCrewChange = (ManageCrewChangeModel)Session["ManageCrewChangeModel"];

            manageCrewChange.CrewChanges = new DataTableModel(1, IrmaServiceSystem.GetQueryable(IrmaConstants.IrmaPobModels.CrewChange));
            manageCrewChange.SelectedCrewChange = manageCrewChange.CrewChanges.GetItem(Id);

            IIrmaServiceDataModel flightDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewFlightManifestView);
            if (manageCrewChange.SelectedCrewChange != null) {
                manageCrewChange.SelectedCrewChange.CrewFlightInfo = new DataTableModel(manageCrewChange.SelectedCrewChange.Id, flightDataModel.GetQueryable(string.Format("CrewChangeId={0}", manageCrewChange.SelectedCrewChange.Id), "Id"));
            }

            return View(manageCrewChange.SelectedCrewChange);
        }
         
        public ActionResult CrewChangeReport(Nullable<int> Id, string status = null) {
            if (status != null) {
                this.GetDataSet("exec usp_CrewChangeReportSubmit " + Id.ToString() + ", " + Utilities.UtilitySystem.CurrentUserId.ToString());
                return RedirectToAction("CrewChangeReport?Id=" + Id.ToString());
            }
            ManageCrewChangeModel manageCrewChange = new ManageCrewChangeModel();
            Session["ManageCrewChangeModel"] = manageCrewChange;
            manageCrewChange.CrewChanges = new DataTableModel(1, IrmaServiceSystem.GetQueryable(IrmaConstants.IrmaPobModels.CrewChange, string.Format("Status!=11"), "Id"));

            manageCrewChange.CrewChanges = new DataTableModel(1, IrmaServiceSystem.GetQueryable(IrmaConstants.IrmaPobModels.CrewChange));
            manageCrewChange.SelectedCrewChange = manageCrewChange.CrewChanges.GetItem(Id);

            IIrmaServiceDataModel flightDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewFlightManifestView);
            if (manageCrewChange.SelectedCrewChange != null) {
                manageCrewChange.SelectedCrewChange.CrewFlightInfo = new DataTableModel(manageCrewChange.SelectedCrewChange.Id, flightDataModel.GetQueryable(string.Format("CrewChangeId={0}", manageCrewChange.SelectedCrewChange.Id), "Id"));
            }

            return View(manageCrewChange.SelectedCrewChange);
        }
        public static DataSet GetTasks(string sourceFormUrl) {
            string sql = " exec usp_GetTasks " + UtilitySystem.CurrentMyUserId.ToString();
            sql += Convert(sourceFormUrl);
            return UtilitySystem.GetDataSet(sql);
        }
        public ActionResult UpdateTask(int id, string flag, string comment) {
            string sql = " exec usp_UpdateTask " + id.ToString() + ", '" + flag + "', '" + comment + "'";
            string url = UtilitySystem.GetDataSet(sql).Tables[0].Rows[0][0].ToString();
            return Redirect(url);
        }
        static string Convert(object obj) {
            if (obj == null)
                return ", null";
            return ", '" + obj.ToString() + "'";
        }
        [HttpPost]
        public ActionResult CrewChangeReport(CrewChangeModel model) {
            this.GetDataSet("exec usp_CrewChangeReportSubmit " + model.Id.ToString() + ", " + Utilities.UtilitySystem.CurrentUserId.ToString());
            return RedirectToAction("CrewChangeReport/" + model.Id.ToString());
        }
        public ActionResult CrewChangeReportPartial() {
            ManageCrewChangeModel manageCrewChange = (ManageCrewChangeModel)Session["ManageCrewChangeModel"];

            return PartialView("CrewChangeReportPartial", manageCrewChange.SelectedCrewChange);
        }

        public ActionResult CrewChangeReportFlightPartial(Nullable<int> Id) {
            ManageCrewChangeModel manageCrewChange = (ManageCrewChangeModel)Session["ManageCrewChangeModel"];
            CrewChangeModel crewChangeReport = manageCrewChange.SelectedCrewChange;

            IIrmaServiceDataModel flightDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewFlightManifestView);
            if (crewChangeReport != null) {
                crewChangeReport.CrewFlightInfo = new DataTableModel(crewChangeReport.Id, flightDataModel.GetQueryable(string.Format("CrewChangeId={0}", crewChangeReport.Id), "Id"));
            }

            return PartialView("CrewChangeReportFlightPartial", crewChangeReport);
        }

        public ActionResult CrewChangeReportInboundPartial(Nullable<int> Id) {
            ManageCrewChangeModel manageCrewChange = (ManageCrewChangeModel)Session["ManageCrewChangeModel"];
            CrewChangeModel crewChangeReport = manageCrewChange.SelectedCrewChange;

            IIrmaServiceDataModel pobDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewPobAll);

            crewChangeReport.InboundPersonnel = new DataTableModel(1, pobDataModel.GetQueryable(string.Format("CrewChangeId={0} AND StatusId=3", Id), "Id"));

            return PartialView("CrewChangeReportInboundPartial", crewChangeReport);
        }

        public ActionResult CrewChangeReportOutboundPartial(Nullable<int> Id) {
            ManageCrewChangeModel manageCrewChange = (ManageCrewChangeModel)Session["ManageCrewChangeModel"];

            IIrmaServiceDataModel pobDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewPobAll);
            CrewChangeModel crewChangeReport = manageCrewChange.SelectedCrewChange;

            crewChangeReport.OutboundPersonnel = new DataTableModel(1, pobDataModel.GetQueryable(string.Format("CrewChangeId={0} AND StatusId=4", Id), "Id"));

            return PartialView("CrewChangeReportOutboundPartial", crewChangeReport);
        }

        public ActionResult CrewChangeReportVerificationPartial(Nullable<int> Id) {
            ManageCrewChangeModel manageCrewChange = (ManageCrewChangeModel)Session["ManageCrewChangeModel"];

            CrewChangeModel crewChangeReport = manageCrewChange.SelectedCrewChange;

            IIrmaServiceDataModel approvalDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.ApprovalView);
            List<dynamic> list = approvalDataModel.GetItems(string.Format("Type={0} and RequestItemId={1}", ApprovalModel.ApprovalType.CrewChange, Id), "Id");
            crewChangeReport.Verification = list.Cast<ApprovalModel>().ToList();
            if (list.Count <= 0) {
                IIrmaServiceDataModel adminCustom = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewChangeApprovalView);
                List<dynamic> items = adminCustom.GetAllItems();
                IIrmaServiceDataModel approvalModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Approval);

                // Add Master
                CrewChangeApproverModel approver = items.FirstOrDefault(x => x.Name == "Master");
                ApprovalModel model = new ApprovalModel();
                model.Requester = UtilitySystem.CurrentUserId;
                model.RequestedDate = DateTime.Now;
                model.RequestItemId = crewChangeReport.Id;
                model.RequestInfo = Url.Action("CrewChangeVerificationStatusUpdate", "Personnel", new { Area = "IRMA" });
                model.Name = crewChangeReport.Title;
                model.Status = (int)CrewChangeModel.ActionStatus.Open;
                model.Type = (int)ApprovalModel.ApprovalType.CrewChange;
                model.Approver = approver.Id;
                model.Sequence = 1;
                model = approvalModel.Add(model, true);

                // Add OIM
                approver = items.FirstOrDefault(x => x.Name == "OIM");
                model = new ApprovalModel();
                model.Requester = UtilitySystem.CurrentUserId;
                model.RequestedDate = DateTime.Now;
                model.RequestItemId = crewChangeReport.Id;
                model.RequestInfo = Url.Action("CrewChangeVerificationStatusUpdate", "Personnel", new { Area = "IRMA" });
                model.Name = crewChangeReport.Title;
                model.Status = (int)CrewChangeModel.ActionStatus.Open;
                model.Type = (int)ApprovalModel.ApprovalType.CrewChange;
                model.Approver = approver.Id;
                model.Sequence = 2;
                model = approvalModel.Add(model, true);

                list = approvalDataModel.GetItems(string.Format("RequestItemId={0} and Type={1}", Id, ApprovalModel.ApprovalType.CrewChange), "Id");
                crewChangeReport.Verification = list.Cast<ApprovalModel>().ToList();
            }

            foreach (ApprovalModel model in crewChangeReport.Verification) {
                model.Initialize();
            }

            return PartialView("CrewChangeReportVerificationPartial", crewChangeReport);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CrewChangeApproverAddPartial(ApprovalModel model) {
            ManageCrewChangeModel manageCrewChange = (ManageCrewChangeModel)Session["ManageCrewChangeModel"];
            CrewChangeModel crewChangeReport = manageCrewChange.SelectedCrewChange;

            if (ModelState.IsValid) {
                IIrmaServiceDataModel approvalDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Approval);
                model.Requester = UtilitySystem.CurrentUserId;
                model.RequestedDate = DateTime.Now;
                model.RequestItemId = crewChangeReport.Id;
                model.RequestInfo = Url.Action("CrewChangeVerificationStatusUpdate", "Personnel", new { Area = "IRMA" });
                model.Name = crewChangeReport.Title;
                model.Status = (int)CrewChangeModel.ActionStatus.PendingApproval;
                model.Type = (int)ApprovalModel.ApprovalType.CrewChange;

                model = approvalDataModel.Add(model, true);

                List<dynamic> list = approvalDataModel.GetItems(string.Format("RequestItemId={0}", crewChangeReport.Id), "Id");
                crewChangeReport.Verification = list.Cast<ApprovalModel>().ToList();
                foreach (ApprovalModel item in crewChangeReport.Verification) {
                    item.Initialize();
                }
            } else {
                ViewData["UpdateError"] = true;
            }


            return PartialView("CrewChangeReportVerificationPartial", crewChangeReport);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CrewChangeApproverUpdatePartial(ApprovalModel model) {
            ManageCrewChangeModel manageCrewChange = (ManageCrewChangeModel)Session["ManageCrewChangeModel"];
            CrewChangeModel crewChangeReport = manageCrewChange.SelectedCrewChange;

            if (ModelState.IsValid) {
                IIrmaServiceDataModel approvalDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Approval);

                model.Requester = UtilitySystem.CurrentUserId;
                model.RequestedDate = DateTime.Now;
                model.RequestItemId = crewChangeReport.Id;
                model.Name = crewChangeReport.Title;
                model.Type = (int)ApprovalModel.ApprovalType.CrewChange;

                bool result = approvalDataModel.Update(model);

                List<dynamic> list = approvalDataModel.GetItems(string.Format("RequestItemId={0} and Type={1}", crewChangeReport.Id, ApprovalModel.ApprovalType.CrewChange), "Id");
                crewChangeReport.Verification = list.Cast<ApprovalModel>().ToList();
                foreach (ApprovalModel item in crewChangeReport.Verification) {
                    item.Initialize();
                }
            } else {
                ViewData["UpdateError"] = true;
            }
            return PartialView("CrewChangeReportVerificationPartial", crewChangeReport);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CrewChangeApproverDeletePartial(ApprovalModel model) {
            ManageCrewChangeModel manageCrewChange = (ManageCrewChangeModel)Session["ManageCrewChangeModel"];
            CrewChangeModel crewChangeReport = manageCrewChange.SelectedCrewChange;

            if (ModelState.IsValid) {
                IIrmaServiceDataModel approvalDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Approval);

                bool result = approvalDataModel.Delete(model);

                List<dynamic> list = approvalDataModel.GetItems(string.Format("RequestItemId={0} and Type={1}", crewChangeReport.Id, ApprovalModel.ApprovalType.CrewChange), "Id");
                crewChangeReport.Verification = list.Cast<ApprovalModel>().ToList();
                foreach (ApprovalModel item in crewChangeReport.Verification) {
                    item.Initialize();
                }
            } else {
                ViewData["UpdateError"] = true;
            }
            return PartialView("CrewChangeReportVerificationPartial", crewChangeReport);
        }

        public ActionResult CrewChangeVerificationStatusUpdate(int Id, int ApprovalId, int Approver, int Status) {
            ManageCrewChangeModel manageCrewChange = (ManageCrewChangeModel)Session["ManageCrewChangeModel"];
            CrewChangeModel crewChangeReport = manageCrewChange.SelectedCrewChange;

            manageCrewChange.CrewChanges = new DataTableModel(1, IrmaServiceSystem.GetQueryable(IrmaConstants.IrmaPobModels.CrewChange));
            crewChangeReport = manageCrewChange.CrewChanges.GetItem(Id);

            IIrmaServiceDataModel flightDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewFlightManifest);
            if (crewChangeReport != null) {
                crewChangeReport.CrewFlightInfo = new DataTableModel(crewChangeReport.Id, flightDataModel.GetQueryable(string.Format("CrewChangeId={0}", crewChangeReport.Id), "Id"));
            }

            IIrmaServiceDataModel statusDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Approval);
            ApprovalModel approval = statusDataModel.GetItem(string.Format("Id={0}", ApprovalId), "Id");
            if (approval != null) {
                approval.Status = Status;
                if (Status != 2)
                    approval.ApprovedDate = DateTime.Now;
                statusDataModel.Update(approval);
            }

            // Here check to see if all the approvers approved to close this CrewChange

            IIrmaServiceDataModel approvalDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.ApprovalView);
            List<dynamic> list = approvalDataModel.GetItems(string.Format("RequestItemId={0} and Type={1}", Id, ApprovalModel.ApprovalType.CrewChange), "Id");
            crewChangeReport.Verification = list.Cast<ApprovalModel>().ToList();
            foreach (ApprovalModel model in crewChangeReport.Verification) {
                model.Initialize();
            }

            // If approved, close the report 
            if (Status == (int)CrewChangeModel.ActionStatus.Approved) {
                CrewChangeModel entityModel = manageCrewChange.CrewChanges.GetItem(Id);
                entityModel.Status = (int)CrewChangeModel.ActionStatus.Closed;
                bool result = IrmaServiceSystem.Save(IrmaConstants.IrmaPobModels.CrewChange, entityModel, true);
            }

            return View("CrewChangeReport", crewChangeReport);
        }

        public ActionResult CloseCrewChange(int Id) {
            ManageCrewChangeModel manageCrewChange = (ManageCrewChangeModel)Session["ManageCrewChangeModel"];
            CrewChangeModel crewChangeReport = manageCrewChange.SelectedCrewChange;

            if (ModelState.IsValid) {
                bool okToClose = true;

                IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewPobAll);
                List<dynamic> items = dataModel.GetItems(string.Format("CrewChangeId={0} AND (StatusId=3 OR StatusId=4)", Id), "Id");
                int count = items.Count;
                if (count > 0) {
                    ModelState.AddModelError("CloseCrewChange", string.Format("Cannot close crew change. There are {0} outstanding personnel either arriving or departing from this crew change. Either onboard / offboard personnel or move to different crew change.", count));
                    okToClose = false;
                }

                // Discuss, if you can close on both Approved and Rejected, if more than one approver in the list.
                // If we cannot close, what should be done to make sure this crew change is closed.
                //

                //IIrmaServiceDataModel approvalDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.ApprovalView);
                //List<dynamic> list = approvalDataModel.GetItems(string.Format("RequestItemId={0}", Id), "Id");
                //List<ApprovalModel> approvals = list.Cast<ApprovalModel>().ToList();
                //foreach(ApprovalModel approval in approvals)
                //{
                //    if(approval.Status == )
                //}

                if (okToClose) {
                    IIrmaServiceDataModel crewModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewChange);
                    dynamic item = crewModel.GetItem(string.Format("Id={0}", Id), "Id");
                    if (item != null) {
                        CrewChangeModel model = item as CrewChangeModel;
                        model.Status = (int)CrewChangeModel.ActionStatus.Closed;
                        crewModel.Update(model);
                    }
                }
            }

            return View("CrewChangeReport", crewChangeReport);
        }

         
        public ActionResult FlightManifestReport(Nullable<int> Id) {
            ManageFlightManifestModel manifestModel = new ManageFlightManifestModel();
            Session["ManageFlightManifestModel"] = manifestModel;

            manifestModel.FlightManifests = new DataTableModel(2, IrmaServiceSystem.GetQueryable(IrmaConstants.IrmaPobModels.CrewFlightManifest));
            manifestModel.SelectedManifest = manifestModel.FlightManifests.GetItem(Id);

            return View(manifestModel.SelectedManifest);
        }

        public ActionResult FlightManifestReportPartial() {
            ManageFlightManifestModel manifestModel = (ManageFlightManifestModel)Session["ManageFlightManifestModel"];

            return PartialView("FlightManifestReportPartial", manifestModel.SelectedManifest);
        }

        public ActionResult FlightManifestArrivingPartial(Nullable<int> Id) {
            ManageFlightManifestModel manifestModel = (ManageFlightManifestModel)Session["ManageFlightManifestModel"];

            CrewFlightModel flightManifestModel = manifestModel.FlightManifests.GetItem(Id);

            manifestModel.SelectedManifest = (flightManifestModel != null) ? flightManifestModel : manifestModel.SelectedManifest;
            IIrmaServiceDataModel flightManifestPobDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.FlightPobAll);
            if (manifestModel.SelectedManifest != null) {
                manifestModel.SelectedManifest.ArrivingPersonnel = flightManifestPobDataModel.GetItems(string.Format("CrewFlightId={0} AND Status=3", flightManifestModel.Id), "Id");
                foreach (CrewFlightPobModel pob in manifestModel.SelectedManifest.ArrivingPersonnel) {
                    pob.PassportIdArriving = pob.PassportId;
                }
            }

            // Visible Fields
            IIrmaServiceDataModel reqModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.RigFieldVisible);
            RigFieldVisibilityModel item = (RigFieldVisibilityModel)reqModel.GetItem(string.Format("Id=11"), "Id");
            if (item != null && manifestModel.SelectedManifest != null) {
                manifestModel.SelectedManifest.ShowAirportInfo = item.Visible;
            }

            return PartialView("FlightManifestArrivingPartial", manifestModel.SelectedManifest);
        }

        public ActionResult FlightManifestDepartingPartial(Nullable<int> Id) {
            ManageFlightManifestModel manifestModel = (ManageFlightManifestModel)Session["ManageFlightManifestModel"];

            CrewFlightModel flightManifestModel = manifestModel.FlightManifests.GetItem(Id);

            manifestModel.SelectedManifest = (flightManifestModel != null) ? flightManifestModel : manifestModel.SelectedManifest;
            IIrmaServiceDataModel flightManifestPobDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.FlightPobAll);
            if (manifestModel.SelectedManifest != null) {
                manifestModel.SelectedManifest.DepartingPersonnel = flightManifestPobDataModel.GetItems(string.Format("CrewFlightId={0} AND Status=4", flightManifestModel.Id), "Id");
                foreach (CrewFlightPobModel pob in manifestModel.SelectedManifest.DepartingPersonnel) {
                    pob.PassportIdDeparting = pob.PassportId;
                }
            }

            // Visible Fields
            IIrmaServiceDataModel reqModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.RigFieldVisible);
            RigFieldVisibilityModel item = (RigFieldVisibilityModel)reqModel.GetItem(string.Format("Id=11"), "Id");
            if (item != null && manifestModel.SelectedManifest != null) {
                manifestModel.SelectedManifest.ShowAirportInfo = item.Visible;
            }

            return PartialView("FlightManifestDepartingPartial", manifestModel.SelectedManifest);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult FlightManifestReportArrivalAddPartial(CrewFlightPobModel model) {
            ManageFlightManifestModel manifestModel = (ManageFlightManifestModel)Session["ManageFlightManifestModel"];

            if (ModelState.IsValid) {
                IIrmaServiceDataModel pobDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewFlightManifestPob);

                model.Status = 3;
                model.CrewFlightId = manifestModel.SelectedManifest.Id;
                model.RigId = UtilitySystem.Settings.RigId;
                model.PassportId = model.PassportIdArriving;

                IIrmaServiceDataModel flightDataModel0 = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.FlightPobAll);
                List<dynamic> list = flightDataModel0.GetItems(string.Format("CrewFlightId={0} AND Status=3 and PassportId={1}", model.CrewFlightId, model.PassportId ), "Id");
                if(list.Count > 0) {
                   // this.IsMemberValid = false;
                    ViewData["UpdateFlightManifestArrivingPartialError"] = true;
                    return PartialView("FlightManifestArrivingPartial", manifestModel.SelectedManifest);
                }

                model = pobDataModel.Add(model);
                if (model != null) {
                    IIrmaServiceDataModel flightDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.FlightPobAll);
                    manifestModel.SelectedManifest.ArrivingPersonnel = flightDataModel.GetItems(string.Format("CrewFlightId={0} AND Status=3", model.CrewFlightId), "Id");
                    foreach (CrewFlightPobModel pob in manifestModel.SelectedManifest.ArrivingPersonnel) {
                        pob.PassportIdArriving = pob.PassportId;
                    }
                }
            }

            return PartialView("FlightManifestArrivingPartial", manifestModel.SelectedManifest);
        }
        public ManageFlightManifestModel GetFlightManifest(Nullable<int> Id) {
            ManageFlightManifestModel manifestModel = new ManageFlightManifestModel();
            manifestModel.FlightManifests = new DataTableModel(2, IrmaServiceSystem.GetQueryable(IrmaConstants.IrmaPobModels.CrewFlightManifest));
            manifestModel.SelectedManifest = manifestModel.FlightManifests.GetItem(Id);
            return manifestModel;
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult FlightManifestReportArrivalDeletePartial2(CrewFlightPobModel model) {
            ManageFlightManifestModel manifestModel = (ManageFlightManifestModel)Session["ManageFlightManifestModel"];

            if(ModelState.IsValid) {
                IIrmaServiceDataModel pobDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewFlightManifestPob);
                bool result = pobDataModel.Delete(model);
                if(result) {
                    if(manifestModel.SelectedManifest != null && manifestModel.SelectedManifest.ArrivingPersonnel != null)
                        manifestModel.SelectedManifest.ArrivingPersonnel.Remove(model);
                    ViewData["ItemDeleted"] = true;
                }
            }

            return PartialView("FlightManifestArrivingPartial", manifestModel.SelectedManifest);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult FlightManifestReportArrivalDeletePartial(CrewFlightPobModel model) {
            int flightId = int.Parse(this.GetDataSet("select  CrewFlightId from POB_CrewFlightPob where id=" + model.Id.ToString()).Tables[0].Rows[0][0].ToString());
            ManageFlightManifestModel manifestModel = this.GetFlightManifest(flightId);//  new ManageFlightManifestModel();// (ManageFlightManifestModel)Session["ManageFlightManifestModel"];

            if (ModelState.IsValid) {
                IIrmaServiceDataModel pobDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewFlightManifestPob);
                bool result = pobDataModel.Delete(model);
                if (result) {
                    if (manifestModel.SelectedManifest != null && manifestModel.SelectedManifest.ArrivingPersonnel != null)
                        manifestModel.SelectedManifest.ArrivingPersonnel.Remove(model);
                    ViewData["ItemDeleted"] = true;
                }
            }
            manifestModel = this.GetFlightManifest(flightId);
            return Redirect(this.Request.UrlReferrer.AbsoluteUri.ToString()); 
            return PartialView("FlightManifestArrivingPartial", manifestModel.SelectedManifest);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult FlightManifestReportDepartureAddPartial(CrewFlightPobModel model) {
            ManageFlightManifestModel manifestModel = (ManageFlightManifestModel)Session["ManageFlightManifestModel"];

            if (ModelState.IsValid) {
                IIrmaServiceDataModel pobDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewFlightManifestPob);

                model.Status = 4;
                model.CrewFlightId = manifestModel.SelectedManifest.Id;
                model.RigId = UtilitySystem.Settings.RigId;
                model.PassportId = model.PassportIdDeparting;

                IIrmaServiceDataModel flightDataModel0 = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.FlightPobAll);
                List<dynamic> list = flightDataModel0.GetItems(string.Format("CrewFlightId={0} AND Status=4 and PassportId={1}", model.CrewFlightId, model.PassportId), "Id");
                if(list.Count > 0) {
                    // this.IsMemberValid = false;
                    ViewData["UpdateFlightManifestDepartingPartialError"] = true;
                    return PartialView("FlightManifestDepartingPartial", manifestModel.SelectedManifest);
                }
                model = pobDataModel.Add(model);

                if (model != null) {
                    IIrmaServiceDataModel flightDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.FlightPobAll);
                    manifestModel.SelectedManifest.DepartingPersonnel = flightDataModel.GetItems(string.Format("CrewFlightId={0} AND Status=4", model.CrewFlightId), "Id");
                    foreach (CrewFlightPobModel pob in manifestModel.SelectedManifest.DepartingPersonnel) {
                        pob.PassportIdDeparting = pob.PassportId;
                    }
                }
            }

            return PartialView("FlightManifestDepartingPartial", manifestModel.SelectedManifest);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult FlightManifestReportDepartureDeletePartial(CrewFlightPobModel model) {
            ManageFlightManifestModel manifestModel = (ManageFlightManifestModel)Session["ManageFlightManifestModel"];

            if (ModelState.IsValid) {
                IIrmaServiceDataModel pobDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewFlightManifestPob);
                bool result = pobDataModel.Delete(model);
                if (result) {
                    if (manifestModel.SelectedManifest != null && manifestModel.SelectedManifest.DepartingPersonnel != null)
                        manifestModel.SelectedManifest.DepartingPersonnel.Remove(model);

                    ViewData["ItemDeleted"] = true;
                }
            }

            return PartialView("FlightManifestDepartingPartial", manifestModel.SelectedManifest);
        }

        [HttpPost]
        public ActionResult MoveToParkingLot(int Id, int CrewChangeId, int CrewFlightId, bool departure) {
            CrewFlightModel parkingLotModel = (CrewFlightModel)Session["ParkingLotModel"];
            if (parkingLotModel == null) {
                parkingLotModel = new CrewFlightModel();
                parkingLotModel.Id = 1;
                Session["ParkingLotModel"] = parkingLotModel;
            }
            if (ModelState.IsValid) {
                IIrmaServiceDataModel pobDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewFlightManifestPob);
                IIrmaServiceDataModel parkDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.ParkingLot);

                CrewFlightPobModel pob = pobDataModel.GetItem(string.Format("Id={0}", Id), "Id");
                ParkingLotModel park = parkDataModel.GetItem(string.Format("PassportId={0} and Status={1}", pob.PassportId, (departure) ? 4 : 3), "Id");
                if (pob != null) {
                    if (park == null) {
                        park = new ParkingLotModel();
                        pob.CopyProperties(park);
                        park.Id = 0;
                        park = parkDataModel.Add(park);
                    }

                    if (park.Id > 0) {
                        pobDataModel.Delete(pob);
                    }

                    if (departure) {
                        IIrmaServiceDataModel flightDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.FlightPobAll);
                        parkingLotModel.DepartingPersonnel = flightDataModel.GetItems(string.Format("CrewFlightId={0} AND Status=4", CrewFlightId), "Id");
                    } else {
                        IIrmaServiceDataModel flightDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.FlightPobAll);
                        parkingLotModel.ArrivingPersonnel = flightDataModel.GetItems(string.Format("CrewFlightId={0} AND Status=3", CrewFlightId), "Id");
                    }
                }
            }

            if (departure)
                return PartialView("FlightManifestDepartingPartial", parkingLotModel);
            else
                return PartialView("FlightManifestArrivingPartial", parkingLotModel);
        }

        #region ScheduleCrewChange
        public ActionResult ScheduleCrewChange() {
            ScheduleCrewChangeModel scheduleCrewChange = new ScheduleCrewChangeModel();
            Session["scheduleCrewChange"] = scheduleCrewChange;

            return View("ScheduleCrewChange", scheduleCrewChange);
        }

        public ActionResult ScheduleCrewChangePartial() {
            // ScheduleCrewChangeModel scheduleCrewChange = (ScheduleCrewChangeModel)Session["scheduleCrewChange"];
            ScheduleCrewChangeModel scheduleCrewChange = new ScheduleCrewChangeModel();
            return PartialView("ScheduleCrewChangePartial", scheduleCrewChange);
        }

        [HttpPost]
        public ActionResult ScheduleCrewChangePartial(ScheduleCrewChangeModel model) {
            if (ModelState.IsValid) {
                IrmaServiceSystem.ScheduleCrewChanges(model);
                model = new ScheduleCrewChangeModel();
            }
            return PartialView("ScheduleCrewChangePartial", model);
        }
        #endregion

        #region Onboarding
         
        [ValidateInput(false)]
        public ActionResult OnboardIndividual(Nullable<int> Id) {
            PobBoardingModel boardingModel = null;// (PobBoardingModel)Session["PobBoardingModel"];
            if (boardingModel == null) {
                boardingModel = new PobBoardingModel();
                Session["PobBoardingModel"] = boardingModel;
            }

            if (Id != null) {
                boardingModel.SelectedPob = IrmaServiceSystem.GetPobForIndividualFromPersonnel((int)Id);
                if (boardingModel.SelectedPob.Id == 0) {
                    boardingModel.SelectedPob = IrmaServiceSystem.GetPobForIndividualFromPassport((int)Id);
                }
            }

            if (boardingModel.SelectedPob != null) {
                boardingModel.SelectedPob.BatchOnboard = false;
            }

            return View(boardingModel);
        }
        public ActionResult OnboardIndividualPartial() {
            PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];

            return PartialView("OnboardIndividualPartial");
        }

        public ActionResult OnboardIndividualSelectPob() {
            PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];

            return PartialView("OnboardIndividualSelectPob", boardingModel.PobSelect);
        }

        [HttpPost]
        public ActionResult OnboardIndividualSelectPob(SelectPobModel model) {
            PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];

            if (ModelState.IsValid && model != null && model.SubmitAction != null) {
                switch ((SelectPobModel.PobType)model.SubmitAction) {
                    case SelectPobModel.PobType.CrewChangePob:
                        boardingModel.SelectedPob = IrmaServiceSystem.GetPobForIndividualFromPassport((int)model.PobCrewChange);
                        break;
                    case SelectPobModel.PobType.PassportPob:
                        boardingModel.SelectedPob = IrmaServiceSystem.GetPobForIndividualFromPassport((int)model.PobPassport);
                        break;
                }
                model = new SelectPobModel();
            }

            return PartialView("OnboardIndividualSelectPob", model);
        }
        public ActionResult OnboardIndividualCompanyChanged(Nullable<int> Id) {
            PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];

            if (Id != null && boardingModel.SelectedPob != null) {
                boardingModel.SelectedPob.CompanyType = Id;
            }
            return View("OnboardInvidualPobPartial", boardingModel.SelectedPob);
        }

        public ActionResult OnboardInvidualPobPartial(Nullable<int> Id) {
            PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];

            if (Id != null) {
                OnboardIndividualPobModel model = boardingModel.OnboardPersons.FirstOrDefault(x => x.Id == Id);
                if (model != null) {
                    model.BatchOnboard = true;
                    boardingModel.SelectedPob = model;
                }
            }

            return PartialView("OnboardInvidualPobPartial", boardingModel.SelectedPob);
        }

        [HttpPost]
        //public ActionResult OnboardIndividual(Nullable<int> Id)
        //        public ActionResult OnboardSaveIndividual(OnboardIndividualPobModel model)
        public ActionResult OnboardSaveIndividual([ModelBinder(typeof(DevExpressEditorsBinder))]  OnboardIndividualPobModel model) {
            PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];

            if (ModelState.IsValid) {
                string errors = "";
                if (model.PassportIssuer != null) {
                    if (model.PassportNumber == null) {
                        errors += string.Format("Passport number is required.\r\n");
                    }
                    if (model.PassportExpires == null) {
                        errors += string.Format("Passport expiration date is required.\r\n");
                    }
                }

                if (model.IdentificationIssuer != null) {
                    if (model.Identification == null) {
                        errors += string.Format("Identification number is required.\r\n");
                    }
                    if (model.IdentificationExpires == null) {
                        errors += string.Format("Identification expiration date is required.\r\n");
                    }
                }

                if (errors.Length > 0) {
                    ModelState.AddModelError("OnboardIndividualError", errors);
                } else {
                    model.DateEstimatedLeave = GetEndDate((int)model.RotationSchedule, (DateTime)model.DateOfArrival);
                    model.UsualRoom = (model.UsualRoom == null) ? model.Room : model.UsualRoom;
                    model.UsualBed = (model.UsualBed == null) ? model.Bed : model.UsualBed;
                    model = IrmaServiceSystem.OnboardIndividual(model);
                    boardingModel.SelectedPob = null;
                }
            } else {
                boardingModel.SelectedPob = model;
            }

            if (boardingModel.SelectedPob != null)
                boardingModel.SelectedPob.BatchOnboard = false;
            this.SetCookie("result", "1"); 
            return RedirectToAction("OnboardIndividual");
            return View("OnboardInvidualPobPartial", boardingModel.SelectedPob);
        }

        public DateTime GetEndDate(int type, DateTime dateStart) {
            DateTime dateEnd = dateStart;
            switch (type) {
                case 1:
                    dateEnd = dateEnd.AddDays(14);
                    break;
                case 2:
                    dateEnd = dateEnd.AddDays(21);
                    break;
                case 3:
                    dateEnd = dateEnd.AddDays(28);
                    break;
                case 4:
                    dateEnd = dateEnd.AddDays(35);
                    break;
            }
            return dateEnd;
        }

         
        public ActionResult OnboardBatch() {
            PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];
            if (boardingModel == null) {
                boardingModel = new PobBoardingModel();
                Session["PobBoardingModel"] = boardingModel;
            }
            boardingModel.Crew = false;

            return View(boardingModel);
        }

        public ActionResult OnboardBatchPartial() {
            PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];

            return PartialView("OnboardBatchPartial", boardingModel);
        }

        [HttpPost]
        public ActionResult OnboardBatchPartial(PobBoardingModel model) {
            PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];

            //IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.BatchOnboardView);

            //List<dynamic> items = dataModel.GetItems(string.Format("CrewFlightId={0}", model.FlightManifest), "Id");
            //boardingModel.OnboardPersons = items.Cast<OnboardIndividualPobModel>().ToList();
            boardingModel.OnboardPersons = IrmaServiceSystem.GetPersonnelForFlight((int)model.FlightManifest);
            boardingModel.CrewFlightId = (int)model.FlightManifest;

            return View("OnboardBatch", boardingModel);
        }

         
        public ActionResult OnboardFlight(int Id, int CrewChangeId) {
            PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];
            if (boardingModel == null) {
                boardingModel = new PobBoardingModel();
                Session["PobBoardingModel"] = boardingModel;
            }

            //IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.BatchOnboardView);

            //List<dynamic> items = dataModel.GetItems(string.Format("CrewFlightId={0}", Id), "Id");

            //boardingModel.OnboardPersons = items.Cast<OnboardIndividualPobModel>().ToList();
            boardingModel.OnboardPersons = IrmaServiceSystem.GetPersonnelForFlight(Id);

            boardingModel.Crew = true;
            boardingModel.CrewChangeId = CrewChangeId;
            boardingModel.CrewFlightId = Id;

            return View(boardingModel);
        }

        public ActionResult OnboardBatchPobPartial() {
            PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];

            return PartialView("OnboardBatchPobPartial", boardingModel);
        }

        public ActionResult OnboardBatchSelected(int id, bool selected) {
            PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];

            int index = boardingModel.OnboardPersons.FindIndex(x => x.Id == id);
            if (index >= 0 && index < boardingModel.OnboardPersons.Count) {
                OnboardIndividualPobModel model = boardingModel.OnboardPersons[index];
                model.Selected = selected;
            }
            return PartialView("OnboardBatchPobPartial", boardingModel);
        }

        [HttpPost]
        public ActionResult OnboardBatchUpdate(OnboardIndividualPobModel model) {
            PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];

            if (ModelState.IsValid) {
                string errors = "";
                if (model.PassportIssuer != null) {
                    if (model.PassportNumber == null) {
                        errors += string.Format("Passport number is required.\r\n");
                    }
                    if (model.PassportExpires == null) {
                        errors += string.Format("Passport expiration date is required.\r\n");
                    }
                }

                if (model.IdentificationIssuer != null) {
                    if (model.Identification == null) {
                        errors += string.Format("Identification number is required.\r\n");
                    }
                    if (model.IdentificationExpires == null) {
                        errors += string.Format("Identification expiration date is required.\r\n");
                    }
                }

                if (errors.Length > 0) {
                    ModelState.AddModelError("OnboardIndividualError", errors);
                } else {
                    OnboardIndividualPobModel entity = boardingModel.OnboardPersons.FirstOrDefault(x => x.Id == model.Id);
                    int index = boardingModel.OnboardPersons.FindIndex(x => x.Id == model.Id);

                    model.DateEstimatedLeave = GetEndDate((int)model.RotationSchedule, (DateTime)model.DateOfArrival);
                    model.UsualRoom = (model.UsualRoom == null) ? model.Room : model.UsualRoom;
                    model.UsualBed = (model.UsualBed == null) ? model.Bed : model.UsualBed;

                    model.Position = entity.Position;
                    model.Company = entity.Company;
                    model.Nationality = entity.Nationality;
                    model.Passport = entity.Passport;
                    model.DisplayName = entity.DisplayName;
                    model.CrewChangeId = entity.CrewChangeId;
                    model.CrewFlightId = entity.CrewFlightId;
                    model.Id = entity.Id;

                    boardingModel.OnboardPersons.Remove(entity);
                    boardingModel.OnboardPersons.Insert(index, model);
                    boardingModel.SelectedPob = model;
                    IrmaServiceSystem.OnboardIndividual(model);
                    //pobHomeModel.BoardPersonnel.SelectedPob = IrmaServiceSystem.OnboardIndividual(model);
                }
            } else {
                boardingModel.SelectedPob = model;
            }

            return PartialView("OnboardBatchPobPartial", boardingModel);
        }

        [ValidateInput(true)]
        public ActionResult OnboardBatchPersonnel() {
            PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];

            IrmaServiceSystem.OnboardBatch(boardingModel.OnboardPersons);

            //IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.BatchOnboardView);
            //List<dynamic> items = dataModel.GetItems(string.Format("CrewFlightId={0}", boardingModel.CrewFlightId), "Id");
            //boardingModel.OnboardPersons = items.Cast<OnboardIndividualPobModel>().ToList();
            boardingModel.OnboardPersons = IrmaServiceSystem.GetPersonnelForFlight(boardingModel.CrewFlightId);
            return Redirect("crewChange");
            if (boardingModel.Crew)
                return RedirectToAction("OnboardFlight", new { Id = boardingModel.CrewChangeId });
            else
                return RedirectToAction("OnboardBatch", new { Id = boardingModel.CrewFlightId, CrewChangeId = boardingModel.CrewChangeId });
        }

         
        public ActionResult OffboardBatch() {
            PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];
            if (boardingModel == null) {
                boardingModel = new PobBoardingModel();
                Session["PobBoardingModel"] = boardingModel;
            }
            boardingModel.Crew = false;

            return View(boardingModel);
        }

        public ActionResult OffboardPersonnelPartial() {
            PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];

            return PartialView(boardingModel);
        }

        [HttpPost]
        public ActionResult OffboardPersonnelPartial(PobBoardingModel model) {
            try {
                PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];

                if (model.FlightManifest != null) {
                    IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.BatchOffboardView);

                    List<dynamic> items = dataModel.GetItems(string.Format("CrewFlightId={0}", model.FlightManifest), "Id");
                    boardingModel.OffboardPersons = items.Cast<OffboardPobModel>().ToList();
                    boardingModel.CrewFlightId = (int)model.FlightManifest;
                }

                return View("OffboardPersonnel", boardingModel);
            } catch (Exception ex) {
                this.da.GetDataSet("insert tbl_error select getdate(), '" + ex.Message.ToString() + ex.StackTrace.ToString());
                return View();
            }
        }


         
        public ActionResult OffboardFlight(int Id) {
            PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];
            if (boardingModel == null) {
                boardingModel = new PobBoardingModel();
                Session["PobBoardingModel"] = boardingModel;
            }
            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.BatchOffboardView);

            List<dynamic> items = dataModel.GetItems(string.Format("CrewFlightId={0}", Id), "Id");
            boardingModel.OffboardPersons = items.Cast<OffboardPobModel>().ToList();
            boardingModel.Crew = true;

            return View(boardingModel);
        }

        public ActionResult OffboardPersonnelPobPartial() {
            PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];

            return PartialView("OffboardPersonnelPobPartial", boardingModel);
        }

        public ActionResult OffboardSelected(int id, bool selected) {
            PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];

            int index = boardingModel.OffboardPersons.FindIndex(x => x.Id == id);
            if (index >= 0 && index < boardingModel.OffboardPersons.Count) {
                OffboardPobModel model = boardingModel.OffboardPersons[index];
                model.Selected = selected;
            }
            return PartialView("OffboardPersonnelPartial", boardingModel);
        }

        public ActionResult OffboardPersonnelSelected() {
            PobBoardingModel boardingModel = (PobBoardingModel)Session["PobBoardingModel"];
            int flightId = boardingModel.CrewFlightId; 
            if (ModelState.IsValid) {
                if (IrmaServiceSystem.OffboardBatch(boardingModel.OffboardPersons)) {
                }
            }
            return Redirect("crewChange"); 

            if (boardingModel.Crew)
                return RedirectToAction("OffboardFlight", new { Id = boardingModel.CrewChangeId });
            else
                return RedirectToAction("OffboardBatch", new { Id = boardingModel.CrewFlightId, CrewChangeId = boardingModel.CrewChangeId });
        }

        #region ScheduleCrewChange
        public ActionResult MovePersonnel() {
            MovePersonnelModel movePersonnel = new MovePersonnelModel();
            Session["MovePersonnelModel"] = movePersonnel;

            return View("MovePersonnel", movePersonnel);
        }

        public ActionResult MovePersonnelPartial() {
            MovePersonnelModel movePersonnel = (MovePersonnelModel)Session["MovePersonnelModel"];

            return PartialView("MovePersonnelPartial", movePersonnel);
        }

        [HttpPost]
        public ActionResult MovePersonnelPartial(MovePersonnelModel model) {
            CrewFlightModel parkingLotModel = (CrewFlightModel)Session["ParkingLotModel"];

            if (ModelState.IsValid && model.FlightManifest != null) {
                IIrmaServiceDataModel parkingModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.ParkingLot);
                IIrmaServiceDataModel pobDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewFlightManifestPob);
                List<FlightManifestPobModel> pobsDeleted = new List<FlightManifestPobModel>();
                foreach (FlightManifestPobModel pob in parkingLotModel.Personnel) {
                    if (!pob.Selected)
                        continue;

                    CrewFlightPobModel entityModel = new CrewFlightPobModel();
                    pob.CopyProperties(entityModel);
                    entityModel.Id = 0;
                    entityModel.CrewFlightId = (int)model.FlightManifest;
                    entityModel.RigId = UtilitySystem.Settings.RigId;
                    entityModel = pobDataModel.Add(entityModel);
                    if (entityModel.Id != 0) {
                        ParkingLotModel park = parkingModel.GetItem(string.Format("Id={0}", pob.Id), "Id");
                        parkingModel.Delete(park);
                        pobsDeleted.Add(pob);
                    }
                }

                foreach (FlightManifestPobModel pob in pobsDeleted) {
                    parkingLotModel.Personnel.Remove(pob);
                }
                model.FlightManifest = null;
            }
            return RedirectToAction("FlightParkingLot", "Personnel", new { Area = "IRMA" });
            return PartialView("MovePersonnelPartial", model);
        }
        #endregion

        public ActionResult FlightParkingLotAddArriving() {
            ParkingLotPobModel model = (ParkingLotPobModel)Session["ParkingLotPobModel"];
            if (model == null) {
                model = new ParkingLotPobModel();
                model.Status = 3;
                Session["ParkingLotPobModel"] = model;
            }

            return View(model);
        }
        public ActionResult FlightParkingLotAddArrivingPartial() {
            ParkingLotPobModel model = (ParkingLotPobModel)Session["ParkingLotPobModel"];
            if (model == null) {
                model = new ParkingLotPobModel();
                model.Status = 3;
                Session["ParkingLotPobModel"] = model;
            }

            return PartialView("FlightParkingLotAddArrivingPartial", model);
        }

        [HttpPost]
        public ActionResult FlightParkingLotAddArrivingPartial(ParkingLotPobModel model) {
            if (ModelState.IsValid && model.Arriving != null) {
                IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.ParkingLot);
                ParkingLotModel entityModel = new ParkingLotModel();
                entityModel.PassportId = (int)model.Arriving;
                entityModel.ModifiedBy = UtilitySystem.CurrentUserId;
                entityModel.ModifiedDate = DateTime.Now;
                entityModel.Status = 3;
                entityModel = dataModel.Add(entityModel);
                model.Arriving = null;
                return RedirectToAction("FlightParkingLot", "Personnel", new { Area = "IRMA" });
            }

            return PartialView("FlightParkingLotAddArrivingPartial", model);
        }

        public ActionResult FlightParkingLotAddDeparting() {
            ParkingLotPobModel model = (ParkingLotPobModel)Session["ParkingLotPobModel"];
            if (model == null) {
                model = new ParkingLotPobModel();
                model.Status = 4;
                Session["ParkingLotPobModel"] = model;
            }

            return View(model);
        }
        public ActionResult FlightParkingLotAddDepartingPartial() {
            ParkingLotPobModel model = (ParkingLotPobModel)Session["ParkingLotPobModel"];
            if (model == null) {
                model = new ParkingLotPobModel();
                model.Status = 4;
                Session["ParkingLotPobModel"] = model;
            }

            return PartialView("FlightParkingLotAddDepartingPartial", model);
        }

        [HttpPost]
        public ActionResult FlightParkingLotAddDepartingPartial(ParkingLotPobModel model) {
            if (ModelState.IsValid && model.Departing != null) {
                IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.ParkingLot);
                ParkingLotModel entityModel = new ParkingLotModel();
                entityModel.PassportId = (int)model.Departing;
                entityModel.ModifiedBy = UtilitySystem.CurrentUserId;
                entityModel.ModifiedDate = DateTime.Now;
                entityModel.Status = 4;
                entityModel = dataModel.Add(entityModel);
                model.Departing = null;
                return RedirectToAction("FlightParkingLot", "Personnel", new { Area = "IRMA" });
            }

            return PartialView("FlightParkingLotAddDepartingPartial", model);
        }

         
        public ActionResult TeamManagement() {
            TeamManagementModel teamManageModel = new TeamManagementModel();
            Session["TeamManagementModel"] = teamManageModel;

            teamManageModel.Teams = new DataTableModel(1, IrmaServiceSystem.GetQueryable(IrmaConstants.IrmaPobModels.TeamView));

            return View("TeamManagement", teamManageModel);
        }

        public ActionResult TeamManagementPartial() {
            TeamManagementModel teamManageModel = (TeamManagementModel)Session["TeamManagementModel"];

            return PartialView("TeamManagementPartial", teamManageModel);
        }

        public ActionResult TeamManagementDetailPartial(Nullable<int> Id) {
            TeamManagementModel teamManageModel = (TeamManagementModel)Session["TeamManagementModel"];

            TeamModel teamModel = (TeamModel)teamManageModel.Teams.GetItem(Id);

            teamManageModel.SelectedTeam = teamModel;
            if (teamModel != null) {
                teamModel.Members = IrmaServiceSystem.GetTeamMembers(teamModel.Id);
            }
            return PartialView("TeamManagementDetailPartial", teamManageModel.SelectedTeam);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TeamMemberAdd(TeamMemberModel model) {
            TeamManagementModel teamManageModel = (TeamManagementModel)Session["TeamManagementModel"];

            if(ModelState.IsValid) {
                model.TeamId = teamManageModel.SelectedTeam.Id;
                List<TeamMemberModel> list = IrmaServiceSystem.GetTeamMembers(model.TeamId);
                var query = list.Where(person => person.Passport == model.Passport);
                if(query.Count() > 0)
                    ViewData["UpdateError"] = true;
                else {
                    model = IrmaServiceSystem.Add(IrmaConstants.IrmaPobModels.TeamMembers, model, true);
                    teamManageModel.SelectedTeam.Members = IrmaServiceSystem.GetTeamMembers(teamManageModel.SelectedTeam.Id);
                }
            } else {
                //pobHomeModel.EmergencyTeams.SelectedTeam = model;
                ViewData["UpdateError"] = true;
            }
            return PartialView("TeamManagementDetailPartial", teamManageModel.SelectedTeam);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TeamMemberUpdate(TeamMemberModel model) {
            TeamManagementModel teamManageModel = (TeamManagementModel)Session["TeamManagementModel"];

            if (ModelState.IsValid) {
                model.TeamId = teamManageModel.SelectedTeam.Id;

                IrmaServiceSystem.Update(IrmaConstants.IrmaPobModels.TeamMembers, model, true);
            } else {
                //pobHomeModel.EmergencyTeams.SelectedTeam = model;
                ViewData["UpdateError"] = true;
            }
            return PartialView("TeamManagementDetailPartial", teamManageModel.SelectedTeam);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TeamMemberDelete(TeamMemberModel model) {
            TeamManagementModel teamManageModel = (TeamManagementModel)Session["TeamManagementModel"];

            if (ModelState.IsValid) {
                IrmaServiceSystem.Delete(IrmaConstants.IrmaPobModels.TeamMembers, model, true);
                teamManageModel.SelectedTeam.Members = IrmaServiceSystem.GetTeamMembers(teamManageModel.SelectedTeam.Id);
            } else {
                //pobHomeModel.EmergencyTeams.SelectedTeam = model;
                ViewData["UpdateError"] = true;
            }
            return PartialView("TeamManagementDetailPartial", teamManageModel.SelectedTeam);
        }

         
        public ActionResult TourManagement() {
            TourManagementModel tourManagement = (TourManagementModel)Session["tourManagement"];

            if (tourManagement == null) {
                tourManagement = new TourManagementModel();

                Session["tourManagement"] = tourManagement;
            }

            List<TourPobModel> currentList = tourManagement.CurrentPob;
            IIrmaServiceDataModel tourModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.TourMangement);
            if (currentList != null) {
                List<dynamic> list = tourModel.GetAllItems();
                tourManagement.CurrentPob = list.Cast<TourPobModel>().ToList();

                foreach (TourPobModel pob in currentList) {
                    TourPobModel item = tourManagement.CurrentPob.FirstOrDefault(x => x.Id == pob.Id);
                    if (item != null)
                        item.Selected = pob.Selected;
                }
            } else {
                List<dynamic> list = tourModel.GetAllItems();
                tourManagement.CurrentPob = list.Cast<TourPobModel>().ToList();
            }

            return View(tourManagement);
        }
        public ActionResult TourManagementPartial() {
            TourManagementModel tourManagement = (TourManagementModel)Session["tourManagement"];

            return PartialView("TourManagementPartial", tourManagement);
        }

        public ActionResult TourManagementChangePartial() {
            TourManagementModel tourManagement = (TourManagementModel)Session["tourManagement"];

            return PartialView("TourManagementChangePartial", tourManagement);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TourManagementUpdatePartial(TourPobModel model) {
            TourManagementModel tourManagement = (TourManagementModel)Session["tourManagement"];

            if (ModelState.IsValid) {
                IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.RigPersonnel);
                RigPersonnelModel entityModel = dataModel.GetItem(string.Format("Id={0}", model.Id), "Id");
                entityModel.Tour = model.Tour;
                dataModel.Update(entityModel);

                IIrmaServiceDataModel tourModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.TourMangement);
                List<dynamic> list = tourModel.GetAllItems();
                tourManagement.CurrentPob = list.Cast<TourPobModel>().ToList();
            }

            return PartialView("TourManagementPartial", tourManagement);
        }

        [HttpPost]
        public ActionResult TourSelected(int id, bool selected) {
            TourManagementModel tourManagement = (TourManagementModel)Session["tourManagement"];

            int index = tourManagement.CurrentPob.FindIndex(x => x.Id == id);
            if (index >= 0 && index < tourManagement.CurrentPob.Count) {
                TourPobModel model = tourManagement.CurrentPob[index];
                model.Selected = selected;
            }

            return PartialView("TourManagementPartial", tourManagement);
        }

        public ActionResult TourChange() {
            TourManagementModel tourManagement = (TourManagementModel)Session["tourManagement"];

            return View(tourManagement);
        }

        public ActionResult TourChangePartial() {
            TourManagementModel tourManagement = (TourManagementModel)Session["tourManagement"];

            return PartialView("TourChangePartial", tourManagement);
        }

        [HttpPost]
        public ActionResult TourChangePartial(TourManagementModel model) {
            TourManagementModel tourManagement = (TourManagementModel)Session["tourManagement"];

            if (ModelState.IsValid) {
                if (model.TourChange != null) {
                    IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.RigPersonnel);
                    IIrmaServiceDataModel tourModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.TourChange);

                    foreach (TourPobModel pob in tourManagement.CurrentPob) {
                        if (!pob.Selected)
                            continue;

                        RigPersonnelModel entityModel = dataModel.GetItem(string.Format("Id={0}", pob.Id), "Id");

                        if (model.TourChangeNow) {
                            if (entityModel != null) {
                                entityModel.Tour = model.TourChange;
                                dataModel.Update(entityModel);
                            }
                        }

                        if (model.TourChangeFrom) {
                            TourChangeModel item = new TourChangeModel();
                            item.ModifiedBy = UtilitySystem.CurrentUserId;
                            item.ModifiedDate = DateTime.Now;
                            item.PobId = entityModel.Id;
                            item.DateFrom = model.TourChangeDate;
                            item.ShortChange = false;
                            item.TourFrom = (int)entityModel.Tour;
                            item.TourTo = (int)model.TourChange;
                            item = tourModel.Add(item);
                        }

                        if (model.ShortChange) {
                            TourChangeModel item = new TourChangeModel();
                            item.ModifiedBy = UtilitySystem.CurrentUserId;
                            item.ModifiedDate = DateTime.Now;
                            item.PobId = entityModel.Id;
                            item.DateFrom = model.ShortChangeBeginDate;
                            item.DateTo = model.ShortChangeEndDate;
                            item.ShortChange = true;
                            item.TourFrom = (int)entityModel.Tour;
                            item.TourTo = (int)model.TourChange;
                            item = tourModel.Add(item);
                        }

                        pob.Selected = false;
                    }
                    model.ShortChange = false;
                    model.TourChangeFrom = false;
                }

                IIrmaServiceDataModel tourDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.TourMangement);
                List<dynamic> list = tourDataModel.GetAllItems();
                tourManagement.CurrentPob = list.Cast<TourPobModel>().ToList();

                HttpCookie c = new HttpCookie("tourChanged");
                c.Value = "1";
                this.Response.Cookies.Add(c);
                return Redirect("TourManagement");
            }
            return PartialView("TourChangePartial", tourManagement);
        }

         
        public ActionResult FlightParkingLot() {
            CrewFlightModel parkingLotModel = (CrewFlightModel)Session["ParkingLotModel"];
            if (parkingLotModel == null) {
                parkingLotModel = new CrewFlightModel();
                parkingLotModel.Id = 1;
                Session["ParkingLotModel"] = parkingLotModel;
            }

            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.FlightParkingLotView);
            List<dynamic> existing = parkingLotModel.Personnel;

            parkingLotModel.Personnel = dataModel.GetAllItems();
            if (existing != null) {
                foreach (FlightManifestPobModel pob in existing) {
                    FlightManifestPobModel plpob = parkingLotModel.Personnel.FirstOrDefault(x => x.Id == pob.Id);
                    if (plpob != null) {
                        plpob.Selected = pob.Selected;
                    }
                }
            }
            // Visible Fields
            IIrmaServiceDataModel reqModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.RigFieldVisible);
            RigFieldVisibilityModel item = (RigFieldVisibilityModel)reqModel.GetItem(string.Format("Id=11"), "Id");
            if (item != null && parkingLotModel != null) {
                parkingLotModel.ShowAirportInfo = item.Visible;
            }

            return View("FlightParkingLot", parkingLotModel);
        }
        public ActionResult FlightParkingLotPartial() {
            CrewFlightModel parkingLotModel = (CrewFlightModel)Session["ParkingLotModel"];

            return PartialView("FlightParkingLotPartial", parkingLotModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult FlightParkingLotPobAdd(FlightManifestPobModel model) {
            CrewFlightModel parkingLotModel = (CrewFlightModel)Session["ParkingLotModel"];

            if (ModelState.IsValid) {
                try {
                    IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.ParkingLot);

                    ParkingLotModel entityModel = new ParkingLotModel();
                    model.CopyProperties(entityModel);

                    entityModel.ModifiedBy = UtilitySystem.CurrentUserId;
                    entityModel.ModifiedDate = DateTime.Now;

                    entityModel = dataModel.Add(entityModel);
                    model.Id = entityModel.Id;
                    model.Status = (int)entityModel.Status;
                } catch (Exception ex) {

                }
            } else {
                ViewData["UpdateError"] = true;
            }
            return PartialView("FlightParkingLotPartial", parkingLotModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult FlightParkingLotPobUpdate(FlightManifestPobModel model) {
            CrewFlightModel parkingLotModel = (CrewFlightModel)Session["ParkingLotModel"];

            if (ModelState.IsValid) {
                try {
                    IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.ParkingLot);

                    ParkingLotModel entityModel = new ParkingLotModel();
                    entityModel.ModifiedBy = UtilitySystem.CurrentUserId;
                    entityModel.ModifiedDate = DateTime.Now;
                    model.CopyProperties(entityModel);

                    bool result = dataModel.Update(entityModel);
                } catch (Exception ex) {

                }
            } else {
                ViewData["UpdateError"] = true;
            }
            return PartialView("FlightParkingLotPartial", parkingLotModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult FlightParkingLotPobDelete(FlightManifestPobModel model) {
            CrewFlightModel parkingLotModel = (CrewFlightModel)Session["ParkingLotModel"];

            if (ModelState.IsValid) {
                try {
                    IIrmaServiceDataModel parkingLotDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.ParkingLot);
                    ParkingLotModel entityModel = parkingLotDataModel.GetItem(string.Format("Id={0}", model.Id), "Id");
                    if (entityModel != null) {
                        parkingLotDataModel.Delete(entityModel);
                    }
                } catch (Exception ex) {
                }
            } else {
                ViewData["UpdateError"] = true;
            }
            return PartialView("FlightParkingLotPartial", parkingLotModel);
        }
        [HttpPost]
        public ActionResult FlightParkingLotPobSelect(int id, bool selected) {
            CrewFlightModel parkingLotModel = (CrewFlightModel)Session["ParkingLotModel"];

            FlightManifestPobModel item = parkingLotModel.Personnel.FirstOrDefault(x => x.Id == id);
            if (item != null) {
                item.Selected = selected;
            }

            return PartialView("FlightParkingLotPartial", parkingLotModel);
        }

        #endregion
         

        public ActionResult CrewChangeSearch() {
            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewChangeSearch);
            DataTableModel model = new DataTableModel(1, dataModel.GetQueryable("Id"));

            return View(model);
        }

        public ActionResult CrewChangeSearchPartial() {
            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewChangeSearch);
            DataTableModel model = new DataTableModel(1, dataModel.GetQueryable("Id"));

            return PartialView("CrewChangeSearchPartial", model);
        }
        public ActionResult GetPersonnelType(int id) {
            return this.GetJson(" exec usp_GetPersonnelType " + id.ToString());
        }
        public ActionResult GetDisplayName(int passportId) {
            return this.GetJson(" exec usp_GetDisplayName " + passportId.ToString());
        }
    }
}

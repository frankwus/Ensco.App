using AutoMapper;
using DevExpress.Web.Mvc;
using Ensco.App.Areas.Oap.Controllers;
using Ensco.App.ProjectUtilities;
using Ensco.FSO.Models;
using Ensco.FSO.Services;
using Ensco.Irma.Models;
using Ensco.Irma.Oap.Common.Models;
using Ensco.Irma.Oap.Web.Oap.Controllers;
using Ensco.Irma.Oap.WebClient.Rig;
using Ensco.Irma.Services;
using Ensco.Models;
using Ensco.Services;
using Ensco.TLC.Models;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Westwind.Globalization;

namespace Ensco.App.Areas.IRMA.Controllers
{
    public class FSODashboardController : GenericDashboardController
    {
 
        public FSODashboardController() : base()
        {
        }

        private OapChecklist corpFSOChecklist;

        // GET: IRMA/Fso
        public override ActionResult Index()
        {
            OapChecklist corpFSOChecklist = GetCorporateFSOChecklist();
            if (corpFSOChecklist == null)
            {
                ViewBag.CorpChecklistNull = "The definition of an FSO Checklist was not found in the system. Please create it in Corporate OAP first";
                return View("Index");
            }

            IEnumerable<OpenFSOGridModel> modelList = GetOpenFsoGridModelList(corpFSOChecklist);
            ViewBag.OpenChecklists = modelList;

            List<TrainedObserversViewModel> trainedObservers = GetTrainedObservers(corpFSOChecklist);
            ViewBag.TrainedObservers = trainedObservers;

            return View("Index");
        }

        public ActionResult IndexOpenChecklistsPartial()
        {
            OapChecklist corpFSOChecklist = GetCorporateFSOChecklist();
            IEnumerable<OpenFSOGridModel> modelList = GetOpenFsoGridModelList(corpFSOChecklist);
            return PartialView("IndexOpenChecklistsPartial", modelList);
        }

        [HttpPost]
        public ActionResult IndexOpenChecklistsAddPartial(int OwnerId, string Title, DateTime ChecklistDateTime)
        {
            if (ModelState.IsValid)
            {
                CreateFSOChecklist(OwnerId, Title, ChecklistDateTime);
            }
            return IndexOpenChecklistsPartial();
        }

        public ActionResult TrainedObserversOnboard()
        {
            OapChecklist corpFSOChecklist = GetCorporateFSOChecklist();
            List<TrainedObserversViewModel> trainedObservers = GetTrainedObservers(corpFSOChecklist);
            ViewBag.TrainedObservers = trainedObservers;
            return PartialView("TrainedObserversOnboardPartial");
        }

        private RigOapChecklist CreateFSOChecklist(int OwnerId, string Title, DateTime ChecklistDateTime)
        {
            OapChecklist oapChecklist = OapChecklistClient.GetByNameAsync("FSO Checklist").Result?.Result?.Data;
            if (oapChecklist == null)
            {
                ViewData["UpdateError"] = true;
                return null;
            }

            RigOapChecklist checklist = new RigOapChecklist();
            checklist.CreatedBy = UtilitySystem.CurrentUserName;
            checklist.OwnerId = OwnerId; //UtilitySystem.CurrentUserId;
            checklist.CreatedDateTime = DateTime.UtcNow;
            checklist.ChecklistDateTime = ChecklistDateTime;//DateTime.UtcNow;
            checklist.Title = Title;
            checklist.UpdatedDateTime = DateTime.UtcNow;
            checklist.Status = "Open";
            
            checklist.OapchecklistId = oapChecklist.Id;
             
            var newChecklist = RigOapChecklistClient.AddRigChecklistAsync(checklist).Result?.Result?.Data;
            return newChecklist;
        }

        // GET: IRMA/Checklist/1
        public ActionResult Checklist(int id)
        {
            RigOapChecklist checklist = RigOapChecklistClient.GetByUniqueIdAsync(id).Result?.Result?.Data;

            if (checklist == null)
                return RedirectToAction("Index");

            return View(checklist);
        }

        private IEnumerable<OpenFSOGridModel> GetOpenFsoGridModelList(OapChecklist corpFSOChecklist)
        {
            IEnumerable<RigOapChecklist> checklists = GetOpenFsoChecklists(corpFSOChecklist);
            List<OpenFSOGridModel> modelList = new List<OpenFSOGridModel>();
            foreach (var checklist in checklists)
            {
                UserModel user = ServiceSystem.GetUser(checklist.OwnerId);
                PositionModel position = ServiceSystem.GetUserPosition(checklist.OwnerId);

                OpenFSOGridModel gridModel = new OpenFSOGridModel()
                {
                    Id = checklist.Id,
                    Title = checklist.Title,
                    ChecklistDateTime = checklist.ChecklistDateTime,
                    RigChecklistUniqueId = checklist.RigChecklistUniqueId,
                    Observer = user?.DisplayName,
                    Position = position?.Name,
                    Company = user?.Company,
                    Location = user?.RigId
                };

                modelList.Add(gridModel);
            }

            return modelList;
        }

        private List<TrainedObserversViewModel> GetTrainedObservers(OapChecklist corpFSOChecklist)
        {
            IIrmaServiceDataModel pobDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.RigPersonnel); //.RigPersonnelHistory);
            IEnumerable<RigPersonnelModel> currentPob = pobDataModel.GetItems("Status = 1", "Id").Cast<RigPersonnelModel>();
            
            IEnumerable<RigOapChecklist> fsoChecklistsLast40Days = 
                RigOapChecklistClient.GetFsoChecklistByMinDateAsync(DateTime.Now.AddDays(-40).Date, corpFSOChecklist.Id).Result?.Result?.Data;

            List<TrainedObserversViewModel> trainedObservers = new List<TrainedObserversViewModel>();
            foreach (RigPersonnelModel person in currentPob)
            {
                UserModel userRecord = ServiceSystem.GetUser(person.PassportId);

                if (userRecord == null || !person.DateStart.HasValue) continue;

                PositionModel position = ServiceSystem.GetUserPosition(person.PassportId);
                TimeSpan timeOnBoard = DateTime.Now.Subtract(person.DateStart.Value);

                TrainedObserversViewModel observer = new TrainedObserversViewModel()
                {
                    Name = userRecord?.DisplayName,
                    Position = position?.Name,
                    DaysOnboard = timeOnBoard.Days
                };

                int numberOfObservations = 0;
                IEnumerable<RigOapChecklist> checklistsWhileOnboard = fsoChecklistsLast40Days.Where(c => c.ChecklistDateTime >= person.DateStart);

                DateTime? lastObservationDate = null;
                foreach (RigOapChecklist fsoChecklist in checklistsWhileOnboard)
                {
                    if (fsoChecklist.Assessors == null)
                        continue;

                    IEnumerable<RigOapChecklistAssessor> checklistParticipations = fsoChecklist.Assessors.Where(a => a.UserId == userRecord.Id);
                    numberOfObservations += checklistParticipations.Count();

                    if (checklistParticipations.Count() > 0 && (lastObservationDate == null || fsoChecklist.ChecklistDateTime > lastObservationDate))
                        lastObservationDate = fsoChecklist.ChecklistDateTime;
                }

                observer.LastObservation = lastObservationDate;

                int weeksOnBoard = timeOnBoard.Days / 7;
                bool isInCompliance = true;
                if (timeOnBoard.Days > 7)
                    isInCompliance = numberOfObservations > 0 ? (numberOfObservations / weeksOnBoard >= 1) : false;
                    
                observer.Observations = numberOfObservations;
                observer.IsInCompliance = isInCompliance;

                trainedObservers.Add(observer);
            }

            return trainedObservers;
        }

        private IEnumerable<RigOapChecklist> GetOpenFsoChecklists(OapChecklist corpFSOChecklist)
        {            
            return RigOapChecklistClient.GetOpenFsoChecklistsAsync(corpFSOChecklist.Id).Result?.Result?.Data;
        }

        private OapChecklist GetCorporateFSOChecklist()
        {
            if (corpFSOChecklist == null)
                corpFSOChecklist = OapChecklistClient.GetByNameAsync("FSO Checklist").Result?.Result?.Data;
            return corpFSOChecklist;
        }
    }
}
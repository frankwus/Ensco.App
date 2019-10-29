using DevExpress.Web.Mvc;
using Ensco.FSO.Models;
using Ensco.FSO.Services;
using Ensco.Models;
using Ensco.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Ensco.FSO.Services.FSOServiceSystem;

namespace Ensco.App.Areas.IRMA.Controllers
{
    public class FsoController : Controller
    {
        IFSOServiceDataModel serviceModel = null;

        // GET: IRMA/Fso
        public ActionResult Index()
        {
            return View();
        }

        // GET: IRMA/Checklist
        public ActionResult Checklist(int? id)
        {
            serviceModel = FSOServiceSystem.GetServiceModel(FSODataModelType.Checklist);
            FSOChecklistViewModel model = new FSOChecklistViewModel();
            if (id.HasValue) 
            {
                model.Checklist = serviceModel.GetItem(string.Format("Id={0}", id.Value), "Id");
                if (model.Checklist == null) // Invalid checklist passed in
                    return RedirectToAction("Index");
            }
            else // New Checklist
            {
                model.Checklist = FSOServiceSystem.CreateChecklist();
                model.Sections = FSOServiceSystem.GetChecklistSections();
                model.Questions = FSOServiceSystem.GetBaselineQuestions().ToList();
                Session["checklistObserverGridModel"] = new ChecklistObserverGridModel() { };
            }

            Session["ChecklistViewModel"] = model;

            return View(model);

        }

        // POST: IRMA/SignChecklist/{id}
        [HttpPost]
        public ActionResult SignChecklist(int checklistId, string OIMComments)
        {
            //var checklist = serviceModel.GetChecklist(checklistId);
            //string userId = HttpContext.User.Identity.Name;
            //serviceModel.SignChecklist(checklistId, userId, OIMComments, DateTime.Now);

            return RedirectToAction("Checklist");
        }

        public ActionResult ChecklistObserversPartial()
        {
            ChecklistObserverGridModel model = (ChecklistObserverGridModel)Session["checklistObserverGridModel"];            
            return PartialView(model);
        }

        public ActionResult ChecklistOberserversPartial()
        {
            //ViewData["Users"] = FSOServiceDataModel.GetOnboardUsers();

            IFSOServiceDataModel dataModel = FSOServiceSystem.GetServiceModel(FSOServiceSystem.FSODataModelType.Observer);
            List<dynamic> items = dataModel.GetAllItems();

            return PartialView();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ChecklistObserversAddPartial(ChecklistObserverGridRowModel rowModel)
        {
            ChecklistObserverGridModel model = (ChecklistObserverGridModel)Session["checklistObserverGridModel"];
            //IFSOServiceDataModel fSOServiceDataModel = new FSOServiceDataModel();

            if (ModelState.IsValid)
            {
                UserModel user = ServiceSystem.GetUser((int)rowModel.PassportId);
                rowModel.Id = model.Observers.Count + 1; // Temp ID
                rowModel.Company = user.Company;
                rowModel.Position = user.Position;
                model.Observers.Add(rowModel);
            } else
            {                
                ViewData["UpdateError"] = true;
            }           

            return PartialView("ChecklistObserversPartial", model);
        }
        public ActionResult ChecklistObserversUpdatePartial(ChecklistObserverGridRowModel rowModel)
        {
            ChecklistObserverGridModel model = (ChecklistObserverGridModel)Session["checklistObserverGridModel"];

            if (ModelState.IsValid)
            {
                UserModel user = ServiceSystem.GetUser((int)rowModel.PassportId);
                rowModel.Company = user.Company;
                rowModel.Position = user.Position;
                var observer = model.Observers.Find(o => o.Id == rowModel.Id);
                model.Observers.Remove(observer);
                model.Observers.Add(rowModel);
            }
            else
            {
                ViewData["UpdateError"] = true;
            }
            
            return PartialView("ChecklistObserversPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ChecklistObserversDeletePartial(ChecklistObserverGridRowModel rowModel)
        {
            ChecklistObserverGridModel model = (ChecklistObserverGridModel)Session["checklistObserverGridModel"];
            if (ModelState.IsValid)
            {
                model.Observers.RemoveAll(o => o.Id == rowModel.Id);
                Session["checklistObserverGridModel"] = model;
            }
            
            return PartialView("ChecklistObserversPartial",model);
        }
    }
}
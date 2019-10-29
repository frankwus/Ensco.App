using System;
using System.Web.Mvc;

namespace Ensco.App.Areas.Oap.Controllers
{
    public class OIMOversightController : GenericController
    {       
        public OIMOversightController()
        {
            BaseController = "OIMOversight";
        }

        
        public int Columns { get; protected set; } = 4;
        protected string ColumnPrefix { get; set; } = "Week";

        public override ActionResult List(Guid id)
        {
            ViewBag.Title = "OIM Oversight Checklist";
            return base.List(id);
        }        

        protected override PartialViewResult DisplayExecutionPartialView()
        {
            ViewBag.ChecklistDateTime = RigChecklist.ChecklistDateTime;
            return base.DisplayExecutionPartialView();
        }
    }
}
using DevExpress.Utils;
using DevExpress.Web.Mvc;
using Ensco.App.App_Start; 
using Ensco.Irma.Oap.Common.Models;
using Ensco.Irma.Oap.Web.Rig.Areas.Oap;
using Ensco.Irma.Oap.WebClient.Rig;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ensco.App.Areas.Oap.Controllers
{
    public class ProtocolScoringDashboardController : ProtocolDashboardController
    {
        public ProtocolScoringDashboardController()   
        {
            ChecklistType = "OP"; 
            FormType = "Scoring";            

            GridDataChecklist = new GridData(oapChecklistGridName, "ProtocolScoringDashboard", "RigChecklists", "Protocol Scoring Checklists", "AddNewRigChecklist", "Add Checklist", "Search", initializeCallBack: true)
            {
                EditController = "ProtocolForm"
            };
        }

        protected override ObservableCollection<RigOapChecklist> GetRigChecklistData(bool useTypeSubTypeCode)
        {
            var rigChecklistData = (useTypeSubTypeCode) ? RigOapChecklistClient.GetAllByTypeSubTypeCodeStatusAsync(ChecklistType, ChecklistSubType, ChecklistStatus).Result?.Result?.Data : RigOapChecklistClient.GetAllByTypeSubTypeCodeFormTypeStatusAsync(ChecklistType, ChecklistSubType, FormType, ChecklistStatus).Result?.Result?.Data;

            return rigChecklistData;
        }

        public override ActionResult Index()
        {
            //var data = GetRigChecklistData(false);
            //return View("GenericDashboard", data);
            return TryCatchCollectionDisplayView("GenericDashboard", "GenericDashBoardErrorsKey", () => GetRigChecklistData(false));
        }

        public override ActionResult RigChecklists()
        {
            //var data = GetRigChecklistData(false);
            //return View("GenericDashboardPartial", data);
            return TryCatchCollectionDisplayPartialView("GenericDashboardPartial", "GenericDashBoardErrorsKey", () => GetRigChecklistData(false));
        } 


    }
}
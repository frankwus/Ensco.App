using Ensco.Irma.Oap.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ensco.App.Areas.Oap.Controllers
{
    public class ProtocolDashboardController : GenericDashboardController
    {
        public ProtocolDashboardController()
        {
            GridDataChecklist.Title = "Open Protocol Checklists";
            ChecklistType = "OP";
            GridDataChecklist = new GridData(oapChecklistGridName, "ProtocolDashboard", "RigChecklists", "Open Protocol Checklists", "AddNewRigChecklist", "Add Protocol Checklist", "Search", initializeCallBack: true);
        }


        protected override void InitializeRigChecklistGridData(HtmlHelper html, ViewContext viewContext, GridData gridData, string createAction, string updateAction, string deleteAction)
        {
            

            base.InitializeRigChecklistGridData(html, viewContext, gridData, createAction, updateAction, deleteAction);

            gridData.EditController = "Protocol";

            var editColumnOapchecklistId = gridData.LayoutColumns.ToList().FirstOrDefault(c => c.Name.Equals("OapchecklistId"));

            if (editColumnOapchecklistId != null)
            {
                editColumnOapchecklistId.DisplayName = "Protocol Type";
            }

        }


    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace Ensco.App.Areas.cOap.Controllers
{
    using Ensco.Irma.Oap.Web.Oap.Controllers;
    using Ensco.Irma.Oap.Common.Extensions;
    using Ensco.Irma.Oap.Common.Models;
    using Ensco.Irma.Oap.WebClient.Corp;
    using Ensco.App.App_Start;
    using System.Web.UI.WebControls;
    using Ensco.Utilities;

    public class OapProtocolFormTypesController : OapCorpGridController
    {
        public const string CurrentProtocolFormTypeId = "CurrentProtocolFormTypeId";
        public const string CurrentProtocolFormTypeName = "CurrentProtocolFormTypeName";

        private OapProtocolFormTypeClient OapProtocolFormTypeClient { get; }

        public GridData GridData { get; } = new GridData("OapProtocolFormTypeGrid", "OapProtocolFormTypes", "ProtocolFormTypes", "Protocol Form Type", "AddNewProtocolFormType", "Add Protocol Form Type", "search protocol form types", initializeCallBack: true);

        private void InitializeGridData(GridData gridData, string createAction, string updateAction, string deleteAction)
        {
            gridData.ToolBarOptions.DisplayXlsExport = true;

            gridData.DisplayColumns = new List<GridDisplayColumn>()
                            {
                                new GridDisplayColumn("Name", displayName:"Name", width:20, editLayoutWidth:100),
                                new GridDisplayColumn("Description", displayName:"Description", width:65, editLayoutWidth:100),
                                new GridDisplayColumn("StartDateTime", displayName:"Start Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g"),
                                new GridDisplayColumn("EndDateTime", displayName:"End Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g"),

                                new GridDisplayColumn("CreatedBy", displayName:"Created By", isReadOnly:true, width:0, isVisible: false),
                                new GridDisplayColumn("CreatedDateTime", displayName:"Date Created", columnType:MVCxGridViewColumnType.DateEdit, width:0, isReadOnly:true, displayFormat: "g", isVisible: false),
                                new GridDisplayColumn("UpdatedBy", displayName:"Updated By", width:0, isReadOnly:true, isVisible: false),
                                new GridDisplayColumn("UpdatedDateTime", displayName:"Date Updated", columnType:MVCxGridViewColumnType.DateEdit, width:0, isReadOnly:true, displayFormat: "g", isVisible: false)
                            };
             

            gridData.Routes = new List<GridRoute>()
                            {
                                new GridRoute(GridRouteTypes.Add, new { Controller = gridData.Controller, Action = createAction }),
                                new GridRoute(GridRouteTypes.Update, new { Controller = gridData.Controller, Action = updateAction }),
                                new GridRoute(GridRouteTypes.Delete, new { Controller = gridData.Controller, Action = deleteAction }),
                            };

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("Name", displayName:"Name"),
                                new GridEditLayoutColumn("Description", displayName:"Description"),
                                new GridEditLayoutColumn("StartDateTime", displayName:"Start Date"),
                                new GridEditLayoutColumn("EndDateTime", displayName:"End Date"),
                            };

            gridData.FormLayout = new GridEditFormLayout(GridData.LayoutColumns
                    , i => {
                        i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                        i.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                    });
        }


        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            settings.CommandColumn.Width = Unit.Percentage(2);

            InitializeGridData(GridData, "CreateProtocolFormType", "UpdateProtocolFormType", "DeleteProtocolFormType");

            //Master Grid Specific configurations
            settings.KeyFieldName = GridData.Key;
            settings.Name = $"{GridData.Name}";
            settings.SettingsDetail.ShowDetailRow = false;
            settings.SettingsDetail.AllowOnlyOneMasterRowExpanded = false;

            //Generic configurations
            settings.Configure(GridData, html, viewContext);

            SelectRow((int)(ViewData[CurrentProtocolFormTypeId] ?? 0), (string)(ViewData[CurrentProtocolFormTypeName] ?? string.Empty), ref settings);
        }

        public OapProtocolFormTypesController() : base()
        {
            OapProtocolFormTypeClient = new OapProtocolFormTypeClient(GetApiBaseUrl(), Client);
        }

        public async Task<ActionResult> Index()
        {
            var response = await OapProtocolFormTypeClient.GetAllAsync(GetAllModelsCorp());
            return View("OapProtocolFormType", response.Result.Data);
        }
         
        public async Task<ActionResult> ProtocolFormTypes()
        {
            var response = await OapProtocolFormTypeClient.GetAllAsync(GetAllModelsCorp());
            return PartialView("OapProtocolFormTypePartial", response.Result.Data);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProtocolFormType(OapProtocolFormType model)
        {           
            // TODO: Add insert logic here
            var response = await OapProtocolFormTypeClient.AddProtocolFormTypeAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateProtocolFormType(OapProtocolFormType model)
        {            
            // TODO: Add update logic here
            var response = OapProtocolFormTypeClient.UpdateProtocolFormTypeAsync(model).Result;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteProtocolFormType(OapProtocolFormType model)
        {
            if (model.Id > 0)
            {
                var response = await OapProtocolFormTypeClient.DeleteProtocolFormTypeAsync(model.Id);
            }
            return RedirectToAction("Index");
        }

    }
}

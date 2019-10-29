using DevExpress.Web.Mvc;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using DevExpress.Web;

namespace Ensco.App.Areas.cOap.Controllers
{
    using Ensco.Irma.Oap.Web.Oap.Controllers;
    using Ensco.Irma.Oap.Common.Extensions;
    using Ensco.Irma.Oap.Common.Models;
    using Ensco.Irma.Oap.WebClient.Corp;
    using Ensco.App.App_Start;
    using System.Web.UI.WebControls;
    using Ensco.Utilities;

    public class OapChecklistQuestionTagController : OapCorpGridController
    {
        public const string CurrentQuestionTagId = "CurrentQuestionTagId";
 

        private OapChecklistQuestionTagClient OapQuestionTagClient { get; }

        public GridData GridData { get; } = new GridData("OapQuestionTagGrid", "OapChecklistQuestionTag", "QuestionTag", "Barrier Health Tags", "AddNewQuestionTag", "Add Tag", "search Tag", initializeCallBack: true);

        private void InitializeGridData(GridData gridData, string createAction, string updateAction, string deleteAction)
        {
            gridData.ToolBarOptions.DisplayXlsExport = true;

            gridData.DisplayColumns = new List<GridDisplayColumn>()
                            {
                                new GridDisplayColumn("Tag", displayName:"Tag",width:100,editLayoutWidth:100, isReadOnly:false),  
                                new GridDisplayColumn("CreatedBy", displayName:"Created By", isReadOnly:true, width:10,isVisible:false),
                                new GridDisplayColumn("CreatedDateTime", displayName:"Date Created", columnType:MVCxGridViewColumnType.DateEdit, width:15, isReadOnly:true, displayFormat: "g",isVisible: false),
                                new GridDisplayColumn("UpdatedBy", displayName:"Updated By", width:10, isReadOnly:true,isVisible: false),                            
                                new GridDisplayColumn("UpdatedDateTime", displayName:"Date Updated", columnType:MVCxGridViewColumnType.DateEdit, width:15, isReadOnly:true, displayFormat: "g",isVisible: false)
                            };


            gridData.Routes = new List<GridRoute>()
                            {
                                new GridRoute(GridRouteTypes.Add, new { Controller = gridData.Controller, Action = createAction }),
                                new GridRoute(GridRouteTypes.Update, new { Controller = gridData.Controller, Action = updateAction }),
                                new GridRoute(GridRouteTypes.Delete, new { Controller = gridData.Controller, Action = deleteAction }),
                            };

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("Tag", displayName:"Tag",width:100),     
                                 
                            };

            gridData.FormLayout = new GridEditFormLayout(GridData.LayoutColumns
                    , i => {
                        i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                        i.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                    },1);
        }


        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            settings.CommandColumn.Width = Unit.Percentage(2);
            InitializeGridData(GridData, "CreateQuestionTag", "UpdateQuestionTag", "DeleteQuestionTag");

            //Master Grid Specific configurations
            settings.KeyFieldName = GridData.Key;
            settings.Name = $"{GridData.Name}";
            settings.SettingsDetail.ShowDetailRow = false;
            settings.SettingsDetail.AllowOnlyOneMasterRowExpanded = false;            

            //Generic configurations
            settings.Configure(GridData, html, viewContext);
            SelectRow((int)(ViewData[CurrentQuestionTagId] ?? 0), (string)(ViewData[CurrentQuestionTagId] ?? string.Empty), ref settings);
        }

        public OapChecklistQuestionTagController() : base()
        {
            OapQuestionTagClient = new OapChecklistQuestionTagClient(GetApiBaseUrl(), Client);
        }

        public async Task<ActionResult> Index()
        {
            var response = await OapQuestionTagClient.GetAllAsync();
            return View("OapQuestionTag", response.Result.Data);
        }
        
        public async Task<ActionResult> QuestionTag()
        {
            var response = await OapQuestionTagClient.GetAllAsync();

            return PartialView("OapQuestionTagPartial", response.Result.Data);
        }

        [HttpPost]
        public async Task<ActionResult> CreateQuestionTag(OapChecklistQuestionTag model)
        {                        
            // TODO: Add insert logic here
            var response = await OapQuestionTagClient.AddQuestionTagAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateQuestionTag(OapChecklistQuestionTag model)
        {           
            // TODO: Add update logic here
            var response = OapQuestionTagClient.UpdateQuestionTagAsync(model).Result;  
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteQuestionTag(OapChecklistQuestionTag model)
        {
            if (model.Id > 0)
            {
                var response = await OapQuestionTagClient.DeleteQuestionTagAsync(model.Id);
            }
            return RedirectToAction("Index");
        }
    }
}

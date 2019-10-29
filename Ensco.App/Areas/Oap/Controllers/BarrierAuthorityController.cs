using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Ensco.Irma.Oap.Common.Extensions;
using Ensco.Irma.Oap.Common.Models;
using Ensco.Irma.Oap.WebClient.Rig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ensco.App.Areas.Oap.Controllers
{
    using Ensco.App.Areas.Oap.Extensions;
    using Ensco.App.Areas.Oap.Models;
    using Ensco.Irma.Oap.Web.Rig.Areas.Oap;
    using MvcSiteMapProvider.Web.Mvc.Filters;
    using System.Web.Mvc.Html;

    public class BarrierAuthorityController : OIMOversightController
    {
        public BarrierAuthorityController():base()
        {
            BaseController = "BarrierAuthority";
            Columns = 7;
            ColumnPrefix = "Day";
            RegisterClient(typeof(OapChecklistAssetDataManagementClient));
            RegisterClient(typeof(RigOapChecklistGroupAssetClient));
            RegisterClient(typeof(RigOapChecklistWorkInstructionClient));
            RegisterClient(typeof(RigOapChecklistThirdPartyJobClient));
        }

        [SiteMapTitle("RigChecklistUniqueId")]
        public override ActionResult List(Guid id)
        {
            RigChecklist = GetRigChecklist(id);
            ViewBag.Title = "Barrier Authority Checklist";
            return View("GenericList", RigChecklist);

        }

        protected override GridData InitializeChecklistExecutionGridData(HtmlHelper html, ViewContext viewContext, string batchUpdateAction = "UpdateQuestions")
        {
            var gridData = base.InitializeChecklistExecutionGridData(html, viewContext);


            var subGroup = new GridDisplayColumn("SubGroup", order: 15, allowHeaderFilter: DefaultBoolean.False, allowSort: DefaultBoolean.False, columnAction: c =>
            {
                c.FieldName = "SubGroup";
                c.Caption = "SubGroup";
                c.GroupIndex = 1;
                c.ReadOnly = true;
                c.Settings.ShowEditorInBatchEditMode = false; 
                c.Settings.AllowGroup = DefaultBoolean.True;
                c.Settings.AllowSort = DefaultBoolean.True; 
            });

            gridData.DisplayColumns.Add(subGroup);


            var frequency = gridData.DisplayColumns.FirstOrDefault(c => c.Name.Equals("Frequency", StringComparison.InvariantCultureIgnoreCase));
            if(frequency != null)
            {
                gridData.DisplayColumns.Remove(frequency);
            }

            
            return gridData;
        }


        public override void ConfigureExecution(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            base.ConfigureExecution(settings, html, viewContext);

            settings.SettingsBehavior.AllowSelectSingleRowOnly = true; 
            settings.Settings.ShowGroupFooter = DevExpress.Web.GridViewGroupFooterMode.VisibleAlways; 
            settings.SetGroupFooterRowTemplateContent(c => { 

                var isPlanMonitoring = (bool) DataBinder.Eval(c.DataItem, "IsPlantMonitoring");
                var subGroupText = (string) DataBinder.Eval(c.DataItem, "SubGroup");

                var content = string.Empty;
                
                if (isPlanMonitoring)
                {

                    var groupId = (int) DataBinder.Eval(c.DataItem, "GroupId");

                    html.RenderPartial("BarrierAuthorityRenderAssetsPartial", RigChecklist.Assets.ToGroupAssetModels(groupId, GetClient<OapChecklistAssetDataManagementClient>()));
                }
                 
            });

            settings.HtmlRowCreated = (sender, e) =>
            {
                var isPlantMonitoringRow = (bool) (e.GetValue("IsPlantMonitoring") ?? false);
                if (e.RowType == DevExpress.Web.GridViewRowType.GroupFooter)
                {
                    e.Row.Visible = isPlantMonitoringRow;
                }

            };

            settings.BeforeGetCallbackResult = (sender, e) =>
            {

                MVCxGridView grid = (MVCxGridView) sender;
                grid.ClearSort();
                var i = grid.GroupCount;
                grid.GroupBy(grid.Columns["Group"]);
                grid.GroupBy(grid.Columns["SubGroup"]);
            };

        }

        public virtual void ConfigureAssets(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            var gridData = InitializeChecklistExecutionAssetsGridData(html, viewContext);

            settings.Configure(gridData, html, viewContext);

            settings.CommandButtonInitialize = (o, e) =>
            {
                if ((e.ButtonType == ColumnCommandButtonType.Delete))
                {
                    e.Visible = false;
                }
            };

        }

        public virtual void ConfigureThirdPartyJobActivities(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            var gridData = InitializeChecklistExecutionThirdPartyJobActivitiesGridData(html, viewContext);

           settings.Configure(gridData, html, viewContext);
        }

        private GridData InitializeChecklistExecutionThirdPartyJobActivitiesGridData(HtmlHelper html, ViewContext viewContext)
        {
            var toolbarOptions = new GridToolBarOptions(false);

            var commandOptions = new GridCommandButtonOptions(true, displayAddButtonInGridHeader: false, displayDeleteButton: false);

            var gridData = new GridData(GridConstants.rigChecklistBAAssetsGrid, BaseController, "DisplayThirdPartyJobs", key: "Id", initializeCallBack: true, callBackRoute: new { Controller = BaseController, Action = "DisplayThirdPartyJobs" }, toolbarOptions: toolbarOptions, commandButtonOptions: commandOptions, showPager: false)
            {
                Title = ""
            };

             
            var displayColumns = new List<GridDisplayColumn>
            {
                new GridDisplayColumn("JobId", displayName: "Job", order:30, width:10, columnType: MVCxGridViewColumnType.TextBox , isReadOnly: true, allowEditLayout: DefaultBoolean.True, allowSort: DefaultBoolean.False, allowHeaderFilter: DefaultBoolean.False),
                new GridDisplayColumn("ThirdPartyCount", displayName: "Third Party Count", order:40,width:70,columnType: MVCxGridViewColumnType.SpinEdit, allowEditLayout: DefaultBoolean.True, allowSort: DefaultBoolean.False , allowHeaderFilter: DefaultBoolean.False),
                
                new GridDisplayColumn("Id", order:200, width:0, displayName: "Rig Work Instruction Id", isVisible: false)
            };

            gridData.DisplayColumns = displayColumns;

            gridData.Routes.Add(new GridRoute(GridRouteTypes.Update, new { Controller = BaseController, Action = "UpdateThirdPartyJob" }));

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("ThirdPartyCount", displayName:"Third Party Count"), 
                                new GridEditLayoutColumn("JobId", displayName:"Job"),
                                new GridEditLayoutColumn("Id", displayName:"Rig Third Party Job Id")
                            };

            gridData.FormLayout = new GridEditFormLayout(
                    gridData.LayoutColumns
                    , editMode: GridViewEditingMode.EditFormAndDisplayRow
                    , processLayout: i =>
                    {
                        i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                        i.Width = Unit.Percentage(100);
                    }
                    );

            return gridData;
        }

        public virtual void ConfigureWorkInstructions(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            var gridData = InitializeChecklistExecutionWorkInstructionsGridData(html, viewContext);

            settings.Configure(gridData, html, viewContext);
        }

        private GridData InitializeChecklistExecutionWorkInstructionsGridData(HtmlHelper html, ViewContext viewContext)
        {
            var toolbarOptions = new GridToolBarOptions(false);

            var commandOptions = new GridCommandButtonOptions(true, displayAddButtonInGridHeader: false, displayDeleteButton: false);

            var gridData = new GridData(GridConstants.rigChecklistBAAssetsGrid, BaseController, "DisplayWorkInstructions", key: "Id", initializeCallBack: true, callBackRoute: new { Controller = BaseController, Action = "DisplayWorkInstructions" }, toolbarOptions: toolbarOptions, commandButtonOptions: commandOptions, showPager: false)
            {
                Title = ""
            };

            var yesNoNAValueCombo = new GridCombo("yesNoNAValue", GetYesNoNaValues(), "Id", "DisplayValue", "Id");

            var displayColumns = new List<GridDisplayColumn>
            {
                new GridDisplayColumn("Title", displayName: "WorkInstruction", order:30, width:10, columnType: MVCxGridViewColumnType.TextBox , isReadOnly: true, allowEditLayout: DefaultBoolean.False, allowSort: DefaultBoolean.False, allowHeaderFilter: DefaultBoolean.False),
                new GridDisplayColumn("Comment", displayName: "Comment", order:40,width:70,columnType: MVCxGridViewColumnType.TextBox, allowEditLayout: DefaultBoolean.False, allowSort: DefaultBoolean.False , allowHeaderFilter: DefaultBoolean.False),
                new GridDisplayColumn("Value", displayName: "Value", order:40,width:70,columnType: MVCxGridViewColumnType.ComboBox, allowSort: DefaultBoolean.False , allowHeaderFilter: DefaultBoolean.False,lookup: yesNoNAValueCombo),

                new GridDisplayColumn("WorkInstructionId", order:200, width:0, displayName: "Work Instruction Id", isVisible: false),
                new GridDisplayColumn("Id", order:200, width:0, displayName: "Rig Work Instruction Id", isVisible: false)
            };

            gridData.DisplayColumns = displayColumns;


            gridData.Routes.Add(new GridRoute(GridRouteTypes.Update, new { Controller = BaseController, Action = "UpdateWorkInstruction" }));

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("Comment", displayName:"Comment"),
                                new GridEditLayoutColumn("Value", displayName:"Asset"), 
                                new GridEditLayoutColumn("WorkInstructionId", displayName:"Work Instruction Id"),
                                new GridEditLayoutColumn("Id", displayName:"Rig Work Instruction Id")
                            };

            gridData.FormLayout = new GridEditFormLayout(
                    gridData.LayoutColumns
                    , editMode: GridViewEditingMode.EditFormAndDisplayRow
                    , processLayout: i =>
                    {
                        i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                        i.Width = Unit.Percentage(100);
                    }
                    );

            return gridData;
        }

        public ActionResult DisplayAssets()
        {
            var groupId = RigChecklist.OapChecklist.Groups.FirstOrDefault(g => g.SubGroups.Any(sg => sg.IsPlantMonitoring))?.Id ?? 0;
            return PartialView("BarrierAuthorityRenderAssetsPartial", RigChecklist.Assets.ToGroupAssetModels(groupId, GetClient<OapChecklistAssetDataManagementClient>()));
        }

        public ActionResult DisplayWorkInstructions()
        { 
            return PartialView("BarrierAuthorityRenderWorkInstructionsPartial", RigChecklist.WorkInstructions);
        }

        public ActionResult DisplayThirdPartyJobs()
        {
            return PartialView("BarrierAuthorityRenderThirdPartyJobsPartial", RigChecklist.ThirdPartyJobs);
        }

        protected virtual GridData InitializeChecklistExecutionAssetsGridData(HtmlHelper html, ViewContext viewContext)
        {
            var toolbarOptions = new GridToolBarOptions(false);

            var commandOptions = new GridCommandButtonOptions(true,displayAddButtonInGridHeader: false,displayDeleteButton: false);

            var gridData = new GridData(GridConstants.rigChecklistBAAssetsGrid, BaseController, "DisplayAssets", initializeCallBack: true, callBackRoute: new { Controller = BaseController, Action = "DisplayAssets" }, toolbarOptions: toolbarOptions, commandButtonOptions: commandOptions, showPager: false)
            {
                Title = "" 
            };

            var assetValueCombo = new GridCombo("assetvalue", GetYesNoNaValues(), "Id", "DisplayValue", "Id");

            var displayColumns = new List<GridDisplayColumn>
            {     
                new GridDisplayColumn("GroupName", displayName: "Group", order:30, width:10, columnType: MVCxGridViewColumnType.TextBox , isReadOnly: true, allowEditLayout: DefaultBoolean.False, allowSort: DefaultBoolean.False, allowHeaderFilter: DefaultBoolean.False),
                new GridDisplayColumn("SystemName", displayName: "System", order:40,width:70,columnType: MVCxGridViewColumnType.TextBox, isReadOnly: true, allowEditLayout: DefaultBoolean.False, allowSort: DefaultBoolean.False , allowHeaderFilter: DefaultBoolean.False),
                new GridDisplayColumn("SubSystemName", displayName: "SubSystem", order:40,width:70,columnType: MVCxGridViewColumnType.TextBox, isReadOnly: true, allowEditLayout: DefaultBoolean.False, allowSort: DefaultBoolean.False , allowHeaderFilter: DefaultBoolean.False),
                new GridDisplayColumn("SubSystemName", displayName: "SubSystem", order:40,width:70,columnType: MVCxGridViewColumnType.TextBox, isReadOnly: true, allowEditLayout: DefaultBoolean.False, allowSort: DefaultBoolean.False , allowHeaderFilter: DefaultBoolean.False),
                new GridDisplayColumn("AssetValue", displayName: "Value", order:40,width:70,columnType: MVCxGridViewColumnType.ComboBox, allowSort: DefaultBoolean.False , allowHeaderFilter: DefaultBoolean.False,lookup: assetValueCombo),

                new GridDisplayColumn("ChecklistGroupId", order:200, width:0, displayName: "Checklist Group Id", isVisible: false),
                new GridDisplayColumn("AssetGroupId", order:200, width:0, displayName: "Asset Group Id", isVisible: false),
                new GridDisplayColumn("SystemId", order:200, width:0, displayName: "System Id", isVisible: false),
                new GridDisplayColumn("SubSystemId", order:200, width:0, displayName: "Sub System Id", isVisible: false),
                new GridDisplayColumn("Id", order:200, width:0, displayName: "Rig Group Asset Id", isVisible: false), 
            };

            gridData.DisplayColumns = displayColumns;


            gridData.Routes.Add(new GridRoute(GridRouteTypes.Update, new { Controller = BaseController, Action = "UpdateAsset" }));

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("AssetValue", displayName:"Asset"),
                                new GridEditLayoutColumn("ChecklistGroupId", displayName:"Checklist Group Id"),
                                new GridEditLayoutColumn("Id", displayName:"Rig Group Asset Id")
                            };

            gridData.FormLayout = new GridEditFormLayout(
                    gridData.LayoutColumns
                    , editMode: GridViewEditingMode.EditFormAndDisplayRow
                    , processLayout: i =>
                    {
                        i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                        i.Width = Unit.Percentage(100);
                    }
                    ); 

            return gridData;
        }

        [ValidateInput(false)]
        public ActionResult UpdateAsset(RigOapChecklistGroupAssetModel model)
        {
            return TryCatchCollectionDisplayPartialView("BarrierAuthorityRenderAssetsPartial", GridConstants.RigOapChecklistGroupAssetsErrorsKey, () => {
                var checklist = RigChecklist;
                if ((checklist != null) && (checklist.Assets != null))
                {
                    var asset = checklist.Assets.FirstOrDefault(a => a.Id == model.Id);
                    if ((asset != null))
                    {
                        var client = GetClient<RigOapChecklistGroupAssetClient>();
                        asset.Value = model.AssetValue;
                        var response = client.UpdateRigChecklistAssetAsync(asset).Result;

                        if ((response != null) && response.Result.Errors.Any())
                        {
                            CommonUtilities.DisplayGridErrors(response.Result.Errors, GridConstants.RigOapChecklistGroupAssetsErrorsKey, ViewData);
                        }
                        else
                        {
                            RigChecklist = GetClient<RigOapChecklistClient>().GetCompleteChecklistAsync(RigChecklist.Id).Result.Result.Data;
                        }
                    }
                }

                return RigChecklist.Assets.ToGroupAssetModels(model.ChecklistGroupId, GetClient<OapChecklistAssetDataManagementClient>());
            });
        }

        [ValidateInput(false)]
        public ActionResult UpdateWorkInstruction(RigOapChecklistWorkInstruction model)
        {
            return TryCatchCollectionDisplayPartialView("BarrierAuthorityRenderWorkInstructionsPartial", GridConstants.RigOapChecklistWorkInstructionsErrorsKey, () => {
                var checklist = RigChecklist;
                if ((checklist != null) && (checklist.WorkInstructions != null))
                {
                    var workInstruction = checklist.WorkInstructions.FirstOrDefault(a => a.Id == model.Id);
                    if ((workInstruction != null))
                    {
                        var client = GetClient<RigOapChecklistWorkInstructionClient>();
                        workInstruction.Value = model.Value;
                        var response = client.UpdateRigChecklistWorkInstructionAsync(workInstruction).Result;

                        if ((response != null) && response.Result.Errors.Any())
                        {
                            CommonUtilities.DisplayGridErrors(response.Result.Errors, GridConstants.RigOapChecklistWorkInstructionsErrorsKey, ViewData);
                        }
                        else
                        {
                            RigChecklist = GetClient<RigOapChecklistClient>().GetCompleteChecklistAsync(RigChecklist.Id).Result.Result.Data;
                        }
                    }
                }

                return RigChecklist.WorkInstructions;
            });
        }

        [ValidateInput(false)]
        public ActionResult UpdateThirdPartyJob(RigOapChecklistThirdPartyJob model)
        {
            return TryCatchCollectionDisplayPartialView("BarrierAuthorityRenderThirdPartyJobsPartial", GridConstants.RigOapChecklistThirdPartyJobsErrorsKey, () => {
                var checklist = RigChecklist;
                if ((checklist != null) && (checklist.ThirdPartyJobs != null))
                {
                    var thirdPartyJob = checklist.ThirdPartyJobs.FirstOrDefault(a => a.Id == model.Id);
                    if ((thirdPartyJob != null))
                    {
                        var client = GetClient<RigOapChecklistThirdPartyJobClient>();
                        thirdPartyJob.JodId = model.JodId;
                        thirdPartyJob.ThirdPartyCount = model.ThirdPartyCount;
                        var response = client.UpdateRigChecklistThirdPartyJobAsync(thirdPartyJob).Result;

                        if ((response != null) && response.Result.Errors.Any())
                        {
                            CommonUtilities.DisplayGridErrors(response.Result.Errors, GridConstants.RigOapChecklistThirdPartyJobsErrorsKey, ViewData);
                        }
                        else
                        {
                            RigChecklist = GetClient<RigOapChecklistClient>().GetCompleteChecklistAsync(RigChecklist.Id).Result.Result.Data;
                        }
                    }
                }

                return RigChecklist.ThirdPartyJobs;
            });
        }


        protected override void ConfigureExecutionQuestionComments(HtmlHelper html, ViewContext viewContext, GridViewDetailRowTemplateContainer container)
        {
            var displayNoControl = (bool) DataBinder.Eval(container.DataItem, "DisplayNoControl");
            if (displayNoControl)
            {
                var rigQuestionId = (Guid) DataBinder.Eval(container.DataItem, "RigQuestionId");
                var rq = RigChecklist.Questions.FirstOrDefault((q) => q.Id == rigQuestionId);

                ViewData[GridConstants.RigChecklistQuestionInlineCommentErrorsKey] = rigQuestionId;

                html.RenderAction("DisplayNoAnswer", "NoAnswer", new { Area = "Oap", question = rq });
            }
            else
            {
                base.ConfigureExecutionQuestionComments(html, viewContext, container);
            }
        }

    }
}
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace Ensco.App.Areas.Oap.Controllers
{
    using Ensco.Irma.Oap.Common.Extensions;
    using Ensco.Irma.Oap.Common.Models;
    using Ensco.Irma.Oap.WebClient.Rig;
    using Ensco.Irma.Oap.Web.Oap.Controllers;
    using Ensco.Irma.Oap.Web.Rig.Areas.Oap;
    using System.Web.Routing;
    using System.Web;
    using Ensco.Models;
    using System.Linq;
    using Ensco.App.App_Start;
    using System.Web.Mvc.Html;
    using Ensco.App.Areas.Oap.Models;
    using Ensco.App.Areas.Oap.Extensions;
    using System.Web.UI.WebControls;
    using DevExpress.Utils;
    using System.Web.UI;

    public class ProtocolFormController : ProtocolController
    {
        public ProtocolFormController()
        {
            BaseController = "ProtocolForm";
        }

        public override void ConfigureTabs(PageControlSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            base.ConfigureTabs(settings, html, viewContext);

            settings.ClientSideEvents.Init = "function (s,e) { s.SetActiveTabIndex(0); }";

            var checklistScoring = settings.TabPages.Add("Scoring");
            checklistScoring.Text = "Scoring";

            checklistScoring.SetContent(() =>
            {
                html.RenderPartial("ProtocolScoringFormPartial", RigChecklist?.ToProtocolScoringFlattenedModel());
            });
        }

        public GridData GridData { get; } = new GridData("rigChecklistProtocolScoringGrid", "ProtocolForm", "DisplayProtocolScoring", "Scoring", "AddNewProtocolForm", "Add", "search results", initializeCallBack: true, showPager: false);

        public void ConfigureProtocolScoring(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            InitializeProtocolScoringGridData(GridData, "UpdateNoValue");
            settings.KeyFieldName = GridData.Key;
            settings.Name = $"{GridData.Name}";
            settings.Configure(GridData, html, viewContext);

            settings.CommandButtonInitialize = (sender, e) =>
            {
                string fieldValue = (sender as MVCxGridView).GetRowValues(e.VisibleIndex, "EditNoValue").ToString();

                if (e.ButtonType == ColumnCommandButtonType.Edit)
                {
                    if (fieldValue.ToLower() == "false")
                    {
                        e.Visible = false;
                    }
                }
            };
             
            settings.Settings.ShowGroupPanel = true;
            settings.Settings.ShowFooter = true;
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "YesValue").DisplayFormat = "n0";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "NoValue").DisplayFormat = "n0";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "MaxScore").DisplayFormat = "n0";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Score").DisplayFormat = "n0";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Average, "AverageScore").DisplayFormat = "n2";

            settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "YesValue").DisplayFormat = "YesValue: {0:n0}";
            settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "NoValue").DisplayFormat = "NoValue: {0:n0}";
            settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "MaxScore").DisplayFormat = "MaxScore: {0:n0}";
            settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Score").DisplayFormat = "Score: {0:n0}";
            settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Average, "AverageScore").DisplayFormat = "AverageScore: {0:n2}";
                        
        }

        private void InitializeProtocolScoringGridData(GridData gridData, string updateAction)
        {
            gridData.ButtonOptions.DisplayDeleteButton = false;
            gridData.ToolBarOptions.DisplayCustomAddNew = false;

            gridData.AddRemoveCustomAddNew();

            var displayColumns = new List<GridDisplayColumn>
            {
                new GridDisplayColumn("Group", displayName:"Group"),
                new GridDisplayColumn("Topic", displayName:"Topic"),
                new GridDisplayColumn("Question", displayName:"Question"),
                new GridDisplayColumn("YesValue", displayName:"Yes"),
                new GridDisplayColumn("NoValue", displayName:"No", columnType:MVCxGridViewColumnType.SpinEdit, editLayoutWidth:50, isReadOnly:false),
                new GridDisplayColumn("MaxScore", displayName:"Max Score"),
                new GridDisplayColumn("Score", displayName:"Score"),
                new GridDisplayColumn("AverageScore", displayName:"Score Average"),
                new GridDisplayColumn("EditNoValue", displayName:"EditNoValue", isVisible:false)
            };

            gridData.DisplayColumns = displayColumns;


            gridData.Routes = new List<GridRoute>()
                            {
                                new GridRoute(GridRouteTypes.Update, new { Controller = BaseController, Action = updateAction }),
                            };

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("NoValue", displayName:"No Value", width:100),
                            };

            gridData.FormLayout = new GridEditFormLayout(GridData.LayoutColumns
                    , i =>
                    {
                        i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                        i.Width = Unit.Percentage(100);
                    }, 1);

        }

        public ActionResult DisplayProtocolScoring()
        {
            return PartialView("ProtocolScoringFormPartial", RigChecklist?.ToProtocolScoringFlattenedModel());
        }

        [HttpPost]
        public ActionResult UpdateNoValue(RigOapChecklistQuestion model)
        {
            var checklist = RigChecklist;
            if (checklist != null)
            {
                var question = checklist.Questions.FirstOrDefault(q => q.Id == model.Id);
                if (question != null)
                {
                    if (model.NoValue < question.OapChecklistQuestion.Maximum)
                    {
                        question.NoValue = model.NoValue;
                        RigChecklist = CommonUtilities.UpdateChecklist(GetClient<RigOapChecklistClient>(), RigChecklist, GridRouteTypes.Update, RigChecklistOriginalHashCode, GridConstants.QuestionScoringErrorsKey, ViewData);
                    }
                }
            }

            return PartialView("ProtocolScoringFormPartial", RigChecklist?.ToProtocolScoringFlattenedModel());
        }

        protected override GridData InitializeChecklistExecutionGridData(HtmlHelper html, ViewContext viewContext, string batchUpdateAction = "UpdateQuestions")
        {
            var gridData = base.InitializeChecklistExecutionGridData(html, viewContext, batchUpdateAction);

            var weightColumn = new GridDisplayColumn("Weight", displayName: "Weight", order: 100, width: 15, columnType: MVCxGridViewColumnType.TextBox, isReadOnly: true, allowEditLayout: DefaultBoolean.False, allowSort: DefaultBoolean.False, allowHeaderFilter: DefaultBoolean.False);
            gridData.DisplayColumns.Add(weightColumn);

            var scoreColumn = new GridDisplayColumn("Score", displayName: "Score", order: 110, width: 15, columnType: MVCxGridViewColumnType.TextBox, isReadOnly: true, allowEditLayout: DefaultBoolean.False, allowSort: DefaultBoolean.False, allowHeaderFilter: DefaultBoolean.False);
            gridData.DisplayColumns.Add(scoreColumn);

            return gridData;
        }

    }
}
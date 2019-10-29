using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Ensco.Irma.Oap.Common.Models;
using Ensco.Irma.Oap.WebClient.Rig;
using Ensco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Ensco.Utilities;
using DevExpress.Web.Mvc.UI;

namespace Ensco.App.Areas.Oap.Controllers
{
    public class ProtocolController : GenericController
    {

        public ProtocolController()
        {
            BaseController = "Protocol";            
        }

        public override ActionResult List(Guid id)
        {
            ViewBag.Title = Translate("Protocol Checklist");
            return base.List(id);
        }

        //protected override GridData InitializeChecklistExecutionGridData(HtmlHelper html, ViewContext viewContext, string controller = "Generic", string batchUpdateAction = "UpdateQuestions")
        //{
        //    return base.InitializeChecklistExecutionGridData(html, viewContext, "Protocol");
        //}

        //public override void ConfigurePlanning(FormLayoutSettings<RigOapChecklist> settings, HtmlHelper htmlHelper, ViewContext viewContext)
        //{
        //    base.ConfigurePlanning(settings, htmlHelper, viewContext);

        //    var planningSection = (MVCxFormLayoutGroup<RigOapChecklist>)settings.Items[0];

        //    planningSection.Items.Add(m => m.OapChecklist.OapType.IsIsm, i =>
        //    {
        //        i.Caption = Translate("ISM Certified");
        //    });

        //}


        protected override GridData InitializeChecklistExecutionGridData(HtmlHelper html, ViewContext viewContext, string batchUpdateAction = "UpdateQuestions")
        {
            var gridData = base.InitializeChecklistExecutionGridData(html, viewContext, batchUpdateAction);

            var sectionColumn = new GridDisplayColumn("Section", displayName: Translate("Section"), order: 70, width: 15, columnType: MVCxGridViewColumnType.TextBox, isReadOnly: true, allowEditLayout: DefaultBoolean.False, allowSort: DefaultBoolean.False, allowHeaderFilter: DefaultBoolean.False);
            gridData.DisplayColumns.Add(sectionColumn);

            var criticalityColumn = new GridDisplayColumn("Criticality", displayName: Translate("Criticality"), order: 71, width: 15, columnType: MVCxGridViewColumnType.TextBox, isReadOnly: true, allowEditLayout: DefaultBoolean.False, allowSort: DefaultBoolean.False, allowHeaderFilter: DefaultBoolean.False);
            gridData.DisplayColumns.Add(criticalityColumn);

            var basicCauseColumn = new GridDisplayColumn("BasicCauseClassification", displayName: Translate("Basic Cause Classification"), order: 72, width: 15, columnType: MVCxGridViewColumnType.TextBox, isReadOnly: true, allowEditLayout: DefaultBoolean.False, allowSort: DefaultBoolean.False, allowHeaderFilter: DefaultBoolean.False);
            gridData.DisplayColumns.Add(basicCauseColumn);            

            return gridData;

        }

        public override void ConfigurePlanning(FormLayoutSettings<RigOapChecklist> settings, HtmlHelper htmlHelper, ViewContext viewContext)
        {
            var lkpRigList = Ensco.Utilities.UtilitySystem.GetLookupList("Rig");
            if (lkpRigList == null)
            {
                lkpRigList = new LookupListModel<dynamic>();
                lkpRigList.Items = new List<object>();
            }

            var items = (lkpRigList.Items as List<object>)?.Cast<RigModel>();

            var item = items?.SingleOrDefault(it => it.Id.ToString() == RigChecklist.RigId);

            if (item != null)
                RigChecklist.RigName = item.Name;



            settings.Name = "planningLayout";
            settings.UseDefaultPaddings = false;
            settings.AlignItemCaptionsInAllGroups = true;
            settings.SettingsAdaptivity.AdaptivityMode = FormLayoutAdaptivityMode.SingleColumnWindowLimit;
            settings.SettingsAdaptivity.SwitchToSingleColumnAtWindowInnerWidth = 600;

            settings.Items.AddGroupItem(g =>
            {
                g.Caption = Translate("Planning");
                g.ColCount = 5;

                g.Items.Add(i =>
                {

                    i.ShowCaption = DefaultBoolean.False;
                    i.Border.BorderStyle = BorderStyle.None;
                    i.CssClass = "buttonToolbar";
                    i.Width = Unit.Percentage(100);
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtension().Button(s =>
                    {
                        s.Name = "savePlanningLayout";
                        s.Text = Translate("Save");
                        s.UseSubmitBehavior = false;
                        s.Width = Unit.Pixel(100);
                        s.ClientSideEvents.Click = $"function (s, e) {{ e.processOnServer =  false;  {GridConstants.rigChecklistAssessorGrid}.PerformCallback();  }}";
                        s.Enabled = RigChecklist.Status == "Open" && (RigChecklist.VerifiedBy.Any(v => v.UserId == UtilitySystem.CurrentUserId) || RigChecklist.Assessors.Any(v => v.UserId == UtilitySystem.CurrentUserId));
                    });
                });

                g.Items.Add(i =>
                {
                    i.ShowCaption = DefaultBoolean.False;
                    i.Name = "EmptyLayout";
                    i.Width = Unit.Percentage(100);
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.SetNestedContent(() => viewContext.Writer.Write("&nbsp;"));
                });

                g.Items.Add(m => m.RigChecklistUniqueId, i =>
                {
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.Width = Unit.Percentage(20);
                    i.Caption = Translate("Protocol Id");
                    i.NestedExtensionSettings.ControlStyle.CssClass = "system-field";
                    i.NestedExtension().TextBox(s =>
                    {
                        s.ReadOnly = true;
                        s.Text = RigChecklist.RigChecklistUniqueId.ToString();
                    });
                });

                g.Items.Add(m => m.Title, i =>
                {
                    i.Caption = Translate("OAP Title");
                    i.Width = Unit.Percentage(40);
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.TextBox;
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtensionSettings.Height = Unit.Percentage(10);
                    i.NestedExtensionSettings.Enabled = RigChecklist.Status == "Open";
                });

                g.Items.Add(m => m.OapChecklist.OapType, i =>
                {
                    i.Caption = Translate("OAP Type");
                    i.Width = Unit.Percentage(25);
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtensionSettings.ControlStyle.CssClass = "system-field";
                    i.NestedExtension().TextBox(s =>
                    {
                        s.ReadOnly = true;
                        s.Text = RigChecklist.OapChecklist.OapType.ToString();
                    });
                });

                g.Items.Add(m => m.Status, i =>
                {
                    i.Caption = Translate("Status");
                    i.Width = Unit.Percentage(15);
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtensionSettings.ControlStyle.CssClass = "system-field";
                    i.NestedExtension().TextBox(s =>
                    {
                        s.ReadOnly = true;
                        s.Text = RigChecklist.Status;
                    });
                });

                g.Items.Add(m => m.ChecklistDateTime, i =>
                {
                    i.Caption = Translate("Assessment Date & Time");
                    i.Width = Unit.Percentage(30);
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.DateEdit;
                    var nsettings = (DateEditSettings)i.NestedExtensionSettings;
                    nsettings.Properties.EditFormatString = UtilitySystem.Settings.ConfigSettings["DateFormat"];
                    nsettings.Properties.DisplayFormatString = UtilitySystem.Settings.ConfigSettings["DateFormat"];
                    nsettings.Enabled = RigChecklist.Status == "Open";
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                });

                g.Items.Add(m => m.ChecklistDateTimeCompleted, i =>
                {
                    i.Caption = Translate("Date / Time Completed");
                    i.Width = Unit.Percentage(30);
                    i.NestedExtensionType = FormLayoutNestedExtensionItemType.DateEdit;
                    var nsettings = (DateEditSettings)i.NestedExtensionSettings;
                    nsettings.Properties.EditFormatString = UtilitySystem.Settings.ConfigSettings["DateFormat"];
                    nsettings.Properties.DisplayFormatString = UtilitySystem.Settings.ConfigSettings["DateFormat"];
                    nsettings.Enabled = RigChecklist.Status == "Open";
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                });

                g.Items.Add(m => m.RigName, i =>
                {
                    i.Caption = Translate("Rig");
                    i.Width = Unit.Percentage(20);
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtensionSettings.ControlStyle.CssClass = "system-field";
                    i.NestedExtension().TextBox(s =>
                    {
                        s.ReadOnly = true;
                        s.Text = RigChecklist.RigName;
                    });
                });

                g.Items.Add(m => m.OapChecklist.OapType.IsIsm, i =>
                {
                    i.Caption = Translate("ISM Certified");
                    i.Width = Unit.Percentage(20);
                    i.NestedExtensionSettings.Width = Unit.Percentage(100);
                    i.NestedExtensionSettings.ControlStyle.CssClass = "system-field";
                    i.NestedExtension().RadioButtonList(s =>
                    {
                        s.Properties.RepeatDirection = RepeatDirection.Horizontal;
                        s.Properties.Items.Add("Yes", "1");
                        s.Properties.Items.Add("No", "0");
                        s.ControlStyle.Border.BorderStyle = BorderStyle.None;
                        s.Properties.ValidationSettings.RequiredField.IsRequired = false;
                    });
                });






            });
        }

    }
}
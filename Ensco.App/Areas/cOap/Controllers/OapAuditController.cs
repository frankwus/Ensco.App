using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System.Linq;
using System.Web;
using Ensco.Irma.Oap.Common.Extensions;
using Ensco.Irma.Oap.Common.Models;
using Ensco.Irma.Oap.WebClient.Corp;
using Ensco.Irma.Oap.Web.Oap.Controllers;
using Ensco.Irma.Oap.Web.Rig.Areas.Oap;
using Westwind.Globalization;

using System.Web.Routing;
using System.Collections.ObjectModel;
using Ensco.Utilities;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.Mvc.Html;
using Ensco.Models;
using Ensco.App.App_Start;
using Ensco.App.Infrastructure;
using Ensco.Services;
using DevExpress.Web.Mvc.UI;
using AutoMapper;
using DevExpress.Utils;
using System.ComponentModel;
using System.Web.UI.HtmlControls;

namespace Ensco.App.Areas.cOap.Controllers
{
   
    public class OapAuditController: OapCorpGridController
    {
        public const string CurrentAuditId = "CurrentAuditId";
        public const string CurrentAuditTitle = "CurrentAuditTitle";
        protected string AuditId { get; set; } = "AuditId";

        protected string AuditName { get; set; } = "Audits";

        private OapAuditClient OapAuditClient { get; }
        protected PeopleClient PeopleClient { get; }


        public IEnumerable<Person> Owner { get; set; }
        public OapAuditController() : base()
        {


            OapAuditClient = new OapAuditClient(GetApiBaseUrl(), Client);

            PeopleClient = new PeopleClient(GetApiBaseUrl(), Client);
        }
        private GridData AuditGridData { get; } = new GridData("OapAuditGrid", "OapAudit", "OpenAudits", "Open Audits", "AddNewAudit", "Add Audit", "search Audits", initializeCallBack: true);

        private void InitializeGridData(HtmlHelper html, ViewContext viewContext, GridData gridData, string createAction, string updateAction, string deleteAction)
        {
            //if (ViewData["OapTypes"] == null)
            //{
            //    var allOapTypes = GetOapTypes();
            //    ViewData["OapTypes"] = allOapTypes.Where(h => h.ParentHierarchyId == null);
            //    ViewData["OapSubTypes"] = allOapTypes.Where(h => h.ParentHierarchyId != null);
            //}
            if (ViewData["Users"] == null)
            {
                ViewData["Users"] = GetUsers(PeopleClient);
            }

            var users = ViewData["Users"] as IEnumerable<Person>;

            var usersCombo = new GridCombo("Users", users);

            var lkpList = Ensco.Utilities.UtilitySystem.GetLookupList("Rig");
            if (lkpList == null)
            {
                lkpList = new LookupListModel<dynamic>();
                lkpList.Items = new List<object>();
            }
            var items = (lkpList.Items as List<object>)?.Cast<RigModel>();
            if (ViewData["Rig"] == null)
            {
                ViewData["Rig"] = items;
            }
            var oapRigs = ViewData["Rig"];
            var oapRigsCombo = new GridCombo("RigModel", oapRigs);
          //  OapAudit t = new OapAudit();
           // t.OapAuditProtocols[0].RigOapCheckListId

            gridData.DisplayColumns = new List<GridDisplayColumn>()
                            {
                              new GridDisplayColumn("Id", displayName: "Audit Id",  columnAction: c =>
                                {
                                    c.FieldName = "Id";
                                    c.Caption = Translate("Audit Id");
                                    c.Width = Unit.Percentage(5);
                                    c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                    //c.SortDescending();
                                    c.EditorProperties().HyperLink(hl =>
                                        {
                                            Session["isAuditIdClick"] = false;
                                            var url = Url.Action("Index", "OapAuditReport", new RouteValueDictionary(new { Area = "coap", Id = "{0}" }));
                                            hl.NavigateUrlFormatString = HttpUtility.UrlDecode(url);
                                            hl.TextField = "Id";
                                            
                                        }
                                        
                                    );
            

                                }),
                                new GridDisplayColumn("Description", displayName:"Audit Description", width:40, editLayoutWidth:100),
                               //  new GridDisplayColumn("CreatedBy", displayName: "Lead Assessor", order:20,columnType:MVCxGridViewColumnType.TextBox, width:15),
                                new GridDisplayColumn("UpdatedBy", displayName: "Lead Assessor1", order:45, columnType:MVCxGridViewColumnType.TextBox, width:0, editLayoutWidth:100,
                                 columnAction: CommonUtilities.GetPassportColumnAction(html, viewContext,"UpdatedBy","Assessor",Translate("Lead Assessor"),users), isVisible:false),
                              //  new GridDisplayColumn("# Protocols", displayName:"Oap Type", columnType:MVCxGridViewColumnType.ComboBox, lookup:oapTypeCombo, width:15, editLayoutWidth:100),
                                new GridDisplayColumn("RigId", displayName:"Rig",width:0, columnType:MVCxGridViewColumnType.ComboBox,  lookup:oapRigsCombo, editLayoutWidth:100, isVisible:false
                                
    

        ),

                                new GridDisplayColumn("IsCVT", displayName:"Is CVT?", columnType:MVCxGridViewColumnType.CheckBox, width:0, editLayoutWidth:10, isVisible:false),
                                new GridDisplayColumn("RepeatFinding", displayName:"Repeat Findings", columnType:MVCxGridViewColumnType.CheckBox, width:0, editLayoutWidth:10, isVisible:false),
                                 new GridDisplayColumn("AuditLevel", displayName:"Audit Level",columnType:MVCxGridViewColumnType.ComboBox, width:0, editLayoutWidth:100, isVisible:false,columnAction:c => {
                                      c.Name = "AuditLevel";
                                    c.Caption = "Audit Level";
                                    c.FieldName = "AuditLevel";
                                    c.Width = Unit.Percentage(100);
                                    c.EditorProperties().ComboBox(cb =>
                                    {
                                        cb.DataSource = Enum.GetNames(typeof(AuditLevel));
                                        cb.Width = Unit.Percentage(100);

                                    });

                                 }),
                                new GridDisplayColumn("StartDateTime", displayName:"Date Started", columnType:MVCxGridViewColumnType.DateEdit, width:15, editLayoutWidth:100, displayFormat: "g"
                                    
                                ),
                               new GridDisplayColumn("EndDateTime", displayName:"Date Completed", columnType:MVCxGridViewColumnType.DateEdit, width:15, editLayoutWidth:100, displayFormat: "g"
                                   
                               ),
                                new GridDisplayColumn("OapAuditProtocols", displayName:"# Prototocols",columnAction: c =>
                                {
                                    c.FieldName = "OapAuditProtocols";
                                    c.Caption = "# Protocols";
                                    c.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
                                    c.Width = Unit.Percentage(8);
                                    c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                }

                                ),
                                new GridDisplayColumn("", displayName:"# Findings", width:10),
                               new GridDisplayColumn("Status", displayName:"Status", width:10, editLayoutWidth:100, isReadOnly:true),

                                new GridDisplayColumn("AuditPurpose", displayName:"Audit Purpose", columnType:MVCxGridViewColumnType.ComboBox, width:0, editLayoutWidth:100,isVisible:false, columnAction:c => {
                                    c.Name = "AuditPurpose";
                                    c.Caption = "Audit Purpose";
                                    c.FieldName = "AuditPurpose";
                                    c.Width = Unit.Percentage(100);
                                    c.EditorProperties().ComboBox(cb =>
                                    {
                                        cb.DataSource = Enum.GetNames(typeof(AuditPurpose));
                                        cb.Width = Unit.Percentage(100);
                                       
                                    });
                                   
                                }),

                                //new GridDisplayColumn("CreatedBy", displayName:"Created By", isReadOnly:true, width:10),
                                //new GridDisplayColumn("CreatedDateTime", displayName:"Date Created", columnType:MVCxGridViewColumnType.DateEdit, width:10, isReadOnly:true, displayFormat: "g"),
                                //new GridDisplayColumn("UpdatedBy", displayName:"Updated By", width:10, isReadOnly:true ),
                                //new GridDisplayColumn("UpdatedDateTime", displayName:"Date Updated", columnType:MVCxGridViewColumnType.DateEdit, width:10, isReadOnly:true, displayFormat: "g")
                            };

           

            gridData.Routes = new List<GridRoute>()
                            {
                                new GridRoute(GridRouteTypes.Add, new { Controller = gridData.Controller, Action = createAction }),
                                new GridRoute(GridRouteTypes.Update, new { Controller = gridData.Controller, Action = updateAction }),
                                new GridRoute(GridRouteTypes.Delete, new { Controller = gridData.Controller, Action = deleteAction }),
                            };






            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {

                                new GridEditLayoutColumn("Description", displayName:"Description", columnType: MVCxGridViewColumnType.Memo, layoutAction: col => {
                                    col.ColumnSpan = 2;
                                    col.Width = Unit.Percentage(100);
                                }),
                                new GridEditLayoutColumn("RigId", displayName:"Rig", columnType:MVCxGridViewColumnType.ComboBox),
                                 new GridEditLayoutColumn("IsCVT", displayName:"Is CVT?", columnType:MVCxGridViewColumnType.CheckBox),
                                 new GridEditLayoutColumn("AuditLevel", displayName:"Audit Level", columnType:MVCxGridViewColumnType.ComboBox),
                                new GridEditLayoutColumn("RepeatFinding", displayName:"Repeat Findings", columnType:MVCxGridViewColumnType.CheckBox),
                                 new GridEditLayoutColumn("UpdatedBy", displayName:"Lead Assessor"),
                                new GridEditLayoutColumn("Status", displayName:"Status"),
                                  new GridEditLayoutColumn("AuditPurpose", displayName:"Audit Purpose", columnType:MVCxGridViewColumnType.ComboBox),
                                new GridEditLayoutColumn("StartDateTime", displayName:"Start Date", columnType: MVCxGridViewColumnType.DateEdit),
                                
                                new GridEditLayoutColumn("EndDateTime", displayName:"End Date", columnType: MVCxGridViewColumnType.DateEdit),
                            };


            gridData.FormLayout = new GridEditFormLayout(
                     gridData.LayoutColumns
                     , editMode: GridViewEditingMode.PopupEditForm
                     , processLayout: i => {
                         i.ShowUpdateButton = true;
                         i.ShowCancelButton = true;
                         i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                     }
                     , columnCount: 2
                     , emptyLayputItemCount: 3
                     );

            gridData.RowInitializeEvent = (s, e) =>
            {

                e.NewValues["UpdatedBy"] = UtilitySystem.CurrentUserId;
                e.NewValues["Status"] = "Open";
                e.NewValues["IsCVT"] = false;
             //   e.NewValues["RigId"] = -1;
                e.NewValues["RepeatFinding"] = false;
                e.NewValues["StartDateTime"] = string.Empty;
                e.NewValues["EndDateTime"] = string.Empty;
                e.NewValues["SiteId"] = UtilitySystem.Settings.ConfigSettings["SiteId"];
                //gridData.DefaultNewRowInitializeFields(e);
            };


        }

        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            InitializeGridData(html, viewContext, AuditGridData, "CreateAudit", "UpdateAudits", "DeleteAudits");

            // Popup form customization
            settings.SettingsPopup.EditForm.Modal = true;
            settings.SettingsPopup.EditForm.HorizontalAlign = PopupHorizontalAlign.Center;
            settings.SettingsPopup.EditForm.VerticalAlign = PopupVerticalAlign.WindowCenter;
            settings.SettingsPopup.EditForm.CloseOnEscape = AutoBoolean.True;
            settings.SettingsText.PopupEditFormCaption = "Add " + AuditName;

            //Master Grid Specific configurations
           
            settings.KeyFieldName = AuditGridData.Key;
            settings.Name = $"{AuditGridData.Name}";
            settings.SettingsDetail.ShowDetailRow = false;
            settings.SettingsDetail.AllowOnlyOneMasterRowExpanded = false;
            settings.ClientSideEvents.BeginCallback = "function(s,e) {OnBeginCallback(s,e);}";
            settings.ClientSideEvents.EndCallback = "function(s,e) {OnEndCallback(s,e);}";
            //settings.ClientSideEvents.ToolbarItemClick = "function(s,e){e.processOnServer = false;}";


            //Generic configurations
            AuditGridData.Title = Translate(AuditGridData.Title);
            settings.Styles.AlternatingRow.BackColor = System.Drawing.Color.Red;
            settings.Configure(AuditGridData, html, viewContext);

            var column = settings.Columns["Description"] as MVCxGridViewColumn;
            if (column != null)
            {
                column.EditorProperties().Memo(m =>
                {
                    m.Width = Unit.Percentage(100);
                    m.Rows = 5;
                });
            }
           

            settings.SettingsExport.BeforeExport = (sender, e) => {
                MVCxGridView gridView = sender as MVCxGridView;
                if (sender == null)
                    return;
                gridView.Columns["OwnerId"].Visible = false;
            };
           
            settings.CellEditorInitialize = (s, e) =>
            {
                e.Editor.ReadOnly = false;

            };

            settings.CommandColumn.Visible = false;

            SelectRow((int)(ViewData[AuditId] ?? 0), (string)(ViewData[AuditName] ?? string.Empty), ref settings);
        }


        public static IEnumerable<Person> GetUsers(PeopleClient peopleClient)
        {
            var users = peopleClient.GetAllAsync().Result?.Result?.Data;

            return users;
        }
        public async Task<ActionResult> Index()
        {
            var response = await OapAuditClient.GetAllOapAuditsAsync("Open");
            IEnumerable<OapAudit> audits = null;
            return View(audits);

            // return (response == null && response.Result != null)? View(response):View(response.Result.Data.Where(c => c.RigId.Equals(1)).OrderByDescending(a=>a.Id));
        }

        public async Task<ActionResult>  RefreshAll()
        {
            var response = await OapAuditClient.GetAllOapAuditsAsync("Open");
            ViewBag.SelectedRBId = 0;
            return (response == null && response.Result == null) ? View("Index",response) : View("Index",response.Result.Data.OrderByDescending(a => a.Id));

        }


        public async Task<ActionResult> OpenAudits(int rrigId=0, bool isBuid = false)
        {
            IEnumerable<OapAudit> audits = null;
            if (rrigId == 0 && !isBuid) return PartialView("OapAuditPartial", audits);
            var response = await OapAuditClient.GetAllOapAuditsAsync("Open");
            IEnumerable<OapAudit> res = null;
            
            if (response != null && response.Result != null)
            {
                var t = response.Result.Data;


            //    if (rrigId == 0) return PartialView("OapAuditPartial", t);
                if (isBuid)
                {
                    if(rrigId==0) return PartialView("OapAuditPartial", t);
                    var lkpList = Ensco.Utilities.UtilitySystem.GetLookupList("Rig");
                    var rigs = (lkpList.Items as List<object>)?.Cast<RigModel>();

                    var rigIds = rigs.Where(r => r.BuId == rrigId).Select(si => si.Id).ToList();
                    res = t.Where(r => rigIds.Contains(r.RigId)).OrderByDescending(a=>a.Id);

                }
                else
                {
                    res = t.Where(c => c.RigId.Equals(rrigId)).OrderByDescending(a => a.Id);
                }
            }
           // ViewData["SelectedAuditId"] = res.FirstOrDefault() != null ? res.FirstOrDefault().Id : 0;
            return PartialView("OapAuditPartial", res);
        }

        

        protected virtual ObservableCollection<OapAudit> GetAuditData(string status = "Open")
        {
            var auditData = OapAuditClient.GetAllOapAuditsAsync(status).Result.Result.Data;

            return auditData;
        }

        [HttpPost]
        public virtual async Task<ActionResult> CreateAudit(OapAudit model)
        {
            RigOapChecklist modelr = new RigOapChecklist() { OapchecklistId = 69, OwnerId = UtilitySystem.CurrentUserId, Title = "Audit-" + model.Id.ToString(), RigId = model.RigId.ToString() };
            model.CreatedBy = model.UpdatedBy;
            var oapaudit = new OapAuditProtocol();
            oapaudit.AuditId = model.Id;
            oapaudit.RigOapCheckListId = Guid.Empty;
            oapaudit.RigOapChecklist = modelr;
            model.OapAuditProtocols = new ObservableCollection<OapAuditProtocol>();
            model.OapAuditProtocols.Add(oapaudit);
            // model.SiteId = "corp";
            var response = await OapAuditClient.AddOapAuditAsync(model);
           //  return RedirectToAction("Index");
           // ViewBag.SelectedRigId = model.RigId;
            return RedirectToAction("RefreshAll");
            // return null;

            // return PartialView("OapAuditPartial", GetAuditData());


        }

        //[HttpPost]
        //public ActionResult CreateAudit(OapAudit model)
        //{
        //    RigOapChecklist modelr = new RigOapChecklist() { OapchecklistId = 69, OwnerId = UtilitySystem.CurrentUserId, Title = "Audit-" + model.Id.ToString(), RigId = model.RigId.ToString() };
        //    model.CreatedBy = model.UpdatedBy;
        //    var oapaudit = new OapAuditProtocol();
        //    oapaudit.AuditId = model.Id;
        //    oapaudit.RigOapCheckListId = Guid.Empty;
        //    oapaudit.RigOapChecklist = modelr;
        //    model.OapAuditProtocols = new ObservableCollection<OapAuditProtocol>();
        //    model.OapAuditProtocols.Add(oapaudit);
        //    // model.SiteId = "corp";
        //    var response = OapAuditClient.AddOapAuditAsync(model);
        //    // return RedirectToAction("Index");
        //     return null;
            


        //}


    }

   

    public enum AuditPurpose
    {
        Audit,
        Mentoring,
        Other
    }

    public enum AuditLevel
    {
        Level_I,
        Level_II,
        Level_III
    }


}
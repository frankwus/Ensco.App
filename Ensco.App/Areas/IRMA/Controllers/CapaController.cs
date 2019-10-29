using Ensco.App.ProjectUtilities;
using Ensco.App.Helpers;
using Ensco.App.App_Start;
using Ensco.App.TLC;
using Ensco.Irma.Models;
using Ensco.Irma.Services;
using Ensco.Models;
using Ensco.Services;
using Ensco.TLC.Models;
using Ensco.TLC.Services;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.DataExtensions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System.Reflection;
using System.Data;
namespace Ensco.App.Areas.IRMA.Controllers
{
    public class CapaController : Ensco.App.Areas.Common.Controllers.BaseController
    {
        public int CapaId {
            get {
                int i = 0;
                if (this.Session["CapaId"] != null)
                    int.TryParse(this.Session["CapaId"].ToString(), out i);
                return i;
            }
            set {
                this.Session["CapaId"] = value;
            }
        }
        public void SetSession(string name, string value) {
            this.Session[name] = value;
        }
        public string GetSession(string name) {
            if (this.Session[name] == null)
                return "";
            return this.Session[name].ToString();
        }
        public ActionResult GridPartial(string type) {
            DataTable dt = this.GetDataSet("select * from vw_CapaAction where CapaId=" + this.CapaId.ToString() + " and type='" + type + "'").Tables[0];
            this.ViewBag.Type = type;
            string name = "grid";
            if (type == "Extension" || type == "Reassignment")
                name = type;
            this.GetRolePermission(new string[] { this.CapaId.ToString(), type });
            return PartialView(name + "Partial", dt);
        }
        public ActionResult ExtensionPartial(string type) {
            return this.GridPartial("Extension");
        }
        public ActionResult ReassignmentPartial(string type) {
            return this.GridPartial("Reassignment");
        }
        DataTableModel GetDataTable(string mType, string dType = null) {
            if (dType == null) {
                dType = "Ensco.Irma.Data." + mType;
                mType = "Ensco.Models." + mType + "Model";
            }
            dynamic handler = IrmaServiceSystem.GetServiceListModel(mType, dType);
            MethodInfo[] AllMethods = handler.GetType().GetMethods();
            MethodInfo fun = AllMethods.FirstOrDefault(mi => mi.Name == "GetQueryable" && mi.GetParameters().Count() == 1);
            DataTableModel dt = new DataTableModel(1, fun.Invoke(handler, new string[] { "Id" }));
            return dt;
        }
        DataTableModel GetActionDataTable(string type) {
            DataTableModel dt = this.GetDataTable("CapaAction");
            dt.Dataset = dt.Dataset.Where(string.Format(@" capaId={0} and type= ""{1}"" ", this.CapaId, type));
            return dt;
        }
        [HttpPost]
        public ActionResult GridUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] CapaActionModel model, string clientAction) {
            model.CapaId = this.CapaId;
            if (this.ModelState.IsValid) {
                if (clientAction == "Add")
                    ServiceSystem.Add(EnscoConstants.EntityModel.CapaAction, model, true);
                else if (clientAction == "Delete")
                    ServiceSystem.Delete(EnscoConstants.EntityModel.CapaAction, model, true);
                else
                    ServiceSystem.Save(EnscoConstants.EntityModel.CapaAction, model, true);
            } else
                this.SaveModelStateErrors();
            this.GetRolePermission(new string[] { this.CapaId.ToString(), model.Type });
            return PartialView("GridPartial", this.GetActionDataTable(model.Type));
        }
        public ActionResult Home() {
            if (this.UserId == null)
                return Redirect("/common/common");

            CorpCapaModel m = new CorpCapaModel();
            return View("Home", m);
        }
        string WhereInclude(string column, string passportId, bool multi = false) {
            if (multi) {
                return "( " + column + ".Contains(\"," + passportId + "\") or " + column + ".Contains(\"" + passportId + ",\") or " + this.WhereInclude(column, passportId) + ")";
            } else
                return string.Format("  " + column + "=\"{0}\" ", passportId);
        }
        public ActionResult OpenCapaPartial(string source, string sourceId, string sourceUrl, string gridName = null, bool readOnly=false  ) {
            string type = sourceUrl;

            if (type.StartsWith("/oap/Generic/List/",StringComparison.OrdinalIgnoreCase)) {
                type = "OapGenericList";
            }

            string where = " Status !='Closed' ";
            switch (type) {
                case "My":
                    where = this.FilterDataSet("0");
                    break;
                case "Rig":
                    where = this.FilterDataSet("1");
                    break;
                case "All":
                    break;
                case "CapaPlan":
                    where = " source like'CapaPlan%'   ";
                    break;
                case "OapGenericList":
                    where = " sourceId='" + sourceId + "'";
                    break;
                default:
                    where = " sourceUrl='" + type + "'";
                    break;
            }
            if (source.ToLower()=="capaplan")
                where = " source like'CapaPlan%' and sourceId=  '" +sourceId+"'" ;
            DataTable dt = this.GetDataSet("select * from vw_IrmaCapa where " + where).Tables[0];
            this.SetSession("source", source);
            this.SetSession("sourceId", sourceId);
            this.SetSession("sourceUrl", sourceUrl);
            this.ViewBag.Type = type;
            this.ViewBag.gridName = gridName;
            this.ViewBag.readOnly = readOnly;

            return PartialView("OpenCapaPartial", dt);
        }
        public ActionResult HistoryLogPartial(string source, string sourceId, string sourceUrl, string gridName = null) {
            string type = sourceUrl;

            string where = " Status !='Closed' ";
            switch(type) {
                case "My":
                    where = this.FilterDataSet("0");
                    break;
                default:
                    where = " sourceUrl='" + type + "'";
                    break;
            }
            where = " source like'"+source+"' and sourceId=  '" + sourceId + "'";
            DataTable dt = this.GetDataSet("select * from vw_IrmaHistoryLog where " + where).Tables[0];
            this.ViewBag.source = source; 
            this.ViewBag.sourceId= sourceId;
            this.ViewBag.sourceUrl=sourceUrl;
            this.ViewBag.Type = type;
            this.ViewBag.gridName = gridName;
            return PartialView("HistoryLogPartial", dt);
        }
        public ActionResult CapaPlanHome() {
            return View("CapaPlanHome");
        }
        public ActionResult OpenCapaPlanPartial() {
            string where = " 1=1";
            DataTable dt = this.GetDataSet("select * from vw_IrmaCapaPlan where " + where).Tables[0];
            return PartialView("OpenCapaPlanPartial", dt);
        }
        public ActionResult DataTableDataBinding() {
            DataTableModel dt = this.GetDataTable("IrmaCapa");
            return View(dt);
        }
        string FilterDataSet(string includeRig) {
            DataSet ds = this.GetDataSet(" exec usp_MyCapa " + UtilitySystem.CurrentMyUserId.ToString() + ", " + includeRig);
            object filter = ds.Tables[0].Rows[0][0];
            if (filter == DBNull.Value)
                return "1=11";
            return filter.ToString();
        }
        public ActionResult SearchPartial(int id) {
            IrmaCapaModel m = new IrmaCapaModel();
            DataTableModel dt = this.GetDataTable("IrmaCapa");
            this.ViewBag.dt = dt;
            return PartialView("SearchPartial", m);
        }
        [HttpPost]
        public ActionResult SearchPartial([ModelBinder(typeof(DevExpressEditorsBinder))]  IrmaCapaModel model) {
            DataTableModel dt = this.GetDataTable("IrmaCapa");
            string where = " 1=1 ";
            object value;
            string name;
            foreach (PropertyInfo prop in model.GetType().GetProperties()) {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                name = prop.Name;
                if (name == "AssignedTos")
                    continue;
                value = prop.GetValue(model, null);
                if (value == null || value.ToString() == "")
                    continue;
                if (type == typeof(DateTime))
                    continue;
                else if (type != typeof(int))
                    value = "\"" + value + "\"";
                else if (value.ToString() == "0")
                    continue;

                where += " and " + name + "=" + value;
            }
            dt.Dataset = dt.Dataset.Where(where);
            this.ViewBag.dt = dt;
            return PartialView("SearchPartial");
        }
        public void UpdateAttachment(int? id) {
            IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.Attachment);
            if (id != 0) {
                string source = this.Session["Source"] == null ? "" : this.Session["Source"].ToString();
                IEnumerable<AttachmentModel> attachments = ServiceSystem.GetAttachments(source, "0");
                if (attachments != null) {
                    foreach (AttachmentModel attachment in attachments) {
                        attachment.SourceFormId = id?.ToString();
                        attachment.SourceForm = "CapaPlan";
                        dataModel.Update(attachment);
                    }
                }
            } else {
                string[] arr = this.Request.Form.GetValues("Removed");
                if (arr != null)
                    foreach (string path in arr) {
                        AttachmentModel attachment = dataModel.GetItem(string.Format("FilePath =\"{0}\"", HttpUtility.UrlDecode(path)), "Id");
                        if (attachment != null) {
                            dataModel.Delete(attachment);
                        }
                    }
            }
        }

        [HttpPost]
        public ActionResult CapaPlan([ModelBinder(typeof(DevExpressEditorsBinder))]  IrmaCapaPlanModel model) {
            if (this.Request.Form["Submit"] == "Submit")
                model.Status = "Submit";
            if (model.OwnersList != null)
                model.Owners = string.Join(",", model.OwnersList);

            if (model.Id == 0) {
                model.DateCreated = DateTime.Now;
                model.Status = "Open";
                model = ServiceSystem.Add(EnscoConstants.EntityModel.IrmaCapaPlan, model, true);
                string newId = this.Request.Form["SourceFormId"];
                if (newId != null)
                    this.GetDataSet("update Common_Attachments set SourceFormId=" + model.Id.ToString() + " where SourceFormId=" + newId);
            } else
                ServiceSystem.Save(EnscoConstants.EntityModel.IrmaCapaPlan, model, true);

            if (model.Status == "Open")
                this.UpdateAttachment(model.Id);
            this.UpdateAttachment(0);
            return Redirect("/Irma/Capa/CapaPlan/" + model.Id.ToString());
        }
        public ActionResult CapaPlan(int? id = 0) {
            IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.IrmaCapaPlan);
            IrmaCapaPlanModel m = new IrmaCapaPlanModel();
            if(id == 0) {
                this.Session["Source"] = Guid.NewGuid().ToString();
                m.RigId = UtilitySystem.CurrentRigId; 
            } else
                m = dataModel.GetItem("Id=" + id.ToString(), "Id");
            m.OwnersList =( m.Owners == null ? UtilitySystem.CurrentUserId.ToString() : m.Owners).Split(',');
            Session["CapaPlan"] = m;
            bool readOnly = true ;
            if ( id==0 || (m.OwnersList.Contains(UtilitySystem.CurrentUserId.ToString()) && m.Status == "Open") ) 
                readOnly = false;
            this.ViewBag.readOnly = readOnly; 

            this.ViewBag.dsSummary = this.GetDataSet("exec usp_GetCapaPlanSummary " + id.ToString());
            return View(m);
        }
        public ActionResult SummaryPartial(string  parameter) {
            this.ViewBag.dsSummary = this.GetDataSet("exec usp_GetCapaPlanSummary " + parameter );
            return PartialView("SummaryPartial", parameter); 
        }
        public ActionResult CapaPlanDelete(int id ) {
            IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.IrmaCapaPlan);
            IrmaCapaPlanModel m = new IrmaCapaPlanModel();
            m.Id = id; 
            dataModel.Delete(m, true);
            return Redirect("/irma/capa/CapaPlanHome"); 
        }
        public ActionResult CapaPlanClone(int id) {
            return this.GetJson(" exec usp_CapaPlanClone " + id.ToString());
        }
        public ActionResult DeleteCapa( ) {
            return this.GetJson(" exec usp_DeleteCapa " + this.CapaId.ToString());
        }
        public ActionResult ExecutionPartial(int id) {
            string mType = "Ensco.Models.CapaPlanExecutionModel";
            string dType = "Ensco.Irma.Data.vw_CapaPlanExecution";
            DataTableModel dt = this.GetDataTable(mType, dType);
            dt.Dataset = dt.Dataset.Where(string.Format(@" sourceId={0} ", id));
            return PartialView("ExecutionPartial", dt);
        }
        public ActionResult Index(int? id = 0, string source = "", string sourceUrl = "", string sourceId = "") {
            IrmaCapaModel model = new IrmaCapaModel();
            Session["IrmaCapaModel"] = model;
            if (id == 0) {
                this.SetSession("source", source);
                this.SetSession("sourceId", sourceId);
                this.SetSession("sourceUrl", sourceUrl);
            }
            this.CapaId = id.Value;
            this.ViewBag.Extension = this.GetActionDataTable("Extension");
            this.ViewBag.Reassignment = this.GetActionDataTable("Reassignment");
            DataSet ds = this.GetDataSet("usp_GetCapaExtensionRequest", new string[] { id.ToString() });
            this.ViewBag.DataSet = ds;
            return View(id);
        }
        public ActionResult AssignPartial(int id) {
            this.GetRolePermission(new string[] { this.CapaId.ToString(), "Create" });
            IrmaCapaModel m = this.GetModel(id);
            if(id == 0) {
                //if (this.ViewData["source"] != null) {
                //    //this.SetSession("source", this.ViewData["source"].ToString());
                //    //this.SetSession("sourceId", this.ViewData["sourceId"].ToString());
                //    //this.SetSession("sourceUrl", this.ViewData["sourceUrl"].ToString());
                //    m.Source = this.GetSession("source");
                //    m.DateAssigned = DateTime.Now; 
                //}
                m.DateAssigned = DateTime.Now;
                m.Source = this.GetSession("source");
                this.Session["Status"] = m.Status;
            } else
                this.SetSession("source", m.Source); 
            return PartialView("AssignPartial", m);
        }
        public ActionResult SubmitForVerification(int id) {
            IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.IrmaCapaPlan);
            IrmaCapaPlanModel m = new IrmaCapaPlanModel();

            m = dataModel.GetItem("Id=" + id.ToString(), "Id");
            m.Status = "Pending Verification";
            ServiceSystem.Save(EnscoConstants.EntityModel.IrmaCapaPlan, m, true);
            TaskModel taskModel = new TaskModel();
            taskModel.AssignedAt = DateTime.Now;
            taskModel.AssignedBy = UtilitySystem.CurrentUserId;
            int oimId = 0;
            DataTable dt = this.GetDataSet("select dbo.fn_OimPassportId() ").Tables[0];
            if(dt.Rows.Count > 0)
                int.TryParse(dt.Rows[0][0].ToString(), out oimId);
            taskModel.AssigneeUserId = oimId; 
            taskModel.SourceForm = "CapaPlan";
            taskModel.SourceFormId = id.ToString();
            taskModel.SourceFormURL = "/irma/capa/capaPlan/" + id.ToString();
            taskModel.Status = "Pending";
            taskModel.Message = "";
            ServiceSystem.Add(EnscoConstants.EntityModel.Task, taskModel, true);
            return null;
        }
        IrmaCapaModel GetModel(int id) {
            IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.IrmaCapa);
            IrmaCapaModel m = dataModel.GetItem("Id=" + id.ToString(), "Id");
            if (m != null) {
                m.AssignedTos = m.AssignedTo == null ? null : m.AssignedTo.Split(',');
                //m.NotifyIds = m.NotifyId == null ? null : m.NotifyId.Split(',');
            } else
                m = new IrmaCapaModel();
            this.CapaId = m.Id;
            return m;
        }
        public ActionResult RespondPartial(int id) {
            this.GetRolePermission(new string[] { this.CapaId.ToString(), "Implementation" });
            return PartialView("RespondPartial", this.GetModel(id));
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AssignPartial([ModelBinder(typeof(DevExpressEditorsBinder))]  IrmaCapaModel model) {
            string hCapaControl = this.Request.Form["hCapaControl"];
            if (model.Id == 0)
                model.Status = "Open";
            if ((hCapaControl != null && hCapaControl.Contains("Submit")) || (hCapaControl == null && this.Request.Form["btnCapaSubmit"] != null))
                model.Status = "Submit";
            if (model.AssignedTos != null)
                model.AssignedTo = string.Join(",", model.AssignedTos);
            model.Source = this.GetSession("source");
            if(model.Id == 0) {
                model.DateAssigned = DateTime.Now;
                model.SourceId = this.GetSession("sourceId");
                model.SourceUrl = this.GetSession("SourceUrl");
                model = ServiceSystem.Add(EnscoConstants.EntityModel.IrmaCapa, model, true);
            } else
                ServiceSystem.Save(EnscoConstants.EntityModel.IrmaCapa, model, true);
            if (hCapaControl != null)
                return null;
            return RedirectToAction("Index/" + model.Id.ToString());
        }
        [HttpPost]
        public ActionResult ReviewPartial([ModelBinder(typeof(DevExpressEditorsBinder))]  IrmaCapaModel model) {
            if (model.Id == 0)
                model = ServiceSystem.Add(EnscoConstants.EntityModel.IrmaCapa, model, true);
            else
                ServiceSystem.Save(EnscoConstants.EntityModel.IrmaCapa, model, true);

            return RedirectToAction("Index/" + model.Id.ToString());
        }
        private void SaveModelStateErrors() {
            string message = string.Empty;
            foreach (var entry in ModelState) {
                if (entry.Value.Errors.Count > 0) {
                    foreach (var error in entry.Value.Errors) {
                        message += string.Format("Field: {0}. Error: {1}", entry.Key, error.ErrorMessage);
                        message += Environment.NewLine;
                    }
                }
            }
            ViewData["EditError"] = message;
        }
        public ActionResult UpdateCapaAction(string[] arr) {
            arr = arr.Concat(new string[] { this.UserId }).ToArray();
            (new Helper()).GetDataSet("usp_UpdateCapaAction", arr);
            return null;// Redirect("index/"+this.CapaId.ToString()) ;
        }
        public ActionResult VerifyCapaPlan(string[] arr) {
            arr = arr.Concat(new string[] { this.UserId }).ToArray();
            (new Helper()).GetDataSet("usp_VerifyCapaPlan", arr);
            return null;
        }
        public ActionResult GetCapaAction(string[] arr) {
            arr = arr.Concat(new string[] { this.UserId }).ToArray();
            return this.GetJson("usp_GetCapaAction", arr);
        }
        public ActionResult GetTaskAlert(int passportId) {
            return this.GetJson(" exec usp_GetTaskAlert " + passportId.ToString());
        }
        [HttpPost]
        public ActionResult RespondPartial([ModelBinder(typeof(DevExpressEditorsBinder))]  IrmaCapaModel model) {
            IrmaCapaModel m = this.GetModel(this.CapaId);
            m.DateCompleted = model.DateCompleted;
            m.WO = model.WO;
            m.CompletionDescription = model.CompletionDescription;
            ServiceSystem.Save(EnscoConstants.EntityModel.IrmaCapa, m, true);

            return RedirectToAction("Index/" + this.CapaId.ToString());
        }
        public ActionResult SubmitResponse(string[] arr) {
            IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.Attachment);
            IrmaCapaModel m = this.GetModel(this.CapaId);
            m.DateCompleted = DateTime.Parse(arr[0]);
            m.WO = arr[1];
            m.CompletionDescription = arr[2];
            ServiceSystem.Save(EnscoConstants.EntityModel.IrmaCapa, m, true);
            // if(arr != null)
            for (int i = 3; i < arr.Length; i++) {
                AttachmentModel attachment = dataModel.GetItem(string.Format("FilePath =\"{0}\"", HttpUtility.UrlDecode(arr[i])), "Id");
                if (attachment != null) {
                    dataModel.Delete(attachment);
                }
            }
            //     this.UpdateAttachment(this.CapaId);
            //this.UpdateAttachment(0);

            return RedirectToAction("Index/" + this.CapaId.ToString());
            return null;
        }
        public ActionResult RespondRequest(string[] arr) {
            this.GetDataSet("usp_UpdateCapaRequest", this.AddArr(arr, this.UserId));
            return null;
        }
        public ActionResult Reset(string[] arr) {
            return this.GetJson("usp_CapaReset", arr);
        }
        public ActionResult GetRolePermission(string[] arr) {
            DataSet ds = this.GetDataSet("usp_CapaRolePermission", this.AddArr(arr, this.UserId));
            bool readOnly = true;
            if (ds.Tables[0].Rows[0][0].ToString() == "1")
                readOnly = false;

            this.ViewBag.ReadOnly = readOnly;
            this.ViewBag.ReadOnlyRespond = ds.Tables[0].Rows[0][1].ToString() == "1" ? false : true;
            return this.GetJson("usp_CapaRolePermission", this.AddArr(arr, this.UserId));
        }
        string[] AddArr(string[] arr, string s) {
            return arr.Concat(new string[] { s }).ToArray();
        }
        public ActionResult GetRigPosition(int id) {
            return this.GetJson(" exec usp_GetRigPositionFromPassport " + id.ToString());
        }
        [ValidateInput(false)]
        public ActionResult AddCapaAction(string data, string type) {
            CapaActionModel model = new CapaActionModel();
            model.CapaId = this.CapaId;
            if (type == "Implementation") {
                model.Who = UtilitySystem.CurrentUserId;
                model.Comment = data;
                model.Date = DateTime.Now;
            } else
                model.Who = int.Parse(data);
            model.Type = type;

            ServiceSystem.Add(EnscoConstants.EntityModel.CapaAction, model, true);
            return null;
        }
        [ValidateInput(false)]
        public ActionResult Clone(string data ) {
            CapaActionModel model = new CapaActionModel();
            this.GetJson(" exec usp_CloneCapa " + this.CapaId.ToString()+", '"+data+"'");
            return null;
        }
        public ActionResult UploadPartial(int id, string source, bool readOnly =false ) {
            AttachmentSource att = new AttachmentSource();
            att.SourceForm = source;
            att.SourceFormId = id;
            att.Attachments = ServiceSystem.GetAttachments(source, id.ToString());
            this.ViewBag.readOnly = readOnly; 
            return PartialView("UploadPartial", att);
        }
        public ActionResult GetTabTranslation(string name) {
            return new ContentResult {
                Content = "[[\"" + EnscoHelper.GetTranslation(name) + "\" ]]",
                ContentType = "application/json",
                ContentEncoding = System.Text.Encoding.UTF8
            };
        }

    }
    public class AttachmentSource
    {
        public string SourceForm;
        public int SourceFormId;
        public IEnumerable<AttachmentModel> Attachments;
    }
}

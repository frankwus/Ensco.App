using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web; 
using System.IO;
using System.Collections; 
using System.Collections.Generic;
using System.Xml.Linq;
using System.Data.Entity.Validation;
using Ensco.Utilities;
using System.Web.Script.Serialization;

using mUtilities.Data;
using System.Data.SqlClient;
using System.Text;
using System.DirectoryServices;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using System.Diagnostics;

namespace Ensco.App.Areas.Common.Controllers { 
    [Ensco.App.Infrastructure.CustomAuthorize]
    public class BaseController : Controller {
     public    DataAccessor da = new DataAccessor(UtilitySystem.Settings.ConfigSettings["ConnectionString"]);
        public string UserId {
            get {
                string userId = null;// this.GetCookie("UserId");
                if (userId == null) {
                    if (HttpContext == null || HttpContext.Session==null )
                        return null; 
                    string key = HttpContext.Session.SessionID + "_UserSessionInfo";
                    UserSession userSession = (UserSession)HttpContext.Session[key];
                    if (userSession == null)
                        return null;
                    userId = userSession.Passport;
                }
                ViewBag.UserId = userId;
                return userId;
            }
            //set { _name = value; }
        }
        public string PassportId {
            get {
                DataSet ds = this.da.GetDataSet("select * from master_users where passport='" + this.UserId + "'"); 
                return ds.Tables[0].Rows.Count==0? "" : ds.Tables[0].Rows[0]["Id"].ToString(); 
            }
        }
        public BaseController() {

        }
       public string GetCookie(string name) {
            HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies[name];
            if (cookie == null)
                return "";
            return cookie.Value;
        }
        public void SetCookie(string name, string value) {
            if (value == null)
                value = "";
            value = value.Trim();
            if (this.ControllerContext.HttpContext.Request == null)
                return;
            this.ControllerContext.HttpContext.Request.Cookies.Remove(name);
            HttpCookie cookie = new HttpCookie(name);
            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddDays(1);
            this.ControllerContext.HttpContext.Request.Cookies.Add(cookie);
            this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
        }
        public string GetParameter(List<string> list) {
            return "'" + string.Join("','", list.ToArray()) + "'";
        }
        public ActionResult GetJson(string sp, string sort, string where, string pageNo, string pageSize) {
            string sql = "exec " + sp + this.GetParameter(new List<string> { sort, where.Replace("'", "''"), pageNo, pageSize });
            if (pageSize == "1000000") {
                DataSet ds = this.GetDataSet(sql);
                ds.Tables.RemoveAt(2);
                ds.Tables.RemoveAt(0);
                string fileName = this.GetCaller(2);
                this.Export(ds, HttpContext.ApplicationInstance.Response, fileName);
                return View();
            }
            return Content(this.Json(sql));
        }
        public ActionResult GetJson(string sql) {
            return Content(this.Json(sql));
        }
        public  string Json(string sql) {
            return DataSetToJSON(GetDataSet(sql));
        }
        public string GetCaller(int i) {
            StackTrace trace = new StackTrace();
            StackFrame frame = trace.GetFrame(i);
            return frame.GetMethod().Name.Replace("Get", "");
        }
        public  DataSet GetDataSet(string sql) {
            DataSet ds = da.GetDataSet(sql);
            return ds;
        }
        public static string DataSetToJSON(DataSet ds) {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            ArrayList root = new ArrayList();
            List<Dictionary<string, object>> table;
            Dictionary<string, object> data;

            foreach (DataTable dt in ds.Tables) {
                table = new List<Dictionary<string, object>>();
                foreach (DataRow dr in dt.Rows) {
                    data = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns) {
                        data.Add(col.ColumnName, dr[col]);
                    }
                    table.Add(data);
                }
                root.Add(table);
            }
            return serializer.Serialize(root);
        }
        
        public  void Export(DataSet ds, HttpResponse Response, string fileName) {
            string html = "";
            fileName += " " + DateTime.Now.ToString() + ".xls";
            foreach (DataTable dt in ds.Tables) {
                var gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();

                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                html += objStringWriter.ToString() + "<br/><br/>";
            }
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";

            Response.Output.Write(html);
            Response.Flush();
            Response.End();
        }
        public void SetObjectProperty(string propertyName, string value, object obj) {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
            if (propertyInfo != null) {
                string dataType = propertyInfo.PropertyType.Name;
                value = value.Replace("\"", "");
                if (dataType == "String")
                    propertyInfo.SetValue(obj, value, null);
                else if (propertyInfo.PropertyType.FullName.Contains("Bool"))
                    propertyInfo.SetValue(obj, value == "true", null);
                else if (propertyInfo.PropertyType.FullName.Contains("DateTime"))
                    propertyInfo.SetValue(obj, DateTime.Parse(value), null);
                else {
                    //"new Date(2018,9,28)"
                    int i; 
                    if (int.TryParse( value, out i ))
                        propertyInfo.SetValue(obj, i, null);
                }
            }
        }
        public ActionResult GetJson( string sp, string[] arr) {
            return Content((new Helper()).GetJsonString(sp, arr ));
        }
        public DataSet GetDataSet (string sp, string[] arr) {
            return (new Helper()).GetDataSet(sp, arr);
        }
    }
}

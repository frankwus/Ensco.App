using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using mUtilities.Data;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Configuration;
using Ensco.Utilities;

namespace Ensco.App
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        char[] delimiterChars = { '\\', '\t' };
        DataAccessor da = new DataAccessor(ConfigurationManager.AppSettings["ConnectionString"]);
        DataSet ds;
        SortedList sl = new SortedList();

        [WebMethod]
        public string GetUserId()
        {
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            string userId = identity.Name.ToString();
            int index = userId.IndexOf('\\');
            if (index > 0)
                userId = userId.Substring(index + 1);

            return userId;
        }

        [WebMethod]
        public string GetUserInfo()
        {
            //System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            //string userId = identity.Name.ToString();
            //int index = userId.IndexOf('\\');
            //if (index > 0)
            //    userId = userId.Substring(index + 1);
            //Helper helper = new Helper();
            //string[] arr = helper.GetDirectoryEntry(userId);
            //return userId + ":" + arr[1];
            string key = HttpContext.Current.Session.SessionID + "_UserSessionInfo";
            UserSession userInfo = (UserSession)Session[key];
            return string.Format("{0}:{1}", userInfo.Passport, userInfo.UserName);
        }
        [WebMethod(EnableSession = true)]
        public string RunArray(object array)
        {
            string userId = HttpContext.Current.User.Identity.Name;
            List<object> list = new JavaScriptSerializer().ConvertToType<List<object>>(array);
            string sp = list[0].ToString();
            //userId = HttpContext.Current.Session["UserId"].ToString();
            list.RemoveAt(0);
            string sql = "exec " + sp;
            if (list.Count > 0)
                sql += " '" + string.Join("', '", list.ToArray()) + "'";
            da.GetDataSet("insert tbl_log select getdate(), '" + sql.Replace("'", "''") + "', '" + userId + "'");
            try
            {
                ds = da.GetDataSet(sql);
                if (ds.Tables.Count == 0)
                    return "";
                return this.GetNewDs(ds).GetXml();
            }
            catch (Exception e)
            {
                this.SendEmail(sql);
                throw;
            }
            return null;
        }
        [WebMethod]
        public string RunArrayRaw(object array)
        {
            string userId = HttpContext.Current.User.Identity.Name;
            List<object> list = new JavaScriptSerializer().ConvertToType<List<object>>(array);
            string sp = list[0].ToString();
            list.RemoveAt(0);
            string sql = "exec " + sp;
            if (list.Count > 0)
                sql += " '" + string.Join("', '", list.ToArray()) + "'";
            try
            {
                ds = da.GetDataSet(sql);
                if (ds.Tables.Count == 0)
                    return "";
                return ds.GetXml();
            }
            catch (Exception e)
            {
                this.SendEmail(sql);
                throw;
            }
            return null;
        }
        DataSet GetNewDs(DataSet ds)
        {
            DataSet ds1 = new DataSet();
            foreach (DataTable dt in ds.Tables)
                ds1.Tables.Add(this.GetNullFilledDataTableForXML(dt));

            return ds1;
        }
        void SendEmail(string sql)
        {
            sl = new SortedList();
            sl.Add("email", "fwang@enscoplc.com");
            string db = ConfigurationManager.AppSettings["ConnectionString"];
            string[] stringSeparators = new string[] { "Source" };
            db = db.Split(stringSeparators, StringSplitOptions.None)[1];
            sl.Add("title", "IRMA error    " + db + this.GetUserInfo());
            sql = HttpUtility.HtmlEncode(sql);
            sl.Add("s", sql);
            sl.Add("cc", "");
            da.GetDataSet("usp_sendEmail", sl);
        }
        [WebMethod]
        public string RunSearch(object array)
        {
            string userId = HttpContext.Current.User.Identity.Name;
            List<string> list = new JavaScriptSerializer().ConvertToType<List<string>>(array);
            string sp = list[0].ToString();
            list.RemoveAt(0);
            for (int i = 0; i < list.Count; i++)
                if (list[i] == "")
                    list[i] = "null";
            string sql = "exec " + sp;
            if (list.Count > 0)
                sql += " '" + string.Join("', '", list.ToArray()) + "'";
            sql = sql.Replace("'null'", "null");
            try
            {

                ds = da.GetDataSet(sql);
                if (ds.Tables.Count == 0)
                    return "";
                return this.GetNewDs(ds).GetXml();
            }
            catch (Exception e)
            {
                sl.Add("email", "fwang@enscoplc.com");
                sl.Add("title", "IRMA error");
                sl.Add("s", sql);
                sl.Add("cc", "");
                da.GetDataSet("usp_sendEmail", sl);
                throw;
            }
            return null;
        }
        [WebMethod]
        public string RunSP(object items)
        {
            List<object> list = new JavaScriptSerializer().ConvertToType<List<object>>(items);
            string sp = "";
            SortedList sl = new SortedList();
            foreach (object o in list)
            {
                Dictionary<string, object> dict = (Dictionary<string, object>)o;
                foreach (string k in dict.Keys)
                {
                    if (k == "sp")
                        sp = dict[k].ToString();
                    else
                        sl.Add("@" + k, dict[k].ToString());
                }
            }
            ds = da.GetDataSet(sp, sl);
            if (ds.Tables.Count == 0)
                return "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    if (dr[dc] == System.DBNull.Value)
                        switch (dc.DataType.Name)
                        {
                            case "Int32":
                                dr[dc] = 0;
                                break;
                            case "String":
                                dr[dc] = "";
                                break;
                            case "DateTime":
                                break;
                            default:
                                break;
                        }
                }
            }

            return ds.GetXml();
        }

        [WebMethod]
        public string RunSP2(string sp, string where)
        {
            where = HttpUtility.HtmlDecode(where);
            SortedList sl = new SortedList();
            sl.Add("@where", where);
            ds = da.GetDataSet(sp, sl);
            return ds.GetXml();
        }

        [WebMethod]
        public string RunSql(string sql)
        {
            return "";
            string userId = HttpContext.Current.User.Identity.Name;
            sql = HttpUtility.HtmlDecode(sql);
            SortedList sl = new SortedList();
            sl.Add("userId", userId);
            sl.Add("s", sql);

            da.GetDataSet("utl_DataTrace", sl);
            ds = da.GetDataSet(sql);
            return ds.GetXml();
        }
        private DataTable GetNullFilledDataTableForXML(DataTable dtSource)
        {

            DataTable dtTarget = dtSource.Clone();
            foreach (DataColumn col in dtTarget.Columns)
                col.DataType = typeof(string);

            int colCountInTarget = dtTarget.Columns.Count;
            foreach (DataRow sourceRow in dtSource.Rows)
            {
                DataRow targetRow = dtTarget.NewRow();
                targetRow.ItemArray = sourceRow.ItemArray;

                for (int ctr = 0; ctr < colCountInTarget; ctr++)
                    if (targetRow[ctr] == DBNull.Value)
                        targetRow[ctr] = String.Empty;

                dtTarget.Rows.Add(targetRow);
            }
            return dtTarget;
        }
    }
}

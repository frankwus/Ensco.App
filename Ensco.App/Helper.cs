using System;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.DirectoryServices;
using System.Collections.Generic;
using System.Threading;
using System.Security;
using System.Security.Principal;
using System.DirectoryServices.AccountManagement;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using mUtilities.Data;
using System.Configuration;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
public class Helper
{
    public DataAccessor da = new DataAccessor(ConfigurationManager.AppSettings["ConnectionString"]);
    public DataSet ds;
    public string BaseUrl = ConfigurationManager.AppSettings["BaseUrl"];
    char[] delimiterChars = { '\\', '\t' };

    protected Color Orange = Color.FromArgb(232, 109, 31);
    protected Color Deepblue = Color.FromArgb(0, 70, 127);
    protected Color Lightblue = Color.FromArgb(61, 183, 228);

    public string ADUserId = ConfigurationManager.AppSettings["ADUserId"];
    public string ADPwd = ConfigurationManager.AppSettings["ADPwd"];
    public string AD = ConfigurationManager.AppSettings["AD"];
    public string UserName = "";
    public string UserId = "";
    public DataSet GetDataSet(string name, string[] arr) {
        string sql = " exec " + name + (arr.Length == 0 ? " " : " '" + string.Join("','", arr) + "'");
        sql = " exec sp_executeSql N'" + sql.Replace("'", "''") + "'";
        try {
            return this.da.GetDataSet(sql);
        } catch ( Exception ex) {
            string s = sql.Replace("'", "''") + "\n" + ex.Message; 
           this.da.GetDataSet(" exec usp_SendEmail 'fwang@enscoplc.com', 'Sql Error', '" + s + "'"); 
        }
        return null;
    }
    public string GetJsonString(string name, string[] arr) {
        return this.DataSetToJSON(this.GetDataSet(name, arr)); 
    }
    public static void WriteCookie(string userId) {
        HttpCookie c = new HttpCookie("Login");
        c.Value = userId;
        c.HttpOnly = true;
        HttpContext.Current.Response.Cookies.Add(c);
    }
    public static string ReadCookie() {
        HttpCookie c = HttpContext.Current.Request.Cookies["Login"];
        if (c == null)
            return "";
        return c.Value;
    }
    public void Log(string s) {
        s = s.Replace("'", "''");
        this.da.GetDataSet(" insert tbl_error select getdate(), '" + s + "'");

    }
    public  string DataSetToJSON(DataSet ds) {
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
    public string[] GetDirectoryEntry(string name) {
        string[] arr = new string[] { "sAMAccountName", "givenName", "sn", "mail" };
        DirectoryEntry entry = new DirectoryEntry("LDAP://" + this.AD + "/dc=ensco,dc=ws", this.ADUserId, this.ADPwd);// CN=users,DC=enscoplc,DC=com");
        DirectorySearcher searcher1 = new DirectorySearcher(entry);
        searcher1.Filter = "(&(objectClass=user)(sAMAccountName=" + name + "))";
        SearchResult r = searcher1.FindOne();

        DirectoryEntry de = r.GetDirectoryEntry();
        this.UserName = de.Properties["DisplayName"][0].ToString();
        string email = de.Properties["mail"][0].ToString();
        string position = de.Properties["title"][0].ToString();
        string[] arr1 = { this.UserId, this.UserName, email, position };

        return arr1;
    }
    public static string GetHash(string inputString) {
        byte[] data = System.Text.Encoding.ASCII.GetBytes(inputString);
        data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
        String hash = System.Text.Encoding.ASCII.GetString(data);
        return hash;
    }


}


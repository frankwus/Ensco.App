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
using System.IO;
//using Tools;
using Ensco.Utilities;

public partial class Base : System.Web.UI.Page {
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
    protected void Page_Init(object sender, EventArgs e) {
        if (this.ExportToExcel())
            return;
        //this.Trace.IsEnabled = true;
        UserId = UtilitySystem.CurrentUserPassport;
        UserName = UtilitySystem.CurrentUserName;
        HttpContext.Current.Session["UserId"] = UserId;

        SortedList sl = new SortedList();
        sl.Add("@userId", this.UserId);
        ds = this.da.GetDataSet("usp_userAccess", sl);
        string s = "<script> var Roles=[";
        ArrayList roleList = new ArrayList();
        foreach (DataRow dr in ds.Tables[0].Rows) {
            roleList.Add("'" + dr["role"] + "'");
        }
        s += String.Join(",", roleList.ToArray()) + "] \n";
        s += " var UserId='" + this.UserId + @"', UserName=""" + this.UserName + @"""    </script>";
        this.Response.Write(s);


        this.InitBreadCrumb();
    }  
    bool ExportToExcel() {
        string h1 = this.Request.Form["h1"]; 
        if ( h1 !=null ) {
            HttpResponse response = this.Response; 

            response.Clear();
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment; filename=irma.xls");
            response.ContentType = "application/vnd.ms-excel";

            response.Write(h1);
            Response.Flush();
            Response.End();
            return true;
        }
        return false ; 
    }
    protected void InitBreadCrumb() {
        string[] arr = { "empdirectory", "popuprig", "wisrequest", "PopupStandard", "upload" };
        string url = this.Request.RawUrl.ToLower();
        foreach (string s in arr) {
            if (url.Contains(s.ToLower()))
                return;
        }

        var form1 = Form.FindControl("form1");
        if (form1 != null) {
            //BreadCrumbControl breadcrumbCtrl = (BreadCrumbControl)LoadControl("~/common/BreadCrumbControl.ascx");
            //breadcrumbCtrl.UserName = this.UserName;
            //form1.Controls.AddAt(0, breadcrumbCtrl);
        }
    }
    DirectoryEntry GetDirectoryEntry(string name) {
        string[] arr = new string[] { "sAMAccountName", "givenName", "sn", "mail" };
        DirectoryEntry entry = new DirectoryEntry("LDAP://" + this.AD + "/dc=ensco,dc=ws", this.ADUserId, this.ADPwd);// CN=users,DC=enscoplc,DC=com");
        DirectorySearcher searcher1 = new DirectorySearcher(entry);
        searcher1.Filter = "(&(objectClass=user)(sAMAccountName=" + name + "))";
        SearchResult r = searcher1.FindOne();

        DirectoryEntry de = r.GetDirectoryEntry();
        return de;
    }
    public string GetUserId() {
        //if (!String.IsNullOrEmpty(Page.User.Identity.Name)) {
        //    this.UserId = Page.User.Identity.Name.ToString().Split(delimiterChars)[1];
        //} else {
        //    this.UserId = System.Web.HttpContext.Current.Request.LogonUserIdentity.Name.Split(delimiterChars)[1];
        //}
        //if (this.Request.QueryString["userId"] != null && this.Request.QueryString["userId"] != "")
        //    this.UserId = this.Request.QueryString["userId"];
        //DirectoryEntry de = this.GetDirectoryEntry(this.UserId);
        //this.UserName = de.Properties["DisplayName"][0].ToString();
        //return this.UserId;
        return UtilitySystem.CurrentUserPassport;
    }
    [WebMethod]
    public static string GetUserStatic() {
        Base class1 = new Base();
        return class1.GetUser(); 
    }
    public string GetUser() {
        try {
            //if (!String.IsNullOrEmpty(Page.User.Identity.Name)) {
            //    this.UserId = Page.User.Identity.Name.ToString().Split(delimiterChars)[1];
            //} else {
            //    this.UserId = System.Web.HttpContext.Current.Request.LogonUserIdentity.Name.Split(delimiterChars)[1];
            //}           
            //Base class1 = new Base(); 
            //DirectoryEntry de = class1.GetDirectoryEntry(this.UserId);
            //this.UserName = de.Properties["DisplayName"][0].ToString();

            string email = "";
            string position = "";
            //try {
            //    email = de.Properties["mail"][0].ToString();
            //    position = de.Properties["title"][0].ToString();
            //} catch (Exception e ) {
            //    Base.Log(e.Message.ToString());
            //}
            string[] arr = { this.UserId, this.UserName, email, position };
            return string.Join(":", arr);
        }catch (Exception e) {
            Base.Log(e.Message.ToString()); 
        }
        return null; 
    }
    [WebMethod]
    public static string Authenticate(string userId, string password, int thirdParty, string password1 = null) {
        Helper.WriteCookie(userId);

        Base c = new Base();
        SortedList sl = new SortedList();
        string hash = Helper.GetHash(password);
        sl.Add("userId", userId);
        sl.Add("hash", hash);
        if (password1 != null)
            sl.Add("hash1", Helper.GetHash(password1));
        DataSet ds = c.da.GetDataSet("usp_CheckHash", sl);
        if (ds.Tables[0].Rows.Count > 0) {
            string s = ds.Tables[0].Rows[0][0].ToString();
            HttpContext.Current.Session["UserId"] = s.Split(':')[0];
            return s;
        }
        using (PrincipalContext pc = new PrincipalContext(ContextType.Domain)) { //, "ensco.ws", this.UserId, this.PWD)) { //, "dc=company,dc=com")) {
            if (pc.ValidateCredentials(userId, password)) {
                Base class1 = new Base();
                DirectoryEntry de = class1.GetDirectoryEntry(userId);
                string userName = de.Properties["DisplayName"][0].ToString();
                userName = userName.Replace("'", "");
                Base.Log(userName);

                HttpContext.Current.Session["UserId"] = userId;
                return userId + ":" + userName;

            }
        }
        return "";
    }
    [WebMethod]
    public static string Authenticate022418(string userId, string password, int thirdParty, string password1 = null) {
        if (thirdParty == 1) {
            Helper.WriteCookie(userId);

            Base c = new Base();
            SortedList sl = new SortedList();
            string hash = Helper.GetHash(password);
            sl.Add("userId", userId);
            sl.Add("hash", hash);
            if (password1 != null)
                sl.Add("hash1", Helper.GetHash(password1));
            DataSet ds = c.da.GetDataSet("usp_CheckHash", sl);
            if (ds.Tables[0].Rows.Count > 0) {
                string s = ds.Tables[0].Rows[0][0].ToString();
                HttpContext.Current.Session["UserId"] = s.Split(':')[0]; 
                return s;
            } else
                return "";

        } else {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain)) { //, "ensco.ws", this.UserId, this.PWD)) { //, "dc=company,dc=com")) {
                if (pc.ValidateCredentials(userId, password)) {
                    Base class1 = new Base();
                    DirectoryEntry de = class1.GetDirectoryEntry(userId);
                    string userName = de.Properties["DisplayName"][0].ToString();
                    userName = userName.Replace("'", "");
                    Base.Log(userName);
                    
                    HttpContext.Current.Session["UserId"] = userId;                                         
                    return userId + ":" + userName;

                } else
                    return "";
            }
        }
        return "";
    }
    [WebMethod]
    public static void ChangePassword(string email) {
        Base c = new Base();
        SortedList sl = new SortedList();
        string password = "123";
        string hash = Helper.GetHash(password); 
        sl.Add("email", email);
        sl.Add("password", password);
        sl.Add("hash", hash);
        c.da.GetDataSet("usp_ChangePassword", sl); 
    }
    [WebMethod]
    public  static string  CheckPdf(string id ) {
        Base c = new Base();
        SortedList sl = new SortedList();
        sl.Add("id", id );
        DataSet ds =c.da.GetDataSet("usp_JobGetWiPdf", sl);
        string path = "";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            path = ds.Tables[0].Rows[0][0].ToString();
        try {
            //using (new Impersonator ("011311", "ensco", "Yibing77!")) {
                if (path != "" && File.Exists(path))
                    return path;
                else
                    return "";
            //}
        }catch(Exception e) {
            c.da.GetDataSet("insert tbl_error select getdate(), '" + e.Message.ToString() + "'"); 
        }
        return ""; 
    }
    public DataSet GetData(string sp, string[] arr) {
        return this.da.GetDataSet(sp + " "+ string.Join(" , ", arr));         
    }
    static void Log(string s) {
        Base c = new Base();
        s = s.Replace("'", "''"); 
        
        c.da.GetDataSet(" insert tbl_error select getdate(), '" + s + "'"); 
    }
}

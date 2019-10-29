using System;
using System.IO;
using System.Collections;

using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Text.RegularExpressions; 
public partial class Excel : Base {
    string id;
    protected void Page_Load(object sender, EventArgs e) {

        this.MyInit(); 
    }
    void Post() {
    }
    void SendEmail(string sql) {
        SortedList  sl = new SortedList();
        sl.Add("email", "fwang@enscoplc.com");
        sl.Add("title", "IRMA error");
        sl.Add("s", sql);
        sl.Add("cc", "");
        da.GetDataSet("usp_sendEmail", sl);
    }
    void MyInit(bool export =false ) {
        string sp=null;
        List<string> list = new List<string>() ; 

        for (int i =0;i<this.Request.Form.Keys.Count; i++) {
            string value = Request.Form["n"+i.ToString() ];
            if (value == null)
                continue; 
            if (value.ToLower().Contains("usp_")) 
                sp = value;
            else {
                string value1 = "'" + value + "'";
                if (value == "")
                    value1 = "null";
                list.Add(value1);
            }
        }
        sp += " " + string.Join(", ", list);
        //try {
            ds = da.GetDataSet(sp);
        //}catch(Exception e) {
        //    this.Response.Write(sp + "<br>" + e.Message.ToString());
        //    return; 
        //}
        GridView gv = new GridView();
        gv.CssClass.Insert(0, "header");
        gv.Width = Unit.Percentage(100);
        gv.ShowHeaderWhenEmpty = true; 
        gv.HeaderStyle.ForeColor = Color.White;
        gv.HeaderStyle.BackColor = Color.FromArgb(0, 70, 127);
        DataTable dt = ds.Tables[0]; 
        foreach(DataColumn dc in dt.Columns) {
            if (dt.Rows.Count == 0)
                continue; 
            string text = dt.Rows[0][dc].ToString();
          
            if (text.StartsWith("<a ")) {
                foreach (DataRow dr in dt.Rows) {
                    text = dr[dc].ToString();
                    text = text.Replace("</a>", "");
                    string[] arr = text.Split('>');
                    if (arr.Length > 1)
                        text = arr[arr.Length - 1];
                    dr[dc] = text; 
                }
            }
            if (text.Contains("<span ")) {
                foreach (DataRow dr in dt.Rows) {
                    text = dr[dc].ToString();
                    dr[dc] = this.GetTextFromHtml(text); 
                }
            }

        }
        dt.AcceptChanges(); 
        gv.DataSource = dt; 
        gv.DataBind();

            this.Export(gv, this.Response);
    }
    string GetTextFromHtml(string s) {
       s = Regex.Replace(s, "<[^>]*(>|$)", "");
        s = s.Replace("&#252;", "Yes");
        return s; 
    }
    void Export(GridView gv, HttpResponse response) {
        System.IO.StringWriter writer = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter html = new HtmlTextWriter(writer);

        gv.RenderControl(html);

        response.Clear();
        response.Buffer = true;
        string page = this.Request.QueryString["page"] + DateTime.Now.ToString();
        response.AddHeader("Content-Disposition", "attachment; filename=" + page +".xls");
        response.ContentType = "application/vnd.ms-excel";
       // string s = HttpUtility.HtmlDecode(writer.ToString());
        response.Write( writer);
        Response.Flush();
        Response.End();
    }
}

using System;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
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
public partial class Summary : Base {
    string id;
    bool TableStart;
    Helper Helper1 = new Helper(); 
    protected void Page_Load(object sender, EventArgs e) {

        DateTime dt = new DateTime(2018, 4, 17);
        this.Response.Write(dt.ToShortDateString());
        return; 

        if (!this.IsPostBack)
            this.MyInit();

    }
    void Post() {
        this.MyInit(true); 
    }
    void MyInit(bool export =false ) {
        this.Helper1.Log("starte"); 
        StringBuilder sb = new StringBuilder();
        SortedList sl = new SortedList();
        sl.Add("date",DBNull.Value  );
        sl.Add("lang", "en"); 
        ds = da.GetDataSet("usp_JobSummary", sl );
        string[] arr = { "Permited Jobs", "Non-Permited Jobs", "New CAPA", "Open Short Term Isolations", "Long Term Isolations"
           , "Onboarding", "Offboarding", "", "", "", "", "", "" };
        int i = 0;
        sb.Append(@" <style> 
            table { border-collapse: collapse; width:100%;
            } 
            .darkblue {
                background-color: rgb(0, 70, 127);
                border: solid;
                border-width: 1px;
                border-color: rgb(0, 70, 127);
                padding-right: 4px;
                color: white;
                text-align: left;
            }
            .header {
                background-color: rgb( 232, 109, 31);
                border: solid;
                border-width: 1px;
                border-color: rgb( 232, 109, 31);
                padding-right: 4px;
                color: white;
                text-align: left;
            }
            </style>");
        foreach (DataTable dt in ds.Tables) {
            this.TableStart = true; 
            GridView gv = new GridView();
            gv.CssClass.Insert(0, "header");
            gv.Width = Unit.Percentage(100);
            gv.ShowHeaderWhenEmpty = true;
            gv.HeaderStyle.BackColor = Color.LightGray;// .FromArgb(0, 70, 127);
            gv.DataSource = dt; 
            gv.DataBind();
          //  this.form1.Controls.Add(gv);
            System.IO.StringWriter writer = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter html = new HtmlTextWriter(writer);

            gv.RenderControl(html);
            if (i == 0)
                sb.Append(this.AddHeader("header", "Process"));
            if (i == 3)
                sb.Append(this.AddHeader("header", "Plant"));
            if (i == 5)
                sb.Append(this.AddHeader("header", "People")) ;
            sb.Append( this.AddHeader( "darkblue", arr[i++] ) );
            sb.Append(HttpUtility.HtmlDecode(writer.ToString()) );
        }
       // this.Response.Write(sb.ToString()); return; 
        sl = new SortedList();
        sl.Add("s", sb.ToString()); 
        da.GetDataSet("usp_JobSummaryEmail", sl ); 

    }
    [WebMethod]
    string  GetSummary() {
    
        this.Helper1.Log("starte");
        StringBuilder sb = new StringBuilder();
        SortedList sl = new SortedList();
        sl.Add("date", DBNull.Value);

        ds = da.GetDataSet("usp_JobSummary", sl);
        string[] arr = { "Permited Jobs", "Non-Permited Jobs", "New CAPA", "Open Short Term Isolations", "Long Term Isolations"
           , "Onboarding", "Offboarding", "", "", "", "", "", "" };
        int i = 0;
        sb.Append(@" <style> 
            table { border-collapse: collapse; width:100%;
            } 
            .darkblue {
                background-color: rgb(0, 70, 127);
                border: solid;
                border-width: 1px;
                border-color: rgb(0, 70, 127);
                padding-right: 4px;
                color: white;
                text-align: left;
            }
            .header {
                background-color: rgb( 232, 109, 31);
                border: solid;
                border-width: 1px;
                border-color: rgb( 232, 109, 31);
                padding-right: 4px;
                color: white;
                text-align: left;
            }
            </style>");
        foreach (DataTable dt in ds.Tables) {
            this.TableStart = true;
            GridView gv = new GridView();
            gv.CssClass.Insert(0, "header");
            gv.Width = Unit.Percentage(100);
            gv.ShowHeaderWhenEmpty = true;
            gv.HeaderStyle.BackColor = Color.LightGray;// .FromArgb(0, 70, 127);
            gv.DataSource = dt;
            gv.DataBind();
            //  this.form1.Controls.Add(gv);
            System.IO.StringWriter writer = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter html = new HtmlTextWriter(writer);

            gv.RenderControl(html);
            if (i == 0)
                sb.Append(this.AddHeader("header", "Process"));
            if (i == 3)
                sb.Append(this.AddHeader("header", "Plant"));
            if (i == 5)
                sb.Append(this.AddHeader("header", "People"));
            sb.Append(this.AddHeader("darkblue", arr[i++]));
            sb.Append(HttpUtility.HtmlDecode(writer.ToString()));
        }
        return sb.ToString(); 
    }
    string AddHeader(string c, string name) {
        string s = ""; 
        if (this.TableStart) {
            s = "<br/>";
            this.TableStart = false; 
        }
        return  s + "<table width=100%  cellspacing=0  ><tr><td class="+c+" >" + name + "</table>";
    }
}

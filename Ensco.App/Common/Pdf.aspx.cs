
using System;
using System.IO;
using System.Collections;
using System.Xml.Linq;
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
public partial class Pdf : Base
{
    public string FileName = "";
    protected void Page_Load(object sender, EventArgs e) {
        string page = this.Request.QueryString["page"].ToLower(); ;
        string id = this.Request.QueryString["id"];
        string print = this.Request.QueryString["print"];
        string lang = this.Request.QueryString["lang"];
        if(lang == null)
            lang = "EN";
        PdfAndWord common = new PdfAndWord();
        switch(page) {
            case "capa":
                common = new PdfCapa();
                break;
            case "ba":
                common = new PdfBa();
                break;
            case "permit":
                common = new PdfPermit();
                break;
            case "gas":
                common = new PdfGas();
                break;
            case "confined":
                common = new PdfCofined();
                break;
            case "lift":
                common = new PdfLift();
                break;
            case "jsa":
                common = new PdfJsa();
                break;
            case "ei":
                common = new PdfEi();
                break;
            case "hotwork":
                common = new PdfHotwork();
                break;
            case "crane":
                common = new PdfCrane();
                break;
            case "riding":
                common = new PdfRiding();
                break;
            case "wi":
                common = new PdfWi();
                break;
            case "fullroster":
                common = new PdfFullRoster();
                break;
            case "swa":
                common = new PdfSwa();
                break;
            case "capaplan":
                common = new PdfCapaPlan();
                break;
            case "oap":
                common = new PdfOap();
                break;
        }
        SortedList sl = new SortedList();
        sl.Add("id", id);
        sl.Add("lang", lang);
        //sl.Add("UserId", this.UserId); 
        //ds = this.da.GetDataSet("usp_PdfGet"+name, sl );
        ds = this.da.GetDataSet("usp_Pdf" + page, sl);
        common.ds = ds;
        if(lang != "EN")
            common.dsLang = this.da.GetDataSet(" select Name, " + lang + " as Name1 from tbl_Label where page like '%" + page + "%'");
        common.BaseUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority;
        common.Lang = lang;
        if(page == "permit")
            common.Title = this.ds.Tables[2].Rows[0]["Type"].ToString();
        string userName = Ensco.Utilities.UtilitySystem.CurrentUser.UserName;
        //common.Start(this.Response, userName);
        //return; 
        //if(print == "1") {
        //    this.FileName = common.Start(null, userName);
        //} else
        //    common.Start(this.Response, userName);
    }
}

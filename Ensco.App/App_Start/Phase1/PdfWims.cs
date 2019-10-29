using System;
using mUtilities.Data;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.collection;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;

using iTextSharp.tool.xml;
using iTextSharp.tool.xml.css;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
//using XMLWorkerRichText;

public class PdfWi : PdfAndWord {
    protected override void CreateTable(Document document) {
        this.CreatePlanning();
        this.CreateEquipment(1);
        this.CreateJobStep();
         return; 
        this.CreateVerification();
        this.AddCommon();
    }
    void CreatePlanning() {
        float[] widths = this.GetWidths(8);
        DataRow dr = this.ds.Tables[0].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeader(t, "Planning", 8);
        this.AddLabel(t, "WI ID");
        this.AddText(t, dr["WiNo"]);
        this.AddLabel(t, "WI Job Title");
        this.AddText(t, dr["JobTitle"], 3);   
        this.AddLabel(t, "Status");
        this.AddText(t, dr["Status"]);

        this.AddLabel(t, "Job ID");
        this.AddText(t, dr["Jobid"]);
        this.AddLabel(t, "WI Equipment Type");
        this.AddText(t, dr["EquipmentType"], 3);
        this.AddLabel(t, "Date/Time ");
        this.AddText(t, dr["dt1"]);

        this.AddLabel(t, "JSA ID ");
        this.AddText(t, dr["Jobid"]);
        this.AddLabel(t, "Job Category");
        this.AddText(t, dr["JobCategory"]);
        this.AddLabel(t, "WI Type  ");
        this.AddText(t, dr["dt1"]);
        this.AddLabel(t, "Job Criticality");
        this.AddText(t, dr["Criticality"]);

        this.PdfDoc.Add(t);
    }
    void CreateJobStep() {
        DataTable dt = this.ds.Tables[2];
        PdfPTable t = this.GetTable(dt);
        this.AddSubHeader(t, "Steps", dt.Columns.Count);
        foreach (DataRow dr in dt.Rows) {
            dr["Description"] = this.HtmlDecodeRichText(dr["Description"]); 
        }
        this.AddTable(t, dt);

        this.PdfDoc.Add(t);
    }
   
    public void AddTable(PdfPTable t, DataTable dt) {
        foreach (DataColumn dc in dt.Columns) {
            this.AddLabel(t, dc.ColumnName, null, null, PdfPCell.ALIGN_CENTER);
        }
        string[] arr = { "Description", "Warning", "Caution", "Note" }; 
        foreach (DataRow dr in dt.Rows) {
            foreach (DataColumn dc in dt.Columns) {
                string text = dr[dc.ColumnName].ToString().Trim() ;
                if (arr.Contains(dc.ColumnName) && text != "")
                    t.AddCell(this.GetRichCell(text));
                else if (dc.ColumnName == "Photo" && text !="")
                    this.CreateImageFromUrl(t, text); 
                else
                    this.AddText(t, text);
            }
        }
    }

    void CreateVerification() {
        DataTable dt = this.ds.Tables[3];
        PdfPTable t = this.GetTable(dt);
        this.AddHeader(t, "Verification", dt.Columns.Count);

        this.AddTable(t, dt);

        this.PdfDoc.Add(t);
    }
    

}

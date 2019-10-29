using System;
using mUtilities.Data;

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
public class PdfEi : PdfAndWord {
    protected override void CreateTable(Document document) {
        this.CreatePlanning();
      //  this.CreateAuthorization();
        this.CreateExecution();
      //  this.CreateVerification(); 
    }
    void CreatePlanning() {
        float[] widths = this.GetWidths(8);

        DataRow dr = this.ds.Tables[0].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeader(t, "Planning", 8 );
        this.AddLabel(t, "Certificate No. ");
            this.AddText(t, dr["id"]);
        this.AddLabel(t, "Job");
        this.AddText(t, dr["JobId"]);
        this.AddLabel(t, "Permit");
        this.AddText(t, dr["permitId"]);
        this.AddLabel(t, "Status");
        this.AddText(t, dr["Status"]);

        this.AddLabel(t, "EI Title");
        this.AddText(t, dr["Title"], 7);
        //this.AddLabel(t, "Isolated Method");
        //this.AddText(t, dr["IsolationMethod"], 7);
        //this.AddLabel(t, "System requires to be re-energized for testing purposes? ", null, null, 2, 3 );
        //this.AddBool(t, dr["Energized"], 7);

        this.PdfDoc.Add(t);
    }
    void CreateAuthorization() {
        DataTable dt = this.ds.Tables[1];
        PdfPTable t = this.GetTable(dt);
        this.AddHeader(t, "Authorization", dt.Columns.Count);

        this.AddTable(t, dt);

        this.PdfDoc.Add(t);
    }
    void CreateExecution() {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("tPlant", "Plant – Equipment to be Isolated – Isolation Authority to complete in order of isolation");
        dict.Add("tPI", "Permit Issuer authorizes isolations  ");
        dict.Add("tIA", "Isolation Verification – Permit Holder and Permit Authority must verify isolations");

        dict.Add("tPA", "Isolation Verification – Permit Holder and Permit Authority must verify isolations");
        dict.Add("tShortTerm", "Equipment managed in short term, long term or temporarily de-isolated status ");

        dict.Add("tLongTerm", "");
        dict.Add("tSuspend", "");
        dict.Add("tDePI", "De-Isolation Authorization");
        dict.Add("tDeIA", "Isolation Authority Verification (to be completed after all isolations completed)");
        dict.Add("tDePA", "");
        dict.Add("tDeIsolated", "DeIsolated");
        string type = "";
        DataTable dt = this.ds.Tables[2];
        float[] widths = this.GetWidths(2);
        widths[1] = 30;
        float ratio = 1-0.00f - widths[0] * 1.0f / widths[1]; 
        int count = dt.Columns.Count; 
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeader(t, "Execution", count );
        int i = 1; 
        foreach (DataRow dr in dt.Rows) {
            string type1 = dr["type"].ToString();
            if (type1 != type) {
                type = type1;
                string name = type;
                if (dict.Keys.Contains(type))
                    name = dict[type]; 
                this.AddSubHeader(t, name, count );
                i = 1; 
            }
            this.AddText(t, (i++).ToString(), 1, 1 );
            this.CreateSubTable(t, dt, dr , ratio);
         //   this.CreateSubTable(t, dt, dr, ratio,  8);
        }

        this.PdfDoc.Add(t);
    }
    void CreateSubTable(PdfPTable t,DataTable dt, DataRow dr , float ratio,  int index = 0) {
        PdfPCell cell = new PdfPCell();
        int count = dt.Columns.Count-1; 
        float[] widths1 = this.GetWidths(count );
        PdfPTable t1 = this.InitTableProperties(widths1, ratio);
        for (int i = 0; i < count ; i++) {
            string name = dt.Columns[i+index].ColumnName;
            this.AddLabel(t1, name, null, null, PdfPCell.ALIGN_CENTER);
        }
        for (int i = 0; i < count ; i++) {
            string name = dt.Columns[i + index].ColumnName;
            string value = dr[name].ToString();
            this.AddText(t1, value);
        }
        t1.SpacingBefore = 0;
        cell.Padding = 0;
        //cell.BorderWidthRight = 0;
        //cell.BorderColorRight = BaseColor.WHITE;
        cell.Border = 0; 
        cell.AddElement(t1); 
        t.AddCell(cell);
    }
    void CreateVerification() {
        DataTable dt = this.ds.Tables[3];
        PdfPTable t = this.GetTable(dt);
        this.AddHeader(t, "Verification", dt.Columns.Count);

        this.AddTable(t, dt); 

        this.PdfDoc.Add(t);
    }

}


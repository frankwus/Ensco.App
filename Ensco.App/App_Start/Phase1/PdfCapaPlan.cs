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
public class PdfCapaPlan: PdfAndWord {
    protected override void CreateTable(Document document) {
        this.CreatePlanning();
        this.CreateSummary();   
        this.CreateExecution();
        this.CreateVerification(3); 
    }
    void CreatePlanning() {

        float[] widths = this.GetWidths(6);
        //widths[0] = 4;
        DataRow dr = this.ds.Tables[0].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeader(t, "Planning", 10);
        this.AddLabel(t, "Action Plan ID");
        this.AddText(t, dr["id"]);
        this.AddLabel(t, "Date / Time Created");
        this.AddText(t, dr["Date"]);
        this.AddLabel(t, "Status");
        this.AddText(t, dr["Status"]);

        this.AddLabel(t, "Plan Title");
        this.AddText(t, dr["Title"], 5);

        this.AddLabel(t, "Action Manager(s)");
        this.AddText(t, dr["Action Manager"], 5 );
        this.AddLabel(t, "General Description");
        this.AddText(t, dr["Description"], 5);
        this.PdfDoc.Add(t);
    }
    void CreateSummary() {
        DataTable dt = this.ds.Tables[1];
        PdfPTable t = this.GetTable(dt);
        this.AddSubHeader(t, "Summary", dt.Columns.Count);

        this.AddTable(t, dt);

        this.PdfDoc.Add(t);
    }
    void CreateExecution() {
        DataTable dt = this.ds.Tables[2];
        PdfPTable t = this.GetTable(dt);
        this.AddSubHeader(t, "Execution", dt.Columns.Count);

        this.AddTable(t, dt);

        this.PdfDoc.Add(t);
    }
}
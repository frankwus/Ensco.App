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
public class PdfOap  : PdfAndWord
{
    protected override void CreateTable(Document document) {
        this.CreatePlanning();
        this.CreateGrid();
    }
    void CreatePlanning() {
        float[] widths = this.GetWidths(8);
        DataRow dr = this.ds.Tables[0].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeader(t, "Planning", 10);
        this.AddLabelText(t, dr, "ID");
        this.AddLabelText(t, dr, "Location");
        this.AddLabelText(t, dr, "AssessmentDate"); 
        this.AddLabelText(t, dr, "Status");

        this.AddLabelText(t, dr, "Title", 7);
        this.AddLabelText(t, dr, "Description", 7);
        this.AddLabelText(t, dr, "Job", 7);

        this.PdfDoc.Add(t);
    }
    void CreateGrid() {
        string[] arr = new string[] { "Assessor", "Checklist", "Verification" , "Comments", "lession learned", "Review", "Findings", "CAPA" };
        for( int i=1; i<ds.Tables.Count; i++) {
            DataTable dt = ds.Tables[i]; 
            string name = arr.Length > i-1 ? arr[i-1] : "";

            PdfPTable t = this.GetTable(dt);
            this.AddSubHeader(t, name, dt.Columns.Count);

            this.AddTable(t, dt);

            this.PdfDoc.Add(t);
        }
    }
}
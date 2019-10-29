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
public class PdfFullRoster : PdfAndWord {
    protected override void CreateTable(Document document) {
        this.CreatePlanning();      
         this.CreateExecution();       
        this.CreateVerification();
    }
    void CreatePlanning() { 
        float[] widths = this.GetWidths(8 );
       // widths[0] = 2;
        DataRow dr = this.ds.Tables[2].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeader(t, "Details", widths.Length );
        this.AddPair(t, "Rig", dr[0].ToString() );
        this.AddPair(t, "Date", "");
        this.AddPair(t, "Time", "");
        this.AddPair(t, "Part of Drill?", "");

       this.AddPair(t, "\n\n Purpose \n\n", "", 7 );

        this.PdfDoc.Add(t);
    }
    void CreateExecution() {
        DataTable dt = this.ds.Tables[0];
        float[] widths = this.GetWidths(dt.Columns.Count);
        widths[0] = 0.2f;
        //widths[5]
        PdfPTable t = this.InitTableProperties(widths);

        this.AddSubHeader(t, "People / Participants / Attendees", dt.Columns.Count);
        this.AddTable(t, dt);
        this.PdfDoc.Add(t);
    }
  
    void CreateVerification() {
        DataTable dt = this.ds.Tables[1];
        PdfPTable t = this.GetTable(dt);
        this.AddHeader(t, "Verification", dt.Columns.Count);

        this.AddTable(t, dt);

        this.PdfDoc.Add(t);
    }
}
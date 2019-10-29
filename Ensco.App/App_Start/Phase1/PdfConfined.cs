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
public class PdfCofined : PdfAndWord {
    protected override void CreateTable(Document document) {
        this.CreatePlanning();

        this.CreateExecution();
       // this.CreateVerification(3);
    }
    void CreatePlanning() {
        float[] widths = this.GetWidths(12);

        DataRow dr = this.ds.Tables[0].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeader(t, "Planning", 12);
        this.AddPair (t, "Entry Log No. . ", dr["id"] );
        this.AddPair(t, "Job", dr["JobId"]);
        this.AddPair(t, "Permit", dr["PermitId"]);
        this.AddPair(t, "Date/Time Performed ", dr["DateDone"]);

        this.AddPair(t, "Date/Time Created ", dr["Date"]);
        this.AddPair(t, "Status", dr["Status"]);

        this.AddLabel(t, "Gas Test Certificate ");
        this.AddText(t, dr["GasId"], 11);         
        this.PdfDoc.Add(t);
    }
    void CreateExecution() {
        this.DateBind(ds.Tables[1], "Execution", "Entrant(s)");
        this.DateBind(ds.Tables[2], "Verification", "Stand By Person(s) ");
    }
  
}


using System;
using mUtilities.Data;

using System.Xml;
using System.IO;
using System.Collections;
using System.Xml.Linq;
using System.Collections.Generic;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.collection;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
public class PdfCav : PdfAndWord {
    protected override  void CreateTable(Document document) {
        this.CreatePlanning();
        this.CreateExecution();
        this.CreateVerification(); 
    } 
    void CreatePlanning() {
        int colSpan = 8; 
        PdfPTable t= base.CreatePlanning(colSpan);
        this.AddImageFromUrl(t, this.BaseUrl + "/images/warning.png");
        this.AddText(t, " Exercise Stop Work Authority (SWA) to decline or stop work when you perceive a threat of danger to people, environment or asset.",  colSpan-1);
        t.SpacingAfter = 8;
        this.PdfDoc.Add(t);
    }
    void CreateVerification() {
        DataTable dt = this.ds.Tables[2];
        float[] widths = new float[dt.Columns.Count];
        for (int i = 0; i < widths.Length; i++)
            widths[i] = 1;
        PdfPTable t = this.InitTableProperties(widths);

        this.AddHeader(t, "Verification", widths.Length );
        foreach (DataColumn dc in dt.Columns) {
            this.AddLabel(t, dc.ColumnName, BaseColor.LIGHT_GRAY, null, PdfPCell.ALIGN_CENTER);
        }
        this.AddTable(t, ds.Tables[2]);
        this.AddLabel(t, "Observations & Comments ",  BaseColor.LIGHT_GRAY, null, PdfPCell.ALIGN_LEFT, widths.Length);
        this.AddTable(t, ds.Tables[3]);
        t.SpacingAfter = 8;
        this.PdfDoc.Add(t);
    }
    void AddTable(PdfPTable t, DataTable dt) {
        foreach (DataRow dr in dt.Rows) {
            foreach (DataColumn dc in dt.Columns) {
                this.AddText(t, dr[dc.ColumnName].ToString());
            }
        }
    }
    void CreateExecution() {
        this.XmlDoc = XDocument.Parse("<r>"+ this.GetXml(1) +"</r>");
        float[] widths = new float[] { 1, 10, 15 };
        PdfPTable S = this.InitTableProperties(widths);
        this.AddHeader(S, "Execution", 3); 
        this.AddText(S, "Category", 2, 1, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_MIDDLE, this.DeepBlue,  0);
        this.AddText(S, "Verification", 1, 1, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_MIDDLE, this.DeepBlue, 0);
        int i = 1; 
        foreach (XElement s in this.XmlDoc.Descendants("s")) {
            this.AddLabel(S, s.Attribute("Section").Value, BaseColor.LIGHT_GRAY, null, PdfPCell.ALIGN_LEFT, 3 ) ;

            PdfPCell cell = new PdfPCell();
            cell.Border = 0;
 
            foreach (XElement t in s.Descendants("t")) {
                this.AddText(S, (i++).ToString());
                this.AddText(S, t.Attribute("Topic").Value);

                float[] widthsQ = new float[] { 10, 1 };
                PdfPTable Q = this.InitTableProperties(widthsQ, this.GetRatio(widths));
                Q.WidthPercentage = 100;
                foreach (XElement q in t.Descendants("q")) {
                    this.AddText(Q,  this.GetElementValue(q, "Question"));
                    this.AddText(Q, this.GetElementValue(q, "Name"));
                    string comment = this.GetElementValue(q, "Comment");
                    if (comment != "")
                        this.AddText(Q, comment, 2); 
                }
                PdfPCell c = new PdfPCell();
                c.Padding = 0; 
                c.AddElement(Q); 
                S.AddCell(c);                 
            }
        }
        S.SpacingAfter = 9; 
        this.PdfDoc.Add(S); 
    } 
}


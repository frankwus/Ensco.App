using System;
using mUtilities.Data;

using System.IO;
using System.Collections;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.collection;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
public class PdfBa : PdfAndWord {
    protected override  void CreateTable(Document document) {
        this.CreatePlanning();
        this.CreateExecution();
        this.CreateVerification(); 
    } 
    void CreatePlanning() {
        int colSpan = 8; 
        PdfPTable t= base.CreatePlanning(colSpan);
        this.AddImageFromUrl(t, this.BaseUrl + "/images/note.png");
        this.AddText(t, "This checklist includes only safety-critical tasks identified as barriers to prevent catastrophic events requiring self-Verification. It does not include all safety-critical tasks or responsibilities.",  colSpan-1);
        t.SpacingAfter = 8;
        this.PdfDoc.Add(t);
    }
    void CreateExecution() {
        this.XmlDoc = XDocument.Parse("<r>"+ this.GetXml(1) +"</r>");
        float[] widths = this.GetWidths(11);
        widths[1] = 10;
        widths[2] = 15;         
        PdfPTable S = this.InitTableProperties(widths);
        this.AddHeader(S, "Execution", 11); 
        this.AddText(S, "Category", 2, 1, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_MIDDLE, this.DeepBlue,  0);
        this.AddText(S, "Verification", 1, 1, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_MIDDLE, this.DeepBlue, 0);
        this.AddText(S, "Day", 1, 1, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_MIDDLE, this.DeepBlue, 0);
        DateTime  dt=DateTime.Now ;
        dt = DateTime.Parse(this.XmlDoc.Descendants("s").First().Attribute("StartDate").Value); 
        for ( int j =0; j< 7; j ++)
            this.AddText(S, dt.AddDays(j).Day.ToString()); 
        int i = 1; 
        foreach (XElement s in this.XmlDoc.Descendants("s")) {            
            this.AddLabel(S, s.Attribute("Section").Value, BaseColor.LIGHT_GRAY, null, PdfPCell.ALIGN_LEFT, 11) ;
            foreach (XElement sub in s.Elements("sub")) {
                this.AddLabel(S, sub.Attribute("Subsection").Value, BaseColor.LIGHT_GRAY, null, PdfPCell.ALIGN_LEFT, 11);

                foreach (XElement t in sub.Elements("t")) {
                    this.AddText(S, (i++).ToString());
                    this.AddText(S, t.Attribute("Topic").Value);

                    float[] widthsQ = this.GetWidths(9);
                    widthsQ[0] = 15; 
                    PdfPTable Q = this.InitTableProperties(widthsQ, 1-11.0f/34);
                    Q.WidthPercentage = 100;
                    foreach (XElement q in t.Elements("q")) {   
                        this.AddText(Q, q.Attribute( "Question").Value , 2      );
                        int days = q.Elements("d").Count()==1? 7:1;

                        foreach (XElement d in q.Elements("d")) {
                            this.AddText(Q, d.Attribute("Name").Value, days );
                        }
                            string comment = this.GetAttr(q, "Comment"); 
                        if (comment != "")
                            this.AddText(Q, comment, 9);
                    }
                    PdfPCell c = new PdfPCell();
                    c.Padding = 0;
                    c.Colspan = 9; 
                    c.AddElement(Q);
                    S.AddCell(c);
                }
            }
        }
        S.SpacingAfter = 9; 
        this.PdfDoc.Add(S); 
    }
    void CreateVerification() {
        DataTable dt = this.ds.Tables[2];
        float[] widths = new float[dt.Columns.Count];
        for (int i = 0; i < widths.Length; i++)
            widths[i] = 1;
        PdfPTable t = this.InitTableProperties(widths);

        this.AddHeader(t, "Verification", widths.Length);
        foreach (DataColumn dc in dt.Columns) {
            this.AddLabel(t, dc.ColumnName, BaseColor.LIGHT_GRAY, null, PdfPCell.ALIGN_CENTER);
        }
        this.AddTable(t, ds.Tables[2]);
        this.AddLabel(t, "Observations & Comments ", BaseColor.LIGHT_GRAY, null, PdfPCell.ALIGN_LEFT, widths.Length);
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
}


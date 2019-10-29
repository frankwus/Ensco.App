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
public class PdfRiding: PdfAndWord {
    protected override void CreateTable(Document document) {
        this.CreatePlanning();
        this.CreateManRiders(); 
        this.CreateExecution();
        //  return; 
        this.CreateVerification();
        this.AddCommon();
    }
    void CreatePlanning() {
        float[] widths = this.GetWidths(8);
        widths[0] = 4;
        DataRow dr = this.ds.Tables[0].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeader(t, "Planning", 8);
        this.AddLabel(t, "Checklist No.");
        this.AddText(t, dr["id"]);
        this.AddLabel(t, "Job");
        this.AddText(t, dr["Jobid"]);
        this.AddLabel(t, "Date/Time ");
        this.AddText(t, dr["dt1"]);
        this.AddLabel(t, "Status");
        this.AddText(t, dr["Status"], 3);

        this.AddLabel(t, "Location");
        this.AddText(t, dr["Location"]);
        this.AddLabel(t, "Winch Operator ");
        this.AddText(t, dr["WinchOperator"], 5);
   
        this.PdfDoc.Add(t);
    }
    void CreateExecution() {
        float[] widths = new float[] { 1, 20, 1 };
        this.XmlDoc = XDocument.Parse("<r>" + this.GetXml(1) + "</r>");
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeader(t, "Execution", 3);
        foreach (XElement s in this.XmlDoc.Descendants("s")) {
            this.AddSubHeader(t, s.Attribute("Section").Value, 3);
            foreach (XElement sub in s.Descendants("sub")) {
                this.AddLabel(t, sub.Attribute("sub").Value, null, null, PdfPCell.ALIGN_LEFT, 3);
                foreach (XElement q in sub.Descendants("q")) {
                    this.AddText(t, q.Attribute("id").Value);
                    string comment = q.Attribute("Comment").Value;
                    if (comment != "")
                        comment = " Comment: " + comment;
                    this.AddText(t, q.Attribute("Q").Value + comment);
                    this.AddText(t, q.Attribute("Answer").Value);
                }
            }
        }
        this.PdfDoc.Add(t);
    }
    void CreateManRiders() {
        DataTable dt = this.ds.Tables[2];
        PdfPTable t = this.GetTable(dt);
        this.AddSubHeader(t, "Man Riders", dt.Columns.Count);

        this.AddTable(t, dt);

        this.PdfDoc.Add(t);
    }
    void CreateVerification() {
        DataTable dt = this.ds.Tables[3];
        PdfPTable t = this.GetTable(dt);
        this.AddHeader(t, "Verification", dt.Columns.Count);

        this.AddTable(t, dt);

        this.PdfDoc.Add(t);
    }
}
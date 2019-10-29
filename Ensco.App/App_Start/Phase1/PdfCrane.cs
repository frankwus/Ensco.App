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
public class PdfCrane : PdfAndWord {
    protected override void CreateTable(Document document) {
        this.CreatePlanning();
       this.CreateEquipment(1 ); 
         this.CreateExecution();
        this.CreateVerification( 3);
        this.AddCommon();
    }
    void CreatePlanning() {
        float[] widths = this.GetWidths(8);
        widths[0] = 2;
        DataRow dr = this.ds.Tables[0].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeader(t, "Planning", 8);
        this.AddPair(t, "Checklist No", dr["id"]);
        this.AddPair(t, "Job", dr["JobId"]);
        this.AddPair(t, "Permit", dr["PermitId"]);
        this.AddPair(t, "Date/Time Performed ", dr["Date1"]);

        this.AddPair(t, "Location", dr);
        this.AddPair(t, "Crane Operator ", dr["CraneOperatorName"]);
        this.AddPair(t, "Status", dr["Status"]);
        this.AddPair(t, "Date/Time Created ", dr["Date1"]);

        this.AddLabel(t, "Description of Personnel Lifting Operation");
        this.AddText(t, dr["Description"], 7 );

        this.PdfDoc.Add(t);
    }
    void CreateExecution() {
        float[] widths = new float[] { 1, 20, 1 };
        this.XmlDoc = XDocument.Parse("<r>" + this.GetXml(2) + "</r>");
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
  
    
}
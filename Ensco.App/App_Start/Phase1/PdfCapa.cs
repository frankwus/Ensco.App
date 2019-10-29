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
public class PdfCapa:  PdfAndWord {
    protected override void CreateTable(Document document) {
        this.CreatePlanning();
        this.CreateImplementation();   
        this.CreateExecution();
    }
    void CreatePlanning() {
        float[] widths = this.GetWidths(6);
        //widths[0] = 4;
        DataRow dr = this.ds.Tables[0].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeader(t, "Create", 10);
        this.AddLabelText(t, dr, "ID");
        this.AddLabelText(t, dr, "Source");
        this.AddLabelText(t, dr, "Status");

        this.AddLabelText(t, dr,  "Criticality");
        this.AddLabelText(t, dr, "ActionType");
        this.AddLabelText(t, dr, "ControlHierarchy");

        this.AddLabelText(t, dr, "DateAssigned");
        this.AddLabelText(t, dr, "DueDate");
        this.AddLabelText(t, dr, "Topic");

        this.AddLabelText(t, dr, "AssignedBy");
        this.AddLabelText(t, dr, "Rig");
        this.AddLabelText(t, dr, "Position");

        this.AddLabelText(t, dr, "AssignedTo", 5 );
        this.AddLabelText(t, dr, "ActionRequired", 5);

        this.PdfDoc.Add(t);
    }
    void CreateImplementation() {
        float[] widths = this.GetWidths(6);
        DataRow dr = this.ds.Tables[0].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeader(t, "Implementation", 10);
        this.AddLabelText(t, dr, "AssignedTo");
        this.AddLabelText(t, dr, "Rig");
        this.AddLabelText(t, dr, "Position");

        this.AddLabelText(t, dr, "DateCompleted");
        this.AddLabelText(t, dr, "WO", 3);
        this.AddLabelText(t, dr, "CompletionDescription", 5);
        this.PdfDoc.Add(t);
    }
    void CreateExecution() {
        string[] arr = new string[] { "Review", "Validate", "Notify" };
        int i = 0; 
        foreach (string name in arr) {
            DataTable dt = this.ds.Tables[1+i];
            PdfPTable t = this.GetTable(dt);
            this.AddSubHeader(t, name , dt.Columns.Count);

            this.AddTable(t, dt);

            this.PdfDoc.Add(t);
            i++; 
        }       
    }
}
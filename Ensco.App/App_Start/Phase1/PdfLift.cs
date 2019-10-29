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
public class PdfLift : PdfAndWord {
    protected override void CreateTable(Document document) {
        this.CreatePlanning();
        this.CreateType();
        this.CreateEnvironmental();
        this.CreatePersonnel(); 
        this.CreateJobPlanning(); 
        this.CreateExecution();
        this.CreateVerification(4); 
    }
    void CreatePlanning() {
        float[] widths = this.GetWidths(11);

        DataRow dr = this.ds.Tables[0].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeader(t, "Planning", 11);
        this.AddLabel(t, "Lift Plan No.");
        this.AddText(t, dr["id"]);
        this.AddLabel(t, "Job No");
        this.AddText(t, dr["jobId"], 3); 
        this.AddLabel(t, "Permit", null, null, 2, 2 );
        this.AddText(t, dr["PermitId"]);

        this.AddLabel(t, "Date / Time");
        this.AddText(t, dr["dt1"]);

        this.AddLabel(t, "Location");
        this.AddText(t, dr["Location"], 5);
        this.AddLabel(t, "Permit Holder ", null, null, 2, 2 );
        this.AddText(t, "Test");
        this.AddLabel(t, "Status");
        this.AddText(t, dr["Status"]);

        this.AddLabel(t, "Description of Lift", null, null,2,  2 );
        this.AddText(t, dr["Description"], 9);

        this.AddLabel(t, "Communication Method ", null, null,2,  2);
        this.AddLabel(t, "Radio");
        this.AddBool(t, dr["Radio"]);
        this.AddLabel(t, "Verbal");
        this.AddBool(t, dr["Verbal"]);
        this.AddLabel(t, "Hand Signals ");
        this.AddBool(t, dr["HandSignals"]);
        this.AddText(t, "", 3);

        this.AddLabel(t, "Potential for conflicting operation (ballast control, other cranes, etc.) ", null, null, 2, 5);
        this.AddBool(t, dr["yes"]);
        this.AddLabel(t, "Specify mitigations ");
        this.AddText(t, dr["mitigations"], 4 );

        this.PdfDoc.Add(t);
    }
    void CreateJobPlanning() {
        float[] widths = this.GetWidths(1);

        DataRow dr = this.ds.Tables[0].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
        this.AddSubHeader(t, "Job Planning", 1);
        this.AddLabel(t, "Lift Sequence: Step-by-Step and Sketch (include material list) " , null, null, PdfPCell.ALIGN_LEFT);
        this.AddText(t, dr["LiftSequence"]);

        this.PdfDoc.Add(t);
    }
    void CreateType() {
        float[] widths = this.GetWidths(14);

        DataRow dr = this.ds.Tables[0].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
        this.AddSubHeader(t, "Type of Lift – Check all that apply ", widths.Length);
        string[] arr = { "Blind" , "Deck", "Marin", "Personnel", "Subsea", "Tandem", "Other" }; 
        foreach (string name in arr) {
            this.AddLabel(t, name);
            this.AddBool(t, dr[name]); 
        }
        this.PdfDoc.Add(t);
    }
    void CreateEnvironmental() {
        float[] widths = this.GetWidths(4);

        DataRow dr = this.ds.Tables[0].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
        this.AddSubHeader(t, "Environmental Conditions  ",  widths.Length);
        string[] arr = { "Wind Speed & Direction", "Sea State", "Sky Conditions (Visibility)", "Comments" };
        foreach (string name in arr) {
            this.AddLabel(t, name, null, null, PdfPCell.ALIGN_CENTER);
        }
        string [] arr1 = { "Wind", "Sea", "Sky", "Comment" };
        foreach (string name in arr1) {
            this.AddText(t, dr[name] );
        }
        this.PdfDoc.Add(t);
    }
    void CreatePersonnel () {
        DataTable dt = this.ds.Tables[2];
        int count = dt.Columns.Count;
        float[] widths = this.GetWidths(count);
        widths[0] = 0.1f; 
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeader(t, "Personnel Required",count);

        this.AddTable(t, dt);

        this.PdfDoc.Add(t);
    }
    void CreateExecution() {
        float[] widths = new float[] { 1, 20, 1 };
        this.XmlDoc = XDocument.Parse("<r>" + this.GetXml(3) + "</r>");
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeader(t, "Execution", 3);
        foreach (XElement s in this.XmlDoc.Descendants("s")) {
            this.AddSubHeader(t, s.Attribute("Section").Value, 3);

                foreach (XElement q in s.Descendants("q")) {
                    this.AddText(t, q.Attribute("id").Value);
                    string comment = q.Attribute("Comment").Value;
                    if (comment != "")
                        comment = " Comment: " + comment;
                    this.AddText(t, q.Attribute("Q").Value + comment);
                    this.AddText(t, q.Attribute("Answer").Value);
                }
        }
        this.PdfDoc.Add(t);
    }
    void CreateExecution091017() {
        this.XmlDoc = XDocument.Parse("<r>" + this.GetXml(3) + "</r>");
        DataTable dt = this.ds.Tables[3]; 
        float[] widths = this.GetWidths(2);
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeader(t, "Execution", widths.Length);
        this.AddSubHeader(t, "Pre-Lift Checks ", widths.Length);

        string[] arr = { "Plant Area", "Plant Equipment", "Process", "People" };
        PdfPCell cell=null;
        PdfPTable t1=null;
        for (int i = 0; i < 4; i++) {
            string name = arr[i];
            if (i % 2 == 0  ) {
                cell = new PdfPCell();
                cell.PaddingLeft = 0; 
                cell.Border = 0; 
                float[] widths1 = this.GetWidths(3);
                widths1[1] = 5; 
                t1 = this.InitTableProperties(widths1, 0.5f);
            }
            this.AddSubHeader(t1, name , 3);

            DataRow[] rows= dt.Select("section='" + name + "'"); 
            foreach (DataRow dr in rows) {
                this.AddText(t1, dr["id"]);
                this.AddText(t1, dr["Q"]);
                this.AddText(t1, dr["Answer"]);
            }
            if (i % 2 == 1  ) {
                cell.AddElement(t1);
                t.AddCell(cell);
            }
        }
        
            this.PdfDoc.Add(t);
    }
}


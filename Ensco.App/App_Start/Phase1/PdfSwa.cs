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
public class PdfSwa: PdfAndWord {
    protected override void CreateTable(Document document) {
        this.CreatePlanning();
        this.CreateObserver();
        
        this.CreateExecution();
        this.CreateCause();
        this.CreateFactor();
        this.CreateCardHazard(); 
        this.CreateVerification();
    }
    void CreatePlanning() {
        float[] widths = this.GetWidths(10);
        //widths[0] = 4;
        DataRow dr = this.ds.Tables[0].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeader(t, "Planning", 10);
        this.AddLabel(t, "Checklist No.");
        this.AddText(t, dr["id"]);
        this.AddLabel(t, "Location");
        this.AddText(t, dr["Location"]);
        this.AddLabel(t, "Date/Time Observed");
        this.AddText(t, dr["dt1"]);
        this.AddLabel(t, "Date/Time Created");
        this.AddText(t, dr["dt"]);
        this.AddLabel(t, "Status");
        this.AddText(t, dr["Status"]);

        this.AddLabel(t, "Drill");
        this.AddText(t, dr["Drill1"]);
        this.AddLabel(t, "Job");
        this.AddText(t, dr["jobId"]);
        this.AddLabel(t, "Job Title");
        this.AddText(t, dr["jobTitle"], 5);
        this.PdfDoc.Add(t);
    }
    void CreateExecution() {
        float[] widths = new float[] { 1, 4, 2, 1 };
        string[] arrHeader = { "1	Stop the Job", "2	Hold a Conversation", "3	Change Work Practice, if Necessary", "4	Resume Work Safely"};
        string[] arr = {"id", "Name", "Question", "Checked" };
        DataRow dr;

        for (int k = 0; k < 4; k++) {
            DataTable dt = this.ds.Tables[3+k];
            PdfPTable t = this.InitTableProperties(widths);
            if (k == 0) {
                this.AddHeader(t, "Execution", dt.Columns.Count);
                this.AddSubSubHeader(t, arrHeader[k], dt.Columns.Count);
                this.AddLabel( t, "Category ", null, null, PdfPCell.ALIGN_CENTER, 2);
                this.AddLabel(t, "Verification", null, null, PdfPCell.ALIGN_CENTER);
                this.AddLabel(t, "Checked ", null, null, PdfPCell.ALIGN_CENTER);
                for (int i = 0; i < 2; i++) {
                    float[] widths1 = new float[] { 1 };
                    dr = dt.Rows[i + 1];

                    for (int j = 0; j < 4; j++) {
                        if (i==1 && j > 1) {
                            PdfPCell c = new PdfPCell();
                            PdfPTable t1 = this.InitTableProperties(widths1, widths[j ] * this.GetRatio(widths));
                            for (int m = 0; m < 4; m++) {
                                this.AddText(t1, dt.Rows[i + m][arr[j]], 1, 1, 0, 5, null, 0);
                            }
                            t1.SpacingBefore = 0; 
                            c.AddElement(t1);
                            t.AddCell(c);
                        }else
                            this.AddText(t, dt.Rows[i][arr[j]]);
                    }
                }
            }else if (k == 2) {
                this.AddSubSubHeader(t, arrHeader[k], dt.Columns.Count);
                for (int i = 0; i < 1; i++) {
                    float[] widths1 = new float[] { 1 };
                    dr = dt.Rows[i + 1];

                    for (int j = 0; j < 4; j++) {
                        if (j !=1 ) {
                            PdfPCell c = new PdfPCell();
                            PdfPTable t1 = this.InitTableProperties(widths1, widths[j] * this.GetRatio(widths));
                            for (int m = 0; m < 2; m++) {
                                this.AddText(t1, dt.Rows[i + m][arr[j]], 1, 1, 0, 5, null, 0 );
                            }
                            t1.SpacingBefore = 0;
                            c.AddElement(t1);
                            t.AddCell(c);
                        } else
                            this.AddText(t, dt.Rows[i][arr[j]]);
                    }
                }
            } else {
                this.AddSubSubHeader(t, arrHeader[k], dt.Columns.Count);
                this.AddTable(t, dt);
            }
            this.PdfDoc.Add(t);
        }
    }
    void CreateCause() {
        DataTable dt = this.ds.Tables[7];
        float[] widths = this.GetWidths(8);
        for (int i = 0; i < 4; i++)
            widths[i * 2] = 8;

        PdfPTable t = this.InitTableProperties(widths);
        this.AddSubHeader(t, "Causes – Why Stop the Job?", widths.Length);
        foreach (DataRow dr in dt.Rows) {
            this.AddLabel(t, dr["name"].ToString());
            this.AddText(t, dr["checked"]);
        }
        this.PdfDoc.Add(t);
    }
    void CreateFactor() {
        DataTable dt = this.ds.Tables[8];
        float[] widths = this.GetWidths(10);
        for (int i = 0; i < 10; i += 2)
            widths[i] = 3;
        PdfPTable t = this.InitTableProperties(widths);
        this.AddSubHeader(t, "Job Factors Applicable to Job – to be completed by the Job Supervisor before commencing the job", 10);
        this.AddText(t, "Job Factors Applicable to Job – to be completed by the Job Supervisor before commencing the job", 10);
        int max = 0;
        for (int i = 1; i < 5; i++) {
            max = Math.Max(dt.Select("seq = " + i.ToString()).Length, max);
        }
        for (int i = 1; i < 5; i++) {
            string name = i.ToString();
            int colSpan = 2;
            if (i == 2)
                colSpan = 4;
            this.AddImageFromUrl(t, this.BaseUrl + "images/jsaFactor/" + name + ".png", colSpan);
        }
        for (int i = 1; i <= max; i++) {
            for (int j = 1; j <= 5; j++) {

                var r = dt.AsEnumerable()
                 .Where(row => row.Field<int>("seq") == j)
                 .OrderBy(row => row.Field<int>("id"))
                 .Take(i);
                string name;
                if (r.Count() < i)
                    name = "";
                else
                    name = r.Last()["name"].ToString(); 
                this.AddLabel(t, name);
                if (name == "")
                    this.AddLabel(t, "");
                else
                    this.AddBool(t, r.Last()["checked"]);

            }
        }
        this.PdfDoc.Add(t);
    }
    void CreateCardHazard( ) {
        this.PdfDoc.NewPage(); 
        DataTable dt = this.ds.Tables[9];
        float[] widths = this.GetWidths(9);
        for (int i = 0; i < 3; i++) {
            widths[3 * i] = 2;
            widths[3 * i + 1] = 9;
        }
        int[ ,  ] arr = { { 0, 3 }, { 1, 4 }, { 2, 4 }, { 9, 4 }, { 13, 5 }, { 14, 3 }, { 21, 3 }, { 23, 4 }, { 28, 4 }, { 30, 3 } };
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeader(t, "", 9);

        for (int i = 0; i<dt.Rows.Count; i++) {
            if (i == 35) {
                this.AddText(t, "Hazards", 3);
            }
            for ( int j=0;j<=arr.GetUpperBound(0) ; j++) {                
                if ( i == arr[j, 0]) {
                    this.AddImageFromUrl(t, this.BaseUrl + "images/jsaHazard/" + (j+1).ToString() + ".png", 1, 1, arr[j, 1] );
                }
            }
            this.AddText(t, dt.Rows[i]["Name"]);
            this.CreateCheckbox(t, dt.Rows[i]["Checked"]);
        }
        this.AddText(t, "", 3);
        this.PdfDoc.Add(t);

        widths = this.GetWidths(2);
        widths[1] = 8; 
        t = this.InitTableProperties(widths);
        this.AddHeader(t, "General Comments", 2);
        this.AddLabel(t, "Comments"); 
        this.AddText(t, this.ds.Tables[0].Rows[0]["comment"]+"\n\n\n\n");
        this.PdfDoc.Add(t);
    }
    void CreateObserver() {
        string[] arr = { "Observers", "Observed Personnel" };
        for (int i = 0; i < 2; i++) {
            DataTable dt = this.ds.Tables[i+1];
            PdfPTable t = this.GetTable(dt);
            this.AddSubHeader(t, arr[i], dt.Columns.Count);

            this.AddTable(t, dt);

            this.PdfDoc.Add(t);
        }
    }
    void CreateVerification() {
        float[] widths = this.GetWidths(4 );
        DataRow dr = this.ds.Tables[0].Rows[0]; 
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeader(t, "Verification", widths.Length);
        this.AddLabel(t, "Role ", null, null, PdfPCell.ALIGN_CENTER);
        this.AddLabel(t, "Name", null, null, PdfPCell.ALIGN_CENTER);
        this.AddLabel(t, "Signature / Review ", null, null, PdfPCell.ALIGN_CENTER);
        this.AddLabel(t, "Date / Time Completed ", null, null, PdfPCell.ALIGN_CENTER);

        this.AddLabel(t, "Lead Observer ");
        this.AddText(t, dr["LeadApprover"]);
        this.AddText(t, dr["LeadStatus"]);
        this.AddText(t, dr["LeadDt"]);

        this.AddLabel(t, "OIM");
        this.AddText(t, dr["OIM"]);
        this.AddText(t, dr["OIMStatus"]);
        this.AddText(t, dr["OIMDt"]);

        this.PdfDoc.Add(t);
    }
}
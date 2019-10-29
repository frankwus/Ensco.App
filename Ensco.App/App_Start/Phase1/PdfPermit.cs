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
public class PdfPermit : PdfAndWord {
    protected override void CreateTable(Document document) {
        this.CreatePlanning();
        this.CreateEquipment();
        this.CreateWims(); 
        this.CreateB();
        this.CreateCDE();
        //this.CreateAuthorization();
        this.CreateExecution();
        this.CreateApproval(); 
        //this.CreateVerification(); 
    }
    void CreatePlanning() {
        float[] widths = this.GetWidths(8);

        DataRow dr = this.ds.Tables[2].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
     //   this.AddPermitColor(t,  dr["type"].ToString()) ; 
        this.AddHeaderBW(t, "A. Permit Request ", widths.Length );
        //this.AddLabel(t, "A. Permit Request (Permit Holder to Complete) ", BaseColor.BLACK, BaseColor.WHITE, 2, widths.Length); 
        this.AddPair(t, "Permit No", dr["id"]);
        this.AddPair(t, "Job No", dr["jobId"]);
        this.AddPair(t, "Location", dr["Location"]);
        this.AddPair(t, "Status", dr["Status"]);

        this.AddPair(t, "Job Title", dr["jobTitle"], 5);
        this.AddPair(t, "Date / Time Created ", dr["Date"]);

        this.AddPair(t, "Job Description", dr["JobDescription"], 7);
        this.AddPair(t, "Additional Documents Reviewed", dr["Document"], 7);

        this.PdfDoc.Add(t);
    }
    void CreateWims() {
        DataTable dt = this.ds.Tables[5];
        int count = dt.Columns.Count;
        float[] widths = this.GetWidths(count);
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeaderBW(t, "Work Instruction", count);

        this.AddTable(t, dt, false);

        this.PdfDoc.Add(t);
    }
    public void AddHeaderBW(PdfPTable t, string name, int colSpan = 1) {
      //  this.AddText(t, name, colSpan); 
      this.AddLabel(t, name,   BaseColor.BLACK, BaseColor.WHITE, PdfPCell.ALIGN_LEFT, colSpan);
    }
    void CreateEquipment() {
        DataTable dt = this.ds.Tables[3];
        dt.Columns["id"].ColumnName = "ID"; 
        dt.Columns.Remove("AdmId"); 
        int i = 1; 
        foreach (DataRow dr in dt.Rows) {
            dr[0] = i++;

        }

        float[]  widths = this.GetWidths(dt.Columns.Count);
        widths[1] = 13;
        PdfPTable  t = this.GetTable(dt, widths);
        this.AddHeaderBW(t, "Equipment", dt.Columns.Count);

        this.AddTable(t, dt);

        this.PdfDoc.Add(t);
    }
    void AddPermitColor (PdfPTable t , string type) {
        BaseColor color = BaseColor.YELLOW;
        if (type == "Hot Work")
            color = BaseColor.RED;
        if (type == "Cold Work")
            color = BaseColor.BLUE; 

            Font font = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL, BaseColor.WHITE);
        Phrase ph = new Phrase(type +" Permit", font);
        PdfPCell cell = new PdfPCell(ph);
        cell.BackgroundColor = color;
        cell.Colspan = 8 ; 
        t.AddCell(cell); 
    }
    void CreateB() {
        float[] widths = this.GetWidths(5);
        widths[0] = 5; 
        DataRow dr = this.ds.Tables[2].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);

        this.AddHeaderBW(t, "B. Period of Validity Note: maximum permit validity is 12 hours ");
        this.AddLabel(t, "From");
        this.AddText(t, dr["FromDate"]);
        this.AddLabel(t, "To");
        this.AddText(t, dr["ToDate"]);
        this.PdfDoc.Add(t);
    }
    void CreateCDE() {
        float[] widths = { 1, 1 };
        PdfPTable t = this.InitTableProperties(widths);
        string[] arr = { "C. Safety Precautions ", "D. Worksite Preparation " }; 
        for (int i = 0; i < 2; i++) {
            PdfPCell cell = new PdfPCell();
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.Border = 0; 
            cell.AddElement(this.CreateCD(i, arr[i])); 
            t.AddCell(cell);
        }

        this.AddHeaderBW(t, "E. Special Precautions ", 2);
        this.AddText(t, this.ds.Tables[2].Rows[0]["ChecklistComment"], 2);
        this.PdfDoc.Add(t); 
    }
    PdfPTable CreateCD(int i, string name ) {
        DataTable dt = this.ds.Tables[i];
        float[] widths = { 1, 30, 2 }; 
        PdfPTable t = this.InitTableProperties(widths, .5f);//  this.GetTable(dt,widths );
        this.AddHeaderBW(t, name , dt.Columns.Count);
        t.SpacingBefore = 0; 
        this.AddTable(t, dt);
        return t; 
    }
    void CreateExecution() {
        float[] widths = this.GetWidths(4);
        for( int i =0; i <widths.Length; i++) {
            widths[i] = (i + 1) % 2 + 1; 
        }
        DataRow dr = this.ds.Tables[2].Rows[0];
       // DataRow dr1 = this.ds.Tables[3].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
        //this.AddHeaderBW(t, "Execution", 10);
        //this.AddHeaderBW(t, "Execution   J. (Permit Holder to Complete) ", 10); 
        this.AddLabel(t, "Work described in section A completed");
        this.AddText(t, dr["WorkCompleted"]);
        //this.AddLabel(t, "Apply for new permit?");
        //this.AddText(t, dr["NewPermit"]);
        this.AddText(t, "", 2); 
        //this.AddLabel(t, "Status");
        //this.AddText(t, dr["NewPermitStatus"]);

        this.PdfDoc.Add(t);
    }
    PdfPTable CreateAuthorization() {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("Permit Holder", "I have completed all the barrier management and actions listed and will comply with all PTW requirements during execution");
        dict.Add("Permit Authority", "I have verified permit detail and the barrier management and actions. I will personally inspect the work area and attend the JSA.");
        dict.Add("Permit Issuer", "I authorize this work under the specified barriers and actions above and pursuant to the Permit Authority personally visiting the work site and attending the JSA.");
        //The Permit to Work is valid only after Permit Issuer approves
        dict.Add("Client", "Client");
        dict.Add("STC", "STC");
        DataTable dt = this.ds.Tables[4];
        PdfPTable t = this.GetTable(dt);
        this.AddHeaderBW(t, "Authorization", dt.Columns.Count);

        this.AddApproverTable(t, dt, dict);

        return t;
    }
    void CreateApproval() {

        DataTable dt = this.ds.Tables[4];
        float[] widths = this.GetWidths(dt.Columns.Count-2);

        PdfPTable t = this.InitTableProperties(widths);
        this.AddPermitApproverTable(t, dt, null); 
        this.PdfDoc.Add(t);
    }
    PdfPTable  CreateVerification() {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("Permit Holder", "I have inspected the worksite and confirm that it is in a safe condition and completed all the work described in this permit.");
        dict.Add("Permit Authority", "I have inspected the worksite and confirm that it is in a safe condition.");
        dict.Add("Permit Issuer", "I have checked the permit details and closed out the permit in accordance with the PTW procedure.");
        //The Permit to Work is cancelled only after section M is complete.
        dict.Add("Client", "Client");
        dict.Add("STC", "STC");
        DataTable dt = this.ds.Tables[5];
        PdfPTable t = this.GetTable(dt);
        this.AddHeaderBW(t, "Verification", dt.Columns.Count);

        this.AddApproverTable(t, dt, dict);

        return t; 
    }
    public void AddPermitApproverTable(PdfPTable t, DataTable dt, Dictionary<string, string> dict) {
        string[] arr = { "Auth", "Veri" };
        this.AddHeaderBW(t, "Authorization/Verification", dt.Columns.Count -2);
        foreach (DataColumn dc in dt.Columns) {
            if (!arr.Contains(dc.ColumnName))
                this.AddLabel(t, dc.ColumnName, null, null, PdfPCell.ALIGN_CENTER);
        }
        foreach (DataRow dr in dt.Rows) {
            for(int i=0; i<arr.Length; i++) { 
                string label = dr[arr[i] ].ToString();
                // string text = dict[role].ToString();
                Font font = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL);
                Phrase ph = new Phrase(label, font);
                PdfPCell cell = new PdfPCell(ph);
                int colSpan = 4;
                if (i > 0)
                    colSpan = dt.Columns.Count - 4 - 2;
                cell.Colspan = colSpan;
               
                t.AddCell(cell);
            }
            foreach (DataColumn dc in dt.Columns) {
                if ( !arr.Contains (dc.ColumnName ) )
                    this.AddText(t, dr[dc.ColumnName].ToString());
            }
        }
    }

}


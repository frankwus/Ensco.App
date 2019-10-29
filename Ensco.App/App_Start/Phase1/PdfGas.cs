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
public class PdfGas : PdfAndWord {
    protected override void CreateTable(Document document) {
        this.CreatePlanning();
        this.CreateWhichGas(); 

        this.CreateAuthorization();
        this.CreateExecution();
        this.CreateInitialTest(); 
//        this.CreateVerification();
    }
    void CreatePlanning() {
        float[] widths = this.GetWidths(8);

        DataRow dr = this.ds.Tables[0].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
        this.AddHeader(t, "Planning", 8 );
        this.AddPair (t, "Certificate No. ", dr["id"] );
        this.AddPair(t, "Job", dr["JobId"]);
        this.AddPair(t, "Permit", dr["PermitId1"]);
        this.AddPair(t, "Status", dr["Status"]);
        this.AddPair(t, "Authorized Gas Tester ", dr["AuthorizedGasTester"]);
        this.AddPair(t, "Location", dr["Location1"]);
        this.AddPair(t, "Date/Time Performed ", dr["DateDone1"]);

        this.AddPair(t, "Date/Time Created ", dr["Date1"]);

        this.AddLabel(t, "Continuous gas monitoring required? ", null, null, 2, 3);
        this.CreateCheckbox(t, dr["ContinuousGas"]);
        this.AddLabel(t, "", null, null, 2, 4); 
        this.AddLabel(t, "Gas detector calibrated and function tested? ", null, null, 2, 3);
        this.CreateCheckbox(t, dr["GasDetector"] );
        this.AddLabel(t, "Has a gas test been done before work start? ", null, null, 2, 3);
        this.CreateCheckbox(t, dr["TestedBefore"]);

        this.PdfDoc.Add(t);
    }
    void CreateWhichGas () {
        float[] widths = this.GetWidths(11);

        DataRow dr = this.ds.Tables[0].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
        this.AddText(t, "Select which gases will be tested: ", 11, 1, 0, 5, LightBlue);
        this.AddLabel(t, "Gas to Test");
        this.AddLabel(t, "O2");
        this.CreateCheckbox(t, dr["O2"]);
        this.AddLabel(t, "H2S ");
        this.CreateCheckbox(t, dr["H2S"]);
        this.AddLabel(t, "Hydrocarbons");
        this.CreateCheckbox(t, dr["Hydrocarbons"]);
        this.AddPair(t, "Toxic", dr);
        this.AddPair(t, "Other", dr);
        this.PdfDoc.Add(t);
    }
    void CreateAuthorization() {
        this.DateBind(ds.Tables[1], "Authorization", "");
    }
    void CreateInitialTest() {
        this.DateBind(ds.Tables[3], "",  "Initial Test");
        this.DateBind(ds.Tables[4],"",  "Subsequent Testing", false);
    }
    void CreateExecution() {
        float[] widths = this.GetWidths(4);
        for (int i = 0; i < widths.Length; i++)
            widths[i] = 1;
        PdfPTable t = this.InitTableProperties(widths);
        DataTable dt = ds.Tables[2];
        this.AddHeader(t, "Execution", 4);
        this.AddSubHeader(t, "Parameters", 4);
        foreach(DataColumn  dc in dt.Columns) 
            this.AddLabel(t, dt.Rows[0][dc].ToString(), null, null, PdfPCell.ALIGN_CENTER);
        foreach (DataColumn dc in dt.Columns)
            this.AddLabel(t, dt.Rows[1][dc].ToString(), null, null, PdfPCell.ALIGN_LEFT);
        for (int i =2; i <dt.Rows.Count; i++) {
            BaseColor[] colors = { BaseColor.WHITE,  BaseColor.GREEN, BaseColor.YELLOW, BaseColor.RED }; 
            for(int j=0;j< dt.Columns.Count; j++)
                this.AddLabel(t, dt.Rows[i][j].ToString(), colors[j],  null, PdfPCell.ALIGN_LEFT);
        }
        this.PdfDoc.Add(t);
    }
    void CreateVerification() {
      
    }  
}


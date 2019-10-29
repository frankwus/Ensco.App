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
public class PdfJsa : PdfAndWord {
    protected override void CreateTable(Document document) {
        this.CreatePlanning();
        this.CreatePersonnel();
        this.CreateDuty();
        this.CreateTask();
        this.CreateEvent();
        this.CreateFactor();
        this.CreateHazard();
        this.CreateHand();
        this.CreateDroppedObject(); 
        this.CreateStep();
        this.CreateChange(); 

        this.CreateVerification(11); 
    }
    void CreatePlanning() {
        float[] widths = this.GetWidths(10);

        DataRow dr = this.ds.Tables[0].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
        string[] arr = { "JSA ID", "Job No", "Permit", "Date", "Status" };
        foreach (string name in arr) {
            this.AddLabel(t, name);
            this.AddText(t, dr[name]); 
        }
        this.AddLabel(t, "Location ");
        this.AddText(t, dr["Location"], 10);

        this.PdfDoc.Add(t);
    }
    void CreatePersonnel() {
        float[] widths = this.GetWidths(5);

        DataRow dr = this.ds.Tables[0].Rows[0];
        PdfPTable t = this.InitTableProperties(widths);
        this.AddSubHeader(t, "Authorization", 5);
        this.AddLabel(t, "Job Supervisor");
        this.AddLabel(t, "Super Position");
        this.AddText(t, dr["supervisorPosition"]);
        this.AddLabel(t, "Supervisor Name");
        this.AddText(t, dr["supervisorName"] );
        this.PdfDoc.Add(t);

        DataTable dt = this.ds.Tables[1];
         widths = this.GetWidths(dt.Columns.Count);

        t = this.GetTable(dt);
        this.AddTable(t, dt);

        this.PdfDoc.Add(t);
    }
    void CreateDuty() {
        DataTable dt = this.ds.Tables[2];
        float [] widths = this.GetWidths(dt.Columns.Count);

         widths[0] = 10;
         PdfPTable t = this.InitTableProperties(widths); // this.GetTable(dt);
        this.AddSubHeader(t, "Job Supervisor Duties", dt.Columns.Count); 
        this.AddTable(t, dt);
        this.PdfDoc.Add(t);
    }
    void CreateTask() {
        DataTable dt = this.ds.Tables[3];
        float[] widths = this.GetWidths(8);
        for (int i = 0; i < 4; i++)
            widths[i*2] = 8; 

        PdfPTable t = this.InitTableProperties(widths);
        this.AddSubHeader(t, "Job Tasks – What am I about to do?", widths.Length );
        foreach (DataRow dr in dt.Rows) {
            this.AddText(t, dr["name"]);
            this.AddText(t, dr["checked"]);
        }
        this.PdfDoc.Add(t);
    }
    void CreateEvent( ) {
        DataTable dt = this.ds.Tables[4];
        float[] widths = this.GetWidths(18);

        PdfPTable t = this.InitTableProperties(widths);
        this.AddSubHeader(t, "Process Safety Events – What are the worst-case scenarios?", 18);
        foreach (DataRow dr in dt.Rows) {
            this.AddImageFromUrl(t, this.BaseUrl+ "images/jsaEvent/"+ dr["name"]+".png");
            this.AddBool(t, dr["checked"]);
        }
        this.PdfDoc.Add(t);
    }
    void CreateFactor() {
        DataTable dt = this.ds.Tables[5];
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
    void CreateFactor2() {
        DataTable dt = this.ds.Tables[5];
        float[] widths = this.GetWidths(10);
        for (int i = 0; i < 10; i += 2)
            widths[i] = 3; 
        PdfPTable t = this.InitTableProperties(widths);
        this.AddSubHeader(t, "Job Factors Applicable to Job – to be completed by the Job Supervisor before commencing the job", 10);
        this.AddText(t, "Job Factors Applicable to Job – to be completed by the Job Supervisor before commencing the job", 10);
        for (int i=1; i<5; i++) {
            string name = i.ToString();
            int colSpan =2;
            if (i == 2)
                colSpan = 4; 
            this.CreateImageFromUrl(t, this.BaseUrl + "images/jsaFactor/" + name+ ".png", colSpan);
        }
        foreach (DataRow dr in dt.Rows) {
            this.AddLabel(t, dr["name"].ToString()); 
            this.AddBool(t, dr["checked"]);
        }
        this.PdfDoc.Add(t);

    }
    void CreateHand() {
        DataTable dt = this.ds.Tables[6];
        int count = 8;
        float[] widths = this.GetWidths(count);
        for (int i1 = 0; i1 <count; i1 += 2)
            widths[i1] = 3;
        PdfPTable t = this.InitTableProperties(widths);
        this.AddSubHeader(t, "Hand Hazards – How can I injure my hands and fingers?", count);

        int i = 0; 
        foreach (DataRow dr in dt.Rows) {
            i++; 
            string name = dr["name"].ToString();
            if (name == "Other") {
                name += ": " + dr["text"];
                this.AddText(t, name, 2 );
            } else {
                this.AddText(t, name);
                this.AddBool(t, dr["checked"]);
            }
            if (i == 13)
                this.AddText(t, "", 6);
            if (i == 16)
                this.AddText(t, "",2);
        }
        this.PdfDoc.Add(t);

    }
    void CreateDroppedObject() {
        DataTable dt = this.ds.Tables[7];
        int count = 8;
        float[] widths = this.GetWidths(count);
        for (int i1 = 0; i1 < count; i1 += 2)
            widths[i1] = 3;
        PdfPTable t = this.InitTableProperties(widths);
        this.AddSubHeader(t, "Dropped Objects – What about potential dropped objects?", count);

        foreach (DataRow dr in dt.Rows) {
            string name = dr["name"].ToString();
                this.AddText(t, name);
                this.AddBool(t, dr["checked"]);
        }
        this.AddText(t, "", 4); 
        this.PdfDoc.Add(t);
    }
    void CreateChange() {
        DataTable dt = this.ds.Tables[10];
        int count = 4;
        float[] widths = this.GetWidths(count);
        for (int i1 = 1; i1 < count + 1; i1 += 2)
            widths[i1] = 4;
        PdfPTable t = this.InitTableProperties(widths);
        this.AddSubHeader(t, "Change Management", count);

        foreach (DataRow dr in dt.Rows) {
            this.AddText(t, dr["id"]);
            this.AddText(t, dr["name"]);
            this.AddBool(t, dr["checked"]);
            this.AddText(t, dr["msg"]);

        }
        this.PdfDoc.Add(t);
    } 
    void ActivityHeader(PdfPTable t, XElement A) {
        this.AddText(t, this.Translate("Job Step "), 1, 1, PdfPCell.ALIGN_LEFT, PdfPCell.ALIGN_MIDDLE, this.Orange, 0);
        this.AddText(t, "");
        this.AddText(t, "");
        
        PdfPCell c =new  PdfPCell(new Phrase (A.Attribute("Text").Value)); 
        c.Colspan = 3;
        c.BorderWidthRight = 1;
        t.AddCell(c);
    }
    void CreateStep() {
        float[] widths = new float[] { 1, 42 };
        this.XmlDoc = XDocument.Parse("<r>"+ this.GetXml(9)+"</r>" );
        PdfPTable A1 = this.InitTableProperties(widths);
      //  this.AddLabel(A1, "Assessment", this.DeepBlue, BaseColor.CYAN, PdfPCell.ALIGN_LEFT, 2);
        foreach (XElement A in this.XmlDoc.Descendants("A")) {
            this.AddText(A1, A.Attribute("id").Value, 1, 1, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_MIDDLE, this.Orange, 0);
            PdfPCell cell = new PdfPCell();
            cell.Border = 0;
            PdfPTable A2 = this.InitTableProperties(new float[] { 10, 3, 1 }, this.GetRatio(widths));
            A2.SpacingBefore = 0; 
            A2.WidthPercentage = 100;
           this.ActivityHeader(A2, A);

            foreach (XElement H in A.Descendants("H")) {
                float[] widthsH1 = new float[] { 1, 42 };
                PdfPTable H1 = this.InitTableProperties(widthsH1, this.GetRatio(widths));
                H1.SpacingBefore = 0; 
                H1.WidthPercentage = 100;

                this.AddText(H1, H.Attribute("id").Value, 1, 1, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_MIDDLE, this.LightBlue, 0);
                float[] widthsH2 = new float[] { 10, 10 };
                PdfPTable H2 = this.InitTableProperties(widthsH2, this.GetRatio(widths) * this.GetRatio(widthsH1));
                H2.WidthPercentage = 100;
                H2.SpacingBefore = 0; 
                this.AddText(H2, this.Translate("Hazard"), 1, 1, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_MIDDLE, this.LightBlue, 0);
                this.AddText(H2, this.Translate("Barriers"), 1, 1, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_MIDDLE, this.DeepBlue);
                this.AddText(H2, H.Attribute("Text").Value) ;
               // H2.AddCell(H.Attribute("Text").Value);
                PdfPCell cellH2 = new PdfPCell();
                cellH2.AddElement(H2);

                PdfPCell cellH1 = new PdfPCell();
                cellH1.Border = 0;
                float[] widthsB1 = new float[] { 2, 10, 10 , 10};
                PdfPTable B1 = this.InitTableProperties(widthsB1, this.GetRatio(widths) * this.GetRatio(widthsH1) * this.GetRatio(widthsH2));
                B1.WidthPercentage = 100;
                B1.SpacingBefore = 0; 
                //this.AddText(B1, "Barrier", 2, 1, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY);

                this.AddLabel(B1, "Barrier", BaseColor.LIGHT_GRAY, null, PdfPCell.ALIGN_CENTER, 2);
                this.AddLabel(B1, "Control Hierarchy Type", BaseColor.LIGHT_GRAY, null, PdfPCell.ALIGN_CENTER, 1);
                this.AddLabel(B1, "Person", BaseColor.LIGHT_GRAY, null, PdfPCell.ALIGN_CENTER, 1);
                //this.AddText(B1, "Control Hierarchy Type:", 1, 1, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY);
                foreach (XElement B in H.Descendants("B")) {
                    this.AddText(B1, B.Attribute("Bid").Value);
                    //B1.AddCell(B.Attribute("Barrier").Value);
                    this.AddText(B1, B.Attribute("Barrier").Value);
                    this.AddText(B1, B.Attribute("CHT").Value);
                    this.AddText(B1, this.GetPerson(B) );
                }
                PdfPCell cellB1 = new PdfPCell();
                cellB1.Padding = 0;
                cellB1.AddElement(B1);
                H2.AddCell(cellB1);

                cellH2.Padding = 0;
                cellH2.Border = 0;

                H1.AddCell(cellH2);
                cellH1.Colspan = 3;
                cellH1.AddElement(H1);
                cellH1.Padding = 0;
                A2.AddCell(cellH1);
            }
            cell.AddElement(A2);
            cell.Padding = 0;
            A1.AddCell(cell);
        }
        // this.SetBorderColor(A1, true);
        //this.SetHeaderGray(t);
        A1.SpacingAfter = 8;
        this.PdfDoc.Add(A1);
    }
    void CreateHazard() {
        float[] widths = new float[] { 1, 42 };
        int i = 0;
        this.XmlDoc = XDocument.Parse("<r>" + this.GetXml(8) + "</r>");
        foreach (XElement H in this.XmlDoc.Descendants("H")) {
            float[] widthsH1 = new float[] { 1, 42 };
            PdfPTable H1 = this.InitTableProperties(widthsH1);

            H1.WidthPercentage = 100;
            if (i++ > 0)
                H1.SpacingBefore = 0;
            this.AddText(H1, H.Attribute("id").Value, 1, 1, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_MIDDLE, this.LightBlue, 0);
            float[] widthsH2 = new float[] { 10, 10 };
            PdfPTable H2 = this.InitTableProperties(widthsH2, this.GetRatio(widthsH1));
            H2.WidthPercentage = 100;
            H2.SpacingBefore = 0;
            this.AddText(H2, this.Translate("Hazard"), 1, 1, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_MIDDLE, this.LightBlue, 0);
            this.AddText(H2, this.Translate("Barriers"), 1, 1, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_MIDDLE, this.DeepBlue);
            this.AddText(H2, H.Attribute("Text").Value);
            // H2.AddCell(H.Attribute("Text").Value);
            PdfPCell cellH2 = new PdfPCell();
            cellH2.AddElement(H2);

            PdfPCell cellH1 = new PdfPCell();
            cellH1.Border = 0;
            float[] widthsB1 = new float[] { 2, 10, 10, 10 };
            PdfPTable B1 = this.InitTableProperties(widthsB1, this.GetRatio(widthsH1) * this.GetRatio(widthsH2));
            B1.WidthPercentage = 100;
            B1.SpacingBefore = 0;
            //this.AddText(B1, "Barrier", 2, 1, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY);

            this.AddLabel(B1, "Barrier", BaseColor.LIGHT_GRAY, null, PdfPCell.ALIGN_CENTER, 2);
            this.AddLabel(B1, "Control Hierarchy Type", BaseColor.LIGHT_GRAY, null, PdfPCell.ALIGN_CENTER, 1);
            this.AddLabel(B1, "Person", BaseColor.LIGHT_GRAY, null, PdfPCell.ALIGN_CENTER, 1);
            //this.AddText(B1, "Control Hierarchy Type:", 1, 1, PdfPCell.ALIGN_CENTER, PdfPCell.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY);
            foreach (XElement B in H.Descendants("B")) {
                this.AddText(B1, B.Attribute("Bid").Value);
                //B1.AddCell(B.Attribute("Barrier").Value);
                this.AddText(B1, B.Attribute("Barrier").Value);
                this.AddText(B1, B.Attribute("CHT").Value);
                this.AddText(B1, this.GetPerson(B));
            }
            PdfPCell cellB1 = new PdfPCell();
            cellB1.Padding = 0;
            cellB1.AddElement(B1);
            H2.AddCell(cellB1);

            cellH2.Padding = 0;
            cellH2.Border = 0;

            H1.AddCell(cellH2);
            cellH1.Colspan = 3;
            cellH1.AddElement(H1);
            cellH1.Padding = 0;
            this.PdfDoc.Add(H1);

        }
    }
    string GetPerson(XElement B) {
        string s = "";
        foreach (XElement P in B.Descendants("P")) {
            s += P.Attribute("UserName").Value+"\n";
        }
        return s; 
    }
}


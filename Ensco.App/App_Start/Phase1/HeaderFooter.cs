using System;
using System.Web.UI.HtmlControls;
//using System.Drawing;
using System.Xml;
using System.IO;
using System.Collections;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Diagnostics;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;

public class HeaderFooter: PdfPageEventHelper {
    PdfContentByte cb;
    PdfTemplate headerTemplate, footerTemplate;
    BaseFont bf = null;
    public string LastUpdatedDate;
    public string LastUpdatedBy;
    public float Margin;
    public string UserName;
    public string SelectedStatus;
    public string WiNo;
    public DataRow Dr;
    public string BaseUrl;
    public float Scale;
    public XDocument XmlDoc;
    string eMocType = "";
    public string Title; 
    public string Type;
    iTextSharp.text.Font PdfFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL);
    iTextSharp.text.Font PdfFontBold = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
    float TotalWidth = 700;
    public override void OnOpenDocument(PdfWriter writer, Document document) {
        try {
           // this.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb = writer.DirectContent;
            headerTemplate = cb.CreateTemplate(0, 0);
            footerTemplate = cb.CreateTemplate(50, 50);
        } catch (DocumentException de) {

        } catch (System.IO.IOException ioe) {

        }
    }
    public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document) {
        base.OnEndPage(writer, document);
        iTextSharp.text.Font baseFontNormal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        iTextSharp.text.Font baseFontBig = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

        String text = "Page " + writer.PageNumber + " of ";
        this.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
        //Add paging to footer
        {
            cb.BeginText();
            var arial_italic_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ariali.ttf");
            var arial_italic_base = BaseFont.CreateFont(arial_italic_path, BaseFont.IDENTITY_H, false);

            cb.SetFontAndSize(arial_italic_base, 8);
            int marginRight = 80;
            cb.SetTextMatrix(document.PageSize.GetRight(marginRight), document.PageSize.GetBottom(20));
            cb.ShowText(text);

            float len = bf.GetWidthPoint(text, 8);
            cb.EndText();
            cb.AddTemplate(footerTemplate, document.PageSize.GetRight(marginRight) + len, document.PageSize.GetBottom(20));
        }
        PdfPTable t = this.DisplayLogo();
        t.TotalWidth = this.TotalWidth;
        t.WriteSelectedRows(0, -1, document.LeftMargin,  document.PageSize.Height -this.Margin, writer.DirectContent);

        t = this.DisplayFooter();
        t.TotalWidth = this.TotalWidth;
        t.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin , writer.DirectContent);
    }
    void AddLabelText(PdfPTable t, string name, string value, int colSpan = 1) {
        PdfPCell cell = new PdfPCell(this.AddLabel(name));
        cell.PaddingLeft = 4;
        t.AddCell(cell);

        cell = new PdfPCell(this.AddText(value));
        cell.PaddingLeft = 4;
        t.AddCell(cell);
    }
    iTextSharp.text.Phrase AddLabel(string s) {
        return new iTextSharp.text.Phrase(s + "  ", this.PdfFontBold);
    }
    iTextSharp.text.Phrase AddText(string s) {
        return new iTextSharp.text.Phrase(s, this.PdfFont);
    }
    PdfPTable DisplayWIHeader() {
        //XElement root = this.XmlDoc.Descendants("eMoc").ElementAt(0);
        //this.eMocType = root.Element("eMocName").Value;

        PdfPTable t = new PdfPTable(2);
        t.LockedWidth = true;
        float[] widths = new float[] { 1.25f, 5f };
        t.SetWidths (widths);
       // t.TotalWidth = 
        t.HorizontalAlignment = 0;
        // this.AddLabelText(t, "Job Title:", this.Dr["jobDescriptionName"].ToString());

        PdfPCell cell = new PdfPCell(this.AddLabel("eMoC:"));
        cell.Border = 0; 
        cell.PaddingLeft = 4;
        t.AddCell(cell);

        cell = new PdfPCell();
        cell.PaddingLeft = 4;

        PdfPTable subTable = new PdfPTable(2);
        subTable.WidthPercentage = 100;
        subTable.SetWidths(new float[] { 10, 1.5f });
        
        PdfPCell c = new PdfPCell(this.AddText(this.eMocType)) ;
        c.Border = 0;
        c.PaddingLeft = 0;
        subTable.AddCell(c);
         c = new PdfPCell(this.AddText("")  ) ;
        c.PaddingLeft = 4;
        c.Border = 0;
        subTable.AddCell(c);
        cell.AddElement(subTable);
        t.AddCell(cell);


        string label = "Title:";
        //this.AddLabelText(t, label, this.Dr["name"].ToString());

        this.SetBorderColor(t, true);
        return t;
    }
    void SetBorderColor(PdfPTable t, bool showTop = false) {
        bool firstRow = true;
        foreach (PdfPRow row in t.Rows) {
            bool firstCell = true;
            foreach (PdfPCell cell in row.GetCells()) {
                // cell.BorderColor = BaseColor.GRAY;
                cell.BorderColorBottom = BaseColor.LIGHT_GRAY;
                cell.BorderColorLeft = BaseColor.LIGHT_GRAY;
                cell.BorderColorRight = BaseColor.LIGHT_GRAY;
                cell.BorderColorTop = BaseColor.LIGHT_GRAY;

                cell.BorderWidthRight = 1f;
                cell.BorderWidthBottom = 1f;
                if (firstRow)
                    cell.BorderWidthTop = 1f;
                else
                    cell.BorderWidthTop = 0f;
                if (firstCell)
                    cell.BorderWidthLeft = 1f;
                else
                    cell.BorderWidthLeft = 0f;
                if (!showTop)
                    cell.BorderWidthTop = 0f;
                firstCell = false;
            }
            firstRow = false;
        }
    }
    PdfPTable DisplayFooter() {
        PdfPTable t = new PdfPTable(2);
        t.LockedWidth = true;
        float[] widths = new float[] { 10, 1 };
        t.WidthPercentage = 100;

        BaseColor grey = new BaseColor(128, 128, 128);
        Font footer = FontFactory.GetFont("Arial", 8, Font.ITALIC);

        //Create a paragraph that contains the footer text
        string s = "Confidential information - uncontrolled copy when printed";
        Phrase p = new Phrase(s, footer);
        t.SetWidths(widths);
        t.HorizontalAlignment = 0;
        PdfPCell c1 = new PdfPCell(p);

       // c1.AddElement(p);
        //s = "Last Updated on: " + DateTime.Parse(this.LastUpdatedDate).ToString("d-MMM-yy") + " By " + this.LastUpdatedBy 
        s= "   Printed on: " + DateTime.Now.ToString("d-MMM-yy") + " By " + this.UserName;
        p = new Phrase(s, footer);
        PdfPCell c2 = new PdfPCell(p);
        //c2.AddElement(p);
        c1.Border = 0;
        c2.Border = 0;
        t.AddCell(c1);
        PdfPCell c = new PdfPCell();
        c.Border = 0;
        t.AddCell(c);
        t.AddCell(c2);
        t.AddCell(c);
        return t;
    }
    PdfPTable DisplayLogo() {
        PdfPTable t = new PdfPTable(2);
        t.LockedWidth = true;
        float[] widths = new float[] { 1f, 5f };
        t.HorizontalAlignment = Element.ALIGN_CENTER;
        t.SetWidths(widths);

        PdfPCell c1 = new PdfPCell();
        c1.PaddingLeft = 0;
        c1.Border = 0;
        string url = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.FilePath + @"/images/ensco.png"; //ResolveUrl("~/")
        url = url.ToLower().Replace(Path.GetFileName(HttpContext.Current.Request.Path).ToLower(), "");
        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(this.BaseUrl + @"/images/ensco.png" );
        img.ScaleToFit(1.85f *this.Scale, 0.93f *this.Scale);
        c1.AddElement(img);//new Chunk(img, 0, 0));

        t.AddCell(c1);
        PdfPCell c2 = new PdfPCell();
        c2.Border = 0;
        if (this.Title !=null ) {
            Phrase p = new Phrase(this.Title + " Permit", FontFactory.GetFont("Arial", 18, iTextSharp.text.Font.BOLD, (this.Title== "Confined Space"?BaseColor.BLACK: BaseColor.WHITE)) );

			c2 = new PdfPCell(p);

            c2.PaddingTop = 0;// 28;
            c2.HorizontalAlignment = Element.ALIGN_CENTER;
            c2.VerticalAlignment = Element.ALIGN_MIDDLE;
			BaseColor color = BaseColor.YELLOW; 
			if (this.Title =="Cold Work")
				color= new BaseColor(0, 70, 127);
			if (this.Title == "Hot Work")
				color = BaseColor.RED;
			c2.BackgroundColor = color;
        } 
         t.AddCell(c2); 
        return t;
    }
    public override void OnCloseDocument(PdfWriter writer, Document document) {
        base.OnCloseDocument(writer, document);

        var arial_italic_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ariali.ttf");
        var arial_italic_base = BaseFont.CreateFont(arial_italic_path, BaseFont.IDENTITY_H, false);

        footerTemplate.BeginText();
        footerTemplate.SetFontAndSize(arial_italic_base, 8);
        footerTemplate.SetTextMatrix(0, 0);
        footerTemplate.ShowText((writer.PageNumber - 1).ToString());
        footerTemplate.EndText();
    }

    /// <summary>
    /// Watermark for each page
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="document"></param>
    public override void OnStartPage(PdfWriter writer, Document document)
    {
        return;
        if (SelectedStatus != "Approved")
        {
            float fontSize = 230;  
            float xPosition = 500;
            float yPosition = 250;
            float angle = 45;
            try
            {
                PdfContentByte under = writer.DirectContentUnder;
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
                under.BeginText();
                BaseColor silver = new BaseColor(240, 240, 240);
                under.SetColorFill(silver);
                under.SetFontAndSize(baseFont, fontSize);
                under.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Draft", xPosition, yPosition, angle);
                under.EndText();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}

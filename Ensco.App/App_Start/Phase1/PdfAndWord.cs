using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
//using System.Drawing;
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

using iTextSharp.text.pdf.collection;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;

using iTextSharp.tool.xml;
using iTextSharp.tool.xml.css;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;

public class PdfAndWord {
    public DataSet ds;
    public DataSet dsLang; 
    public string BaseUrl;
    public string LessThanEncode = "dfdsfds";
    public string AmpEncode = "fdsfdsfdsf";
    public string Rig;
    public string SelectedStatus;
    public string Prefix = "WIT-";
    public string Title; 
    public iTextSharp.text.BaseColor Orange = new BaseColor(232, 109, 31);
    public iTextSharp.text.BaseColor DeepBlue = new BaseColor(0, 70, 127);
    public iTextSharp.text.BaseColor LightBlue = new BaseColor(61, 183, 228);
    public string Lang = "EN";
    public float TotalWidth;
    public float TotalHeight;
    public iTextSharp.text.Font PdfFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL);
    public iTextSharp.text.Font PdfFontWhite = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.WHITE);
    public iTextSharp.text.Font PdfFontBold = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
    public string id;
    public string XML;
    public string XMLLabel;

    public PdfWriter Writer;
    public Document PdfDoc;
    public XDocument XmlDoc;
    public XDocument XmlDocLabel;
    public string EncodeString = "!1s23!";
    public float Scale = 63;
    public int IconSize = 40;
    public string BaseImagePath;

    public string[] Headers;
    public string WiNo;

    string GetFileName() {
        return this.GetType().Name.Replace("Pdf", "") + " " + DateTime.Now.ToString(); 
    }
    public string Start(MemoryStream workStream,  string userName) {
        string f = this.GetFileName() + ".pdf", f1 = "";

        this.RenderPDF( workStream, f1, userName);
        return f;
    }
    public string  Start2(System.Web.HttpResponse response, string userName) {
        string f = this.GetFileName() + ".pdf", f1=""; 
        if (response == null) {
            f = f.Replace("/", ".").Replace(":", "."); 
            f = "../upload/" + f;
            f1 = HttpContext.Current.Server.MapPath(f); 
        } else {
            response.Buffer = true;
            response.Clear();
            response.AddHeader("Content-Disposition", "attachment; filename=" + f);
            response.ContentType = "application/pdf";
        }
        //this.RenderPDF(response, f1 , userName);
        return f; 
    }
    void RenderPDF(MemoryStream workStream, string f, string userName) {
        float margin = 0.3f * this.Scale;
        float headerMargin = 62, footerMargin = 30;
        this.PdfDoc = new Document(PageSize.A4.Rotate(), margin, margin, headerMargin, footerMargin);
        this.TotalWidth = this.PdfDoc.PageSize.Width - this.PdfDoc.LeftMargin - this.PdfDoc.RightMargin;
        this.TotalHeight = this.PdfDoc.PageSize.Height - this.PdfDoc.TopMargin - this.PdfDoc.BottomMargin;
        //if(response != null)
        //    Writer = PdfWriter.GetInstance(this.PdfDoc, response.OutputStream);
        //else
        //    Writer = PdfWriter.GetInstance(this.PdfDoc, new FileStream(f, FileMode.Create));
        Writer = PdfWriter.GetInstance(this.PdfDoc, workStream);
        Writer.CloseStream = false;

        HeaderFooter PageEventHandler = new HeaderFooter();
        PageEventHandler.BaseUrl = this.BaseUrl;
        PageEventHandler.XmlDoc = this.XmlDoc;
        PageEventHandler.Margin = 0;// margin;
        PageEventHandler.Scale = this.Scale;
        PageEventHandler.LastUpdatedBy = "";// dr["revisedBy"].ToString();
        PageEventHandler.UserName = userName;
        PageEventHandler.SelectedStatus = SelectedStatus;
        PageEventHandler.Title = this.Title;
        Writer.PageEvent = PageEventHandler;

        // HTMLWorker worker = new HTMLWorker(this.PdfDoc);

        this.PdfDoc.Open();
        this.CreateTable(this.PdfDoc);
        this.PdfDoc.Close();
    }
    void RenderPDF2(System.Web.HttpResponse response, string f, string userName) {
        float margin = 0.3f * this.Scale;
        float headerMargin = 62, footerMargin = 30;
        this.PdfDoc = new Document(PageSize.A4.Rotate(), margin, margin, headerMargin, footerMargin);
        this.TotalWidth = this.PdfDoc.PageSize.Width - this.PdfDoc.LeftMargin - this.PdfDoc.RightMargin;
        this.TotalHeight = this.PdfDoc.PageSize.Height - this.PdfDoc.TopMargin - this.PdfDoc.BottomMargin;
        if (response != null)
            Writer = PdfWriter.GetInstance(this.PdfDoc, response.OutputStream);
        else
            Writer = PdfWriter.GetInstance(this.PdfDoc, new FileStream(f, FileMode.Create));

        HeaderFooter PageEventHandler = new HeaderFooter();
        PageEventHandler.BaseUrl = this.BaseUrl;
        PageEventHandler.XmlDoc = this.XmlDoc;
        PageEventHandler.Margin = 0;// margin;
        PageEventHandler.Scale = this.Scale;
        PageEventHandler.LastUpdatedBy = "";// dr["revisedBy"].ToString();
        PageEventHandler.UserName = userName;
        PageEventHandler.SelectedStatus = SelectedStatus;
        PageEventHandler.Title = this.Title; 
        Writer.PageEvent = PageEventHandler;

        // HTMLWorker worker = new HTMLWorker(this.PdfDoc);

        this.PdfDoc.Open();
        this.CreateTable(this.PdfDoc);
        this.PdfDoc.Close();
    }
    protected PdfPTable CreatePlanning(int count ) {
        float[] widths = new float[count];
        for (int i = 0; i < count; i++)
            widths[i] = 1; 
        PdfPTable t = this.InitTableProperties(widths);
        DataTable dt = this.ds.Tables[0];
        DataRow dr = dt.Rows[0];
        this.AddHeader(t, "Planning", 8);
        foreach (DataColumn dc in dt.Columns) {
            string value = dr[dc].ToString();
            this.AddLabel(t, dc.ColumnName, BaseColor.LIGHT_GRAY, null, PdfPCell.ALIGN_RIGHT);
            this.AddText(t, value);
        }
        for (int i = 0; i < widths.Length - (dt.Columns.Count * 2) % widths.Length; i++)
            this.AddText(t, "");
        return t; 
    }
    protected virtual void CreateTable(Document doc) {
    }
    public string GetXml(int i, bool decode = false) {
        string s = "";
        DataTable dt = ds.Tables[i];
        foreach (DataRow dr in dt.Rows)
            s += dr[0].ToString();
        if (decode)
            s = HttpUtility.HtmlDecode(s);
        s = s.Replace(@"\", @"\\");
        s = s.Replace(@"'", @"\'");
        return s;//.Replace("'", "\\'") ;
    }
    public void AddHeader(PdfPTable t, string name, int colSpan = 1) {
        this.AddLabel(t, name, this.Orange, BaseColor.CYAN, PdfPCell.ALIGN_LEFT, colSpan);
    }
	 public void AddSubHeader(PdfPTable t, string name, DataTable dt ) {
        int colSpan = dt.Columns.Count; 
        this.AddLabel(t, name, this.DeepBlue, BaseColor.CYAN, PdfPCell.ALIGN_LEFT, colSpan);
    }
    public void AddSubHeader(PdfPTable t, string name, int colSpan=1) {
        this.AddLabel(t, name, this.DeepBlue, BaseColor.CYAN, PdfPCell.ALIGN_LEFT, colSpan);
    }
    public void AddSubSubHeader(PdfPTable t, string name, int colSpan = 1) {
        this.AddLabel(t, name, this.LightBlue, BaseColor.CYAN, PdfPCell.ALIGN_LEFT, colSpan);
    }
    public void AddBool(PdfPTable t, object obj, int colSpan=1 ) {
        string s = obj.ToString();
        if (s == "True")
            s = "Yes";
        else
            s = ""; 
        this.AddText(t, s, colSpan , 1, PdfPCell.ALIGN_CENTER); 
    }
  public   void CreateCheckbox(PdfPTable t, object value) {
        bool b = (bool)value;
        if (b)
            this.CreateImageFromUrl(t, this.BaseUrl + "images/checkbox.png");
        else {
            this.AddText(t, " ");
        }
    }
    public void AddLabelText(PdfPTable t,  DataRow dr , string name, int colSpan=1 ) {
        this.AddLabel(t, name);
        DataColumn c = dr.Table.Columns[name]; 
        //if ( c.DataType.ToString().ToLower().Contains("date"))
        //    u
        this.AddText(t, dr[name], colSpan); 
    }
    public PdfPCell AddText(PdfPTable t, object obj, int colSpan = 1, int rowSpan = 1
    , int HorizontalAlignment = PdfPCell.ALIGN_LEFT, int VerticalAlignment = PdfPCell.ALIGN_MIDDLE, BaseColor bgColor = null, int border = 1 ) {
        PdfPCell cell;
        DateTime date;
		string text =obj.ToString(); 
        //if (DateTime.TryParse(text, out date))
        //    text = date.ToString("d-MMM-yy");
        cell = new PdfPCell(this.AddText(text));

        if (bgColor != null && bgColor != BaseColor.LIGHT_GRAY) {
            cell = new PdfPCell(this.AddTextWHite(text));
            cell.BackgroundColor = bgColor;
        }
        cell.PaddingLeft = 2;
        cell.PaddingTop = 2;
        cell.PaddingRight = 2; 
        cell.PaddingBottom = 3;
        cell.BorderWidth = 0.01f;
        cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
        // cell.Border = 0;
       // cell.Padding = 2;
        cell.HorizontalAlignment = HorizontalAlignment;
        cell.VerticalAlignment = VerticalAlignment;
        cell.Colspan = colSpan;
        cell.Rowspan = rowSpan;
        if (border != 1) {
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0;
            cell.BorderWidthTop = 0;
        }
        //  cell.Border = border; 
        t.AddCell(cell);
        return cell;
    }
    public string Translate(string s) {
        if (this.Lang == "EN")
            return s;
        else {
            DataRow[] drs = this.dsLang.Tables[0].Select(" Name='" + s + "'");
            if (drs.Length > 0)
                return drs[0]["Name1"].ToString();
            else
                return s; 
        }
            return "P" + s; 
        XElement result = this.XmlDocLabel.Descendants("r")
        .FirstOrDefault(el => el.Attribute("en") != null &&
                              el.Attribute("en").Value == s);
        if (result != null && result.Attribute("pt") != null)
            return result.Attribute("pt").Value;

        return s;
    }
    public iTextSharp.text.Phrase AddLabel(string s) {
        return new iTextSharp.text.Phrase(s + "  ", this.PdfFontBold);
    }
    public iTextSharp.text.Phrase AddText(string s) {
        return new iTextSharp.text.Phrase(s, this.PdfFont);
    }
    public iTextSharp.text.Phrase AddTextWHite(string s) {
        return new iTextSharp.text.Phrase(s, this.PdfFontWhite);
    }
    public void AddPair(PdfPTable t, string name , object obj, int colSpan =1) {
        this.AddLabel(t, name);
        if ( obj.GetType().Name.ToLower() == "datarow") {
            DataRow dr = (DataRow)obj;
            this.AddText(t, dr[name], colSpan);
        } else
            this.AddText(t, obj, colSpan );

    }
    public void AddLabel(PdfPTable t, string text, BaseColor bgColor = null, BaseColor color = null, int align = PdfPCell.ALIGN_RIGHT, int colSpan = 1) {
        PdfPCell cell;
        text = this.Translate( Ensco.Utilities.UtilitySystem.SplitPascalCase( text)) ;
        iTextSharp.text.Font font = this.PdfFont;
        if (color != null)
            font = this.PdfFontWhite;
        cell = new PdfPCell(new Phrase(text, font));

        cell.PaddingLeft = 2;
        cell.PaddingTop = 2;
        cell.PaddingRight = 2; 
        cell.PaddingBottom = 3;
        cell.BorderWidth = 0.01f;
        
        // cell.Border = 0;
        cell.Colspan = colSpan;
        cell.HorizontalAlignment = align;
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        if (bgColor == null)
			bgColor=BaseColor.LIGHT_GRAY; 
            cell.BackgroundColor = bgColor;
        //cell.BorderWidthRight = 0;
        //cell.BorderColorRight = BaseColor.WHITE;
        t.AddCell(cell);
    }
    public void SetTableProperties(PdfPTable t, float[] widths) {
        t.SpacingBefore = 10;
        t.DefaultCell.FixedHeight = 70;
        t.DefaultCell.PaddingLeft = 11;

        t.TotalWidth = this.TotalWidth;
        //t.KeepTogether = true;
        //t.SplitRows = false;
        // t.SplitLate = false;

        t.SplitLate = false;
        t.LockedWidth = true;
        if (widths != null)
            t.SetWidths(widths);
        t.HorizontalAlignment = 0;
    }
    public PdfPTable InitTableProperties(float[] widths, float ratio = 1) {
        PdfPTable t = new PdfPTable(widths.Length);
        t.SpacingBefore = 10;
        t.DefaultCell.FixedHeight = 70;
        t.DefaultCell.Padding = 5;

        t.DefaultCell.SetLeading(10, 10);
        t.TotalWidth = this.TotalWidth * ratio;
        //t.KeepTogether = true;
        t.SplitRows = true;
        // t.SplitLate = false;

        t.SplitLate = false;
        t.LockedWidth = true;
        if (widths != null)
            t.SetWidths(widths);
        t.HorizontalAlignment = 0;
        t.WidthPercentage = 100;
        return t;
    }
    public float GetRatio(float[] widths) {
        float total = 0;
        for (int i = 0; i < widths.Length; i++)
            total += widths[i];

        return widths[widths.Length - 1] / total;
    }
    public void ConvertHtmlToPdf(string xHtml, string css) {
        // instantiate custom tag processor and add to `HtmlPipelineContext`.
        var tagProcessorFactory = Tags.GetHtmlTagProcessorFactory();
        //tagProcessorFactory.AddProcessor(
        //    new TableDataProcessor(),
        //    new string[] { HTML.Tag.TD }
        //);
        var htmlPipelineContext = new HtmlPipelineContext(null);
        htmlPipelineContext.SetTagFactory(tagProcessorFactory);

        var pdfWriterPipeline = new PdfWriterPipeline(this.PdfDoc, this.Writer);
        var htmlPipeline = new HtmlPipeline(htmlPipelineContext, pdfWriterPipeline);

        // get an ICssResolver and add the custom CSS
        var cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
        cssResolver.AddCss(css, "utf-8", true);
        var cssResolverPipeline = new CssResolverPipeline(
            cssResolver, htmlPipeline
        );

        var worker = new XMLWorker(cssResolverPipeline, true);
        var parser = new XMLParser(worker);
        using (var stringReader = new StringReader(xHtml)) {
            parser.Parse(stringReader);
        }

    }
    public string Recursive(string s) {
        int k = 25;
        if (s.Length > k) {
            string y = s.Substring(0, k - 1);
            var index = y.LastIndexOf("/");
            if (index < 1)
                return s;
            return s.Substring(0, index) + "\n" + Recursive(s.Substring(index, s.Length - index));
        } else {
            return s;
        }
    }
    void CreateLabel(PdfPTable t, string name, XElement element, bool isRadio = false) {
        string value = "";

        this.AddLabel(t, name, BaseColor.LIGHT_GRAY, null, PdfPCell.ALIGN_RIGHT);
        name = name.Replace(" ", "");
        if (element.Element(name) != null)
            value = element.Element(name).Value;
        else {
            foreach (XAttribute a in element.Attributes()) {
                if (a.Name.LocalName.ToLower() == name.ToLower())
                    value = a.Value;
            }
        }
        if (isRadio)
            if (value == "1")
                value = "Yes";
            else
                value = "No";
        if (true && name == "HighestRiskLevel") {
            this.WriteWingDing(t, value.Trim());
        } else
            this.AddText(t, value.Trim());
    }
    void WriteWingDing(PdfPTable t, string level) {
        PdfPCell c = new PdfPCell();
        if (level == "0") {
            t.AddCell(c);
            return;
        }
        c.PaddingLeft = 5;
        level = level.Substring(0, 1);
        Paragraph ph = new Paragraph();

        ph.Add(new Chunk(level));

        string color = "green";
        if (level == "2")
            color = "yellow";
        if (level == "3")
            color = "red";
        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(this.BaseUrl + @"images/" + color + ".png");
        float size = 7;
        img.ScaleToFit(size, size);
        ph.Add(new Chunk(img, 0, 0));
        c.AddElement(ph);
        c.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        c.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        t.AddCell(c);
    }
    public void DateBind(DataTable dt, string header, string subHeader, bool showColumn = true) {
        PdfPTable t = this.GetTable(dt);
        if (header != "")
            this.AddHeader(t, header, dt.Columns.Count);
        if (subHeader != "")
            this.AddSubHeader(t, subHeader, dt.Columns.Count);
        this.AddTable(t, dt, showColumn);
        this.PdfDoc.Add(t);
    }
    public void CreateText(PdfPTable t, string name, XElement element) {
        string value = "";
        foreach (XAttribute a in element.Attributes()) {
            if (a.Name.LocalName.ToLower() == name.ToLower())
                value = a.Value;
        }
        if (name.ToLower() == "status")
            value = this.Translate(value);
        this.AddText(t, (value));
        // this.AddText(t, this.Decode(value));
    }
    public void AddLink(PdfPTable t, string guid, string title, bool isLink = false, bool withBorder = true) {
        PdfPCell cellOut = new PdfPCell();
        Paragraph p = new Paragraph();
        PdfPCell c = new PdfPCell();

        Chunk anchor = new Chunk(title);
        Uri uri;
        guid = guid.Replace("upload/", "");
        string link = this.BaseUrl + "upload/" + guid;
        if (isLink)
            link = guid;
        if (Uri.TryCreate(link, UriKind.Absolute, out uri)) {
            anchor.SetAnchor(uri);
            // anchor.SetUnderline(1, 1);
            anchor.Font.Size = 10;
            anchor.Font.Color = BaseColor.BLUE;
            p.Add(anchor);
        }
        c.AddElement(p);
        c.PaddingBottom = 2;
        c.PaddingLeft = 2;
        c.SetLeading(2, 2);
        if (!withBorder)
            c.Border = 0;
        t.AddCell(c);
    }
    public void CreateAtt() {
        float[] widths = new float[] { 1, 8, 8, 8 };
        PdfPTable t = this.InitTableProperties(widths);
        this.AddLabel(t, "Attachment and Links ", this.Orange, BaseColor.WHITE, PdfPCell.ALIGN_LEFT, widths.Length);
        this.AddLabel(t, "Attachments", this.DeepBlue, BaseColor.WHITE, PdfPCell.ALIGN_LEFT, widths.Length);

        XElement root = this.XmlDoc.Elements("RA").ElementAt(0);
        if (root.Descendants("ATT").Count() == 0)
            return;
        string[] arr = { "No.", "Name / Title ", "Uploaded by ", "Date Uploaded " };

        foreach (string name in arr)
            this.AddLabel(t, name, BaseColor.LIGHT_GRAY, null, PdfPCell.ALIGN_CENTER);
        string[] arr1 = { "id", "guid", "uploadedBY", "DateUploaded" };

        foreach (XElement att in root.Descendants("ATT")) {
            foreach (string name in arr1)
                if (name == "guid") {
                    this.AddLink(t, att.Attribute("guid").Value, att.Attribute("name").Value);
                } else
                    this.CreateText(t, name, att);
        }
        // t.SpacingAfter = 8;
        if (root.Descendants("ATT").Count() == 0)
            return;
        this.PdfDoc.Add(t);
    }
    public void CreateLinks() {
        float[] widths = new float[] { 1, 6, 6, 6, 6 };
        PdfPTable t = this.InitTableProperties(widths);
        this.AddLabel(t, "Links", this.DeepBlue, BaseColor.WHITE, PdfPCell.ALIGN_LEFT, widths.Length);

        XElement root = this.XmlDoc.Elements("RA").ElementAt(0);
        if (root.Descendants("L").Count() == 0)
            return;
        string[] arr = { "No.", "Name / Title ", "Hyperlink ", "Poster ", "Date" };

        foreach (string name in arr)
            this.AddLabel(t, name, BaseColor.LIGHT_GRAY, null, PdfPCell.ALIGN_CENTER);
        string[] arr1 = { "id", "name", "Link", "Poster", "date" };

        foreach (XElement link in root.Descendants("L")) {
            foreach (string name in arr1)
                if (name == "guid") {
                    this.AddLink(t, link.Attribute("guid").Value, link.Attribute("name").Value);
                } else
                    this.CreateText(t, name, link);
        }
        t.SpacingAfter = 8;
        this.PdfDoc.Add(t);
    }
    public void AddAttachments(PdfPTable t, XElement action) {
        if (action.Attribute("attachment") == null || action.Attribute("attachment").Value == "") {
            this.AddText(t, "");
            return;
        }
        this.AddAtt(t, action.Attribute("attachment").Value);
        //PdfPTable t1 = this.InitTableProperties(new float[] { 1 });

        //foreach (string s in arr) {
        //    string[] arr1 = s.Split(':');
        //    this.AddLink(t1, arr1[0], arr1[1], false );
        //}
        //PdfPCell cell = new PdfPCell();
        //cell.AddElement(t1);
        //t.AddCell(cell); 

    }
    public void AddAtt(PdfPTable t, string value) {
        string[] arr = value.Split('|');
        PdfPTable t1 = this.InitTableProperties(new float[] { 1 });

        foreach (string s in arr) {
            string[] arr1 = s.Split(':');
            this.AddLink(t1, arr1[0], arr1[1], false, false);
        }
        PdfPCell cell = new PdfPCell();
        cell.AddElement(t1);
        t.AddCell(cell);
    }

    public string GetElementValue(XElement root, string name) {
        if (root.Element(name) == null)
            return "";
        return root.Element(name).Value;
    }
    public string GetAttr(XElement x, string name) {
        XAttribute attr = x.Attribute(name);
        if (attr != null)
            return attr.Value;
        return "";
    }
    public void SetMargins() {
        this.PdfDoc.SetMargins(15, 15, 110 + 20, 60);
    }

    public void AddImageFromUrl11(PdfPTable t, string url, int colSpan=1, int HorizontalAlignment = PdfPCell.ALIGN_CENTER) {
      //  t.AddCell(this.WarningIcon(url)); return; 
        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(url);
        float size = 30;
       // img.ScaleToFit(size, size);
        img.ScaleAbsoluteWidth(size);
        img.ScaleAbsoluteHeight(size);
        Chunk ch = new Chunk(img, 0, 0 );
        PdfPCell c  = new PdfPCell();        
        c.AddElement(ch);
        c.Colspan = colSpan;

        //        image1.SetAbsolutePosition((PageSize.A4.Width - image1.ScaledWidth) / 2, (PageSize.A4.Height - image1.ScaledHeight) / 2);

        c.FixedHeight = 50;
        c.HorizontalAlignment = PdfPCell.ALIGN_CENTER; 
        c.VerticalAlignment = Element.ALIGN_MIDDLE;
        img.ScaleAbsolute(200, 50);

        t.AddCell(c);
    }
    PdfPTable WarningIcon(string url) {
        PdfPTable subTable = new PdfPTable(1);
        subTable.SplitLate = false;
        subTable.SpacingBefore = 1;
        subTable.SetWidths(new float[] { 1 });
       // string url = this.BaseUrl + "images/" + name + ".png";
        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(url);
        // img.ScaleToFit(this.IconSize - 10, this.IconSize - 10);
        img.ScaleAbsoluteHeight(25);
        img.ScaleAbsoluteWidth(25);
        Chunk ch = new Chunk(img, 0, 0 );

        PdfPCell c = new PdfPCell();
        Phrase ph1 = new Phrase();
        ph1.Font.Size = 5;
        ph1.Add("t");

        for (int i = 0; i < 2; i++) {
            c = new PdfPCell();
            c.Border = 0;
            c.SetLeading(1, 0);
            if (i == 1)
                c.AddElement(ch);
            else {
                continue;
                c.FixedHeight = 5;
                c.AddElement(ph1);
            }
            subTable.AddCell(c);
        }
        subTable.DefaultCell.Border = 0;
        return subTable;

    }
    public void AddImageFromUrl(PdfPTable t, string url, int colSpan = 1, int HorizontalAlignment = PdfPCell.ALIGN_CENTER, int rowSpan=1) {
        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(url);
        float size = 40;
        // img.ScaleToFit(size, size);
        img.ScaleAbsolute(size, size);

        //img.ScaleAbsoluteWidth(size);
        //img.ScaleAbsoluteHeight(size);
        //img.ScaleAbsolute(200, 50);
        img.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
        PdfPCell c = new PdfPCell();
        c.AddElement(img);
        c.Colspan = colSpan;
        c.Rowspan = rowSpan; 

        //        image1.SetAbsolutePosition((PageSize.A4.Width - image1.ScaledWidth) / 2, (PageSize.A4.Height - image1.ScaledHeight) / 2);

        c.FixedHeight = 50;
      //  c.Border = 1;
        c.HorizontalAlignment = PdfPCell.ALIGN_CENTER; 
        c.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;

        t.AddCell(c);
    }
    public void CreateImageFromUrl(PdfPTable t, string url, int colSpan = 1) {
        PdfPCell c;
        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(url);

        float h = 7;
        float w = img.Width * h / img.Height;
        if (w > 2.2f * this.Scale)
            w = 2.2f * this.Scale;
        img.ScaleAbsoluteWidth(w);
        img.ScaleAbsoluteHeight(h);

        c = new PdfPCell(img);
        // c.Border = 0;
        c.Padding = 5;
        c.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        c.Colspan = colSpan;
        t.AddCell(c);
    }
    public void AddEmptyImage(PdfPTable t, int total) {
        for (int i = 0; i < total; i++) {
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(this.BaseUrl + @"images/empty.png");
            PdfPCell c = new PdfPCell(img);
            c.Border = 0;
            t.AddCell(c);
        }
    }

    public void SetHeaderGray(PdfPTable t) {
        foreach (PdfPRow row in t.Rows) {
            foreach (PdfPCell cell in row.GetCells())
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            return;
        }
    }
    public void SetBorderColor(PdfPTable t, bool showTop = false) {
        // return;
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
                //cell.BorderWidthBottom = 1f;
                //if (firstRow)
                //    cell.BorderWidthTop = 1f;
                //else
                //    cell.BorderWidthTop = 1f;
                if (firstCell)
                    cell.BorderWidthLeft = 1f;
                else
                    cell.BorderWidthLeft = 0f;
                if (!showTop)
                    cell.BorderWidthTop = .5f;
                cell.BorderWidthTop = .5f;
                cell.BorderWidthBottom = .5f;
                cell.UseVariableBorders = true;

                firstCell = false;
            }
            firstRow = false;
        }
    }
    public string ReplaceFont(string v) {
        string value = v;
        value = value.Replace("font-size: xx-small", "font-size:10pt ");
        value = value.Replace("font-size: x-small", "font-size:10pt ");
        value = value.Replace("font-size: small", "font-size:10pt");
        value = value.Replace("font-size: Medium", "font-size:10pt");
        value = value.Replace("font-size: large", "font-size:10pt");
        value = value.Replace("font-size: x-large", "font-size:10pt");
        value = value.Replace("font-size: xx-large", "font-size:10pt ");
        value = value.Replace(@" style=""small", @" style=""font-size:10pt ");
        //value = value.Replace(@"style=""margin: 0px;""", @"style=""margin: 0px;font-size:10pt""");
        //value = value.Replace(@"style=""", @"style=""font-size:10pt;");
        value = value.Replace("font-family: arial,helvetica,sans-serif; small", "font-family: arial;font-size: 10pt");
        value = @"<body style=""font-size:10pt;"">" + value + @"</body>";
        return value;
    }
   public   float [] GetWidths(int count ) {
        float[] arr = new float[count ];
        for (int i = 0; i < count; i++)
            arr[i] = 1;
        return arr; 
    }
   public  PdfPTable GetTable(DataTable dt, float[] arr = null) {
        float[] widths = new float[dt.Columns.Count];
        for (int i = 0; i < widths.Length; i++)
            widths[i] = 1;
        if (arr != null)
            widths = arr;
        PdfPTable t = this.InitTableProperties(widths);

        return t;
    }
    public void AddApproverTable(PdfPTable t, DataTable dt, Dictionary<string, string> dict) {
        foreach (DataColumn dc in dt.Columns) {
            this.AddLabel(t, dc.ColumnName, null, null, PdfPCell.ALIGN_CENTER);
        }
        foreach (DataRow dr in dt.Rows) {
            string role = dr["role"].ToString();
            string text = dict[role].ToString();
            Font font = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL);
            Phrase ph = new Phrase(text, font);
            PdfPCell cell = new PdfPCell(ph);
            cell.Colspan = dt.Columns.Count;

            t.AddCell(cell);
            foreach (DataColumn dc in dt.Columns) {
                this.AddText(t, dr[dc.ColumnName].ToString());
            }
        }
    }
    public void AddTable(PdfPTable t, DataTable dt, bool showColumn = true) {
        if (showColumn)
            foreach (DataColumn dc in dt.Columns) {
                this.AddLabel(t, dc.ColumnName, null, null, PdfPCell.ALIGN_CENTER);
            }
        foreach (DataRow dr in dt.Rows) {
            foreach (DataColumn dc in dt.Columns) {
                object obj = dr[dc.ColumnName];
                string text = obj.ToString();
                if (dc.DataType.Name == "Boolean") {
                    if (obj == DBNull.Value || (bool)obj == false)
                        text = "";
                    else
                        text = "Yes";
                }
                this.AddText(t, text);
            }
        }
    }
    public void CreateEquipment(int index) {
        DataTable dt = this.ds.Tables[index];
        PdfPTable t = this.GetTable(dt);
        this.AddLabel(t, "Equipment Worked On", null, null, PdfPCell.ALIGN_CENTER, dt.Columns.Count - 5);
        this.AddLabel(t, "EAMS Criticality ", null, null, PdfPCell.ALIGN_CENTER, 5);

        this.AddTable(t, dt);

        this.PdfDoc.Add(t);
    }
    public void CreateVerification(int index ) {
        DataTable dt = this.ds.Tables[index ];
        PdfPTable t = this.GetTable(dt);
        this.AddHeader(t, "Verification", dt.Columns.Count);

        this.AddTable(t, dt);
        this.PdfDoc.Add(t);
    }
    public  void AddCommon() {

    }
    public string HtmlDecodeRichText(object obj) {
        string value = obj.ToString();
        value = HttpUtility.HtmlDecode(value);
        value = value.Replace(this.LessThanEncode, "&lt;");
        value = value.Replace(this.AmpEncode, "&amp;");
        return value;
    }
    public PdfPCell GetRichCell(string html, int colSpan = 1) {
        html = this.HtmlDecodeRichText(html); 
        var cell = new PdfPCell {
            // Border = 1,
            HorizontalAlignment = Element.ALIGN_LEFT
        };
        iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph();
        p.SetLeading(0.1f, 0f);

     //   try {
            List<IElement> htmlArrayList = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(html), this.GetStyleSheet(true));
            p.InsertRange(0, htmlArrayList);
        //} catch (Exception ex) {
        //    var mh = new MySampleHandler();
        //    using (TextReader sr = new StringReader(html)) {
        //        XMLWorkerHelper.GetInstance().ParseXHtml(mh, sr);
        //    }
        //    p.InsertRange(0, mh.elements);
        //}
        cell.AddElement(p);
        cell.PaddingLeft = 4;
        cell.Colspan = colSpan;
        return cell;
    }
    public StyleSheet GetStyleSheet(bool isUnicode) {
        //if (!isUnicode)
        //    return null;
        string path = HttpContext.Current.Server.MapPath("../app_code/ARIALUNI.TTF");
        FontFactory.Register(path);// "c:/windows/fonts/ARIALUNI.TTF");
        StyleSheet style = new StyleSheet();
        style.LoadTagStyle("body", "face", "times Unicode MS");
        style.LoadTagStyle("body", "encoding", BaseFont.IDENTITY_H);
        style.LoadTagStyle("body", "line - height", "110px");
        style.LoadTagStyle("td", "line - height", "110px");
        //style.LoadTagStyle("td", "style", "line-height:125pt");
        //style.LoadTagStyle("body", "style", "line-height:125pt");

        return style;
    }
}
public class MySampleHandler : IElementHandler {
    //Generic list of elements
    public List<IElement> elements = new List<IElement>();
    //Add the supplied item to the list
    public void Add(IWritable w) {
        if (w is WritableElement) {
            elements.AddRange(((WritableElement)w).Elements());
        }
    }
}
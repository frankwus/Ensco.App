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
public class PdfPobLifeboat : PdfAndWord {
    protected override void CreateTable(Document document) {
        foreach (DataTable dt in this.ds.Tables) {
            this.CreateTableByLifeboat(dt); 
        }
    }

    void CreateTableByLifeboat(DataTable dt ) {
        PdfPTable t = this.GetTable(dt);
        this.AddSubHeader(t, "", dt.Columns.Count);

        this.AddTable(t, dt);

        this.PdfDoc.Add(t);
        this.PdfDoc.NewPage(); 
    }
}
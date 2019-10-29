using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for PobSummaryReport
/// </summary>
public class PobSummaryReport : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell8;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell16;
    private XRPageInfo xrPageInfo1;
    private XRPageInfo xrPageInfo2;
    private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
    private ReportHeaderBand reportHeaderBand1;
    private DetailReportBand detailReportBand1;
    private GroupHeaderBand groupHeaderBand1;
    private XRTable xrTable2;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell18;
    private GroupHeaderBand groupHeaderBand2;
    private XRPanel xrPanel1;
    private XRTable xrTable3;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell19;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell21;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell23;
    private XRTableCell xrTableCell24;
    private XRTableCell xrTableCell25;
    private XRTableCell xrTableCell26;
    private XRTableCell xrTableCell27;
    private XRTableCell xrTableCellVan;
    private XRTableCell xrTableCellEss;
    private DetailBand detailBand1;
    private XRTable xrTable4;
    private XRTableRow xrTableRow5;
    private XRTableCell xrTableCell30;
    private XRTableCell xrTableCell31;
    private XRTableCell xrTableCell32;
    private XRTableCell xrTableCell33;
    private XRTableCell xrTableCell34;
    private XRTableCell xrTableCell35;
    private XRTableCell xrTableCell36;
    private XRTableCell xrTableCell37;
    private XRTableCell xrTableCell38;
    private XRTableCell xrTableCell39;
    private XRTableCell xrTableCell40;
    private GroupFooterBand groupFooterBand1;
    private DetailReportBand detailReportBand2;
    private GroupHeaderBand groupHeaderBand3;
    private XRPanel xrPanel2;
    private XRTable xrTable5;
    private XRTableRow xrTableRow6;
    private XRTableCell xrTableCell41;
    private XRTableCell xrTableCell42;
    private XRTableCell xrTableCell43;
    private XRTableCell xrTableCell44;
    private XRTableCell xrTableCell45;
    private DetailBand detailBand2;
    private XRTable xrTable6;
    private XRTableRow xrTableRow7;
    private XRTableCell xrTableCell46;
    private XRTableCell xrTableCell47;
    private XRTableCell xrTableCell48;
    private XRTableCell xrTableCell49;
    private XRTableCell xrTableCell50;
    private DetailReportBand detailReportBand3;
    private GroupHeaderBand groupHeaderBand4;
    private XRPanel xrPanel3;
    private XRTable xrTable7;
    private XRTableRow xrTableRow8;
    private XRTableCell xrTableCell51;
    private XRTableCell xrTableCell52;
    private XRTableCell xrTableCell53;
    private XRTableCell xrTableCell54;
    private XRTableCell xrTableCell55;
    private DetailBand detailBand3;
    private XRTable xrTable8;
    private XRTableRow xrTableRow9;
    private XRTableCell xrTableCell56;
    private XRTableCell xrTableCell57;
    private XRTableCell xrTableCell58;
    private XRTableCell xrTableCell59;
    private XRTableCell xrTableCell60;
    private DetailReportBand detailReportBand4;
    private GroupHeaderBand groupHeaderBand5;
    private XRPanel xrPanel4;
    private XRTable xrTable9;
    private XRTableRow xrTableRow10;
    private XRTableCell xrTableCell61;
    private XRTableCell xrTableCell62;
    private DetailBand detailBand4;
    private XRTable xrTable10;
    private XRTableRow xrTableRow11;
    private XRTableCell xrTableCell63;
    private XRTableCell xrTableCell64;
    private DetailReportBand detailReportBand5;
    private GroupHeaderBand groupHeaderBand6;
    private XRPanel xrPanel5;
    private XRTable xrTable11;
    private XRTableRow xrTableRow12;
    private XRTableCell xrTableCell65;
    private XRTableCell xrTableCell66;
    private XRTableCell xrTableCell67;
    private XRTableCell xrTableCell68;
    private DetailBand detailBand5;
    private XRTable xrTable12;
    private XRTableRow xrTableRow13;
    private XRTableCell xrTableCell69;
    private XRTableCell xrTableCell70;
    private XRTableCell xrTableCell71;
    private XRTableCell xrTableCell72;
    private XRControlStyle Title;
    private XRControlStyle DetailCaption1;
    private XRControlStyle DetailData1;
    private XRControlStyle GroupCaption3;
    private XRControlStyle GroupData3;
    private XRControlStyle DetailCaption3;
    private XRControlStyle DetailData3;
    private XRControlStyle DetailData3_Odd;
    private XRControlStyle DetailCaptionBackground3;
    private XRControlStyle PageInfo;
    private PageFooterBand PageFooter;
    private XRLabel xrLabel3;
    private XRLabel xrLabel5;
    private XRLabel xrLabel4;
    private XRLabel xrLabel6;
    private XRLabel xrLabel7;
    private XRLabel xrLabel8;
    private XRLabel xrLabel9;
    private GroupFooterBand groupFooterBeds;
    private XRLabel xrLabel11;
    private XRLabel xrLabel10;
    private XRLabel xrLabel13;
    private XRPageInfo xrPageInfo3;
    private XRLabel xrLabel15;
    private XRPageBreak xrPageBreak1;
    public DevExpress.XtraReports.Parameters.Parameter RigName;
    private XRPanel xrPanel6;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrLabel1;
    private XRLabel xrLabel2;
    private XRPageInfo xrPageInfo4;
    private XRPictureBox xrPictureBox2;
    private XRLabel xrLabel20;
    private XRLabel xrLabel12;
    private XRLabel xrLabel14;
    private XRLabel xrLabel16;
    private XRLabel xrLabel17;
    private XRLabel xrLabel18;
    private XRLabel xrLabel19;
    public DevExpress.XtraReports.Parameters.Parameter LogoFile;
    public DevExpress.XtraReports.Parameters.Parameter IrmaFile;
    public DevExpress.XtraReports.Parameters.Parameter ShowVantage;
    public DevExpress.XtraReports.Parameters.Parameter ShowEssential;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public PobSummaryReport()
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrPageInfo3 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.reportHeaderBand1 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrPanel6 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo4 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.detailReportBand1 = new DevExpress.XtraReports.UI.DetailReportBand();
            this.groupHeaderBand1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.groupHeaderBand2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCellVan = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCellEss = new DevExpress.XtraReports.UI.XRTableCell();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
            this.groupFooterBand1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.detailReportBand2 = new DevExpress.XtraReports.UI.DetailReportBand();
            this.groupHeaderBand3 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrPageBreak1 = new DevExpress.XtraReports.UI.XRPageBreak();
            this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell44 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell45 = new DevExpress.XtraReports.UI.XRTableCell();
            this.detailBand2 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable6 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell46 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell47 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell48 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell49 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell50 = new DevExpress.XtraReports.UI.XRTableCell();
            this.detailReportBand3 = new DevExpress.XtraReports.UI.DetailReportBand();
            this.groupHeaderBand4 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrPanel3 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrTable7 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell51 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell52 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell53 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell54 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell55 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.detailBand3 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable8 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell56 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell57 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell58 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell59 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell60 = new DevExpress.XtraReports.UI.XRTableCell();
            this.detailReportBand4 = new DevExpress.XtraReports.UI.DetailReportBand();
            this.groupHeaderBand5 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrPanel4 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrTable9 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell61 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell62 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.detailBand4 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable10 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow11 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell63 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell64 = new DevExpress.XtraReports.UI.XRTableCell();
            this.detailReportBand5 = new DevExpress.XtraReports.UI.DetailReportBand();
            this.groupHeaderBand6 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrPanel5 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrTable11 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell65 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell66 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell67 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell68 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.detailBand5 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable12 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell69 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell70 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell71 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell72 = new DevExpress.XtraReports.UI.XRTableCell();
            this.groupFooterBeds = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailCaption1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.GroupCaption3 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.GroupData3 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailCaption3 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData3 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData3_Odd = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailCaptionBackground3 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.RigName = new DevExpress.XtraReports.Parameters.Parameter();
            this.LogoFile = new DevExpress.XtraReports.Parameters.Parameter();
            this.IrmaFile = new DevExpress.XtraReports.Parameters.Parameter();
            this.ShowVantage = new DevExpress.XtraReports.Parameters.Parameter();
            this.ShowEssential = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo3,
            this.xrTable1,
            this.xrLabel13,
            this.xrLabel15});
            this.Detail.HeightF = 80.52829F;
            this.Detail.KeepTogether = true;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPageInfo3
            // 
            this.xrPageInfo3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(109)))), ((int)(((byte)(31)))));
            this.xrPageInfo3.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo3.ForeColor = System.Drawing.Color.White;
            this.xrPageInfo3.LocationFloat = new DevExpress.Utils.PointFloat(265.4862F, 0F);
            this.xrPageInfo3.Name = "xrPageInfo3";
            this.xrPageInfo3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo3.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo3.SizeF = new System.Drawing.SizeF(735.5139F, 23F);
            this.xrPageInfo3.StylePriority.UseBackColor = false;
            this.xrPageInfo3.StylePriority.UseFont = false;
            this.xrPageInfo3.StylePriority.UseForeColor = false;
            this.xrPageInfo3.StylePriority.UseTextAlignment = false;
            this.xrPageInfo3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrPageInfo3.TextFormatString = "{0:dd-MMM-yyyy hh:mm tt}";
            // 
            // xrTable1
            // 
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0.0001439508F, 23F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1,
            this.xrTableRow2});
            this.xrTable1.SizeF = new System.Drawing.SizeF(1001F, 56F);
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell3,
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell6,
            this.xrTableCell7,
            this.xrTableCell8});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 0.5D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(127)))));
            this.xrTableCell1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell1.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell1.Multiline = true;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell1.StyleName = "DetailCaption1";
            this.xrTableCell1.StylePriority.UseBackColor = false;
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Ensco Per Contract";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 0.19167321259280393D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(127)))));
            this.xrTableCell2.Multiline = true;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell2.StyleName = "DetailCaption1";
            this.xrTableCell2.StylePriority.UseBackColor = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "Ensco Standard";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 0.13753622553797537D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(127)))));
            this.xrTableCell3.Multiline = true;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell3.StyleName = "DetailCaption1";
            this.xrTableCell3.StylePriority.UseBackColor = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Ensco Other";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell3.Weight = 0.11733782360896586D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(127)))));
            this.xrTableCell4.Multiline = true;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell4.StyleName = "DetailCaption1";
            this.xrTableCell4.StylePriority.UseBackColor = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "Ensco Service";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell4.Weight = 0.12160072181441106D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(127)))));
            this.xrTableCell5.Multiline = true;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell5.StyleName = "DetailCaption1";
            this.xrTableCell5.StylePriority.UseBackColor = false;
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.Text = "Ensco Catering";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell5.Weight = 0.13793982329437432D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(127)))));
            this.xrTableCell6.Multiline = true;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell6.StyleName = "DetailCaption1";
            this.xrTableCell6.StylePriority.UseBackColor = false;
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.Text = "Client";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell6.Weight = 0.069682566938880974D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(127)))));
            this.xrTableCell7.Multiline = true;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell7.StyleName = "DetailCaption1";
            this.xrTableCell7.StylePriority.UseBackColor = false;
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.Text = "Client Service";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell7.Weight = 0.12808938764614888D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(127)))));
            this.xrTableCell8.Multiline = true;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell8.StyleName = "DetailCaption1";
            this.xrTableCell8.StylePriority.UseBackColor = false;
            this.xrTableCell8.StylePriority.UseTextAlignment = false;
            this.xrTableCell8.Text = "Total POB";
            this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell8.Weight = 0.09614024443520465D;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell9,
            this.xrTableCell10,
            this.xrTableCell11,
            this.xrTableCell12,
            this.xrTableCell13,
            this.xrTableCell14,
            this.xrTableCell15,
            this.xrTableCell16});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 0.5D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell9.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Summary.EnscoPerContract]")});
            this.xrTableCell9.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell9.Multiline = true;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell9.StyleName = "DetailData1";
            this.xrTableCell9.StylePriority.UseBorders = false;
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "xrTableCell9";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell9.Weight = 0.19167321259280393D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Summary.EnscoStandard]")});
            this.xrTableCell10.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell10.Multiline = true;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell10.StyleName = "DetailData1";
            this.xrTableCell10.StylePriority.UseFont = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "xrTableCell10";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell10.Weight = 0.13753626617389855D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Summary.EnscoOther]")});
            this.xrTableCell11.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell11.Multiline = true;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell11.StyleName = "DetailData1";
            this.xrTableCell11.StylePriority.UseFont = false;
            this.xrTableCell11.StylePriority.UseTextAlignment = false;
            this.xrTableCell11.Text = "xrTableCell11";
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell11.Weight = 0.11733783376794663D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Summary.EnscoService]")});
            this.xrTableCell12.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell12.Multiline = true;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell12.StyleName = "DetailData1";
            this.xrTableCell12.StylePriority.UseFont = false;
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.Text = "xrTableCell12";
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell12.Weight = 0.12160072181441105D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Summary.EnscoCatering]")});
            this.xrTableCell13.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell13.Multiline = true;
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell13.StyleName = "DetailData1";
            this.xrTableCell13.StylePriority.UseFont = false;
            this.xrTableCell13.StylePriority.UseTextAlignment = false;
            this.xrTableCell13.Text = "xrTableCell13";
            this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell13.Weight = 0.13793982329437432D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Summary.Client]")});
            this.xrTableCell14.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell14.Multiline = true;
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell14.StyleName = "DetailData1";
            this.xrTableCell14.StylePriority.UseFont = false;
            this.xrTableCell14.StylePriority.UseTextAlignment = false;
            this.xrTableCell14.Text = "xrTableCell14";
            this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell14.Weight = 0.06968256185939059D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Summary.ClientService]")});
            this.xrTableCell15.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell15.Multiline = true;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell15.StyleName = "DetailData1";
            this.xrTableCell15.StylePriority.UseFont = false;
            this.xrTableCell15.StylePriority.UseTextAlignment = false;
            this.xrTableCell15.Text = "xrTableCell15";
            this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell15.Weight = 0.12793748548583195D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Summary.TotalPOB]")});
            this.xrTableCell16.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell16.Multiline = true;
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell16.StyleName = "DetailData1";
            this.xrTableCell16.StylePriority.UseFont = false;
            this.xrTableCell16.StylePriority.UseTextAlignment = false;
            this.xrTableCell16.Text = "xrTableCell16";
            this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell16.Weight = 0.09629210088010802D;
            // 
            // xrLabel13
            // 
            this.xrLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(109)))), ((int)(((byte)(31)))));
            this.xrLabel13.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[RigName]")});
            this.xrLabel13.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel13.ForeColor = System.Drawing.Color.White;
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(121.3051F, 0F);
            this.xrLabel13.Multiline = true;
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(144.1811F, 23F);
            this.xrLabel13.StylePriority.UseBackColor = false;
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseForeColor = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.Text = "PoB Summary: ";
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel15
            // 
            this.xrLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(109)))), ((int)(((byte)(31)))));
            this.xrLabel15.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel15.ForeColor = System.Drawing.Color.White;
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel15.Multiline = true;
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(121.3049F, 23F);
            this.xrLabel15.StylePriority.UseBackColor = false;
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UseForeColor = false;
            this.xrLabel15.StylePriority.UseTextAlignment = false;
            this.xrLabel15.Text = "PoB Summary:";
            this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 49F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Arial", 8.150944F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(313F, 23F);
            this.xrPageInfo1.StyleName = "PageInfo";
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Font = new System.Drawing.Font("Arial", 8.150944F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(438F, 0F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(313F, 23F);
            this.xrPageInfo2.StyleName = "PageInfo";
            this.xrPageInfo2.StylePriority.UseFont = false;
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrPageInfo2.TextFormatString = "Page {0} of {1}";
            // 
            // reportHeaderBand1
            // 
            this.reportHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel6});
            this.reportHeaderBand1.HeightF = 75.09438F;
            this.reportHeaderBand1.Name = "reportHeaderBand1";
            // 
            // xrPanel6
            // 
            this.xrPanel6.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPanel6.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1,
            this.xrLabel1,
            this.xrLabel2,
            this.xrPageInfo4,
            this.xrPictureBox2});
            this.xrPanel6.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPanel6.Name = "xrPanel6";
            this.xrPanel6.SizeF = new System.Drawing.SizeF(998.9999F, 64.62264F);
            this.xrPanel6.StylePriority.UseBorders = false;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPictureBox1.ImageUrl = "C:\\Workspace\\IRMA\\Development\\EnscoApps\\Ensco.App\\Ensco.App\\Images\\ensco.png";
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(10F, 4.830155F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(128.1132F, 55.0755F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.xrPictureBox1.StylePriority.UseBorders = false;
            this.xrPictureBox1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrPictureBox1_BeforePrint);
            // 
            // xrLabel1
            // 
            this.xrLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(109)))), ((int)(((byte)(31)))));
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 14.26415F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.ForeColor = System.Drawing.Color.White;
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(138.1132F, 4.830155F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(715.0671F, 25.55338F);
            this.xrLabel1.StyleName = "Title";
            this.xrLabel1.StylePriority.UseBackColor = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseForeColor = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "PoB Summary";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Parameters].[RigName]")});
            this.xrLabel2.Font = new System.Drawing.Font("Arial", 12.22642F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(138.1132F, 31.01884F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(242.1887F, 28.88681F);
            this.xrLabel2.StylePriority.UseBorders = false;
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPageInfo4
            // 
            this.xrPageInfo4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPageInfo4.Font = new System.Drawing.Font("Arial", 12.22642F, System.Drawing.FontStyle.Bold);
            this.xrPageInfo4.LocationFloat = new DevExpress.Utils.PointFloat(696.0566F, 31.01884F);
            this.xrPageInfo4.Name = "xrPageInfo4";
            this.xrPageInfo4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo4.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo4.SizeF = new System.Drawing.SizeF(157.1237F, 28.88681F);
            this.xrPageInfo4.StylePriority.UseBorders = false;
            this.xrPageInfo4.StylePriority.UseFont = false;
            this.xrPageInfo4.StylePriority.UseTextAlignment = false;
            this.xrPageInfo4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrPageInfo4.TextFormatString = "{0:dd-MMM-yyyy}";
            // 
            // xrPictureBox2
            // 
            this.xrPictureBox2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPictureBox2.ImageUrl = "C:\\Workspace\\IRMA\\Development\\EnscoApps\\Ensco.App\\Ensco.App\\Images\\irma.png";
            this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(853.1802F, 4.830155F);
            this.xrPictureBox2.Name = "xrPictureBox2";
            this.xrPictureBox2.SizeF = new System.Drawing.SizeF(135.8197F, 55.0755F);
            this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.xrPictureBox2.StylePriority.UseBorders = false;
            this.xrPictureBox2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrPictureBox2_BeforePrint);
            // 
            // detailReportBand1
            // 
            this.detailReportBand1.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.groupHeaderBand1,
            this.groupHeaderBand2,
            this.detailBand1,
            this.groupFooterBand1});
            this.detailReportBand1.DataMember = "SummaryByCompanyType";
            this.detailReportBand1.DataSource = this.objectDataSource1;
            this.detailReportBand1.Level = 0;
            this.detailReportBand1.Name = "detailReportBand1";
            // 
            // groupHeaderBand1
            // 
            this.groupHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.xrTable2});
            this.groupHeaderBand1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("CompanyType", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.groupHeaderBand1.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WithFirstDetail;
            this.groupHeaderBand1.HeightF = 25F;
            this.groupHeaderBand1.Level = 2;
            this.groupHeaderBand1.Name = "groupHeaderBand1";
            // 
            // xrLabel3
            // 
            this.xrLabel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(183)))), ((int)(((byte)(228)))));
            this.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLabel3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumCount([Id])")});
            this.xrLabel3.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.ForeColor = System.Drawing.Color.White;
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(143.9466F, 0F);
            this.xrLabel3.Multiline = true;
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(857.0535F, 25F);
            this.xrLabel3.StylePriority.UseBackColor = false;
            this.xrLabel3.StylePriority.UseBorders = false;
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseForeColor = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrLabel3.Summary = xrSummary1;
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel3.TextFormatString = "({0})";
            // 
            // xrTable2
            // 
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
            this.xrTable2.SizeF = new System.Drawing.SizeF(143.9466F, 25F);
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell18});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(183)))), ((int)(((byte)(228)))));
            this.xrTableCell18.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell18.BorderWidth = 1F;
            this.xrTableCell18.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CompanyType]")});
            this.xrTableCell18.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell18.Multiline = true;
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell18.StyleName = "GroupData3";
            this.xrTableCell18.StylePriority.UseBackColor = false;
            this.xrTableCell18.StylePriority.UseBorders = false;
            this.xrTableCell18.StylePriority.UseBorderWidth = false;
            this.xrTableCell18.StylePriority.UseFont = false;
            this.xrTableCell18.Text = "xrTableCell18";
            this.xrTableCell18.Weight = 1.4626696398915653D;
            // 
            // groupHeaderBand2
            // 
            this.groupHeaderBand2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel1});
            this.groupHeaderBand2.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WithFirstDetail;
            this.groupHeaderBand2.HeightF = 48F;
            this.groupHeaderBand2.Level = 1;
            this.groupHeaderBand2.Name = "groupHeaderBand2";
            // 
            // xrPanel1
            // 
            this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3});
            this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPanel1.Name = "xrPanel1";
            this.xrPanel1.SizeF = new System.Drawing.SizeF(1001F, 48F);
            this.xrPanel1.StyleName = "DetailCaptionBackground3";
            // 
            // xrTable3
            // 
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 20.00001F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
            this.xrTable3.SizeF = new System.Drawing.SizeF(1001F, 28F);
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell19,
            this.xrTableCell20,
            this.xrTableCell21,
            this.xrTableCell22,
            this.xrTableCell23,
            this.xrTableCell24,
            this.xrTableCell25,
            this.xrTableCell26,
            this.xrTableCell27,
            this.xrTableCellVan,
            this.xrTableCellEss});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell19.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell19.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell19.Multiline = true;
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell19.StyleName = "DetailCaption3";
            this.xrTableCell19.StylePriority.UseBackColor = false;
            this.xrTableCell19.StylePriority.UseFont = false;
            this.xrTableCell19.StylePriority.UseForeColor = false;
            this.xrTableCell19.StylePriority.UseTextAlignment = false;
            this.xrTableCell19.Text = "Company";
            this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell19.Weight = 0.10147447072090514D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell20.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell20.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell20.Multiline = true;
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell20.StyleName = "DetailCaption3";
            this.xrTableCell20.StylePriority.UseBackColor = false;
            this.xrTableCell20.StylePriority.UseFont = false;
            this.xrTableCell20.StylePriority.UseForeColor = false;
            this.xrTableCell20.StylePriority.UseTextAlignment = false;
            this.xrTableCell20.Text = "Name";
            this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell20.Weight = 0.11827441451142136D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell21.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell21.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell21.Multiline = true;
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell21.StyleName = "DetailCaption3";
            this.xrTableCell21.StylePriority.UseBackColor = false;
            this.xrTableCell21.StylePriority.UseFont = false;
            this.xrTableCell21.StylePriority.UseForeColor = false;
            this.xrTableCell21.StylePriority.UseTextAlignment = false;
            this.xrTableCell21.Text = "Position";
            this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell21.Weight = 0.10946067354880579D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell22.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell22.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell22.Multiline = true;
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell22.StyleName = "DetailCaption3";
            this.xrTableCell22.StylePriority.UseBackColor = false;
            this.xrTableCell22.StylePriority.UseFont = false;
            this.xrTableCell22.StylePriority.UseForeColor = false;
            this.xrTableCell22.StylePriority.UseTextAlignment = false;
            this.xrTableCell22.Text = "Nationality";
            this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell22.Weight = 0.10727959638924686D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell23.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell23.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell23.Multiline = true;
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell23.StyleName = "DetailCaption3";
            this.xrTableCell23.StylePriority.UseBackColor = false;
            this.xrTableCell23.StylePriority.UseFont = false;
            this.xrTableCell23.StylePriority.UseForeColor = false;
            this.xrTableCell23.StylePriority.UseTextAlignment = false;
            this.xrTableCell23.Text = "Tour";
            this.xrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell23.Weight = 0.069948094959388057D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell24.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell24.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell24.Multiline = true;
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell24.StyleName = "DetailCaption3";
            this.xrTableCell24.StylePriority.UseBackColor = false;
            this.xrTableCell24.StylePriority.UseFont = false;
            this.xrTableCell24.StylePriority.UseForeColor = false;
            this.xrTableCell24.StylePriority.UseTextAlignment = false;
            this.xrTableCell24.Text = "Cabin";
            this.xrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell24.Weight = 0.061710934228235752D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell25.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell25.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell25.Multiline = true;
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell25.StyleName = "DetailCaption3";
            this.xrTableCell25.StylePriority.UseBackColor = false;
            this.xrTableCell25.StylePriority.UseFont = false;
            this.xrTableCell25.StylePriority.UseForeColor = false;
            this.xrTableCell25.StylePriority.UseTextAlignment = false;
            this.xrTableCell25.Text = "L /B";
            this.xrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell25.Weight = 0.07950770985861863D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell26.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell26.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell26.Multiline = true;
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell26.StyleName = "DetailCaption3";
            this.xrTableCell26.StylePriority.UseBackColor = false;
            this.xrTableCell26.StylePriority.UseFont = false;
            this.xrTableCell26.StylePriority.UseForeColor = false;
            this.xrTableCell26.StylePriority.UseTextAlignment = false;
            this.xrTableCell26.Text = "On";
            this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell26.Weight = 0.1604951440242077D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell27.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell27.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell27.Multiline = true;
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell27.StyleName = "DetailCaption3";
            this.xrTableCell27.StylePriority.UseBackColor = false;
            this.xrTableCell27.StylePriority.UseFont = false;
            this.xrTableCell27.StylePriority.UseForeColor = false;
            this.xrTableCell27.StylePriority.UseTextAlignment = false;
            this.xrTableCell27.Text = "DoB";
            this.xrTableCell27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell27.Weight = 0.095709062770755027D;
            // 
            // xrTableCellVan
            // 
            this.xrTableCellVan.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCellVan.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([Parameters].[ShowVantage], \'Van.\', \'\')")});
            this.xrTableCellVan.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCellVan.ForeColor = System.Drawing.Color.Black;
            this.xrTableCellVan.Multiline = true;
            this.xrTableCellVan.Name = "xrTableCellVan";
            this.xrTableCellVan.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCellVan.StyleName = "DetailCaption3";
            this.xrTableCellVan.StylePriority.UseBackColor = false;
            this.xrTableCellVan.StylePriority.UseFont = false;
            this.xrTableCellVan.StylePriority.UseForeColor = false;
            this.xrTableCellVan.StylePriority.UseTextAlignment = false;
            this.xrTableCellVan.Text = "Van.";
            this.xrTableCellVan.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCellVan.Weight = 0.04229013675826146D;
            // 
            // xrTableCellEss
            // 
            this.xrTableCellEss.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCellEss.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([Parameters].[ShowEssential], \'Ess.\',\'\' )")});
            this.xrTableCellEss.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCellEss.ForeColor = System.Drawing.Color.Black;
            this.xrTableCellEss.Multiline = true;
            this.xrTableCellEss.Name = "xrTableCellEss";
            this.xrTableCellEss.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCellEss.StyleName = "DetailCaption3";
            this.xrTableCellEss.StylePriority.UseBackColor = false;
            this.xrTableCellEss.StylePriority.UseFont = false;
            this.xrTableCellEss.StylePriority.UseForeColor = false;
            this.xrTableCellEss.StylePriority.UseTextAlignment = false;
            this.xrTableCellEss.Text = "Ess.";
            this.xrTableCellEss.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCellEss.Weight = 0.053849815049039476D;
            // 
            // detailBand1
            // 
            this.detailBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4});
            this.detailBand1.HeightF = 25F;
            this.detailBand1.Name = "detailBand1";
            // 
            // xrTable4
            // 
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.OddStyleName = "DetailData3_Odd";
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow5});
            this.xrTable4.SizeF = new System.Drawing.SizeF(1001F, 25F);
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell30,
            this.xrTableCell31,
            this.xrTableCell32,
            this.xrTableCell33,
            this.xrTableCell34,
            this.xrTableCell35,
            this.xrTableCell36,
            this.xrTableCell37,
            this.xrTableCell38,
            this.xrTableCell39,
            this.xrTableCell40});
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 11.5D;
            // 
            // xrTableCell30
            // 
            this.xrTableCell30.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Company]")});
            this.xrTableCell30.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell30.Multiline = true;
            this.xrTableCell30.Name = "xrTableCell30";
            this.xrTableCell30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell30.StyleName = "DetailData3";
            this.xrTableCell30.StylePriority.UseFont = false;
            this.xrTableCell30.Text = "xrTableCell30";
            this.xrTableCell30.Weight = 0.10147446745692704D;
            // 
            // xrTableCell31
            // 
            this.xrTableCell31.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Name]")});
            this.xrTableCell31.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell31.Multiline = true;
            this.xrTableCell31.Name = "xrTableCell31";
            this.xrTableCell31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell31.StyleName = "DetailData3";
            this.xrTableCell31.StylePriority.UseFont = false;
            this.xrTableCell31.Text = "xrTableCell31";
            this.xrTableCell31.Weight = 0.11827441249537921D;
            // 
            // xrTableCell32
            // 
            this.xrTableCell32.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Position]")});
            this.xrTableCell32.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell32.Multiline = true;
            this.xrTableCell32.Name = "xrTableCell32";
            this.xrTableCell32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell32.StyleName = "DetailData3";
            this.xrTableCell32.StylePriority.UseFont = false;
            this.xrTableCell32.Text = "xrTableCell32";
            this.xrTableCell32.Weight = 0.109460664728535D;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Nationality]")});
            this.xrTableCell33.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell33.Multiline = true;
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell33.StyleName = "DetailData3";
            this.xrTableCell33.StylePriority.UseFont = false;
            this.xrTableCell33.Text = "xrTableCell33";
            this.xrTableCell33.Weight = 0.11733786405233398D;
            // 
            // xrTableCell34
            // 
            this.xrTableCell34.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Tour]")});
            this.xrTableCell34.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell34.Multiline = true;
            this.xrTableCell34.Name = "xrTableCell34";
            this.xrTableCell34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell34.StyleName = "DetailData3";
            this.xrTableCell34.StylePriority.UseFont = false;
            this.xrTableCell34.Text = "xrTableCell34";
            this.xrTableCell34.TextFormatString = "{0:dd-MMM-yy}";
            this.xrTableCell34.Weight = 0.059889818288243614D;
            // 
            // xrTableCell35
            // 
            this.xrTableCell35.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Cabin]")});
            this.xrTableCell35.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell35.Multiline = true;
            this.xrTableCell35.Name = "xrTableCell35";
            this.xrTableCell35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell35.StyleName = "DetailData3";
            this.xrTableCell35.StylePriority.UseFont = false;
            this.xrTableCell35.Text = "xrTableCell35";
            this.xrTableCell35.TextFormatString = "{0:dd-MMM-yy}";
            this.xrTableCell35.Weight = 0.0617109320140579D;
            // 
            // xrTableCell36
            // 
            this.xrTableCell36.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LifeBoat]")});
            this.xrTableCell36.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell36.Multiline = true;
            this.xrTableCell36.Name = "xrTableCell36";
            this.xrTableCell36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell36.StyleName = "DetailData3";
            this.xrTableCell36.StylePriority.UseFont = false;
            this.xrTableCell36.Text = "xrTableCell36";
            this.xrTableCell36.Weight = 0.079507714998902246D;
            // 
            // xrTableCell37
            // 
            this.xrTableCell37.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[On]")});
            this.xrTableCell37.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell37.Multiline = true;
            this.xrTableCell37.Name = "xrTableCell37";
            this.xrTableCell37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell37.StyleName = "DetailData3";
            this.xrTableCell37.StylePriority.UseFont = false;
            this.xrTableCell37.Text = "xrTableCell37";
            this.xrTableCell37.Weight = 0.16049501855600307D;
            // 
            // xrTableCell38
            // 
            this.xrTableCell38.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DoB]")});
            this.xrTableCell38.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell38.Multiline = true;
            this.xrTableCell38.Name = "xrTableCell38";
            this.xrTableCell38.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell38.StyleName = "DetailData3";
            this.xrTableCell38.StylePriority.UseFont = false;
            this.xrTableCell38.Text = "xrTableCell38";
            this.xrTableCell38.TextFormatString = "{0:dd-MMM-yyyy}";
            this.xrTableCell38.Weight = 0.095557003999881651D;
            // 
            // xrTableCell39
            // 
            this.xrTableCell39.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([Parameters].[ShowVantage], [Van], \'\')")});
            this.xrTableCell39.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell39.Multiline = true;
            this.xrTableCell39.Name = "xrTableCell39";
            this.xrTableCell39.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell39.StyleName = "DetailData3";
            this.xrTableCell39.StylePriority.UseFont = false;
            this.xrTableCell39.Text = "xrTableCell39";
            this.xrTableCell39.Weight = 0.042442265284188069D;
            // 
            // xrTableCell40
            // 
            this.xrTableCell40.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([Parameters].[ShowEssential], [Ess], \'\')")});
            this.xrTableCell40.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell40.Multiline = true;
            this.xrTableCell40.Name = "xrTableCell40";
            this.xrTableCell40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell40.StyleName = "DetailData3";
            this.xrTableCell40.StylePriority.UseFont = false;
            this.xrTableCell40.Text = "xrTableCell40";
            this.xrTableCell40.Weight = 0.05384980878172315D;
            // 
            // groupFooterBand1
            // 
            this.groupFooterBand1.GroupUnion = DevExpress.XtraReports.UI.GroupFooterUnion.WithLastDetail;
            this.groupFooterBand1.HeightF = 6F;
            this.groupFooterBand1.Name = "groupFooterBand1";
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(Ensco.Irma.Models.PobSummaryReportModel);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // detailReportBand2
            // 
            this.detailReportBand2.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.groupHeaderBand3,
            this.detailBand2});
            this.detailReportBand2.DataMember = "ArrivalLog";
            this.detailReportBand2.DataSource = this.objectDataSource1;
            this.detailReportBand2.Level = 1;
            this.detailReportBand2.Name = "detailReportBand2";
            // 
            // groupHeaderBand3
            // 
            this.groupHeaderBand3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageBreak1,
            this.xrPanel2});
            this.groupHeaderBand3.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WithFirstDetail;
            this.groupHeaderBand3.HeightF = 71.58491F;
            this.groupHeaderBand3.Name = "groupHeaderBand3";
            // 
            // xrPageBreak1
            // 
            this.xrPageBreak1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPageBreak1.Name = "xrPageBreak1";
            // 
            // xrPanel2
            // 
            this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel5,
            this.xrLabel4,
            this.xrTable5});
            this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPanel2.Name = "xrPanel2";
            this.xrPanel2.SizeF = new System.Drawing.SizeF(1001F, 71.58491F);
            this.xrPanel2.StyleName = "DetailCaptionBackground3";
            // 
            // xrLabel5
            // 
            this.xrLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(127)))));
            this.xrLabel5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumCount([Id])")});
            this.xrLabel5.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel5.ForeColor = System.Drawing.Color.White;
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(156.6603F, 10.00001F);
            this.xrLabel5.Multiline = true;
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(844.3398F, 23F);
            this.xrLabel5.StylePriority.UseBackColor = false;
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseForeColor = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrLabel5.Summary = xrSummary2;
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel4
            // 
            this.xrLabel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(127)))));
            this.xrLabel4.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.ForeColor = System.Drawing.Color.White;
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
            this.xrLabel4.Multiline = true;
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(156.6603F, 23F);
            this.xrLabel4.StylePriority.UseBackColor = false;
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseForeColor = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "Arrival Last 24 Hours: ";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTable5
            // 
            this.xrTable5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 33.58488F);
            this.xrTable5.Name = "xrTable5";
            this.xrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
            this.xrTable5.SizeF = new System.Drawing.SizeF(1001F, 28F);
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell41,
            this.xrTableCell42,
            this.xrTableCell43,
            this.xrTableCell44,
            this.xrTableCell45});
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // xrTableCell41
            // 
            this.xrTableCell41.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell41.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell41.Multiline = true;
            this.xrTableCell41.Name = "xrTableCell41";
            this.xrTableCell41.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell41.StyleName = "DetailCaption3";
            this.xrTableCell41.StylePriority.UseBackColor = false;
            this.xrTableCell41.StylePriority.UseFont = false;
            this.xrTableCell41.Text = "Name";
            this.xrTableCell41.Weight = 0.15454115647536057D;
            // 
            // xrTableCell42
            // 
            this.xrTableCell42.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell42.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell42.Multiline = true;
            this.xrTableCell42.Name = "xrTableCell42";
            this.xrTableCell42.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell42.StyleName = "DetailCaption3";
            this.xrTableCell42.StylePriority.UseBackColor = false;
            this.xrTableCell42.StylePriority.UseFont = false;
            this.xrTableCell42.Text = "Position";
            this.xrTableCell42.Weight = 0.19896906926081731D;
            // 
            // xrTableCell43
            // 
            this.xrTableCell43.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell43.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell43.Multiline = true;
            this.xrTableCell43.Name = "xrTableCell43";
            this.xrTableCell43.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell43.StyleName = "DetailCaption3";
            this.xrTableCell43.StylePriority.UseBackColor = false;
            this.xrTableCell43.StylePriority.UseFont = false;
            this.xrTableCell43.Text = "Company Type";
            this.xrTableCell43.Weight = 0.3273814039963942D;
            // 
            // xrTableCell44
            // 
            this.xrTableCell44.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell44.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell44.Multiline = true;
            this.xrTableCell44.Name = "xrTableCell44";
            this.xrTableCell44.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell44.StyleName = "DetailCaption3";
            this.xrTableCell44.StylePriority.UseBackColor = false;
            this.xrTableCell44.StylePriority.UseFont = false;
            this.xrTableCell44.Text = "Company";
            this.xrTableCell44.Weight = 0.22281625600961538D;
            // 
            // xrTableCell45
            // 
            this.xrTableCell45.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell45.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell45.Multiline = true;
            this.xrTableCell45.Name = "xrTableCell45";
            this.xrTableCell45.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell45.StyleName = "DetailCaption3";
            this.xrTableCell45.StylePriority.UseBackColor = false;
            this.xrTableCell45.StylePriority.UseFont = false;
            this.xrTableCell45.Text = "Onboard";
            this.xrTableCell45.Weight = 0.096292067307692311D;
            // 
            // detailBand2
            // 
            this.detailBand2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable6});
            this.detailBand2.HeightF = 25F;
            this.detailBand2.Name = "detailBand2";
            // 
            // xrTable6
            // 
            this.xrTable6.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable6.Name = "xrTable6";
            this.xrTable6.OddStyleName = "DetailData3_Odd";
            this.xrTable6.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7});
            this.xrTable6.SizeF = new System.Drawing.SizeF(1001F, 25F);
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell46,
            this.xrTableCell47,
            this.xrTableCell48,
            this.xrTableCell49,
            this.xrTableCell50});
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Weight = 11.5D;
            // 
            // xrTableCell46
            // 
            this.xrTableCell46.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Name]")});
            this.xrTableCell46.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell46.Multiline = true;
            this.xrTableCell46.Name = "xrTableCell46";
            this.xrTableCell46.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell46.StyleName = "DetailData3";
            this.xrTableCell46.StylePriority.UseFont = false;
            this.xrTableCell46.Text = "xrTableCell46";
            this.xrTableCell46.Weight = 0.15454116821289063D;
            // 
            // xrTableCell47
            // 
            this.xrTableCell47.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Position]")});
            this.xrTableCell47.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell47.Multiline = true;
            this.xrTableCell47.Name = "xrTableCell47";
            this.xrTableCell47.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell47.StyleName = "DetailData3";
            this.xrTableCell47.StylePriority.UseFont = false;
            this.xrTableCell47.Text = "xrTableCell47";
            this.xrTableCell47.Weight = 0.19896906926081731D;
            // 
            // xrTableCell48
            // 
            this.xrTableCell48.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CompanyType]")});
            this.xrTableCell48.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell48.Multiline = true;
            this.xrTableCell48.Name = "xrTableCell48";
            this.xrTableCell48.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell48.StyleName = "DetailData3";
            this.xrTableCell48.StylePriority.UseFont = false;
            this.xrTableCell48.Text = "xrTableCell48";
            this.xrTableCell48.Weight = 0.32738142747145432D;
            // 
            // xrTableCell49
            // 
            this.xrTableCell49.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Company]")});
            this.xrTableCell49.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell49.Multiline = true;
            this.xrTableCell49.Name = "xrTableCell49";
            this.xrTableCell49.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell49.StyleName = "DetailData3";
            this.xrTableCell49.StylePriority.UseFont = false;
            this.xrTableCell49.Text = "xrTableCell49";
            this.xrTableCell49.Weight = 0.22281625600961538D;
            // 
            // xrTableCell50
            // 
            this.xrTableCell50.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[On]")});
            this.xrTableCell50.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell50.Multiline = true;
            this.xrTableCell50.Name = "xrTableCell50";
            this.xrTableCell50.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell50.StyleName = "DetailData3";
            this.xrTableCell50.StylePriority.UseFont = false;
            this.xrTableCell50.Text = "xrTableCell50";
            this.xrTableCell50.Weight = 0.096292067307692311D;
            // 
            // detailReportBand3
            // 
            this.detailReportBand3.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.groupHeaderBand4,
            this.detailBand3});
            this.detailReportBand3.DataMember = "DepartureLog";
            this.detailReportBand3.DataSource = this.objectDataSource1;
            this.detailReportBand3.Level = 2;
            this.detailReportBand3.Name = "detailReportBand3";
            // 
            // groupHeaderBand4
            // 
            this.groupHeaderBand4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel3});
            this.groupHeaderBand4.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WithFirstDetail;
            this.groupHeaderBand4.HeightF = 63.09435F;
            this.groupHeaderBand4.Name = "groupHeaderBand4";
            // 
            // xrPanel3
            // 
            this.xrPanel3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable7,
            this.xrLabel6,
            this.xrLabel7});
            this.xrPanel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPanel3.Name = "xrPanel3";
            this.xrPanel3.SizeF = new System.Drawing.SizeF(1001F, 63.09435F);
            this.xrPanel3.StyleName = "DetailCaptionBackground3";
            // 
            // xrTable7
            // 
            this.xrTable7.LocationFloat = new DevExpress.Utils.PointFloat(0F, 35.09435F);
            this.xrTable7.Name = "xrTable7";
            this.xrTable7.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow8});
            this.xrTable7.SizeF = new System.Drawing.SizeF(1001F, 28.00001F);
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell51,
            this.xrTableCell52,
            this.xrTableCell53,
            this.xrTableCell54,
            this.xrTableCell55});
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Weight = 1D;
            // 
            // xrTableCell51
            // 
            this.xrTableCell51.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell51.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell51.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell51.Multiline = true;
            this.xrTableCell51.Name = "xrTableCell51";
            this.xrTableCell51.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell51.StyleName = "DetailCaption3";
            this.xrTableCell51.StylePriority.UseBackColor = false;
            this.xrTableCell51.StylePriority.UseBorderColor = false;
            this.xrTableCell51.StylePriority.UseFont = false;
            this.xrTableCell51.Text = "Name";
            this.xrTableCell51.Weight = 0.15454115647536057D;
            // 
            // xrTableCell52
            // 
            this.xrTableCell52.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell52.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell52.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell52.Multiline = true;
            this.xrTableCell52.Name = "xrTableCell52";
            this.xrTableCell52.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell52.StyleName = "DetailCaption3";
            this.xrTableCell52.StylePriority.UseBackColor = false;
            this.xrTableCell52.StylePriority.UseBorderColor = false;
            this.xrTableCell52.StylePriority.UseFont = false;
            this.xrTableCell52.Text = "Position";
            this.xrTableCell52.Weight = 0.19896906926081731D;
            // 
            // xrTableCell53
            // 
            this.xrTableCell53.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell53.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell53.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell53.Multiline = true;
            this.xrTableCell53.Name = "xrTableCell53";
            this.xrTableCell53.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell53.StyleName = "DetailCaption3";
            this.xrTableCell53.StylePriority.UseBackColor = false;
            this.xrTableCell53.StylePriority.UseBorderColor = false;
            this.xrTableCell53.StylePriority.UseFont = false;
            this.xrTableCell53.Text = "Company Type";
            this.xrTableCell53.Weight = 0.3273814039963942D;
            // 
            // xrTableCell54
            // 
            this.xrTableCell54.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell54.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell54.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell54.Multiline = true;
            this.xrTableCell54.Name = "xrTableCell54";
            this.xrTableCell54.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell54.StyleName = "DetailCaption3";
            this.xrTableCell54.StylePriority.UseBackColor = false;
            this.xrTableCell54.StylePriority.UseBorderColor = false;
            this.xrTableCell54.StylePriority.UseFont = false;
            this.xrTableCell54.Text = "Company";
            this.xrTableCell54.Weight = 0.22281625600961538D;
            // 
            // xrTableCell55
            // 
            this.xrTableCell55.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell55.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell55.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell55.Multiline = true;
            this.xrTableCell55.Name = "xrTableCell55";
            this.xrTableCell55.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell55.StyleName = "DetailCaption3";
            this.xrTableCell55.StylePriority.UseBackColor = false;
            this.xrTableCell55.StylePriority.UseBorderColor = false;
            this.xrTableCell55.StylePriority.UseFont = false;
            this.xrTableCell55.Text = "Offboard";
            this.xrTableCell55.Weight = 0.096292067307692311D;
            // 
            // xrLabel6
            // 
            this.xrLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(127)))));
            this.xrLabel6.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel6.ForeColor = System.Drawing.Color.White;
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(0F, 9.999977F);
            this.xrLabel6.Multiline = true;
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(176.4716F, 23F);
            this.xrLabel6.StylePriority.UseBackColor = false;
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseForeColor = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "Departure Last 24 Hours: ";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel7
            // 
            this.xrLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(127)))));
            this.xrLabel7.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumCount([Id])")});
            this.xrLabel7.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel7.ForeColor = System.Drawing.Color.White;
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(176.4716F, 9.999977F);
            this.xrLabel7.Multiline = true;
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(824.5284F, 23F);
            this.xrLabel7.StylePriority.UseBackColor = false;
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseForeColor = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrLabel7.Summary = xrSummary3;
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // detailBand3
            // 
            this.detailBand3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable8});
            this.detailBand3.HeightF = 25F;
            this.detailBand3.Name = "detailBand3";
            // 
            // xrTable8
            // 
            this.xrTable8.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable8.Name = "xrTable8";
            this.xrTable8.OddStyleName = "DetailData3_Odd";
            this.xrTable8.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow9});
            this.xrTable8.SizeF = new System.Drawing.SizeF(1001F, 25F);
            // 
            // xrTableRow9
            // 
            this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell56,
            this.xrTableCell57,
            this.xrTableCell58,
            this.xrTableCell59,
            this.xrTableCell60});
            this.xrTableRow9.Name = "xrTableRow9";
            this.xrTableRow9.Weight = 11.5D;
            // 
            // xrTableCell56
            // 
            this.xrTableCell56.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Name]")});
            this.xrTableCell56.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell56.Multiline = true;
            this.xrTableCell56.Name = "xrTableCell56";
            this.xrTableCell56.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell56.StyleName = "DetailData3";
            this.xrTableCell56.StylePriority.UseFont = false;
            this.xrTableCell56.Text = "xrTableCell56";
            this.xrTableCell56.Weight = 0.15454116821289063D;
            // 
            // xrTableCell57
            // 
            this.xrTableCell57.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Position]")});
            this.xrTableCell57.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell57.Multiline = true;
            this.xrTableCell57.Name = "xrTableCell57";
            this.xrTableCell57.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell57.StyleName = "DetailData3";
            this.xrTableCell57.StylePriority.UseFont = false;
            this.xrTableCell57.Text = "xrTableCell57";
            this.xrTableCell57.Weight = 0.19896906926081731D;
            // 
            // xrTableCell58
            // 
            this.xrTableCell58.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CompanyType]")});
            this.xrTableCell58.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell58.Multiline = true;
            this.xrTableCell58.Name = "xrTableCell58";
            this.xrTableCell58.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell58.StyleName = "DetailData3";
            this.xrTableCell58.StylePriority.UseFont = false;
            this.xrTableCell58.Text = "xrTableCell58";
            this.xrTableCell58.Weight = 0.32738142747145432D;
            // 
            // xrTableCell59
            // 
            this.xrTableCell59.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Company]")});
            this.xrTableCell59.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell59.Multiline = true;
            this.xrTableCell59.Name = "xrTableCell59";
            this.xrTableCell59.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell59.StyleName = "DetailData3";
            this.xrTableCell59.StylePriority.UseFont = false;
            this.xrTableCell59.Text = "xrTableCell59";
            this.xrTableCell59.Weight = 0.22281625600961538D;
            // 
            // xrTableCell60
            // 
            this.xrTableCell60.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Off]")});
            this.xrTableCell60.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell60.Multiline = true;
            this.xrTableCell60.Name = "xrTableCell60";
            this.xrTableCell60.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell60.StyleName = "DetailData3";
            this.xrTableCell60.StylePriority.UseFont = false;
            this.xrTableCell60.Text = "xrTableCell60";
            this.xrTableCell60.Weight = 0.096292067307692311D;
            // 
            // detailReportBand4
            // 
            this.detailReportBand4.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.groupHeaderBand5,
            this.detailBand4});
            this.detailReportBand4.DataMember = "CrewBreakdown";
            this.detailReportBand4.DataSource = this.objectDataSource1;
            this.detailReportBand4.Level = 3;
            this.detailReportBand4.Name = "detailReportBand4";
            // 
            // groupHeaderBand5
            // 
            this.groupHeaderBand5.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel4});
            this.groupHeaderBand5.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WithFirstDetail;
            this.groupHeaderBand5.HeightF = 60.26416F;
            this.groupHeaderBand5.Name = "groupHeaderBand5";
            // 
            // xrPanel4
            // 
            this.xrPanel4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable9,
            this.xrLabel8});
            this.xrPanel4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPanel4.Name = "xrPanel4";
            this.xrPanel4.SizeF = new System.Drawing.SizeF(1001F, 58.75475F);
            this.xrPanel4.StyleName = "DetailCaptionBackground3";
            // 
            // xrTable9
            // 
            this.xrTable9.LocationFloat = new DevExpress.Utils.PointFloat(0F, 32.99998F);
            this.xrTable9.Name = "xrTable9";
            this.xrTable9.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow10});
            this.xrTable9.SizeF = new System.Drawing.SizeF(1001F, 28F);
            // 
            // xrTableRow10
            // 
            this.xrTableRow10.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell61,
            this.xrTableCell62});
            this.xrTableRow10.Name = "xrTableRow10";
            this.xrTableRow10.Weight = 1D;
            // 
            // xrTableCell61
            // 
            this.xrTableCell61.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell61.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell61.Multiline = true;
            this.xrTableCell61.Name = "xrTableCell61";
            this.xrTableCell61.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell61.StyleName = "DetailCaption3";
            this.xrTableCell61.StylePriority.UseBackColor = false;
            this.xrTableCell61.StylePriority.UseFont = false;
            this.xrTableCell61.Text = "Name";
            this.xrTableCell61.Weight = 0.52434391902043265D;
            // 
            // xrTableCell62
            // 
            this.xrTableCell62.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell62.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell62.Multiline = true;
            this.xrTableCell62.Name = "xrTableCell62";
            this.xrTableCell62.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell62.StyleName = "DetailCaption3";
            this.xrTableCell62.StylePriority.UseBackColor = false;
            this.xrTableCell62.StylePriority.UseFont = false;
            this.xrTableCell62.StylePriority.UseTextAlignment = false;
            this.xrTableCell62.Text = "Total";
            this.xrTableCell62.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell62.Weight = 0.4756560809795673D;
            // 
            // xrLabel8
            // 
            this.xrLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(127)))));
            this.xrLabel8.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel8.ForeColor = System.Drawing.Color.White;
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(0F, 9.999977F);
            this.xrLabel8.Multiline = true;
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(1001F, 23F);
            this.xrLabel8.StylePriority.UseBackColor = false;
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseForeColor = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "Crew Breakdown";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // detailBand4
            // 
            this.detailBand4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable10});
            this.detailBand4.HeightF = 25F;
            this.detailBand4.Name = "detailBand4";
            // 
            // xrTable10
            // 
            this.xrTable10.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable10.Name = "xrTable10";
            this.xrTable10.OddStyleName = "DetailData3_Odd";
            this.xrTable10.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow11});
            this.xrTable10.SizeF = new System.Drawing.SizeF(1001F, 25F);
            // 
            // xrTableRow11
            // 
            this.xrTableRow11.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell63,
            this.xrTableCell64});
            this.xrTableRow11.Name = "xrTableRow11";
            this.xrTableRow11.Weight = 11.5D;
            // 
            // xrTableCell63
            // 
            this.xrTableCell63.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Name]")});
            this.xrTableCell63.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell63.Multiline = true;
            this.xrTableCell63.Name = "xrTableCell63";
            this.xrTableCell63.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell63.StyleName = "DetailData3";
            this.xrTableCell63.StylePriority.UseFont = false;
            this.xrTableCell63.Text = "xrTableCell63";
            this.xrTableCell63.Weight = 0.52434391902043265D;
            // 
            // xrTableCell64
            // 
            this.xrTableCell64.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Total]")});
            this.xrTableCell64.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell64.Multiline = true;
            this.xrTableCell64.Name = "xrTableCell64";
            this.xrTableCell64.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell64.StyleName = "DetailData3";
            this.xrTableCell64.StylePriority.UseFont = false;
            this.xrTableCell64.StylePriority.UseTextAlignment = false;
            this.xrTableCell64.Text = "xrTableCell64";
            this.xrTableCell64.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell64.Weight = 0.4756560809795673D;
            // 
            // detailReportBand5
            // 
            this.detailReportBand5.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.groupHeaderBand6,
            this.detailBand5,
            this.groupFooterBeds});
            this.detailReportBand5.DataMember = "LifeboatBreakdown";
            this.detailReportBand5.DataSource = this.objectDataSource1;
            this.detailReportBand5.Level = 4;
            this.detailReportBand5.Name = "detailReportBand5";
            // 
            // groupHeaderBand6
            // 
            this.groupHeaderBand6.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel5});
            this.groupHeaderBand6.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WithFirstDetail;
            this.groupHeaderBand6.HeightF = 63.09435F;
            this.groupHeaderBand6.Name = "groupHeaderBand6";
            // 
            // xrPanel5
            // 
            this.xrPanel5.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable11,
            this.xrLabel9});
            this.xrPanel5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPanel5.Name = "xrPanel5";
            this.xrPanel5.SizeF = new System.Drawing.SizeF(1001F, 63.09435F);
            this.xrPanel5.StyleName = "DetailCaptionBackground3";
            // 
            // xrTable11
            // 
            this.xrTable11.LocationFloat = new DevExpress.Utils.PointFloat(0F, 35.09435F);
            this.xrTable11.Name = "xrTable11";
            this.xrTable11.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow12});
            this.xrTable11.SizeF = new System.Drawing.SizeF(1001F, 28.00001F);
            // 
            // xrTableRow12
            // 
            this.xrTableRow12.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell65,
            this.xrTableCell66,
            this.xrTableCell67,
            this.xrTableCell68});
            this.xrTableRow12.Name = "xrTableRow12";
            this.xrTableRow12.Weight = 1D;
            // 
            // xrTableCell65
            // 
            this.xrTableCell65.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell65.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell65.Multiline = true;
            this.xrTableCell65.Name = "xrTableCell65";
            this.xrTableCell65.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell65.StyleName = "DetailCaption3";
            this.xrTableCell65.StylePriority.UseBackColor = false;
            this.xrTableCell65.StylePriority.UseFont = false;
            this.xrTableCell65.Text = "Name";
            this.xrTableCell65.Weight = 0.16152475210336539D;
            // 
            // xrTableCell66
            // 
            this.xrTableCell66.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell66.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell66.Multiline = true;
            this.xrTableCell66.Name = "xrTableCell66";
            this.xrTableCell66.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell66.StyleName = "DetailCaption3";
            this.xrTableCell66.StylePriority.UseBackColor = false;
            this.xrTableCell66.StylePriority.UseFont = false;
            this.xrTableCell66.Text = "Maximum";
            this.xrTableCell66.Weight = 0.24352212759164663D;
            // 
            // xrTableCell67
            // 
            this.xrTableCell67.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell67.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell67.Multiline = true;
            this.xrTableCell67.Name = "xrTableCell67";
            this.xrTableCell67.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell67.StyleName = "DetailCaption3";
            this.xrTableCell67.StylePriority.UseBackColor = false;
            this.xrTableCell67.StylePriority.UseFont = false;
            this.xrTableCell67.Text = "Actual Assigned";
            this.xrTableCell67.Weight = 0.36573864276592549D;
            // 
            // xrTableCell68
            // 
            this.xrTableCell68.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell68.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell68.Multiline = true;
            this.xrTableCell68.Name = "xrTableCell68";
            this.xrTableCell68.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell68.StyleName = "DetailCaption3";
            this.xrTableCell68.StylePriority.UseBackColor = false;
            this.xrTableCell68.StylePriority.UseFont = false;
            this.xrTableCell68.Text = "Available";
            this.xrTableCell68.Weight = 0.2292145244891827D;
            // 
            // xrLabel9
            // 
            this.xrLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(127)))));
            this.xrLabel9.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel9.ForeColor = System.Drawing.Color.White;
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(0F, 9.999977F);
            this.xrLabel9.Multiline = true;
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(1001F, 23F);
            this.xrLabel9.StylePriority.UseBackColor = false;
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseForeColor = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "Lifeboat Breakdown";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // detailBand5
            // 
            this.detailBand5.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable12});
            this.detailBand5.HeightF = 25F;
            this.detailBand5.Name = "detailBand5";
            // 
            // xrTable12
            // 
            this.xrTable12.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable12.Name = "xrTable12";
            this.xrTable12.OddStyleName = "DetailData3_Odd";
            this.xrTable12.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow13});
            this.xrTable12.SizeF = new System.Drawing.SizeF(1001F, 25F);
            // 
            // xrTableRow13
            // 
            this.xrTableRow13.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell69,
            this.xrTableCell70,
            this.xrTableCell71,
            this.xrTableCell72});
            this.xrTableRow13.Name = "xrTableRow13";
            this.xrTableRow13.Weight = 11.5D;
            // 
            // xrTableCell69
            // 
            this.xrTableCell69.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Name]")});
            this.xrTableCell69.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell69.Multiline = true;
            this.xrTableCell69.Name = "xrTableCell69";
            this.xrTableCell69.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell69.StyleName = "DetailData3";
            this.xrTableCell69.StylePriority.UseFont = false;
            this.xrTableCell69.Text = "xrTableCell69";
            this.xrTableCell69.Weight = 0.16152474036583533D;
            // 
            // xrTableCell70
            // 
            this.xrTableCell70.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Maximum]")});
            this.xrTableCell70.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell70.Multiline = true;
            this.xrTableCell70.Name = "xrTableCell70";
            this.xrTableCell70.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell70.StyleName = "DetailData3";
            this.xrTableCell70.StylePriority.UseFont = false;
            this.xrTableCell70.Text = "xrTableCell70";
            this.xrTableCell70.Weight = 0.24352212759164663D;
            // 
            // xrTableCell71
            // 
            this.xrTableCell71.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ActualAssigned]")});
            this.xrTableCell71.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell71.Multiline = true;
            this.xrTableCell71.Name = "xrTableCell71";
            this.xrTableCell71.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell71.StyleName = "DetailData3";
            this.xrTableCell71.StylePriority.UseFont = false;
            this.xrTableCell71.Text = "xrTableCell71";
            this.xrTableCell71.Weight = 0.36573861929086537D;
            // 
            // xrTableCell72
            // 
            this.xrTableCell72.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Available]")});
            this.xrTableCell72.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell72.Multiline = true;
            this.xrTableCell72.Name = "xrTableCell72";
            this.xrTableCell72.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell72.StyleName = "DetailData3";
            this.xrTableCell72.StylePriority.UseFont = false;
            this.xrTableCell72.Text = "xrTableCell72";
            this.xrTableCell72.Weight = 0.2292145244891827D;
            // 
            // groupFooterBeds
            // 
            this.groupFooterBeds.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel20,
            this.xrLabel12,
            this.xrLabel11,
            this.xrLabel10,
            this.xrLabel14,
            this.xrLabel16,
            this.xrLabel17,
            this.xrLabel18,
            this.xrLabel19});
            this.groupFooterBeds.HeightF = 46.22641F;
            this.groupFooterBeds.Name = "groupFooterBeds";
            // 
            // xrLabel20
            // 
            this.xrLabel20.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EssentialNo]")});
            this.xrLabel20.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold);
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(689.7449F, 23.22641F);
            this.xrLabel20.Multiline = true;
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(54.94336F, 23F);
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.StylePriority.UseTextAlignment = false;
            this.xrLabel20.Text = "xrLabel20";
            this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel12
            // 
            this.xrLabel12.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Essential]")});
            this.xrLabel12.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold);
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(511.3305F, 23.22641F);
            this.xrLabel12.Multiline = true;
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(61.77264F, 23F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.Text = "xrLabel12";
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel11
            // 
            this.xrLabel11.BackColor = System.Drawing.Color.LightGray;
            this.xrLabel11.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(573.1031F, 23F);
            this.xrLabel11.Multiline = true;
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(116.6418F, 23F);
            this.xrLabel11.StylePriority.UseBackColor = false;
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "Essentials No";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel10
            // 
            this.xrLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(127)))));
            this.xrLabel10.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel10.ForeColor = System.Drawing.Color.White;
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel10.Multiline = true;
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(1001F, 23F);
            this.xrLabel10.StylePriority.UseBackColor = false;
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseForeColor = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "Other";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel14
            // 
            this.xrLabel14.BackColor = System.Drawing.Color.LightGray;
            this.xrLabel14.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(0F, 23.22641F);
            this.xrLabel14.Multiline = true;
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(143.9466F, 23F);
            this.xrLabel14.StylePriority.UseBackColor = false;
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.Text = "Beds Available";
            this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel16
            // 
            this.xrLabel16.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel16.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[BedsAvailable]")});
            this.xrLabel16.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(143.9466F, 23.22641F);
            this.xrLabel16.Multiline = true;
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(76.02205F, 23F);
            this.xrLabel16.StylePriority.UseBackColor = false;
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel17
            // 
            this.xrLabel17.BackColor = System.Drawing.Color.LightGray;
            this.xrLabel17.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(219.9686F, 23F);
            this.xrLabel17.Multiline = true;
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(109.5702F, 23F);
            this.xrLabel17.StylePriority.UseBackColor = false;
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.StylePriority.UseTextAlignment = false;
            this.xrLabel17.Text = "Short Service";
            this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel18
            // 
            this.xrLabel18.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ShortService]")});
            this.xrLabel18.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold);
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(329.5388F, 23F);
            this.xrLabel18.Multiline = true;
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(65.14996F, 23F);
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.StylePriority.UseTextAlignment = false;
            this.xrLabel18.Text = "xrLabel12";
            this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel19
            // 
            this.xrLabel19.BackColor = System.Drawing.Color.LightGray;
            this.xrLabel19.Font = new System.Drawing.Font("Arial", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(394.6888F, 23.22641F);
            this.xrLabel19.Multiline = true;
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(116.6418F, 23F);
            this.xrLabel19.StylePriority.UseBackColor = false;
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.StylePriority.UseTextAlignment = false;
            this.xrLabel19.Text = "Essentials Yes";
            this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // Title
            // 
            this.Title.BackColor = System.Drawing.Color.Transparent;
            this.Title.BorderColor = System.Drawing.Color.Black;
            this.Title.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Title.BorderWidth = 1F;
            this.Title.Font = new System.Drawing.Font("Tahoma", 14F);
            this.Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.Title.Name = "Title";
            // 
            // DetailCaption1
            // 
            this.DetailCaption1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.DetailCaption1.BorderColor = System.Drawing.Color.White;
            this.DetailCaption1.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.DetailCaption1.BorderWidth = 2F;
            this.DetailCaption1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.DetailCaption1.ForeColor = System.Drawing.Color.White;
            this.DetailCaption1.Name = "DetailCaption1";
            this.DetailCaption1.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailCaption1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailData1
            // 
            this.DetailData1.BackColor = System.Drawing.Color.Transparent;
            this.DetailData1.BorderColor = System.Drawing.Color.Transparent;
            this.DetailData1.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.DetailData1.BorderWidth = 2F;
            this.DetailData1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.DetailData1.ForeColor = System.Drawing.Color.Black;
            this.DetailData1.Name = "DetailData1";
            this.DetailData1.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailData1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // GroupCaption3
            // 
            this.GroupCaption3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(137)))), ((int)(((byte)(137)))));
            this.GroupCaption3.BorderColor = System.Drawing.Color.White;
            this.GroupCaption3.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.GroupCaption3.BorderWidth = 2F;
            this.GroupCaption3.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.GroupCaption3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(228)))));
            this.GroupCaption3.Name = "GroupCaption3";
            this.GroupCaption3.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 2, 0, 0, 100F);
            this.GroupCaption3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // GroupData3
            // 
            this.GroupData3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(137)))), ((int)(((byte)(137)))));
            this.GroupData3.BorderColor = System.Drawing.Color.White;
            this.GroupData3.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.GroupData3.BorderWidth = 2F;
            this.GroupData3.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.GroupData3.ForeColor = System.Drawing.Color.White;
            this.GroupData3.Name = "GroupData3";
            this.GroupData3.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 2, 0, 0, 100F);
            this.GroupData3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailCaption3
            // 
            this.DetailCaption3.BackColor = System.Drawing.Color.Transparent;
            this.DetailCaption3.BorderColor = System.Drawing.Color.Transparent;
            this.DetailCaption3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.DetailCaption3.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.DetailCaption3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.DetailCaption3.Name = "DetailCaption3";
            this.DetailCaption3.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailCaption3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailData3
            // 
            this.DetailData3.Font = new System.Drawing.Font("Tahoma", 8F);
            this.DetailData3.ForeColor = System.Drawing.Color.Black;
            this.DetailData3.Name = "DetailData3";
            this.DetailData3.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailData3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailData3_Odd
            // 
            this.DetailData3_Odd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.DetailData3_Odd.BorderColor = System.Drawing.Color.Transparent;
            this.DetailData3_Odd.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.DetailData3_Odd.BorderWidth = 1F;
            this.DetailData3_Odd.Font = new System.Drawing.Font("Tahoma", 8F);
            this.DetailData3_Odd.ForeColor = System.Drawing.Color.Black;
            this.DetailData3_Odd.Name = "DetailData3_Odd";
            this.DetailData3_Odd.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailData3_Odd.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailCaptionBackground3
            // 
            this.DetailCaptionBackground3.BackColor = System.Drawing.Color.Transparent;
            this.DetailCaptionBackground3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
            this.DetailCaptionBackground3.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.DetailCaptionBackground3.BorderWidth = 2F;
            this.DetailCaptionBackground3.Name = "DetailCaptionBackground3";
            // 
            // PageInfo
            // 
            this.PageInfo.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.PageInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.PageInfo.Name = "PageInfo";
            this.PageInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo2,
            this.xrPageInfo1});
            this.PageFooter.HeightF = 24.5283F;
            this.PageFooter.Name = "PageFooter";
            // 
            // RigName
            // 
            this.RigName.Description = "RigName";
            this.RigName.Name = "RigName";
            this.RigName.Visible = false;
            // 
            // LogoFile
            // 
            this.LogoFile.Description = "LogoFIle";
            this.LogoFile.Name = "LogoFile";
            this.LogoFile.Visible = false;
            // 
            // IrmaFile
            // 
            this.IrmaFile.Description = "IrmaFile";
            this.IrmaFile.Name = "IrmaFile";
            this.IrmaFile.Visible = false;
            // 
            // ShowVantage
            // 
            this.ShowVantage.Description = "ShowVantage";
            this.ShowVantage.Name = "ShowVantage";
            this.ShowVantage.Type = typeof(bool);
            this.ShowVantage.ValueInfo = "True";
            this.ShowVantage.Visible = false;
            // 
            // ShowEssential
            // 
            this.ShowEssential.Description = "ShowEssential";
            this.ShowEssential.Name = "ShowEssential";
            this.ShowEssential.Type = typeof(bool);
            this.ShowEssential.ValueInfo = "True";
            this.ShowEssential.Visible = false;
            // 
            // PobSummaryReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.reportHeaderBand1,
            this.detailReportBand1,
            this.detailReportBand2,
            this.detailReportBand3,
            this.detailReportBand4,
            this.detailReportBand5,
            this.PageFooter});
            this.BorderColor = System.Drawing.Color.White;
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataSource = this.objectDataSource1;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(49, 50, 49, 100);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.RigName,
            this.LogoFile,
            this.IrmaFile,
            this.ShowVantage,
            this.ShowEssential});
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.DetailCaption1,
            this.DetailData1,
            this.GroupCaption3,
            this.GroupData3,
            this.DetailCaption3,
            this.DetailData3,
            this.DetailData3_Odd,
            this.DetailCaptionBackground3,
            this.PageInfo});
            this.Version = "18.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    private void xrPictureBox1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        Image image = Image.FromFile((string)LogoFile.Value);
        ((XRPictureBox)(sender)).Image = image;
    }

    private void xrPictureBox2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        Image image = Image.FromFile((string)IrmaFile.Value);
        ((XRPictureBox)(sender)).Image = image;
    }
}

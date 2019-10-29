using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for PersonnelSearch
/// </summary>
public class PersonnelSearch : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private XRTable xrTable2;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell95;
    private XRTableCell xrTableCell96;
    private XRTableCell xrTableCell97;
    private XRTableCell xrTableCell99;
    private XRTableCell xrTableCell100;
    private XRTableCell xrTableCell101;
    private XRTableCell xrTableCell102;
    private XRTableCell xrTableCell103;
    private XRTableCell xrTableCell104;
    private XRTableCell xrTableCell105;
    private XRTableCell xrTableCell106;
    private XRTableCell xrTableCell107;
    private XRTableCell xrTableCell108;
    private XRTableCell xrTableCell109;
    private XRTableCell xrTableCell110;
    private XRTableCell xrTableCell111;
    private XRTableCell xrTableCell112;
    private XRTableCell xrTableCell113;
    private XRTableCell xrTableCell114;
    private XRTableCell xrTableCell115;
    private XRTableCell xrTableCell116;
    private XRTableCell xrTableCell117;
    private XRTableCell xrTableCell118;
    private XRTableCell xrTableCell119;
    private XRTableCell xrTableCell120;
    private XRTableCell xrTableCell121;
    private XRTableCell xrTableCell122;
    private XRTableCell xrTableCell123;
    private XRTableCell xrTableCell124;
    private XRTableCell xrTableCell125;
    private XRTableCell xrTableCell126;
    private XRTableCell xrTableCell127;
    private XRTableCell xrTableCell128;
    private XRTableCell xrTableCell129;
    private XRTableCell xrTableCell130;
    private XRTableCell xrTableCell131;
    private XRTableCell xrTableCell132;
    private XRTableCell xrTableCell133;
    private XRTableCell xrTableCell134;
    private XRTableCell xrTableCell135;
    private XRTableCell xrTableCell136;
    private XRTableCell xrTableCell137;
    private XRTableCell xrTableCell138;
    private XRTableCell xrTableCell139;
    private XRTableCell xrTableCell140;
    private XRTableCell xrTableCell141;
    private XRTableCell xrTableCell142;
    private XRTableCell xrTableCell143;
    private XRTableCell xrTableCell144;
    private XRTableCell xrTableCell145;
    private XRTableCell xrTableCell146;
    private XRTableCell xrTableCell147;
    private XRTableCell xrTableCell148;
    private XRTableCell xrTableCell149;
    private XRTableCell xrTableCell150;
    private XRTableCell xrTableCell151;
    private XRTableCell xrTableCell152;
    private XRTableCell xrTableCell153;
    private XRTableCell xrTableCell154;
    private XRTableCell xrTableCell155;
    private XRTableCell xrTableCell156;
    private XRTableCell xrTableCell157;
    private XRTableCell xrTableCell158;
    private XRTableCell xrTableCell159;
    private XRTableCell xrTableCell160;
    private XRTableCell xrTableCell161;
    private XRTableCell xrTableCell162;
    private XRTableCell xrTableCell163;
    private XRTableCell xrTableCell164;
    private XRTableCell xrTableCell165;
    private XRTableCell xrTableCell166;
    private XRTableCell xrTableCell167;
    private XRTableCell xrTableCell168;
    private XRTableCell xrTableCell169;
    private XRTableCell xrTableCell170;
    private XRTableCell xrTableCell171;
    private XRTableCell xrTableCell172;
    private XRTableCell xrTableCell173;
    private XRTableCell xrTableCell174;
    private XRTableCell xrTableCell175;
    private XRTableCell xrTableCell176;
    private XRTableCell xrTableCell177;
    private XRTableCell xrTableCell178;
    private XRTableCell xrTableCell179;
    private XRTableCell xrTableCell180;
    private XRTableCell xrTableCell181;
    private XRTableCell xrTableCell182;
    private XRTableCell xrTableCell183;
    private XRTableCell xrTableCell184;
    private XRTableCell xrTableCell185;
    private XRTableCell xrTableCell186;
    private XRTableCell xrTableCell187;
    private XRTableCell xrTableCell188;
    private XRPageInfo xrPageInfo1;
    private XRPageInfo xrPageInfo2;
    private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
    private ReportHeaderBand reportHeaderBand1;
    private GroupHeaderBand groupHeaderBand1;
    private XRPanel xrPanel1;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell8;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell16;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell18;
    private XRTableCell xrTableCell19;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell21;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell23;
    private XRTableCell xrTableCell24;
    private XRTableCell xrTableCell25;
    private XRTableCell xrTableCell26;
    private XRTableCell xrTableCell27;
    private XRTableCell xrTableCell28;
    private XRTableCell xrTableCell29;
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
    private XRTableCell xrTableCell41;
    private XRTableCell xrTableCell42;
    private XRTableCell xrTableCell43;
    private XRTableCell xrTableCell44;
    private XRTableCell xrTableCell45;
    private XRTableCell xrTableCell46;
    private XRTableCell xrTableCell47;
    private XRTableCell xrTableCell48;
    private XRTableCell xrTableCell49;
    private XRTableCell xrTableCell50;
    private XRTableCell xrTableCell51;
    private XRTableCell xrTableCell52;
    private XRTableCell xrTableCell53;
    private XRTableCell xrTableCell54;
    private XRTableCell xrTableCell55;
    private XRTableCell xrTableCell56;
    private XRTableCell xrTableCell57;
    private XRTableCell xrTableCell58;
    private XRTableCell xrTableCell59;
    private XRTableCell xrTableCell60;
    private XRTableCell xrTableCell61;
    private XRTableCell xrTableCell62;
    private XRTableCell xrTableCell63;
    private XRTableCell xrTableCell64;
    private XRTableCell xrTableCell65;
    private XRTableCell xrTableCell66;
    private XRTableCell xrTableCell67;
    private XRTableCell xrTableCell68;
    private XRTableCell xrTableCell69;
    private XRTableCell xrTableCell70;
    private XRTableCell xrTableCell71;
    private XRTableCell xrTableCell72;
    private XRTableCell xrTableCell73;
    private XRTableCell xrTableCell74;
    private XRTableCell xrTableCell75;
    private XRTableCell xrTableCell76;
    private XRTableCell xrTableCell77;
    private XRTableCell xrTableCell78;
    private XRTableCell xrTableCell79;
    private XRTableCell xrTableCell80;
    private XRTableCell xrTableCell81;
    private XRTableCell xrTableCell82;
    private XRTableCell xrTableCell83;
    private XRTableCell xrTableCell84;
    private XRTableCell xrTableCell85;
    private XRTableCell xrTableCell86;
    private XRTableCell xrTableCell87;
    private XRTableCell xrTableCell88;
    private XRTableCell xrTableCell89;
    private XRTableCell xrTableCell90;
    private XRTableCell xrTableCell91;
    private XRTableCell xrTableCell92;
    private XRTableCell xrTableCell93;
    private XRTableCell xrTableCell94;
    private XRControlStyle Title;
    private XRControlStyle DetailCaption3;
    private XRControlStyle DetailData3;
    private XRControlStyle DetailData3_Odd;
    private XRControlStyle DetailCaptionBackground3;
    private XRControlStyle PageInfo;
    private XRPanel xrPanel2;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrLabel9;
    private XRLabel xrLabel1;
    private XRPageInfo xrPageInfo3;
    private XRPictureBox xrPictureBox2;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public PersonnelSearch()
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
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.reportHeaderBand1 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.groupHeaderBand1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
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
            this.xrTableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell44 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell45 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell46 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell47 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell48 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell49 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell50 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell51 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell52 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell53 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell54 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell55 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell56 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell57 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell58 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell59 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell60 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell61 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell62 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell63 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell64 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell65 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell66 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell67 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell68 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell69 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell70 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell71 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell72 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell73 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell74 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell75 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell76 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell77 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell78 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell79 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell80 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell81 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell82 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell83 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell84 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell85 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell86 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell87 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell88 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell89 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell90 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell91 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell92 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell93 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell94 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell95 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell96 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell97 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell99 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell100 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell101 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell102 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell103 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell104 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell105 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell106 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell107 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell108 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell109 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell110 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell111 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell112 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell113 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell114 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell115 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell116 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell117 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell118 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell119 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell120 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell121 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell122 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell123 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell124 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell125 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell126 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell127 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell128 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell129 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell130 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell131 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell132 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell133 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell134 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell135 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell136 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell137 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell138 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell139 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell140 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell141 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell142 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell143 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell144 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell145 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell146 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell147 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell148 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell149 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell150 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell151 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell152 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell153 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell154 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell155 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell156 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell157 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell158 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell159 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell160 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell161 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell162 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell163 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell164 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell165 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell166 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell167 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell168 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell169 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell170 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell171 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell172 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell173 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell174 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell175 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell176 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell177 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell178 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell179 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell180 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell181 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell182 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell183 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell184 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell185 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell186 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell187 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell188 = new DevExpress.XtraReports.UI.XRTableCell();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailCaption3 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData3 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData3_Odd = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailCaptionBackground3 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo3 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail.Font = new System.Drawing.Font("Arial", 8.830189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Detail.HeightF = 25F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UseFont = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1,
            this.xrPageInfo2});
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(Ensco.Irma.Models.RigPersonnelArchiveModel);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(6F, 6F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(313F, 23F);
            this.xrPageInfo1.StyleName = "PageInfo";
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(331F, 6F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(313F, 23F);
            this.xrPageInfo2.StyleName = "PageInfo";
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrPageInfo2.TextFormatString = "Page {0} of {1}";
            // 
            // reportHeaderBand1
            // 
            this.reportHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel2});
            this.reportHeaderBand1.HeightF = 78.86793F;
            this.reportHeaderBand1.Name = "reportHeaderBand1";
            // 
            // groupHeaderBand1
            // 
            this.groupHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel1});
            this.groupHeaderBand1.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WithFirstDetail;
            this.groupHeaderBand1.HeightF = 48F;
            this.groupHeaderBand1.Name = "groupHeaderBand1";
            // 
            // xrPanel1
            // 
            this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPanel1.Name = "xrPanel1";
            this.xrPanel1.SizeF = new System.Drawing.SizeF(7500F, 48F);
            this.xrPanel1.StyleName = "DetailCaptionBackground3";
            // 
            // xrTable1
            // 
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 20.00001F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(7434.189F, 28F);
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell3,
            this.xrTableCell5,
            this.xrTableCell6,
            this.xrTableCell7,
            this.xrTableCell8,
            this.xrTableCell9,
            this.xrTableCell10,
            this.xrTableCell11,
            this.xrTableCell12,
            this.xrTableCell13,
            this.xrTableCell14,
            this.xrTableCell15,
            this.xrTableCell16,
            this.xrTableCell17,
            this.xrTableCell18,
            this.xrTableCell19,
            this.xrTableCell20,
            this.xrTableCell21,
            this.xrTableCell22,
            this.xrTableCell23,
            this.xrTableCell24,
            this.xrTableCell25,
            this.xrTableCell26,
            this.xrTableCell27,
            this.xrTableCell28,
            this.xrTableCell29,
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
            this.xrTableCell40,
            this.xrTableCell41,
            this.xrTableCell42,
            this.xrTableCell43,
            this.xrTableCell44,
            this.xrTableCell45,
            this.xrTableCell46,
            this.xrTableCell47,
            this.xrTableCell48,
            this.xrTableCell49,
            this.xrTableCell50,
            this.xrTableCell51,
            this.xrTableCell52,
            this.xrTableCell53,
            this.xrTableCell54,
            this.xrTableCell55,
            this.xrTableCell56,
            this.xrTableCell57,
            this.xrTableCell58,
            this.xrTableCell59,
            this.xrTableCell60,
            this.xrTableCell61,
            this.xrTableCell62,
            this.xrTableCell63,
            this.xrTableCell64,
            this.xrTableCell65,
            this.xrTableCell66,
            this.xrTableCell67,
            this.xrTableCell68,
            this.xrTableCell69,
            this.xrTableCell70,
            this.xrTableCell71,
            this.xrTableCell72,
            this.xrTableCell73,
            this.xrTableCell74,
            this.xrTableCell75,
            this.xrTableCell76,
            this.xrTableCell77,
            this.xrTableCell78,
            this.xrTableCell79,
            this.xrTableCell80,
            this.xrTableCell81,
            this.xrTableCell82,
            this.xrTableCell83,
            this.xrTableCell84,
            this.xrTableCell85,
            this.xrTableCell86,
            this.xrTableCell87,
            this.xrTableCell88,
            this.xrTableCell89,
            this.xrTableCell90,
            this.xrTableCell91,
            this.xrTableCell92,
            this.xrTableCell93,
            this.xrTableCell94});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Multiline = true;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell1.StyleName = "DetailCaption3";
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Id";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell1.Weight = 0.0044607969884226158D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Multiline = true;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell2.StyleName = "DetailCaption3";
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "Crew Change Id";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell2.Weight = 0.0093643640956967977D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Multiline = true;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell3.StyleName = "DetailCaption3";
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Crew Flight Id";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell3.Weight = 0.009301181512521987D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Multiline = true;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell5.StyleName = "DetailCaption3";
            this.xrTableCell5.Text = "Passport";
            this.xrTableCell5.Weight = 0.009015616170506802D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Multiline = true;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell6.StyleName = "DetailCaption3";
            this.xrTableCell6.Text = "Display Name";
            this.xrTableCell6.Weight = 0.012724466665347786D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Multiline = true;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell7.StyleName = "DetailCaption3";
            this.xrTableCell7.Text = "First Name";
            this.xrTableCell7.Weight = 0.0086826324462890622D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Multiline = true;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell8.StyleName = "DetailCaption3";
            this.xrTableCell8.Text = "Last Name";
            this.xrTableCell8.Weight = 0.0084893366006704475D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Multiline = true;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell9.StyleName = "DetailCaption3";
            this.xrTableCell9.Text = "Middle Name";
            this.xrTableCell9.Weight = 0.0067504915013471417D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Multiline = true;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell10.StyleName = "DetailCaption3";
            this.xrTableCell10.Text = "Email";
            this.xrTableCell10.Weight = 0.008512521237981097D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Multiline = true;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell11.StyleName = "DetailCaption3";
            this.xrTableCell11.Text = "Primary Phone";
            this.xrTableCell11.Weight = 0.011259042299710788D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Multiline = true;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell12.StyleName = "DetailCaption3";
            this.xrTableCell12.Text = "Status";
            this.xrTableCell12.Weight = 0.0057092527242807245D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.Multiline = true;
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell13.StyleName = "DetailCaption3";
            this.xrTableCell13.Text = "Nationality";
            this.xrTableCell13.Weight = 0.0087903066781850961D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Multiline = true;
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell14.StyleName = "DetailCaption3";
            this.xrTableCell14.Text = "Position";
            this.xrTableCell14.Weight = 0.0068352963374211234D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Multiline = true;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell15.StyleName = "DetailCaption3";
            this.xrTableCell15.Text = "Department";
            this.xrTableCell15.Weight = 0.0094467339148888215D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Multiline = true;
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell16.StyleName = "DetailCaption3";
            this.xrTableCell16.Text = "Company";
            this.xrTableCell16.Weight = 0.0076545333862304684D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Multiline = true;
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell17.StyleName = "DetailCaption3";
            this.xrTableCell17.Text = "Company Type";
            this.xrTableCell17.Weight = 0.011246717893160306D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.Multiline = true;
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell18.StyleName = "DetailCaption3";
            this.xrTableCell18.Text = "Personnel Type";
            this.xrTableCell18.Weight = 0.011658606162438025D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.Multiline = true;
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell19.StyleName = "DetailCaption3";
            this.xrTableCell19.Text = "Short Service";
            this.xrTableCell19.Weight = 0.010328240027794471D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.Multiline = true;
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell20.StyleName = "DetailCaption3";
            this.xrTableCell20.Text = "Date Of Birth";
            this.xrTableCell20.Weight = 0.010176457625169021D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.Multiline = true;
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell21.StyleName = "DetailCaption3";
            this.xrTableCell21.Text = "Essential";
            this.xrTableCell21.Weight = 0.007407399691068209D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.Multiline = true;
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell22.StyleName = "DetailCaption3";
            this.xrTableCell22.Text = "Vantage Number";
            this.xrTableCell22.Weight = 0.012682813497690054D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.Multiline = true;
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell23.StyleName = "DetailCaption3";
            this.xrTableCell23.Text = "Crew";
            this.xrTableCell23.Weight = 0.004867961956904485D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.Multiline = true;
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell24.StyleName = "DetailCaption3";
            this.xrTableCell24.Text = "Tour";
            this.xrTableCell24.Weight = 0.0044943435375507058D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.Multiline = true;
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell25.StyleName = "DetailCaption3";
            this.xrTableCell25.Text = "Room";
            this.xrTableCell25.Weight = 0.0053057964031512916D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.Multiline = true;
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell26.StyleName = "DetailCaption3";
            this.xrTableCell26.Text = "Bed";
            this.xrTableCell26.Weight = 0.0039708893115703876D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.Multiline = true;
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell27.StyleName = "DetailCaption3";
            this.xrTableCell27.Text = "Muster Station1";
            this.xrTableCell27.Weight = 0.0121184935936561D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.Multiline = true;
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell28.StyleName = "DetailCaption3";
            this.xrTableCell28.Text = "Muster Station2";
            this.xrTableCell28.Weight = 0.0121184935936561D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.Multiline = true;
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell29.StyleName = "DetailCaption3";
            this.xrTableCell29.Text = "Primary Life Boat";
            this.xrTableCell29.Weight = 0.013001945202167218D;
            // 
            // xrTableCell30
            // 
            this.xrTableCell30.Multiline = true;
            this.xrTableCell30.Name = "xrTableCell30";
            this.xrTableCell30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell30.StyleName = "DetailCaption3";
            this.xrTableCell30.Text = "Secondary Life Boat";
            this.xrTableCell30.Weight = 0.014722146254319411D;
            // 
            // xrTableCell31
            // 
            this.xrTableCell31.Multiline = true;
            this.xrTableCell31.Name = "xrTableCell31";
            this.xrTableCell31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell31.StyleName = "DetailCaption3";
            this.xrTableCell31.Text = "Home Airport";
            this.xrTableCell31.Weight = 0.010384671871478741D;
            // 
            // xrTableCell32
            // 
            this.xrTableCell32.Multiline = true;
            this.xrTableCell32.Name = "xrTableCell32";
            this.xrTableCell32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell32.StyleName = "DetailCaption3";
            this.xrTableCell32.Text = "Body Weight";
            this.xrTableCell32.Weight = 0.009949432519766D;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.Multiline = true;
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell33.StyleName = "DetailCaption3";
            this.xrTableCell33.Text = "Bag Weight";
            this.xrTableCell33.Weight = 0.0091600337395301226D;
            // 
            // xrTableCell34
            // 
            this.xrTableCell34.Multiline = true;
            this.xrTableCell34.Name = "xrTableCell34";
            this.xrTableCell34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell34.StyleName = "DetailCaption3";
            this.xrTableCell34.Text = "Bags";
            this.xrTableCell34.Weight = 0.004661045441260705D;
            // 
            // xrTableCell35
            // 
            this.xrTableCell35.Multiline = true;
            this.xrTableCell35.Name = "xrTableCell35";
            this.xrTableCell35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell35.StyleName = "DetailCaption3";
            this.xrTableCell35.Text = "Airline Flight";
            this.xrTableCell35.Weight = 0.0099630531897911666D;
            // 
            // xrTableCell36
            // 
            this.xrTableCell36.Multiline = true;
            this.xrTableCell36.Name = "xrTableCell36";
            this.xrTableCell36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell36.StyleName = "DetailCaption3";
            this.xrTableCell36.Text = "Location";
            this.xrTableCell36.Weight = 0.0071336724207951476D;
            // 
            // xrTableCell37
            // 
            this.xrTableCell37.Multiline = true;
            this.xrTableCell37.Name = "xrTableCell37";
            this.xrTableCell37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell37.StyleName = "DetailCaption3";
            this.xrTableCell37.Text = "Terminal";
            this.xrTableCell37.Weight = 0.0073269682664137623D;
            // 
            // xrTableCell38
            // 
            this.xrTableCell38.Multiline = true;
            this.xrTableCell38.Name = "xrTableCell38";
            this.xrTableCell38.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell38.StyleName = "DetailCaption3";
            this.xrTableCell38.Text = "Date Start";
            this.xrTableCell38.Weight = 0.00828241935143104D;
            // 
            // xrTableCell39
            // 
            this.xrTableCell39.Multiline = true;
            this.xrTableCell39.Name = "xrTableCell39";
            this.xrTableCell39.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell39.StyleName = "DetailCaption3";
            this.xrTableCell39.Text = "Date End";
            this.xrTableCell39.Weight = 0.0074696702223557695D;
            // 
            // xrTableCell40
            // 
            this.xrTableCell40.Multiline = true;
            this.xrTableCell40.Name = "xrTableCell40";
            this.xrTableCell40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell40.StyleName = "DetailCaption3";
            this.xrTableCell40.Text = "Date Estimated Leave";
            this.xrTableCell40.Weight = 0.01594937984759991D;
            // 
            // xrTableCell41
            // 
            this.xrTableCell41.Multiline = true;
            this.xrTableCell41.Name = "xrTableCell41";
            this.xrTableCell41.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell41.StyleName = "DetailCaption3";
            this.xrTableCell41.Text = "Authentication";
            this.xrTableCell41.Weight = 0.011307689960186298D;
            // 
            // xrTableCell42
            // 
            this.xrTableCell42.Multiline = true;
            this.xrTableCell42.Name = "xrTableCell42";
            this.xrTableCell42.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell42.StyleName = "DetailCaption3";
            this.xrTableCell42.Text = "Optional1";
            this.xrTableCell42.Weight = 0.0079587466900165257D;
            // 
            // xrTableCell43
            // 
            this.xrTableCell43.Multiline = true;
            this.xrTableCell43.Name = "xrTableCell43";
            this.xrTableCell43.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell43.StyleName = "DetailCaption3";
            this.xrTableCell43.Text = "Optional2";
            this.xrTableCell43.Weight = 0.0079587466900165257D;
            // 
            // xrTableCell44
            // 
            this.xrTableCell44.Multiline = true;
            this.xrTableCell44.Name = "xrTableCell44";
            this.xrTableCell44.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell44.StyleName = "DetailCaption3";
            this.xrTableCell44.Text = "Optional3";
            this.xrTableCell44.Weight = 0.0079587466900165257D;
            // 
            // xrTableCell45
            // 
            this.xrTableCell45.Multiline = true;
            this.xrTableCell45.Name = "xrTableCell45";
            this.xrTableCell45.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell45.StyleName = "DetailCaption3";
            this.xrTableCell45.Text = "Optional4";
            this.xrTableCell45.Weight = 0.0079587466900165257D;
            // 
            // xrTableCell46
            // 
            this.xrTableCell46.Multiline = true;
            this.xrTableCell46.Name = "xrTableCell46";
            this.xrTableCell46.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell46.StyleName = "DetailCaption3";
            this.xrTableCell46.Text = "Role";
            this.xrTableCell46.Weight = 0.0044087230242215673D;
            // 
            // xrTableCell47
            // 
            this.xrTableCell47.Multiline = true;
            this.xrTableCell47.Name = "xrTableCell47";
            this.xrTableCell47.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell47.StyleName = "DetailCaption3";
            this.xrTableCell47.Text = "Date Created";
            this.xrTableCell47.Weight = 0.01018942979665903D;
            // 
            // xrTableCell48
            // 
            this.xrTableCell48.Multiline = true;
            this.xrTableCell48.Name = "xrTableCell48";
            this.xrTableCell48.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell48.StyleName = "DetailCaption3";
            this.xrTableCell48.Text = "Date Modified";
            this.xrTableCell48.Weight = 0.010742723024808443D;
            // 
            // xrTableCell49
            // 
            this.xrTableCell49.Multiline = true;
            this.xrTableCell49.Name = "xrTableCell49";
            this.xrTableCell49.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell49.StyleName = "DetailCaption3";
            this.xrTableCell49.Text = "Modified By";
            this.xrTableCell49.Weight = 0.0092767898853008558D;
            // 
            // xrTableCell50
            // 
            this.xrTableCell50.Multiline = true;
            this.xrTableCell50.Name = "xrTableCell50";
            this.xrTableCell50.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell50.StyleName = "DetailCaption3";
            this.xrTableCell50.Text = "Passport Issuer";
            this.xrTableCell50.Weight = 0.011868118139413688D;
            // 
            // xrTableCell51
            // 
            this.xrTableCell51.Multiline = true;
            this.xrTableCell51.Name = "xrTableCell51";
            this.xrTableCell51.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell51.StyleName = "DetailCaption3";
            this.xrTableCell51.Text = "Passport Number";
            this.xrTableCell51.Weight = 0.012988323798546425D;
            // 
            // xrTableCell52
            // 
            this.xrTableCell52.Multiline = true;
            this.xrTableCell52.Name = "xrTableCell52";
            this.xrTableCell52.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell52.StyleName = "DetailCaption3";
            this.xrTableCell52.Text = "Passport Expires";
            this.xrTableCell52.Weight = 0.012549193455622746D;
            // 
            // xrTableCell53
            // 
            this.xrTableCell53.Multiline = true;
            this.xrTableCell53.Name = "xrTableCell53";
            this.xrTableCell53.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell53.StyleName = "DetailCaption3";
            this.xrTableCell53.Text = "Visa Number";
            this.xrTableCell53.Weight = 0.00994489156282865D;
            // 
            // xrTableCell54
            // 
            this.xrTableCell54.Multiline = true;
            this.xrTableCell54.Name = "xrTableCell54";
            this.xrTableCell54.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell54.StyleName = "DetailCaption3";
            this.xrTableCell54.Text = "Visa Expires";
            this.xrTableCell54.Weight = 0.0095057604863093455D;
            // 
            // xrTableCell55
            // 
            this.xrTableCell55.Multiline = true;
            this.xrTableCell55.Name = "xrTableCell55";
            this.xrTableCell55.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell55.StyleName = "DetailCaption3";
            this.xrTableCell55.Text = "Identification Issuer";
            this.xrTableCell55.Weight = 0.015146359663743239D;
            // 
            // xrTableCell56
            // 
            this.xrTableCell56.Multiline = true;
            this.xrTableCell56.Name = "xrTableCell56";
            this.xrTableCell56.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell56.StyleName = "DetailCaption3";
            this.xrTableCell56.Text = "Identification";
            this.xrTableCell56.Weight = 0.010532562549297627D;
            // 
            // xrTableCell57
            // 
            this.xrTableCell57.Multiline = true;
            this.xrTableCell57.Name = "xrTableCell57";
            this.xrTableCell57.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell57.StyleName = "DetailCaption3";
            this.xrTableCell57.Text = "Identification Expires";
            this.xrTableCell57.Weight = 0.015827434246356669D;
            // 
            // xrTableCell58
            // 
            this.xrTableCell58.Multiline = true;
            this.xrTableCell58.Name = "xrTableCell58";
            this.xrTableCell58.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell58.StyleName = "DetailCaption3";
            this.xrTableCell58.Text = "Comment";
            this.xrTableCell58.Weight = 0.007865990858811598D;
            // 
            // xrTableCell59
            // 
            this.xrTableCell59.Multiline = true;
            this.xrTableCell59.Name = "xrTableCell59";
            this.xrTableCell59.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell59.StyleName = "DetailCaption3";
            this.xrTableCell59.Text = "Address Line1";
            this.xrTableCell59.Weight = 0.010783587235670824D;
            // 
            // xrTableCell60
            // 
            this.xrTableCell60.Multiline = true;
            this.xrTableCell60.Name = "xrTableCell60";
            this.xrTableCell60.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell60.StyleName = "DetailCaption3";
            this.xrTableCell60.Text = "Address Line2";
            this.xrTableCell60.Weight = 0.010783587235670824D;
            // 
            // xrTableCell61
            // 
            this.xrTableCell61.Multiline = true;
            this.xrTableCell61.Name = "xrTableCell61";
            this.xrTableCell61.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell61.StyleName = "DetailCaption3";
            this.xrTableCell61.Text = "City";
            this.xrTableCell61.Weight = 0.0040389966964721677D;
            // 
            // xrTableCell62
            // 
            this.xrTableCell62.Multiline = true;
            this.xrTableCell62.Name = "xrTableCell62";
            this.xrTableCell62.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell62.StyleName = "DetailCaption3";
            this.xrTableCell62.Text = "State";
            this.xrTableCell62.Weight = 0.00496396174797645D;
            // 
            // xrTableCell63
            // 
            this.xrTableCell63.Multiline = true;
            this.xrTableCell63.Name = "xrTableCell63";
            this.xrTableCell63.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell63.StyleName = "DetailCaption3";
            this.xrTableCell63.Text = "Country";
            this.xrTableCell63.Weight = 0.0067347570566030652D;
            // 
            // xrTableCell64
            // 
            this.xrTableCell64.Multiline = true;
            this.xrTableCell64.Name = "xrTableCell64";
            this.xrTableCell64.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell64.StyleName = "DetailCaption3";
            this.xrTableCell64.Text = "Postal Code";
            this.xrTableCell64.Weight = 0.0092800323779766384D;
            // 
            // xrTableCell65
            // 
            this.xrTableCell65.Multiline = true;
            this.xrTableCell65.Name = "xrTableCell65";
            this.xrTableCell65.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell65.StyleName = "DetailCaption3";
            this.xrTableCell65.Text = "Secondary Phone";
            this.xrTableCell65.Weight = 0.012979244819054237D;
            // 
            // xrTableCell66
            // 
            this.xrTableCell66.Multiline = true;
            this.xrTableCell66.Name = "xrTableCell66";
            this.xrTableCell66.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell66.StyleName = "DetailCaption3";
            this.xrTableCell66.Text = "Marital Status";
            this.xrTableCell66.Weight = 0.010804992822500376D;
            // 
            // xrTableCell67
            // 
            this.xrTableCell67.Multiline = true;
            this.xrTableCell67.Name = "xrTableCell67";
            this.xrTableCell67.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell67.StyleName = "DetailCaption3";
            this.xrTableCell67.Text = "Personal Email";
            this.xrTableCell67.Weight = 0.011292773026686449D;
            // 
            // xrTableCell68
            // 
            this.xrTableCell68.Multiline = true;
            this.xrTableCell68.Name = "xrTableCell68";
            this.xrTableCell68.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell68.StyleName = "DetailCaption3";
            this.xrTableCell68.Text = "Contact Info Comment";
            this.xrTableCell68.Weight = 0.016621373983529898D;
            // 
            // xrTableCell69
            // 
            this.xrTableCell69.Multiline = true;
            this.xrTableCell69.Name = "xrTableCell69";
            this.xrTableCell69.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell69.StyleName = "DetailCaption3";
            this.xrTableCell69.Text = "Emergency Contact First Name";
            this.xrTableCell69.Weight = 0.022007057483379656D;
            // 
            // xrTableCell70
            // 
            this.xrTableCell70.Multiline = true;
            this.xrTableCell70.Name = "xrTableCell70";
            this.xrTableCell70.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell70.StyleName = "DetailCaption3";
            this.xrTableCell70.Text = "Emergency Contact Last Name";
            this.xrTableCell70.Weight = 0.021813762371356672D;
            // 
            // xrTableCell71
            // 
            this.xrTableCell71.Multiline = true;
            this.xrTableCell71.Name = "xrTableCell71";
            this.xrTableCell71.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell71.StyleName = "DetailCaption3";
            this.xrTableCell71.Text = "Emergency Contact Relationship";
            this.xrTableCell71.Weight = 0.023102615063007061D;
            // 
            // xrTableCell72
            // 
            this.xrTableCell72.Multiline = true;
            this.xrTableCell72.Name = "xrTableCell72";
            this.xrTableCell72.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell72.StyleName = "DetailCaption3";
            this.xrTableCell72.Text = "Emergency Contact Email";
            this.xrTableCell72.Weight = 0.018440168820894682D;
            // 
            // xrTableCell73
            // 
            this.xrTableCell73.Multiline = true;
            this.xrTableCell73.Name = "xrTableCell73";
            this.xrTableCell73.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell73.StyleName = "DetailCaption3";
            this.xrTableCell73.Text = "Emergency Contact Primary Phone";
            this.xrTableCell73.Weight = 0.024583467336801383D;
            // 
            // xrTableCell74
            // 
            this.xrTableCell74.Multiline = true;
            this.xrTableCell74.Name = "xrTableCell74";
            this.xrTableCell74.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell74.StyleName = "DetailCaption3";
            this.xrTableCell74.Text = "Emergency Contact Secondary Phone";
            this.xrTableCell74.Weight = 0.02630366985614483D;
            // 
            // xrTableCell75
            // 
            this.xrTableCell75.Multiline = true;
            this.xrTableCell75.Name = "xrTableCell75";
            this.xrTableCell75.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell75.StyleName = "DetailCaption3";
            this.xrTableCell75.Text = "Emergency Contact Comment";
            this.xrTableCell75.Weight = 0.021190417363093451D;
            // 
            // xrTableCell76
            // 
            this.xrTableCell76.Multiline = true;
            this.xrTableCell76.Name = "xrTableCell76";
            this.xrTableCell76.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell76.StyleName = "DetailCaption3";
            this.xrTableCell76.Text = "Vehicle Make";
            this.xrTableCell76.Weight = 0.010221214294433593D;
            // 
            // xrTableCell77
            // 
            this.xrTableCell77.Multiline = true;
            this.xrTableCell77.Name = "xrTableCell77";
            this.xrTableCell77.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell77.StyleName = "DetailCaption3";
            this.xrTableCell77.Text = "Vehicle Model";
            this.xrTableCell77.Weight = 0.010682399456317607D;
            // 
            // xrTableCell78
            // 
            this.xrTableCell78.Multiline = true;
            this.xrTableCell78.Name = "xrTableCell78";
            this.xrTableCell78.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell78.StyleName = "DetailCaption3";
            this.xrTableCell78.Text = "Vehicle Color";
            this.xrTableCell78.Weight = 0.010153755041269155D;
            // 
            // xrTableCell79
            // 
            this.xrTableCell79.Multiline = true;
            this.xrTableCell79.Name = "xrTableCell79";
            this.xrTableCell79.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell79.StyleName = "DetailCaption3";
            this.xrTableCell79.Text = "Vehicle Plate Issuer";
            this.xrTableCell79.Weight = 0.014674148559570312D;
            // 
            // xrTableCell80
            // 
            this.xrTableCell80.Multiline = true;
            this.xrTableCell80.Name = "xrTableCell80";
            this.xrTableCell80.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell80.StyleName = "DetailCaption3";
            this.xrTableCell80.Text = "Vehicle Plate Number";
            this.xrTableCell80.Weight = 0.015794353485107424D;
            // 
            // xrTableCell81
            // 
            this.xrTableCell81.Multiline = true;
            this.xrTableCell81.Name = "xrTableCell81";
            this.xrTableCell81.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell81.StyleName = "DetailCaption3";
            this.xrTableCell81.Text = "Vehicle Location";
            this.xrTableCell81.Weight = 0.012349410423865686D;
            // 
            // xrTableCell82
            // 
            this.xrTableCell82.Multiline = true;
            this.xrTableCell82.Name = "xrTableCell82";
            this.xrTableCell82.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell82.StyleName = "DetailCaption3";
            this.xrTableCell82.Text = "Vehicle Comment";
            this.xrTableCell82.Weight = 0.013081728128286509D;
            // 
            // xrTableCell83
            // 
            this.xrTableCell83.Multiline = true;
            this.xrTableCell83.Name = "xrTableCell83";
            this.xrTableCell83.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell83.StyleName = "DetailCaption3";
            this.xrTableCell83.Text = "Work Boots";
            this.xrTableCell83.Weight = 0.0091723581460806045D;
            // 
            // xrTableCell84
            // 
            this.xrTableCell84.Multiline = true;
            this.xrTableCell84.Name = "xrTableCell84";
            this.xrTableCell84.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell84.StyleName = "DetailCaption3";
            this.xrTableCell84.Text = "Rubber Boots";
            this.xrTableCell84.Weight = 0.010468995020939753D;
            // 
            // xrTableCell85
            // 
            this.xrTableCell85.Multiline = true;
            this.xrTableCell85.Name = "xrTableCell85";
            this.xrTableCell85.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell85.StyleName = "DetailCaption3";
            this.xrTableCell85.Text = "Gloves";
            this.xrTableCell85.Weight = 0.005886332438542293D;
            // 
            // xrTableCell86
            // 
            this.xrTableCell86.Multiline = true;
            this.xrTableCell86.Name = "xrTableCell86";
            this.xrTableCell86.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell86.StyleName = "DetailCaption3";
            this.xrTableCell86.Text = "TShirt";
            this.xrTableCell86.Weight = 0.0054686062152569112D;
            // 
            // xrTableCell87
            // 
            this.xrTableCell87.Multiline = true;
            this.xrTableCell87.Name = "xrTableCell87";
            this.xrTableCell87.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell87.StyleName = "DetailCaption3";
            this.xrTableCell87.Text = "Coverall Tan";
            this.xrTableCell87.Weight = 0.0097198119530311D;
            // 
            // xrTableCell88
            // 
            this.xrTableCell88.Multiline = true;
            this.xrTableCell88.Name = "xrTableCell88";
            this.xrTableCell88.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell88.StyleName = "DetailCaption3";
            this.xrTableCell88.Text = "Coverall FRC";
            this.xrTableCell88.Weight = 0.009883919495802659D;
            // 
            // xrTableCell89
            // 
            this.xrTableCell89.Multiline = true;
            this.xrTableCell89.Name = "xrTableCell89";
            this.xrTableCell89.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell89.StyleName = "DetailCaption3";
            this.xrTableCell89.Text = "Coverall Chemical";
            this.xrTableCell89.Weight = 0.013350916642409104D;
            // 
            // xrTableCell90
            // 
            this.xrTableCell90.Multiline = true;
            this.xrTableCell90.Name = "xrTableCell90";
            this.xrTableCell90.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell90.StyleName = "DetailCaption3";
            this.xrTableCell90.Text = "Coverall Winter";
            this.xrTableCell90.Weight = 0.011794173167302057D;
            // 
            // xrTableCell91
            // 
            this.xrTableCell91.Multiline = true;
            this.xrTableCell91.Name = "xrTableCell91";
            this.xrTableCell91.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell91.StyleName = "DetailCaption3";
            this.xrTableCell91.Text = "PPEComment";
            this.xrTableCell91.Weight = 0.010429427807147687D;
            // 
            // xrTableCell92
            // 
            this.xrTableCell92.Multiline = true;
            this.xrTableCell92.Name = "xrTableCell92";
            this.xrTableCell92.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell92.StyleName = "DetailCaption3";
            this.xrTableCell92.Text = "Gender";
            this.xrTableCell92.Weight = 0.0062644914480356074D;
            // 
            // xrTableCell93
            // 
            this.xrTableCell93.Multiline = true;
            this.xrTableCell93.Name = "xrTableCell93";
            this.xrTableCell93.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell93.StyleName = "DetailCaption3";
            this.xrTableCell93.Text = "Locker Number";
            this.xrTableCell93.Weight = 0.011614498725304236D;
            // 
            // xrTableCell94
            // 
            this.xrTableCell94.Multiline = true;
            this.xrTableCell94.Name = "xrTableCell94";
            this.xrTableCell94.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell94.StyleName = "DetailCaption3";
            this.xrTableCell94.Text = "ISNNumber";
            this.xrTableCell94.Weight = 0.0092866127307598412D;
            // 
            // xrTable2
            // 
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.OddStyleName = "DetailData3_Odd";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(7434.189F, 25F);
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell95,
            this.xrTableCell96,
            this.xrTableCell97,
            this.xrTableCell99,
            this.xrTableCell100,
            this.xrTableCell101,
            this.xrTableCell102,
            this.xrTableCell103,
            this.xrTableCell104,
            this.xrTableCell105,
            this.xrTableCell106,
            this.xrTableCell107,
            this.xrTableCell108,
            this.xrTableCell109,
            this.xrTableCell110,
            this.xrTableCell111,
            this.xrTableCell112,
            this.xrTableCell113,
            this.xrTableCell114,
            this.xrTableCell115,
            this.xrTableCell116,
            this.xrTableCell117,
            this.xrTableCell118,
            this.xrTableCell119,
            this.xrTableCell120,
            this.xrTableCell121,
            this.xrTableCell122,
            this.xrTableCell123,
            this.xrTableCell124,
            this.xrTableCell125,
            this.xrTableCell126,
            this.xrTableCell127,
            this.xrTableCell128,
            this.xrTableCell129,
            this.xrTableCell130,
            this.xrTableCell131,
            this.xrTableCell132,
            this.xrTableCell133,
            this.xrTableCell134,
            this.xrTableCell135,
            this.xrTableCell136,
            this.xrTableCell137,
            this.xrTableCell138,
            this.xrTableCell139,
            this.xrTableCell140,
            this.xrTableCell141,
            this.xrTableCell142,
            this.xrTableCell143,
            this.xrTableCell144,
            this.xrTableCell145,
            this.xrTableCell146,
            this.xrTableCell147,
            this.xrTableCell148,
            this.xrTableCell149,
            this.xrTableCell150,
            this.xrTableCell151,
            this.xrTableCell152,
            this.xrTableCell153,
            this.xrTableCell154,
            this.xrTableCell155,
            this.xrTableCell156,
            this.xrTableCell157,
            this.xrTableCell158,
            this.xrTableCell159,
            this.xrTableCell160,
            this.xrTableCell161,
            this.xrTableCell162,
            this.xrTableCell163,
            this.xrTableCell164,
            this.xrTableCell165,
            this.xrTableCell166,
            this.xrTableCell167,
            this.xrTableCell168,
            this.xrTableCell169,
            this.xrTableCell170,
            this.xrTableCell171,
            this.xrTableCell172,
            this.xrTableCell173,
            this.xrTableCell174,
            this.xrTableCell175,
            this.xrTableCell176,
            this.xrTableCell177,
            this.xrTableCell178,
            this.xrTableCell179,
            this.xrTableCell180,
            this.xrTableCell181,
            this.xrTableCell182,
            this.xrTableCell183,
            this.xrTableCell184,
            this.xrTableCell185,
            this.xrTableCell186,
            this.xrTableCell187,
            this.xrTableCell188});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 11.5D;
            // 
            // xrTableCell95
            // 
            this.xrTableCell95.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Id]")});
            this.xrTableCell95.Multiline = true;
            this.xrTableCell95.Name = "xrTableCell95";
            this.xrTableCell95.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell95.StyleName = "DetailData3";
            this.xrTableCell95.StylePriority.UseTextAlignment = false;
            this.xrTableCell95.Text = "xrTableCell95";
            this.xrTableCell95.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell95.Weight = 0.0044600655337080353D;
            // 
            // xrTableCell96
            // 
            this.xrTableCell96.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CrewChangeId]")});
            this.xrTableCell96.Multiline = true;
            this.xrTableCell96.Name = "xrTableCell96";
            this.xrTableCell96.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell96.StyleName = "DetailData3";
            this.xrTableCell96.StylePriority.UseTextAlignment = false;
            this.xrTableCell96.Text = "xrTableCell96";
            this.xrTableCell96.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell96.Weight = 0.010746983029148786D;
            // 
            // xrTableCell97
            // 
            this.xrTableCell97.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CrewFlightId]")});
            this.xrTableCell97.Multiline = true;
            this.xrTableCell97.Name = "xrTableCell97";
            this.xrTableCell97.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell97.StyleName = "DetailData3";
            this.xrTableCell97.StylePriority.UseTextAlignment = false;
            this.xrTableCell97.Text = "xrTableCell97";
            this.xrTableCell97.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell97.Weight = 0.0079155039749512178D;
            // 
            // xrTableCell99
            // 
            this.xrTableCell99.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Passport]")});
            this.xrTableCell99.Multiline = true;
            this.xrTableCell99.Name = "xrTableCell99";
            this.xrTableCell99.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell99.StyleName = "DetailData3";
            this.xrTableCell99.Text = "xrTableCell99";
            this.xrTableCell99.Weight = 0.009014140555023184D;
            // 
            // xrTableCell100
            // 
            this.xrTableCell100.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DisplayName]")});
            this.xrTableCell100.Multiline = true;
            this.xrTableCell100.Name = "xrTableCell100";
            this.xrTableCell100.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell100.StyleName = "DetailData3";
            this.xrTableCell100.Text = "xrTableCell100";
            this.xrTableCell100.Weight = 0.012722877017491682D;
            // 
            // xrTableCell101
            // 
            this.xrTableCell101.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[FirstName]")});
            this.xrTableCell101.Multiline = true;
            this.xrTableCell101.Name = "xrTableCell101";
            this.xrTableCell101.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell101.StyleName = "DetailData3";
            this.xrTableCell101.Text = "xrTableCell101";
            this.xrTableCell101.Weight = 0.0086812078035794771D;
            // 
            // xrTableCell102
            // 
            this.xrTableCell102.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LastName]")});
            this.xrTableCell102.Multiline = true;
            this.xrTableCell102.Name = "xrTableCell102";
            this.xrTableCell102.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell102.StyleName = "DetailData3";
            this.xrTableCell102.Text = "xrTableCell102";
            this.xrTableCell102.Weight = 0.0084879442361684952D;
            // 
            // xrTableCell103
            // 
            this.xrTableCell103.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[MiddleName]")});
            this.xrTableCell103.Multiline = true;
            this.xrTableCell103.Name = "xrTableCell103";
            this.xrTableCell103.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell103.StyleName = "DetailData3";
            this.xrTableCell103.Text = "xrTableCell103";
            this.xrTableCell103.Weight = 0.010145604060246394D;
            // 
            // xrTableCell104
            // 
            this.xrTableCell104.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Email]")});
            this.xrTableCell104.Multiline = true;
            this.xrTableCell104.Name = "xrTableCell104";
            this.xrTableCell104.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell104.StyleName = "DetailData3";
            this.xrTableCell104.Text = "xrTableCell104";
            this.xrTableCell104.Weight = 0.0051149049172034633D;
            // 
            // xrTableCell105
            // 
            this.xrTableCell105.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PrimaryPhone]")});
            this.xrTableCell105.Multiline = true;
            this.xrTableCell105.Name = "xrTableCell105";
            this.xrTableCell105.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell105.StyleName = "DetailData3";
            this.xrTableCell105.Text = "xrTableCell105";
            this.xrTableCell105.Weight = 0.011257195105919471D;
            // 
            // xrTableCell106
            // 
            this.xrTableCell106.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Status]")});
            this.xrTableCell106.Multiline = true;
            this.xrTableCell106.Name = "xrTableCell106";
            this.xrTableCell106.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell106.StyleName = "DetailData3";
            this.xrTableCell106.Text = "xrTableCell106";
            this.xrTableCell106.Weight = 0.005708316289461576D;
            // 
            // xrTableCell107
            // 
            this.xrTableCell107.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Nationality]")});
            this.xrTableCell107.Multiline = true;
            this.xrTableCell107.Name = "xrTableCell107";
            this.xrTableCell107.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell107.StyleName = "DetailData3";
            this.xrTableCell107.Text = "xrTableCell107";
            this.xrTableCell107.Weight = 0.0087888644291804381D;
            // 
            // xrTableCell108
            // 
            this.xrTableCell108.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Position]")});
            this.xrTableCell108.Multiline = true;
            this.xrTableCell108.Name = "xrTableCell108";
            this.xrTableCell108.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell108.StyleName = "DetailData3";
            this.xrTableCell108.Text = "xrTableCell108";
            this.xrTableCell108.Weight = 0.0068341754033015324D;
            // 
            // xrTableCell109
            // 
            this.xrTableCell109.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Department]")});
            this.xrTableCell109.Multiline = true;
            this.xrTableCell109.Name = "xrTableCell109";
            this.xrTableCell109.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell109.StyleName = "DetailData3";
            this.xrTableCell109.Text = "xrTableCell109";
            this.xrTableCell109.Weight = 0.0094451845609224757D;
            // 
            // xrTableCell110
            // 
            this.xrTableCell110.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Company]")});
            this.xrTableCell110.Multiline = true;
            this.xrTableCell110.Name = "xrTableCell110";
            this.xrTableCell110.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell110.StyleName = "DetailData3";
            this.xrTableCell110.Text = "xrTableCell110";
            this.xrTableCell110.Weight = 0.0076532774705153246D;
            // 
            // xrTableCell111
            // 
            this.xrTableCell111.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CompanyType]")});
            this.xrTableCell111.Multiline = true;
            this.xrTableCell111.Name = "xrTableCell111";
            this.xrTableCell111.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell111.StyleName = "DetailData3";
            this.xrTableCell111.Text = "xrTableCell111";
            this.xrTableCell111.Weight = 0.011244872900155875D;
            // 
            // xrTableCell112
            // 
            this.xrTableCell112.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PersonnelType]")});
            this.xrTableCell112.Multiline = true;
            this.xrTableCell112.Name = "xrTableCell112";
            this.xrTableCell112.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell112.StyleName = "DetailData3";
            this.xrTableCell112.Text = "xrTableCell112";
            this.xrTableCell112.Weight = 0.011656693678635817D;
            // 
            // xrTableCell113
            // 
            this.xrTableCell113.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ShortService]")});
            this.xrTableCell113.Multiline = true;
            this.xrTableCell113.Name = "xrTableCell113";
            this.xrTableCell113.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell113.StyleName = "DetailData3";
            this.xrTableCell113.Text = "xrTableCell113";
            this.xrTableCell113.Weight = 0.01032654542189378D;
            // 
            // xrTableCell114
            // 
            this.xrTableCell114.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DateOfBirth]")});
            this.xrTableCell114.Multiline = true;
            this.xrTableCell114.Name = "xrTableCell114";
            this.xrTableCell114.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell114.StyleName = "DetailData3";
            this.xrTableCell114.Text = "xrTableCell114";
            this.xrTableCell114.Weight = 0.010174787961519681D;
            // 
            // xrTableCell115
            // 
            this.xrTableCell115.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Essential]")});
            this.xrTableCell115.Multiline = true;
            this.xrTableCell115.Name = "xrTableCell115";
            this.xrTableCell115.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell115.StyleName = "DetailData3";
            this.xrTableCell115.Text = "xrTableCell115";
            this.xrTableCell115.Weight = 0.0074061848567082336D;
            // 
            // xrTableCell116
            // 
            this.xrTableCell116.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[VantageNumber]")});
            this.xrTableCell116.Multiline = true;
            this.xrTableCell116.Name = "xrTableCell116";
            this.xrTableCell116.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell116.StyleName = "DetailData3";
            this.xrTableCell116.Text = "xrTableCell116";
            this.xrTableCell116.Weight = 0.012680733020489032D;
            // 
            // xrTableCell117
            // 
            this.xrTableCell117.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Crew]")});
            this.xrTableCell117.Multiline = true;
            this.xrTableCell117.Name = "xrTableCell117";
            this.xrTableCell117.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell117.StyleName = "DetailData3";
            this.xrTableCell117.Text = "xrTableCell117";
            this.xrTableCell117.Weight = 0.0048671634380634014D;
            // 
            // xrTableCell118
            // 
            this.xrTableCell118.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Tour]")});
            this.xrTableCell118.Multiline = true;
            this.xrTableCell118.Name = "xrTableCell118";
            this.xrTableCell118.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell118.StyleName = "DetailData3";
            this.xrTableCell118.Text = "xrTableCell118";
            this.xrTableCell118.Weight = 0.0044936062739445613D;
            // 
            // xrTableCell119
            // 
            this.xrTableCell119.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Room]")});
            this.xrTableCell119.Multiline = true;
            this.xrTableCell119.Name = "xrTableCell119";
            this.xrTableCell119.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell119.StyleName = "DetailData3";
            this.xrTableCell119.Text = "xrTableCell119";
            this.xrTableCell119.Weight = 0.0053049259919386643D;
            // 
            // xrTableCell120
            // 
            this.xrTableCell120.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Bed]")});
            this.xrTableCell120.Multiline = true;
            this.xrTableCell120.Name = "xrTableCell120";
            this.xrTableCell120.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell120.StyleName = "DetailData3";
            this.xrTableCell120.Text = "xrTableCell120";
            this.xrTableCell120.Weight = 0.00397023787865272D;
            // 
            // xrTableCell121
            // 
            this.xrTableCell121.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[MusterStation1]")});
            this.xrTableCell121.Multiline = true;
            this.xrTableCell121.Name = "xrTableCell121";
            this.xrTableCell121.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell121.StyleName = "DetailData3";
            this.xrTableCell121.Text = "xrTableCell121";
            this.xrTableCell121.Weight = 0.012116505549504207D;
            // 
            // xrTableCell122
            // 
            this.xrTableCell122.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[MusterStation2]")});
            this.xrTableCell122.Multiline = true;
            this.xrTableCell122.Name = "xrTableCell122";
            this.xrTableCell122.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell122.StyleName = "DetailData3";
            this.xrTableCell122.Text = "xrTableCell122";
            this.xrTableCell122.Weight = 0.012116505549504207D;
            // 
            // xrTableCell123
            // 
            this.xrTableCell123.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PrimaryLifeBoat]")});
            this.xrTableCell123.Multiline = true;
            this.xrTableCell123.Name = "xrTableCell123";
            this.xrTableCell123.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell123.StyleName = "DetailData3";
            this.xrTableCell123.Text = "xrTableCell123";
            this.xrTableCell123.Weight = 0.012999811906080979D;
            // 
            // xrTableCell124
            // 
            this.xrTableCell124.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SecondaryLifeBoat]")});
            this.xrTableCell124.Multiline = true;
            this.xrTableCell124.Name = "xrTableCell124";
            this.xrTableCell124.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell124.StyleName = "DetailData3";
            this.xrTableCell124.Text = "xrTableCell124";
            this.xrTableCell124.Weight = 0.014719731257512018D;
            // 
            // xrTableCell125
            // 
            this.xrTableCell125.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[HomeAirport]")});
            this.xrTableCell125.Multiline = true;
            this.xrTableCell125.Name = "xrTableCell125";
            this.xrTableCell125.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell125.StyleName = "DetailData3";
            this.xrTableCell125.Text = "xrTableCell125";
            this.xrTableCell125.Weight = 0.010382968462430513D;
            // 
            // xrTableCell126
            // 
            this.xrTableCell126.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[BodyWeight]")});
            this.xrTableCell126.Multiline = true;
            this.xrTableCell126.Name = "xrTableCell126";
            this.xrTableCell126.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell126.StyleName = "DetailData3";
            this.xrTableCell126.Text = "xrTableCell126";
            this.xrTableCell126.Weight = 0.00994780026949369D;
            // 
            // xrTableCell127
            // 
            this.xrTableCell127.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[BagWeight]")});
            this.xrTableCell127.Multiline = true;
            this.xrTableCell127.Name = "xrTableCell127";
            this.xrTableCell127.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell127.StyleName = "DetailData3";
            this.xrTableCell127.Text = "xrTableCell127";
            this.xrTableCell127.Weight = 0.00915853133568397D;
            // 
            // xrTableCell128
            // 
            this.xrTableCell128.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Bags]")});
            this.xrTableCell128.Multiline = true;
            this.xrTableCell128.Name = "xrTableCell128";
            this.xrTableCell128.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell128.StyleName = "DetailData3";
            this.xrTableCell128.Text = "xrTableCell128";
            this.xrTableCell128.Weight = 0.00466028066781851D;
            // 
            // xrTableCell129
            // 
            this.xrTableCell129.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[AirlineFlight]")});
            this.xrTableCell129.Multiline = true;
            this.xrTableCell129.Name = "xrTableCell129";
            this.xrTableCell129.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell129.StyleName = "DetailData3";
            this.xrTableCell129.Text = "xrTableCell129";
            this.xrTableCell129.Weight = 0.0099614187387319711D;
            // 
            // xrTableCell130
            // 
            this.xrTableCell130.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Location]")});
            this.xrTableCell130.Multiline = true;
            this.xrTableCell130.Name = "xrTableCell130";
            this.xrTableCell130.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell130.StyleName = "DetailData3";
            this.xrTableCell130.Text = "xrTableCell130";
            this.xrTableCell130.Weight = 0.00713250233576848D;
            // 
            // xrTableCell131
            // 
            this.xrTableCell131.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Terminal]")});
            this.xrTableCell131.Multiline = true;
            this.xrTableCell131.Name = "xrTableCell131";
            this.xrTableCell131.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell131.StyleName = "DetailData3";
            this.xrTableCell131.Text = "xrTableCell131";
            this.xrTableCell131.Weight = 0.00732576663677509D;
            // 
            // xrTableCell132
            // 
            this.xrTableCell132.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DateStart]")});
            this.xrTableCell132.Multiline = true;
            this.xrTableCell132.Name = "xrTableCell132";
            this.xrTableCell132.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell132.StyleName = "DetailData3";
            this.xrTableCell132.Text = "xrTableCell132";
            this.xrTableCell132.Weight = 0.0082810607323279741D;
            // 
            // xrTableCell133
            // 
            this.xrTableCell133.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DateEnd]")});
            this.xrTableCell133.Multiline = true;
            this.xrTableCell133.Name = "xrTableCell133";
            this.xrTableCell133.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell133.StyleName = "DetailData3";
            this.xrTableCell133.Text = "xrTableCell133";
            this.xrTableCell133.Weight = 0.0074684451176570011D;
            // 
            // xrTableCell134
            // 
            this.xrTableCell134.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DateEstimatedLeave]")});
            this.xrTableCell134.Multiline = true;
            this.xrTableCell134.Name = "xrTableCell134";
            this.xrTableCell134.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell134.StyleName = "DetailData3";
            this.xrTableCell134.Text = "xrTableCell134";
            this.xrTableCell134.Weight = 0.015946763845590444D;
            // 
            // xrTableCell135
            // 
            this.xrTableCell135.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Authentication]")});
            this.xrTableCell135.Multiline = true;
            this.xrTableCell135.Name = "xrTableCell135";
            this.xrTableCell135.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell135.StyleName = "DetailData3";
            this.xrTableCell135.Text = "xrTableCell135";
            this.xrTableCell135.Weight = 0.011305834696843074D;
            // 
            // xrTableCell136
            // 
            this.xrTableCell136.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Optional1]")});
            this.xrTableCell136.Multiline = true;
            this.xrTableCell136.Name = "xrTableCell136";
            this.xrTableCell136.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell136.StyleName = "DetailData3";
            this.xrTableCell136.Text = "xrTableCell136";
            this.xrTableCell136.Weight = 0.0079574408897986771D;
            // 
            // xrTableCell137
            // 
            this.xrTableCell137.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Optional2]")});
            this.xrTableCell137.Multiline = true;
            this.xrTableCell137.Name = "xrTableCell137";
            this.xrTableCell137.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell137.StyleName = "DetailData3";
            this.xrTableCell137.Text = "xrTableCell137";
            this.xrTableCell137.Weight = 0.0079574408897986771D;
            // 
            // xrTableCell138
            // 
            this.xrTableCell138.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Optional3]")});
            this.xrTableCell138.Multiline = true;
            this.xrTableCell138.Name = "xrTableCell138";
            this.xrTableCell138.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell138.StyleName = "DetailData3";
            this.xrTableCell138.Text = "xrTableCell138";
            this.xrTableCell138.Weight = 0.0079574408897986771D;
            // 
            // xrTableCell139
            // 
            this.xrTableCell139.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Optional4]")});
            this.xrTableCell139.Multiline = true;
            this.xrTableCell139.Name = "xrTableCell139";
            this.xrTableCell139.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell139.StyleName = "DetailData3";
            this.xrTableCell139.Text = "xrTableCell139";
            this.xrTableCell139.Weight = 0.0079574408897986771D;
            // 
            // xrTableCell140
            // 
            this.xrTableCell140.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Role]")});
            this.xrTableCell140.Multiline = true;
            this.xrTableCell140.Name = "xrTableCell140";
            this.xrTableCell140.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell140.StyleName = "DetailData3";
            this.xrTableCell140.Text = "xrTableCell140";
            this.xrTableCell140.Weight = 0.004407999698932354D;
            // 
            // xrTableCell141
            // 
            this.xrTableCell141.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DateCreated]")});
            this.xrTableCell141.Multiline = true;
            this.xrTableCell141.Name = "xrTableCell141";
            this.xrTableCell141.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell141.StyleName = "DetailData3";
            this.xrTableCell141.Text = "xrTableCell141";
            this.xrTableCell141.Weight = 0.010187758665818435D;
            // 
            // xrTableCell142
            // 
            this.xrTableCell142.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DateModified]")});
            this.xrTableCell142.Multiline = true;
            this.xrTableCell142.Name = "xrTableCell142";
            this.xrTableCell142.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell142.StyleName = "DetailData3";
            this.xrTableCell142.Text = "xrTableCell142";
            this.xrTableCell142.Weight = 0.010740960928109977D;
            // 
            // xrTableCell143
            // 
            this.xrTableCell143.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ModifiedBy]")});
            this.xrTableCell143.Multiline = true;
            this.xrTableCell143.Name = "xrTableCell143";
            this.xrTableCell143.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell143.StyleName = "DetailData3";
            this.xrTableCell143.Text = "xrTableCell143";
            this.xrTableCell143.Weight = 0.0092752684079683741D;
            // 
            // xrTableCell144
            // 
            this.xrTableCell144.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PassportIssuer]")});
            this.xrTableCell144.Multiline = true;
            this.xrTableCell144.Name = "xrTableCell144";
            this.xrTableCell144.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell144.StyleName = "DetailData3";
            this.xrTableCell144.Text = "xrTableCell144";
            this.xrTableCell144.Weight = 0.011866171176616962D;
            // 
            // xrTableCell145
            // 
            this.xrTableCell145.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PassportNumber]")});
            this.xrTableCell145.Multiline = true;
            this.xrTableCell145.Name = "xrTableCell145";
            this.xrTableCell145.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell145.StyleName = "DetailData3";
            this.xrTableCell145.Text = "xrTableCell145";
            this.xrTableCell145.Weight = 0.012986193436842698D;
            // 
            // xrTableCell146
            // 
            this.xrTableCell146.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PassportExpires]")});
            this.xrTableCell146.Multiline = true;
            this.xrTableCell146.Name = "xrTableCell146";
            this.xrTableCell146.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell146.StyleName = "DetailData3";
            this.xrTableCell146.Text = "xrTableCell146";
            this.xrTableCell146.Weight = 0.012547134986290565D;
            // 
            // xrTableCell147
            // 
            this.xrTableCell147.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[VisaNumber]")});
            this.xrTableCell147.Multiline = true;
            this.xrTableCell147.Name = "xrTableCell147";
            this.xrTableCell147.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell147.StyleName = "DetailData3";
            this.xrTableCell147.Text = "xrTableCell147";
            this.xrTableCell147.Weight = 0.0099432600461519683D;
            // 
            // xrTableCell148
            // 
            this.xrTableCell148.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[VisaExpires]")});
            this.xrTableCell148.Multiline = true;
            this.xrTableCell148.Name = "xrTableCell148";
            this.xrTableCell148.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell148.StyleName = "DetailData3";
            this.xrTableCell148.Text = "xrTableCell148";
            this.xrTableCell148.Weight = 0.0095042008620042067D;
            // 
            // xrTableCell149
            // 
            this.xrTableCell149.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[IdentificationIssuer]")});
            this.xrTableCell149.Multiline = true;
            this.xrTableCell149.Name = "xrTableCell149";
            this.xrTableCell149.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell149.StyleName = "DetailData3";
            this.xrTableCell149.Text = "xrTableCell149";
            this.xrTableCell149.Weight = 0.015143875708946816D;
            // 
            // xrTableCell150
            // 
            this.xrTableCell150.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Identification]")});
            this.xrTableCell150.Multiline = true;
            this.xrTableCell150.Name = "xrTableCell150";
            this.xrTableCell150.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell150.StyleName = "DetailData3";
            this.xrTableCell150.Text = "xrTableCell150";
            this.xrTableCell150.Weight = 0.010530834931593675D;
            // 
            // xrTableCell151
            // 
            this.xrTableCell151.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[IdentificationExpires]")});
            this.xrTableCell151.Multiline = true;
            this.xrTableCell151.Name = "xrTableCell151";
            this.xrTableCell151.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell151.StyleName = "DetailData3";
            this.xrTableCell151.Text = "xrTableCell151";
            this.xrTableCell151.Weight = 0.015824837317833535D;
            // 
            // xrTableCell152
            // 
            this.xrTableCell152.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Comment]")});
            this.xrTableCell152.Multiline = true;
            this.xrTableCell152.Name = "xrTableCell152";
            this.xrTableCell152.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell152.StyleName = "DetailData3";
            this.xrTableCell152.Text = "xrTableCell152";
            this.xrTableCell152.Weight = 0.0078647004641019379D;
            // 
            // xrTableCell153
            // 
            this.xrTableCell153.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[AddressLine1]")});
            this.xrTableCell153.Multiline = true;
            this.xrTableCell153.Name = "xrTableCell153";
            this.xrTableCell153.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell153.StyleName = "DetailData3";
            this.xrTableCell153.Text = "xrTableCell153";
            this.xrTableCell153.Weight = 0.010781818536611703D;
            // 
            // xrTableCell154
            // 
            this.xrTableCell154.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[AddressLine2]")});
            this.xrTableCell154.Multiline = true;
            this.xrTableCell154.Name = "xrTableCell154";
            this.xrTableCell154.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell154.StyleName = "DetailData3";
            this.xrTableCell154.Text = "xrTableCell154";
            this.xrTableCell154.Weight = 0.010781818536611703D;
            // 
            // xrTableCell155
            // 
            this.xrTableCell155.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[City]")});
            this.xrTableCell155.Multiline = true;
            this.xrTableCell155.Name = "xrTableCell155";
            this.xrTableCell155.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell155.StyleName = "DetailData3";
            this.xrTableCell155.Text = "xrTableCell155";
            this.xrTableCell155.Weight = 0.00403833425962008D;
            // 
            // xrTableCell156
            // 
            this.xrTableCell156.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[State]")});
            this.xrTableCell156.Multiline = true;
            this.xrTableCell156.Name = "xrTableCell156";
            this.xrTableCell156.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell156.StyleName = "DetailData3";
            this.xrTableCell156.Text = "xrTableCell156";
            this.xrTableCell156.Weight = 0.0049631474568293645D;
            // 
            // xrTableCell157
            // 
            this.xrTableCell157.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Country]")});
            this.xrTableCell157.Multiline = true;
            this.xrTableCell157.Name = "xrTableCell157";
            this.xrTableCell157.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell157.StyleName = "DetailData3";
            this.xrTableCell157.Text = "xrTableCell157";
            this.xrTableCell157.Weight = 0.00673365226158729D;
            // 
            // xrTableCell158
            // 
            this.xrTableCell158.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PostalCode]")});
            this.xrTableCell158.Multiline = true;
            this.xrTableCell158.Name = "xrTableCell158";
            this.xrTableCell158.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell158.StyleName = "DetailData3";
            this.xrTableCell158.Text = "xrTableCell158";
            this.xrTableCell158.Weight = 0.009278510167048528D;
            // 
            // xrTableCell159
            // 
            this.xrTableCell159.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SecondaryPhone]")});
            this.xrTableCell159.Multiline = true;
            this.xrTableCell159.Name = "xrTableCell159";
            this.xrTableCell159.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell159.StyleName = "DetailData3";
            this.xrTableCell159.Text = "xrTableCell159";
            this.xrTableCell159.Weight = 0.012977115924541767D;
            // 
            // xrTableCell160
            // 
            this.xrTableCell160.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[MaritalStatus]")});
            this.xrTableCell160.Multiline = true;
            this.xrTableCell160.Name = "xrTableCell160";
            this.xrTableCell160.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell160.StyleName = "DetailData3";
            this.xrTableCell160.Text = "xrTableCell160";
            this.xrTableCell160.Weight = 0.010803220455463116D;
            // 
            // xrTableCell161
            // 
            this.xrTableCell161.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PersonalEmail]")});
            this.xrTableCell161.Multiline = true;
            this.xrTableCell161.Name = "xrTableCell161";
            this.xrTableCell161.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell161.StyleName = "DetailData3";
            this.xrTableCell161.Text = "xrTableCell161";
            this.xrTableCell161.Weight = 0.011290920697725736D;
            // 
            // xrTableCell162
            // 
            this.xrTableCell162.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ContactInfoComment]")});
            this.xrTableCell162.Multiline = true;
            this.xrTableCell162.Name = "xrTableCell162";
            this.xrTableCell162.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell162.StyleName = "DetailData3";
            this.xrTableCell162.Text = "xrTableCell162";
            this.xrTableCell162.Weight = 0.016618647942176233D;
            // 
            // xrTableCell163
            // 
            this.xrTableCell163.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmergencyContactFirstName]")});
            this.xrTableCell163.Multiline = true;
            this.xrTableCell163.Name = "xrTableCell163";
            this.xrTableCell163.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell163.StyleName = "DetailData3";
            this.xrTableCell163.Text = "xrTableCell163";
            this.xrTableCell163.Weight = 0.022003446725698619D;
            // 
            // xrTableCell164
            // 
            this.xrTableCell164.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmergencyContactLastName]")});
            this.xrTableCell164.Multiline = true;
            this.xrTableCell164.Name = "xrTableCell164";
            this.xrTableCell164.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell164.StyleName = "DetailData3";
            this.xrTableCell164.Text = "xrTableCell164";
            this.xrTableCell164.Weight = 0.021810183891883262D;
            // 
            // xrTableCell165
            // 
            this.xrTableCell165.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmergencyContactRelationship]")});
            this.xrTableCell165.Multiline = true;
            this.xrTableCell165.Name = "xrTableCell165";
            this.xrTableCell165.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell165.StyleName = "DetailData3";
            this.xrTableCell165.Text = "xrTableCell165";
            this.xrTableCell165.Weight = 0.023098825307992787D;
            // 
            // xrTableCell166
            // 
            this.xrTableCell166.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmergencyContactEmail]")});
            this.xrTableCell166.Multiline = true;
            this.xrTableCell166.Name = "xrTableCell166";
            this.xrTableCell166.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell166.StyleName = "DetailData3";
            this.xrTableCell166.Text = "xrTableCell166";
            this.xrTableCell166.Weight = 0.01843714347252479D;
            // 
            // xrTableCell167
            // 
            this.xrTableCell167.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmergencyContactPrimaryPhone]")});
            this.xrTableCell167.Multiline = true;
            this.xrTableCell167.Name = "xrTableCell167";
            this.xrTableCell167.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell167.StyleName = "DetailData3";
            this.xrTableCell167.Text = "xrTableCell167";
            this.xrTableCell167.Weight = 0.02457943402803861D;
            // 
            // xrTableCell168
            // 
            this.xrTableCell168.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmergencyContactSecondaryPhone]")});
            this.xrTableCell168.Multiline = true;
            this.xrTableCell168.Name = "xrTableCell168";
            this.xrTableCell168.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell168.StyleName = "DetailData3";
            this.xrTableCell168.Text = "xrTableCell168";
            this.xrTableCell168.Weight = 0.026299356313852162D;
            // 
            // xrTableCell169
            // 
            this.xrTableCell169.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmergencyContactComment]")});
            this.xrTableCell169.Multiline = true;
            this.xrTableCell169.Name = "xrTableCell169";
            this.xrTableCell169.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell169.StyleName = "DetailData3";
            this.xrTableCell169.Text = "xrTableCell169";
            this.xrTableCell169.Weight = 0.021186941587007964D;
            // 
            // xrTableCell170
            // 
            this.xrTableCell170.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[VehicleMake]")});
            this.xrTableCell170.Multiline = true;
            this.xrTableCell170.Name = "xrTableCell170";
            this.xrTableCell170.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell170.StyleName = "DetailData3";
            this.xrTableCell170.Text = "xrTableCell170";
            this.xrTableCell170.Weight = 0.010219537294827975D;
            // 
            // xrTableCell171
            // 
            this.xrTableCell171.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[VehicleModel]")});
            this.xrTableCell171.Multiline = true;
            this.xrTableCell171.Name = "xrTableCell171";
            this.xrTableCell171.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell171.StyleName = "DetailData3";
            this.xrTableCell171.Text = "xrTableCell171";
            this.xrTableCell171.Weight = 0.010680646896362304D;
            // 
            // xrTableCell172
            // 
            this.xrTableCell172.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[VehicleColor]")});
            this.xrTableCell172.Multiline = true;
            this.xrTableCell172.Name = "xrTableCell172";
            this.xrTableCell172.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell172.StyleName = "DetailData3";
            this.xrTableCell172.Text = "xrTableCell172";
            this.xrTableCell172.Weight = 0.010152089779193584D;
            // 
            // xrTableCell173
            // 
            this.xrTableCell173.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[VehiclePlateIssuer]")});
            this.xrTableCell173.Multiline = true;
            this.xrTableCell173.Name = "xrTableCell173";
            this.xrTableCell173.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell173.StyleName = "DetailData3";
            this.xrTableCell173.Text = "xrTableCell173";
            this.xrTableCell173.Weight = 0.0146717408987192D;
            // 
            // xrTableCell174
            // 
            this.xrTableCell174.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[VehiclePlateNumber]")});
            this.xrTableCell174.Multiline = true;
            this.xrTableCell174.Name = "xrTableCell174";
            this.xrTableCell174.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell174.StyleName = "DetailData3";
            this.xrTableCell174.Text = "xrTableCell174";
            this.xrTableCell174.Weight = 0.015791762425349309D;
            // 
            // xrTableCell175
            // 
            this.xrTableCell175.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[VehicleLocation]")});
            this.xrTableCell175.Multiline = true;
            this.xrTableCell175.Name = "xrTableCell175";
            this.xrTableCell175.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell175.StyleName = "DetailData3";
            this.xrTableCell175.Text = "xrTableCell175";
            this.xrTableCell175.Weight = 0.012347384232741137D;
            // 
            // xrTableCell176
            // 
            this.xrTableCell176.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[VehicleComment]")});
            this.xrTableCell176.Multiline = true;
            this.xrTableCell176.Name = "xrTableCell176";
            this.xrTableCell176.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell176.StyleName = "DetailData3";
            this.xrTableCell176.Text = "xrTableCell176";
            this.xrTableCell176.Weight = 0.013079581627478966D;
            // 
            // xrTableCell177
            // 
            this.xrTableCell177.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[WorkBoots]")});
            this.xrTableCell177.Multiline = true;
            this.xrTableCell177.Name = "xrTableCell177";
            this.xrTableCell177.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell177.StyleName = "DetailData3";
            this.xrTableCell177.Text = "xrTableCell177";
            this.xrTableCell177.Weight = 0.0091708535414475652D;
            // 
            // xrTableCell178
            // 
            this.xrTableCell178.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[RubberBoots]")});
            this.xrTableCell178.Multiline = true;
            this.xrTableCell178.Name = "xrTableCell178";
            this.xrTableCell178.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell178.StyleName = "DetailData3";
            this.xrTableCell178.Text = "xrTableCell178";
            this.xrTableCell178.Weight = 0.010467277673574594D;
            // 
            // xrTableCell179
            // 
            this.xrTableCell179.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Gloves]")});
            this.xrTableCell179.Multiline = true;
            this.xrTableCell179.Name = "xrTableCell179";
            this.xrTableCell179.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell179.StyleName = "DetailData3";
            this.xrTableCell179.Text = "xrTableCell179";
            this.xrTableCell179.Weight = 0.0058853670266958381D;
            // 
            // xrTableCell180
            // 
            this.xrTableCell180.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[TShirt]")});
            this.xrTableCell180.Multiline = true;
            this.xrTableCell180.Name = "xrTableCell180";
            this.xrTableCell180.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell180.StyleName = "DetailData3";
            this.xrTableCell180.Text = "xrTableCell180";
            this.xrTableCell180.Weight = 0.0054677090278038611D;
            // 
            // xrTableCell181
            // 
            this.xrTableCell181.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CoverallTan]")});
            this.xrTableCell181.Multiline = true;
            this.xrTableCell181.Name = "xrTableCell181";
            this.xrTableCell181.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell181.StyleName = "DetailData3";
            this.xrTableCell181.Text = "xrTableCell181";
            this.xrTableCell181.Weight = 0.0097182178497314461D;
            // 
            // xrTableCell182
            // 
            this.xrTableCell182.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CoverallFRC]")});
            this.xrTableCell182.Multiline = true;
            this.xrTableCell182.Name = "xrTableCell182";
            this.xrTableCell182.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell182.StyleName = "DetailData3";
            this.xrTableCell182.Text = "xrTableCell182";
            this.xrTableCell182.Weight = 0.00988229824946477D;
            // 
            // xrTableCell183
            // 
            this.xrTableCell183.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CoverallChemical]")});
            this.xrTableCell183.Multiline = true;
            this.xrTableCell183.Name = "xrTableCell183";
            this.xrTableCell183.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell183.StyleName = "DetailData3";
            this.xrTableCell183.Text = "xrTableCell183";
            this.xrTableCell183.Weight = 0.013348726125863882D;
            // 
            // xrTableCell184
            // 
            this.xrTableCell184.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CoverallWinter]")});
            this.xrTableCell184.Multiline = true;
            this.xrTableCell184.Name = "xrTableCell184";
            this.xrTableCell184.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell184.StyleName = "DetailData3";
            this.xrTableCell184.Text = "xrTableCell184";
            this.xrTableCell184.Weight = 0.01179223867563101D;
            // 
            // xrTableCell185
            // 
            this.xrTableCell185.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PPEComment]")});
            this.xrTableCell185.Multiline = true;
            this.xrTableCell185.Name = "xrTableCell185";
            this.xrTableCell185.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell185.StyleName = "DetailData3";
            this.xrTableCell185.Text = "xrTableCell185";
            this.xrTableCell185.Weight = 0.010427717062143179D;
            // 
            // xrTableCell186
            // 
            this.xrTableCell186.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Gender]")});
            this.xrTableCell186.Multiline = true;
            this.xrTableCell186.Name = "xrTableCell186";
            this.xrTableCell186.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell186.StyleName = "DetailData3";
            this.xrTableCell186.Text = "xrTableCell186";
            this.xrTableCell186.Weight = 0.0062634636805607722D;
            // 
            // xrTableCell187
            // 
            this.xrTableCell187.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LockerNumber]")});
            this.xrTableCell187.Multiline = true;
            this.xrTableCell187.Name = "xrTableCell187";
            this.xrTableCell187.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell187.StyleName = "DetailData3";
            this.xrTableCell187.Text = "xrTableCell187";
            this.xrTableCell187.Weight = 0.011612593577458308D;
            // 
            // xrTableCell188
            // 
            this.xrTableCell188.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ISNNumber]")});
            this.xrTableCell188.Multiline = true;
            this.xrTableCell188.Name = "xrTableCell188";
            this.xrTableCell188.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell188.StyleName = "DetailData3";
            this.xrTableCell188.Text = "xrTableCell188";
            this.xrTableCell188.Weight = 0.00928466796875D;
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
            // xrPanel2
            // 
            this.xrPanel2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1,
            this.xrLabel9,
            this.xrLabel1,
            this.xrPageInfo3,
            this.xrPictureBox2});
            this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPanel2.Name = "xrPanel2";
            this.xrPanel2.SizeF = new System.Drawing.SizeF(998.9999F, 64.62264F);
            this.xrPanel2.StylePriority.UseBorders = false;
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
            // 
            // xrLabel9
            // 
            this.xrLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(109)))), ((int)(((byte)(31)))));
            this.xrLabel9.Font = new System.Drawing.Font("Arial", 14.26415F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel9.ForeColor = System.Drawing.Color.White;
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(138.1132F, 4.830155F);
            this.xrLabel9.Multiline = true;
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(715.0671F, 25.55338F);
            this.xrLabel9.StyleName = "Title";
            this.xrLabel9.StylePriority.UseBackColor = false;
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseForeColor = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "Emergency Teams";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Parameters].[RigName]")});
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 12.22642F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(138.1132F, 31.01884F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(242.1887F, 28.88681F);
            this.xrLabel1.StylePriority.UseBorders = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPageInfo3
            // 
            this.xrPageInfo3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPageInfo3.Font = new System.Drawing.Font("Arial", 12.22642F, System.Drawing.FontStyle.Bold);
            this.xrPageInfo3.LocationFloat = new DevExpress.Utils.PointFloat(696.0566F, 31.01884F);
            this.xrPageInfo3.Name = "xrPageInfo3";
            this.xrPageInfo3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo3.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo3.SizeF = new System.Drawing.SizeF(157.1237F, 28.88681F);
            this.xrPageInfo3.StylePriority.UseBorders = false;
            this.xrPageInfo3.StylePriority.UseFont = false;
            this.xrPageInfo3.StylePriority.UseTextAlignment = false;
            this.xrPageInfo3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrPageInfo3.TextFormatString = "{0:dd-MMM-yyyy}";
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
            // 
            // PersonnelSearch
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.reportHeaderBand1,
            this.groupHeaderBand1});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataSource = this.objectDataSource1;
            this.Landscape = true;
            this.PageHeight = 850;
            this.PageWidth = 7700;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.DetailCaption3,
            this.DetailData3,
            this.DetailData3_Odd,
            this.DetailCaptionBackground3,
            this.PageInfo});
            this.Version = "18.1";
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}

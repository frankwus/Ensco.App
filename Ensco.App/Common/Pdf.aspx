<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pdf.aspx.cs" Inherits="Pdf" %>
<%@ Reference Page="~/Base.aspx" %>
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <style>
    </style>
    <script type="text/javascript" src="../js/jquery.js"></script>
    <script type="text/javascript" src="../base.js?a=1"></script>
    <link rel="stylesheet" href="../js/jquery-ui.css"></link>
    <script type="text/javascript" src="../js/jquery-ui.js"></script>
</head>
<form id="form1" runat="server">
</form>
<object id="Object1" name="Pdf2"
    type="application/pdf" width="1" height="1">
    <param name='SRC' value='<%= FileName %>' />
</object>

<embed src="test.pdf" id="Pdf1" name="Pdf1" hidden>
<a onclick="document.getElementById('Pdf1').printWithDialog()" style="cursor:hand;">Print file</a>

<script language="JavaScript">
    $(document).ready(function () {
        if ($('param').val() != '')
            document.Pdf2.printAll()
    })
    function PrintPdf() {
        if ($('param').val() != '')
            document.Pdf2.printWithDialog()
    }

</script>

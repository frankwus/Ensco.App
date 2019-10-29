
dim URL, oArgs, objXML
Set oArgs = WScript.Arguments
URL = "https://irma122.enscoplc.com/job/summary.aspx"

'URL = oArgs(0)
on error resume next

Set objXML = CreateObject("Microsoft.XMLDOM")
objXML.async = "false"
objXML.load(URL)
Set objXML = Nothing  
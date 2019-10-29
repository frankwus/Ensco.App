
dim URL, oArgs, objXML
Set oArgs = WScript.Arguments
URL = "http://localhost/emoc/empDirectory2.aspx?loadAll=1" 'oArgs(0)

'URL = oArgs(0)
on error resume next

Set objXML = CreateObject("Microsoft.XMLDOM")
objXML.async = "false"
objXML.load(URL)
Set objXML = Nothing  
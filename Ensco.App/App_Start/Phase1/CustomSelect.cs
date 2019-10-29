using System.Xml;
using System.Xml.Linq;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml.XPath;

using System;
using System.IO;
using System.Collections;

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

using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;

namespace CustomSelect {
    [
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal),
    DefaultEvent("Submit"),
    DefaultProperty("ButtonText"),
    ToolboxData("<{0}:Register runat=\"server\"> </{0}:Register>"),
    ]
    public class MyDropDownList : CompositeControl {
        XDocument XmlDoc;
        TextBox TextBox1 = new TextBox();
        ImageButton ImageButton1 = new ImageButton();

        LiteralControl LiteralControl1 = new LiteralControl(@"<img id=RigImg width=20 src=Images/downArrow.png />");
        DataTable DataTable1;
        string xml;
        string[] arr;
        public DataTable Data {
            get {
                EnsureChildControls();

                return null;
            }
            set {
                EnsureChildControls();
                this.DataTable1 = value;
                this.CreateChildControls();

            }
        }
        public string[] Arr {
            get {
                EnsureChildControls();

                return null;
            }
            set {
                EnsureChildControls();
                this.arr = value;
                this.CreateChildControls();

            }
        }
        [
        Bindable(true),
        Category("Default"),
        DefaultValue(""),
        Description("The user name.")
        ]
        public string XML {
            get {
                EnsureChildControls();
                return "";
            }
            set {
                EnsureChildControls();
                this.xml = value;
                this.CreateChildControls();
            }
        }

        protected override void CreateChildControls() {

            Controls.Clear();
            this.TextBox1.ID = "test"; 
            //this.Controls.Add(this.TextBox1);
            this.Controls.Add(this.LiteralControl1);
            if (this.xml != null)
                this.Controls.Add(this.GetFrame());
        }
        LiteralControl GetFrame() {
            string s = "";// 
            this.XmlDoc = XDocument.Parse(this.xml);
            foreach (XElement el in this.XmlDoc.Descendants("id")) {
                s += @"<div><input type=checkbox id=""" + el.Value + @""" />" + ((XElement)el.NextNode).Value + "  </div>";
            }
            s = string.Format(@"<iframe id="+this.ID+ @" style=""display:none"" data='{0}' >", s);
            s += "</iframe>";
            return new LiteralControl(s);
        }

        protected override void Render(HtmlTextWriter writer) {
            AddAttributesToRender(writer);

            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "1", false);

            this.MyRender(writer, "<table><tr><td><input type=text />");
            this.MyRender(writer, "<td>");
            this.LiteralControl1.RenderControl(writer);

            if (this.xml != null)
                this.GetFrame().RenderControl(writer);
            this.MyRender(writer, "</table>");
        }
        void MyRender(HtmlTextWriter writer, string s) {
            LiteralControl l = new LiteralControl(s);
            l.RenderControl(writer);
        }
    }
}
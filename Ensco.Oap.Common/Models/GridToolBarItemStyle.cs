using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Ensco.Irma.Oap.Common.Models
{
    public class GridToolBarItemStyle
    {
        public GridToolBarItemStyle():this(Color.RoyalBlue, Color.White, Color.Transparent,BorderStyle.None)
        {

        }
        public GridToolBarItemStyle(Color backColor, Color foreColor, Color ItemStyleForeColor, BorderStyle borderStyle)
        {
            BackColor = backColor;
            ForeColor = foreColor;
            this.ItemStyleForeColor = ItemStyleForeColor;
            BorderStyle = borderStyle;
        }

        public Color BackColor { get; }
        public Color ForeColor { get; }
        public Color ItemStyleForeColor { get; }
        public BorderStyle BorderStyle { get; }
    }
}
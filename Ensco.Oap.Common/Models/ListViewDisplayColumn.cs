using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Web.Mvc;

namespace Ensco.Irma.Oap.Common.Models
{
    public class ListViewDisplayColumn : GridBaseColumn
    {
        public ListViewDisplayColumn(string name, string displayName = "", MVCxGridViewColumnType columnType = MVCxGridViewColumnType.TextBox, int width = 20, int height = 10, bool isVisible = true, bool isWidthAndHeightInPercentage = true) : base(name, displayName, columnType, width, height, isVisible, isWidthAndHeightInPercentage)
        {
        }
    }
}
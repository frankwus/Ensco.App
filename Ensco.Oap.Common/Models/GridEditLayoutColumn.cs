using DevExpress.Utils;
using DevExpress.Web.Mvc;
using System;

namespace Ensco.Irma.Oap.Common.Models
{

    public class GridEditLayoutColumn : GridBaseColumn
    {
        public GridEditLayoutColumn(string name, string displayName = "", MVCxGridViewColumnType columnType = MVCxGridViewColumnType.TextBox, int? width = null, int height = 10, bool isVisible = true, bool isWidthAndHeightInPercentage = true, Action<MVCxGridViewColumnLayoutItem> layoutAction = null, DefaultBoolean allowEditLayout = DefaultBoolean.True) : base(name, displayName, columnType, width, height, isVisible, isWidthAndHeightInPercentage, allowEditLayout)
        {
            LayoutAction = layoutAction;
        }

        public Action<MVCxGridViewColumnLayoutItem> LayoutAction { get; }
    }
}

using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Oap.Common.Models
{
    public class GridSettingOptions
    {
        public GridSettingOptions(bool showTitle = false, ScrollBarMode horizontalScrollBarMode = ScrollBarMode.Hidden,ScrollBarMode verticalScrollBarMode = ScrollBarMode.Hidden, int? verticalScrollableHeight = null)
        {
            ShowTitle = showTitle;
            HorizontalScrollBarMode = horizontalScrollBarMode;
            VerticalScrollBarMode = verticalScrollBarMode;
            VerticalScrollableHeight = verticalScrollableHeight;
        }

        public bool ShowTitle { get; }
        public ScrollBarMode HorizontalScrollBarMode { get; }
        public ScrollBarMode VerticalScrollBarMode { get; }
        public int? VerticalScrollableHeight { get; }
    }
}

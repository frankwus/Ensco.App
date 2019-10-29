using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ensco.Irma.Oap.Common.Models
{
    public class GridCommandButtonOptions
    {
        public GridCommandButtonOptions(bool show, int width = 10, bool displayAddButtonInGridHeader = true, bool displayAddButton = false, bool displayEditButton = true, bool displayDeleteButton = true, bool displayUpdateButton = true, bool displayCancelButton = true )
        {
            Show = show;
            Width = width;
            DisplayAddButtonInGridHeader = displayAddButtonInGridHeader;
            DisplayAddButton = displayAddButton;
            DisplayEditButton = displayEditButton;
            DisplayDeleteButton = displayDeleteButton;
            DisplayUpdateButton = displayUpdateButton;
            DisplayCancelButton = displayCancelButton;
        }
        public bool Show { get;  set; }

        public bool DisplayAddButtonInGridHeader { get;  set; }

        public bool DisplayAddButton { get;  set; }

        public bool DisplayUpdateButton { get; set; }
        public bool DisplayDeleteButton { get;  set; }

        public bool DisplayCancelButton { get;  set; }

        public bool DisplayEditButton { get;  set; }

        public int Width { get; set; }
    }
}
using Ensco.Irma.Oap.Common.Extensions;

namespace Ensco.Irma.Oap.Common.Models
{
    public class GridToolBarOptions
    {
        private bool displaySearchPanel;
        private bool displayRefresh;
        private bool displayFilter;
        private bool displayClearFilter;
        private bool displayPdfExport;
        private bool displayXlsExport;

        public GridToolBarOptions(bool show, int width = 24, int height = 24, string searchName = "search", string searchText = "", bool displayCustomAddNew = true,string addNewToolbarButtonName = "AddNew", string addNewToolbarButtonText = "Add New", bool displaySearchPanel = true, bool displayRefresh = true, bool displayFilter = true, bool displayClearFilter = true, bool displayPdfExport = false, bool displayXlsExport = false, GridToolBarItemStyle addToolBarItemStyle = null)
        {
            Show = show;
            Width = width;
            Height = height;
            SearchName = searchName.Translate();
            DisplayCustomAddNew = displayCustomAddNew;
            AddNewToolbarButtonName = addNewToolbarButtonName.Translate();
            AddNewToolbarButtonText = addNewToolbarButtonText.Translate();
            AddToolBarItemStyle = addToolBarItemStyle;
            SearchText = (string.IsNullOrEmpty(searchText)? "search": searchText).Translate();
            DisplaySearchPanel = displaySearchPanel && show;
            DisplayRefresh = displayRefresh && show;
            DisplayFilter = displayFilter && show;
            DisplayClearFilter = displayClearFilter && show;
            DisplayPdfExport = displayPdfExport && show;
            DisplayXlsExport = displayXlsExport && show;
        }

        public bool Show { get; set; } 

        public bool DisplaySearchPanel
        {
            get
            {
                return displaySearchPanel && Show;
            }
            set
            {
                displaySearchPanel = value;
            }
        }

        public bool DisplayRefresh
        {
            get
            {
                return displayRefresh && Show;
            }
            set
            {
                displayRefresh = value;
            }
        }

        public bool DisplayFilter
        {
            get
            {
                return displayFilter && Show;
            }
            set
            {
                displayFilter = value;
            }
        }

        public bool DisplayClearFilter
        {
            get
            {
                return displayClearFilter && Show;
            }
            set
            {
                displayClearFilter = value;
            }
        }

        public bool DisplayPdfExport
        {
            get
            {
                return displayPdfExport && Show;
            }
            set
            {
                displayPdfExport = value;
            }
        }

        public bool DisplayXlsExport
        {
            get
            {
                return displayXlsExport && Show;
            }
            set
            {
                displayXlsExport = value;
            }
        }
 
        public int Width { get; set; }

        public int Height { get; set; }

        public string SearchName { get; set; }

        public bool DisplayCustomAddNew { get; set; }

        public string AddNewToolbarButtonName { get; set; }

        public string AddNewToolbarButtonText { get; set; }

        public GridToolBarItemStyle AddToolBarItemStyle { get; }

        public string SearchText { get; set; }
    }
}
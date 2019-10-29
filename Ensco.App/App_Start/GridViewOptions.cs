using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ensco.App.App_Start
{
    public class GridViewOptions
    {
        public bool ShowToolbar { get; set; }
        public bool ShowPager { get; set; }
        public bool ShowCustomizationWindow { get; set; }
        public bool ShowCommandColumn { get; set; }        
        public bool ShowAddButton { get; set; }
        public string AddButtonText { get; set; }
        public bool ShowEditButton { get; set; }
        public string EditButtonImage { get; set; }
        public bool ShowDeleteButton { get; set; }
        public bool ShowTitle { get; set; }
        public string Title { get; set; }        
        public GridViewOptions()
        {
            ShowToolbar = true;
            ShowPager = true;
            ShowCustomizationWindow = false;
            ShowCommandColumn = false;
            ShowAddButton = false;
            ShowEditButton = false;
            ShowDeleteButton = false;
            ShowTitle = true;
            AddButtonText = "Add New";
            EditButtonImage = null;
        }
    }
}
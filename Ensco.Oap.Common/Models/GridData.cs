using DevExpress.Web;
using DevExpress.Web.Data;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.WebControls;

namespace Ensco.Irma.Oap.Common.Models
{
    using Ensco.Irma.Oap.Common.Extensions;
    using Ensco.Utilities;
    using System.Linq;

    public class GridData
    {
        /// <summary>
        /// Base Constructor
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="toolbarOptions"></param>
        /// <param name="commandOptions"></param>
        /// <param name="historicalRow"></param>
        /// <param name="showPager"></param>
        public GridData(string gridName, GridToolBarOptions toolbarOptions, GridCommandButtonOptions commandOptions, bool historicalRow = true, bool showPager = true, string editController = "Generic")
        {
            ButtonOptions = commandOptions;
            HistoricalRow = historicalRow;
            ShowPager = showPager;
            EditController = editController;
            ToolBarOptions = toolbarOptions;
            Name = gridName;

            AddRemoveCustomAddNew();

            Buttons = new List<GridButton>()
                            {
                                new GridButton(GridButtonTypes.Add),
                                new GridButton(GridButtonTypes.Update),
                                new GridButton(GridButtonTypes.Delete),
                                new GridButton(GridButtonTypes.Cancel),
                                new GridButton(GridButtonTypes.Edit)
                            };

            PageSizes = DevExpressGridExtensions.DefaultPageSizeItems;

            RowInitializeEvent = (s, e) =>
            {
                DefaultNewRowInitializeFields(e);
            };
             
        }

        public GridData(string gridName, bool historicalRow = true, bool showPager = true, string editController = "Generic") :this(gridName, new GridToolBarOptions(false), new GridCommandButtonOptions(false), historicalRow: historicalRow, showPager: showPager, editController:editController)
        {
        }

        public GridData(string gridName, string controller, string action, string title, string addNewButtonName, string addNewButtonText, string searchText, bool initializeCallBack = false, object callBackRoute = null, string key = "Id", string displayColumnName = "Name", IDictionary<GridRouteTypes,string> routes = null, bool historicalRow = true, bool showPager = true, string editController = "Generic") : this(gridName, new GridToolBarOptions(true,addNewToolbarButtonName: addNewButtonName, addNewToolbarButtonText: addNewButtonText, searchText: searchText, addToolBarItemStyle: new GridToolBarItemStyle()), new GridCommandButtonOptions(true, displayAddButtonInGridHeader: false), historicalRow: historicalRow, showPager: showPager, editController : editController)
        {
            Key = key;
            DisplayColumnName = displayColumnName.Translate();
            Name = gridName;
            Action = action;
            Controller = controller;
            Title = title.Translate();

            if (routes != null && routes.Count > 0)
            {
                foreach(var kv in routes)
                {
                    Routes.Add(new GridRoute(kv.Key, new { Controller = Controller, Action = kv.Value }));
                }
            }

            if (initializeCallBack)
            {
                if (callBackRoute == null)
                {
                    CallBackRoute = new { Controller = controller, Action };
                }
                else
                {
                    CallBackRoute = callBackRoute;
                }
            }
        }

        public GridData(string gridName, string controller, string action, object callBackRoute, bool displayEditButton = false, bool displayDeleteButton = false, bool initializeCallBack = false, string key = "Id", string displayColumnName = "Name", IDictionary<GridRouteTypes, string> routes = null, bool historicalRow = false, bool showPager = true, string editController = "Generic") : this(gridName, new GridToolBarOptions(false), new GridCommandButtonOptions(true, displayAddButtonInGridHeader: false, displayEditButton: displayEditButton, displayDeleteButton: displayDeleteButton), historicalRow: historicalRow, showPager: showPager, editController: editController)
        {
            Key = key;
            DisplayColumnName = displayColumnName.Translate();
            Name = gridName; 
            Controller = controller;
            Action = action;

            if (routes != null && routes.Count > 0)
            {
                foreach (var kv in routes)
                {
                    Routes.Add(new GridRoute(kv.Key, new { Controller = Controller, Action = kv.Value }));
                }
            }

            if (initializeCallBack)
            {
                CallBackRoute = callBackRoute;
            }
        }

        public GridData(string gridName, string controller, string action, bool initializeCallBack = false, object callBackRoute = null, string key = "Id", string displayColumnName = "Name", IDictionary<GridRouteTypes, string> routes = null, bool historicalRow = false, GridToolBarOptions toolbarOptions = null, GridCommandButtonOptions commandButtonOptions = null, bool showPager = true, string editController = "Generic") : this(gridName, toolbarOptions ?? new GridToolBarOptions(false), commandButtonOptions ?? new GridCommandButtonOptions(true, displayAddButtonInGridHeader: false), historicalRow: historicalRow, showPager: showPager, editController: editController)
        {
            Key = key;
            DisplayColumnName = displayColumnName.Translate();
            Name = gridName;
            Controller = controller;
            Action = action;

            if (routes != null && routes.Count > 0)
            {
                foreach (var kv in routes)
                {
                    Routes.Add(new GridRoute(kv.Key, new { Controller = Controller, Action = kv.Value }));
                }
            }

            if (initializeCallBack)
            {
                CallBackRoute = callBackRoute;
            }
        }

        public GridData(string gridName, string controller, string action, string title, string addNewButtonName, string addNewButtonText, string searchName, string searchText, GridToolBarItemStyle addToolBarItemStyle, bool initializeCallBack = false, string key = "Id", string displayColumnName = "Name", IDictionary<GridRouteTypes, string> routes = null, bool historicalRow = true, bool showPager = true, string editController = "Generic") : this(gridName, new GridToolBarOptions(true, addNewToolbarButtonName: addNewButtonName, addNewToolbarButtonText: addNewButtonText, searchName: searchName, searchText: searchText, addToolBarItemStyle: addToolBarItemStyle), new GridCommandButtonOptions(true, displayAddButtonInGridHeader: false), historicalRow: historicalRow, showPager: showPager, editController: editController)
        {
            Key = key;
            DisplayColumnName = displayColumnName.Translate();
            Name = gridName;
            Action = action;
            Controller = controller;
            Title = title;

            if (routes != null && routes.Count > 0)
            {
                foreach (var kv in routes)
                {
                    Routes.Add(new GridRoute(kv.Key, new { Controller = Controller, Action = kv.Value }));
                }
            }

            if (initializeCallBack)
            {
                CallBackRoute = new { Controller = controller, Action };
            }
        }

        public GridData(string gridName, string controller, string action, string title, string searchName, string searchText, bool initializeCallBack = false,bool displayDeleteButton = false, string key = "Id", string displayColumnName = "Name", IDictionary<GridRouteTypes, string> routes = null, bool historicalRow = true, bool showPager = true, string editController = "Generic") : this(gridName, new GridToolBarOptions(true, displayCustomAddNew: false, searchName: searchName, searchText: searchText), new GridCommandButtonOptions(true, displayDeleteButton: displayDeleteButton, displayAddButtonInGridHeader: false), historicalRow: historicalRow, showPager: showPager, editController: editController)
        {
            Key = key;
            DisplayColumnName = displayColumnName.Translate();
            Name = gridName;
            Action = action;
            Controller = controller;
            Title = title.Translate();

            if (routes != null && routes.Count > 0)
            {
                foreach (var kv in routes)
                {
                    Routes.Add(new GridRoute(kv.Key, new { Controller = Controller, Action = kv.Value }));
                }
            }

            if (initializeCallBack)
            {
                CallBackRoute = new { Controller = controller, Action };
            }
        }
         

        public void AddRemoveCustomAddNew()
        {
            if (ToolBarOptions.DisplayCustomAddNew)
            {
                var addItem = AddToolbarItem(ToolBarOptions.AddNewToolbarButtonName, ToolBarOptions.AddNewToolbarButtonText, ToolBarOptions.Width, ToolBarOptions.Height);
                if (ToolBarOptions.AddToolBarItemStyle != null)
                {
                    addItem.ItemStyle.BackColor = ToolBarOptions.AddToolBarItemStyle.BackColor;
                    addItem.ItemStyle.ForeColor = ToolBarOptions.AddToolBarItemStyle.ForeColor;
                    addItem.ItemStyle.HoverStyle.ForeColor = ToolBarOptions.AddToolBarItemStyle.ItemStyleForeColor;
                    addItem.ItemStyle.Border.BorderStyle = ToolBarOptions.AddToolBarItemStyle.BorderStyle;
                }

                var spaceItem = AddToolbarItem("Space1", string.Empty, ToolBarOptions.Width, ToolBarOptions.Height, isClientEnabled: false);
                spaceItem.SetTemplateContent("&nbsp;&nbsp;");
            }
            else
            {
                if (ToolbarItems.Any())
                {
                    var item = ToolbarItems.SingleOrDefault(i => i.Name.Equals(ToolBarOptions.AddNewToolbarButtonName, StringComparison.InvariantCultureIgnoreCase));
                    if (item != null)
                    {
                        ToolbarItems.Remove(item);
                    }
                }
            }
        }

        public void SetGridReadOnly()
        {
            ToolBarOptions.DisplayCustomAddNew = false;
            ButtonOptions.DisplayEditButton = false;
            ButtonOptions.DisplayDeleteButton = false;
            AddRemoveCustomAddNew();
        }
         
        public MVCxGridViewToolbarItem AddToolbarItem(string name, string displayText, int width , int height,string imageUri = "",bool isClientEnabled = true)
        {
            var item = new MVCxGridViewToolbarItem
            {
                Name = name,
                Text = displayText
            };

            item.SetProperties(width, height, GridViewToolbarCommand.Custom, imageUri, isClientEnabled);
            
            ToolbarItems.Add(item);

            return item;
        }

        public string EditController { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public string DisplayColumnName { get; set; }

        public string Action { get; set; }     
        
        public string Controller { get; set; }

        public object CallBackRoute { get; set; }

        public IList<GridDisplayColumn> DisplayColumns { get; set; }

        public IList<GridButton> Buttons { get; set; }

        public IList<GridRoute> Routes { get; set; } = new List<GridRoute>();

        public IList<GridEditLayoutColumn> LayoutColumns { get; set; }

        public GridEditFormLayout FormLayout { get; set; }

        public GridCommandButtonOptions ButtonOptions { get; set; }
        public bool HistoricalRow { get; }
        public bool ShowPager { get; }
        public string[] PageSizes { get; set; }

        public ASPxDataInitNewRowEventHandler  RowInitializeEvent { get; set; }

        public void DefaultNewRowInitializeFields(ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["CreatedBy"] = UtilitySystem.CurrentUserName;
            e.NewValues["UpdatedBy"] = UtilitySystem.CurrentUserName;
            e.NewValues["CreateDateTime"] = DateTime.UtcNow;
            e.NewValues["UpdatedDateTime"] = DateTime.UtcNow;
            if (HistoricalRow)
            {
                e.NewValues["StartDateTime"] = DateTime.UtcNow;
                e.NewValues["EndDateTime"] = DateTime.MaxValue;
            }
        }

        public void DefaultUpdateRowInitializeFields(ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["UpdatedBy"] = UtilitySystem.CurrentUserName;
            e.NewValues["UpdatedDateTime"] = DateTime.UtcNow;          
        }

        public GridToolBarOptions ToolBarOptions { get; }

        public ASPxGridViewEditorEventHandler RowEditEvent { get; set; }

        public Color AlternateRowColor { get; set; } = ColorTranslator.FromHtml("#f4f4f4");  //Color.LightGray;

        public IList<MVCxGridViewToolbarItem> ToolbarItems { get; private set; } = new List<MVCxGridViewToolbarItem>();

        public IDictionary<string, GridLookup> LookupList { get; set; } = new Dictionary<string, GridLookup>();

        public GridGroupBehaviorSetting GroupSetting { get; set; }

        public GridSettings Settings { get; set; }
    }
}
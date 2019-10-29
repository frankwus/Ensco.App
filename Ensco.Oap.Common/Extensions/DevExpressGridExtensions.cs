using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.Web.Mvc.UI;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Linq;
using Ensco.Irma.Oap.Common.Models;
using Ensco.Utilities;
using System;
using Westwind.Globalization;

namespace Ensco.Irma.Oap.Common.Extensions
{
    public static class DevExpressGridExtensions
    {
        public static string[] DefaultPageSizeItems { get; } = new string[] { "10", "20", "50", "100" };

        public const string HideCellCSSName = "hideCell";

        public static string DefaultDateMask { get; set; } = "d";

        public static string DefaultDateTimeMask { get; set; } = "G";

        public static void Hide(this MVCxGridViewColumn gcol, bool hide)
        {
            if (hide)
            {
                gcol.CellStyle.CssClass = HideCellCSSName;
                gcol.HeaderStyle.CssClass = HideCellCSSName;
                gcol.FooterCellStyle.CssClass = HideCellCSSName;
                gcol.GroupFooterCellStyle.CssClass = HideCellCSSName;
                gcol.FilterCellStyle.CssClass = HideCellCSSName;
                gcol.Settings.ShowEditorInBatchEditMode = false;
                gcol.BatchEditModifiedCellStyle.CssClass = HideCellCSSName;
                gcol.EditFormSettings.Visible = DefaultBoolean.False;
            }
            else
            {
                gcol.CellStyle.CssClass = string.Empty;
                gcol.HeaderStyle.CssClass = string.Empty;
                gcol.FooterCellStyle.CssClass = string.Empty;
                gcol.GroupFooterCellStyle.CssClass = string.Empty;
                gcol.FilterCellStyle.CssClass = string.Empty; 
                gcol.BatchEditModifiedCellStyle.CssClass = string.Empty;
                gcol.EditFormSettings.Visible = DefaultBoolean.True;
            }

        }

        private static void SetProperties(this MVCxGridViewColumn gcol, GridDisplayColumn columnInfo)
        {
            gcol.Settings.AllowSort = columnInfo.AllowSort;
            gcol.Settings.FilterMode = columnInfo.FilterMode;
            gcol.Settings.AllowHeaderFilter = columnInfo.AllowHeaderFilter;
            gcol.Settings.AutoFilterCondition = columnInfo.AutoFilterCondition;
            gcol.ReadOnly = columnInfo.IsReadOnly;
            gcol.EditFormSettings.Visible = columnInfo.AllowEditLayout;
            gcol.Hide(!columnInfo.IsVisible);
            gcol.Settings.ShowEditorInBatchEditMode = !columnInfo.IsReadOnly;

            if (columnInfo.IsReadOnly)
            { 
               gcol.EditFormSettings.Visible = DefaultBoolean.False;
            }
            gcol.ShowInCustomizationForm = columnInfo.Customizable;

            if(columnInfo.GroupIndex.HasValue)
            {
                gcol.GroupIndex = columnInfo.GroupIndex.Value;
            }
        }

        public static void Set(this MVCxGridViewColumn gcol, GridDisplayColumn columnInfo)
        {
            gcol.SetProperties(columnInfo);
             

            //Default Value will be used as EmptyLayoutItem
            switch (columnInfo.ColumnType)
            {
                case MVCxGridViewColumnType.TimeEdit:
                    if (columnInfo.Width.HasValue)
                    {
                        gcol.Width = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.Width.Value : Unit.Percentage(columnInfo.Width.Value);
                    }

                    gcol.EditorProperties().TimeEdit(s =>
                    { 
                        s.EditFormat = EditFormat.Custom;
                        if (!string.IsNullOrEmpty(columnInfo.DisplayFormat))
                        {
                            s.EditFormatString = columnInfo.DisplayFormat;
                            s.DisplayFormatString = columnInfo.DisplayFormat;
                        }
                        s.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                        s.ValidationSettings.Display = Display.Dynamic;
                        s.ValidationSettings.SetFocusOnError = true;
                        s.NullDisplayText = "";
                        //s.Height = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.EditLayoutHeight : Unit.Percentage(columnInfo.EditLayoutHeight);
                        if (columnInfo.EditLayoutWidth.HasValue)
                        {
                            s.Width = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.EditLayoutWidth.Value : Unit.Percentage(columnInfo.EditLayoutWidth.Value);
                        }
                    });
                    break;
                case MVCxGridViewColumnType.DateEdit:
                    if (columnInfo.Width.HasValue)
                    {
                        gcol.Width = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.Width.Value : Unit.Percentage(columnInfo.Width.Value);
                    }

                    gcol.EditorProperties().DateEdit(s =>
                    {
                        s.ValidationSettings.ErrorDisplayMode = columnInfo.ErrorDisplayMode;
                        if (string.IsNullOrEmpty(columnInfo.DisplayFormat))
                        {
                            columnInfo.DisplayFormat = DefaultDateMask;
                        }

                        if (!string.IsNullOrEmpty(columnInfo.DisplayFormat))
                        {
                            var editFormatString = UtilitySystem.Settings.ConfigSettings["DateFormat"];
                            var displayFormatString = UtilitySystem.Settings.ConfigSettings["DateFormat"];

                            

                            if (columnInfo.DisplayFormat.ToLower().Contains("hh") || columnInfo.DisplayFormat.ToLower().Equals("f")
                                || columnInfo.DisplayFormat.ToLower().Equals("r") || columnInfo.DisplayFormat.ToLower().Equals("s") 
                                || columnInfo.DisplayFormat.ToLower().Equals("u"))
                            {

                                editFormatString = UtilitySystem.Settings.ConfigSettings["DateTimeFormat"];
                                displayFormatString = UtilitySystem.Settings.ConfigSettings["DateTimeFormat"]; 
                            } 

                            s.EditFormatString = editFormatString;
                            s.DisplayFormatString = displayFormatString;
                            s.EditFormat = EditFormat.Custom;
                            s.TimeSectionProperties.ShowSecondHand = false;
                            s.TimeSectionProperties.Visible = true;

                        }

                        s.ValidationSettings.Display = Display.Dynamic;
                        s.ValidationSettings.SetFocusOnError = true;
                        s.NullDisplayText = "";
                        //s.Height = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.EditLayoutHeight : Unit.Percentage(columnInfo.EditLayoutHeight);
                        if (columnInfo.EditLayoutWidth.HasValue)
                        {
                            s.Width = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.EditLayoutWidth.Value : Unit.Percentage(columnInfo.EditLayoutWidth.Value);
                        }
                    });
                    break;
                case MVCxGridViewColumnType.SpinEdit:
                    if (columnInfo.Width.HasValue)
                    {
                        gcol.Width = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.Width.Value : Unit.Percentage(columnInfo.Width.Value);
                    }

                    
                    gcol.EditorProperties().SpinEdit(s =>
                    {
                        if (columnInfo.EditorProperties != null)
                        {
                            try
                            {
                                s.NumberType =  columnInfo.EditorProperties.NumberType;
                            }
                            catch (System.Exception)
                            {
                                ;
                            }
                        }
                        s.MinValue = 0;
                        s.MaxValue = Int32.MaxValue;
                        s.ValidationSettings.ErrorDisplayMode = columnInfo.ErrorDisplayMode;
                        s.ValidationSettings.Display = Display.Dynamic;
                        s.ValidationSettings.SetFocusOnError = true;
                        //s.Height = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.EditLayoutHeight : Unit.Percentage(columnInfo.EditLayoutHeight);
                        if (columnInfo.EditLayoutWidth.HasValue)
                        {
                            s.Width = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.EditLayoutWidth.Value : Unit.Percentage(columnInfo.EditLayoutWidth.Value);
                        }
                    });
                    break;
                case MVCxGridViewColumnType.CheckBox:
                    if (columnInfo.Width.HasValue)
                    {
                        gcol.Width = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.Width.Value : Unit.Percentage(columnInfo.Width.Value);
                    }
                    gcol.EditorProperties().CheckBox(s =>
                    { 
                        s.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                        s.ValidationSettings.Display = Display.Dynamic;
                        s.ValidationSettings.SetFocusOnError = true;
                        s.ValidationSettings.RequiredField.IsRequired = false;  
                    });
                    break; 
                case MVCxGridViewColumnType.BinaryImage:
                    if (columnInfo.Width.HasValue)
                    {
                        gcol.Width = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.Width.Value : Unit.Percentage(columnInfo.Width.Value);
                    }

                    gcol.EditorProperties().BinaryImage(p =>
                    {
                        if (columnInfo.EditLayoutHeight.HasValue)
                        {
                            p.ImageHeight = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.EditLayoutHeight.Value : Unit.Percentage(columnInfo.EditLayoutHeight.Value);
                        }

                        if (columnInfo.EditLayoutWidth.HasValue)
                        {
                            p.ImageWidth = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.EditLayoutWidth.Value : Unit.Percentage(columnInfo.EditLayoutWidth.Value);
                        }
                        p.EnableServerResize = true;
                        p.ImageSizeMode = ImageSizeMode.FitProportional;
                        p.CallbackRouteValues = columnInfo.CallBackRoute;
                        p.EditingSettings.Enabled = true;
                        p.EditingSettings.UploadSettings.UploadValidationSettings.MaxFileSize = 4194304;
                    });
                    break;
                case MVCxGridViewColumnType.ComboBox:
                    if (columnInfo.Lookup != null && !columnInfo.IsGridLookup)
                    {
                        if (columnInfo.Width.HasValue)
                        {
                            gcol.Width = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.Width.Value : Unit.Percentage(columnInfo.Width.Value);
                        }

                        gcol.EditorProperties().ComboBox(cb =>
                        {
                            cb.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                            cb.ValidationSettings.Display = Display.Dynamic;
                            cb.ValidationSettings.SetFocusOnError = true;
                            cb.Width = Unit.Percentage(100);
                            cb.NullText = columnInfo.Lookup.NullCaption;
                            cb.TextField = columnInfo.Lookup.DisplayColumnName;
                            cb.ValueField = columnInfo.Lookup.KeyColumnName;
                            cb.DropDownStyle = columnInfo.Lookup.Style;
                            cb.DataSource = columnInfo.Lookup.DataSource;
                            if (columnInfo.Lookup.CallbackRouteValues != null)
                            {
                                cb.CallbackRouteValues = columnInfo.Lookup.CallbackRouteValues;
                            }
                            if (!string.IsNullOrEmpty(columnInfo.Lookup.BeginCallBackEvent))
                            {
                                cb.ClientSideEvents.BeginCallback = columnInfo.Lookup.BeginCallBackEvent;
                            }
                            if (!string.IsNullOrEmpty(columnInfo.Lookup.SelectedIndexChangedEvent))
                            {
                                cb.ClientSideEvents.SelectedIndexChanged = columnInfo.Lookup.SelectedIndexChangedEvent;
                            }

                            //cb.Height = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.EditLayoutHeight : Unit.Percentage(columnInfo.EditLayoutHeight);
                            if (columnInfo.EditLayoutWidth.HasValue)
                            {
                                cb.Width = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.EditLayoutWidth.Value : Unit.Percentage(columnInfo.EditLayoutWidth.Value);
                            }

                        });
                    }
                    else //gridView Lookup
                    {

                    }
                    break;
                case MVCxGridViewColumnType.TextBox:
                    if (columnInfo.Width.HasValue)
                    {
                        gcol.Width = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.Width.Value : Unit.Percentage(columnInfo.Width.Value);
                    }

                    gcol.EditorProperties().TextBox(s =>
                    {
                        s.EncodeHtml = columnInfo.EncodeHtml;
                        s.ValidationSettings.ErrorDisplayMode = columnInfo.ErrorDisplayMode;
                        s.ValidationSettings.Display = Display.Dynamic;
                        s.ValidationSettings.SetFocusOnError = true;
                        //s.Height = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.EditLayoutHeight : Unit.Percentage(columnInfo.EditLayoutHeight);
                        if (columnInfo.EditLayoutWidth.HasValue)
                        {
                            s.Width = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.EditLayoutWidth.Value : Unit.Percentage(columnInfo.EditLayoutWidth.Value);
                        }

                        if (columnInfo.DisplayFormat != null && columnInfo.DisplayFormat.Length > 0)
                        {
                            s.MaskSettings.Mask = columnInfo.DisplayFormat;
                            s.MaskSettings.IncludeLiterals = MaskIncludeLiteralsMode.None;
                            s.DisplayFormatString = columnInfo.DisplayFormat;
                        }
                    });
                    break;
               case MVCxGridViewColumnType.Memo:
                    if (columnInfo.Width.HasValue)
                    {
                        gcol.Width = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.Width.Value : Unit.Percentage(columnInfo.Width.Value);
                    }
                    gcol.EditorProperties().Memo(s =>
                    {
                        s.EncodeHtml = columnInfo.EncodeHtml;
                        s.ValidationSettings.ErrorDisplayMode = columnInfo.ErrorDisplayMode;
                        s.ValidationSettings.Display = Display.Dynamic;
                        s.ValidationSettings.SetFocusOnError = true;
                        if (columnInfo.EditLayoutWidth.HasValue)
                        {
                            s.Width = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.EditLayoutWidth.Value : Unit.Percentage(columnInfo.EditLayoutWidth.Value);
                        }
                        if (columnInfo.EditLayoutHeight.HasValue)
                        {
                            s.Height = (!columnInfo.IsWidthAndHeightInPercentage) ? columnInfo.EditLayoutHeight.Value : Unit.Percentage(columnInfo.EditLayoutHeight.Value);
                        }
                    });
                    break;
            }

            if(!columnInfo.IsVisible)
            {
                gcol.Width = 0;
            }
        }

        public static void AddRoutes(this GridViewSettings gridView, GridRouteTypes buttonType, object routeValues)
        {
            switch (buttonType)
            {
                case GridRouteTypes.Add:
                    gridView.SettingsEditing.AddNewRowRouteValues = routeValues;
                    break;
                case GridRouteTypes.Update:
                    gridView.SettingsEditing.UpdateRowRouteValues = routeValues;
                    break;
                case GridRouteTypes.Delete:
                    gridView.SettingsEditing.DeleteRowRouteValues = routeValues;
                    break;
                case GridRouteTypes.Batch:
                    gridView.SettingsEditing.BatchUpdateRouteValues = routeValues;
                    break;
            }
        }

        public static void AddButton(this GridViewSettings gridView, GridButtonTypes buttonType, bool displayButton = true, string imageBaseUrl = "~/Images", int width = 16, int height = 16)
        {
            gridView.SettingsCommandButton.RenderMode = GridCommandButtonRenderMode.Image;

            switch (buttonType)
            {
                case GridButtonTypes.Add:
                    gridView.SettingsCommandButton.NewButton.Image.Url = $"{imageBaseUrl}/Create.png";
                    gridView.SettingsCommandButton.NewButton.Image.Width = width;
                    gridView.SettingsCommandButton.NewButton.Image.Height = height;
                    break;
                case GridButtonTypes.Edit:
                    gridView.SettingsCommandButton.EditButton.Image.Url = $"{imageBaseUrl}/Edit.png";
                    gridView.SettingsCommandButton.EditButton.Image.Width = width;
                    gridView.SettingsCommandButton.EditButton.Image.Height = height;
                    break;
                case GridButtonTypes.Update:
                    gridView.SettingsCommandButton.UpdateButton.Image.Url = $"{imageBaseUrl}/Save.png";
                    gridView.SettingsCommandButton.UpdateButton.Image.Width = width;
                    gridView.SettingsCommandButton.UpdateButton.Image.Height = height;
                    gridView.SettingsCommandButton.UpdateButton.Image.ToolTip = "Save";
                    break;
                case GridButtonTypes.Delete:
                    gridView.SettingsCommandButton.DeleteButton.Image.Url = $"{imageBaseUrl}/Delete.png";
                    gridView.SettingsCommandButton.DeleteButton.Image.Width = width;
                    gridView.SettingsCommandButton.DeleteButton.Image.Height = height;
                    break;
                case GridButtonTypes.Cancel:
                    gridView.SettingsCommandButton.CancelButton.Image.Url = $"{imageBaseUrl}/Cancel.png";
                    gridView.SettingsCommandButton.CancelButton.Image.Width = width;
                    gridView.SettingsCommandButton.CancelButton.Image.Height = height;
                    break;
            }
        }

        public static void SetPropeties(this MVCxGridViewPopupControlSettings popup)
        {
            popup.HeaderFilter.Height = Unit.Pixel(300);
            popup.HeaderFilter.SettingsAdaptivity.Mode = PopupControlAdaptivityMode.OnWindowInnerWidth;
            popup.HeaderFilter.SettingsAdaptivity.SwitchAtWindowInnerWidth = 700;
        }

        public static void BuildEditLayout(this GridViewSettings settings, GridEditFormLayout gridLayout)
        {
            if (gridLayout == null || (settings.SettingsEditing.Mode == GridViewEditingMode.Inline))
            {
                settings.SettingsEditing.Mode = GridViewEditingMode.Inline;
                return;
            }

            settings.SettingsEditing.Mode = gridLayout.EditMode;
            if (gridLayout.EditMode == GridViewEditingMode.Batch)
            {
                settings.SettingsEditing.BatchEditSettings.EditMode = gridLayout.BatchEditMode;
                settings.SettingsEditing.BatchEditSettings.StartEditAction = gridLayout.BatchEditAction;
                settings.SettingsEditing.BatchEditSettings.AllowRegularDataItemTemplate = gridLayout.BatchEditAllowRegularDataItemTemplate;

                return;
            }

            settings.EditFormLayoutProperties.ColumnCount = gridLayout.ColumnCount;

            if (gridLayout.DisplayColumns.Any())
            {
                foreach (var k in gridLayout.DisplayColumns)
                {
                    //Setting a column Type as default.  Adds a Empty Layout item
                    if (k.ColumnType == MVCxGridViewColumnType.Default)
                    {
                        settings.EditFormLayoutProperties.Items.AddEmptyItem(new EmptyLayoutItem());
                        continue;
                    }

                    MVCxGridViewColumnLayoutItem col;
                    if (k.LayoutAction != null)
                    {
                        col = settings.EditFormLayoutProperties.Items.Add(k.LayoutAction);
                        col.ColumnName = k.Name;
                    }
                    else
                    {
                        col = settings.EditFormLayoutProperties.Items.Add(k.Name);
                        col.Caption = k.DisplayName;
                    }

                    if (k.Width.HasValue)
                    {
                        col.Width = (!k.IsWidthAndHeightInPercentage) ? k.Width.Value : Unit.Percentage(k.Width.Value);
                    }
                    if (k.Height.HasValue)
                    {
                        col.Height = (!k.IsWidthAndHeightInPercentage) ? k.Height.Value : Unit.Percentage(k.Height.Value);
                    }
                    col.ClientVisible = k.IsVisible;
                }
            }

            for (int i = 0; i < gridLayout.EmptyLayputItemCount; i++)
            {
                settings.EditFormLayoutProperties.Items.AddEmptyItem(new EmptyLayoutItem());
            }

            settings.EditFormLayoutProperties.Items.AddCommandItem(gridLayout.ProcessLayout);
            settings.EditFormLayoutProperties.SettingsAdaptivity.AdaptivityMode = gridLayout.AdaptiveMode;
            settings.EditFormLayoutProperties.SettingsAdaptivity.SwitchToSingleColumnAtWindowInnerWidth = gridLayout.AdaptiveModeSingleColumnWindowInnerWidth;
            
        }

        public static void BuildEditTemplateLayout(this FormLayoutSettings<dynamic> flSettings, GridViewSettings settings, GridEditFormLayout gridLayout)
        {
            if (gridLayout == null)
            {
                settings.SettingsEditing.Mode = GridViewEditingMode.Inline;
                return;
            }
            settings.SettingsEditing.Mode = gridLayout.EditMode;
            settings.EditFormLayoutProperties.ColumnCount = gridLayout.ColumnCount; 
            foreach (var k in gridLayout.DisplayColumns)
            {
                MVCxGridViewColumnLayoutItem col;
                if (k.LayoutAction != null)
                {
                    col = settings.EditFormLayoutProperties.Items.Add(k.LayoutAction);
                    col.ColumnName = k.Name;
                }
                else
                {
                    col = settings.EditFormLayoutProperties.Items.Add(k.Name);
                    col.Caption = k.DisplayName;
                    if (k.Width.HasValue)
                    {
                        col.Width = (!k.IsWidthAndHeightInPercentage) ? k.Width.Value : Unit.Percentage(k.Width.Value);
                    }
                }
                col.ClientVisible = k.IsVisible;
            }
            for (int i = 0; i < gridLayout.EmptyLayputItemCount; i++)
            {
                settings.EditFormLayoutProperties.Items.AddEmptyItem(new EmptyLayoutItem());
            }
            settings.EditFormLayoutProperties.Items.AddCommandItem(gridLayout.ProcessLayout);
            //settings.EditFormLayoutProperties.SettingsAdaptivity.AdaptivityMode = FormLayoutAdaptivityMode.SingleColumnWindowLimit;
            settings.EditFormLayoutProperties.SettingsAdaptivity.SwitchToSingleColumnAtWindowInnerWidth = 700; 
        }


        public static void SettingPaging(this GridViewSettings gridView,GridData gridData, PagerPosition position = PagerPosition.Bottom)
        { 
            gridView.SettingsPager.Visible = gridData.ShowPager;

            if (gridData.ShowPager)
            {
                gridView.SettingsPager.Position = position;
                gridView.SettingsPager.FirstPageButton.Visible = true;
                gridView.SettingsPager.LastPageButton.Visible = true;
                gridView.SettingsPager.PageSizeItemSettings.Visible = true;
                gridView.SettingsPager.PageSizeItemSettings.ShowAllItem = true;
                gridView.SettingsPager.EllipsisMode = PagerEllipsisMode.InsideNumeric;
                gridView.SettingsPager.PageSizeItemSettings.ShowAllItem = true;
                gridView.SettingsPager.PageSizeItemSettings.Items = (gridData.PageSizes != null && gridData.PageSizes.Any()) ? gridData.PageSizes : DefaultPageSizeItems;
                gridView.SettingsPager.AlwaysShowPager = false;
            }
            else
            {
                gridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            }


            gridView.SettingsPager.EnableAdaptivity = true;
        }

        public static void SetProperties(this MVCxGridViewToolbar toolbar, GridToolBarOptions options)
        {
            toolbar.Enabled = true;
            toolbar.EnableAdaptivity = true;
            toolbar.Position = GridToolbarPosition.Top;
            toolbar.ItemAlign = GridToolbarItemAlign.Right; 
        }
        public static void SetProperties(this MVCxGridViewToolbarItem toolbarItem,int width, int height, GridViewToolbarCommand command, string imageUri, bool isClientEnabled = true)
        {
            toolbarItem.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            toolbarItem.Command = command;
            if (!string.IsNullOrEmpty(imageUri))
            {
                toolbarItem.DisplayMode = GridToolbarItemDisplayMode.Image;
                toolbarItem.Image.Url = imageUri;
                toolbarItem.Image.Width = Unit.Pixel(width);
                toolbarItem.Image.Height = Unit.Pixel(height);
            } 
            toolbarItem.ItemStyle.HoverStyle.ForeColor = System.Drawing.Color.Transparent;
            toolbarItem.ItemStyle.Border.BorderStyle = BorderStyle.None;
            toolbarItem.ClientEnabled = isClientEnabled;
        }

        public static void AddToolBar(this GridViewSettings gridView, HtmlHelper html, ViewContext viewContext, GridData gridData)
        {
            gridView.Toolbars.Add(toolbar =>
            {
                toolbar.SetProperties(gridData.ToolBarOptions);

                if(gridData != null && gridData.ToolbarItems  != null && gridData.ToolbarItems.Any())
                {
                    gridData.ToolbarItems.ToList().ForEach(t => toolbar.Items.Add(t));
                    gridView.ClientSideEvents.ToolbarItemClick = "function(s, e){if (typeof onToolbarItemClick === 'function' ) onToolbarItemClick(s, e); else s.AddNewRow() ;}";
                }

                if (gridData.ToolBarOptions.DisplaySearchPanel)
                {
                    gridView.SettingsSearchPanel.CustomEditorName = "search_" + gridData.Name;

                    toolbar.Items.Add(i =>
                    {
                        i.BeginGroup = true;
                        i.SetTemplateContent(c =>
                        {
                            html.DevExpress().ButtonEdit(s =>
                            {
                                s.Name = "search_" + gridData.Name;
                                s.Properties.NullText = $"{gridData.ToolBarOptions.SearchText}...";
                                s.Properties.Buttons
                                    .Add()
                                    .Image.IconID = DevExpress.Web.ASPxThemes.IconID.FindFind16x16gray;
                            }).Render();
                        });

                    });
                }


                if (gridData.ToolBarOptions.DisplayRefresh)
                {
                    toolbar.Items.Add(item =>
                    {
                        item.SetProperties(gridData.ToolBarOptions.Width, gridData.ToolBarOptions.Height, GridViewToolbarCommand.Refresh, "~/Images/Refresh.png");
                    });
                }

                if (gridData.ToolBarOptions.DisplayFilter)
                {
                    toolbar.Items.Add(item =>
                    {
                        item.SetProperties(gridData.ToolBarOptions.Width, gridData.ToolBarOptions.Height, GridViewToolbarCommand.ShowFilterRow, "~/Images/Filter.png");
                    });
                }

                if (gridData.ToolBarOptions.DisplayClearFilter)
                {
                    toolbar.Items.Add(item =>
                    {
                        item.SetProperties(gridData.ToolBarOptions.Width, gridData.ToolBarOptions.Height, GridViewToolbarCommand.ClearFilter, "~/Images/ClearFilter.png");
                    });
                }

                if (gridData.ToolBarOptions.DisplayPdfExport)
                {
                    toolbar.Items.Add(item =>
                    {
                        item.SetProperties(gridData.ToolBarOptions.Width, gridData.ToolBarOptions.Height, GridViewToolbarCommand.ExportToPdf, "~/Images/Pdf.png");
                    });
                }

                if (gridData.ToolBarOptions.DisplayXlsExport)
                {
                    toolbar.Items.Add(item =>
                    {
                        item.SetProperties(gridData.ToolBarOptions.Width, gridData.ToolBarOptions.Height, GridViewToolbarCommand.ExportToXls, "~/Images/Excel.png");
                    });
                }

                toolbar.Items.Add(item =>
                {
                    item.SetProperties(gridData.ToolBarOptions.Width, gridData.ToolBarOptions.Height, GridViewToolbarCommand.Custom, ""); 
                    item.Name = "Space1";
                    item.SetTemplateContent(c =>
                    {
                        viewContext.Writer.Write("&nbsp;&nbsp;");
                    });
                    item.ClientEnabled = false;
                });
            });
        }

        public static void AddToolBar(this GridLookupSettings gridView, HtmlHelper html, ViewContext viewContext, IList<MVCxGridViewToolbarItem> toolbarItems, GridToolBarOptions options)
        {

            gridView.GridViewProperties.Toolbars.Add(toolbar =>
            {
                toolbar.SetProperties(options);

                if (toolbarItems != null && toolbarItems.Any())
                {
                    toolbarItems.ToList().ForEach(t => toolbar.Items.Add(t));
                }

                if (options.DisplaySearchPanel)
                {
                    gridView.GridViewProperties.SettingsSearchPanel.CustomEditorName = "search";

                    toolbar.Items.Add(i =>
                    {
                        i.BeginGroup = true;
                        i.SetTemplateContent(c =>
                        {
                            html.DevExpress().ButtonEdit(s =>
                            {
                                s.Name = options.SearchName;
                                s.Properties.NullText = $"{options.SearchText}...";
                                s.Properties.Buttons
                                    .Add()
                                    .Image.IconID = DevExpress.Web.ASPxThemes.IconID.FindFind16x16gray;
                            }).Render();
                        });

                    });
                }


                if (options.DisplayRefresh)
                {
                    toolbar.Items.Add(item =>
                    {
                        item.SetProperties(options.Width, options.Height, GridViewToolbarCommand.Refresh, "~/Images/Refresh.png");
                    });
                }

                if (options.DisplayFilter)
                {
                    toolbar.Items.Add(item =>
                    {
                        item.SetProperties(options.Width, options.Height, GridViewToolbarCommand.ShowFilterRow, "~/Images/Filter.png");
                    });
                }

                if (options.DisplayClearFilter)
                {
                    toolbar.Items.Add(item =>
                    {
                        item.SetProperties(options.Width, options.Height, GridViewToolbarCommand.ClearFilter, "~/Images/ClearFilter.png");
                    });
                }

                if (options.DisplayPdfExport)
                {
                    toolbar.Items.Add(item =>
                    {
                        item.SetProperties(options.Width, options.Height, GridViewToolbarCommand.ExportToPdf, "~/Images/Pdf.png");
                    });
                }

                if (options.DisplayXlsExport)
                {
                    toolbar.Items.Add(item =>
                    {
                        item.SetProperties(options.Width, options.Height, GridViewToolbarCommand.ExportToXls, "~/Images/Excel.png");
                    });
                }

                toolbar.Items.Add(item =>
                {
                    item.SetProperties(options.Width, options.Height, GridViewToolbarCommand.Custom, "");
                    item.Name = "Space1";
                    item.SetTemplateContent(c =>
                    {
                        viewContext.Writer.Write("&nbsp;&nbsp;");
                    });
                    item.ClientEnabled = false;
                });
            });
        }

        public static void Configure(this GridViewSettings gridSettings, GridData gridData, HtmlHelper html, ViewContext viewContext, GridSettingOptions gridSettingOptions = null, params string[] arguments)
        {
            gridSettings.Width = Unit.Percentage(100);
            gridSettings.Height = Unit.Percentage(100);

            if(gridSettingOptions == null)
            {
                gridSettingOptions = new GridSettingOptions();
            }

            gridSettings.Settings.HorizontalScrollBarMode = gridSettingOptions.HorizontalScrollBarMode;
            gridSettings.Settings.VerticalScrollBarMode = gridSettingOptions.VerticalScrollBarMode;

            if (gridSettingOptions.VerticalScrollableHeight.HasValue)
            {
                gridSettings.Settings.VerticalScrollableHeight = gridSettingOptions.VerticalScrollableHeight.Value;
            }

            //Grid Specific configurations
            gridSettings.KeyFieldName = gridData.Key;
            gridSettings.Name = $"{gridData.Name}";


            gridSettings.Styles.Header.Wrap = DefaultBoolean.True;
             
            gridSettings.SettingsExport.EnableClientSideExportAPI = true;
            gridSettings.SettingsExport.ExcelExportMode = DevExpress.Export.ExportType.DataAware;

            if (gridSettingOptions.ShowTitle)
            {
                gridSettings.Settings.ShowTitlePanel = true;
                gridSettings.SettingsText.Title = gridData.Title;
            }
            else
            {
                gridSettings.Caption = gridData.Title;
            }

            if (gridData.GroupSetting != null)
            {
                gridSettings.SettingsBehavior.AutoExpandAllGroups = gridData.GroupSetting.AutoExpandAllGroups;  
                gridSettings.SettingsBehavior.MergeGroupsMode = gridData.GroupSetting.GroupMode;

                if (gridData.GroupSetting.GroupFormatForMergedGroup != null)
                {
                    gridSettings.Settings.GroupFormatForMergedGroup = gridData.GroupSetting.GroupFormatForMergedGroup;
                }

                if (gridData.GroupSetting.MergedGroupSeparator != null)
                {
                    gridSettings.Settings.MergedGroupSeparator = gridData.GroupSetting.MergedGroupSeparator;
                }
            }


            gridSettings.Styles.AlternatingRow.BackColor = gridData.AlternateRowColor;

            #region Configure Toolbars


            if (gridData.ToolBarOptions.Show)
            {
                gridSettings.AddToolBar(html, viewContext, gridData); 
            }
            #endregion

            #region Configure Columns
            foreach (var col in gridData.DisplayColumns.OrderBy(c => c.Order))
            {
                if (col.ColumnAction != null)
                {
                    gridSettings.Columns.Add(col.ColumnAction);
                    var lcol = gridSettings.Columns[col.Name] as MVCxGridViewColumn;
                    lcol.Hide(!col.IsVisible);

                    continue;
                }

                MVCxGridViewColumn gcol = gridSettings.Columns.Add(col.Name, col.DisplayName, col.ColumnType);
                gcol.Set(col);
            }
            #endregion

            #region Configure Callback Route
            if (gridData.CallBackRoute != null)
            {
                gridSettings.CallbackRouteValues = gridData.CallBackRoute;
            }
            #endregion

            #region Configure Buttons
            foreach (var btn in gridData.Buttons)
            {
                gridSettings.AddButton(btn.Type, btn.Display);
            }
            #endregion



            if (gridData.ButtonOptions != null)
            {
                gridSettings.CommandColumn.Visible = gridData.ButtonOptions.Show;
                gridSettings.CommandColumn.ShowNewButtonInHeader = gridData.ButtonOptions.DisplayAddButtonInGridHeader;
                gridSettings.CommandColumn.ShowNewButton = gridData.ButtonOptions.DisplayAddButton;
                gridSettings.CommandColumn.ShowCancelButton = gridData.ButtonOptions.DisplayCancelButton;
                gridSettings.CommandColumn.ShowEditButton = gridData.ButtonOptions.DisplayEditButton;
                gridSettings.CommandColumn.ShowUpdateButton = gridData.ButtonOptions.DisplayUpdateButton;
                gridSettings.CommandColumn.ShowDeleteButton = gridData.ButtonOptions.DisplayDeleteButton;
                gridSettings.CommandColumn.Width = Unit.Percentage(gridData.ButtonOptions.Width);                 
            }

             
            gridSettings.CommandColumn.Width = 70;
            gridSettings.CommandColumn.Caption = " ";

           

            #region Configure Routes
            foreach (var route in gridData.Routes)
            {
                gridSettings.AddRoutes(route.Type, route.Route);
            }
            #endregion

            #region Configure Layout

            gridSettings.BuildEditLayout(gridData.FormLayout);
            #endregion

            gridSettings.SettingsBehavior.ConfirmDelete = true;

            gridSettings.SettingPaging(gridData);            

            if (gridData.RowInitializeEvent != null)
            {
                gridSettings.InitNewRow = gridData.RowInitializeEvent;
            }

            if (gridData.RowEditEvent != null)
            {
                gridSettings.CellEditorInitialize = gridData.RowEditEvent;
            }

            //settings.SetupGlobalGridViewBehavior();
        }

        public static void Configure(this GridLookupSettings settings, GridLookup gridLkp, HtmlHelper html, ViewContext viewContext, params string[] arguments)
        {
            settings.Width = Unit.Percentage(100);
            settings.Height = Unit.Percentage(100);

            settings.GridViewStyles.Header.Wrap = DefaultBoolean.True;

            settings.GridViewProperties.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
            settings.GridViewProperties.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;

            //settings.GridViewProperties.SettingsExport.EnableClientSideExportAPI = true;
            //settings.SettingsExport.ExcelExportMode = DevExpress.Export.ExportType.DataAware;

            settings.GridViewProperties.Settings.ShowTitlePanel = true;
            //settings.GridViewProperties.Ti = gridData.Title;

            settings.GridViewStyles.AlternatingRow.BackColor = gridLkp.Data.AlternateRowColor;


            settings.Name = gridLkp.Name;
            settings.KeyFieldName = gridLkp.KeyColumnName;

            #region Configure Toolbars


            if (gridLkp.Data.ToolBarOptions.Show)
            {
                settings.AddToolBar(html, viewContext, gridLkp.Data.ToolbarItems, gridLkp.Data.ToolBarOptions);
            }
            #endregion

            #region Configure Columns
            foreach (var col in gridLkp.Data.DisplayColumns)
            {
                if (col.ColumnAction != null)
                {
                    settings.Columns.Add(col.ColumnAction);
                    var lcol = settings.Columns[col.Name] as MVCxGridViewColumn; 

                    if (!string.IsNullOrEmpty(col.DisplayName))
                    {
                        lcol.Caption = col.DisplayName;
                    }

                    lcol.SetProperties(col); 

                    continue;
                }

                MVCxGridViewColumn gcol = settings.Columns.Add(col.Name, col.DisplayName, col.ColumnType);
                gcol.Set(col);
            }
            #endregion

            #region Configure Callback Route
            if (gridLkp.Data.CallBackRoute != null)
            {
                settings.GridViewProperties.CallbackRouteValues = gridLkp.Data.CallBackRoute;
            }
            #endregion

            //#region Configure Buttons
            //foreach (var btn in gridData.Buttons)
            //{
            //    settings.AddButton(btn.Type, btn.Display);
            //}
            //#endregion

            if (gridLkp.Data.ButtonOptions != null)
            {
                settings.CommandColumn.Visible = gridLkp.Data.ButtonOptions.Show;
                settings.CommandColumn.ShowNewButtonInHeader = gridLkp.Data.ButtonOptions.DisplayAddButtonInGridHeader;
                settings.CommandColumn.ShowNewButton = gridLkp.Data.ButtonOptions.DisplayAddButton;
                settings.CommandColumn.ShowCancelButton = gridLkp.Data.ButtonOptions.DisplayCancelButton;
                settings.CommandColumn.ShowEditButton = gridLkp.Data.ButtonOptions.DisplayEditButton;
                settings.CommandColumn.ShowUpdateButton = gridLkp.Data.ButtonOptions.DisplayUpdateButton;
                settings.CommandColumn.ShowDeleteButton = gridLkp.Data.ButtonOptions.DisplayDeleteButton;
                settings.CommandColumn.Width = Unit.Percentage(gridLkp.Data.ButtonOptions.Width);
            } 
        }

        public static void SetupGlobalGridViewBehavior(this GridViewSettings settings)
        {
            settings.EnablePagingGestures = AutoBoolean.False; 
            settings.Styles.Header.Wrap = DefaultBoolean.True;
            //settings.Styles.GroupPanel.CssClass = "GridNoWrapGroupPanel";
        }
        public static MvcHtmlString GetHeadPartialResources()
        {
            return new MvcHtmlString(GetGridNoWrapGroupPanelCssStyle());
        }
        public static string GetGridNoWrapGroupPanelCssStyle()
        {
            return "\r\n<style>.GridNoWrapGroupPanel td.dx-wrap { white-space: nowrap !important; }</style>\r\n";
        }

        public static string Translate(this string value, string resourceSet = "OapResources")
        {
            return DbRes.T(value, resourceSet);
        }
    }
}

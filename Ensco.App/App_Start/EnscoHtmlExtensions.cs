using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.Web.Mvc.UI;
using Ensco.App.App_Start;
using Ensco.Irma.Models;
using Ensco.Models;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Westwind.Globalization;

namespace Ensco.App.Helpers {
    public static class EnscoHelper {
        public enum EventHandlerType { CheckedChanged, FocusRowChanged, RowClick, SelectionChanged, ComboBoxSelectionChanged };
        public static object BindDataTable(this HtmlHelper helper, string gridName, string caption, DataTable dataTable, string area=null, string controller=null, string action=null, string parameter=null  ) {
            GridViewOptions options = new GridViewOptions();
            options.ShowAddButton = false;
            options.ShowEditButton = false;
            options.ShowDeleteButton = false;
            helper.DevExpress().GridView(grid => {
                grid.Name = gridName;

                grid.Caption = GetTranslation(caption);
                grid.SettingsPager.PageSize = 10;
                grid.Width = Unit.Percentage(100);
                if (action !=null )
                    grid.CallbackRouteValues = new { Area = area, Controller = controller, Action =action, parameter= parameter };
                grid.ClientSideEvents.BeginCallback = "function(s, e){ if (typeof onBeginCallback === 'function' ) onBeginCallback(s, e ) }";
                grid.ClientSideEvents.EndCallback = "function(s, e){ if (typeof onEndCallback === 'function' ) onEndCallback(s, e ) }";
                //  helper.EnscoStandardGrid(grid, null, options);
                foreach(DataColumn dc in dataTable.Columns) {
                    MVCxGridViewColumn column = grid.Columns.Add(dc.ColumnName);
                    string caption1 = GetTranslation(dc.ColumnName);

                    column.Caption = UtilitySystem.SplitPascalCase(caption1);
                    if(column.Caption == "ID" || dc.DataType.ToString().ToLower().Contains("int")) {
                        column.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }).Bind(dataTable).GetHtml();
            return null;
        }
        public static object BindCapaDataTable(this HtmlHelper helper, string gridName, DataTable dataTable, bool readOnly, string type) {
            string action = "grid";
            if (type == "Extension" || type == "Reassignment") {
                action = type;
            }
            GridViewOptions options = new GridViewOptions();
            options.ShowAddButton = (!readOnly && action == "grid") || type == "Validate";
            options.ShowEditButton = false;
            options.ShowDeleteButton = false;
            helper.DevExpress().GridView(grid => {
                if (gridName != "grid")
                    gridName += "Grid";
                grid.Name = gridName;
                string gridCaption = (type == "Implementation" ? "ProgressJournal" : type);
                if ((type == "Extension" || type == "Reassignment") && dataTable.Rows.Count == 0)
                    grid.ClientVisible = false;
                grid.Caption = GetTranslation(gridCaption);
                grid.SettingsPager.PageSize = 10;
                grid.Width = Unit.Percentage(100);

                grid.CallbackRouteValues = new { Area = "Irma", Controller = "Capa", Action = action + "Partial" };
                grid.ClientSideEvents.BeginCallback = "function(s, e){  e.customArgs['Type'] = '" + type + "'}";
                grid.ClientSideEvents.EndCallback = "function(s, e){if (typeof onEndCallback === 'function' ) onEndCallback(s, e) }";
                helper.EnscoStandardGrid(grid, null, options);
                foreach (DataColumn dc in dataTable.Columns) {
                    string[] arr = new string[] { "CapaId", "Who", "Type" };
                    if (type == "Extension")
                        arr = AddArr(arr, "AssignedTo");
                    else if (type == "Reassignment")
                        arr = AddArr(arr, "Extension");
                    else if (type == "Implementation")
                        arr = new string[] { "CapaId", "Who", "Type", "Signature", "Status", "Extension", "AssignedTo" };
                    else if (type == "Notify")
                        arr = new string[] { "CapaId", "Who", "Type", "Signature", "Status","Comment", "Extension", "AssignedTo" };
                    else
                        arr = AddArr(AddArr(arr, "AssignedTo"), "Extension");
                    if (arr.Contains(dc.ColumnName, StringComparer.OrdinalIgnoreCase))
                        continue;
                    MVCxGridViewColumn column = grid.Columns.Add(dc.ColumnName);
                    string caption = GetTranslation(dc.ColumnName);

                    column.Caption = UtilitySystem.SplitPascalCase(caption);
                    if (column.Caption == "ID") {
                        column.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                        if (type == "Validate") {
                            column.SetDataItemTemplateContent(c => {
                                helper.DevExpress().Label(settings => {
                                    settings.Name = Guid.NewGuid().ToString();
                                    settings.Text = DataBinder.Eval(c.DataItem, "Id").ToString();
                                }).Render();
                                helper.ViewContext.Writer.Write("&nbsp;");
                                if (DataBinder.Eval(c.DataItem, "Extension").ToString() == "" && DataBinder.Eval(c.DataItem, "Signature").ToString() == "")
                                    AddButton(helper, c, "Delete");
                            });
                        }
                    }
                    column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                    column.Settings.AllowHeaderFilter = DefaultBoolean.True;
                    column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                    if (dc.ColumnName == "Comment" && type != "Implementation") {
                        column.SetDataItemTemplateContent(c => {
                            if (Editable(c)) {
                                helper.DevExpress().TextBox(settings => {
                                    settings.Name = "Row" + DataBinder.Eval(c.DataItem, "Id");
                                    settings.Width = Unit.Percentage(100);
                                    settings.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                    settings.Properties.ClientSideEvents.Validation = " function(s,e){ onValidationReject(s, e) }";
                                }).Render();
                            } else
                                helper.DevExpress().Label(settings => {
                                    settings.Text = DataBinder.Eval(c.DataItem, "Comment").ToString();
                                }).Render();
                        });
                    }
                    if (dc.ColumnName == "Signature") {
                        column.SetDataItemTemplateContent(c => {
                            if (Editable(c)) {
                                AddButton(helper, c, "Sign");
                                AddButton(helper, c, "Reject");
                            } else
                                helper.DevExpress().Label(settings => {
                                    settings.Text = DataBinder.Eval(c.DataItem, "Signature").ToString();
                                }).Render();
                        });
                    }
                    if (dc.DataType.FullName.Contains("Date")) {
                        column.EditorProperties().DateEdit(s => {
                            s.DisplayFormatString = (type != "Implementation") ? UtilitySystem.Settings.ConfigSettings["DateTimeFormat"] : UtilitySystem.Settings.ConfigSettings["DateTimeFormat"];
                            s.ValidationSettings.Display = Display.Dynamic;
                            s.NullDisplayText = "";
                            s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                        });
                    }
                }
            }).Bind(dataTable).GetHtml();
            return null;
        }

        public static string Translate(this HtmlHelper helper, string text, string resourceSet = "OapResources")
        {
            return DbRes.T(text, resourceSet);
        }

        static bool Editable(GridViewDataItemTemplateContainer c) {
            return DataBinder.Eval(c.DataItem, "Status").ToString() == "Pending" && UtilitySystem.CurrentUserId == (int)DataBinder.Eval(c.DataItem, "Who");
        }
        static string[] AddArr(string[] arr, string s) {
            return arr.Concat(new string[] { s }).ToArray();
        }
        public static void AddButton(this HtmlHelper helper, GridViewDataItemTemplateContainer c, string action) {
            helper.DevExpress().Button(settings => {
                settings.Name = Guid.NewGuid().ToString();
                settings.Text = action;
                settings.UseSubmitBehavior = false;
                settings.ClientSideEvents.Click = "function(s, e) {on_Action('" + action + "') }";
                settings.RenderMode = ButtonRenderMode.Button;
            }).Render();
            helper.ViewContext.Writer.Write("&nbsp;");
        }
        public static void CreateCapaGrid(this HtmlHelper helper, string name, DataTableModel dt, string[] HideColumns) {
            string html = "<br><span class=\"dxflGroupBoxCaption_EnscoTheme\">" + name + "</span>";
            helper.ViewContext.Writer.Write(html);
            helper.DevExpress().GridView(grid => {
                grid.Name = name;
                grid.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                grid.DataBound = (s, e) => {
                    ASPxGridView g = (ASPxGridView)s;
                    foreach (GridViewColumn c in g.Columns) {
                        if (HideColumns.Contains(c.ToString()))
                            c.Visible = false;
                    }
                };
            }).BindToLINQ(string.Empty, string.Empty, (s, e) => {
                e.KeyExpression = dt.KeyFieldName;
                e.QueryableSource = dt.Dataset;
            }).GetHtml();
        }
        public static string GetTranslation(string name, string type = "Property") {
            string name1 = name;
            UserSession info = UtilitySystem.SessionInfo.GetSessionUser();
            if (info != null && info.Language != "en") {
                name1 = (string)HttpContext.GetGlobalResourceObject("Resources", "Ensco." + type + "." + name);

                if (name1.Contains("not found"))
                    name1 = name;
            }
            return UtilitySystem.SplitPascalCase(name1);
        }
        public static void InitColumn(this HtmlHelper helper, MVCxGridViewColumn column, GridViewSettings grid, DataColumn dc ) {
            
            string caption = GetTranslation(dc.ColumnName);
            column.Caption = UtilitySystem.SplitPascalCase(caption);
            column.Settings.FilterMode = ColumnFilterMode.DisplayText;
            column.Settings.AllowSort = DefaultBoolean.True;
            column.Settings.SortMode = DevExpress.XtraGrid.ColumnSortMode.Default;
            column.Settings.AllowHeaderFilter = DefaultBoolean.True;
            column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            if (dc.DataType.Name.Contains("Date")) {
                column.EditorProperties().DateEdit(s => {
                    s.DisplayFormatString = Ensco.Utilities.UtilitySystem.Settings.ConfigSettings["DateFormat"];
                });
            }
            if (column.Caption == "ID")
                column.CellStyle.HorizontalAlign = HorizontalAlign.Center;
        }
        public static void EnscoStandardGrid(this HtmlHelper helper, GridViewSettings grid, Type type, GridViewOptions options, Dictionary<EventHandlerType, string> eventHandlers = null) {
            if (grid == null || options == null)
                return;

            if (options.ShowTitle && options.Title != null) {
                grid.Settings.ShowTitlePanel = true;
                grid.SettingsText.Title = DbRes.T(options.Title,"Resources");
            }

            grid.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            grid.Height = System.Web.UI.WebControls.Unit.Percentage(100);
            grid.SettingsExport.EnableClientSideExportAPI = true;
            grid.SettingsExport.ExcelExportMode = DevExpress.Export.ExportType.DataAware;
            grid.Styles.Header.Wrap = DevExpress.Utils.DefaultBoolean.True;
            grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Hidden;
            grid.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
            //grid.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
            grid.ClientSideEvents.BeginCallback = "function(s, e){ if (typeof onBeginCallback === 'function' ) onBeginCallback(s, e ) }";

            // Pager
            if (options.ShowPager) {
                grid.SettingsPager.Position = System.Web.UI.WebControls.PagerPosition.Bottom;
                grid.SettingsPager.FirstPageButton.Visible = true;
                grid.SettingsPager.LastPageButton.Visible = true;
                grid.SettingsPager.PageSizeItemSettings.Visible = true;
                grid.SettingsPager.PageSizeItemSettings.Items = new string[] { "10", "20", "50", "100" };
                grid.SettingsPager.PageSizeItemSettings.ShowAllItem = true;
            }

            // Command column images
            grid.SettingsCommandButton.RenderMode = GridCommandButtonRenderMode.Image;
            grid.SettingsCommandButton.NewButton.Image.Url = "~/Images/Create.png";

            grid.SettingsCommandButton.EditButton.Image.Url = (options.EditButtonImage != null) ? options.EditButtonImage : "~/Images/Edit.png";
            grid.SettingsCommandButton.EditButton.Image.Width = 16;
            grid.SettingsCommandButton.EditButton.Image.Height = 16;
            grid.SettingsCommandButton.DeleteButton.Image.Url = "~/Images/Delete.png";
            grid.SettingsCommandButton.DeleteButton.Image.Width = 16;
            grid.SettingsCommandButton.DeleteButton.Image.Height = 16;
            grid.SettingsCommandButton.UpdateButton.Image.Url = "~/Images/Save.png";
            grid.SettingsCommandButton.UpdateButton.Image.Width = 16;
            grid.SettingsCommandButton.UpdateButton.Image.Height = 16;
            grid.SettingsCommandButton.UpdateButton.Image.ToolTip = "Save";
            grid.SettingsCommandButton.CancelButton.Image.Url = "~/Images/Cancel.png";
            grid.SettingsCommandButton.CancelButton.Image.Width = 16;
            grid.SettingsCommandButton.CancelButton.Image.Height = 16;

            // Popup form
            grid.SettingsEditing.Mode = GridViewEditingMode.PopupEditForm;
            grid.SettingsPopup.EditForm.Modal = true;
            grid.SettingsPopup.EditForm.HorizontalAlign = PopupHorizontalAlign.Center;
            grid.SettingsPopup.EditForm.VerticalAlign = PopupVerticalAlign.WindowCenter;
            grid.SettingsPopup.EditForm.CloseOnEscape = AutoBoolean.True;

            grid.SettingsEditing.ShowModelErrorsForEditors = true;


            grid.CommandColumn.Visible = options.ShowCommandColumn;
            grid.Styles.Header.Wrap = DefaultBoolean.True;
            grid.SettingsPager.EnableAdaptivity = true;
            grid.CommandColumn.ShowEditButton = options.ShowEditButton;
            grid.CommandColumn.ShowDeleteButton = options.ShowDeleteButton;
            grid.CommandColumn.ShowUpdateButton = true;
            grid.CommandColumn.ShowCancelButton = true;
            grid.CommandColumn.Width = 100;
            if (options.ShowDeleteButton)
                grid.SettingsBehavior.ConfirmDelete = true;

            // Setup Grid Columns
            if (type != null)
                SetEnscoGridColumns(helper, grid, type, eventHandlers);

            if (options.ShowToolbar) {
                grid.Toolbars.Add(toolbar => {
                    toolbar.Name = "toolbar" + grid.Name;
                    toolbar.Enabled = true;
                    toolbar.EnableAdaptivity = true;
                    toolbar.Position = GridToolbarPosition.Top;
                    toolbar.ItemAlign = GridToolbarItemAlign.Right;

                    if (options.ShowAddButton) {
                        toolbar.Items.Add(item => {
                            item.Name = "dividerAddItem" + grid.Name;
                            item.SetTemplateContent(c => {
                                helper.ViewContext.Writer.Write("&nbsp;&nbsp;");
                            });
                        });
                        toolbar.Items.Add(item => {
                            item.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                            item.Command = GridViewToolbarCommand.Custom;
                            item.Name = "AddItem" + grid.Name;
                            item.Text = DbRes.T(options.AddButtonText,"Resources");
                            item.ItemStyle.BackColor = System.Drawing.Color.RoyalBlue;
                            item.ItemStyle.ForeColor = System.Drawing.Color.White;
                            item.ItemStyle.HoverStyle.ForeColor = System.Drawing.Color.Transparent;
                            item.ItemStyle.Border.BorderStyle = BorderStyle.None;
                            item.ClientEnabled = true;
                        });
                        grid.ClientSideEvents.ToolbarItemClick = "function(s, e){if (typeof onToolbarItemClick === 'function' ) onToolbarItemClick(s, e); else s.AddNewRow() ;}";
                    }

                    if (options.ShowCustomizationWindow) {
                        toolbar.Items.Add(item => {
                            item.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                            item.Command = GridViewToolbarCommand.Custom;
                            item.Name = "btnCustomization" + grid.Name;
                            item.Text = DbRes.T("Show Column Chooser","Resources");
                            item.ItemStyle.BackColor = System.Drawing.Color.RoyalBlue;
                            item.ItemStyle.ForeColor = System.Drawing.Color.White;
                            item.ItemStyle.HoverStyle.ForeColor = System.Drawing.Color.Transparent;
                            item.ItemStyle.Border.BorderStyle = BorderStyle.None;
                            item.ClientEnabled = true;
                        });
                    }

                    toolbar.Items.Add(i => {
                        i.BeginGroup = true;
                        i.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                        i.SetTemplateContent(c => {
                            helper.DevExpress().ButtonEdit(s => {
                                s.Name = "search" + grid.Name;
                                s.Properties.NullText = DbRes.T("Search...","Resources");
                                s.Properties.Buttons
                                    .Add()
                                    .Image.IconID = DevExpress.Web.ASPxThemes.IconID.FindFind16x16gray;
                            }).Render();
                        });
                    });

                    toolbar.Items.Add(item => {
                        item.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                        item.Command = GridViewToolbarCommand.Refresh;
                        item.DisplayMode = GridToolbarItemDisplayMode.Image;
                        item.Image.Url = "~/Images/Refresh.png";
                        item.Image.Width = Unit.Pixel(24);
                        item.Image.Height = Unit.Pixel(24);
                        item.ItemStyle.HoverStyle.ForeColor = System.Drawing.Color.Transparent;
                        item.ItemStyle.Border.BorderStyle = BorderStyle.None;
                        item.ClientEnabled = true;
                    });
                    toolbar.Items.Add(item => {
                        item.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                        item.Command = GridViewToolbarCommand.ShowFilterRow;
                        item.DisplayMode = GridToolbarItemDisplayMode.Image;
                        item.Image.Url = "~/Images/Filter.png";
                        item.Image.Width = Unit.Pixel(24);
                        item.Image.Height = Unit.Pixel(24);
                        item.ItemStyle.HoverStyle.ForeColor = System.Drawing.Color.Transparent;
                        item.ItemStyle.Border.BorderStyle = BorderStyle.None;
                        item.ClientEnabled = true;
                    });
                    toolbar.Items.Add(item => {
                        item.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                        item.Command = GridViewToolbarCommand.ClearFilter;
                        item.DisplayMode = GridToolbarItemDisplayMode.Image;
                        item.Image.Url = "~/Images/ClearFilter.png";
                        item.Image.Width = Unit.Pixel(24);
                        item.Image.Height = Unit.Pixel(24);
                        item.ItemStyle.HoverStyle.ForeColor = System.Drawing.Color.Transparent;
                        item.ItemStyle.Border.BorderStyle = BorderStyle.None;
                        item.ClientEnabled = true;
                    });
                    toolbar.Items.Add(item => {
                        item.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                        item.Command = GridViewToolbarCommand.ExportToCsv;
                        item.DisplayMode = GridToolbarItemDisplayMode.Image;
                        item.Image.Url = "~/Images/Excel.png";
                        item.Image.Width = Unit.Pixel(24);
                        item.Image.Height = Unit.Pixel(24);
                        item.ItemStyle.HoverStyle.BackColor = System.Drawing.Color.Transparent;
                        item.ItemStyle.Border.BorderStyle = BorderStyle.None;
                        item.ClientEnabled = true;
                    });
                });
            }
            grid.SettingsSearchPanel.CustomEditorName = "search" + grid.Name;
        }
        public static void SetEnscoGridColumns(this HtmlHelper helper, GridViewSettings grid, Type type
            , Dictionary<EventHandlerType, string> eventHandlers = null) {
            List<ColumnAttribute> attribs = UtilitySystem.GetCustomAttributes(type);
            if (attribs == null || attribs.Count == 0 || grid == null)
                return;

            UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            grid.Styles.AlternatingRow.BackColor = System.Drawing.Color.LightGray;

            ColumnAttribute keyField = attribs.FirstOrDefault(x => x.Key == true);
            string keyFieldName = (keyField != null) ? keyField.FieldName : "Id";
            foreach (ColumnAttribute attrib in attribs) {
                MVCxGridViewColumn column = new MVCxGridViewColumn();
                column.Name = attrib.FieldName;
                column.FieldName = attrib.FieldName;
                column.Visible = attrib.Visible;
                if (attrib.DisplayFormat != null && attrib.DisplayFormat != "")
                    column.PropertiesEdit.DisplayFormatString = attrib.DisplayFormat;
                column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                column.Settings.AllowHeaderFilter = DefaultBoolean.True;
                column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                //column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                column.ReadOnly = !attrib.Editable;
                column.ShowInCustomizationForm = attrib.Customization;

                Attribute requiredAttrib = System.Reflection.CustomAttributeExtensions.GetCustomAttribute(attrib.PropInfo, typeof(RequiredAttribute));
                Attribute displayAttrib = System.Reflection.CustomAttributeExtensions.GetCustomAttribute(attrib.PropInfo, typeof(DisplayAttribute));
                string caption = "";
                if (displayAttrib != null) {
                    DisplayAttribute dattr = (DisplayAttribute)displayAttrib;
                    caption= DbRes.T(dattr.Name,"Resources");
                } else {
                    caption = (attrib.DisplayName != null && attrib.DisplayName != "") ? attrib.DisplayName : attrib.Name;
                }
                column.Caption = DbRes.T(UtilitySystem.SplitPascalCase(caption), "Resources"); 
                string typeName = attrib.PropInfo.PropertyType.Name;
                if (typeName.Contains("Nullable")) {
                    typeName = (attrib.PropInfo.PropertyType.UnderlyingSystemType.GenericTypeArguments != null) ? attrib.PropInfo.PropertyType.UnderlyingSystemType.GenericTypeArguments[0].Name : "";
                }
                switch (typeName) {
                    case "String":
                        if (attrib.HyperLinkField != null) {
                            column.SetDataItemTemplateContent(c => {
                                SetHyperLink(helper, column.FieldName, DataBinder.Eval(c.DataItem, column.FieldName), DataBinder.Eval(c.DataItem, attrib.HyperLinkField), attrib, c.DataItem);
                            });
                        } else if (attrib.LookupList != null) {
                            LookupListModel<dynamic> lkpList = Ensco.Utilities.UtilitySystem.GetLookupList(attrib.LookupList);
                            if (lkpList.GridLookup) {
                                column.ColumnType = MVCxGridViewColumnType.ComboBox;
                                ComboBoxProperties properties = column.PropertiesEdit as ComboBoxProperties;
                                properties.NullText = "[Select]";
                                properties.TextField = lkpList.DisplayField;
                                properties.ValueField = lkpList.KeyFieldName;
                                properties.ValueType = typeof(int); // Get this from lookup list???
                                column.SetDataItemTemplateContent(c => {
                                    var val = DataBinder.Eval(c.DataItem, column.FieldName);
                                    if (val == null)
                                        val = -1;
                                    if (attrib.LookupList == "Passport") {
                                        helper.ViewContext.Writer.Write((string)lkpList.GetDisplayValue(int.Parse(val.ToString())));
                                    } else
                                        helper.ViewContext.Writer.Write((string)lkpList.GetDisplayValue(val));
                                });

                                column.SetEditItemTemplateContent(c => {
                                    helper.RenderAction("GridLookupPartial", "Common", new { Area = "Common", name = column.FieldName, lookup = attrib.LookupList, multi = lkpList.MultiSelect, selected = 0 /*DataBinder.Eval(c.DataItem, keyFieldName)*/ });
                                });
                            } else {
                                column.EditorProperties().ComboBox(combo => {
                                    combo.ValidationSettings.RequiredField.IsRequired = (requiredAttrib != null) ? true : false;
                                    combo.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                    combo.ValidationSettings.Display = Display.Dynamic;
                                    combo.ValidationSettings.SetFocusOnError = true;
                                    combo.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                    combo.NullText = "[Select]";
                                    combo.TextField = lkpList.DisplayField;
                                    combo.ValueField = lkpList.KeyFieldName;
                                    combo.DropDownStyle = DropDownStyle.DropDownList;
                                    combo.DataSource = lkpList.Items;
                                });
                            }
                        } else {
                            column.EditorProperties().TextBox(s => {
                                s.ValidationSettings.RequiredField.IsRequired = (requiredAttrib != null) ? true : false;
                                s.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                s.ValidationSettings.Display = Display.Dynamic;
                                s.ValidationSettings.SetFocusOnError = true;
                                s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                if (attrib.Mask != null && attrib.Mask.Length > 0) {
                                    s.MaskSettings.Mask = attrib.Mask;
                                    s.MaskSettings.IncludeLiterals = MaskIncludeLiteralsMode.None;
                                    s.DisplayFormatString = attrib.Mask;
                                }
                            });
                        }
                        break;
                    case "DateTime":
                    case "DateTimeOffset": {
                            if (attrib.DateFormatType == ColumnAttribute.DateTimeDisplayType.TimeOnly) {
                                column.EditorProperties().TimeEdit(s => {
                                    s.ValidationSettings.RequiredField.IsRequired = (requiredAttrib != null) ? true : false;
                                    s.EditFormat = EditFormat.Custom;
                                    s.EditFormatString = UtilitySystem.Settings.ConfigSettings["TimeFormat"];
                                    s.DisplayFormatString = UtilitySystem.Settings.ConfigSettings["TimeFormat"];
                                    s.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                    s.ValidationSettings.Display = Display.Dynamic;
                                    s.ValidationSettings.SetFocusOnError = true;
                                    s.NullDisplayText = "";
                                    s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                });
                            } else {
                                column.EditorProperties().DateEdit(s => {
                                    s.ValidationSettings.RequiredField.IsRequired = (requiredAttrib != null) ? true : false;
                                    s.EditFormat = EditFormat.Custom;
                                    s.UseMaskBehavior = true;
                                    s.EditFormatString = (attrib.DateFormatType == ColumnAttribute.DateTimeDisplayType.DateOnly) ? UtilitySystem.Settings.ConfigSettings["DateFormat"] : UtilitySystem.Settings.ConfigSettings["DateTimeFormat"];
                                    s.DisplayFormatString = (attrib.DateFormatType == ColumnAttribute.DateTimeDisplayType.DateOnly) ? UtilitySystem.Settings.ConfigSettings["DateFormat"] : UtilitySystem.Settings.ConfigSettings["DateTimeFormat"];
                                    s.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                    s.ValidationSettings.Display = Display.Dynamic;
                                    s.ValidationSettings.SetFocusOnError = true;
                                    s.NullDisplayText = "";
                                    s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                });
                            }
                        }
                        break;
                    case "String[]": {
                            //if (attrib.LookupList != null) {
                            //    LookupListModel<dynamic> lkpList = Ensco.Utilities.UtilitySystem.GetLookupList(attrib.LookupList);
                            //    if (lkpList.GridLookup) {
                            //        item.NestedExtensionType = FormLayoutNestedExtensionItemType.ComboBox;
                            //        item.SetNestedContent(() => {
                            //            helper.RenderAction("GridLookupPartial", "Common", new { Area = "Common", name = item.FieldName, lookup = attrib.LookupList, multi = lkpList.MultiSelect, selected = metadata.Model });
                            //        });
                            //    }
                            //}
                            LookupListModel<dynamic> lkpList = Ensco.Utilities.UtilitySystem.GetLookupList(attrib.LookupList);
                            if (lkpList == null)
                                break;
                            if (lkpList.GridLookup) {
                                column.ColumnType = MVCxGridViewColumnType.ComboBox;
                                ComboBoxProperties properties = column.PropertiesEdit as ComboBoxProperties;
                                properties.NullText = "[Select]";
                                properties.TextField = lkpList.DisplayField;
                                properties.ValueField = lkpList.KeyFieldName;
                                properties.ValueType = typeof(int); // Get this from lookup list???
                                column.SetDataItemTemplateContent(c => {
                                    var val = DataBinder.Eval(c.DataItem, column.FieldName);
                                    helper.ViewContext.Writer.Write((string)lkpList.GetDisplayValue(val));
                                });
                                string selectionChanged = (eventHandlers != null && eventHandlers.ContainsKey(EventHandlerType.SelectionChanged)) ? eventHandlers[EventHandlerType.SelectionChanged] : null;
                                string focusChanged = (eventHandlers != null && eventHandlers.ContainsKey(EventHandlerType.FocusRowChanged)) ? eventHandlers[EventHandlerType.FocusRowChanged] : null;
                                string rowClickEvent = (eventHandlers != null && eventHandlers.ContainsKey(EventHandlerType.RowClick)) ? eventHandlers[EventHandlerType.RowClick] : null;


                                column.SetEditItemTemplateContent(c => {
                                    helper.RenderAction("GridLookupPartial", "Common", new { Area = "Common", name = column.FieldName, lookup = attrib.LookupList, multi = lkpList.MultiSelect, focusRowChangedEvent = focusChanged, selected = 0, rowClick = rowClickEvent /*DataBinder.Eval(c.DataItem, keyFieldName)*/ });
                                });
                            }
                        }
                        break;
                    case "Int64":
                    case "Int32": {
                            if (attrib.LookupList != null) {
                                LookupListModel<dynamic> lkpList = Ensco.Utilities.UtilitySystem.GetLookupList(attrib.LookupList);
                                if (lkpList == null)
                                    break;
                                if (lkpList.GridLookup) {
                                    column.ColumnType = MVCxGridViewColumnType.ComboBox;
                                    ComboBoxProperties properties = column.PropertiesEdit as ComboBoxProperties;
                                    properties.NullText = "[Select]";
                                    properties.TextField = lkpList.DisplayField;
                                    properties.ValueField = lkpList.KeyFieldName;
                                    properties.ValueType = typeof(int); // Get this from lookup list???
                                    column.SetDataItemTemplateContent(c => {
                                        var val = DataBinder.Eval(c.DataItem, column.FieldName);
                                        helper.ViewContext.Writer.Write((string)lkpList.GetDisplayValue(val));
                                    });
                                    string selectionChanged = (eventHandlers != null && eventHandlers.ContainsKey(EventHandlerType.SelectionChanged)) ? eventHandlers[EventHandlerType.SelectionChanged] : null;
                                    string focusChanged = (eventHandlers != null && eventHandlers.ContainsKey(EventHandlerType.FocusRowChanged)) ? eventHandlers[EventHandlerType.FocusRowChanged] : null;
                                    string rowClickEvent = (eventHandlers != null && eventHandlers.ContainsKey(EventHandlerType.RowClick)) ? eventHandlers[EventHandlerType.RowClick] : null;


                                    column.SetEditItemTemplateContent(c => {
                                        helper.RenderAction("GridLookupPartial", "Common", new { Area = "Common", name = column.FieldName, lookup = attrib.LookupList, multi = false, focusRowChangedEvent = focusChanged, selected = 0, rowClick = rowClickEvent /*DataBinder.Eval(c.DataItem, keyFieldName)*/ });
                                    });
                                } else if (lkpList.TreeLookup) {
                                    column.ColumnType = MVCxGridViewColumnType.DropDownEdit;
                                    column.SetDataItemTemplateContent(c => {
                                        var val = DataBinder.Eval(c.DataItem, column.FieldName);
                                        helper.ViewContext.Writer.Write((string)lkpList.GetDisplayValue(val));
                                    });
                                    column.SetEditItemTemplateContent(c => {
                                        helper.ViewContext.Writer.Write(helper.Hidden(column.FieldName).ToHtmlString());

                                        helper.DevExpress().DropDownEdit(ddedit => {
                                            ddedit.Name = column.FieldName;
                                            ddedit.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                            ddedit.PreRender = (s, e) => {
                                                MVCxDropDownEdit dde = (MVCxDropDownEdit)s;
                                                var val = DataBinder.Eval(c.DataItem, column.FieldName);
                                                dde.Text = (string)lkpList.GetDisplayValue(val);
                                            };
                                            ddedit.SetDropDownWindowTemplateContent(cc => {
                                                helper.RenderAction("TreeLookupPartial", "Common", new { Area = "Common", name = column.FieldName, lookup = attrib.LookupList, multi = false, selected = DataBinder.Eval(c.DataItem, column.FieldName) });
                                            });
                                        }).GetHtml();
                                    });
                                } else {
                                    column.EditorProperties().ComboBox(combo => {
                                        combo.ValidationSettings.RequiredField.IsRequired = (requiredAttrib != null) ? true : false;
                                        combo.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                        combo.ValidationSettings.Display = Display.Dynamic;
                                        combo.ValidationSettings.SetFocusOnError = true;
                                        combo.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                        combo.NullText = "[Select]";
                                        combo.TextField = lkpList.DisplayField;
                                        combo.ValueField = lkpList.KeyFieldName;
                                        //combo.DataMember = lkpList.DisplayField;
                                        combo.DropDownStyle = DropDownStyle.DropDownList;
                                        combo.DataSource = lkpList.Items;
                                        string fields = attrib.LookupDisplayFields;

                                        if (lkpList.ModelType == typeof(CrewFlightLookupModel)) {
                                            combo.Columns.Add("CrewChange");
                                            combo.Columns.Add("FlightManifest");
                                            combo.Columns.Add("FlightNumber");
                                        }
                                    });
                                }
                            } else if (attrib.HyperLinkField != null) {
                                column.SetDataItemTemplateContent(c => {
                                    SetHyperLink(helper, column.FieldName, DataBinder.Eval(c.DataItem, column.FieldName), DataBinder.Eval(c.DataItem, attrib.HyperLinkField), attrib, c.DataItem);
                                });
                            } else {
                                column.EditorProperties().SpinEdit(s => {
                                    s.ValidationSettings.RequiredField.IsRequired = (requiredAttrib != null) ? true : false;
                                    s.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                    s.ValidationSettings.Display = Display.Dynamic;
                                    s.ValidationSettings.SetFocusOnError = true;
                                    s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                });
                            }
                        }
                        break;
                    case "Boolean": {
                            grid.HeaderFilterFillItems += (s, e) => {
                                if (e.Column.FieldName == column.FieldName) {
                                    e.Values.Clear();
                                    e.AddShowAll();
                                    e.AddValue("Yes", string.Empty, string.Format("[{0}]", column.FieldName));
                                    e.AddValue("No", string.Empty, string.Format("Not [{0}]", column.FieldName));
                                    e.Values[0].DisplayText = "ShowAll";
                                }
                            };
                            column.EditorProperties().CheckBox(s => {
                                s.ValidationSettings.RequiredField.IsRequired = (requiredAttrib != null) ? true : false;
                                s.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                s.ValidationSettings.Display = Display.Dynamic;
                                s.ValidationSettings.SetFocusOnError = true;
                                s.ValidationSettings.RequiredField.IsRequired = false;
                            });
                        }
                        break;
                    case "Byte[]": {
                            column.EditorProperties().BinaryImage(p => {
                                p.ImageHeight = 100;
                                p.ImageWidth = 100;
                                p.EnableServerResize = true;
                                p.ImageSizeMode = ImageSizeMode.FitProportional;
                                p.CallbackRouteValues = new { Action = "PassportDetailUserImageUpdate", Controller = "Admin" };
                                p.EditingSettings.Enabled = true;
                                p.EditingSettings.UploadSettings.UploadValidationSettings.MaxFileSize = 4194304;
                            });
                        }
                        break;
                    case "SignatureModel": {
                            column.SetDataItemTemplateContent(c => {
                                SignatureModel model = (SignatureModel)DataBinder.Eval(c.DataItem, "Signature");
                                model.Refresh();
                                if (model.Approver == UtilitySystem.CurrentUserId && model.Status == 2) // Approval
                                {
                                    helper.DevExpress().Button(btSettings => {
                                        btSettings.Name = "btnSignatureApprove_" + c.VisibleIndex;
                                        btSettings.Text = "Approve";
                                        btSettings.UseSubmitBehavior = false;
                                        btSettings.ClientSideEvents.Click = string.Format("function(s, e) {{ var comment = Comment_" + c.VisibleIndex + ".GetValue(); ; document.location='{0}&Comment=' + comment; }}", model.ApproveUrl);
                                        btSettings.RenderMode = ButtonRenderMode.Button;
                                    }).Render();
                                    helper.ViewContext.Writer.Write("&nbsp;");
                                    helper.DevExpress().Button(btSettings => {
                                        btSettings.Name = "btnSignatureReject_" + c.VisibleIndex;
                                        btSettings.Text = "Reject";
                                        btSettings.UseSubmitBehavior = false;
                                        btSettings.ClientSideEvents.Click = string.Format("function(s, e) {{ var comment = Comment_" + c.VisibleIndex + ".GetValue(); ; document.location='{0}&Comment=' + comment; }}", model.RejectUrl);
                                        btSettings.RenderMode = ButtonRenderMode.Button;
                                    }).Render();
                                    helper.ViewContext.Writer.Write("&nbsp;");
                                    helper.DevExpress().TextBox(txt => {
                                        txt.Name = "Comment_" + c.VisibleIndex;
                                        txt.Width = Unit.Percentage(100);
                                        txt.Style.Add("margin-top", "10px;");
                                        txt.Properties.NullText = "Approver Comments";
                                    }).Render();
                                } else if (model.Approver == UtilitySystem.CurrentUserId && model.Status == 4) // Review
                                  {
                                    helper.DevExpress().Button(btSettings => {
                                        btSettings.Name = "btnSignatureReview_" + c.VisibleIndex;
                                        btSettings.Text = "Review";
                                        btSettings.UseSubmitBehavior = false;
                                        btSettings.ClientSideEvents.Click = string.Format("function(s, e) {{ var comment = ReviewComment_" + c.VisibleIndex + ".GetValue(); ; document.location='{0}&Comment=' + comment; }}", model.ReviewUrl);
                                        btSettings.RenderMode = ButtonRenderMode.Button;
                                    }).Render();
                                    helper.ViewContext.Writer.Write("&nbsp;");
                                    helper.DevExpress().Button(btSettings => {
                                        btSettings.Name = "btnSignatureReviewReject_" + c.VisibleIndex;
                                        btSettings.Text = "Reject";
                                        btSettings.UseSubmitBehavior = false;
                                        btSettings.ClientSideEvents.Click = string.Format("function(s, e) {{ var comment = ReviewComment_" + c.VisibleIndex + ".GetValue(); ; document.location='{0}&Comment=' + comment; }}", model.RejectUrl);
                                        btSettings.RenderMode = ButtonRenderMode.Button;
                                    }).Render();
                                    helper.ViewContext.Writer.Write("&nbsp;");
                                    helper.DevExpress().TextBox(txt => {
                                        txt.Name = "ReviewComment_" + c.VisibleIndex;
                                        txt.Width = Unit.Percentage(100);
                                        txt.Style.Add("margin-top", "10px;");
                                        txt.Properties.NullText = "Reviewer Comments";
                                    }).Render();
                                } else if (model.Approver == UtilitySystem.CurrentUserId && model.Status == 1 && model.Sequence == 1) {
                                    helper.DevExpress().Button(btSettings => {
                                        btSettings.Name = "btnSignatureSubmit_" + c.VisibleIndex;
                                        btSettings.Text = "Submit";
                                        btSettings.UseSubmitBehavior = false;
                                        btSettings.ClientSideEvents.Click = string.Format("function(s, e) {{ document.location='{0}'; }}", model.SubmitUrl);
                                        btSettings.RenderMode = ButtonRenderMode.Button;
                                    }).Render();
                                } else {
                                    helper.ViewContext.Writer.Write(model.SignatureText);
                                }
                            });
                        }
                        break;
                    default:
                        column.EditorProperties().TextBox(s => {
                            s.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                            s.ValidationSettings.Display = Display.Dynamic;
                            s.ValidationSettings.SetFocusOnError = true;
                            s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                            if (attrib.Mask != null && attrib.Mask.Length > 0) {
                                s.MaskSettings.Mask = attrib.Mask;
                                s.MaskSettings.IncludeLiterals = MaskIncludeLiteralsMode.None;
                                s.DisplayFormatString = attrib.Mask;
                            }
                        });
                        break;
                }

                grid.Columns.Add(column);
            }
        }

        public static void SetHyperLink(this HtmlHelper helper, string fieldName, object value, object hyperlinkValue, ColumnAttribute attrib, object dataItem) {
            UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            if (value == null)
                value = "";
            switch (attrib.HyperLinkType) {
                case ColumnAttribute.HyperLinkFieldType.Passport:
                    helper.DevExpress().HyperLink(s => {
                        s.Properties.Text = (string)value;
                        s.NavigateUrl = urlHelper.Action("MyPassport", "Irma", new { Area = "IRMA", UserId = (int)hyperlinkValue });
                    }).Render();
                    break;
                case ColumnAttribute.HyperLinkFieldType.Capa:
                    helper.DevExpress().HyperLink(s => {
                        s.Properties.Text = value.ToString();
                        string source = DataBinder.Eval(dataItem, "Source").ToString();
                        string sourceId = DataBinder.Eval(dataItem, "SourceId").ToString();
                        if (source.ToLower().Contains("emoc"))
                            s.NavigateUrl = sourceId;
                        else
                            s.NavigateUrl = urlHelper.Action("Index", "Capa", new { Area = "IRMA", Id = (int)hyperlinkValue });
                    }).Render();
                    break;
                case ColumnAttribute.HyperLinkFieldType.Common:
                    helper.DevExpress().HyperLink(s => {
                        s.Properties.Text = value.ToString();
                        s.NavigateUrl = hyperlinkValue == null ? "" : hyperlinkValue.ToString();
                    }).Render();
                    break;
                case ColumnAttribute.HyperLinkFieldType.Job:
                    helper.DevExpress().HyperLink(s => {
                        s.Properties.Text = value.ToString();
                        s.NavigateUrl = urlHelper.Action("ShowJobWindow", "Jobs", new { Area = "IRMA", name = "popupJobsHome", title = "Home", url = "~/JOB/JOB.htm?id=" + hyperlinkValue, });
                    }).Render();
                    break;
                case ColumnAttribute.HyperLinkFieldType.Permit:
                    helper.DevExpress().HyperLink(s => {
                        s.Properties.Text = value.ToString();
                        s.NavigateUrl = urlHelper.Action("ShowJobWindow", "Jobs", new { Area = "IRMA", name = "popupJobsHome", title = "Home", url = "~/JOB/permit.htm?id=" + hyperlinkValue, });
                    }).Render();
                    break;
                case ColumnAttribute.HyperLinkFieldType.Ei:
                    helper.DevExpress().HyperLink(s => {
                        s.Properties.Text = value.ToString();
                        s.NavigateUrl = urlHelper.Action("ShowJobWindow", "Jobs", new { Area = "IRMA", name = "popupJobsHome", title = "Home", url = "~/JOB/ei.htm?id=" + hyperlinkValue, });
                    }).Render();
                    break;
                case ColumnAttribute.HyperLinkFieldType.CapaPlan:
                    helper.DevExpress().HyperLink(s => {
                        s.Properties.Text = (string)value;
                        s.NavigateUrl = urlHelper.Action("CapaPlan", "Job", new { Area = "IRMA", UserId = (int)hyperlinkValue });
                    }).Render();
                    break;
                case ColumnAttribute.HyperLinkFieldType.LessonLearned:
                    helper.DevExpress().HyperLink(s => {
                        s.Properties.Text = value.ToString();
                        s.NavigateUrl = urlHelper.Action("Edit", "LessonsLearned", new { Area = "IRMA", Id = (int)hyperlinkValue });
                    }).Render();
                    break;
            }
        }
        public static void SetEnscoTreeLookupColumns(this HtmlHelper helper, TreeListSettings grid, Type type) {
            List<ColumnAttribute> attribs = UtilitySystem.GetCustomAttributes(type);
            if (attribs == null || attribs.Count == 0 || grid == null)
                return;

            foreach (ColumnAttribute attrib in attribs) {
                MVCxTreeListColumn column = new MVCxTreeListColumn();
                column.Name = (attrib.DisplayName != null && attrib.DisplayName != "") ? attrib.DisplayName : attrib.Name;
                column.FieldName = attrib.FieldName;
                column.Visible = attrib.Visible;
                if (attrib.DisplayFormat != null && attrib.DisplayFormat != "")
                    column.PropertiesEdit.DisplayFormatString = attrib.DisplayFormat;
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AllowHeaderFilter = DefaultBoolean.True;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                //column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                string typeName = attrib.PropInfo.PropertyType.Name;
                if (typeName.Contains("Nullable")) {
                    typeName = (attrib.PropInfo.PropertyType.UnderlyingSystemType.GenericTypeArguments != null) ? attrib.PropInfo.PropertyType.UnderlyingSystemType.GenericTypeArguments[0].Name : "";
                }
                switch (typeName) {
                    case "String":
                        column.EditorProperties().TextBox(s => {
                            s.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                            s.ValidationSettings.Display = Display.Dynamic;
                            s.ValidationSettings.SetFocusOnError = true;
                            s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                        });
                        break;
                    case "DateTime": {
                            string datetimeformat = UtilitySystem.Settings.ConfigSettings["DateTimeFormat"];
                            string timeformat = UtilitySystem.Settings.ConfigSettings["TimeFormat"];
                            string dateformat = UtilitySystem.Settings.ConfigSettings["DateFormat"];
                            switch (attrib.DateFormatType) {
                                case ColumnAttribute.DateTimeDisplayType.TimeOnly:
                                    column.EditorProperties().TimeEdit(s => {
                                        s.EditFormat = EditFormat.Custom;
                                        s.EditFormatString = timeformat;
                                        s.DisplayFormatString = timeformat;
                                        s.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                        s.ValidationSettings.Display = Display.Dynamic;
                                        s.ValidationSettings.SetFocusOnError = true;
                                        s.NullDisplayText = "";
                                        s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                    });
                                    break;
                                case ColumnAttribute.DateTimeDisplayType.DateOnly:
                                    column.EditorProperties().DateEdit(s => {
                                        s.EditFormat = EditFormat.Custom;
                                        s.UseMaskBehavior = true;
                                        s.EditFormatString = dateformat;
                                        s.DisplayFormatString = dateformat;
                                        s.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                        s.ValidationSettings.Display = Display.Dynamic;
                                        s.ValidationSettings.SetFocusOnError = true;
                                        s.NullDisplayText = "";
                                        s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                    });
                                    break;
                                case ColumnAttribute.DateTimeDisplayType.DateTime:
                                    column.EditorProperties().DateEdit(s => {
                                        s.EditFormat = EditFormat.Custom;
                                        s.UseMaskBehavior = true;
                                        s.EditFormatString = datetimeformat;
                                        s.DisplayFormatString = datetimeformat;
                                        s.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                        s.ValidationSettings.Display = Display.Dynamic;
                                        s.ValidationSettings.SetFocusOnError = true;
                                        s.NullDisplayText = "";
                                        s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                    });
                                    break;
                            }
                        }
                        break;
                    case "Int32": {
                            column.EditorProperties().SpinEdit(s => {
                                s.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                s.ValidationSettings.Display = Display.Dynamic;
                                s.ValidationSettings.SetFocusOnError = true;
                                s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                            });
                        }
                        break;
                    case "Boolean": {
                            //grid.HeaderFilterFillItems += (s, e) => {
                            //    if (e.Column.FieldName == column.FieldName)
                            //    {
                            //        e.Values.Clear();
                            //        e.AddShowAll();
                            //        e.AddValue("Yes", String.Empty, "");
                            //        e.AddValue("No", String.Empty, "");
                            //        e.Values[0].DisplayText = "ShowAll";
                            //    }
                            //};
                            column.EditorProperties().CheckBox(s => {
                                s.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                s.ValidationSettings.Display = Display.Dynamic;
                                s.ValidationSettings.SetFocusOnError = true;
                                s.ValidationSettings.RequiredField.IsRequired = false;
                            });
                        }
                        break;
                }

                grid.Columns.Add(column);
            }
        }
        public static void SetEnscoGridLookupColumns(this HtmlHelper helper, GridLookupSettings grid, Type type) {
            List<ColumnAttribute> attribs = UtilitySystem.GetCustomAttributes(type);
            if (attribs == null || attribs.Count == 0 || grid == null)
                return;

            foreach (ColumnAttribute attrib in attribs) {
                MVCxGridViewColumn column = new MVCxGridViewColumn();
                column.Name = (attrib.DisplayName != null && attrib.DisplayName != "") ? attrib.DisplayName : attrib.Name;
                column.FieldName = attrib.FieldName;
                column.Visible = attrib.Visible;
                if (attrib.DisplayFormat != null && attrib.DisplayFormat != "")
                    column.PropertiesEdit.DisplayFormatString = attrib.DisplayFormat;
                column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                column.Settings.AllowHeaderFilter = DefaultBoolean.True;
                column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                string typeName = attrib.PropInfo.PropertyType.Name;

                column.Caption = attrib.DisplayName ?? attrib.FieldName;

                if (typeName.Contains("Nullable")) {
                    typeName = (attrib.PropInfo.PropertyType.UnderlyingSystemType.GenericTypeArguments != null) ? attrib.PropInfo.PropertyType.UnderlyingSystemType.GenericTypeArguments[0].Name : "";
                }
                switch (typeName) {
                    case "String":
                        column.EditorProperties().TextBox(s => {
                            s.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                            s.ValidationSettings.Display = Display.Dynamic;
                            s.ValidationSettings.SetFocusOnError = true;
                            s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                        });
                        break;
                    case "DateTime": {
                            column.EditorProperties().DateEdit(s => {
                                if (attrib.DisplayFormat != null) {
                                    s.EditFormat = EditFormat.Custom;
                                    s.EditFormatString = attrib.DisplayFormat;
                                    if (attrib.DisplayFormat.ToLower().Contains("hh")) {
                                        s.TimeSectionProperties.ShowSecondHand = false;
                                        s.TimeSectionProperties.Visible = true;
                                    }
                                }

                                s.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                s.ValidationSettings.Display = Display.Dynamic;
                                s.ValidationSettings.SetFocusOnError = true;
                                s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                            });
                        }
                        break;
                    case "Int32": {
                            column.EditorProperties().SpinEdit(s => {
                                s.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                s.ValidationSettings.Display = Display.Dynamic;
                                s.ValidationSettings.SetFocusOnError = true;
                                s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                s.ValidationSettings.RequiredField.IsRequired = true;
                            });
                        }
                        break;
                    case "Boolean": {
                            grid.GridViewProperties.HeaderFilterFillItems += (s, e) => {
                                if (e.Column.FieldName == column.FieldName) {
                                    e.Values.Clear();
                                    e.AddShowAll();
                                    e.AddValue("Yes", String.Empty, "");
                                    e.AddValue("No", String.Empty, "");
                                    e.Values[0].DisplayText = "ShowAll";
                                }
                            };
                            column.EditorProperties().CheckBox(s => {
                                s.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                s.ValidationSettings.Display = Display.Dynamic;
                                s.ValidationSettings.SetFocusOnError = true;
                                s.ValidationSettings.RequiredField.IsRequired = false;
                            });
                        }
                        break;
                }

                grid.Columns.Add(column);
            }
        }

        public static MvcHtmlString EnscoEditor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression) {
            MvcHtmlString html = null;

            var name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            List<ColumnAttribute> attribs = UtilitySystem.GetCustomAttributes(typeof(TModel));
            ColumnAttribute attrib = attribs.FirstOrDefault(x => x.FieldName == name);
            if (attrib == null)
                return html;

            string typeName = attrib.PropInfo.PropertyType.Name;
            if (typeName.Contains("Nullable")) {
                typeName = (attrib.PropInfo.PropertyType.UnderlyingSystemType.GenericTypeArguments != null) ? attrib.PropInfo.PropertyType.UnderlyingSystemType.GenericTypeArguments[0].Name : "";
            }

            switch (typeName) {
                case "String":
                    html = helper.DevExpress().TextBoxFor(expression, s => {
                        s.Name = name;
                        s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                        s.Properties.ValidationSettings.Display = Display.Dynamic;
                        s.ShowModelErrors = true;
                        s.EncodeHtml = false;
                        s.ReadOnly = !attrib.Editable;
                        s.Width = Unit.Percentage(100);
                        if (attrib.Mask != null && attrib.Mask.Length > 0) {
                            s.Properties.MaskSettings.Mask = attrib.Mask;
                            s.Properties.MaskSettings.IncludeLiterals = MaskIncludeLiteralsMode.None;
                            s.Properties.DisplayFormatString = attrib.Mask;
                        }
                    }).GetHtml();
                    break;
                case "DateTime":
                    if (attrib.DateFormatType == ColumnAttribute.DateTimeDisplayType.TimeOnly) {
                        html = helper.DevExpress().TimeEditFor(expression, s => {
                            s.Name = name;
                            s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                            s.Properties.ValidationSettings.Display = Display.Dynamic;
                            s.Properties.DisplayFormatString = UtilitySystem.Settings.ConfigSettings["TimeFormat"];
                            s.ShowModelErrors = true;
                            s.EncodeHtml = false;
                            s.ReadOnly = !attrib.Editable;
                            s.Width = Unit.Percentage(100);
                        }).GetHtml();
                    } else {
                        html = helper.DevExpress().DateEditFor(expression, s => {
                            s.Name = name;
                            s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                            s.Properties.ValidationSettings.Display = Display.Dynamic;
                            s.Properties.DisplayFormatString = (attrib.DateFormatType == ColumnAttribute.DateTimeDisplayType.DateOnly) ? UtilitySystem.Settings.ConfigSettings["DateFormat"] : UtilitySystem.Settings.ConfigSettings["DateTimeFormat"];
                            s.ShowModelErrors = true;
                            s.EncodeHtml = false;
                            s.ReadOnly = !attrib.Editable;
                            s.Width = Unit.Percentage(100);
                        }).GetHtml();
                    }
                    break;
                case "Boolean":
                    html = helper.DevExpress().CheckBoxFor(expression, s => {
                        s.Name = name;
                        s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                        s.Properties.ValidationSettings.Display = Display.Dynamic;
                        s.ShowModelErrors = true;
                        s.EncodeHtml = false;
                        s.ReadOnly = !attrib.Editable;
                        s.Width = Unit.Percentage(100);
                    }).GetHtml();
                    break;
                case "Int64":
                case "Int32": {
                        if (attrib.LookupList != null) {
                            LookupListModel<dynamic> lkpList = Ensco.Utilities.UtilitySystem.GetLookupList(attrib.LookupList);
                            if (lkpList.GridLookup) {
                                helper.RenderAction("GridLookupPartial", "Common", new { Area = "Common", name = name, lookup = lkpList.Name, multi = lkpList.MultiSelect, selected = metadata.Model });
                            } else {
                                html = helper.DevExpress().ComboBoxFor(expression, s => {
                                    s.ShowModelErrors = true;
                                    s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                    s.Properties.ValidationSettings.Display = Display.Dynamic;
                                    s.Properties.ValidationSettings.SetFocusOnError = true;
                                    s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                    s.Properties.DataSource = lkpList.Items;
                                    s.Properties.NullText = "[Select]";
                                    s.Properties.TextField = lkpList.DisplayField;
                                    s.Properties.ValueField = lkpList.KeyFieldName;
                                    s.Properties.DataMember = lkpList.DisplayField;
                                    s.Properties.DropDownStyle = DropDownStyle.DropDownList;
                                    s.ReadOnly = !attrib.Editable;
                                }).GetHtml();
                            }
                        } else {
                            html = helper.DevExpress().SpinEditFor(expression, s => {
                                s.Name = name;
                                s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                s.Properties.ValidationSettings.Display = Display.Dynamic;
                                s.ShowModelErrors = true;
                                s.EncodeHtml = false;
                                s.ReadOnly = !attrib.Editable;
                                s.Width = Unit.Percentage(100);
                            }).GetHtml();
                        }
                    }
                    break;
            }

            return html;
        }

        public static MvcHtmlString EnscoNestedLabel<TModel, TProperty>(this HtmlHelper<TModel> helper, MVCxFormLayoutItem item, Expression<Func<TModel, TProperty>> expression) {
            var name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            List<ColumnAttribute> attribs = UtilitySystem.GetCustomAttributes(typeof(TModel));
            ColumnAttribute attrib = attribs.FirstOrDefault(x => x.FieldName == name);
            if (attrib == null)
                return null;

            string typeName = attrib.PropInfo.PropertyType.Name;
            if (typeName.Contains("Nullable")) {
                typeName = (attrib.PropInfo.PropertyType.UnderlyingSystemType.GenericTypeArguments != null) ? attrib.PropInfo.PropertyType.UnderlyingSystemType.GenericTypeArguments[0].Name : "";
            }
            string displayValue = "";
            switch (typeName) {
                case "String":
                    displayValue = metadata.Model.ToString();
                    break;
                case "DateTime": {
                        switch (attrib.DateFormatType) {
                            case ColumnAttribute.DateTimeDisplayType.DateOnly: {
                                    displayValue = ((DateTime)(metadata.Model)).ToString(UtilitySystem.Settings.ConfigSettings["DateFormat"]);
                                }
                                break;
                            case ColumnAttribute.DateTimeDisplayType.TimeOnly: {
                                    displayValue = ((DateTime)(metadata.Model)).ToString(UtilitySystem.Settings.ConfigSettings["TimeFormat"]);
                                }
                                break;
                            case ColumnAttribute.DateTimeDisplayType.DateTime: {
                                    displayValue = ((DateTime)(metadata.Model)).ToString(UtilitySystem.Settings.ConfigSettings["DateTimeFormat"]);
                                }
                                break;
                        }
                    }
                    break;
                case "Boolean":
                    displayValue = ((bool)metadata.Model) ? (string)HttpContext.GetGlobalResourceObject("Resources", "Ensco.Property.Yes") : (string)HttpContext.GetGlobalResourceObject("Resources", "Ensco.Property.No");
                    break;
                case "Int32":
                    displayValue = metadata.Model.ToString();
                    break;
            }

            //item.NestedExtension().TextBox(settings =>
            //{
            //    settings.Name = name;
            //    settings.Width = Unit.Percentage(100);
            //    settings.ReadOnly = true;
            //    settings.Text = displayValue;
            //});

            return new MvcHtmlString(displayValue);
        }
        public static MvcHtmlString EnscoMenoEditor<TModel, TProperty>(this HtmlHelper<TModel> helper, MVCxFormLayoutItem item, Expression<Func<TModel, TProperty>> expression, Nullable<bool> readOnly = null, Dictionary<EventHandlerType, string> eventHandlers = null, Predicate<dynamic> filter = null, bool multi = false) {
            MvcHtmlString html = null;

            var name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));

            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            List<ColumnAttribute> attribs = UtilitySystem.GetCustomAttributes(typeof(TModel));
            ColumnAttribute attrib = attribs.FirstOrDefault(x => x.FieldName == name);
            if (attrib == null)
                return html;

            UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            Attribute requiredAttrib = System.Reflection.CustomAttributeExtensions.GetCustomAttribute(attrib.PropInfo, typeof(RequiredAttribute));

            string typeName = attrib.PropInfo.PropertyType.Name;
            if (typeName.Contains("Nullable")) {
                typeName = (attrib.PropInfo.PropertyType.UnderlyingSystemType.GenericTypeArguments != null) ? attrib.PropInfo.PropertyType.UnderlyingSystemType.GenericTypeArguments[0].Name : "";
            }
            switch (typeName) {
                case "String":
                    if (attrib.LookupList != null) {
                        LookupListModel<dynamic> lkpList = Ensco.Utilities.UtilitySystem.GetLookupList(attrib.LookupList);
                        if (lkpList.GridLookup) {
                            item.NestedExtensionType = FormLayoutNestedExtensionItemType.ComboBox;
                            item.SetNestedContent(() => {
                                helper.RenderAction("GridLookupPartial", "Common", new { Area = "Common", name = item.FieldName, lookup = attrib.LookupList, multi = lkpList.MultiSelect, selected = metadata.Model });
                            });
                        } else {
                            string comboSelectionChanged = (eventHandlers != null && eventHandlers.ContainsKey(EventHandlerType.ComboBoxSelectionChanged)) ? eventHandlers[EventHandlerType.ComboBoxSelectionChanged] : null;
                            item.NestedExtension().ComboBox(s => {
                                s.Name = name;
                                s.ShowModelErrors = true;
                                s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                s.Properties.ValidationSettings.Display = Display.Dynamic;
                                s.Properties.ValidationSettings.SetFocusOnError = true;
                                s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                s.Properties.DataSource = (filter != null) ? lkpList.Items.FindAll(filter) : lkpList.Items;
                                s.Properties.NullText = "[Select]";
                                s.Properties.TextField = lkpList.DisplayField;
                                s.Properties.ValueField = lkpList.KeyFieldName;
                                s.Properties.DataMember = lkpList.DisplayField;
                                s.Properties.DropDownStyle = DropDownStyle.DropDownList;
                                s.Properties.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                                s.Properties.AllowNull = false;
                                s.Properties.ClientSideEvents.SelectedIndexChanged = comboSelectionChanged;
                                s.Properties.ClientSideEvents.Validation = (requiredAttrib != null) ? "function(s,e){ if (s.GetText() == \"\") e.isValid = false; }" : "function(s, e){}";
                                s.ReadOnly = (readOnly == null) ? !attrib.Editable : (bool)readOnly;
                            });
                        }
                    } else if (attrib.Memo) {
                        item.NestedExtension().Memo(s => {
                            InitMemo(s, name, readOnly, attrib, requiredAttrib);
                        });
                    } else {
                        item.NestedExtension().TextBox(s => {
                            s.Name = name;
                            s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                            s.Properties.ValidationSettings.Display = Display.Dynamic;
                            s.ShowModelErrors = true;
                            s.EncodeHtml = false;
                            s.ReadOnly = (readOnly == null) ? !attrib.Editable : (bool)readOnly;
                            s.Width = Unit.Percentage(100);
                        });
                    }
                    break;
                case "String[]": {
                        if (attrib.LookupList != null) {
                            LookupListModel<dynamic> lkpList = Ensco.Utilities.UtilitySystem.GetLookupList(attrib.LookupList);
                            if (lkpList.GridLookup) {
                                item.NestedExtensionType = FormLayoutNestedExtensionItemType.ComboBox;
                                item.SetNestedContent(() => {
                                    helper.RenderAction("GridLookupPartial", "Common", new { Area = "Common", name = item.FieldName, lookup = attrib.LookupList, multi = lkpList.MultiSelect, selected = metadata.Model });
                                });
                            }
                        }
                    }
                    break;
                case "DateTime": {
                        if (attrib.DateFormatType == ColumnAttribute.DateTimeDisplayType.TimeOnly) {
                            item.NestedExtension().TimeEdit(s => {
                                s.Properties.ValidationSettings.RequiredField.IsRequired = (requiredAttrib != null) ? true : false;
                                s.Properties.EditFormat = EditFormat.Custom;
                                s.Properties.EditFormatString = UtilitySystem.Settings.ConfigSettings["TimeFormat"];
                                s.Properties.DisplayFormatString = UtilitySystem.Settings.ConfigSettings["TimeFormat"];
                                s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                s.Properties.ValidationSettings.Display = Display.Dynamic;
                                s.Properties.ValidationSettings.SetFocusOnError = true;
                                s.Properties.NullDisplayText = "";
                                s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                s.ReadOnly = (readOnly == null) ? false : (bool)readOnly;
                                s.Width = Unit.Percentage(100);
                            });
                        } else {
                            item.NestedExtension().DateEdit(s => {
                                s.Properties.ValidationSettings.RequiredField.IsRequired = (requiredAttrib != null) ? true : false;
                                s.Properties.EditFormat = EditFormat.Custom;
                                s.Properties.UseMaskBehavior = true;
                                s.Properties.EditFormatString = (attrib.DateFormatType == ColumnAttribute.DateTimeDisplayType.DateOnly) ? UtilitySystem.Settings.ConfigSettings["DateFormat"] : UtilitySystem.Settings.ConfigSettings["DateTimeFormat"];
                                s.Properties.DisplayFormatString = (attrib.DateFormatType == ColumnAttribute.DateTimeDisplayType.DateOnly) ? UtilitySystem.Settings.ConfigSettings["DateFormat"] : UtilitySystem.Settings.ConfigSettings["DateTimeFormat"];
                                s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                s.Properties.ValidationSettings.Display = Display.Dynamic;
                                s.Properties.ValidationSettings.SetFocusOnError = true;
                                s.Properties.NullDisplayText = "";
                                s.ReadOnly = (readOnly == null) ? false : (bool)readOnly;
                                s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                            });
                        }
                    }
                    break;
                case "Boolean":
                    item.NestedExtension().CheckBox(s => {
                        s.Name = name;
                        s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                        s.Properties.ValidationSettings.Display = Display.Dynamic;
                        s.ShowModelErrors = true;
                        s.EncodeHtml = false;
                        s.ClientEnabled = true;
                        s.ReadOnly = (readOnly == null) ? !attrib.Editable : (bool)readOnly;
                        s.Width = Unit.Percentage(100);
                        s.Properties.ClientSideEvents.CheckedChanged = (eventHandlers != null && eventHandlers.ContainsKey(EnscoHelper.EventHandlerType.CheckedChanged)) ? eventHandlers[EnscoHelper.EventHandlerType.CheckedChanged] : "";
                    });
                    break;
                case "Int64":
                case "Int32": {
                        if (attrib.LookupList != null) {
                            string selectionChanged = (eventHandlers != null && eventHandlers.ContainsKey(EventHandlerType.SelectionChanged)) ? eventHandlers[EventHandlerType.SelectionChanged] : null;
                            string focusChanged = (eventHandlers != null && eventHandlers.ContainsKey(EventHandlerType.FocusRowChanged)) ? eventHandlers[EventHandlerType.FocusRowChanged] : null;
                            string rowClickEvent = (eventHandlers != null && eventHandlers.ContainsKey(EventHandlerType.RowClick)) ? eventHandlers[EventHandlerType.RowClick] : null;

                            LookupListModel<dynamic> lkpList = Ensco.Utilities.UtilitySystem.GetLookupList(attrib.LookupList);
                            if (lkpList.GridLookup) {
                                item.NestedExtensionType = FormLayoutNestedExtensionItemType.ComboBox;
                                item.SetNestedContent(() => {
                                    helper.RenderAction("GridLookupPartial", "Common", new { Area = "Common", name = item.FieldName, lookup = attrib.LookupList, multi = multi, selected = metadata.Model, focusRowChangedEvent = focusChanged, selectionChangedEvent = selectionChanged, rowClick = rowClickEvent });
                                });
                            } else if (lkpList.TreeLookup) {
                                helper.ViewContext.Writer.Write(helper.HiddenFor(expression).ToHtmlString());
                                item.Name = "DropDownEdit" + name;
                                item.NestedExtension().DropDownEdit(ddedit => {
                                    ddedit.Name = item.Name;
                                    ddedit.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                    ddedit.PreRender = (s, e) => {
                                        MVCxDropDownEdit dde = (MVCxDropDownEdit)s;
                                        dde.Text = (string)lkpList.GetDisplayValue(metadata.Model);
                                    };

                                    ddedit.SetDropDownWindowTemplateContent(cc => {
                                        helper.RenderAction("TreeLookupPartial", "Common", new { Area = "Common", name = name, lookup = attrib.LookupList, multi = false, selected = metadata.Model, focusRowChangedEvent = focusChanged, selectionChangedEvent = selectionChanged });
                                    });
                                });
                            } else {
                                string comboSelectionChanged = (eventHandlers != null && eventHandlers.ContainsKey(EventHandlerType.ComboBoxSelectionChanged)) ? eventHandlers[EventHandlerType.ComboBoxSelectionChanged] : null;
                                item.NestedExtension().ComboBox(s => {
                                    s.Name = name;
                                    s.ShowModelErrors = true;
                                    s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                    s.Properties.ValidationSettings.Display = Display.Dynamic;
                                    s.Properties.ValidationSettings.SetFocusOnError = true;
                                    s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                    s.Properties.DataSource = (filter != null) ? lkpList.Items.FindAll(filter) : lkpList.Items;
                                    s.Properties.NullText = "[Select]";
                                    s.Properties.TextField = lkpList.DisplayField;
                                    s.Properties.ValueField = lkpList.KeyFieldName;
                                    s.Properties.DataMember = lkpList.DisplayField;
                                    s.Properties.DropDownStyle = DropDownStyle.DropDownList;
                                    s.Properties.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                                    s.Properties.AllowNull = false;
                                    s.Properties.ClientSideEvents.SelectedIndexChanged = comboSelectionChanged;
                                    s.Properties.ClientSideEvents.Validation = (requiredAttrib != null) ? "function(s,e){ if (s.GetText() == \"\") e.isValid = false; }" : "function(s, e){}";
                                    s.ReadOnly = (readOnly == null) ? !attrib.Editable : (bool)readOnly;
                                    s.ClientEnabled = !s.ReadOnly;
                                });
                            }
                        } else {
                            item.NestedExtension().SpinEdit(s => {
                                s.Name = name;
                                s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                s.Properties.ValidationSettings.Display = Display.Dynamic;
                                s.ShowModelErrors = true;
                                s.EncodeHtml = false;
                                s.ReadOnly = (readOnly == null) ? !attrib.Editable : (bool)readOnly;
                                s.Width = Unit.Percentage(100);
                            });
                        }
                    }
                    break;
                case "Byte[]": {
                        item.NestedExtension().BinaryImage(p => {
                            p.Properties.ImageHeight = 100;
                            p.Properties.ImageWidth = 100;
                            p.Properties.EnableServerResize = true;
                            p.Properties.ImageSizeMode = ImageSizeMode.FitProportional;
                            p.CallbackRouteValues = new { Action = "PassportDetailUserImageUpdate", Controller = "Admin" };
                            p.Properties.EditingSettings.Enabled = (readOnly == null) ? !attrib.Editable : (bool)readOnly;
                            p.Properties.EditingSettings.UploadSettings.UploadValidationSettings.MaxFileSize = 4194304;
                            p.ContentBytes = (byte[])metadata.Model;
                        });
                    }
                    break;
            }

            return html;
        }
        public static void InitMemo(MemoSettings s, string name, bool? readOnly, ColumnAttribute attrib, Attribute requiredAttrib) {
            s.Name = name;
            s.Properties.ValidationSettings.RequiredField.IsRequired = (requiredAttrib != null) ? true : false;
            s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
            s.Properties.ValidationSettings.Display = Display.Dynamic;
            s.ShowModelErrors = true;
            s.EncodeHtml = false;
            s.ReadOnly = (readOnly == null) ? !attrib.Editable : (bool)readOnly;
            s.ClientEnabled = !s.ReadOnly;
            s.Properties.Rows = 5;
            //s.ControlStyle.Border.BorderWidth = Unit.Pixel(2);
            // s.ControlStyle.Border.BorderStyle = BorderStyle.Solid;
            //s.ControlStyle.Border.BorderColor = System.Drawing.Color.LightGray;
            //s.ControlStyle.BackColor = System.Drawing.Color.White;
            s.Width = Unit.Percentage(100);
            s.Properties.ClientSideEvents.Validation = (requiredAttrib != null) ? "function(s,e){ if (s.GetText() == \"\") e.isValid = false; }" : "function(s, e){}";
        }
        public static MvcHtmlString EnscoCapaEditor<TModel, TProperty>(this HtmlHelper<TModel> helper, MVCxFormLayoutItem item, Expression<Func<TModel, TProperty>> expression, Nullable<bool> readOnly = null, Dictionary<EventHandlerType, string> eventHandlers = null, Predicate<dynamic> filter = null, bool multi = false, bool isRequired = false, string caption = null) {
            item.FieldName = expression.Body.ToString().Split('.')[1];
            item.Caption = GetTranslation(caption == null ? item.FieldName : caption);
            return EnscoNestedEditor(helper, item, expression, readOnly, eventHandlers, filter, multi, isRequired );
        }
        public static MvcHtmlString EnscoNestedEditor<TModel, TProperty>(this HtmlHelper<TModel> helper, MVCxFormLayoutItem item, Expression<Func<TModel, TProperty>> expression, Nullable<bool> readOnly = null, Dictionary<EventHandlerType, string> eventHandlers = null, Predicate<dynamic> filter = null, bool multi = false, bool isRequired = false) {
            MvcHtmlString html = null;

            var name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            List<ColumnAttribute> attribs = UtilitySystem.GetCustomAttributes(typeof(TModel));
            ColumnAttribute attrib = attribs.FirstOrDefault(x => x.FieldName == name);
            if (attrib == null)
                return html;

            UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            Attribute requiredAttrib = System.Reflection.CustomAttributeExtensions.GetCustomAttribute(attrib.PropInfo, typeof(RequiredAttribute));
            if (isRequired)
                requiredAttrib = new ColumnAttribute();
            string typeName = attrib.PropInfo.PropertyType.Name;
            if (typeName.Contains("Nullable")) {
                typeName = (attrib.PropInfo.PropertyType.UnderlyingSystemType.GenericTypeArguments != null) ? attrib.PropInfo.PropertyType.UnderlyingSystemType.GenericTypeArguments[0].Name : "";
            }
            switch (typeName) {
                case "String":
                    if (attrib.LookupList != null) {
                        LookupListModel<dynamic> lkpList = Ensco.Utilities.UtilitySystem.GetLookupList(attrib.LookupList);
                        if (lkpList.GridLookup) {
                            item.NestedExtensionType = FormLayoutNestedExtensionItemType.ComboBox;
                            item.SetNestedContent(() => {
                                helper.RenderAction("GridLookupPartial", "Common", new { Area = "Common", name = item.FieldName, lookup = attrib.LookupList, multi = lkpList.MultiSelect, selected = metadata.Model });
                            });
                        } else {
                            string comboSelectionChanged = (eventHandlers != null && eventHandlers.ContainsKey(EventHandlerType.ComboBoxSelectionChanged)) ? eventHandlers[EventHandlerType.ComboBoxSelectionChanged] : null;
                            item.NestedExtension().ComboBox(s => {
                                s.Name = name;
                                s.ShowModelErrors = true;
                                s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                s.Properties.ValidationSettings.Display = Display.Dynamic;
                                s.Properties.ValidationSettings.SetFocusOnError = true;
                                s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                s.Properties.DataSource = (filter != null) ? lkpList.Items.FindAll(filter) : lkpList.Items;
                                s.Properties.NullText = "[Select]";
                                s.Properties.TextField = lkpList.DisplayField;
                                s.Properties.ValueField = lkpList.KeyFieldName;
                                s.Properties.DataMember = lkpList.DisplayField;
                                s.Properties.DropDownStyle = DropDownStyle.DropDownList;
                                s.Properties.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                                s.Properties.AllowNull = false;
                                s.Properties.ClientSideEvents.SelectedIndexChanged = comboSelectionChanged;
                                s.Properties.ClientSideEvents.Validation = (requiredAttrib != null) ? "function(s,e){ if (s.GetText() == \"\") e.isValid = false; }" : "function(s, e){}";
                                s.ReadOnly = (readOnly == null) ? !attrib.Editable : (bool)readOnly;
                            });
                        }
                    } else if (attrib.Memo) {
                        item.NestedExtension().Memo(s => {
                            InitMemo(s, name, readOnly, attrib, requiredAttrib);
                        });
                    } else {
                        item.NestedExtension().TextBox(s => {
                            s.Name = name;
                            s.Properties.ValidationSettings.RequiredField.IsRequired = (requiredAttrib != null) ? true : false;
                            s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                            s.Properties.ValidationSettings.Display = Display.Dynamic;
                            s.ShowModelErrors = true;
                            s.EncodeHtml = false;
                            s.ReadOnly = (readOnly == null) ? !attrib.Editable : (bool)readOnly;
                            s.ClientEnabled = !s.ReadOnly;
                            s.ReadOnly = false;
                            s.Width = Unit.Percentage(100);
                            s.Properties.ClientSideEvents.Validation = (requiredAttrib != null) ? "function(s,e){ if (s.GetText() == \"\") e.isValid = false; }" : "function(s, e){}";
                        });
                    }
                    break;
                case "String[]": {
                        if (attrib.LookupList != null) {
                            LookupListModel<dynamic> lkpList = Ensco.Utilities.UtilitySystem.GetLookupList(attrib.LookupList);
                            if (lkpList.GridLookup) {
                                item.NestedExtensionType = FormLayoutNestedExtensionItemType.ComboBox;
                                item.SetNestedContent(() => {
                                    helper.RenderAction("GridLookupPartial", "Common", new { Area = "Common", name = item.FieldName, lookup = attrib.LookupList, multi = lkpList.MultiSelect, selected = metadata.Model, IsRequired = (requiredAttrib != null), readOnly });
                                });
                            }
                        }
                    }
                    break;
                case "DateTime": {
                        if (attrib.DateFormatType == ColumnAttribute.DateTimeDisplayType.TimeOnly) {
                            item.NestedExtension().TimeEdit(s => {
                                s.Properties.ValidationSettings.RequiredField.IsRequired = (requiredAttrib != null) ? true : false;
                                s.Properties.EditFormat = EditFormat.Custom;
                                s.Properties.EditFormatString = UtilitySystem.Settings.ConfigSettings["TimeFormat"];
                                s.Properties.DisplayFormatString = UtilitySystem.Settings.ConfigSettings["TimeFormat"];
                                s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                s.Properties.ValidationSettings.Display = Display.Dynamic;
                                s.Properties.ValidationSettings.SetFocusOnError = true;
                                s.Properties.NullDisplayText = "";
                                s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                s.ReadOnly = (readOnly == null) ? false : (bool)readOnly;
                                s.Width = Unit.Percentage(100);
                            });
                        } else {
                            item.NestedExtension().DateEdit(s => {
                                s.Properties.ValidationSettings.RequiredField.IsRequired = (requiredAttrib != null) ? true : false;
                                s.Properties.EditFormat = EditFormat.Custom;
                                s.Properties.UseMaskBehavior = true;
                                s.Properties.EditFormatString = (attrib.DateFormatType == ColumnAttribute.DateTimeDisplayType.DateOnly) ? UtilitySystem.Settings.ConfigSettings["DateFormat"] : UtilitySystem.Settings.ConfigSettings["DateTimeFormat"];
                                s.Properties.DisplayFormatString = (attrib.DateFormatType == ColumnAttribute.DateTimeDisplayType.DateOnly) ? UtilitySystem.Settings.ConfigSettings["DateFormat"] : UtilitySystem.Settings.ConfigSettings["DateTimeFormat"];
                                s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                s.Properties.ValidationSettings.Display = Display.Dynamic;
                                s.Properties.ValidationSettings.SetFocusOnError = true;
                                s.Properties.NullDisplayText = "";
                                if (readOnly != null && (bool)readOnly) {
                                    s.ReadOnly = true;
                                    s.ClientEnabled = false;
                                }
                                s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                s.Properties.ClientSideEvents.Validation = (requiredAttrib != null) ? "function(s,e){ if (s.GetValue() == null ) e.isValid = false; }" : "function(s, e){}";
                            });
                        }
                    }
                    break;
                case "Boolean":
                    item.NestedExtension().CheckBox(s => {
                        s.Name = name;
                        s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                        s.Properties.ValidationSettings.Display = Display.Dynamic;
                        s.ShowModelErrors = true;
                        s.EncodeHtml = false;
                        s.ClientEnabled = true;
                        s.ReadOnly = (readOnly == null) ? !attrib.Editable : (bool)readOnly;
                        s.Width = Unit.Percentage(100);
                        s.Properties.ClientSideEvents.CheckedChanged = (eventHandlers != null && eventHandlers.ContainsKey(EnscoHelper.EventHandlerType.CheckedChanged)) ? eventHandlers[EnscoHelper.EventHandlerType.CheckedChanged] : "";
                    });
                    break;
                case "Int64":
                case "Int32": {
                        if (attrib.LookupList != null) {
                            string selectionChanged = (eventHandlers != null && eventHandlers.ContainsKey(EventHandlerType.SelectionChanged)) ? eventHandlers[EventHandlerType.SelectionChanged] : null;
                            string focusChanged = (eventHandlers != null && eventHandlers.ContainsKey(EventHandlerType.FocusRowChanged)) ? eventHandlers[EventHandlerType.FocusRowChanged] : null;
                            string rowClickEvent = (eventHandlers != null && eventHandlers.ContainsKey(EventHandlerType.RowClick)) ? eventHandlers[EventHandlerType.RowClick] : null;

                            LookupListModel<dynamic> lkpList = Ensco.Utilities.UtilitySystem.GetLookupList(attrib.LookupList);
                            if (lkpList.GridLookup) {
                                item.NestedExtensionType = FormLayoutNestedExtensionItemType.ComboBox;
                                item.SetNestedContent(() => {
                                    helper.RenderAction("GridLookupPartial", "Common", new { Area = "Common", name = item.FieldName, lookup = attrib.LookupList, multi = multi, selected = metadata.Model, focusRowChangedEvent = focusChanged, selectionChangedEvent = selectionChanged, IsRequired = (requiredAttrib != null), rowClick = rowClickEvent, readOnly });
                                });
                            } else if (lkpList.TreeLookup) {
                                helper.ViewContext.Writer.Write(helper.HiddenFor(expression).ToHtmlString());
                                item.Name = "DropDownEdit" + name;
                                item.NestedExtension().DropDownEdit(ddedit => {
                                    ddedit.Name = item.Name;
                                    ddedit.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                    ddedit.PreRender = (s, e) => {
                                        MVCxDropDownEdit dde = (MVCxDropDownEdit)s;
                                        dde.Text = (string)lkpList.GetDisplayValue(metadata.Model);
                                    };

                                    ddedit.SetDropDownWindowTemplateContent(cc => {
                                        helper.RenderAction("TreeLookupPartial", "Common", new { Area = "Common", name = name, lookup = attrib.LookupList, multi = false, selected = metadata.Model, focusRowChangedEvent = focusChanged, selectionChangedEvent = selectionChanged });
                                    });
                                });
                            } else {
                                string comboSelectionChanged = (eventHandlers != null && eventHandlers.ContainsKey(EventHandlerType.ComboBoxSelectionChanged)) ? eventHandlers[EventHandlerType.ComboBoxSelectionChanged] : null;
                                item.NestedExtension().ComboBox(s => {
                                    s.Name = name;
                                    s.ShowModelErrors = true;
                                    s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                    s.Properties.ValidationSettings.Display = Display.Dynamic;
                                    s.Properties.ValidationSettings.SetFocusOnError = true;
                                    s.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                    s.Properties.DataSource = (filter != null) ? lkpList.Items.FindAll(filter) : lkpList.Items;
                                    s.Properties.NullText = "[Select]";
                                    s.Properties.TextField = lkpList.DisplayField;
                                    s.Properties.ValueField = lkpList.KeyFieldName;
                                    s.Properties.DataMember = lkpList.DisplayField;
                                    s.Properties.DropDownStyle = DropDownStyle.DropDownList;
                                    s.Properties.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                                    s.Properties.AllowNull = false;
                                    s.Properties.ClientSideEvents.SelectedIndexChanged = comboSelectionChanged;
                                    s.Properties.ClientSideEvents.Validation = (requiredAttrib != null) ? "function(s,e){ if (s.GetText() == \"\") e.isValid = false; }" : "function(s, e){}";
                                    s.ReadOnly = (readOnly == null) ? !attrib.Editable : (bool)readOnly;
                                    s.ClientEnabled = !s.ReadOnly;
                                });
                            }
                        } else {
                            item.NestedExtension().SpinEdit(s => {
                                s.Name = name;
                                s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                                s.Properties.ValidationSettings.Display = Display.Dynamic;
                                s.ShowModelErrors = true;
                                s.EncodeHtml = false;
                                s.ReadOnly = (readOnly == null) ? !attrib.Editable : (bool)readOnly;
                                s.Width = Unit.Percentage(100);

                                if(s.ReadOnly) {
                                    s.ClientEnabled = false;
                                }
                            });
                        }
                    }
                    break;
                case "Byte[]": {
                        item.NestedExtension().BinaryImage(p => {
                            p.Properties.ImageHeight = 100;
                            p.Properties.ImageWidth = 100;
                            p.Properties.EnableServerResize = true;
                            p.Properties.ImageSizeMode = ImageSizeMode.FitProportional;
                            p.CallbackRouteValues = new { Action = "PassportDetailUserImageUpdate", Controller = "Admin" };
                            p.Properties.EditingSettings.Enabled = (readOnly == null) ? !attrib.Editable : (bool)readOnly;
                            p.Properties.EditingSettings.UploadSettings.UploadValidationSettings.MaxFileSize = 4194304;
                            p.ContentBytes = (byte[])metadata.Model;
                        });
                    }
                    break;
            }

            return html;
        }
        public static void EnscoSetupMenu(this HtmlHelper helper, DevExpress.Web.MenuItem item) {
            if (item.HasChildren) {
                Ensco.Models.MenuItem menuItem = (Ensco.Models.MenuItem)item.DataItem;
                object obj = HttpContext.GetGlobalResourceObject("Resources", menuItem.Text);
                item.Text = (obj != null) ? (string)obj : "RESOURCE NOT FOUND";
                foreach (DevExpress.Web.MenuItem child in item.Items) {
                    EnscoSetupMenu(helper, child);
                }
            } else {
                Ensco.Models.MenuItem menuItem = (Ensco.Models.MenuItem)item.DataItem;
                item.ClientVisible = menuItem.ClientVisible;
                object obj = HttpContext.GetGlobalResourceObject("Resources", menuItem.Text);
                item.Text = (obj != null) ? (string)obj : "RESOURCE NOT FOUND";
            }
        }

        public static MvcHtmlString FormatDateTime(this HtmlHelper helper, DateTimeOffset inputDate) {
            string dateTimeFormat = WebConfigurationManager.AppSettings["DateTimeFormat"];
            return MvcHtmlString.Create(inputDate.ToString(dateTimeFormat));
        }

        public static string GetDateTimeFormatString(this HtmlHelper helper) {
            return WebConfigurationManager.AppSettings["DateTimeFormat"];
        }
    }
}

using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Dynamic;

namespace Ensco.Irma.Oap.Common.Models
{
    public class GridDisplayColumn : GridBaseColumn
    {
        public GridDisplayColumn(string name, string displayName = "", MVCxGridViewColumnType columnType = MVCxGridViewColumnType.TextBox, int? width = 50, int? height = 100, int? editLayoutWidth = null, int? editLayoutHeight = null, bool isReadOnly = false, bool customizable = true, ErrorDisplayMode errorDisplayMode = ErrorDisplayMode.ImageWithTooltip, string displayFormat = "", bool isVisible = true, bool isWidthAndHeightInPercentage = true, object callBackRoute = null, Action<MVCxGridViewColumn> columnAction = null, GridCombo lookup = null, bool encodeHtml = true, int? groupIndex = null, DefaultBoolean allowHeaderFilter = DefaultBoolean.True, DefaultBoolean allowSort = DefaultBoolean.True, AutoFilterCondition autoFilterCondition = AutoFilterCondition.Contains, ColumnFilterMode filterMode = ColumnFilterMode.DisplayText, DefaultBoolean allowEditLayout = DefaultBoolean.True, int order = 0, ExpandoObject editorProperties = null ) : base(name, displayName, columnType, width, height, isVisible, isWidthAndHeightInPercentage, allowEditLayout, order)
        {
            IsReadOnly = isReadOnly;
            Customizable = customizable;
            ErrorDisplayMode = errorDisplayMode;
            DisplayFormat = displayFormat;
            CallBackRoute = callBackRoute;
            ColumnAction = columnAction;
            Lookup = lookup;
            EncodeHtml = encodeHtml;
            GroupIndex = groupIndex;
            AllowSort = allowSort;
            AllowHeaderFilter = allowHeaderFilter;
            AutoFilterCondition = autoFilterCondition;
            FilterMode = filterMode;
            EditorProperties = editorProperties;
            EditLayoutWidth = editLayoutWidth ?? width;
            EditLayoutHeight = editLayoutHeight ?? height;
            IsGridLookup = (lookup != null && lookup is GridLookup);
        }

        public bool IsGridLookup { get;}

        public bool IsReadOnly { get; }

        public bool Customizable { get; }

        public ErrorDisplayMode ErrorDisplayMode {get; }

        public string DisplayFormat { get; set; }

        public int? EditLayoutWidth { get; }

        public int? EditLayoutHeight { get; }

        public object CallBackRoute { get; }

        public Action<MVCxGridViewColumn> ColumnAction { get; }

        public GridCombo Lookup { get; }
        public bool EncodeHtml { get; }
        public int? GroupIndex { get; }
        public DefaultBoolean AllowHeaderFilter { get; }
        public DefaultBoolean AllowSort { get; }
        public AutoFilterCondition AutoFilterCondition { get; }
        public ColumnFilterMode FilterMode { get; }
        public dynamic EditorProperties { get; }
    }
}
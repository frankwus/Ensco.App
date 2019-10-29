using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ensco.Irma.Oap.Common.Models
{
    public class GridEditFormLayout
    {
        public GridEditFormLayout(IEnumerable<GridEditLayoutColumn> displayColumns, Action<EditModeCommandLayoutItem> processLayout, int? columnCount = null, int emptyLayputItemCount= 0,int adaptiveModeSingleColumnWindowInnerWidth = 700, GridViewEditingMode editMode = GridViewEditingMode.PopupEditForm, FormLayoutAdaptivityMode adaptiveMode = FormLayoutAdaptivityMode.SingleColumnWindowLimit, GridViewBatchStartEditAction batchEditAction = GridViewBatchStartEditAction.Click, GridViewBatchEditMode batchEditMode = GridViewBatchEditMode.Cell, bool batchEditAllowRegularDataItemTemplate = true)
        {
            
            ColumnCount = (!columnCount.HasValue) ? displayColumns.Where(c => c.IsVisible).Count()/2: columnCount.Value;
            DisplayColumns = displayColumns??new List<GridEditLayoutColumn>();
            ProcessLayout = processLayout;
            EditMode = editMode;
            AdaptiveMode = adaptiveMode;
            BatchEditAction = batchEditAction;
            BatchEditMode = batchEditMode;
            BatchEditAllowRegularDataItemTemplate = batchEditAllowRegularDataItemTemplate;
            EmptyLayputItemCount = emptyLayputItemCount;
            AdaptiveModeSingleColumnWindowInnerWidth = adaptiveModeSingleColumnWindowInnerWidth;
        }

        public int ColumnCount { get; }

        public IEnumerable<GridEditLayoutColumn> DisplayColumns { get; }

        public Action<EditModeCommandLayoutItem> ProcessLayout { get; }

        public GridViewEditingMode EditMode { get; }

        public FormLayoutAdaptivityMode AdaptiveMode { get; }
        public GridViewBatchStartEditAction BatchEditAction { get; }
        public GridViewBatchEditMode BatchEditMode { get; }
        public bool BatchEditAllowRegularDataItemTemplate { get; }
        public int EmptyLayputItemCount { get; }

        public int AdaptiveModeSingleColumnWindowInnerWidth { get; }
    }
     
}
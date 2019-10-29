using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;

namespace Ensco.Irma.Oap.Common.Models
{
    public class GridCombo
    {
        public GridCombo()
        {

        }

        public GridCombo(string name, object dataSource, string keyColumnName = "Id", string displayColumnName = "Name", string valueColumnName = "Id",string selectedIndexChangedEvent = "", object callbackRouteValues = null):this()
        {
            Name = name;
            DataSource = dataSource;
            KeyColumnName = keyColumnName;
            DisplayColumnName = displayColumnName;
            ValueColumnName = valueColumnName;
            SelectedIndexChangedEvent = selectedIndexChangedEvent;
            CallbackRouteValues = callbackRouteValues;
        }


        public string Name { get; set; }

        public object DataSource { get; set; }

        public string KeyColumnName { get; set; }

        public string DisplayColumnName { get; set; }

        public string ValueColumnName { get; set; }

        public string NullCaption { get; set; } = "[Select]";

        public Type ValueType { get; set; }

        public int Width { get; set; }

        public DropDownStyle Style { get; set; } = DropDownStyle.DropDownList;

        public IList<ListViewDisplayColumn> Columns { get; set; }

        public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

        public string SelectedIndexChangedEvent { get; set; }

        public string BeginCallBackEvent { get; set; }

        public object CallbackRouteValues { get; set; }
    }
}
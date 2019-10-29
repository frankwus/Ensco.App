using DevExpress.Utils;
using DevExpress.Web.Mvc;
using Ensco.Irma.Oap.Common.Extensions;
using System;
using Westwind.Globalization;

namespace Ensco.Irma.Oap.Common.Models
{
    public abstract class GridBaseColumn
    {
        public GridBaseColumn(string name, string displayName = "", MVCxGridViewColumnType columnType = MVCxGridViewColumnType.TextBox, int? width = 20, int? height = 10, bool isVisible = true, bool isWidthAndHeightInPercentage = true, DefaultBoolean allowEditLayout = DefaultBoolean.True, int Order = 0)
        {
            Name = name;
            DisplayName =  (string.IsNullOrEmpty(displayName) ? Name : displayName).Translate();
            ColumnType = columnType;
            IsVisible = isVisible;
            Width = width;
            Height = height;
            this.Order = Order;
            IsWidthAndHeightInPercentage = isWidthAndHeightInPercentage;
            AllowEditLayout = allowEditLayout;
        }

        public string Name { get; }

        public string DisplayName { get; set; }

        public MVCxGridViewColumnType ColumnType { get; set; }

        public bool IsVisible { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public int Order { get; set; }

        public bool IsWidthAndHeightInPercentage { get; set; }

        public DefaultBoolean AllowEditLayout { get; set; }

       
    }

    
}
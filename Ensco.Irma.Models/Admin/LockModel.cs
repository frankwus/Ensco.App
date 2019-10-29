using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models.Admin
{
    public class LockModel
    {
        [Column(Visible = false, Key=true)]
        public int id { get; set; }
        public string No { get; set; }
        [Column(LookupList = "YesNoList")]
        [Display(Name = "Ensco.Property.Available")]
        public string Available { get; set; }
        [Display(Name = "Ensco.Property.Comment")]
        public string Comment { get; set; }
        [Column(LookupList = "IsolationType")]
        [Display(Name = "Ensco.Property.IsolationType")]
        public Nullable<int> Craft { get; set; }
        [Column(Editable = false, DateFormatType = ColumnAttribute.DateTimeDisplayType.DateTime)]
        [Display(Name = "Ensco.Property.DateCreated")]
        public Nullable<System.DateTime> dt { get; set; }
    }
}

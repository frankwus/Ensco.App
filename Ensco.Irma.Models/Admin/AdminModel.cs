using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models.Admin
{
    public class AdminModel
    {
        [Column(Visible = false, Key =true)]
        public int id { get; set; }
        [Column(LookupList = "PobAdminRole")]
        [Display(Name = "Ensco.Property.Role")]
        public Nullable<int> RoleId { get; set; }
        [Column(HyperLinkField ="Passport", HyperLinkType=ColumnAttribute.HyperLinkFieldType.Passport)]
        [Display(Name = "Ensco.Property.Passport")]
        public string UserId { get; set; }
        [Display(Name = "Ensco.Property.Name")]
        public string Name { get; set; }
        [Display(Name = "Ensco.Property.Position")]
        public string Position { get; set; }
        [Display(Name = "Ensco.Property.LastOnboard")]
        public Nullable<System.DateTime> LastOnboard { get; set; }

        [Display(Name = "Ensco.Property.DateCreated")]
        public Nullable<System.DateTime> dt { get; set; }
        [Column(Visible =false, LookupList ="Passport")]
        public int Passport { get; set; }
        public AdminModel()
        {

        }
    }
}

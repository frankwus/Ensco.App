using Ensco.Models;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class PobCurrentModel
    {
        [Column(Visible =false, Key =true)]
        public int Id { get; set; }
        [Column(Visible = false)]
        public int PassportId { get; set; }
        [Column(Visible = false)]
        public int PositionId { get; set; }
        [Column(HyperLinkField = "PassportId", HyperLinkType = ColumnAttribute.HyperLinkFieldType.Passport)]
        public string Passport { get; set; }
        public string Company { get; set; }
        public string Nationality { get; set; }
        [Display(Name = "Ensco.Property.Name")]
        public string Name { get; set; }
        public string Position { get; set; }
        [Column(DisplayName = "Life Boat")]
        public string PrimaryLifeBoat { get; set; }
        [Column(DisplayName ="Room / Bed")]
        public string Cabin { get; set; }
        [Column(DisplayName = "Onboarded", DateFormatType = ColumnAttribute.DateTimeDisplayType.DateOnly)]
        public DateTime DateStart { get; set; }

    }
}

using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class RigPersonnelArchiveModel
    {
        [Column(Visible = false, Key = true, Customization = false)]
        public int Id { get; set; }
        [Column(HyperLinkField = "PassportId", HyperLinkType = ColumnAttribute.HyperLinkFieldType.Passport)]
        public string Passport { get; set; }
        [Column(Visible = false ) ]
        public int PassportId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Status { get; set; }
        public string Nationality { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public string Company { get; set; }
        public string Crew { get; set; }
        public string PersonnelType { get; set; }
        public Nullable<bool> ShortService { get; set; }
        public Nullable<bool> Essential { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }

        //[Column(Visible = false, Key =true, Customization =false)]
        //public int Id { get; set; }
        //[Column(Visible = false)]
        //public int CrewChangeId { get; set; }
        //[Column(Visible = false)]
        //public int CrewFlightId { get; set; }
        //[Column(Visible = false)]
        //public int CrewPobId { get; set; }
        //[Column(Visible = false)]
        //public int PassportId { get; set; }
        //[Column(HyperLinkField = "PassportId", HyperLinkType = ColumnAttribute.HyperLinkFieldType.Passport)]
        //public string Passport { get; set; }
        //[Column(Visible = false)]
        //[Display(Name ="Ensco.Property.Name")]
        //public string DisplayName { get; set; }
        //[Display(Name = "Ensco.Property.FirstName")]
        //public string FirstName { get; set; }
        //[Display(Name = "Ensco.Property.LastName")]
        //public string LastName { get; set; }
        //[Display(Name = "Ensco.Property.MiddleName")]
        //public string MiddleName { get; set; }
        //[Column(Visible = false)]
        //public string Email { get; set; }
        //[Column(Visible = false)]
        //[Display(Name = "Ensco.Property.PrimaryPhone")]
        //public string PrimaryPhone { get; set; }
        //public string Status { get; set; }
        //public string Nationality { get; set; }
        //public string Position { get; set; }
        //public string Department { get; set; }
        //public string Company { get; set; }
        //[Display(Name = "Ensco.Property.CompanyType")]
        //public string CompanyType { get; set; }
        //[Display(Name = "Ensco.Property.PersonnelType")]
        //public string PersonnelType { get; set; }
        //[Display(Name = "Ensco.Property.ShortService")]
        //public Nullable<bool> ShortService { get; set; }
        //[Column(Visible = false)]
        //[Display(Name = "Ensco.Property.DateOfBirth")]
        //public Nullable<System.DateTime> DateOfBirth { get; set; }
        //public Nullable<bool> Essential { get; set; }
        //[Column(Visible = false)]
        //[Display(Name = "Ensco.Property.VantageNumber")]
        //public string VantageNumber { get; set; }
        //[Column(Visible = false)]
        //public string Crew { get; set; }
        //[Column(Visible = false)]
        //public string Tour { get; set; }
        //[Column(Visible = false)]
        //public string Room { get; set; }
        //[Column(Visible = false)]
        //public string Bed { get; set; }
        //[Column(Visible = false)]
        //public string MusterStation1 { get; set; }
        //[Column(Visible = false)]
        //public string MusterStation2 { get; set; }
        //[Column(Visible = false)]
        //public string PrimaryLifeBoat { get; set; }
        //[Column(Visible = false)]
        //public string SecondaryLifeBoat { get; set; }
        //[Column(Visible = false)]
        //public string HomeAirport { get; set; }
        //[Column(Visible = false)]
        //public Nullable<double> BodyWeight { get; set; }
        //[Column(Visible = false)]
        //public Nullable<double> BagWeight { get; set; }
        //[Column(Visible = false)]
        //public Nullable<short> Bags { get; set; }
        //[Column(Visible = false)]
        //public string AirlineFlight { get; set; }
        //[Column(Visible = false)]
        //public string Location { get; set; }
        //[Column(Visible = false)]
        //public string Terminal { get; set; }
        //[Column(DateFormatType = ColumnAttribute.DateTimeDisplayType.DateOnly)]
        //[Display(Name ="Ensco.RigPersonnelArchiveModel.DateStart")]
        //public Nullable<System.DateTime> DateStart { get; set; }
        //[Column(DateFormatType = ColumnAttribute.DateTimeDisplayType.DateOnly)]
        //[Display(Name = "Ensco.RigPersonnelArchiveModel.DateEnd")]
        //public Nullable<System.DateTime> DateEnd { get; set; }
        //[Column(Visible = false)]
        //public Nullable<System.DateTime> DateEstimatedLeave { get; set; }
        //[Column(Visible = false)]
        //public string Optional1 { get; set; }
        //[Column(Visible = false)]
        //public string Optional2 { get; set; }
        //[Column(Visible = false)]
        //public string Optional3 { get; set; }
        //[Column(Visible = false)]
        //public string Optional4 { get; set; }
        //[Column(Visible = false)]
        //public string Role { get; set; }
        //[Column(Visible = false)]
        //public Nullable<System.DateTime> DateCreated { get; set; }
        //[Column(Visible = false)]
        //public Nullable<System.DateTime> DateModified { get; set; }
        //[Column(Visible = false)]
        //public string ModifiedBy { get; set; }
        //[Column(Visible = false)]
        //public string PassportIssuer { get; set; }
        //[Column(Visible = false)]
        //public string PassportNumber { get; set; }
        //[Column(Visible = false)]
        //public Nullable<System.DateTime> PassportExpires { get; set; }
        //[Column(Visible = false)]
        //public string VisaNumber { get; set; }
        //[Column(Visible = false)]
        //public Nullable<System.DateTime> VisaExpires { get; set; }
        //[Column(Visible = false)]
        //public string IdentificationIssuer { get; set; }
        //[Column(Visible = false)]
        //public string Identification { get; set; }
        //[Column(Visible = false)]
        //public Nullable<System.DateTime> IdentificationExpires { get; set; }
        //[Column(Visible = false)]
        //public string Comment { get; set; }
        //[Column(Visible = false)]
        //public string AddressLine1 { get; set; }
        //[Column(Visible = false)]
        //public string AddressLine2 { get; set; }
        //[Column(Visible = false)]
        //public string City { get; set; }
        //[Column(Visible = false)]
        //public string State { get; set; }
        //[Column(Visible = false)]
        //public string Country { get; set; }
        //[Column(Visible = false)]
        //public string PostalCode { get; set; }
        //[Column(Visible = false)]
        //public string SecondaryPhone { get; set; }
        //[Column(Visible = false)]
        //public string MaritalStatus { get; set; }
        //[Column(Visible = false)]
        //public string PersonalEmail { get; set; }
        //[Column(Visible = false)]
        //public string ContactInfoComment { get; set; }
        //[Column(Visible = false)]
        //public string EmergencyContactFirstName { get; set; }
        //[Column(Visible = false)]
        //public string EmergencyContactLastName { get; set; }
        //[Column(Visible = false)]
        //public string EmergencyContactRelationship { get; set; }
        //[Column(Visible = false)]
        //public string EmergencyContactEmail { get; set; }
        //[Column(Visible = false)]
        //public string EmergencyContactPrimaryPhone { get; set; }
        //[Column(Visible = false)]
        //public string EmergencyContactSecondaryPhone { get; set; }
        //[Column(Visible = false)]
        //public string EmergencyContactComment { get; set; }
        //[Column(Visible = false)]
        //public string VehicleMake { get; set; }
        //[Column(Visible = false)]
        //public string VehicleModel { get; set; }
        //[Column(Visible = false)]
        //public string VehicleColor { get; set; }
        //[Column(Visible = false)]
        //public string VehiclePlateIssuer { get; set; }
        //[Column(Visible = false)]
        //public string VehiclePlateNumber { get; set; }
        //[Column(Visible = false)]
        //public string VehicleLocation { get; set; }
        //[Column(Visible = false)]
        //public string VehicleComment { get; set; }
        //[Column(Visible = false)]
        //public string WorkBoots { get; set; }
        //[Column(Visible = false)]
        //public string RubberBoots { get; set; }
        //[Column(Visible = false)]
        //public string Gloves { get; set; }
        //[Column(Visible = false)]
        //public string TShirt { get; set; }
        //[Column(Visible = false)]
        //public string CoverallTan { get; set; }
        //[Column(Visible = false)]
        //public string CoverallFRC { get; set; }
        //[Column(Visible = false)]
        //public string CoverallChemical { get; set; }
        //[Column(Visible = false)]
        //public string CoverallWinter { get; set; }
        //[Column(Visible = false)]
        //public string PPEComment { get; set; }
        //[Column(Visible = false)]
        //public string Gender { get; set; }
        //[Column(Visible = false)]
        //public Nullable<int> LockerNumber { get; set; }
        //[Column(Visible = false)]
        //public string ISNNumber { get; set; }
    }
}

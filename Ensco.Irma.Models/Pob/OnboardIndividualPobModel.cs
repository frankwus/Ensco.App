using Ensco.Models;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class OnboardIndividualPobModel
    {
        public enum PobStatus { None, Onboard, Offboard, PlannedOnboard, PlannedOffboard, Disabled };

        [Column(Visible = false, Key = true)]
        public int Id { get; set; }
        [Column(Visible=false)]
        public int PassportId { get; set; }
        [Column(LookupList = "SharedAccount", DisplayName = "SharedAccount")]
       // [Required]
        public Nullable<int> SharedAccountId { get; set; }
        [Column(Visible = false)]
        public int CrewChangeId { get; set; }
        [Column(Visible = false)]
        public int CrewFlightId { get; set; }
        [Column(Visible = false, LookupList = "PassportNotOnboard")]
        public Nullable<int> SelectPoB { get; set; }
        [Column(Editable = false, HyperLinkField = "PassportId", HyperLinkType = ColumnAttribute.HyperLinkFieldType.Passport)]
        public string Passport { get; set; }
        [Column(Editable = false, Visible = false)]
        public string FirstName { get; set; }
        [Column(Editable = false, Visible = false)]
        public string LastName { get; set; }
        [Display(Name = "Ensco.Property.Name")]
        public string DisplayName { get; set; }
        [Column(LookupList = "Nationality")]
        [Display(Name = "Ensco.Property.Nationality")]
        public Nullable<int> Nationality { get; set; }
        [Column(Visible = false)]
        public string NationalityName { get { LookupListModel<dynamic> lkpList = UtilitySystem.GetLookupList("Nationality"); return (Nationality != null) ? (string)lkpList.GetDisplayValue(Nationality) : ""; } }

        [Column(LookupList = "Department")]
        [Display(Name = "Ensco.Property.Department")]
        public Nullable<int> Department { get; set; }
        [Column(Visible = false)]
        public string DepartmentName { get { LookupListModel<dynamic> lkpList = UtilitySystem.GetLookupList("Department"); return (Department != null) ? (string)lkpList.GetDisplayValue(Department) : ""; } }

        [Column(LookupList = "Position")]
        [Display(Name = "Ensco.Property.Position")]
        public Nullable<int> Position { get; set; }
        [Column(Visible = false)]
        public string PositionName { get { LookupListModel<dynamic> lkpList = UtilitySystem.GetLookupList("Position"); return (Position != null) ? (string)lkpList.GetDisplayValue(Position) : ""; } }

        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.Email")]
        public string Email { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.DateOfBirth")]
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        [Column(LookupList = "UserType")]
        [Display(Name = "Ensco.Property.CompanyType")]
        [Required]
        public Nullable<int> CompanyType { get; set; }
        [Column(LookupList = "Company")]
        [Display(Name = "Ensco.Property.Company")]
        public Nullable<int> Company { get; set; }
        [Column(Visible = false)]
        public string CompanyName { get { LookupListModel<dynamic> lkpList = UtilitySystem.GetLookupList("Company"); return (Company != null) ? (string)lkpList.GetDisplayValue(Company) : ""; } }

        [Column(LookupList = "PersonnelType")]
        [Display(Name = "Ensco.Property.PersonnelType")]
        [Required]
        public Nullable<int> PersonnelType { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.ShortService")]
        public Nullable<bool> ShortService { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.Essential")]
        public Nullable<bool> Essential { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.VantageNumber")]
        public string VantageNumber { get; set; }
        [Column(Visible = false)]
        public string HomeAirport { get; set; }
        [Column(LookupList = "Tour", Visible = false)]
        [Display(Name = "Ensco.Property.Tour")]
        [Required]
        public Nullable<int> Tour { get; set; }
        [Column(LookupList = "RigCrew")]
        [Display(Name = "Ensco.Property.Crew")]
        public Nullable<int> Crew { get; set; }
        [Column(LookupList = "Room")]
        [Display(Name = "Ensco.Property.Room")]
        [Required]
        public Nullable<int> Room { get; set; }
        [Column(LookupList = "RoomBed")]
        [Display(Name = "Ensco.Property.Bed")]
        [Required]
        public Nullable<int> Bed { get; set; }
        [Column(LookupList = "Room", Visible = false)]
        public Nullable<int> UsualRoom { get; set; }
        [Column(LookupList = "RoomBed", Visible = false)]
        public Nullable<int> UsualBed { get; set; }
        [Column(LookupList = "LifeBoat")]
        [Display(Name = "Ensco.Property.PrimaryLifeboard")]
        [Required]
        public Nullable<int> PrimaryLifeBoat { get; set; }
        [Column(LookupList = "LifeBoat",Visible = false)]
        [Display(Name = "Ensco.Property.SecondaryLifeboat")]
        [Required]
        public Nullable<int> SecondaryLifeBoat { get; set; }
        [Column(LookupList = "MusterStation", Visible =false)]
        [Display(Name = "Ensco.Property.MusterStation1")]
        public Nullable<int> MusterStation1 { get; set; }
        [Column(LookupList = "MusterStation", Visible =false)]
        [Display(Name = "Ensco.Property.MusterStation2")]
        public Nullable<int> MusterStation2 { get; set; }
        [Display(Name = "Ensco.Property.DateOfArrival")]
        [Required]
        public Nullable<System.DateTime> DateOfArrival { get; set; }
        [Display(Name = "Ensco.Property.DateEstimatedLeave")]
        public Nullable<System.DateTime> DateEstimatedLeave { get; set; }
        [Column(Visible = false)]
        public string Optional1 { get; set; }
        [Column(Visible = false)]
        public string Optional2 { get; set; }
        [Column(Visible = false)]
        public string Optional3 { get; set; }
        [Column(Visible = false)]
        public string Optional4 { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.Status")]
        public Nullable<int> Status { get; set; }
        [Display(Name = "Ensco.Property.DateCreated")]
        public DateTime DateCreated { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.DateModified")]
        public DateTime DateModified { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.ModifiedBy")]
        public Nullable<int> ModifiedBy { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.PrimaryPhone")]
        public string PrimaryPhone { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.PassportNumber")]
        public string PassportNumber { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.PassportExpires")]
        public Nullable<System.DateTime> PassportExpires { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.VisaExpires")]
        public Nullable<System.DateTime> VisaExpires { get; set; }
        [Column(Visible = false, LookupList ="Nationality")]
        [Display(Name = "Ensco.Property.IdentificationIssuer")]
        public Nullable<int> IdentificationIssuer { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.Identification")]
        public string Identification { get; set; }
        [Column(Visible = false, LookupList = "Nationality")]
        [Display(Name = "Ensco.Property.PassportIssuer")]
        public Nullable<int> PassportIssuer { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.IdentificationExpires")]
        public Nullable<System.DateTime> IdentificationExpires { get; set; }
        [Column(Visible = false, Memo =true)]
        [Display(Name = "Ensco.Property.Comments")]
        public string Comment { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.AddressLine1")]
        public string AddressLine1 { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.AddressLine2")]
        public string AddressLine2 { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.City")]
        public string City { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.State")]
        public string State { get; set; }
        [Column(LookupList = "Nationality", Visible = false)]
        [Display(Name = "Ensco.Property.Country")]
        public Nullable<int> Country { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.PostalCode")]
        public string PostalCode { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.SecondaryPhone")]
        public string SecondaryPhone { get; set; }
        [Column(LookupList = "MaritalStatus", Visible = false)]
        [Display(Name = "Ensco.Property.MaritalStatus")]
        public Nullable<int> MaritalStatus { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.PersonalEmail")]
        public string PersonalEmail { get; set; }
        [Column(Visible = false, Memo =true)]
        [Display(Name = "Ensco.Property.Comments")]
        public string ContactInfoComment { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.FirstName")]
        public string EmergencyContactFirstName { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.LastName")]
        public string EmergencyContactLastName { get; set; }
        [Column(LookupList = "Relationship", Visible = false)]        
        [Display(Name = "Ensco.Property.Relationship")]
        public Nullable<int> EmergencyContactRelationship { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.Email")]
        public string EmergencyContactEmail { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.PrimaryPhone")]
        public string EmergencyContactPrimaryPhone { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.SecondaryPhone")]
        public string EmergencyContactSecondaryPhone { get; set; }
        [Column(Visible = false, Memo =true)]
        [Display(Name = "Ensco.Property.Comments")]
        public string EmergencyContactComment { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.VehicleMake")]
        public string VehicleMake { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.VehicleModel")]
        public string VehicleModel { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.VehicleColor")]
        public string VehicleColor { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.VehiclePlateIssuer")]
        public string VehiclePlateIssuer { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.VehiclePlateNumber")]
        public string VehiclePlateNumber { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.VehicleLocation")]
        public string VehicleLocation { get; set; }
        [Column(Visible = false, Memo = true)]
        [Display(Name = "Ensco.Property.Comments")]
        public string VehicleComment { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.WorkBoots")]
        public string WorkBoots { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.RubberBoots")]
        public string RubberBoots { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.Gloves")]
        public string Gloves { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.TShirt")]
        public string TShirt { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.CoverAllTan")]
        public string CoverallTan { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.CoverAllFRC")]
        public string CoverallFRC { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.CoverAllChemical")]
        public string CoverallChemical { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.CoverAllWinter")]
        public string CoverallWinter { get; set; }
        [Column(Visible = false, Memo =true)]
        [Display(Name = "Ensco.Property.Comments")]
        public string PPEComment { get; set; }
        [Column(LookupList = "Gender", Visible = false)]
        [Display(Name = "Ensco.Property.Gender")]
        public Nullable<int> Gender { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.LockerNumber")]
        public Nullable<int> LockerNumber { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.ISNNumber")]
        public string ISNNumber { get; set; }

        [Column(Visible = false, LookupList = "RotationSchedule")]
        [Display(Name = "Ensco.Property.RotationSchedule")]
        public int  RotationSchedule { get; set; }
        [Display(Name = "Ensco.Property.Authentication")]
        public bool EnscoAuthentication { get; set; }

        [Column(Visible = false)]
        public bool Selected { get; set; }
        [Column(Visible = false)]
        public bool BatchOnboard { get; set; }

        [Column(Visible =false)]
        public List<RigFieldVisibilityModel> Requirements { get; set; }
        public OnboardIndividualPobModel()
        {
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
            DateOfArrival = DateTime.Now;
            BatchOnboard = true;
        }
        public CrewFlightPobModel Copy()
        {
            return (CrewFlightPobModel)this.MemberwiseClone();
        }

        public void Assign(CrewFlightPobModel model)
        {
            PropertyInfo[] props = model.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object value = prop.GetValue(model);
                if (value != null)
                {
                    prop.SetValue(this, value);
                }
            }
        }
    }
}

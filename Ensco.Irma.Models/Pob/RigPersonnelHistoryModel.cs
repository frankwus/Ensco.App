﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class RigPersonnelHistoryModel
    {
        public int Id { get; set; }
        public int RigId { get; set; }
        public int CrewChangeId { get; set; }
        public int CrewFlightId { get; set; }
        public int CrewPobId { get; set; }
        public int PassportId { get; set; }
        public string Passport { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string PrimaryPhone { get; set; }
        public string Status { get; set; }
        public string Nationality { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public string Company { get; set; }
        public string CompanyType { get; set; }
        public string PersonnelType { get; set; }
        public Nullable<bool> ShortService { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<bool> Essential { get; set; }
        public string VantageNumber { get; set; }
        public string Crew { get; set; }
        public string Tour { get; set; }
        public string Room { get; set; }
        public string Bed { get; set; }
        public string MusterStation1 { get; set; }
        public string MusterStation2 { get; set; }
        public string PrimaryLifeBoat { get; set; }
        public string SecondaryLifeBoat { get; set; }
        public string HomeAirport { get; set; }
        public Nullable<double> BodyWeight { get; set; }
        public Nullable<double> BagWeight { get; set; }
        public Nullable<short> Bags { get; set; }
        public string AirlineFlight { get; set; }
        public string Location { get; set; }
        public string Terminal { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public Nullable<System.DateTime> DateEstimatedLeave { get; set; }
        public string Optional1 { get; set; }
        public string Optional2 { get; set; }
        public string Optional3 { get; set; }
        public string Optional4 { get; set; }
        public string Role { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public string ModifiedBy { get; set; }
        public string PassportIssuer { get; set; }
        public string PassportNumber { get; set; }
        public Nullable<System.DateTime> PassportExpires { get; set; }
        public string VisaNumber { get; set; }
        public Nullable<System.DateTime> VisaExpires { get; set; }
        public string IdentificationIssuer { get; set; }
        public string Identification { get; set; }
        public Nullable<System.DateTime> IdentificationExpires { get; set; }
        public string Comment { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string SecondaryPhone { get; set; }
        public string MaritalStatus { get; set; }
        public string PersonalEmail { get; set; }
        public string ContactInfoComment { get; set; }
        public string EmergencyContactFirstName { get; set; }
        public string EmergencyContactLastName { get; set; }
        public string EmergencyContactRelationship { get; set; }
        public string EmergencyContactEmail { get; set; }
        public string EmergencyContactPrimaryPhone { get; set; }
        public string EmergencyContactSecondaryPhone { get; set; }
        public string EmergencyContactComment { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleColor { get; set; }
        public string VehiclePlateIssuer { get; set; }
        public string VehiclePlateNumber { get; set; }
        public string VehicleLocation { get; set; }
        public string VehicleComment { get; set; }
        public string WorkBoots { get; set; }
        public string RubberBoots { get; set; }
        public string Gloves { get; set; }
        public string TShirt { get; set; }
        public string CoverallTan { get; set; }
        public string CoverallFRC { get; set; }
        public string CoverallChemical { get; set; }
        public string CoverallWinter { get; set; }
        public string PPEComment { get; set; }
        public string Gender { get; set; }
        public Nullable<int> LockerNumber { get; set; }
        public string ISNNumber { get; set; }
    }
}
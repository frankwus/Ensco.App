using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class IrmaHomePassportModel
    {
        [Display(Name = "Ensco.Property.FirstName")]
        public string FirstName { get; set; }
        [Display(Name = "Ensco.Property.MiddleName")]
        public string MiddleName { get; set; }
        [Display(Name = "Ensco.Property.LastName")]
        public string LastName { get; set; }
        [Display(Name = "Ensco.Property.Passport")]
        public string EmployeeId { get; set; }
        [Display(Name = "Ensco.Property.Position")]
        public string Position { get; set; }
        [Display(Name = "Ensco.Property.Status")]
        public string Status { get; set; }
        [Display(Name = "Ensco.Property.RigFacility")]
        public string Rig { get; set; }
        [Display(Name = "Ensco.Property.Department")]
        public string Department { get; set; }
        [Display(Name = "Ensco.Property.Nationality")]
        public string Nationality { get; set; }
        [Display(Name = "Ensco.Property.BusinessUnit")]
        public string BusinessUnit { get; set; }
        [Display(Name = "Ensco.Property.Supervisor")]
        public string Supervisor { get; set; }
        [Display(Name = "Ensco.Property.Email")]
        public string EmailAddress { get; set; }
        [Display(Name = "Ensco.Property.Company")]
        public string Company { get; set; }
        [Display(Name = "Ensco.Property.RigManager")]
        public string RigManager { get; set; }
        [Display(Name = "Ensco.Property.Phone")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Ensco.Property.Country")]
        public string Country { get; set; }
        [Display(Name = "Ensco.Property.State")]
        public string State { get; set; }
        [Display(Name = "Ensco.Property.City")]
        public string City { get; set; }
        public IrmaHomePassportModel()
        {

        }
    }
}

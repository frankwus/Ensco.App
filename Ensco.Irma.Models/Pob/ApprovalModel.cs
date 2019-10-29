using Ensco.Models;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Ensco.Irma.Models
{
    public class ApprovalModel
    {
        public enum ApprovalType { CrewChange, Position, Company, LessonsLearnedPreApproval, LessonsLearnedApproval, CapBook };

        [Column(Visible = false, Key = true)]
        public int Id { get; set; }
        [Column(LookupList = "Position")]
        [Display(Name = "Ensco.Property.Position")]
        public int Position { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.Sequence")]
        public int Sequence { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.Name")]
        public string Name { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.Type")]
        public int Type { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.Requestor")]
        public int Requester { get; set; }
        [Column(Visible = false)]
        [Display(Name = "Ensco.Property.RequestedDate")]
        public System.DateTime RequestedDate { get; set; }
        [Display(Name = "Ensco.Property.RequestorComments")]
        public string RequesterComments { get; set; }
        [Column(Visible = false)]
        public string RequestInfo { get; set; }
        [Column(LookupList = "Passport")]
        [Display(Name = "Ensco.Property.Name")]
        public Nullable<int> Approver { get; set; }
        public SignatureModel Signature { get; set; }

        [Column(DateFormatType = ColumnAttribute.DateTimeDisplayType.DateOnly, Visible = false)]
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        [Column(LookupList = "ActionApprovalStatus")]
        [Display(Name = "Ensco.Property.Status")]
        public int Status { get; set; }
        [Display(Name = "Ensco.Property.Comments")]
        public string ApproverComments { get; set; }
        [Column(Visible = false)]
        public Nullable<int> RequestItemId { get; set; }
        public Nullable<DateTime> DueDate { get; set; }
        public ApprovalModel()
        {
            Signature = new SignatureModel();
            Status = 1; // Initially set as open
        }

        public void Initialize()
        {
            Signature.ItemId = (int)RequestItemId;
            Signature.Sequence = Sequence;
            Signature.Url = RequestInfo;
            Signature.Requester = Requester;
            Signature.Approver = Approver;
            Signature.ApprovalId = Id;
            Signature.Status = Status;
            Signature.SignatureText = (ApprovedDate != null) ? ((DateTime)ApprovedDate).ToString(UtilitySystem.Settings.ConfigSettings["DateFormat"]) : "";
        }
    }
}

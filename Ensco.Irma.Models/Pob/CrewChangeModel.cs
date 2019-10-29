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
    public class CrewChangeModel
    {
        public enum ActionStatus { None, Open, PendingApproval, PendingVerification, PendingReview, Verified, Reviewed, Approved, Closed, Disabled, Active };

        [Column(Visible = false, Key = true)]
        public int Id { get; set; }
        [Column(Visible = false)]
        public int RigId { get; set; }
        public string Title { get; set; }
        [Column(DateFormatType = ColumnAttribute.DateTimeDisplayType.DateOnly)]
        public System.DateTime DateCrewChange { get; set; }
        [Column(Visible = false)]
        public string Date { get { return DateCrewChange.ToString(UtilitySystem.Settings.ConfigSettings["DateFormat"]); } }
        [Column(LookupList = "CrewChangeStatus")]
        public int Status { get; set; }

        [Column(LookupList = "RigCrew")]
        [Display(Name ="Ensco.CrewChangeModel.InboundCrew")]
        public int InboundCrew { get; set; }
        [Column(DateFormatType = ColumnAttribute.DateTimeDisplayType.DateOnly, Editable = false)]
        public System.DateTime DateCreated { get; set; }
        [Column(Visible =false)]
        public string CreatedOn { get { return DateCrewChange.ToString(UtilitySystem.Settings.ConfigSettings["DateFormat"]); } }

        [Column(Visible = false)]
        public DataTableModel CrewFlightInfo { get; set; }
        [Column(Visible = false)]
        public DataTableModel InboundPersonnel { get; set; }
        [Column(Visible = false)]
        public DataTableModel OutboundPersonnel { get; set; }
        [Column(Visible = false)]
        public List<ApprovalModel> Verification { get; set; }

        [Column(Visible = false)]
        public string StatusName { get { LookupListModel<dynamic> lkpList = UtilitySystem.GetLookupList("CrewChangeStatus"); return (string)lkpList.GetDisplayValue(Status); } }
        [Column(Visible = false)]
        public string InboundCrewName { get { LookupListModel<dynamic> lkpList = UtilitySystem.GetLookupList("RigCrew"); return (string)lkpList.GetDisplayValue(InboundCrew); } }

        public CrewChangeModel()
        {
            Id = -1;
        }

        public CrewChangeModel Copy()
        {
            return (CrewChangeModel)this.MemberwiseClone();
        }

        public void Assign(CrewChangeModel model)
        {
            PropertyInfo[] props = model.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object value = prop.GetValue(model);
                if (value != null && prop.CanWrite)
                {
                    prop.SetValue(this, value);
                }
            }
        }
    }

}

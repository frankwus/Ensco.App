using Ensco.Models;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class CrewFlightModel
    {
        [Column(Visible=false, Key =true)]
        public int Id { get; set; }
        [Column(Visible =false)]
        public int RigId { get; set; }
        [Column(Visible = false)]
        public Nullable<int> CrewChangeId { get; set; }        
        public string Title { get; set; }
        [Column(Visible =false)]
        public string Number { get; set; }
        [Column(DateFormatType = ColumnAttribute.DateTimeDisplayType.DateTime)]
        public System.DateTime ScheduledTime { get; set; }
        public int MaxPeople { get; set; }
        [Column(Editable = false)]
        public Nullable<int> Assigned { get; set; }
        [Column(LookupList = "PobFlightStatus")]
        public int Status { get; set; }
        [Column(Editable = false)]
        public System.DateTime DateCreated { get; set; }
        [Column(Visible = false)]
        public List<dynamic> Personnel { get; set; }
        [Column(Visible = false)]
        public List<dynamic> ArrivingPersonnel { get; set; }
        [Column(Visible = false)]
        public List<dynamic> DepartingPersonnel { get; set; }
        [Column(Visible = false)]
        public string StatusName { get { LookupListModel<dynamic> lkpList = UtilitySystem.GetLookupList("PobFlightStatus"); return (string)lkpList.GetDisplayValue(Status); } }
        [Column(Visible = false)]
        public string CrewChange { get { LookupListModel<dynamic> lkpList = UtilitySystem.GetLookupList("CrewChange"); return (string)lkpList.GetDisplayValue(CrewChangeId); } }
        [Column(Visible = false)]
        public bool ShowAirportInfo { get; set; }

        public CrewFlightModel()
        {
            Number = "1";
            ShowAirportInfo = true;
        }

        public CrewFlightModel Copy()
        {
            return (CrewFlightModel)this.MemberwiseClone();
        }

        public void Assign(CrewFlightModel model)
        {
            PropertyInfo[] props = model.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object value = prop.GetValue(model);
                if (value != null)
                {
                    if (prop.CanWrite)
                        prop.SetValue(this, value);
                }
            }
        }
    }
}

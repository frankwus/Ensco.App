using Ensco.Models;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models.Admin
{
    public class JobCodeModel
    {
        [Column(Visible = false, Key = true)]
        public int  Id { get; set; }
        [Column(LookupList = "Rig", DisplayName = "Rig", Visible = true)]
        public Nullable<int> RigId { get; set; }

        //public string Rig {
        //    get 
        //        {
        //            LookupListModel<dynamic> lkp = UtilitySystem.GetLookupList("Rig");
        //            return (string)lkp.GetDisplayValue(RigId);
        //        }
        //}
        [Column( Editable = false)]
        public string JobCode { get; set; }
        public string EnscoPosition { get; set; }
        [Column(LookupList ="Position")]
        public Nullable<int> PositionId { get; set; }
        [Column(LookupList = "Department")]
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<bool> IsHod { get; set; }
        public Nullable<bool> IsAssessor { get; set; }
        public Nullable<bool> Status { get; set; }

    }
}

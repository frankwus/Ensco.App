using DevExpress.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Ensco.Irma.Oap.WebClient.Corp
{
    public partial class OapCheckListQuestionRelatedQuestionMap
    {
        public OapCheckListQuestionRelatedQuestionMap()
        {
        }

        public int? ChecklistId { get; set; }
    }

    public partial class RigOapChecklist
    {

        public RigOapChecklist()
        {
            if (Assessors == null)
            {
                Assessors = new System.Collections.ObjectModel.ObservableCollection<RigOapChecklistAssessor>();
            }
        }
        public string LocationName { get; set; }
        public string RigName { get; set; }

        public int? PositionId
        {
            get { return Owner?.PositionId; }
        }

        public string OwnerName
        {
            get { return Owner?.Name; }
        }

        public string OapType
        {
            get { return OapChecklist?.OapType.Name; }

        }

        public override int GetHashCode()
        {
            var settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            try
            {
                var sobject = JsonConvert.SerializeObject(this, settings);
                return sobject.GetHashCode();
            }
            catch (System.Exception)
            {
                ;
            }

            return 0;
        }
    }

    public partial class RigOapChecklistAssessor
    {
        public RigOapChecklistAssessor()
        {
            Role = "Assessor";
        }
    }

    public partial class RigOapChecklistQuestionFinding
    {
        [JsonIgnore]
        public UploadedFile[] UploadedFile { get; set; }
    }


}
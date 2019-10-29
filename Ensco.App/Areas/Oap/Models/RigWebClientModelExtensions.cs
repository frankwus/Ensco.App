using DevExpress.Web;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Ensco.Irma.Oap.WebClient.Rig
{
    //[MetadataType(typeof(RigOapChecklistAssessorValidation))]
    public partial class RigOapChecklistAssessor
    {
        public RigOapChecklistAssessor()
        {
            Role = "Assessor";
        }

        private string _name;
        private int? _positionId;
        private int? _departmentId;

        public string Name
        {
            get { return _name = User?.Name; }
            set{ _name = value; }            
        }

        public int? PositionId
        {
            get { return _positionId = User?.PositionId; }
            set { _positionId = value;  } 
        }

        public int? DepartmentId
        {
            get { return _departmentId = User?.DepartmentId; }
            set { _departmentId = value; }
        }

        public int? TourId
        {
            get;
            set;
        }

        //internal sealed class RigOapChecklistAssessorValidation
        //{
        //    [Remote("CheckIfLeadExists", "Generic", ErrorMessage = "Another accessor has been already set as lead.")]
        //    public bool IsLead { get; set; }
        //}
    }

    public partial class RigOapChecklistQuestion
    {
       public string Question {
            get { return OapChecklistQuestion?.Question; }
            set { OapChecklistQuestion.Question = value; }
        }
    }
   
    public partial class RigOapChecklist
    {
        public RigOapChecklist()
        {
            if (Assessors == null)
            {
                Assessors = new System.Collections.ObjectModel.ObservableCollection<RigOapChecklistAssessor>();
            }

            RigId = "0";
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

    public partial class RigOapChecklistQuestionNoAnswerComment
    {
        [JsonIgnore]
        public UploadedFile[] UploadedFile { get; set; }
    }

    
    public partial class RigOapChecklistQuestionFinding
    {
        [JsonIgnore]
        public UploadedFile[] UploadedFile { get; set; }

        //public string Question { get { return RigOapChecklistQuestion.OapChecklistQuestion.Question; } }

        //public int QuestionId { get { return RigOapChecklistQuestion.OapChecklistQuestion.Id; } }
    }

}
using Ensco.Irma.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.FSO.Models
{
    public class FSOChecklistViewModel
    {
        public int Id { get; set; }
        public FSOChecklist Checklist { get; set; }

        public List<FSOChecklistSection> Sections { get; set; }

        public List<FSOQuestion> Questions { get; set; } 

        //public List<FSOChecklistAnswer> Answers { get; set; } 

        public List<FSOChecklistObserver> Observers { get; set; }
        public FSOChecklistViewModel()
        {
            Checklist = new FSOChecklist() { Status = "New" };
            Observers = new List<FSOChecklistObserver>();
            Sections = new List<FSOChecklistSection>();            
        }
    }
}

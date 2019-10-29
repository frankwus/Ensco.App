using Ensco.FSO.Services;
using Ensco.Irma.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSOTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            //EnscoIrmaEntities _db = new EnscoIrmaEntities();

            //FSOServiceDataModel db = new FSOServiceDataModel();

            //var checklist = db.GetChecklist(2);
            

            //FSO_ChecklistSection section = new FSO_ChecklistSection
            //{
            //    Name = "Section 01",
            //    Type = "Type 1231"
            //};

            //_db.FSO_ChecklistSection.Add(section);
            //_db.SaveChanges();

            //FSO_ChecklistQuestion question = new FSO_ChecklistQuestion
            //{
            //    Question = "What is your name?",
            //    QuestionTopic = "Topic 1",
            //    ChecklistSectionId = section.Id
            //};

            //_db.FSO_ChecklistQuestion.Add(question);
            //_db.SaveChanges();

            //FSO_Checklist checklist = new FSO_Checklist
            //{
            //    CreatedAt = DateTime.Now,
            //    DateTimeObserved = DateTime.Now,
            //    Description = "This is a test",
            //    JobId = 1231,
            //    LocationId = 1231231,
            //    Status = "Open",
            //    LeadObserverPassport = "12313",
            //    OIMPassport = "1231231",
            //    SafeActsObserved = "Everything",
            //    UnsafeActsObserved = "Nothing",
            //    TimeSpentOnObservation = "30",
            //    NumberOfPeopleObserved = "10",
            //    NumberOfPeopleContacted = "2",
            //    FSO_ChecklistAnswer = new List<FSO_ChecklistAnswer>
            //    {
            //        new FSO_ChecklistAnswer { Description = "This is an answer", QuestionId = question.Id, Safe = true }                    
            //    }                

            //};

            //_db.FSO_Checklist.Add(checklist);
            //_db.SaveChanges();

        }
    }
}

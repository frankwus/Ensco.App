using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.OAP.Models
{
    public class CommentModel
    {
        [Column(Visible = false)]
        public int Id { get; set; }

        [Column(Visible = false)]
        public string SourceForm { get; set; }

        [Column(Visible = false)]
        public string SourceFormId { get; set; }

        [Column(LookupList = "Passport", DisplayName = "Name")]
        public int CommenterPassport { get; set; }

        [Column(LookupList = "Position")]
        public int Position { get; set; }

        [Column(DisplayName = "Question", Visible = false)]
        public string QuestionText { get; set; }

        [Column(DisplayName = "Date")]
        public DateTimeOffset CommentDate { get; set; }

        public string Comment { get; set; }
        [Column(LookupList = "Rig", Visible = false)]
        public Nullable<int> Rig { get; set; }

        [Column(Visible = false)]
        public Nullable<int> SiteId { get; set; }

        [Column(Visible = false)]
        public string QuestionId { get; set; }

    }
}

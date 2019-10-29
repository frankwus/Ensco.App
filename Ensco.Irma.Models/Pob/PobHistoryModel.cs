using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class PobHistoryModel
    {
        [Column(Visible =false, Key =true)]
        public int Id { get; set; }
        [Column(Visible = false)]
        public int PassportId { get; set; }
        public string BusinessUnit { get; set; }
        public string RigName { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int Days { get; set; } //Number of days on this rig
        public PobHistoryModel()
        {

        }
    }
}

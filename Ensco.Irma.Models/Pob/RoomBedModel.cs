using Ensco.Models;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class RoomBedModel
    {
        [Column(Visible = false, Key = true)]
        public int Id { get; set; }
        //[Column(Visible = false)]
        //public int RoomId { get; set; }
        public string Name { get; set; }

        public RoomBedModel()
        {

        }
    }
}

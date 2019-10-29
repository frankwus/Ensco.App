﻿using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class RoomBedSummaryModel
    {
        [Column(Visible = false, Key =true)]
        public int Id { get; set; }
        public string Room { get; set; }
        public string Bed { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Gender { get; set; }
        public string Company { get; set; }
        public string Crew { get; set; }
    }
}

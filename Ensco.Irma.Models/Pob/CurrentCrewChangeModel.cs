﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Models
{
    public class CurrentCrewChangeModel
    {
        public CrewChangeModel CrewChange { get; set; }
        public ApprovalModel Approval { get; set; }
    }
}

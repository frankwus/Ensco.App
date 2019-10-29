using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.OAP.Models
{
    public enum LessonsLearnedStatus
    {
        Open,
        Review,
        Approval,
        Approved,
        Rejected,
        Canceled,
        Closed
    }

    public enum LessonsLearnedImpact
    {
        Rig,
        BU,
        Global
    }

    public enum LessonsLearnedApprovalSection {
        PreApproval,
        Approval
    }
}

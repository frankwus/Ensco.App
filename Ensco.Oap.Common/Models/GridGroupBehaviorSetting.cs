using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Oap.Common.Models
{
    public class GridGroupBehaviorSetting
    {
        public GridGroupBehaviorSetting(bool autoExpandAllGroups = true, GridViewMergeGroupsMode groupMode = GridViewMergeGroupsMode.Always, string groupFormatForMergedGroup = null, string mergedGroupSeparator = null)
        {
            AutoExpandAllGroups = autoExpandAllGroups;
            GroupMode = groupMode;
            GroupFormatForMergedGroup = groupFormatForMergedGroup;
            MergedGroupSeparator = mergedGroupSeparator;
        }

        public bool AutoExpandAllGroups { get; }

        public GridViewMergeGroupsMode GroupMode { get; }

        public string GroupFormatForMergedGroup { get; }

        public string MergedGroupSeparator { get; }
    }
}

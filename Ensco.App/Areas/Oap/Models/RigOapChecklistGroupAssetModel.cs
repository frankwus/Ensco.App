using Ensco.Irma.Oap.Common.Extensions;
using Ensco.Irma.Oap.WebClient.Rig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ensco.App.Areas.Oap.Models
{
    public class RigOapChecklistGroupAssetModel
    { 
        public RigOapChecklistGroupAssetModel()
        {

        }

        public RigOapChecklistGroupAssetModel(Guid rigGroupAssetId, int checklistGroupId, int assetId, int assetGroupId, string groupName, int systemId, string systemName, int subSystemId, string subSystemName, string assetValue ) : this()
        {
            Id = rigGroupAssetId;
            ChecklistGroupId = checklistGroupId;
            AssetId = assetId;
            AssetGroupId = assetGroupId;
            GroupName = groupName?.Translate();
            SystemId = systemId;
            SystemName = systemName;
            SubSystemId = subSystemId;
            SubSystemName = subSystemName;
            AssetValue = assetValue;
        }

        public Guid Id { get; set;  }
        public int ChecklistGroupId { get; set; }
        public int AssetId { get; set; }
        public int AssetGroupId { get; set; }
        public string GroupName { get; set; }
        public int SystemId { get; set; }
        public string SystemName { get; set; }
        public int SubSystemId { get; set; }
        public string SubSystemName { get; set; }
        public string AssetValue { get; set; }
    }
}
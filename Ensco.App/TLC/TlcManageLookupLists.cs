using Ensco.Models;
using Ensco.TLC.Models;
using Ensco.TLC.Services;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.DataExtensions;
using System.Web;

namespace Ensco.App.TLC
{
    public static class TlcManageLookupLists
    {
        private static TLCLookupListData lookupList = new TLCLookupListData();
        public static void InitLookupLists()
        {
            UtilitySystem.LookupList.Add(lookupList);
        }
        public static LookupListModel<dynamic> GetLookupList(TlcConstants.TlcLookupLists type)
        {
            LookupListModel<dynamic> model = new LookupListModel<dynamic>();
            model.Name = type.ToString();
            string filter = null;

            switch (type)
            {
                case TlcConstants.TlcLookupLists.Competency:
                    {
                        RigCapService service = new RigCapService();
                        model.ModelType = typeof(CAP_CompLookupModel);
                        model.GridLookup = true;
                        model.DisplayField = "CompetencyTitle";
                        model.KeyFieldName = "CompId";
                        model.DataTable = service.GetListCompLookupQueryable();
                        model.DataTable = model.DataTable.Where(string.Format("RigId={0}", UtilitySystem.Settings.RigId));
                        model.Items = new List<dynamic>();
                        foreach (var item in model.DataTable)
                        {
                            model.Items.Add(item);
                        }
                    }
                    break;
                case TlcConstants.TlcLookupLists.KSA:
                    {
                        RigCapService service = new RigCapService();

                        model.ModelType = typeof(CAP_CompKSALookupModel);
                        model.GridLookup = true;
                        model.MultiSelect = true;
                        model.DisplayField = "KSATitle";
                        model.KeyFieldName = "KSAId";
                        model.DataTable = service.GetListCompKSALookupQueryable();
                        model.DataTable = model.DataTable.Where(string.Format("RigId={0}", UtilitySystem.Settings.RigId));
                        model.Items = new List<dynamic>();
                        foreach(var item in model.DataTable)
                        {
                            model.Items.Add(item);
                        }
                    }
                    break;
                case TlcConstants.TlcLookupLists.AssessmentType:
                    {
                        model.ModelType = typeof(TLC_AdminModel);
                        model.DisplayField = "AdminText";
                        model.KeyFieldName = "AdminText";
                        DropdownService service = new DropdownService();
                        model.Items = service.GetTLC_AdminModels("AssessmentType").Cast<dynamic>().ToList();
                    }
                    break;
                case TlcConstants.TlcLookupLists.AssessmentMethod:
                    {
                        model.ModelType = typeof(TLC_AdminModel);
                        model.DisplayField = "AdminText";
                        model.KeyFieldName = "AdminText";
                        DropdownService service = new DropdownService();
                        model.Items = service.GetTLC_AdminModels("AssessmentMethod").Cast<dynamic>().ToList();
                    }
                    break;
            }

            model.Initialize();

            return model;
        }
    }
}
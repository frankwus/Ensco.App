using Ensco.TLC.Models;
using Ensco.TLC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ensco.Utilities;

namespace Ensco.App.ProjectUtilities
{
    public static class CommonMethods
    {
        public static List<IRMA_ConfigurationModel> GetIrmaConfig()
        {
            DropdownService _dropdownService = new DropdownService();
            List<IRMA_ConfigurationModel> oMaster_IRMAConfiguration = CacheManager.GetObjectFromCache<IRMA_ConfigurationModel>("IRMAConfiguration");
            if (oMaster_IRMAConfiguration == null)
            {
                var result = _dropdownService.GetMasterConfigQueryable().ToList();
                oMaster_IRMAConfiguration = CacheManager.GetObjectFromCache("IRMAConfiguration", _dropdownService.GetMasterConfigQueryable().ToList());
            }

            return oMaster_IRMAConfiguration;
        }
    }
}
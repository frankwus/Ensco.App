using DevExpress.Web.Mvc;
using Ensco.Models;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ensco.Irma.Oap.Web.Rig.Areas.Oap
{
    public class OapUtilities
    {
        
        public static object GetCache(string cacheKey)
        {
            return null;
        }


        public static bool AddCache(string cacheKey, object cacheObject)
        {
            return true;
        }
         
        public static IEnumerable<T> GetLookup<T>(string lookupName)  
        {
            LookupListModel<dynamic> lkpList = Ensco.Utilities.UtilitySystem.GetLookupList(lookupName);
            if (lkpList != null)
            {
                var items = lkpList.Items.Cast<T>();
                return items;
            }
            return new List<T>(); 
        }
    }
}
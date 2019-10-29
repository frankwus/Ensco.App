using Ensco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.DataExtensions;

namespace Ensco.Irma.Models.Jobs
{
    public class ManageAssetsModel
    {
        public DataTableModel Assets { get; set; }
        public IQueryable TopLevelAssets { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }      
        
        public bool HasChildren(SafetyCriticalAssetModel item)
        {
            bool result = false;

            IQueryable qry = Assets.Dataset.Where(string.Format("Parent={0}", item.Id));
            result = (qry.Count() > 0) ? true : false;

            return result;
        }
        public List<SafetyCriticalAssetModel> GetTopLevelItems()
        {
            List<SafetyCriticalAssetModel> items = new List<SafetyCriticalAssetModel>();

            IQueryable qry = TopLevelAssets.Take(20);

            foreach(SafetyCriticalAssetModel item in qry)
            {
                items.Add(item);
            }

            return items;
        }
        public List<SafetyCriticalAssetModel> GetItems(SafetyCriticalAssetModel parent)
        {
            List<SafetyCriticalAssetModel> items = new List<SafetyCriticalAssetModel>();
            IQueryable qry = Assets.Dataset.Where(string.Format("Parent={0}", parent.Id));
            qry = TopLevelAssets.Take(20);

            foreach (SafetyCriticalAssetModel item in qry)
            {
                items.Add(item);
            }
            return items;
        }
    }
}

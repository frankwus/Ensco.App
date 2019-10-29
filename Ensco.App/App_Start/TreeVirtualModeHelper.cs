using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;
using Ensco.Irma.Models.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ensco.App.App_Start
{
    public static class TreeVirtualModeHelper
    {
        public static void VirtualModeCreateChildren(TreeListVirtualModeCreateChildrenEventArgs e)
        {
            ManageAssetsModel model = (ManageAssetsModel)HttpContext.Current.Session["ManageAssetsModel"];
            
            if(e.NodeObject == null)
            {
                e.Children = model.GetTopLevelItems();
            }
            else
            {
                e.Children = model.GetItems((SafetyCriticalAssetModel)e.NodeObject);
            }
        }

        //public static void CreateChildren(TreeViewVirtualModeCreateChildrenEventArgs e)
        //{
        //    ManageAssetsModel model = (ManageAssetsModel)HttpContext.Current.Session["ManageAssetsModel"];

        //    if (e.NodeObject == null)
        //    {
        //        e.Children = model.GetTopLevelItems(); 
        //    }
        //    else
        //    {
        //        e.Children = model.GetItems((SafetyCriticalAssetModel)e.NodeObject);
        //    }
        //}
        public static void VirtualModeNodeCreating(TreeListVirtualModeNodeCreatingEventArgs e)
        {
            ManageAssetsModel model = (ManageAssetsModel)HttpContext.Current.Session["ManageAssetsModel"];
            SafetyCriticalAssetModel item = e.NodeObject as SafetyCriticalAssetModel;
            if(item != null)
            {
                e.NodeKeyValue = GetNodeGuid(item.Id.ToString());
                e.IsLeaf = !model.HasChildren(item);
                e.SetNodeValue("Name", item.Name);
            }
        }

        static HttpRequest Request { get { return HttpContext.Current.Request; } }
        static Dictionary<string, Guid> Map
        {
            get
            {
                const string key = "DX_PATH_GUID_MAP";
                if (HttpContext.Current.Session[key] == null)
                    HttpContext.Current.Session[key] = new Dictionary<string, Guid>();
                return HttpContext.Current.Session[key] as Dictionary<string, Guid>;
            }
        }
        static Guid GetNodeGuid(string path)
        {
            if (!Map.ContainsKey(path))
                Map[path] = Guid.NewGuid();
            return Map[path];
        }
    }
}
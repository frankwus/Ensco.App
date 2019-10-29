using Ensco.App.Areas.Oap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ensco.App.ModelBinders
{
    public class OapChecklistAnswerBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //IEnumerable<OapGenericCheckListFlatModel> valuesList = new List<OapGenericCheckListFlatModel>();

            //var request = HttpContext.Current.Request;

            //foreach (var key in request.Params.AllKeys)
            //{
            //    if (key.Contains("YesNoNa"))
            //    {
            //        new OapGenericCheckListFlatModel()
            //        {

            //        }
            //    }
            //} 
            return null;
        }
    }
}
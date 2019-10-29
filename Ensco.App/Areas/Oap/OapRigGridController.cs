namespace Ensco.Irma.Oap.Web.Oap.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using AutoMapper;
    using DevExpress.Web.Mvc;
    using Ensco.Irma.Oap.Common.Controllers;
    using Ensco.Irma.Oap.WebClient.Rig;
    using Westwind.Globalization;

    public abstract class OapRigGridController : OapRigController
    {

        protected void SelectRow(int id, string name, ref GridViewSettings settings)
        {
            //Select the Current Row when it defined.
            if (id != 0)
            {
                settings.Columns.Grid.Selection.SelectRowByKey(id);
            }
        }

        public abstract void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext);

        //protected virtual string Translate(string value, string resourceSet = "OapResources")
        //{
        //    return DbRes.T(value, resourceSet);
        //}

        protected IEnumerable<TDestination> MapToViewModel<TSource, TDestination>(IEnumerable<TSource> sourceIEnumerable)
        {
            return MapToViewModel<TSource, TDestination>(sourceIEnumerable, null);
        }
        protected IEnumerable<TDestination> MapToViewModel<TSource, TDestination>(IEnumerable<TSource> sourceIEnumerable, Action<TSource, TDestination> perItemAction)
        {
            if (sourceIEnumerable == null) throw new ArgumentException("Source IEnumerable cannot be null");

            // Mapping to view model
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
            var mapper = config.CreateMapper();
            IList<TDestination> viewModelList = new List<TDestination>();

            foreach (var item in sourceIEnumerable)
            {
                var viewModel = mapper.Map<TDestination>(item);
                perItemAction?.Invoke(item, viewModel);
                viewModelList.Add(viewModel);
            }

            return viewModelList;
        }
    }
}
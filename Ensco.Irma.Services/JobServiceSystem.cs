using Ensco.Irma.Data;
using Ensco.Irma.Models.Jobs;
using Ensco.Models;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Services
{
    public static class JobServiceSystem
    {
        private static JobLookupListData lookupList = new JobLookupListData();
        public static void Initialize()
        {
            // Initialize Lookup List interface to load lookup on demand
            UtilitySystem.LookupList.Add(lookupList);
        }

        public static void Terminate()
        {

        }

        static public JobsHomeModel GetJobsHomeModel()
        {
            JobsHomeModel model = new JobsHomeModel();

            // Permits summary
            IIrmaServiceDataModel summaryModel = GetServiceModel(JobConstants.Models.JobSummary);
            JobSummaryModel item = (JobSummaryModel)summaryModel.GetItem("", "Id");
            model.Summary = new List<JobSummaryModel>();
            model.Summary.Add(item);

            // Permits
            IIrmaServiceDataModel permitsModel = GetServiceModel(JobConstants.Models.Permits);
            model.Permits = new DataTableModel(1, permitsModel.GetQueryable("Id"));

            return model;
        }
        static public LookupListModel<dynamic> GetLookupList(JobConstants.LookupLists type)
        {
            IIrmaServiceDataModel dataModel = null;
            LookupListModel<dynamic> model = new LookupListModel<dynamic>();
            model.Name = type.ToString();
            string filter = null;
            switch (type)
            {
            }

            if (dataModel != null)
            {
                model.Items = (filter != null) ? dataModel.GetItems(filter, "Id") : dataModel.GetAllItems();
            }

            model.Initialize();

            return model;
        }

        public static IIrmaServiceDataModel GetServiceModel(JobConstants.Models modelType)
        {
            IIrmaServiceDataModel service = null;

            switch (modelType)
            {
                case JobConstants.Models.JobSummary:
                    service = new IrmaServiceDataModel<JobSummaryModel, JOB_SummaryView>();
                    break;
                case JobConstants.Models.Permits:
                    service = new IrmaServiceDataModel<PermitModel, JOB_PermitView>();
                    break;
            }

            return service;
        }
    }
}

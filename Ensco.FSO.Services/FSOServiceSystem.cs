using Ensco.FSO.Models;
using Ensco.Irma.Data;
using Ensco.Services;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.FSO.Services
{
    public static class FSOServiceSystem
    {
        public enum FSODataModelType {
            SyncedCourse
        };

        public static IFSOServiceDataModel GetServiceModel(FSODataModelType modelType)
        {
            IFSOServiceDataModel dataModel = null;
            switch (modelType)
            {
                case FSODataModelType.SyncedCourse:
                    dataModel = new FSOServiceDataModel<SyncedCourseModel, TRN_CoursesSyncedToRig>();
                    break;
            }

            return dataModel;
        }

    }
}

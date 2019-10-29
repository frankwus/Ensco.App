using Ensco.Irma.Data;
using Ensco.Irma.Models;
using Ensco.Irma.Models.Admin;
using Ensco.Irma.Models.Irma;
using Ensco.Models;
using Ensco.Services;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.DataExtensions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;
using Ensco.Irma.Data.Repositories.POB;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using System.Data.Entity.SqlServer;

namespace Ensco.Irma.Services {
    public static class IrmaServiceSystem {
        private static IrmaLookupListData lookupList = new IrmaLookupListData();
        public static void Initialize() {
            // Initialize Resource
            ResourceDataManager.Resources.Add(GetResources);

            // Load Localizations for Irma and merge to _resources in Ensco.Services (ServiceSystem)

            // Initialize Lookup List interface to load lookup on demand
            UtilitySystem.LookupList.Add(lookupList);

        }
        public static void Terminate() {

        }

        public static IQueryable GetResources(int appId, string culture) {
            IQueryable queryable = IrmaServiceSystem.GetQueryable(IrmaConstants.IrmaPobModels.Resources, "Name");

            if (queryable != null) {
                queryable = queryable.Where(string.Format("ApplicationId={0}", appId));
            }

            return queryable;
        }

        public static List<int> GetAdminRoles(string passport) {
            List<int> roles = new List<int>();

            IIrmaServiceDataModel dataModel = GetServiceModel(IrmaConstants.IrmaPobModels.Admin);
            List<dynamic> items = dataModel.GetItems(string.Format("UserId=\"{0}\"", passport), "RoleId");

            if (items.Count > 0) {
                foreach (AdminModel item in items) {
                    if (item.RoleId != null)
                        roles.Add((int)item.RoleId);
                }
            }
            return roles;
        }

        static public int GetMaxPOB() {
            IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.Rig);

            RigModel rig = dataModel.GetItem(string.Format("Id={0}", UtilitySystem.Settings.RigId), "Id") as RigModel;
            if (rig != null) {
                return (int)(rig.MaxPOB != null ? rig.MaxPOB : 0);
            }
            return 0;
        }
        static public bool SaveMaxPOB(int maxPob) {
            bool result = true;

            IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.Rig);
            RigModel rig = dataModel.GetItem(string.Format("Id={0}", UtilitySystem.Settings.RigId), "Id") as RigModel;
            if (rig != null) {
                rig.MaxPOB = maxPob;
                result = dataModel.Update(rig);
            }

            return result;
        }

        static public string GetAdminCustomValue(string name) {
            string value = "";

            List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>();
            parameters.Add(new KeyValuePair<string, object>("Name", name));

            object result = RunStoredProcedure("GetAdminCustomValue", parameters);
            value = result.ToString();

            return value;
        }
        static public bool UpdateAdminCustomValue(string name, string value) {
            List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>();
            parameters.Add(new KeyValuePair<string, object>("Name", name));
            parameters.Add(new KeyValuePair<string, object>("Value", value));

            object result = RunStoredProcedure("UpdateAdminCustomValue", parameters);

            return true;
        }

        static public PobSummaryReportModel GetSummaryReportData() {
            PobSummaryReportModel model = new PobSummaryReportModel();

            DateTime curDate = DateTime.Now;
            curDate = curDate.AddDays(-1.0);

            IIrmaServiceDataModel onoffboardModel = GetServiceModel(IrmaConstants.IrmaPobModels.PobOnOffboardView);
            List<dynamic> list = onoffboardModel.GetItems(string.Format("StatusId=1"), "Id");
            List<PersonnelOnOffBoardModel> arrivals = list.Cast<PersonnelOnOffBoardModel>().ToList();
            model.ArrivalLog = arrivals.FindAll(x => x.On >= curDate);

            list = onoffboardModel.GetItems(string.Format("StatusId=2"), "Id");
            List<PersonnelOnOffBoardModel> departs = list.Cast<PersonnelOnOffBoardModel>().ToList();
            model.DepartureLog = departs.FindAll(x => x.Off >= curDate);

            IIrmaServiceDataModel summary = GetServiceModel(IrmaConstants.IrmaPobModels.PobSummaryView);
            List<dynamic> items = summary.GetAllItems();
            int totalPob = 0;
            model.Summary = new PobSummaryModel();
            foreach (PobSummaryDataModel item in items) {
                totalPob += item.Count;
                switch (item.PersonnelType) {
                    case 165:
                        model.Summary.EnscoStandard = item.Count;
                        break;
                    case 166:
                        model.Summary.EnscoOther = item.Count;
                        break;
                    case 167:
                        model.Summary.EnscoService = item.Count;
                        break;
                    case 168:
                        model.Summary.EnscoCatering = item.Count;
                        break;
                    case 169:
                        model.Summary.Client = item.Count;
                        break;
                    case 170:
                        model.Summary.ClientService = item.Count;
                        break;
                }
            }
            model.Summary.EnscoPerContract = GetMaxPOB();
            model.Summary.TotalPOB = totalPob;

            IIrmaServiceDataModel summaryCompany = GetServiceModel(IrmaConstants.IrmaPobModels.PobSummaryByCompanyType);
            model.SummaryByCompanyType = summaryCompany.GetAllItems().Cast<PobSummaryByCompanyTypeModel>().ToList();

            model.RigName = UtilitySystem.Settings.RigName;

            IIrmaServiceDataModel crewBD = GetServiceModel(IrmaConstants.IrmaPobModels.CrewBreakDown);
            model.CrewBreakdown = crewBD.GetAllItems().Cast<PobCrewBreakdownModel>().ToList();

            IIrmaServiceDataModel lbComp = GetServiceModel(IrmaConstants.IrmaPobModels.LifeBoatCompliance);
            model.LifeboatBreakdown = lbComp.GetAllItems().Cast<LifeBoatComplianceReportModel>().ToList();

            IIrmaServiceDataModel countsDataModel = GetServiceModel(IrmaConstants.IrmaPobModels.PobSummaryCounts);
            PobSummaryCountsModel countsModel = countsDataModel.GetItem(string.Format("Id=1"), "Id");
            model.ShortService = countsModel.ShortServiceYes;
            model.EssentialNo = countsModel.EsstentialNo;
            model.Essential = countsModel.EsstentialYes;

            // Get Available Beds
            List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>();
            parameters.Add(new KeyValuePair<string, object>("rigid", UtilitySystem.Settings.RigId));
            object result = RunStoredProcedure("GetAvailableBeds", parameters);
            model.BedsAvailable = (result != null) ? (int)result : 0;

            return model;
        }
        public static List<T> GetListLookup2<T>(string name  ) where T : class 
        {
            Type objectType = typeof(IrmaServiceSystem );
            Assembly assem = typeof(Ensco.Irma.Data.Master_ActionStatus).Assembly;
            MethodInfo method = objectType.GetMethod("GetList");
            Type type = assem.GetType(name, true);
            MethodInfo fun = method.MakeGenericMethod(type); 
            return (List <T> ) fun.Invoke(null, null);
        }
        public static object  GetListLookup(string lookup) {
            string name = ""; 
            switch(lookup) {
                case "PassportEmail":
                    name = "Ensco.Irma.Data.Master_UserLookupView";
                    break;
                case "Passport":
                    name = "Ensco.Irma.Data.Master_UserLookupView";
                    break;
            }
            Type objectType = typeof(IrmaServiceSystem);
            Assembly assem = typeof(Ensco.Irma.Data.Master_ActionStatus).Assembly;
            MethodInfo method = objectType.GetMethod("GetList");
            Type type = assem.GetType(name, true);
            MethodInfo genericMethod = method.MakeGenericMethod(type);
            return genericMethod.Invoke(null, null);
        }
        public static List<T> GetList <T>()where T : class 
        {
            string s = typeof(T).Name; 
            EnscoIrmaEntities db = new EnscoIrmaEntities();
            return db.Set<T>().ToList();
        }
        public static  List<Master_UserLookupView> GetList2()
        {
            Ensco.Irma.Data.Master_UserLookupView type = new Irma.Data.Master_UserLookupView();
            GetList<Ensco.Irma.Data.Master_UserLookupView>(); 
            EnscoIrmaEntities db = new EnscoIrmaEntities();
            return db.Master_UserLookupView.ToList(); 
        }
        static public LookupListModel<dynamic> GetLookupList(IrmaConstants.IrmaLookupLists type) {
            IIrmaServiceDataModel dataModel = null;
            LookupListModel<dynamic> model = new LookupListModel<dynamic>();
            model.Name = type.ToString();
            string filter = null;
            switch (type) {
                case IrmaConstants.IrmaLookupLists.PobStatus:
                    dataModel = new IrmaServiceDataModel<PobStatusModel, POB_PobStatus>();
                    model.ModelType = typeof(PobStatusModel);
                    break;
                case IrmaConstants.IrmaLookupLists.PobPlanningStatus:
                    dataModel = new IrmaServiceDataModel<PobStatusModel, POB_PobStatus>();
                    model.ModelType = typeof(PobStatusModel);
                    filter = string.Format("Id = 3 OR Id = 4"); // Arriving and Departing
                    break;
                case IrmaConstants.IrmaLookupLists.PobFlightStatus:
                    dataModel = new IrmaServiceDataModel<ActionStatusModel, Master_ActionStatus>();
                    model.ModelType = typeof(ActionStatusModel);
                    filter = string.Format("Id = 1 OR Id = 9"); // PlannedOnboard and PlannedOffboard
                    break;
                case IrmaConstants.IrmaLookupLists.LifeBoat:
                    dataModel = new IrmaServiceDataModel<LifeBoatModel, POB_LifeBoat>();
                    model.ModelType = typeof(LifeBoatModel);
                    break;
                case IrmaConstants.IrmaLookupLists.MusterStation:
                    dataModel = new IrmaServiceDataModel<MusterStationModel, POB_MusterStation>();
                    model.ModelType = typeof(MusterStationModel);
                    break;
                case IrmaConstants.IrmaLookupLists.Room:
                    dataModel = new IrmaServiceDataModel<RoomModel, POB_Room>();
                    model.ModelType = typeof(RoomModel);
                    break;
                case IrmaConstants.IrmaLookupLists.RoomBed:
                    dataModel = new IrmaServiceDataModel<RoomBedModel, POB_RoomBed>();
                    model.ModelType = typeof(RoomBedModel);
                    break;
                case IrmaConstants.IrmaLookupLists.Tour:
                    dataModel = new IrmaServiceDataModel<TourModel, POB_Tour>();
                    model.ModelType = typeof(TourModel);
                    break;
                case IrmaConstants.IrmaLookupLists.RigCrew:
                    dataModel = new IrmaServiceDataModel<RigCrewModel, POB_RigCrew>();
                    model.ModelType = typeof(RigCrewModel);
                    break;
                case IrmaConstants.IrmaLookupLists.ScheduleType:
                    dataModel = new IrmaServiceDataModel<ScheduleTypeModel, POB_ScheduleType>();
                    model.ModelType = typeof(ScheduleTypeModel);
                    break;
                case IrmaConstants.IrmaLookupLists.CrewChange:
                    dataModel = new IrmaServiceDataModel<CrewChangeModel, POB_CrewChange>();
                    model.ModelType = typeof(CrewChangeModel);
                    model.DisplayField = "Title";
                    model.MultiSelect = true;
                    break;
                case IrmaConstants.IrmaLookupLists.FlightManifest:
                    dataModel = new IrmaServiceDataModel<CrewFlightModel, POB_CrewFlight>();
                    model.ModelType = typeof(CrewFlightModel);
                    model.DisplayField = "Title";
                    model.MultiSelect = true;
                    break;
                case IrmaConstants.IrmaLookupLists.FlightManifestUnassigned:
                    dataModel = new IrmaServiceDataModel<CrewFlightModel, POB_FlightManifestUnassigned>();
                    model.ModelType = typeof(CrewFlightModel);
                    model.DisplayField = "Title";
                    model.MultiSelect = true;
                    break;
                case IrmaConstants.IrmaLookupLists.CrewPobAll:
                    dataModel = new IrmaServiceDataModel<CrewPobViewModel, POB_CrewPobView>();
                    model.ModelType = typeof(CrewPobViewModel);
                    model.DisplayField = "Name";
                    model.MultiSelect = true;
                    break;
                case IrmaConstants.IrmaLookupLists.CrewPobArriving:
                    dataModel = new IrmaServiceDataModel<CrewPobViewModel, POB_CrewPobView>();
                    model.ModelType = typeof(CrewPobViewModel);
                    model.KeyFieldName = "PassportId";
                    model.DisplayField = "Name";
                    model.MultiSelect = true;
                    filter = string.Format("StatusId=3"); // Arriving personnel only
                    break;
                case IrmaConstants.IrmaLookupLists.TeamType:
                    dataModel = new IrmaServiceDataModel<TeamTypeModel, POB_TeamType>();
                    model.ModelType = typeof(TeamTypeModel);
                    break;
                case IrmaConstants.IrmaLookupLists.TeamPersonnel:
                    dataModel = new IrmaServiceDataModel<PobLookupModel, Master_UserLookupView>();
                    model.ModelType = typeof(PobLookupModel);
                    filter = string.Format("Status=1 Or Status=3");
                    break;
                case IrmaConstants.IrmaLookupLists.PersonnelType:
                    dataModel = new IrmaServiceDataModel<PersonnelTypeModel, POB_PersonnelType>();
                    model.ModelType = typeof(PersonnelTypeModel);
                    break;
                case IrmaConstants.IrmaLookupLists.CrewFlights:
                    dataModel = new IrmaServiceDataModel<CrewFlightLookupModel, POB_CrewChangeFlights>();
                    model.ModelType = typeof(CrewFlightLookupModel);
                    model.DisplayField = "Manifest";
                    break;
                case IrmaConstants.IrmaLookupLists.RotationSchedule:
                    dataModel = new IrmaServiceDataModel<RotationScheduleModel, POB_RotationDays>();
                    model.ModelType = typeof(RotationScheduleModel);
                    break;
                case IrmaConstants.IrmaLookupLists.PobAdminRole:
                    dataModel = new IrmaServiceDataModel<AdminRoleModel, AdminRole>();
                    model.ModelType = typeof(AdminRole);
                    break;
                case IrmaConstants.IrmaLookupLists.IsolationType:
                    dataModel = null;// new IrmaServiceDataModel<IsolationTypeModel,   ListIAType>();
                    model.ModelType = typeof(IsolationTypeModel);
                    break;                
            }

            if (dataModel != null) {
                model.Items = (filter != null) ? dataModel.GetItems(filter, "Id") : dataModel.GetAllItems();
            }

            model.Initialize();

            return model;
        }

        private static List<dynamic> GetItemsFromEnum(Type type) {
            List<dynamic> list = new List<dynamic>();

            foreach (string name in Enum.GetNames(type)) {
                int value = (int)Enum.Parse(type, name, true);
                list.Add(new KeyValuePair<int, string>(value, name));
            }

            return list;
        }

        public static IEnumerable<RigPersonnelModel> GetPersonnelOnboard()
        {
            IIrmaServiceDataModel model = GetServiceModel(IrmaConstants.IrmaPobModels.RigPersonnel);            
            IEnumerable<RigPersonnelModel> personnel = model.GetItems(string.Format("Status=1"), "Id").Cast<RigPersonnelModel>();
            return personnel;
        }

        public static IEnumerable<RigPersonnelModel> GetOnboardedByDate(DateTime date)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<POB_RigPersonnel, RigPersonnelModel>());            

            RigPersonnelRepository repository = new RigPersonnelRepository();
            IEnumerable<RigPersonnelModel> rigPersonnel = 
                repository.Filter(rp => SqlFunctions.DatePart("dayofyear", rp.DateStart) == SqlFunctions.DatePart("dayofyear", date))
                .ProjectTo<RigPersonnelModel>(config).ToList();
            
            return rigPersonnel;
        }

        public static bool LogRigPersonnelHistory(int id, int pobId, bool onboard) {
            IIrmaServiceDataModel pobModel = GetServiceModel(IrmaConstants.IrmaPobModels.CrewFlightManifestPob);
            IIrmaServiceDataModel historyModel = GetServiceModel(IrmaConstants.IrmaPobModels.RigPersonnelHistory);
            IIrmaServiceDataModel historyViewModel = GetServiceModel(IrmaConstants.IrmaPobModels.RigPersonnelHistoryView);
            List<dynamic> items = historyViewModel.GetItems(string.Format("Id={0}", id), "Id");
            if (items != null && items.Count > 0) {
                CrewFlightPobModel pob = null;
                List<dynamic> pobs = (pobId != -1) ? pobModel.GetItems(string.Format("Id={0}", pobId), "Id") : null;
                if (pobs != null && pobs.Count() > 0) {
                    pob = pobs[0] as CrewFlightPobModel;
                }
                RigPersonnelHistoryModel historyItem = items[0] as RigPersonnelHistoryModel;
                historyItem.RigId = UtilitySystem.Settings.RigId;
                if (pob != null) {
                    historyItem.AirlineFlight = pob.AirlineFlight;
                    historyItem.HomeAirport = pob.HomeAirport;
                    historyItem.BodyWeight = pob.BodyWeight;
                    historyItem.BagWeight = pob.BagWeight;
                    historyItem.Bags = pob.Bags;
                    historyItem.Location = pob.Location;
                    historyItem.Terminal = pob.Terminal;
                }
                if (onboard) {
                    historyModel.Add(historyItem);
                } else {
                    RigPersonnelHistoryModel item = historyModel.GetItem(string.Format("Id={0} AND Status='Onboard'", id), "Id");
                    if (item != null) {
                        item.DateEnd = historyItem.DateEnd;
                        item.Status = historyItem.Status;
                        historyModel.Update(item);
                    }
                }
            }

            return true;
        }

        public static bool OnboardBatch(List<OnboardIndividualPobModel> models) {
            bool result = false;

            foreach (OnboardIndividualPobModel model in models) {
                if (!model.Selected)
                    continue;

                OnboardBatchIndividual(model);
            }

            return result;
        }

        public static OnboardIndividualPobModel OnboardBatchIndividual(OnboardIndividualPobModel model) {
            if (model == null)
                return null;

            IIrmaServiceDataModel dataModel = GetServiceModel(IrmaConstants.IrmaPobModels.RigPersonnel);
            RigPersonnelModel pobOnboard = dataModel.GetItem(string.Format("PassportId={0} AND Status=1", model.PassportId), "Id");

            UserModel user = ServiceSystem.GetUser((int)model.PassportId);

            int pobId = -1;
            if (pobOnboard == null) {
                RigPersonnelModel rigPob = new RigPersonnelModel();
                model.CopyProperties(rigPob);
                rigPob.PassportId = (int)model.PassportId;
                rigPob.Id = 0; // do not use  this id
                rigPob.CrewPobId = model.Id;
                rigPob.DateCreated = DateTime.Now;
                rigPob.DateModified = DateTime.Now;
                rigPob.DateStart = DateTime.Now;
                rigPob.Status = (int)RigPersonnelModel.PobStatus.Onboard;
                rigPob.ModifiedBy = UtilitySystem.CurrentUserId;
                rigPob = dataModel.Add(rigPob);
                pobId = rigPob.Id;
                LogRigPersonnelHistory(model.Id, rigPob.Id, true);

                // Add RigRequirements for Compliance            
                IIrmaServiceDataModel compModel = GetServiceModel(IrmaConstants.IrmaPobModels.RigCompliance);
                IIrmaServiceDataModel reqModel = GetServiceModel(IrmaConstants.IrmaPobModels.PobRigCompliance);
                List<dynamic> reqList = reqModel.GetAllItems();
                foreach (RigComplianceModel item in reqList) {
                    RigRequirementsComplianceModel comp = new RigRequirementsComplianceModel();
                    comp.RequirementId = item.Requirement;
                    comp.SubRequirementId = item.Type;
                    comp.Required = item.Required;
                    comp.EvidenceRequired = item.EvidenceRequired;
                    comp.PobId = pobId;
                    comp = compModel.Add(comp);
                }
            } else {
                model.CopyProperties(pobOnboard);
                dataModel.Update(pobOnboard);
                pobOnboard.DateModified = DateTime.Now;
                pobOnboard.ModifiedBy = UtilitySystem.CurrentUserId;
                pobId = pobOnboard.Id;
                LogRigPersonnelHistory(model.Id, pobOnboard.Id, true);
            }

            // Change the CrewFlightPob status to Onboard
            IIrmaServiceDataModel pobModel = GetServiceModel(IrmaConstants.IrmaPobModels.CrewFlightManifestPob);
            List<dynamic> pobs = pobModel.GetItems(string.Format("Id={0}", model.Id), "Id");
            if (pobs != null && pobs.Count() > 0) {
                CrewFlightPobModel pob = pobs[0] as CrewFlightPobModel;
                pob.Status = (int)RigPersonnelModel.PobStatus.Onboard;
                pobModel.Update(pob);
            }

            return model;
        }

        public static bool OffboardBatch(List<OffboardPobModel> persons) {
            if (persons == null || persons.Count <= 0)
                return false;

            List<OffboardPobModel> listOffboarded = new List<OffboardPobModel>();
            foreach (OffboardPobModel person in persons) {
                if (!person.Selected)
                    continue;

                bool result = OffboardPersonnel(person);
                if (result) {
                    listOffboarded.Add(person);
                }
            }

            foreach (OffboardPobModel item in listOffboarded) {
                persons.Remove(item);
            }

            return true;
        }
        public static bool OffboardPersonnel(OffboardPobModel model) {
            bool result = false;
            if (model == null)
                return false;

            IIrmaServiceDataModel dataModel = GetServiceModel(IrmaConstants.IrmaPobModels.RigPersonnel);
            RigPersonnelModel entityModel = GetRigPersonnel(model.Id);
            if (entityModel != null) {
                entityModel.Status = (int)RigPersonnelModel.PobStatus.Offboard;
                entityModel.DateEnd = DateTime.Now;
                result = dataModel.Update(entityModel);
                if (result) {
                    LogRigPersonnelHistory(entityModel.Id, entityModel.CrewPobId, false);
                }
            }

            // Change the CrewFlightPob status to Onboard
            IIrmaServiceDataModel pobModel = GetServiceModel(IrmaConstants.IrmaPobModels.CrewFlightManifestPob);
            List<dynamic> pobs = pobModel.GetItems(string.Format("Id={0}", model.CrewPobId), "Id");
            if (pobs != null && pobs.Count() > 0) {
                CrewFlightPobModel pob = pobs[0] as CrewFlightPobModel;
                pob.Status = (int)RigPersonnelModel.PobStatus.Offboard;
                pobModel.Update(pob);
            }

            return result;
        }

        public static List<OnboardIndividualPobModel> GetPersonnelForFlight(int flightId) {
            List<OnboardIndividualPobModel> list = new List<OnboardIndividualPobModel>();

            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.BatchOnboardView);
            List<dynamic> items = dataModel.GetItems(string.Format("CrewFlightId={0}", flightId), "Id");
            foreach (OnboardIndividualPobModel item in items) {
                OnboardIndividualPobModel model = GetPobForIndividualFromPersonnel(item.PassportId);
                if (model != null && model.Id != 0) {
                    list.Add(model);
                } else
                    list.Add(item);
            }

            return list;
        }

        public static OnboardIndividualPobModel GetPobForIndividualFromPassport(int passportId) {
            OnboardIndividualPobModel model = null;

            IIrmaServiceDataModel viewModel = GetServiceModel(IrmaConstants.IrmaPobModels.IndividualOnboardView);
            IQueryable queryable = viewModel.GetQueryable(string.Format("PassportId={0}", passportId), "Id desc");
            int count = (queryable != null) ? queryable.Count() : 0;
            if (count <= 0) {
                UserModel user = ServiceSystem.GetUser(passportId);

                model = new OnboardIndividualPobModel();
                user.CopyProperties(model);
                model.Id = 0; // do not map Passport table id to model id (rig personnel db)
                model.Status = null; // do not map Passport status to onboard status
                model.PassportId = user.Id;
                model.CompanyType = user.UserType;
                model.EnscoAuthentication = (bool)user.ADUser;
            } else {
                foreach (OnboardIndividualPobModel item in queryable) {
                    model = item;
                    if (item.Status != 1) {
                        model.Room = model.UsualRoom;
                        model.Bed = model.UsualBed;
                    }
                    break;
                }
            }

            // Get the rig requirements
            IIrmaServiceDataModel reqModel = GetServiceModel(IrmaConstants.IrmaPobModels.RigFieldVisible);
            List<dynamic> list = reqModel.GetAllItems();
            model.Requirements = list.Cast<RigFieldVisibilityModel>().ToList();

            return model;
        }

        public static OnboardIndividualPobModel GetPobForIndividualFromPersonnel(int Id) {
            OnboardIndividualPobModel model = new OnboardIndividualPobModel();

            IIrmaServiceDataModel viewModel = GetServiceModel(IrmaConstants.IrmaPobModels.IndividualOnboardView);
            IQueryable queryable = viewModel.GetQueryable(string.Format("PassportId={0}", Id), "Id desc");

            foreach (OnboardIndividualPobModel item in queryable) {
                item.CopyProperties(model);

                UserModel user = ServiceSystem.GetUser(Id);
                user.CopyProperties(model);
                model.PassportId = user.Id;
                model.Id = item.Id; // do not map Passport table id to model id (rig personnel db)
                model.Status = item.Status; // do not map Passport status to onboard status
                model.CompanyType = user.UserType;
                model.EnscoAuthentication = (bool)user.ADUser;
                if (item.Status != 1) {
                    model.Room = model.UsualRoom;
                    model.Bed = model.UsualBed;
                }
                break;
            }

            // Get the rig requirements
            IIrmaServiceDataModel reqModel = GetServiceModel(IrmaConstants.IrmaPobModels.RigFieldVisible);
            List<dynamic> list = reqModel.GetAllItems();
            model.Requirements = list.Cast<RigFieldVisibilityModel>().ToList();

            return model;
        }


        public static OnboardIndividualPobModel OnboardIndividual(OnboardIndividualPobModel model) {
            if (model == null)
                return null;

            RigPersonnelModel rigPob = null;
            IIrmaServiceDataModel dataModel = GetServiceModel(IrmaConstants.IrmaPobModels.RigPersonnel);
            RigPersonnelModel pobOnboard = dataModel.GetItem(string.Format("PassportId={0} AND Status=1", model.PassportId), "Id");
            UserModel user = ServiceSystem.GetUser(model.PassportId);
            if (pobOnboard == null) {
                rigPob = new RigPersonnelModel();
                model.CopyProperties(rigPob);
                rigPob.Status = (int)RigPersonnelModel.PobStatus.Onboard;
                rigPob.DateCreated = DateTime.Now;
                rigPob.DateModified = DateTime.Now;
                rigPob.DateStart = DateTime.Now;
                rigPob.ModifiedBy = UtilitySystem.CurrentUserId;
                rigPob = dataModel.Add(rigPob);
                model.Id = rigPob.Id;
                LogRigPersonnelHistory(model.Id, -1, true);
            } else {
                rigPob = new RigPersonnelModel();
                model.CopyProperties(rigPob);
                rigPob.Status = (int)RigPersonnelModel.PobStatus.Onboard;
                rigPob.DateStart = model.DateOfArrival;
                rigPob.DateModified = DateTime.Now;
                rigPob.ModifiedBy = UtilitySystem.CurrentUserId;
                dataModel.Update(rigPob);
                LogRigPersonnelHistory(model.Id, -1, true);
            }

            // Update passsport fields
            try {
                IServiceDataModel userDataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.User);
                user.AddressLine1 = model.AddressLine1;
                user.AddressLine2 = model.AddressLine2;
                user.City = model.City;
                user.State = model.State;
                user.Country = model.Country;
                user.PostalCode = model.PostalCode;
                user.Email = model.Email;
                user.EmergencyContactComment = model.EmergencyContactComment;
                user.EmergencyContactEmail = model.EmergencyContactEmail;
                user.EmergencyContactFirstName = model.EmergencyContactFirstName;
                user.EmergencyContactLastName = model.EmergencyContactLastName;
                user.EmergencyContactPrimaryPhone = model.EmergencyContactPrimaryPhone;
                user.EmergencyContactRelationship = model.EmergencyContactRelationship;
                user.EmergencyContactSecondaryPhone = model.EmergencyContactSecondaryPhone;
                user.MaritalStatus = model.MaritalStatus;
                user.Gender = model.Gender;
                user.PersonalEmail = model.PersonalEmail;
                user.UserType = (int)model.CompanyType;
                user.ContactInfoComment = model.ContactInfoComment;
                user.SecondaryPhone = model.SecondaryPhone;
                if ((bool)user.ADUser && !model.EnscoAuthentication) {
                    string password = "123";
                    user.RequirePasswordChange = true;
                    user.Password = Cryptography.Encrypt(model.Passport, password);
                }
                user.ADUser = model.EnscoAuthentication;
                userDataModel.Update(user);
            } catch (Exception ex) {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));
            }

            model.Status = (int)RigPersonnelModel.PobStatus.Onboard;

            // Add RigRequirements for Compliance            
            IIrmaServiceDataModel compModel = GetServiceModel(IrmaConstants.IrmaPobModels.RigCompliance);
            IIrmaServiceDataModel reqModel = GetServiceModel(IrmaConstants.IrmaPobModels.PobRigCompliance);
            List<dynamic> reqList = reqModel.GetAllItems();
            foreach (RigComplianceModel item in reqList) {
                RigRequirementsComplianceModel comp = new RigRequirementsComplianceModel();
                comp.RequirementId = item.Requirement;
                comp.SubRequirementId = item.Type;
                comp.Required = item.Required;
                comp.EvidenceRequired = item.EvidenceRequired;
                comp.PobId = rigPob.Id;
                comp = compModel.Add(comp);
            }

            return model;
        }

        public static RigPersonnelModel GetRigPersonnel(int id) {
            RigPersonnelModel model = null;

            IIrmaServiceDataModel dataModel = GetServiceModel(IrmaConstants.IrmaPobModels.RigPersonnel);
            List<dynamic> items = dataModel.GetItems(string.Format("Id={0}", id), "Id");
            foreach (RigPersonnelModel item in items) {
                model = item;
                break;
            }

            return model;
        }

        public static RigPersonnelModel GetRigPersonnelFromPassportId(int passportId) {
            RigPersonnelModel model = null;

            IIrmaServiceDataModel dataModel = GetServiceModel(IrmaConstants.IrmaPobModels.RigPersonnel);
            List<dynamic> items = dataModel.GetItems(string.Format("PassportId={0}", passportId), "Id");
            foreach (RigPersonnelModel item in items) {
                model = item;
                break;
            }

            return model;
        }

        public static List<TeamMemberModel> GetTeamMembers(int teamId) {
            List<TeamMemberModel> members = new List<TeamMemberModel>();

            IIrmaServiceDataModel memberDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.TeamMemberView);
            if (memberDataModel != null) {
                List<dynamic> items = memberDataModel.GetItems(string.Format("TeamId={0}", teamId), "Id");
                members = items.Cast<TeamMemberModel>().ToList();
            }

            return members;
        }

        public static TeamMemberModel GetTeamMemberFromPassport(int passportId) {
            TeamMemberModel model = new TeamMemberModel();
            model.Passport = passportId;
            RigPersonnelModel rigUser = IrmaServiceSystem.GetRigPersonnelFromPassportId(passportId);
            if (rigUser != null) {
                model.Company = (int)rigUser.Company;
                model.Position = (int)rigUser.Position;
                model.OnboardNow = (rigUser.Status == 1) ? true : false;
            } else {
                UserModel user = ServiceSystem.GetUser(passportId);
                model.Company = (int)user.Company;
                model.Position = (int)user.Position;
                model.OnboardNow = false;
            }

            return model;
        }
        public static Type GetModelType(string type) {
            try {
                string[] arr = type.Split('.');
                string name = arr[arr.Length - 1];
                String fullName = type.Replace("." + name, "");
                var an = new AssemblyName(fullName);
                var assem = Assembly.Load(an);
                System.Type modelType = assem.GetType(type);
                return modelType;
            } catch (Exception e) {

            }
            return null;
        }
        public static object GetServiceListModel(string mType, string dType) {
            System.Type modelType = GetModelType(mType);
            System.Type dataType = GetModelType(dType); // typeof(Master_BusinessUnit).Assembly.GetType(dType);// "Ensco.Irma.Data.Master_" + type);

            Type objectType = typeof(IrmaServiceDataModel<,>);
            var genericType = objectType.MakeGenericType(modelType, dataType);
            var instance = Activator.CreateInstance(genericType);

            return instance;
        }
        public static IIrmaServiceDataModel GetServiceModel(IrmaConstants.IrmaPobModels modelType) {
            IIrmaServiceDataModel service = null;

            switch (modelType) {
                case IrmaConstants.IrmaPobModels.Resources:
                   // service = new IrmaServiceDataModel<ResourceEntry, Master_Localizations>();
                    break;
                case IrmaConstants.IrmaPobModels.CrewChange:
                    service = new IrmaServiceDataModel<CrewChangeModel, POB_CrewChange>();
                    break;
                case IrmaConstants.IrmaPobModels.CrewFlightManifest:
                    service = new IrmaServiceDataModel<CrewFlightModel, POB_CrewFlight>();
                    break;
                case IrmaConstants.IrmaPobModels.CrewFlightManifestView:
                    service = new IrmaServiceDataModel<CrewFlightModel, POB_CrewFlightManifestView>();
                    break;
                case IrmaConstants.IrmaPobModels.FlightManifestUnassigned:
                    service = new IrmaServiceDataModel<CrewFlightModel, POB_FlightManifestUnassigned>();
                    break;
                case IrmaConstants.IrmaPobModels.CrewFlightManifestPob:
                    service = new IrmaServiceDataModel<CrewFlightPobModel, POB_CrewFlightPob>();
                    break;
                case IrmaConstants.IrmaPobModels.RigPersonnel:
                    service = new IrmaServiceDataModel<RigPersonnelModel, POB_RigPersonnel>();
                    break;
                case IrmaConstants.IrmaPobModels.RigPersonnelTeams:
                    break;
                case IrmaConstants.IrmaPobModels.BatchOnboardView:
                    service = new IrmaServiceDataModel<OnboardIndividualPobModel, POB_BatchOnboardView>();
                    break;
                case IrmaConstants.IrmaPobModels.BatchOffboardView:
                    service = new IrmaServiceDataModel<OffboardPobModel, POB_BatchOffboardView>();
                    break;
                //case IrmaConstants.IrmaPobModels.FlightPobView:
                //    service = new IrmaServiceDataModel<OnboardBatchModel, POB_FlightPobView>();
                //    break;
                case IrmaConstants.IrmaPobModels.FlightManifestPobView:
                    service = new IrmaServiceDataModel<FlightManifestPobModel, POB_FlightPobView>();
                    break;
                case IrmaConstants.IrmaPobModels.ParkingLot:
                    service = new IrmaServiceDataModel<ParkingLotModel, POB_FlightParkingLot>();
                    break;
                case IrmaConstants.IrmaPobModels.FlightParkingLotView:
                    service = new IrmaServiceDataModel<FlightManifestPobModel, POB_FlightParkingLotView>();
                    break;
                case IrmaConstants.IrmaPobModels.CurrentPobView:
                    service = new IrmaServiceDataModel<PobCurrentModel, POB_CurrentPobView>();
                    break;
                case IrmaConstants.IrmaPobModels.IndividualOnboardView:
                    service = new IrmaServiceDataModel<OnboardIndividualPobModel, POB_IndividualOnboardView>();
                    break;
                case IrmaConstants.IrmaPobModels.PobSummaryView:
                    service = new IrmaServiceDataModel<PobSummaryDataModel, POB_SummaryView>();
                    break;
                case IrmaConstants.IrmaPobModels.PobSummaryByCompanyType:
                    service = new IrmaServiceDataModel<PobSummaryByCompanyTypeModel, POB_SummaryByCompanyTypeView>();
                    break;
                case IrmaConstants.IrmaPobModels.Teams:
                    service = new IrmaServiceDataModel<TeamModel, POB_EmergencyTeam>();
                    break;
                case IrmaConstants.IrmaPobModels.TeamMembers:
                    service = new IrmaServiceDataModel<TeamMemberModel, POB_EmergencyTeamMembers>();
                    break;
                case IrmaConstants.IrmaPobModels.TeamView:
                    service = new IrmaServiceDataModel<TeamModel, POB_TeamView>();
                    break;
                case IrmaConstants.IrmaPobModels.TeamMemberView:
                    service = new IrmaServiceDataModel<TeamMemberModel, POB_TeamMemberView>();
                    break;
                case IrmaConstants.IrmaPobModels.PobRigCompliance:
                    service = new IrmaServiceDataModel<RigComplianceModel, POB_RigComplianceView>();
                    break;
                case IrmaConstants.IrmaPobModels.PobRigRequirements:
                    service = new IrmaServiceDataModel<RigRequirementsModel, POB_RigRequirements>();
                    break;
                case IrmaConstants.IrmaPobModels.PobRigSubRequirements:
                    service = new IrmaServiceDataModel<RigSubRequirementsModel, POB_RigSubRequirement>();
                    break;
                case IrmaConstants.IrmaPobModels.RigFieldVisible:
                    service = new IrmaServiceDataModel<RigFieldVisibilityModel, POB_RigFieldVisibility>();
                    break;
                //case IrmaConstants.IrmaPobModels.PobRigRequirementDocs:
                //    service = new IrmaServiceDataModel<RigRequirementDocsModel, POB_RigRequirementDocuments>();
                //    break;
                case IrmaConstants.IrmaPobModels.CrewArrivalDepartureLog:
                    service = new IrmaServiceDataModel<PobArrivalDepartureLogModel, POB_CrewArrivalDepartureLogView>();
                    break;
                case IrmaConstants.IrmaPobModels.CrewArrivalLog:
                    service = new IrmaServiceDataModel<PobArrivalLogModel, POB_CrewArrivalDepartureLogView>();
                    break;
                case IrmaConstants.IrmaPobModels.CrewDepartureLog:
                    service = new IrmaServiceDataModel<PobDepartureLogModel, POB_CrewArrivalDepartureLogView>();
                    break;
                case IrmaConstants.IrmaPobModels.PobSummaryArrivalReport:
                    service = new IrmaServiceDataModel<ArrivalLogReportModel, POB_CrewArrivalDepartureLogView>();
                    break;
                case IrmaConstants.IrmaPobModels.PobSummaryDepartureReport:
                    service = new IrmaServiceDataModel<DepartureLogReportModel, POB_CrewArrivalDepartureLogView>();
                    break;
                case IrmaConstants.IrmaPobModels.FlightArrivalDepartureLog:
                    service = new IrmaServiceDataModel<PobArrivalDepartureLogModel, POB_FlightArrivalDepartureLogView>();
                    break;
                case IrmaConstants.IrmaPobModels.FlightArrivalLog:
                    service = new IrmaServiceDataModel<PobArrivalLogModel, POB_FlightArrivalDepartureLogView>();
                    break;
                case IrmaConstants.IrmaPobModels.FlightDepartureLog:
                    service = new IrmaServiceDataModel<PobDepartureLogModel, POB_FlightArrivalDepartureLogView>();
                    break;
                case IrmaConstants.IrmaPobModels.PobOnOffboardView:
                    service = new IrmaServiceDataModel<PersonnelOnOffBoardModel, POB_OnOffBoardReportView>();
                    break;
                case IrmaConstants.IrmaPobModels.LifeBoatCompliance:
                    service = new IrmaServiceDataModel<LifeBoatComplianceReportModel, POB_LifeBoatComplianceView>();
                    break;
                case IrmaConstants.IrmaPobModels.LifeBoatRosterList:
                    service = new IrmaServiceDataModel<LifeBoatRosterListModel, POB_LifeBoatRosterListView>();
                    break;
                case IrmaConstants.IrmaPobModels.RoomBedSummary:
                    service = new IrmaServiceDataModel<RoomBedSummaryModel, POB_RoomBedView>();
                    break;
                case IrmaConstants.IrmaPobModels.CrewBreakDown:
                    service = new IrmaServiceDataModel<PobCrewBreakdownModel, POB_CrewBreakdownView>();
                    break;
                case IrmaConstants.IrmaPobModels.RosterByLifeBoat:
                    service = new IrmaServiceDataModel<RosterUserModel, POB_RosterByLifeBoatView>();
                    break;
                case IrmaConstants.IrmaPobModels.RosterByMusterStation:
                    service = new IrmaServiceDataModel<RosterUserModel, POB_RosterByMusterStationVIew>();
                    break;
                case IrmaConstants.IrmaPobModels.RosterFull:
                    service = new IrmaServiceDataModel<RosterUserModel, POB_RosterSignInView>();
                    break;
                case IrmaConstants.IrmaPobModels.Approval:
                    service = new IrmaServiceDataModel<ApprovalModel, Master_Approvals>();
                    break;
                case IrmaConstants.IrmaPobModels.ApprovalView:
                    service = new IrmaServiceDataModel<ApprovalModel, Master_ApprovalVew>();
                    break;
                case IrmaConstants.IrmaPobModels.CrewChangeApprovalView:
                    service = new IrmaServiceDataModel<CrewChangeApproverModel, POB_CrewChangeApproverView>();
                    break;
                case IrmaConstants.IrmaPobModels.RigPersonnelHistory:
                    service = new IrmaServiceDataModel<RigPersonnelHistoryModel, POB_RigPersonnelHistory>();
                    break;
                case IrmaConstants.IrmaPobModels.RigPersonnelHistoryView:
                    service = new IrmaServiceDataModel<RigPersonnelHistoryModel, POB_RigPersonnelHistoryView>();
                    break;
                case IrmaConstants.IrmaPobModels.RigPersonnelArchive:
                    service = new IrmaServiceDataModel<RigPersonnelArchiveModel, vw_POB_RigPersonnel > ();
                    break;
                case IrmaConstants.IrmaPobModels.CrewPobAll:
                    service = new IrmaServiceDataModel<CrewPobViewModel, POB_CrewPobView>();
                    break;
                case IrmaConstants.IrmaPobModels.FlightPobAll:
                    service = new IrmaServiceDataModel<CrewFlightPobModel, POB_FlightPobView>();
                    break;
                case IrmaConstants.IrmaPobModels.TourMangement:
                    service = new IrmaServiceDataModel<TourPobModel, POB_TourMangementView>();
                    break;
                case IrmaConstants.IrmaPobModels.TourChange:
                    service = new IrmaServiceDataModel<TourChangeModel, POB_TourChange>();
                    break;
                case IrmaConstants.IrmaPobModels.EmergencyTeamReportView:
                    service = new IrmaServiceDataModel<EmergencyTeamReportModel, POB_EmergencyTeamReportView>();
                    break;
                case IrmaConstants.IrmaPobModels.RigComplianceUsers:
                    service = new IrmaServiceDataModel<RigComplianceUserModel, POB_RigComplianceUserView>();
                    break;
                case IrmaConstants.IrmaPobModels.RigCompliance:
                    service = new IrmaServiceDataModel<RigRequirementsComplianceModel, POB_RigRequirementCompliance>();
                    break;
                case IrmaConstants.IrmaPobModels.CrewChangeSearch:
                    service = new IrmaServiceDataModel<CrewChangeSearchModel, POB_CrewSearchView>();
                    break;
                case IrmaConstants.IrmaPobModels.PobSummaryCounts:
                    service = new IrmaServiceDataModel<PobSummaryCountsModel, POB_SummaryOtherView>();
                    break;
                case IrmaConstants.IrmaPobModels.Emails:
                    service = new IrmaServiceDataModel<PobEmailModel, POB_Emails>();
                    break;
                case IrmaConstants.IrmaPobModels.PobHistory:
                    service = new IrmaServiceDataModel<PobHistoryModel, POB_History>();
                    break;
                case IrmaConstants.IrmaPobModels.Admin:
                    service = new IrmaServiceDataModel<AdminModel, POB_Admin>();
                    break;
                case IrmaConstants.IrmaPobModels.AdminView:
                    service = new IrmaServiceDataModel<AdminModel, POB_AdminView>();
                    break;
                case IrmaConstants.IrmaPobModels.AdminRole:
                    service = new IrmaServiceDataModel<AdminRoleModel, AdminRole>();
                    break;
                case IrmaConstants.IrmaPobModels.IsolationLock:
                    service = new IrmaServiceDataModel<LockModel, Lock>();
                    break;
                case IrmaConstants.IrmaPobModels.TasksCapa:
                    service = new IrmaServiceDataModel<IrmaHomeTasksCapaModel, vw_Capa>();
                    break;
                case IrmaConstants.IrmaPobModels.TasksIsolation:
                    service = new IrmaServiceDataModel<IrmaHomeTasksIsolationModel, vw_EiIsolation>();
                    break;
                case IrmaConstants.IrmaPobModels.TasksTask:
                    service = new IrmaServiceDataModel<IrmaHomeTasksTaskModel, vw_Task>();
                    break;
                case IrmaConstants.IrmaPobModels.TasksJob:
                    service = new IrmaServiceDataModel<IrmaHomeTasksJobModel, vw_JobLookup>();
                    break;
                case IrmaConstants.IrmaPobModels.TasksCap:
                    service = new IrmaServiceDataModel< IrmaHomeTasksCapModel, vw_TaskCap>();
                    break;
                case IrmaConstants.IrmaPobModels.TasksTlc:
                    service = new IrmaServiceDataModel<IrmaHomeTasksTlcModel, vw_TaskTlc >();
                    break;
                case IrmaConstants.IrmaPobModels.ReconcilePassport:
                    service = new IrmaServiceDataModel<ReconcileModel, Master_PassportReconciliationView>();
                    break;
                case IrmaConstants.IrmaPobModels.JobCode:
                    service = new IrmaServiceDataModel<Models.Admin.JobCodeModel, Master_JobCode>();
                    break;
            }

            return service;
        }

        public static IQueryable GetQueryable(IrmaConstants.IrmaPobModels entityId, string sortColumn = "Id") {
            IIrmaServiceDataModel dataModel = GetServiceModel(entityId);
            if (dataModel != null) {
                return dataModel.GetQueryable(sortColumn);
            }

            return null;
        }

        public static IQueryable GetQueryable(IrmaConstants.IrmaPobModels entityId, string search, string sortColumn = "Id") {
            IIrmaServiceDataModel dataModel = GetServiceModel(entityId);
            if (dataModel != null) {
                return dataModel.GetQueryable(search, sortColumn);
            }

            return null;
        }

        public static bool Save(IrmaConstants.IrmaPobModels entityId, dynamic item, bool commit) {
            bool result = false;
            try {
                IIrmaServiceDataModel model = GetServiceModel(entityId);
                if (model != null) {
                    result = model.Update(item, commit);
                    if (result) {
                        LogChangeHistory(entityId, "Modified", GetOldEntityModel(entityId, item), item);
                    }
                }
            } catch (Exception ex) {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));

                result = false;
            }

            return result;
        }

        public static dynamic Add(IrmaConstants.IrmaPobModels entityId, dynamic item, bool commit) {
            try {
                IIrmaServiceDataModel model = GetServiceModel(entityId);
                if (model != null) {
                    item = model.Add(item, commit);
                    if (item != null) {
                        LogChangeHistory(entityId, "Added", null, item);
                    }
                }
            } catch (Exception ex) {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));

                return null;
            }

            return item;
        }

        public static dynamic Update(IrmaConstants.IrmaPobModels entityId, dynamic item, bool commit) {
            try {
                IIrmaServiceDataModel model = GetServiceModel(entityId);
                if (model != null) {
                    item = model.Update(item, commit);
                    if (item != null) {
                        LogChangeHistory(entityId, "Modified", null, item);
                    }
                }
            } catch (Exception ex) {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));

                return null;
            }

            return item;
        }

        public static bool Delete(IrmaConstants.IrmaPobModels entityId, dynamic item, bool commit) {
            bool result = false;
            try {
                IIrmaServiceDataModel model = GetServiceModel(entityId);
                if (model != null) {
                    result = model.Delete(item, commit);
                    if (result) {
                        LogChangeHistory(entityId, "Deleted", item, null);
                    }
                }
            } catch (Exception ex) {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));

                return false;
            }

            return result;
        }

        public static dynamic GetOldEntityModel(IrmaConstants.IrmaPobModels entityId, dynamic item) {
            object olditem = null;

            IIrmaServiceDataModel model = GetServiceModel(entityId);
            if (model != null) {
                Type type = item.GetType();
                olditem = Activator.CreateInstance(type);
                UtilitySystem.CopyProperties(item, olditem);

                olditem = model.Refresh(olditem);
            }

            return olditem;
        }

        public static void LogChangeHistory(IrmaConstants.IrmaPobModels entityId, string changetype, dynamic olditem, dynamic item) {
            try {
                if (olditem == null && item == null)
                    return;

                string name = (olditem != null) ? olditem.GetType().Name : "";
                if (name == "")
                    name = (item != null) ? item.GetType().Name : "";

                IIrmaServiceDataModel model = GetServiceModel(entityId);
                if (model != null) {
                    ChangeHistoryModel historyModel = new ChangeHistoryModel();
                    historyModel.RigId = UtilitySystem.Settings.RigId;
                    historyModel.Module = name;
                    historyModel.Type = changetype;
                    historyModel.OldValues = UtilitySystem.GetObjectAsXml(olditem);
                    historyModel.NewValues = UtilitySystem.GetObjectAsXml(item);
                    historyModel.CreatedDate = DateTime.Now;
                    historyModel.CreatedBy = UtilitySystem.CurrentUserId;

                    ServiceSystem.Add(EnscoConstants.EntityModel.History, historyModel, true);
                }
            } catch (Exception ex) {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));
            }
        }

        static object RunStoredProcedure(string name, List<KeyValuePair<string, object>> parameters) {
            object result = null;

            try {
                using (EnscoIrmaEntities entities = new EnscoIrmaEntities()) {
                    switch (name) {
                        case "GetAvailableBeds": {
                                int rigid = (int)parameters.Find(x => x.Key == "rigid").Value;
                                ObjectParameter returnValue = new ObjectParameter("ReturnValue", 0);
                                entities.GetAvailableBeds(rigid, returnValue);
                                result = returnValue.Value;
                            }
                            break;
                        case "GetAdminCustomValue": {
                                string fieldname = (string)parameters.Find(x => x.Key == "Name").Value;
                                ObjectParameter returnValue = new ObjectParameter("ReturnValue", "");

                                entities.GetAdminCustomValue(fieldname, returnValue);
                                result = returnValue.Value;
                            }
                            break;
                        case "UpdateAdminCustomValue": {
                                string fieldname = (string)parameters.Find(x => x.Key == "Name").Value;
                                string value = (string)parameters.Find(x => x.Key == "Value").Value;

                                entities.UpdateAdminCustomValue(fieldname, value);
                            }
                            break;
                    }
                }
            } catch (Exception ex) {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));
                return null;
            }

            return result;
        }

        static IIrmaServiceDataModel GetLookupDataModel(IrmaConstants.IrmaLookupLists type) {
            IIrmaServiceDataModel dataModel = null;
            switch (type) {
                case IrmaConstants.IrmaLookupLists.PobStatus:
                    dataModel = new IrmaServiceDataModel<PobStatusModel, POB_PobStatus>();
                    break;
                case IrmaConstants.IrmaLookupLists.PobPlanningStatus:
                    dataModel = new IrmaServiceDataModel<PobStatusModel, POB_PobStatus>();
                    break;
                case IrmaConstants.IrmaLookupLists.LifeBoat:
                    dataModel = new IrmaServiceDataModel<LifeBoatModel, POB_LifeBoat>();
                    break;
                case IrmaConstants.IrmaLookupLists.MusterStation:
                    dataModel = new IrmaServiceDataModel<MusterStationModel, POB_MusterStation>();
                    break;
                case IrmaConstants.IrmaLookupLists.Room:
                    dataModel = new IrmaServiceDataModel<RoomModel, POB_Room>();
                    break;
                case IrmaConstants.IrmaLookupLists.RoomBed:
                    dataModel = new IrmaServiceDataModel<RoomBedModel, POB_RoomBed>();
                    break;
                case IrmaConstants.IrmaLookupLists.Tour:
                    dataModel = new IrmaServiceDataModel<TourModel, POB_Tour>();
                    break;
                case IrmaConstants.IrmaLookupLists.RigCrew:
                    dataModel = new IrmaServiceDataModel<RigCrewModel, POB_RigCrew>();
                    break;
                case IrmaConstants.IrmaLookupLists.ScheduleType:
                    dataModel = new IrmaServiceDataModel<ScheduleTypeModel, POB_ScheduleType>();
                    break;
                case IrmaConstants.IrmaLookupLists.CrewChange:
                    dataModel = new IrmaServiceDataModel<CrewChangeModel, POB_CrewChange>();
                    break;
                case IrmaConstants.IrmaLookupLists.FlightManifest:
                    dataModel = new IrmaServiceDataModel<CrewFlightModel, POB_CrewFlight>();
                    break;
                case IrmaConstants.IrmaLookupLists.FlightManifestUnassigned:
                    dataModel = new IrmaServiceDataModel<CrewFlightModel, POB_FlightManifestUnassigned>();
                    break;
                case IrmaConstants.IrmaLookupLists.CrewPobAll:
                    dataModel = new IrmaServiceDataModel<CrewPobViewModel, POB_CrewPobView>();
                    break;
                case IrmaConstants.IrmaLookupLists.TeamType:
                    dataModel = new IrmaServiceDataModel<TeamTypeModel, POB_TeamType>();
                    break;
            }

            return dataModel;
        }
        public static dynamic LookupAdd(IrmaConstants.IrmaLookupLists entityId, dynamic item, bool commit) {
            try {
                IIrmaServiceDataModel model = GetLookupDataModel(entityId);
                if (model != null) {
                    item = model.Add(item, commit);
                    if (item != null) {
                        //LogChangeHistory(EnscoConstants.EntityModel.History, "Added", null, item);
                    }
                    LookupListModel<dynamic> lkpList = UtilitySystem.GetLookupList(entityId.ToString());
                    lkpList.Items.Add(item);
                }
            } catch (Exception ex) {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));

                return null;
            }

            return item;
        }

        public static dynamic LookupUpdate(IrmaConstants.IrmaLookupLists entityId, dynamic item, bool commit) {
            try {
                IIrmaServiceDataModel model = GetLookupDataModel(entityId);
                if (model != null) {
                    item = model.Update(item, commit);
                    if (item != null) {
                        //LogChangeHistory(EnscoConstants.EntityModel.History, "Added", null, item);
                    }
                }
            } catch (Exception ex) {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));

                return null;
            }

            return item;
        }

        public static bool LookupDelete(IrmaConstants.IrmaLookupLists entityId, dynamic item, bool commit) {
            bool result = false;
            try {
                IIrmaServiceDataModel model = GetLookupDataModel(entityId);
                if (model != null) {
                    result = model.Delete(item, commit);
                    if (result) {
                        //LogChangeHistory(EnscoConstants.EntityModel.History, "Added", item, null);
                    }
                    LookupListModel<dynamic> lkpList = UtilitySystem.GetLookupList(entityId.ToString());
                    lkpList.Items.Remove(item);
                }
            } catch (Exception ex) {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));

                return false;
            }

            return false;
        }

        public static IrmaHomeModel GetLandingPageInfo(int userId) {
            IrmaHomeModel model = new IrmaHomeModel();
            model.Id = userId;

            UserModel user = ServiceSystem.GetUser(userId);
            if (user != null) {
                model.UserInfo.BusinessUnit = "";
                model.UserInfo.City = user.City;

                LookupListModel<dynamic> lkp = (LookupListModel<dynamic>)UtilitySystem.GetLookupList(Ensco.Utilities.UtilitySystem.EnscoLookupList.Company.ToString());
                model.UserInfo.Company = (user.Company != null) ? (string)lkp.GetDisplayValue(user.Company) : "";

                lkp = (LookupListModel<dynamic>)UtilitySystem.GetLookupList(Ensco.Utilities.UtilitySystem.EnscoLookupList.Nationality.ToString());
                model.UserInfo.Country = (user.Country != null) ? (string)lkp.GetDisplayValue(user.Country) : "";
                model.UserInfo.Nationality = (user.Nationality != null) ? (string)lkp.GetDisplayValue(user.Nationality) : "";

                lkp = (LookupListModel<dynamic>)UtilitySystem.GetLookupList(Ensco.Utilities.UtilitySystem.EnscoLookupList.Position.ToString());
                model.UserInfo.Position = (user.Position != null) ? (string)lkp.GetDisplayValue(user.Position) : "";

                lkp = (LookupListModel<dynamic>)UtilitySystem.GetLookupList(Ensco.Utilities.UtilitySystem.EnscoLookupList.UserStatus.ToString());
                model.UserInfo.Status = (user.Status != null) ? (string)lkp.GetDisplayValue(user.Status) : "";

                model.UserInfo.Department = "";
                model.UserInfo.EmailAddress = user.Email;
                model.UserInfo.EmployeeId = user.Passport;
                model.UserInfo.FirstName = user.FirstName;
                model.UserInfo.LastName = user.LastName;
                model.UserInfo.MiddleName = user.MiddleName;
                model.UserInfo.PhoneNumber = user.PrimaryPhone;
                model.UserInfo.Rig = UtilitySystem.Settings.RigName;
                model.UserInfo.RigManager = "";
                model.UserInfo.State = user.State;
                if (user.Manager != null) {
                    UserModel managerModel = ServiceSystem.GetUser((int)user.Manager);
                    if (managerModel != null) {
                        model.UserInfo.Supervisor = managerModel.DisplayName;
                    }
                }
            }

            // Get PoB History
            model.PobHistory = new DataTableModel(1, GetQueryable(IrmaConstants.IrmaPobModels.PobHistory, string.Format("PassportId={0}", userId), "id"));
            model.TasksCapa = new DataTableModel(1, GetQueryable(IrmaConstants.IrmaPobModels.TasksCapa, string.Format("assigneeUserId='{0}' or AssignorUserId='{1}'", user.Passport, user.Passport), "id"));
            model.TasksJob = new DataTableModel(1, GetQueryable(IrmaConstants.IrmaPobModels.TasksJob, string.Format("userId='{0}'", user.Passport), "id"));
            model.TasksTask = new DataTableModel(1, GetQueryable(IrmaConstants.IrmaPobModels.TasksTask, string.Format("UserId='{0}'", user.Passport), "id"));
            model.TasksIsolation = new DataTableModel(1, GetQueryable(IrmaConstants.IrmaPobModels.TasksIsolation, /*string.Format("UserId='{0}'", user.Passport),*/ "id"));

            model.TasksCap = new DataTableModel(1, GetQueryable(IrmaConstants.IrmaPobModels.TasksCap, string.Format("UserId='{0}'", user.Passport), "id"));
            model.TasksTlc = new DataTableModel(1, GetQueryable(IrmaConstants.IrmaPobModels.TasksTlc, string.Format("UserId='{0}'", user.Passport), "id"));
            return model;
        }

        public static bool ScheduleCrewChanges(ScheduleCrewChangeModel schedule) {
            try {
                DateTime nextDate = (DateTime)schedule.DateStart;
                IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.CrewChange);
                do {
                    CrewChangeModel crewChange = new CrewChangeModel();
                    crewChange.DateCreated = DateTime.Now;
                    crewChange.DateCrewChange = nextDate;
                    crewChange.InboundCrew = (int)schedule.Crew;
                    crewChange.Status = (int)CrewChangeModel.ActionStatus.Open;
                    crewChange.Title = string.Format("{0}_{1}", schedule.Title, nextDate.ToString("ddMMMyyyy"));
                    crewChange.RigId = UtilitySystem.Settings.RigId;
                    dataModel.Add(crewChange);
                } while ((nextDate = GetNextDate((ScheduleCrewChangeModel.ScheduleTypeEnum)schedule.ScheduleType, nextDate, (DateTime)schedule.DateEnd)) <= schedule.DateEnd);
            } catch (Exception ex) {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));
            }
            return true;
        }

        private static DateTime GetNextDate(ScheduleCrewChangeModel.ScheduleTypeEnum type, DateTime curDate, DateTime endDate) {
            DateTime nextDate = endDate;

            switch (type) {
                case ScheduleCrewChangeModel.ScheduleTypeEnum.Daily:
                    nextDate = curDate.AddDays(1.0);
                    break;
                case ScheduleCrewChangeModel.ScheduleTypeEnum.Weekly:
                    nextDate = curDate.AddDays(7.0);
                    break;
                case ScheduleCrewChangeModel.ScheduleTypeEnum.BiWeekly:
                    nextDate = curDate.AddDays(14.0);
                    break;
                case ScheduleCrewChangeModel.ScheduleTypeEnum.EveryThreeWeeks:
                    nextDate = curDate.AddDays(21.0);
                    break;
                case ScheduleCrewChangeModel.ScheduleTypeEnum.Monthly:
                    nextDate = curDate.AddMonths(1);
                    break;
                case ScheduleCrewChangeModel.ScheduleTypeEnum.Quarterly:
                    nextDate = curDate.AddMonths(3);
                    break;
                case ScheduleCrewChangeModel.ScheduleTypeEnum.EverySixMonths:
                    nextDate = curDate.AddMonths(6);
                    break;
                case ScheduleCrewChangeModel.ScheduleTypeEnum.Yearly:
                    nextDate = curDate.AddYears(1);
                    break;
            }

            return nextDate;
        }
    }
}

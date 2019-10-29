using DevExpress.Web;
using DevExpress.Web.Mvc;
using Ensco.App.App_Start;
using System.Data;
using Ensco.Irma.Models;
using Ensco.Irma.Services;
using Ensco.Models;
using Ensco.Services;
using Ensco.Utilities;
using mUtilities.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using Ensco.Irma.Models.Admin;
using System.Reflection;
using System.Web.Configuration;
using Ensco.App.Infrastructure;

namespace Ensco.App.Areas.Common.Controllers
{
    [CustomAuthorize]
    public class AdminController : BaseController
    {
        string mType;
        string dType;
        public AdminController() {
            string[] arr = UtilitySystem.GetGridType();
            mType = arr[0];
            dType = arr[1];
        }
        public ActionResult GridPartial() {
            if (mType == null || mType == "")
                return null;
            try {
                dynamic handler = IrmaServiceSystem.GetServiceListModel(mType, dType);

                MethodInfo[] AllMethods = handler.GetType().GetMethods();
                MethodInfo fun = AllMethods.FirstOrDefault(mi => mi.Name == "GetQueryable" && mi.GetParameters().Count() == 1);

                DataTableModel dt = new DataTableModel(1, fun.Invoke(handler, new string[] { "Id" }));
                Session["GridPartial"] = dt;
                return PartialView("GridPartial", dt);
            }catch (Exception e) {

            }
            return null; 
        }
        [HttpPost]
        public ActionResult GridUpdate(string id, string action1) {
            Type modelType = IrmaServiceSystem.GetModelType(mType);
            var instance = Activator.CreateInstance(modelType);

            foreach (string key in this.Request.Form) {
                this.SetObjectProperty(key, Request.Form[key], instance);
            }
            ServiceSystem.SaveList(mType, dType, instance, true, action1);

            DataTableModel dt = (DataTableModel)Session["GridPartial"];
            return PartialView("GridPartial", dt);
        }
        [HttpPost]
        public ActionResult GridAdd(string id) {
            return this.GridUpdate(id, "Add");
        }
        [HttpPost]
        public ActionResult GridDelete(string id) {
            return this.GridUpdate(id, "Delete");
        }
        public ActionResult ManagePassport() {
            ManagePassportModel managePassportModel = new ManagePassportModel();
            Session["ManagePassportModel"] = managePassportModel;

            // If Corporate IRMA, show all users, otherwise just show non-AD users
            if (UtilitySystem.Settings.RigId == 1)
                managePassportModel.Passports = new DataTableModel(1, ServiceSystem.GetQueryable(EnscoConstants.EntityModel.User, "ADUser"));
            else
                managePassportModel.Passports = new DataTableModel(1, ServiceSystem.GetQueryable(EnscoConstants.EntityModel.User, string.Format("ADUser=false"), "Id"));

            return View(managePassportModel);
        }
        public ActionResult PassportPartial() {
            ManagePassportModel managePassportModel = (ManagePassportModel)Session["ManagePassportModel"];

            return PartialView("PassportPartial", managePassportModel);
        }

        [ValidateInput(false)]
        public ActionResult PassportDetailPartialRecords(int userId) {
            ManagePassportModel managePassportModel = (ManagePassportModel)Session["ManagePassportModel"];

            UserModel userModel = managePassportModel.SelectedUser;
            IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.UserRecord);
            if (userModel != null) {
                List<dynamic> list = dataModel.GetItems(string.Format("PassportId={0}", userModel.Id), "Id");
                userModel.RecordModel.UserRecords = list.Cast<UserRecordModel>().ToList();
            } else {
                userModel = new UserModel();
            }
            userModel.RecordModel.Id = userId;
            // Load user records for the passport
            return PartialView("PassportDetailPartialRecords", userModel.RecordModel);
        }

        public ActionResult PassportDetailPartialRecord(Nullable<int> recordId) {
            ManagePassportModel managePassportModel = (ManagePassportModel)Session["ManagePassportModel"];

            UserModel userModel = managePassportModel.SelectedUser;
            UserRecordModel recordModel = null;
            if (recordId == null) {
                recordModel = new UserRecordModel();
            } else if (ManagePassportModel.SelectedRecord != null) {
                recordModel = ManagePassportModel.SelectedRecord;
            } else {
                recordModel = userModel.RecordModel.UserRecords.Find(x => x.Id == recordId);
            }
            ManagePassportModel.SelectedRecord = recordModel;

            return PartialView("PassportDetailPartialRecords", userModel.RecordModel);
        }

        [HttpPost]
        public ActionResult PassportDetailPartialRecord(UserRecordModel model) {
            ManagePassportModel managePassportModel = (ManagePassportModel)Session["ManagePassportModel"];

            if (ModelState.IsValid) {
                UserRecordModel entityModel = managePassportModel.SelectedUser.RecordModel.UserRecords.FirstOrDefault(x => x.Id == model.Id);
                model.PassportId = entityModel.PassportId;
                model.DateCreated = entityModel.DateCreated;
                SafeExecute(() => ServiceSystem.Save(EnscoConstants.EntityModel.UserRecord, model, true));
            }

            ManagePassportModel.SelectedRecord = model;

            return PartialView("PassportDetailPartialRecord", model);
        }
        public ActionResult PassportDetailRecordImageUpdate() {
            ContentResult result = BinaryImageEditExtension.GetCallbackResult();

            return result;
        }

        public ActionResult PassportDetailUserImageUpdate() {
            ContentResult result = BinaryImageEditExtension.GetCallbackResult();

            return result;
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PassportAddPartial(UserModel model) {
            ManagePassportModel managePassportModel = (ManagePassportModel)Session["ManagePassportModel"];

            if (ModelState.IsValid) {
                // These fields need to be set for new User
                model.RequirePasswordChange = true;
                model.DateCreated = DateTime.Now;
                model.ADUser = false;
                model.Passport = model.FirstName[0] + model.LastName;
                model.Passport = model.Passport.ToLower();
                model.SiteId = "CORP";
                model.UserType = 1;
                model.RigId = UtilitySystem.Settings.RigId;

                string password = (model.Password != null) ? model.Password : "123";
                if (model.Password == null)
                    model.RequirePasswordChange = true;

                model.Password = Cryptography.Encrypt(model.Passport, password);
                if (model.DisplayName == null || model.DisplayName.Length <= 0) {
                    model.DisplayName = string.Format("{0} {1}", model.FirstName, model.LastName);
                }
                model = ServiceSystem.Add(EnscoConstants.EntityModel.User, model, true);
                if (model != null) {
                    List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>();
                    parameters.Add(new KeyValuePair<string, object>("rigid", model.RigId));
                    parameters.Add(new KeyValuePair<string, object>("passportid", model.Id));
                    ServiceSystem.RunStoredProcedure("CreatePassport", parameters);
                }
            } else {
                managePassportModel.SelectedUser = model;
                ViewData["UpdateError"] = true;
            }

            return PartialView("PassportPartial", managePassportModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PassportUpdatePartial(UserModel model) {
            ManagePassportModel managePassportModel = (ManagePassportModel)Session["ManagePassportModel"];

            if (ModelState.IsValid) {
                UserModel entityModel = managePassportModel.Passports.GetItem(model.Id);
                entityModel.Assign(model);
                bool result = ServiceSystem.Save(EnscoConstants.EntityModel.User, entityModel, true);
            } else {
                managePassportModel.SelectedUser = model;
            }

            return PartialView("PassportPartial", managePassportModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PassportDeletePartial(UserModel model) {
            ManagePassportModel managePassportModel = (ManagePassportModel)Session["ManagePassportModel"];

            if (ModelState.IsValid) {
                // Removing user from database (if used at least once, then disable the user, otherwise allow user to be deleted?)
                UserModel entityModel = managePassportModel.Passports.GetItem(model.Id);
                entityModel.Status = 3; // Disabled
                bool result = ServiceSystem.Save(EnscoConstants.EntityModel.User, entityModel, true);
                //bool result = ServiceSystem.Delete(EnscoConstants.EntityModel.User, entityModel, true);
            } else {
                managePassportModel.SelectedUser = model;
            }

            return PartialView("PassportPartial", managePassportModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PassportAddRecordPartial(UserRecordModel model) {
            ManagePassportModel managePassportModel = (ManagePassportModel)Session["ManagePassportModel"];

            UploadedFile item = Session["UploadedFile"] as UploadedFile;
            string fileName = Session["UploadedFileName"] as string;

            if (ModelState.IsValid) {
                model.PassportId = managePassportModel.SelectedUser.Id;
                model.DateCreated = DateTime.Now;
                if (item != null && fileName != null) {
                    model.DocumentFile = fileName;
                }

                model = ServiceSystem.Add(EnscoConstants.EntityModel.UserRecord, model, true);
                if (model != null && model.Id != 0) {
                    managePassportModel.SelectedUser.RecordModel.UserRecords.Add(model);
                }
            }

            ManagePassportModel.SelectedRecord = model;

            return PartialView("PassportDetailPartialRecords", managePassportModel.SelectedUser.RecordModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PassportUpdateRecordPartial(UserRecordModel model) {
            ManagePassportModel managePassportModel = (ManagePassportModel)Session["ManagePassportModel"];

            UploadedFile item = Session["UploadedFile"] as UploadedFile;
            string fileName = Session["UploadedFileName"] as string;
            if (ModelState.IsValid) {
                UserRecordModel entityModel = managePassportModel.SelectedUser.RecordModel.UserRecords.FirstOrDefault(x => x.Id == model.Id);
                model.PassportId = entityModel.PassportId;
                model.DateCreated = entityModel.DateCreated;
                if (item != null && fileName != null) {
                    model.DocumentFile = fileName;
                }
                bool result = ServiceSystem.Save(EnscoConstants.EntityModel.UserRecord, model, true);
            }

            ManagePassportModel.SelectedRecord = model;

            return PartialView("PassportDetailPartialRecords", managePassportModel.SelectedUser.RecordModel);
        }

        public void SafeExecute(Action method) {
            try {
                method();
            } catch (Exception e) {
                ViewData["EditError"] = e.Message;
            }
        }

        public ActionResult CreatePassport() {
            UserModel model = new UserModel();
            Session["UserModel"] = model;

            return View(model);
        }

        public ActionResult CreatePassportPartial() {
            UserModel model = (UserModel)Session["UserModel"];

            return PartialView("CreatePassportPartial", model);
        }

        public ActionResult PassportSearch(string parameter) {
            return PartialView("PassportSearch", parameter);
        }
        public ActionResult PassportSearch2(string filter) {
            IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.User);

            DataTableModel dataTable = new DataTableModel(1, dataModel.GetQueryable("Id"));

            dataTable.Filter = "Id=2";
            ViewData["PassportSearchString"] = filter;

            return PartialView("PassportSearch", dataTable);
        }
        [HttpPost]
        public ActionResult CreatePassportPartial(UserModel model) {
            if (ModelState.IsValid) {
                // These fields need to be set for new User
                model.RequirePasswordChange = true;
                model.DateCreated = DateTime.Now;
                model.ADUser = false;
                model.Passport = model.FirstName[0] + model.LastName;
                model.Passport = model.Passport.ToLower();
                model.RigId = UtilitySystem.Settings.RigId;
                model.SiteId = "CORP";
                model.UserType = 1;
                string password = (model.Password != null) ? model.Password : "123";
                if (model.Password == null)
                    model.RequirePasswordChange = true;

                model.Password = Cryptography.Encrypt(model.Passport, password);
                if (model.DisplayName == null || model.DisplayName.Length <= 0) {
                    model.DisplayName = string.Format("{0} {1}", model.FirstName, model.LastName);
                }
                model = ServiceSystem.Add(EnscoConstants.EntityModel.User, model, true);
                if (model != null) {
                    List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>();
                    parameters.Add(new KeyValuePair<string, object>("rigid", model.RigId));
                    parameters.Add(new KeyValuePair<string, object>("passportid", model.Id));
                    ServiceSystem.RunStoredProcedure("CreatePassport", parameters);
                }

                IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.User);
                model = dataModel.GetItem(string.Format("Id={0}", model.Id), "Id");
            }
            return Redirect("CreatePassport"); 
            return View("CreatePassportPartial", model);
        }

        public ActionResult ChangeHistory() {
            ManageChangeHistoryModel manageChangeHistory = new ManageChangeHistoryModel();
            Session["ManageChangeHistoryModel"] = manageChangeHistory;

            manageChangeHistory.Dataset = new DataTableModel(1, ServiceSystem.GetQueryable(EnscoConstants.EntityModel.History));

            return View(manageChangeHistory);
        }

        public ActionResult ChangeHistoryPartial() {
            ManageChangeHistoryModel manageChangeHistory = (ManageChangeHistoryModel)Session["ManageChangeHistoryModel"];

            return PartialView("ChangeHistoryPartial", manageChangeHistory);
        }

        public ActionResult ChangeHistoryDetailPartial(DataTableModel data) {
            return PartialView("ChangeHistoryDetailPartial", data);
        }

        public ActionResult ManageLists() {
            DataSet ds = da.GetDataSet(" exec usp_GetAdminList ");
            this.ViewData["AdminList"] = ds.Tables[0];

            if (UtilitySystem.Settings.RigId == 1) {
                CorpAdminManageModel manageCorpModel = new CorpAdminManageModel();
                Session["CorpAdminManageModel"] = manageCorpModel;

                return View(manageCorpModel);
            }
            return RedirectToAction("RigRequirements", "RigAdmin", new { Area = "IRMA" });
        }
        public ActionResult ManageCorpListPartial() {
            CorpAdminManageModel manageCorpModel = (CorpAdminManageModel)Session["CorpAdminManageModel"];

            return PartialView("ManageCorpListPartial", manageCorpModel);
        }

        [HttpPost]
        public ActionResult ManageCorpListPartial(CorpAdminManageModel model) {
            CorpAdminManageModel manageCorpModel = (CorpAdminManageModel)Session["CorpAdminManageModel"];

            if (model.Type != null) {
                switch (model.Type) {
                    case CorpAdminManageModel.ListType.ActionStatus:
                        manageCorpModel.List = UtilitySystem.GetLookupList("ActionStatus");
                        break;
                    case CorpAdminManageModel.ListType.BusinessUnit:
                        manageCorpModel.List = UtilitySystem.GetLookupList("BusinessUnit");
                        break;
                    case CorpAdminManageModel.ListType.Company:
                        manageCorpModel.List = UtilitySystem.GetLookupList("Company");
                        break;
                    case CorpAdminManageModel.ListType.Department:
                        manageCorpModel.List = UtilitySystem.GetLookupList("Department");
                        break;
                    case CorpAdminManageModel.ListType.Gender:
                        manageCorpModel.List = UtilitySystem.GetLookupList("Gender");
                        break;
                    case CorpAdminManageModel.ListType.MaritalStatus:
                        manageCorpModel.List = UtilitySystem.GetLookupList("MaritalStatus");
                        break;
                    case CorpAdminManageModel.ListType.Nationality:
                        manageCorpModel.List = UtilitySystem.GetLookupList("Nationality");
                        break;
                    case CorpAdminManageModel.ListType.Position:
                        manageCorpModel.List = UtilitySystem.GetLookupList("Position");
                        break;
                    case CorpAdminManageModel.ListType.GovtIdType:
                        manageCorpModel.List = UtilitySystem.GetLookupList("UserGovtIdType");
                        break;
                    case CorpAdminManageModel.ListType.RigStatus:
                        manageCorpModel.List = UtilitySystem.GetLookupList("RigStatus");
                        break;
                    case CorpAdminManageModel.ListType.RigType:
                        manageCorpModel.List = UtilitySystem.GetLookupList("RigType");
                        break;
                    case CorpAdminManageModel.ListType.UserRole:
                        manageCorpModel.List = UtilitySystem.GetLookupList("UserRole");
                        break;
                    case CorpAdminManageModel.ListType.UserStatus:
                        manageCorpModel.List = UtilitySystem.GetLookupList("UserStatus");
                        break;
                    case CorpAdminManageModel.ListType.UserType:
                        manageCorpModel.List = UtilitySystem.GetLookupList("UserType");
                        break;
                    case CorpAdminManageModel.ListType.PobStatus:
                        manageCorpModel.List = UtilitySystem.GetLookupList("PobStatus");
                        break;
                    case CorpAdminManageModel.ListType.LifeBoat:
                        manageCorpModel.List = UtilitySystem.GetLookupList("LifeBoat");
                        break;
                    case CorpAdminManageModel.ListType.MusterStation:
                        manageCorpModel.List = UtilitySystem.GetLookupList("MusterStation");
                        break;
                    case CorpAdminManageModel.ListType.RigCrew:
                        manageCorpModel.List = UtilitySystem.GetLookupList("RigCrew");
                        break;
                    case CorpAdminManageModel.ListType.Room:
                        manageCorpModel.List = UtilitySystem.GetLookupList("Room");
                        break;
                    case CorpAdminManageModel.ListType.Bed:
                        manageCorpModel.List = UtilitySystem.GetLookupList("RoomBed");
                        break;
                    case CorpAdminManageModel.ListType.ScheduleType:
                        manageCorpModel.List = UtilitySystem.GetLookupList("ScheduleType");
                        break;
                    case CorpAdminManageModel.ListType.EmergencyTeamType:
                        manageCorpModel.List = UtilitySystem.GetLookupList("TeamType");
                        break;
                    case CorpAdminManageModel.ListType.Tour:
                        manageCorpModel.List = UtilitySystem.GetLookupList("Tour");
                        break;
                }
            }
            return PartialView("ManageCorpListPartial", manageCorpModel);
        }

        public ActionResult ManageCorpListItemsPartial() {
            CorpAdminManageModel manageCorpModel = (CorpAdminManageModel)Session["CorpAdminManageModel"];

            return PartialView("ManageCorpListItemsPartial", manageCorpModel);
        }

        [HttpPost]
        public ActionResult ManageCorpListItemsUpdate(ListItem model) {
            CorpAdminManageModel manageCorpModel = (CorpAdminManageModel)Session["CorpAdminManageModel"];

            switch (manageCorpModel.List.ModelType.Name) {
                case "ActionStatusModel": {
                    ActionStatusModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    LookupListSystem.Save(UtilitySystem.EnscoLookupList.ActionStatus, item, true);
                }
                break;
                case "BusinessUnitModel": {
                    BusinessUnitModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    LookupListSystem.Save(UtilitySystem.EnscoLookupList.BusinessUnit, item, true);
                }
                break;
                case "CompanyModel": {
                    CompanyModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    LookupListSystem.Save(UtilitySystem.EnscoLookupList.Company, item, true);
                }
                break;
                case "DepartmentModel": {
                    DepartmentModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    LookupListSystem.Save(UtilitySystem.EnscoLookupList.Department, item, true);
                }
                break;
                case "GenderModel": {
                    GenderModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    LookupListSystem.Save(UtilitySystem.EnscoLookupList.Gender, item, true);
                }
                break;
                case "MaritalStatusModel": {
                    MaritalStatusModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    LookupListSystem.Save(UtilitySystem.EnscoLookupList.MaritalStatus, item, true);
                }
                break;
                case "NationalityModel": {
                    NationalityModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    LookupListSystem.Save(UtilitySystem.EnscoLookupList.Nationality, item, true);
                }
                break;
                case "PositionModel": {
                    PositionModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    LookupListSystem.Save(UtilitySystem.EnscoLookupList.Position, item, true);
                }
                break;
                case "RigStatusModel": {
                    RigStatusModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    LookupListSystem.Save(UtilitySystem.EnscoLookupList.RigStatus, item, true);
                }
                break;
                case "RigAssetTypeModel": {
                    RigAssetTypeModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    LookupListSystem.Save(UtilitySystem.EnscoLookupList.RigType, item, true);
                }
                break;
                case "UserGovtIdTypeModel": {
                    UserGovtIdTypeModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    LookupListSystem.Save(UtilitySystem.EnscoLookupList.UserGovtIdType, item, true);
                }
                break;
                case "UserRoleModel": {
                    UserRoleModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Role = model.Name;
                    LookupListSystem.Save(UtilitySystem.EnscoLookupList.UserRole, item, true);
                }
                break;
                case "UserStatusModel": {
                    UserStatusModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    LookupListSystem.Save(UtilitySystem.EnscoLookupList.UserStatus, item, true);
                }
                break;
                case "UserTypeModel": {
                    UserTypeModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    LookupListSystem.Save(UtilitySystem.EnscoLookupList.UserType, item, true);
                }
                break;
                case "PobStatusModel": {
                    PobStatusModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    IrmaServiceSystem.LookupUpdate(IrmaConstants.IrmaLookupLists.PobStatus, item, true);
                }
                break;
                case "LifeBoatModel": {
                    LifeBoatModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    IrmaServiceSystem.LookupUpdate(IrmaConstants.IrmaLookupLists.LifeBoat, item, true);
                }
                break;
                case "MusterStationModel": {
                    MusterStationModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    IrmaServiceSystem.LookupUpdate(IrmaConstants.IrmaLookupLists.MusterStation, item, true);
                }
                break;
                case "RoomModel": {
                    RoomModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    IrmaServiceSystem.LookupUpdate(IrmaConstants.IrmaLookupLists.Room, item, true);
                }
                break;
                case "RoomBedModel": {
                    RoomBedModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    IrmaServiceSystem.LookupUpdate(IrmaConstants.IrmaLookupLists.RoomBed, item, true);
                }
                break;
                case "ScheduleType": {
                    ScheduleTypeModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    IrmaServiceSystem.LookupUpdate(IrmaConstants.IrmaLookupLists.ScheduleType, item, true);
                }
                break;
                case "RigCrewModel": {
                    RigCrewModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    IrmaServiceSystem.LookupUpdate(IrmaConstants.IrmaLookupLists.PobStatus, item, true);
                }
                break;
                case "TeamTypeModel": {
                    TeamTypeModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    IrmaServiceSystem.LookupUpdate(IrmaConstants.IrmaLookupLists.PobStatus, item, true);
                }
                break;
                case "TourModel": {
                    TourModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    item.Name = model.Name;
                    IrmaServiceSystem.LookupUpdate(IrmaConstants.IrmaLookupLists.PobStatus, item, true);
                }
                break;
            }
            return PartialView("ManageCorpListItemsPartial", manageCorpModel);
        }

        [HttpPost]
        public ActionResult ManageCorpListItemsAdd(ListItem model) {
            CorpAdminManageModel manageCorpModel = (CorpAdminManageModel)Session["CorpAdminManageModel"];

            switch (manageCorpModel.List.ModelType.Name) {
                case "ActionStatusModel": {
                    ActionStatusModel item = new ActionStatusModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    item = LookupListSystem.Add(UtilitySystem.EnscoLookupList.ActionStatus, item, true);
                }
                break;
                case "BusinessUnitModel": {
                    BusinessUnitModel item = new BusinessUnitModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    item = LookupListSystem.Add(UtilitySystem.EnscoLookupList.BusinessUnit, item, true);
                }
                break;
                case "CompanyModel": {
                    CompanyModel item = new CompanyModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    item = LookupListSystem.Add(UtilitySystem.EnscoLookupList.Company, item, true);
                }
                break;
                case "NationalityModel": {
                    NationalityModel item = new NationalityModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    item = LookupListSystem.Add(UtilitySystem.EnscoLookupList.Nationality, item, true);
                }
                break;
                case "DepartmentModel": {
                    DepartmentModel item = new DepartmentModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    item = LookupListSystem.Add(UtilitySystem.EnscoLookupList.Department, item, true);
                }
                break;
                case "GenderModel": {
                    GenderModel item = new GenderModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    item = LookupListSystem.Add(UtilitySystem.EnscoLookupList.Gender, item, true);
                }
                break;
                case "MaritalStatusModel": {
                    MaritalStatusModel item = new MaritalStatusModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    item = LookupListSystem.Add(UtilitySystem.EnscoLookupList.MaritalStatus, item, true);
                }
                break;
                case "PositionModel": {
                    PositionModel item = new PositionModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    item = LookupListSystem.Add(UtilitySystem.EnscoLookupList.Position, item, true);
                }
                break;
                case "RigStatusModel": {
                    RigStatusModel item = new RigStatusModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    item = LookupListSystem.Add(UtilitySystem.EnscoLookupList.RigStatus, item, true);
                }
                break;
                case "RigAssetTypeModel": {
                    RigAssetTypeModel item = new RigAssetTypeModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    item = LookupListSystem.Add(UtilitySystem.EnscoLookupList.RigType, item, true);
                }
                break;
                case "UserGovtIdTypeModel": {
                    UserGovtIdTypeModel item = new UserGovtIdTypeModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    item = LookupListSystem.Add(UtilitySystem.EnscoLookupList.UserGovtIdType, item, true);
                }
                break;
                case "UserRoleModel": {
                    UserRoleModel item = new UserRoleModel();
                    item.Id = model.Id;
                    item.Role = model.Name;
                    item = LookupListSystem.Add(UtilitySystem.EnscoLookupList.UserRole, item, true);
                }
                break;
                case "UserStatusModel": {
                    UserStatusModel item = new UserStatusModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    item = LookupListSystem.Add(UtilitySystem.EnscoLookupList.UserStatus, item, true);
                }
                break;
                case "UserTypeModel": {
                    UserTypeModel item = new UserTypeModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    item = LookupListSystem.Add(UtilitySystem.EnscoLookupList.UserType, item, true);
                }
                break;
                case "PobStatusModel": {
                    PobStatusModel item = new PobStatusModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    IrmaServiceSystem.LookupAdd(IrmaConstants.IrmaLookupLists.PobStatus, item, true);
                }
                break;
                case "LifeBoatModel": {
                    LifeBoatModel item = new LifeBoatModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    IrmaServiceSystem.LookupAdd(IrmaConstants.IrmaLookupLists.LifeBoat, item, true);
                }
                break;
                case "MusterStationModel": {
                    MusterStationModel item = new MusterStationModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    IrmaServiceSystem.LookupAdd(IrmaConstants.IrmaLookupLists.MusterStation, item, true);
                }
                break;
                case "RoomModel": {
                    RoomModel item = new RoomModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    IrmaServiceSystem.LookupAdd(IrmaConstants.IrmaLookupLists.Room, item, true);
                }
                break;
                case "RoomBedModel": {
                    RoomBedModel item = new RoomBedModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    IrmaServiceSystem.LookupAdd(IrmaConstants.IrmaLookupLists.RoomBed, item, true);
                }
                break;
                case "RigCrewModel": {
                    RigCrewModel item = new RigCrewModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    IrmaServiceSystem.LookupAdd(IrmaConstants.IrmaLookupLists.RigCrew, item, true);
                }
                break;
                case "ScheduleTypeModel": {
                    ScheduleTypeModel item = new ScheduleTypeModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    IrmaServiceSystem.LookupAdd(IrmaConstants.IrmaLookupLists.ScheduleType, item, true);
                }
                break;
                case "TeamTypeModel": {
                    TeamTypeModel item = new TeamTypeModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    IrmaServiceSystem.LookupAdd(IrmaConstants.IrmaLookupLists.TeamType, item, true);
                }
                break;
                case "TourModel": {
                    TourModel item = new TourModel();
                    item.Id = model.Id;
                    item.Name = model.Name;
                    IrmaServiceSystem.LookupAdd(IrmaConstants.IrmaLookupLists.Tour, item, true);
                }
                break;
            }

            return PartialView("ManageCorpListItemsPartial", manageCorpModel);
        }

        [HttpPost]
        public ActionResult ManageCorpListItemsDelete(ListItem model) {
            CorpAdminManageModel manageCorpModel = (CorpAdminManageModel)Session["CorpAdminManageModel"];

            switch (manageCorpModel.List.ModelType.Name) {
                case "ActionStatusModel": {
                    ActionStatusModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    LookupListSystem.Delete(UtilitySystem.EnscoLookupList.ActionStatus, item, true);
                }
                break;
                case "BusinessUnitModel": {
                    BusinessUnitModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    LookupListSystem.Delete(UtilitySystem.EnscoLookupList.BusinessUnit, item, true);
                }
                break;
                case "CompanyModel": {
                    CompanyModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    LookupListSystem.Delete(UtilitySystem.EnscoLookupList.Company, item, true);
                }
                break;
                case "DepartmentModel": {
                    DepartmentModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    LookupListSystem.Delete(UtilitySystem.EnscoLookupList.Department, item, true);
                }
                break;
                case "PositionModel": {
                    PositionModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    LookupListSystem.Delete(UtilitySystem.EnscoLookupList.Position, item, true);
                }
                break;
                case "NationalityModel": {
                    NationalityModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    LookupListSystem.Delete(UtilitySystem.EnscoLookupList.Nationality, item, true);
                }
                break;
                case "GenderModel": {
                    GenderModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    LookupListSystem.Delete(UtilitySystem.EnscoLookupList.Gender, item, true);
                }
                break;
                case "MaritalStatusModel": {
                    MaritalStatusModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    LookupListSystem.Delete(UtilitySystem.EnscoLookupList.MaritalStatus, item, true);
                }
                break;
                case "RigStatusModel": {
                    RigStatusModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    LookupListSystem.Delete(UtilitySystem.EnscoLookupList.RigStatus, item, true);
                }
                break;
                case "RigAssetTypeModel": {
                    RigAssetTypeModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    LookupListSystem.Delete(UtilitySystem.EnscoLookupList.RigType, item, true);
                }
                break;
                case "UserGovtIdTypeModel": {
                    UserGovtIdTypeModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    LookupListSystem.Delete(UtilitySystem.EnscoLookupList.UserGovtIdType, item, true);
                }
                break;
                case "UserRoleModel": {
                    UserRoleModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    LookupListSystem.Delete(UtilitySystem.EnscoLookupList.UserRole, item, true);
                }
                break;
                case "UserStatusModel": {
                    UserStatusModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    LookupListSystem.Delete(UtilitySystem.EnscoLookupList.UserStatus, item, true);
                }
                break;
                case "UserTypeModel": {
                    UserTypeModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    LookupListSystem.Delete(UtilitySystem.EnscoLookupList.UserType, item, true);
                }
                break;
                case "PobStatusModel": {
                    PobStatusModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    IrmaServiceSystem.LookupDelete(IrmaConstants.IrmaLookupLists.PobStatus, item, true);
                }
                break;
                case "LifeBoatModel": {
                    LifeBoatModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    IrmaServiceSystem.LookupDelete(IrmaConstants.IrmaLookupLists.LifeBoat, item, true);
                }
                break;
                case "MusterStationModel": {
                    MusterStationModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    IrmaServiceSystem.LookupDelete(IrmaConstants.IrmaLookupLists.MusterStation, item, true);
                }
                break;
                case "RoomModel": {
                    RoomModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    IrmaServiceSystem.LookupDelete(IrmaConstants.IrmaLookupLists.Room, item, true);
                }
                break;
                case "RoomBedModel": {
                    RoomBedModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    IrmaServiceSystem.LookupDelete(IrmaConstants.IrmaLookupLists.RoomBed, item, true);
                }
                break;
                case "ScheduleType": {
                    ScheduleTypeModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    IrmaServiceSystem.LookupDelete(IrmaConstants.IrmaLookupLists.ScheduleType, item, true);
                }
                break;
                case "RigCrewModel": {
                    RigCrewModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    IrmaServiceSystem.LookupDelete(IrmaConstants.IrmaLookupLists.PobStatus, item, true);
                }
                break;
                case "TeamTypeModel": {
                    TeamTypeModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    IrmaServiceSystem.LookupDelete(IrmaConstants.IrmaLookupLists.PobStatus, item, true);
                }
                break;
                case "TourModel": {
                    TourModel item = manageCorpModel.List.Items.FirstOrDefault(x => x.Id == model.Id);
                    IrmaServiceSystem.LookupDelete(IrmaConstants.IrmaLookupLists.PobStatus, item, true);
                }
                break;
            }
            return PartialView("ManageCorpListItemsPartial", manageCorpModel);
        }

        public ActionResult ManageRigsPartial() {
            RigAdminManageModel manageRigModel = new RigAdminManageModel();
            Session["RigAdminManageModel"] = manageRigModel;

            IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.Rig);
            manageRigModel.Rigs = dataModel.GetAllItems().Cast<RigModel>().ToList();
            manageRigModel.Rigs.RemoveAt(0);

            return PartialView("ManageRigsPartial", manageRigModel);
        }
        [HttpPost]
        public ActionResult ManageRigsUpdate(RigModel model) {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["RigAdminManageModel"];
            if (ModelState.IsValid) {
                ServiceSystem.Save(EnscoConstants.EntityModel.Rig, model, true);
            }
            return PartialView("ManageRigsPartial", manageRigModel);
        }
        public ActionResult ManageAdminListPartial(string mType, string dType) {
            dynamic handler = IrmaServiceSystem.GetServiceListModel(mType, dType);// this.GetListCookie()  );
            MethodInfo[] AllMethods = handler.GetType().GetMethods();
            MethodInfo fun = AllMethods.FirstOrDefault(mi => mi.Name == "GetQueryable" && mi.GetParameters().Count() == 1);

            DataTableModel dt = new DataTableModel(1, fun.Invoke(handler, new string[] { "Id" }));
            //DataTableModel dt = new DataTableModel(1, dataModel.GetQueryable("Id"));
            Session["ManageAdminList"] = dt;

            return PartialView("ManageAdminListPartial", dt);// manageRigModel.JobCodes);
        }
        [HttpPost]
        public ActionResult ManageAdminListUpdate(string action, string mType, string dType, string id) {
            string type = this.GetListCookie();// this.Request.Cookies["ListName"].Value;
            Type modelType = IrmaServiceSystem.GetModelType(mType);
            var instance = Activator.CreateInstance(modelType);
            if (action == "Delete") {
                this.SetObjectProperty("Id", id, instance);
            } else {
                action = "Add";
                foreach (string key in this.Request.Form) {
                    if (key == "Id")
                        action = "Update";
                    this.SetObjectProperty(key, Request.Form[key], instance);
                }
            }
            if (true || ModelState.IsValid) {
                ServiceSystem.SaveList(mType, dType, instance, true, action);
            }

            DataTableModel manageRigModel = (DataTableModel)Session["ManageAdminList"];
            return PartialView("ManageAdminListPartial", manageRigModel);
        }
        [HttpPost]
        public ActionResult ManageAdminListDelete(string id) {
            return this.ManageAdminListUpdate("Delete", "", "", id);
        }
        public string GetListCookie() {
            string type = "Rig";
            if (this.Request.Cookies["ListName"] != null && this.Request.Cookies["ListName"].Value != "")
                type = this.Request.Cookies["ListName"].Value;
            return type;
        }
 
        public ActionResult ManageJobCodePartial() {

            RigAdminManageModel manageRigModel = new RigAdminManageModel();
            Session["RigAdminManageModel"] = manageRigModel;

            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.JobCode);
            // manageRigModel.JobCodes = new DataTableModel(1, dataModel.GetQueryable("Id"));
            DataTableModel dt = new DataTableModel(1, dataModel.GetQueryable("Id"));

            return PartialView("ManageJobCodePartial", dt);// manageRigModel.JobCodes);
        }
        [HttpPost]
        public ActionResult ManageJobCodeUpdate(Irma.Models.Admin.JobCodeModel item) {
            // RigAdminManageModel manageRigModel = new RigAdminManageModel();
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["RigAdminManageModel"];
            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.JobCode);

            if (ModelState.IsValid) {
                dataModel.Update(item);
            }

            return PartialView("ManageJobCodePartial", manageRigModel.JobCodes);
        }


        public ActionResult ManageRigPersonnelRelationPartial() {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["RigAdminManageModel"];
            IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.Rig);
            manageRigModel.Rigs = dataModel.GetAllItems().Cast<RigModel>().ToList();
            manageRigModel.Rigs.RemoveAt(0);

            IServiceDataModel rigRelation = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.RigPersonnelRelation);
            if (rigRelation != null) {
                manageRigModel.Personnel = rigRelation.GetItems(string.Format("RigId={0}", (int)0), "Id");
            }
            return PartialView("ManageRigPersonnelRelationPartial", manageRigModel);
        }

        [HttpPost]
        public ActionResult ManageRigPersonnelRelationPartial(RigAdminManageModel model) {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["RigAdminManageModel"];

            IServiceDataModel rigRelation = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.RigPersonnelRelation);
            Nullable<int> rigid = model.SelectedPersonnelRigId;
            if (rigid == null && manageRigModel.SelectedPersonnelRigId != null)
                rigid = manageRigModel.SelectedPersonnelRigId;

            if (rigid != null) {
                manageRigModel.SelectedPersonnelRigId = rigid;
                if (rigRelation != null) {
                    manageRigModel.Personnel = rigRelation.GetItems(string.Format("RigId={0}", (int)rigid), "Id");
                }
            } else {
                manageRigModel.Personnel = rigRelation.GetItems(string.Format("RigId={0}", (int)0), "Id");
            }

            return PartialView("ManageRigPersonnelRelationPartial", manageRigModel);
        }

        public ActionResult ManageCorpPersonnelRelationPartial() {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["RigAdminManageModel"];

            return PartialView("ManageCorpPersonnelRelationPartial", manageRigModel);
        }

        [HttpPost]
        public ActionResult CorpPersonnelRelationAdd(CorpPeronnelRelationModel model) {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["RigAdminManageModel"];

            if (ModelState.IsValid) {
                IServiceDataModel rigRelation = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.CorpPersonnelRelation);
                model = rigRelation.Add(model);
                manageRigModel.Personnel.Add(model);
            }
            return PartialView("ManageCorpPersonnelRelationPartial", manageRigModel);
        }

        [HttpPost]
        public ActionResult CorpPersonnelRelationUpdate(RigPersonnelRelationModel model) {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["RigAdminManageModel"];

            if (ModelState.IsValid) {
                IServiceDataModel rigRelation = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.CorpPersonnelRelation);
                model.RigId = (int)manageRigModel.SelectedPersonnelCorpId;
                rigRelation.Update(model);
                manageRigModel.Personnel = rigRelation.GetItems(string.Format("RigId={0}", (int)model.RigId), "Id");
            }
            return PartialView("ManageCorpPersonnelRelationPartial", manageRigModel);
        }

        [HttpPost]
        public ActionResult CorpPersonnelRelationDelete(RigPersonnelRelationModel model) {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["RigAdminManageModel"];

            if (ModelState.IsValid) {
                IServiceDataModel rigRelation = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.CorpPersonnelRelation);
                model.RigId = (int)manageRigModel.SelectedPersonnelCorpId;
                rigRelation.Delete(model);
            }
            return PartialView("ManageCorpPersonnelRelationPartial", manageRigModel);
        }

        public ActionResult ImageUpload(Nullable<int> Id) {
            UploadControlExtension.GetUploadedFiles("FilesUpload", UploadControlHelper.ValidationSettings, UploadControlHelper.uploadControl_FileUploadComplete);

            return null;
        }

        public ActionResult MultiFileUploadPartial(string SourceForm, string SourceFormId, string controlName=null) {
            ViewBag.SourceForm = SourceForm;
            ViewBag.SourceFormId = SourceFormId;
            ViewBag.ControlName = controlName;
            return PartialView("MultiFileUploadPartial");
        }

        public ActionResult MultiFileUpload(string SourceForm, string SourceFormId, string controlName=null) {
            // Stores file
            UploadControlExtension.GetUploadedFiles(controlName ?? "MultiFileUpload", UploadControlHelper.ValidationSettings, UploadControlHelper.MultiFileUpload_FileUploadComplete);
            return null;
        }
        public ActionResult DeleteAttachment(int id, string returnUrl) {
            IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.Attachment);
            AttachmentModel attachment = dataModel.GetItem(string.Format("Id={0}", id), "Id");
            if (attachment != null) {
                var filePath = Server.MapPath(attachment.FilePath);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
                dataModel.Delete(attachment);
            }
            return Redirect(returnUrl);
        }

        public ActionResult Globalization()
        {
            return Redirect("/localizationadmin/index.html");
        }
    }

    public class MultiFileSelectionBinder : DevExpressEditorsBinder
    {
        public MultiFileSelectionBinder() {
            UploadControlBinderSettings.ValidationSettings.Assign(UploadValidationSettings);
            UploadControlBinderSettings.FileUploadCompleteHandler = UploadControlHelper.MultiFileUpload_FileUploadComplete;
        }

        public static readonly UploadControlValidationSettings UploadValidationSettings = new UploadControlValidationSettings {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".gif", ".png" },
            MaxFileSize = 4194304
        };
    }

    public class UploadControlHelper
    {
        public static string UploadDirectory = WebConfigurationManager.AppSettings["uploadfilepath"].ToString();

        public static readonly UploadControlValidationSettings ValidationSettings = new UploadControlValidationSettings {
            //AllowedFileExtensions = new string[] { ".png", ".jpg", ".jpeg", ".jpe" },
            MaxFileSize = 4000000
        };

        public static void uploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e) {
            if (e.UploadedFile.IsValid) {
                string uploadPath = UtilitySystem.Settings.ConfigSettings["uploadfilepath"];
                string fileName = string.Format("{0}_{1}", Guid.NewGuid().ToString(), e.UploadedFile.FileName);
                string resultFilePath =HttpContext.Current.Server.MapPath(Path.Combine(uploadPath, fileName)) ;// Path.Combine(uploadPath, fileName);
                e.UploadedFile.SaveAs(resultFilePath);
                //if (HttpContext.Current.Session["UploadedFile"] == null)
                {
                    HttpContext.Current.Session["UploadedFile"] = e.UploadedFile;
                    HttpContext.Current.Session["UploadedFileName"] = fileName;
                }
                //((List<UploadedFile>)HttpContext.Current.Session["UploadedFile"]).Add(e.UploadedFile);
                //IUrlResolutionService urlResolver = sender as IUrlResolutionService;

                //if (urlResolver != null)
                //    e.CallbackData = new JavaScriptSerializer().Serialize(new { URL = urlResolver.ResolveClientUrl(resultFilePath) + "?refresh=" + Guid.NewGuid().ToString(), UserID = userId });
            }
        }

        public static void MultiFileUpload_FileUploadComplete(object sender, FileUploadCompleteEventArgs e) {
            string resultFileName = Path.GetRandomFileName() + "_" + e.UploadedFile.FileName;
            string resultURL = UploadDirectory + resultFileName;
            string resultFilePath = HttpContext.Current.Server.MapPath(resultURL);
            e.UploadedFile.SaveAs(resultFilePath);

            IUrlResolutionService urlResolver = sender as IUrlResolutionService;
            if (urlResolver != null) {
                string url = urlResolver.ResolveClientUrl(resultURL);
                e.CallbackData = GetCallbackData(e.UploadedFile, url);
            }

            string SourceForm = HttpContext.Current.Request.QueryString["SourceForm"];
            string sourceFormId = HttpContext.Current.Request.QueryString["SourceFormId"];
            sourceFormId = sourceFormId ?? HttpContext.Current.Request.Form["SourceFormId"];
            // Stores info in DB
            IServiceDataModel dataModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.Attachment);
            AttachmentModel attachment = new AttachmentModel() {
                FilePath = resultURL,
                SourceForm = SourceForm,
                SourceFormId = sourceFormId,
                FileName = e.UploadedFile.FileName
            };
            dataModel.Add(attachment);

        }

        static string GetCallbackData(UploadedFile uploadedFile, string fileUrl) {
            string name = uploadedFile.FileName;
            long sizeInKilobytes = uploadedFile.ContentLength / 1024;
            string sizeText = sizeInKilobytes.ToString() + " KB";

            return name + "|" + fileUrl + "|" + sizeText;
        }
    }
}

using Ensco.App.App_Start;
using System.Data; 
using Ensco.Irma.Models;
using Ensco.Irma.Models.Admin;
using Ensco.Irma.Services;
using Ensco.Models;
using Ensco.Services;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
namespace Ensco.App.Areas.IRMA.Controllers
{
    public class JsonStringResult : ContentResult
    {
        public JsonStringResult(string json)
        {
            Content = json;
            ContentType = "application/json";
        }
    }
    public class RigAdminController : Ensco.App.Areas.Common.Controllers.BaseController
    {

         
        public ActionResult RigRequirements()
        {
            RigAdminManageModel manageRigModel = new RigAdminManageModel();
            Session["manageRigModel"] = manageRigModel;

            IServiceDataModel rigRelation = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.RigAssetRelation);
            if(rigRelation != null) {
                manageRigModel.Assets = new DataTableModel(UtilitySystem.Settings.RigId, rigRelation.GetQueryable(string.Format("RigId={0}", UtilitySystem.Settings.RigId), "Id"));
            }

            return View(manageRigModel);
        }
        public ActionResult ValidateDateFormat(string[] arr)
        {
            string[] arr1 = new string[2] { "", "" };
            int i = 0;
            foreach(string pattern in arr) {
                DateTime date;
                string s = DateTime.Now.ToString(pattern);
                if(!DateTime.TryParse(s, out date))
                    arr1[i] = "failed";
                i++; 
            }
            return Json(string.Join(",", arr1), JsonRequestBehavior.AllowGet);
            return Content("{\"result_code\":200,{\"name\":\"John\", \"lastName\": \"Doe\"}}", "application/json");

            return Content("{\"result_code\":200,{\"d\":\"" + arr1[0] + "\", \"dt\": \"" + arr1[1] + "\"}}", "application/json");
        }
        public ActionResult RigAdminCurrentPartial()
        {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

            manageRigModel.SelectedPersonnelRigId = UtilitySystem.Settings.RigId;
            IIrmaServiceDataModel rigReqs = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.PobRigRequirements);
            if(rigReqs != null) {
                manageRigModel.Requirements = rigReqs.GetAllItems();
            }

            return PartialView("RigAdminCurrentPartial", manageRigModel);
        }

        public ActionResult RigAdminFieldsVisiblePartial()
        {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

            manageRigModel.SelectedPersonnelRigId = UtilitySystem.Settings.RigId;
            IIrmaServiceDataModel rigReqs = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.RigFieldVisible);
            if(rigReqs != null) {
                manageRigModel.FieldsVisible = rigReqs.GetAllItems();
            }

            return PartialView("RigAdminFieldsVisiblePartial", manageRigModel);
        }

        [HttpPost]
        public ActionResult RigAdminFieldsVisibleUpdate(RigFieldVisibilityModel model)
        {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

            if(ModelState.IsValid) {
                IIrmaServiceDataModel rigReqs = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.RigFieldVisible);
                RigFieldVisibilityModel entityModel = rigReqs.GetItem(string.Format("Id={0}", model.Id), "Id");
                if(entityModel != null) {
                    entityModel.Visible = model.Visible;
                    entityModel.Name = model.Name;
                    rigReqs.Update(entityModel);
                    manageRigModel.FieldsVisible = rigReqs.GetAllItems();
                }
            }

            return PartialView("RigAdminFieldsVisiblePartial", manageRigModel);
        }

        public ActionResult RigAdminAssetAutoPopulate()
        {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

            // Auto populate records
            //IServiceDataModel rigRelModel = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.RigAssetRelation);
            //LookupListModel<dynamic> lkpRoom = UtilitySystem.GetLookupList("Room");
            //LookupListModel<dynamic> lkpBed = UtilitySystem.GetLookupList("RoomBed");
            //LookupListModel<dynamic> lkpMuster = UtilitySystem.GetLookupList("MusterStation");
            //LookupListModel<dynamic> lkpLife = UtilitySystem.GetLookupList("LifeBoat");

            //foreach (RoomModel room in lkpRoom.Items)
            //{
            //    foreach (RoomBedModel bed in lkpBed.Items)
            //    {
            //        RigAssetRelationModel asset = new RigAssetRelationModel();
            //        asset.RigId = UtilitySystem.Settings.RigId;
            //        asset.Room = room.Id;
            //        asset.Bed = bed.Id;
            //        asset.MusterStation1 = 1;
            //        asset.MusterStation2 = 2;
            //        asset.PrimaryLifeBoat = 1;
            //        asset.SecondaryLifeBoat = 2;
            //        rigRelModel.Add(asset);
            //    }
            //}

            IServiceDataModel rigRelation = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.RigAssetRelation);
            if(rigRelation != null) {
                manageRigModel.Assets = new DataTableModel(UtilitySystem.Settings.RigId, rigRelation.GetQueryable(string.Format("RigId={0}", UtilitySystem.Settings.RigId), "Id"));
            }

            return RedirectToAction("RigRequirements", "RigAdmin");
        }

        public ActionResult RigAdminAssetsPartial()
        {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

            return PartialView("RigAdminAssetsPartial", manageRigModel);
        }


        [HttpPost]
        public ActionResult RigAssetRelationAdd(RigAssetRelationModel model)
        {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["RigAdminManageModel"];

            if(ModelState.IsValid) {
                IServiceDataModel rigRelation = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.RigAssetRelation);
                if(rigRelation != null) {
                    model.RigId = UtilitySystem.Settings.RigId;
                    rigRelation.Add(model);
                }
            }
            return PartialView("RigAdminAssetsPartial", manageRigModel);
        }

        [HttpPost]
        public ActionResult RigAssetRelationUpdate(RigAssetRelationModel model)
        {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["RigAdminManageModel"];

            if(ModelState.IsValid) {
                IServiceDataModel rigRelation = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.RigAssetRelation);
                if(rigRelation != null) {
                    model.RigId = UtilitySystem.Settings.RigId;
                    rigRelation.Update(model);
                }
            }

            return PartialView("RigAdminAssetsPartial", manageRigModel);
        }

        [HttpPost]
        public ActionResult RigAssetRelationDelete(RigAssetRelationModel model)
        {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["RigAdminManageModel"];

            if(ModelState.IsValid) {
                IServiceDataModel rigRelation = ServiceSystem.GetServiceModel(EnscoConstants.EntityModel.RigAssetRelation);
                if(rigRelation != null) {
                    model.RigId = UtilitySystem.Settings.RigId;
                    rigRelation.Delete(model);
                }
            }

            return PartialView("RigAdminAssetsPartial", manageRigModel);
        }


        public ActionResult RigAdminListsPartial()
        {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

            IIrmaServiceDataModel emailDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Emails);
            PobEmailModel pobEmail = emailDataModel.GetItem(string.Format("Name=\"PobSummaryReport\""), "Name");
            PobEmailModel dailyEmail = emailDataModel.GetItem(string.Format("Name=\"DailySummaryReport\""), "Name");

            char[] sep = { ';' };
            manageRigModel.MaxPOB = IrmaServiceSystem.GetMaxPOB();
            manageRigModel.DateFormat = UtilitySystem.Settings.ConfigSettings["DateFormat"];
            manageRigModel.DateTimeFormat = UtilitySystem.Settings.ConfigSettings["DateTimeFormat"];
            manageRigModel.PobSummaryEmailList = (pobEmail != null && pobEmail.Recipients != null) ? pobEmail.Recipients.Split(sep) : null;
            manageRigModel.DailySummaryEmailList = (dailyEmail != null && dailyEmail.Recipients != null) ? dailyEmail.Recipients.Split(sep) : null;

            // Phase 1 Parameters
            string val = IrmaServiceSystem.GetAdminCustomValue("EmailTime");
            manageRigModel.EmailTime = (val != null) ? DateTime.Parse(val) : DateTime.Now;

            val = IrmaServiceSystem.GetAdminCustomValue("Brazil");
            manageRigModel.IsRigInBrazil = (val != null && val == "1") ? true : false;
            val = IrmaServiceSystem.GetAdminCustomValue("RequireClientSignature");
            manageRigModel.ChooseClientSignAtTimeOfPermit = (val != null && val == "1") ? true : false;
            val = IrmaServiceSystem.GetAdminCustomValue("ClientHotWork");
            manageRigModel.IsClientRequireSignHotWorkPermit = (val != null && val == "1") ? true : false;
            val = IrmaServiceSystem.GetAdminCustomValue("ClientColdWork");
            manageRigModel.IsClientRequireSignColdWorkPermit = (val != null && val == "1") ? true : false;
            val = IrmaServiceSystem.GetAdminCustomValue("ClientConfinedSpace");
            manageRigModel.IsClientRequireSignConfinedWorkPermit = (val != null && val == "1") ? true : false;
            val = IrmaServiceSystem.GetAdminCustomValue("OIM");
            UserModel userOIM = ServiceSystem.GetUserFromPassport(val.Trim());
            manageRigModel.CurrentOIM = userOIM.Id;
            val = IrmaServiceSystem.GetAdminCustomValue("Master");
            UserModel userMaster = ServiceSystem.GetUserFromPassport(val.Trim());
            if(userMaster != null)
                manageRigModel.CurrentMaster = userMaster.Id;

            Session["manageRigModel"] = manageRigModel;
            return PartialView("RigAdminListsPartial", manageRigModel);
        }

        [HttpPost]
        public ActionResult RigAdminListsPartial(RigAdminManageModel model)
        {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

            if(ModelState.IsValid) {
                IrmaServiceSystem.SaveMaxPOB(model.MaxPOB);
                //UtilitySystem.Settings.ConfigSettings["DateFormat"] = model.DateFormat;
                //UtilitySystem.Settings.ConfigSettings["DateTimeFormat"] = model.DateTimeFormat;
                //UtilitySystem.SaveConfigSettings();
                IrmaServiceSystem.UpdateAdminCustomValue("DateFormat", model.DateFormat);
                IrmaServiceSystem.UpdateAdminCustomValue("DateTimeFormat", model.DateTimeFormat);
                UtilitySystem.Settings.ConfigSettings["DateFormat"] = model.DateFormat;
                UtilitySystem.Settings.ConfigSettings["DateTimeFormat"] = model.DateTimeFormat;

                IIrmaServiceDataModel emailDataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Emails);
                PobEmailModel emailModel = emailDataModel.GetItem(string.Format("Name=\"PobSummaryReport\""), "Name");
                emailModel.Recipients = (model.PobSummaryEmailList != null) ? string.Join(";", model.PobSummaryEmailList) : null;
                emailDataModel.Update(emailModel);

                emailModel = emailDataModel.GetItem(string.Format("Name=\"DailySummaryReport\""), "Name");
                emailModel.Recipients = (model.DailySummaryEmailList != null) ? string.Join(";", model.DailySummaryEmailList) : null;
                emailDataModel.Update(emailModel);

                string dailyEmails = "";
                if(model.DailySummaryEmailList != null) {
                    char[] sep = { ';' };
                    foreach(string sid in model.DailySummaryEmailList) {
                        int id = 0;
                        if(!int.TryParse(sid, out id))
                            continue;

                        UserModel user = ServiceSystem.GetUser(id);
                        if(user != null && user.Email != null && user.Email.Length > 0) {
                            dailyEmails += user.Email + ";";
                        }
                    }
                    dailyEmails = dailyEmails.Trim(sep);
                }

                UserModel userOIM = ServiceSystem.GetUser(model.CurrentOIM);
                UserModel userMaster = ServiceSystem.GetUser(model.CurrentMaster);

                // Phase 1 parameters
                IrmaServiceSystem.UpdateAdminCustomValue("EmailTime", model.EmailTime.ToString("HH:mm"));

                IrmaServiceSystem.UpdateAdminCustomValue("Brazil", string.Format("{0}", model.IsRigInBrazil ? "1" : "0"));
                IrmaServiceSystem.UpdateAdminCustomValue("RequireClientSignature", string.Format("{0}", model.ChooseClientSignAtTimeOfPermit ? "1" : "0"));
                IrmaServiceSystem.UpdateAdminCustomValue("ClientHotWork", string.Format("{0}", model.IsClientRequireSignHotWorkPermit ? "1" : "0"));
                IrmaServiceSystem.UpdateAdminCustomValue("ClientColdWork", string.Format("{0}", model.IsClientRequireSignColdWorkPermit ? "1" : "0"));
                IrmaServiceSystem.UpdateAdminCustomValue("ClientConfinedSpace", string.Format("{0}", model.IsClientRequireSignConfinedWorkPermit ? "1" : "0"));
                IrmaServiceSystem.UpdateAdminCustomValue("DailySummaryEmails", dailyEmails);
                if(userOIM != null) {
                    IrmaServiceSystem.UpdateAdminCustomValue("OIM", userOIM.Passport);
                    IrmaServiceSystem.UpdateAdminCustomValue("OIMName", userOIM.DisplayName);
                }
                if(userMaster != null) {
                    IrmaServiceSystem.UpdateAdminCustomValue("Master", userMaster.Passport);
                    IrmaServiceSystem.UpdateAdminCustomValue("MasterName", userMaster.DisplayName);
                }

                manageRigModel = model;
            }
            ControllerContext.HttpContext.Response.Redirect("RigRequirements");
            ViewBag.Flag = "Save";
            return null;
            return PartialView("RigAdminListsPartial", manageRigModel);
        }

        public ActionResult RigAdminUserRolePartial()
        {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.AdminView);
            manageRigModel.AdminUsers = new DataTableModel(1, dataModel.GetQueryable("id"));
            this.ViewBag.IaType = this.GetIaType(); 
            return PartialView("RigAdminUserRolePartial", manageRigModel.AdminUsers);
        }
        DataTable GetIaType() {
            return this.GetDataSet("select * from ListIAType").Tables[0]; 
        }
        [HttpPost]
        public ActionResult RigAdminUserRoleAdd(AdminModel model)
        {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

            if(ModelState.IsValid) {
                LookupListModel<dynamic> lkpList = UtilitySystem.GetLookupList("Position");

                UserModel user = ServiceSystem.GetUser(model.Passport);
                model.Name = user.DisplayName;
                model.UserId = user.Passport;
                model.dt = DateTime.Now;
                DataTable dt= this.GetIaType(); 
                if (model.Position==null || dt.Select("Name='"+model.Position+"'").Length==0)
                    model.Position = (string)lkpList.GetDisplayValue(user.Position);

                IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Admin);
                AdminModel item = dataModel.GetItem(string.Format("UserId=\"{0}\" and RoleId={1} and Position=\"{2}\" ", model.UserId, model.RoleId, model.Position), "id");
                if(item == null) {
                    model = IrmaServiceSystem.Add(IrmaConstants.IrmaPobModels.Admin, model, true);
                }
            }

            return PartialView("RigAdminUserRolePartial", manageRigModel.AdminUsers);
        }

        [HttpPost]
        public ActionResult RigAdminUserRoleDelete(AdminModel model)
        {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

            if(ModelState.IsValid) {
                IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.Admin);
                AdminModel entity = dataModel.GetItem(string.Format("id={0}", model.id), "id");
                if(entity != null) {
                    bool bResult = dataModel.Delete(entity);
                }
            }

            return PartialView("RigAdminUserRolePartial", manageRigModel.AdminUsers);
        }

        public ActionResult RigAdminIsolationLockPartial()
        {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.IsolationLock);
            manageRigModel.IsolationLocks = new DataTableModel(1, dataModel.GetQueryable("id"));

            return PartialView("RigAdminIsolationLockPartial", manageRigModel.IsolationLocks);
        }

        [HttpPost]
        public ActionResult RigAdminIsolationLockAdd(LockModel model)
        {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

            if(ModelState.IsValid) {
                LookupListModel<dynamic> lkpList = UtilitySystem.GetLookupList("YesNoList");
                int listId = 0;
                if(!int.TryParse(model.Available, out listId))
                    listId = 0;
                model.Available = (string)lkpList.GetDisplayValue(listId);

                model = IrmaServiceSystem.Add(IrmaConstants.IrmaPobModels.IsolationLock, model, true);
            }

            return PartialView("RigAdminIsolationLockPartial", manageRigModel.IsolationLocks);
        }

        [HttpPost]
        public ActionResult RigAdminIsolationLockDelete(LockModel model)
        {
            RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

            if(ModelState.IsValid) {
                IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.IsolationLock);
                LockModel entity = dataModel.GetItem(string.Format("id={0}", model.id), "id");
                if(entity != null) {
                    bool bResult = dataModel.Delete(entity);
                }
            }

            return PartialView("RigAdminIsolationLockPartial", manageRigModel.IsolationLocks);
        }

         
        public ActionResult PersonnelArchive()
        {
            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.RigPersonnelArchive);
            DataTableModel model = new DataTableModel(1, dataModel.GetQueryable("Id"));

            return View(model);
        }

        public ActionResult PersonnelArchivePartial()
        {
            IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.RigPersonnelArchive);
            DataTableModel model = new DataTableModel(1, dataModel.GetQueryable("Id"));

            return PartialView("PersonnelArchivePartial", model);
        }

        //public ActionResult RigAdminCurrentDetailPartial(Nullable<int> Id)
        //{
        //    RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

        //    RigRequirementsModel requirementModel = null;
        //    if (Id == null)
        //    {
        //        manageRigModel.SelectedRequirement = new RigRequirementsModel();
        //        requirementModel = manageRigModel.SelectedRequirement;
        //    }
        //    else if (manageRigModel.SelectedRequirement != null && Id == manageRigModel.SelectedRequirement.Id)
        //    {
        //        requirementModel = manageRigModel.SelectedRequirement;
        //    }
        //    else
        //    {
        //        requirementModel = manageRigModel.Requirements.FirstOrDefault(x => x.Id == Id);
        //        if (requirementModel != null)
        //            manageRigModel.SelectedRequirement = requirementModel;
        //        else
        //            requirementModel = manageRigModel.SelectedRequirement;
        //    }

        //    IIrmaServiceDataModel subReqModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.PobRigSubRequirements);
        //    if (requirementModel != null)
        //    {
        //        List<dynamic> list = subReqModel.GetItems(string.Format("Requirement={0}", requirementModel.Id), "Id");
        //        requirementModel.SubRequirements = list.Cast<RigSubRequirementsModel>().ToList();
        //    }

        //    return PartialView("RigAdminCurrentDetailPartial", manageRigModel.SelectedRequirement);
        //}

        //[HttpPost]
        //public ActionResult RigAdminCurrentDetailAdd(RigSubRequirementsModel model)
        //{
        //    RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

        //    if (ModelState.IsValid)
        //    {
        //        IIrmaServiceDataModel rigReqs = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.PobRigSubRequirements);
        //        model.Requirement = manageRigModel.SelectedRequirement.Id;
        //        model = rigReqs.Add(model);
        //        manageRigModel.SelectedRequirement.SubRequirements.Add(model);
        //    }

        //    return PartialView("RigAdminCurrentDetailPartial", manageRigModel.SelectedRequirement);
        //}

        //[HttpPost]
        //public ActionResult RigAdminCurrentDetailUpdate(RigSubRequirementsModel model)
        //{
        //    RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

        //    if (ModelState.IsValid)
        //    {
        //        IIrmaServiceDataModel rigReqs = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.PobRigSubRequirements);
        //        RigSubRequirementsModel entityModel = rigReqs.GetItem(string.Format("Id={0}", model.Id), "Id");
        //        entityModel.EvidenceRequired = model.EvidenceRequired;
        //        bool result = rigReqs.Update(entityModel);
        //    }

        //    return PartialView("RigAdminCurrentDetailPartial", manageRigModel.SelectedRequirement);
        //}

        //[HttpPost]
        //public ActionResult RigAdminCurrentDetailDelete(RigSubRequirementsModel model)
        //{
        //    RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

        //    if (ModelState.IsValid)
        //    {
        //        IIrmaServiceDataModel rigReqs = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.PobRigSubRequirements);
        //        RigSubRequirementsModel entityModel = rigReqs.GetItem(string.Format("Id={0}", model.Id), "Id");
        //        bool result = rigReqs.Delete(entityModel);
        //    }

        //    return PartialView("RigAdminCurrentDetailPartial", manageRigModel.SelectedRequirement);
        //}


        //[HttpPost]
        //public ActionResult RigAdminCurrentAdd(RigRequirementsModel model)
        //{
        //    RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

        //    if (ModelState.IsValid)
        //    {
        //        IIrmaServiceDataModel rigReqs = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.PobRigRequirements);
        //        rigReqs.Add(model);
        //        manageRigModel.Requirements.Add(model);
        //    }

        //    return PartialView("RigAdminCurrentPartial", manageRigModel);
        //}
        //[HttpPost]
        //public ActionResult RigAdminCurrentUpdate(RigRequirementsModel model)
        //{
        //    RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

        //    if (ModelState.IsValid)
        //    {
        //        IIrmaServiceDataModel rigReqs = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.PobRigRequirements);
        //        RigRequirementsModel entityModel = rigReqs.GetItem(string.Format("Id={0}", model.Id), "Id");
        //        if (entityModel != null)
        //        {
        //            entityModel.Required = model.Required;
        //            rigReqs.Update(entityModel);
        //            manageRigModel.Requirements = rigReqs.GetAllItems();
        //        }
        //    }

        //    return PartialView("RigAdminCurrentPartial", manageRigModel);
        //}

        //[HttpPost]
        //public ActionResult RigAdminCurrentDelete(RigRequirementsModel model)
        //{
        //    RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

        //    if (ModelState.IsValid)
        //    {
        //        IIrmaServiceDataModel rigReqs = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.PobRigRequirements);
        //        rigReqs.Delete(model);
        //    }
        //    return PartialView("RigAdminCurrentPartial", manageRigModel);
        //}

        // 
        //public ActionResult RigCompliance()
        //{
        //    RigAdminManageModel manageRigModel = new RigAdminManageModel();
        //    Session["manageRigModel"] = manageRigModel;

        //    return View(manageRigModel);
        //}

        //public ActionResult RigCompliancePartial()
        //{
        //    RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

        //    IIrmaServiceDataModel dataModel = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.RigComplianceUsers);
        //    manageRigModel.Personnel = dataModel.GetAllItems();


        //    return PartialView("RigCompliancePartial", manageRigModel);
        //}

        //public ActionResult RigComplianceDetailPartial(Nullable<int> Id)
        //{
        //    RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

        //    manageRigModel.SelectedPersonnelRigId = UtilitySystem.Settings.RigId;
        //    IIrmaServiceDataModel rigCompliance = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.PobRigCompliance);
        //    if (rigCompliance != null)
        //    {
        //        manageRigModel.Compliance = rigCompliance.GetItems(string.Format("Id={0}", Id), "Id");
        //    }

        //    return PartialView("RigComplianceDetailPartial", manageRigModel);
        //}

        //public ActionResult RigAdminCompliancePartial()
        //{
        //    RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

        //    manageRigModel.SelectedPersonnelRigId = UtilitySystem.Settings.RigId;
        //    IIrmaServiceDataModel rigCompliance = IrmaServiceSystem.GetServiceModel(IrmaConstants.IrmaPobModels.PobRigCompliance);
        //    if (rigCompliance != null)
        //    {
        //        manageRigModel.Compliance = rigCompliance.GetAllItems();
        //    }

        //    return PartialView("RigAdminCompliancePartial", manageRigModel);
        //}


        //public ActionResult RigAdminPersonnelPartial()
        //{
        //    RigAdminManageModel manageRigModel = (RigAdminManageModel)Session["manageRigModel"];

        //    manageRigModel.SelectedPersonnelRigId = UtilitySystem.Settings.RigId;

        //    return PartialView("RigAdminPersonnelPartial", manageRigModel);
        //}


    }
}
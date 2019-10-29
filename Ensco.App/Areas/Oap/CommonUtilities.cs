using DevExpress.Web.Mvc;
using Ensco.Irma.Oap.Common.Models;
using corpModels = Ensco.Irma.Oap.WebClient.Corp;
using Ensco.Irma.Oap.WebClient.Rig;
using Ensco.Models;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Ensco.Irma.Oap.WebClient.Corp;

namespace Ensco.Irma.Oap.Web.Rig.Areas.Oap
{
    public class CommonUtilities
    {     

        public static IEnumerable<Person> GetUsers(PeopleClient peopleClient)
        {
            var users = peopleClient.GetAllAsync().Result?.Result?.Data;
            return users;
        }

        public static IEnumerable<corpModels.Person> GetUsers(corpModels.PeopleClient peopleClient)
        {
            var users = peopleClient.GetAllAsync().Result?.Result?.Data;
            return users;
        }


        public static int? GetTourId(PeopleClient peopleClient, int userId)
        {
            var tourId = peopleClient.GetTourIdAsync(userId).Result?.Result?.Data;

            return tourId;
        }


        public static IEnumerable<Criticality> GetCriticalities(LookupClient lookupClient)
        {
            var criticalities = lookupClient.GetAllCriticalityAsync().Result?.Result?.Data;

            return criticalities;
        }

        public static IEnumerable<FindingType> GetFindingTypes(FindingTypeClient client)
        {
            var values = client.GetAllAsync().Result?.Result?.Data;

            return values;
        }

        public static IEnumerable<FindingSubType> GetFindingSubTypes(FindingSubTypeClient client)
        {
            var values = client.GetAllAsync().Result?.Result?.Data;

            return values;
        }

        public static IEnumerable<OapChecklist> GetChecklists(OapChecklistClient client, GetAllModel model)
        {
            var values = client.GetAllAsync(model).Result?.Result?.Data;

            return values;
        }

        public static IEnumerable<OapChecklist> GetChecklists(OapChecklistClient client, string typeName, string subTypeName, string formType = "All", bool useTypeSubTypeFormTypeCode = false)
        {
            var values = (useTypeSubTypeFormTypeCode) ? client.GetAllByTypeSubTypeFormTypeCodeAsync(typeName, subTypeName, formType).Result?.Result?.Data : client.GetAllByTypeSubTypeFormTypeAsync(typeName, subTypeName, formType).Result?.Result?.Data;
            
            return values;
        }    

        //public static IEnumerable<OapChecklist> GetChecklists(OapChecklistClient client, string typeName, string subTypeName, bool useTypeSubTypeCode = false)
        //{
        //    var values = (useTypeSubTypeCode) ? client.GetAllByTypeSubTypeCodeAsync(typeName, subTypeName).Result?.Result?.Data : client.GetAllByTypeSubTypeAsync(typeName, subTypeName).Result?.Result?.Data;

        //    return values;
        //}

        public static Action<MVCxGridViewColumn> GetPassportColumnAction(HtmlHelper html, ViewContext viewContext, string fieldName, string name, string caption,IEnumerable<Person> users)
        {
            return (c) =>
            {
                c.Width = Unit.Percentage(10);
                c.FieldName = fieldName;
                c.Caption = caption;
                c.Name = name;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Left;
               

                //c.SetDataItemTemplateContent((ic) =>
                //{
                //    var ownerId = (int)(DataBinder.Eval(ic.DataItem, fieldName) ?? 0);
                //    var selectedUser = users?.FirstOrDefault(p => p.Id == ownerId);
                //    var ownerName = (selectedUser != null) ? selectedUser.Name : string.Empty;

                //    viewContext.Writer.Write(ownerName);
                //});


                c.SetEditItemTemplateContent((ic) =>
                {  
                    var ownerId =  UtilitySystem.CurrentUserId;   

                    html.RenderAction("GridLookupPartial", "Common", new { Area = "Common", name = c.FieldName, lookup = "Passport", multi = false, selected = ownerId });
                });
            };
        }


        public static Action<MVCxGridViewColumn> GetPassportColumnAction(HtmlHelper html, ViewContext viewContext, string fieldName, string name, string caption, IEnumerable<corpModels.Person> users)
        {
            return (c) =>
            {
                c.Width = Unit.Percentage(10);
                c.FieldName = fieldName;
                c.Caption = caption;
                c.Name = name;
                c.CellStyle.HorizontalAlign = HorizontalAlign.Left;


                //c.SetDataItemTemplateContent((ic) =>
                //{
                //    var ownerId = (int)(DataBinder.Eval(ic.DataItem, fieldName) ?? 0);
                //    var selectedUser = users?.FirstOrDefault(p => p.Id == ownerId);
                //    var ownerName = (selectedUser != null) ? selectedUser.Name : string.Empty;

                //    viewContext.Writer.Write(ownerName);
                //});


                c.SetEditItemTemplateContent((ic) =>
                {
                    var ownerId = UtilitySystem.CurrentUserId;

                    html.RenderAction("GridLookupPartial", "Common", new { Area = "Common", name = c.FieldName, lookup = "Passport", multi = false, selected = ownerId });
                });
            };
        }


        public static RigOapChecklist UpdateChecklist(RigOapChecklistClient client, RigOapChecklist checklist, GridRouteTypes type, int originalHasCode, string gridErrorKey, ViewDataDictionary viewData)
        {

            if (client == null)
            {
                viewData[gridErrorKey] = "Rig Oap client is null. Refresh Page and try again. Contact system administrator if the error persists";
                return checklist;
            }
            var currentHashCode = checklist.GetHashCode();

            if (currentHashCode != originalHasCode)
            {
                try
                {
                    switch (type)
                    {
                        case GridRouteTypes.Add:
                        case GridRouteTypes.Update:
                        case GridRouteTypes.Delete:
                            var updateResponse = client.UpdateRigChecklistAsync(checklist).Result;

                            if (updateResponse.Result.Errors.Any())
                            {
                                DisplayGridErrors(updateResponse.Result.Errors, gridErrorKey, viewData);
                            }
                            else
                            {
                                checklist = updateResponse.Result.Data;
                            }

                            break;
                    }
                }
                catch (Exception ex)
                {

                    viewData[gridErrorKey] = ex.Message;
                }

                return checklist;
            }

            return checklist;
        }


        public static corpModels.RigOapChecklist UpdateProtocol(corpModels.OapAuditClient client, corpModels.RigOapChecklist protocol, GridRouteTypes type, int originalHasCode, string gridErrorKey, ViewDataDictionary viewData)
        {

            if (client == null)
            {
                viewData[gridErrorKey] = "Client is null. Refresh Page and try again. Contact system administrator if the error persists";
                return protocol;
            }
            var currentHashCode = protocol.GetHashCode();

            if (currentHashCode != originalHasCode)
            {
                try
                {
                    switch (type)
                    {
                        case GridRouteTypes.Add:
                        case GridRouteTypes.Update:
                        case GridRouteTypes.Delete:
                            var updateResponse = client.UpdateRigChecklistAsync(protocol).Result;

                            if (updateResponse.Result.Errors.Any())
                            {
                                DisplayGridErrors(updateResponse.Result.Errors, gridErrorKey, viewData);
                            }
                            else
                            {
                                protocol = updateResponse.Result.Data;
                            }

                            break;
                    }
                }
                catch (Exception ex)
                {

                    viewData[gridErrorKey] = ex.Message;
                }

                return protocol;
            }

            return protocol;
        }


        public static void DisplayGridErrors(ObservableCollection<string> errors, string gridErrorKey, ViewDataDictionary viewData) 
        {
            if (errors.Any())
            {
                viewData[gridErrorKey] = errors.Aggregate((s, n) => $"{s}; {n}");
            }
        }
    }
}
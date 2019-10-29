using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Threading.Tasks;
using DevExpress.Web;
using DevExpress.Web.Mvc;


namespace Ensco.App.Areas.cOap.Controllers
{
    using DevExpress.Utils;
    using Ensco.Irma.Oap.Web.Oap.Controllers;
    using Ensco.Irma.Oap.Common.Extensions;
    using Ensco.Irma.Oap.Common.Models;
    using Ensco.Irma.Oap.WebClient.Corp;
    using System.Drawing;
    using System.Web.UI.WebControls;
    using Ensco.App.App_Start;
    using System.Configuration;
    using Ensco.Utilities;

    public class OapGraphicsController : OapCorpGridController
    {
        public const string CurrentGraphicId = "CurrentGraphicId";
        public const string CurrentGraphicName = "CurrentGraphicName";

        private OapGraphicClient OapGraphicClient { get; }

        private GridData GridData { get; } = new GridData("OapGraphicGrid", "OapGraphics", "Graphics", "Open Graphics", "AddNewGraphic", "Add Graphic", "search graphics", initializeCallBack: true);

        private void InitializeGridData(GridData gridData, string createAction, string updateAction, string deleteAction)
        {
            gridData.ToolBarOptions.DisplayXlsExport = true;

            gridData.DisplayColumns = new List<GridDisplayColumn>()
                            {
                                new GridDisplayColumn("Name", displayName:"Name", width:25, editLayoutWidth:100),
                                new GridDisplayColumn("Description", displayName:"Description", width:60, editLayoutWidth:100),
                                new GridDisplayColumn("StartDateTime", displayName:"Start Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g"),
                                new GridDisplayColumn("EndDateTime", displayName:"End Date", columnType:MVCxGridViewColumnType.DateEdit, width:7, editLayoutWidth:100, displayFormat: "g"),
                               new GridDisplayColumn("Image", isVisible:false, columnAction: (c) => {
                                    c.Name = "Image";
                                    c.FieldName = "Image";
                                    c.EditorProperties().BinaryImage(p => {
                                    p.ImageHeight = 170;
                                    p.ImageWidth = 160;
                                    p.EnableServerResize = true;
                                    p.ImageSizeMode = ImageSizeMode.FitProportional;
                                    p.CallbackRouteValues = new { Controller = GridData.Controller, Action = "ImageUpload"};
                                    p.EditingSettings.Enabled = true;
                                    p.EditingSettings.UploadSettings.UploadValidationSettings.MaxFileSize = 4194304;
                                    });
                                }),

                                new GridDisplayColumn("CreatedBy", displayName:"Created By", isReadOnly:true, width:0, isVisible:false),
                                new GridDisplayColumn("CreatedDateTime", displayName:"Date Created", columnType:MVCxGridViewColumnType.DateEdit, width:0, isReadOnly:true, displayFormat: "g", isVisible:false),
                                new GridDisplayColumn("UpdatedBy", displayName:"Updated By", width:0, isReadOnly:true, isVisible:false),
                                new GridDisplayColumn("UpdatedDateTime", displayName:"Date Updated", columnType:MVCxGridViewColumnType.DateEdit, width:0, isReadOnly:true, displayFormat: "g", isVisible:false)
                            };

            gridData.Routes = new List<GridRoute>()
                            {
                                new GridRoute(GridRouteTypes.Add, new { Controller = gridData.Controller, Action = createAction }),
                                new GridRoute(GridRouteTypes.Update, new { Controller = gridData.Controller, Action = updateAction }),
                                new GridRoute(GridRouteTypes.Delete, new { Controller = gridData.Controller, Action = deleteAction }),
                            };

            gridData.LayoutColumns = new List<GridEditLayoutColumn>()
                            {
                                new GridEditLayoutColumn("Image", displayName:"Image", columnType: MVCxGridViewColumnType.BinaryImage, width: 20, isWidthAndHeightInPercentage :true, layoutAction: (c) => {
                                    c.ColumnName = "Image";
                                    c.ShowCaption = DefaultBoolean.False;
                                    c.RowSpan = 4;
                                }),

                                new GridEditLayoutColumn("Name", displayName:"Name"),
                                new GridEditLayoutColumn("Description", displayName:"Description"),
                                new GridEditLayoutColumn("StartDateTime", displayName:"Start Date", columnType: MVCxGridViewColumnType.DateEdit),
                                new GridEditLayoutColumn("EndDateTime", displayName:"End Date", columnType: MVCxGridViewColumnType.DateEdit),
                            };

            gridData.FormLayout = new GridEditFormLayout(
                    gridData.LayoutColumns
                    , editMode: GridViewEditingMode.EditFormAndDisplayRow
                    , processLayout: i =>
                    {
                        i.ShowUpdateButton = true;
                        i.ShowCancelButton = true;
                        i.HorizontalAlign = FormLayoutHorizontalAlign.Right;
                    }
                    , columnCount: 3
                    , emptyLayputItemCount: 1
                    , adaptiveMode: FormLayoutAdaptivityMode.Off
                    );
        }

        public override void Configure(GridViewSettings settings, HtmlHelper html, ViewContext viewContext)
        {
            settings.CommandColumn.Width = Unit.Percentage(2);      

            InitializeGridData(GridData, "CreateGraphic", "UpdateGraphic", "DeleteGraphic");

            //Master Grid Specific configurations
            settings.KeyFieldName = GridData.Key;
            settings.Name = $"{GridData.Name}";

            //Generic configurations
            settings.Configure(GridData, html, viewContext);

            SelectRow((int)(ViewData[CurrentGraphicId] ?? 0), (string)(ViewData[CurrentGraphicName] ?? string.Empty), ref settings);

            settings.CellEditorInitialize = (s, e) =>
            {
                e.Editor.ReadOnly = false;
            };
        }

        public OapGraphicsController() : base()
        {
            OapGraphicClient = new OapGraphicClient(GetApiBaseUrl(), Client);
        }

        public async Task<ActionResult> Index()
        {
            var response = await OapGraphicClient.GetAllAsync(GetAllModelsCorp());
            return View("OapGraphic", response.Result.Data);
        }

        public async Task<ActionResult> Graphics()
        {
            var response = await OapGraphicClient.GetAllAsync(GetAllModelsCorp());
            return PartialView("OapGraphicPartial", response.Result.Data);
        }

        [HttpPost]
        public async Task<ActionResult> CreateGraphic(OapGraphic model)
        {            
            // TODO: Add insert logic here
            var response = await OapGraphicClient.AddGraphicAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateGraphic(OapGraphic model)
        {            
            // TODO: Add update logic here
            var response = OapGraphicClient.UpdateGraphicAsync(model).Result;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteGraphic(OapGraphic model)
        {
            if (model.Id > 0)
            {
                var response = await OapGraphicClient.DeleteGraphicAsync(model.Id);
            }
            return RedirectToAction("Index");
        }

        public ActionResult ImageUpload()
        {
            ContentResult imageContent = BinaryImageEditExtension.GetCallbackResult();
            return imageContent;
        }
    }
}

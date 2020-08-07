using AlbatrosSoft.Common.Web;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UploadGeoInfo.Models;
using UploadGeoInfo.Utils;

namespace UploadGeoInfo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadRouteFile()
        {
            ActionResult result = null;
            try
            {
                string errorMessage = string.Empty;
                string jsonResult = string.Empty;
                HttpPostedFileBase fileBase = this.Request.Files[0] as HttpPostedFileBase;
                if (fileBase.IsMapValidFile(out errorMessage))
                {
                    string fileName = string.Format(CultureInfo.InvariantCulture, "GeoFence{0}{1}", DateTime.Now.ToString("yyyyMMdd"), Path.GetExtension(fileBase.FileName));
                    string savedFileName = Path.Combine(Server.MapPath("~/DataFiles"), fileName);
                    FileUtils.DeleteTempFiles(savedFileName);
                    fileBase.SaveAs(savedFileName);
                    UploadFileResult uploadedFile = new UploadFileResult()
                    {
                        Name = fileName,
                        Length = fileBase.ContentLength,
                        FileType = fileBase.ContentType,
                        Directory = string.Format(CultureInfo.InvariantCulture, "/DataFiles/{0}", fileName)
                    };
                    //Obtener Json del resultado del cargue del archivo.
                    jsonResult = JsonHelper.Serialize<UploadFileResult>(uploadedFile);
                    result = this.Content(jsonResult, "application/json");
                }
                else
                {
                    //Devolver mensaje de error generado por archivo no valido
                    result = this.Json(new { error = errorMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                //Devolver mensaje de error por error desconocido subiendo archivo seleccionado.
                result = this.Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return result;
        }

        public ActionResult GetPreviewRouteFileData(string fileName)
        {
            ActionResult result = null;
            string inputFilePath = Path.Combine(this.Server.MapPath("~/DataFiles"), fileName);
            //El archivo existe.
            if (System.IO.File.Exists(inputFilePath))
            {
                try
                {
                    IEnumerable<GeofenceInfo> routes = this.GetGeofencesInfoFromFile(inputFilePath);
                    //Se cargaron rutas desde el archivo de origen
                    if (routes.Any())
                    {
                        this.ViewBag.TotalElements = routes.Count();
                        result = this.Json(new { geoFenceInfo = this.BuildImportGeofences(routes) }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        result = this.Json(new { errorMessage = "El archivo se encuentra vacio o es inválido." }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception exc)
                {
                    result = this.Json(new { errorMessage = string.Format(CultureInfo.InvariantCulture, "Error desconocido en lectura de archivo desde el servidor {0}", exc.Message) }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                result = this.Json(new { errorMessage = "El archivo no esta disponible para hacer vista previa." }, JsonRequestBehavior.AllowGet);
            }
            return result;
        }

        private IEnumerable<GeofenceInfo> GetGeofencesInfoFromFile(string inputFilePath)
        {
            List<GeofenceInfo> geofences = new List<GeofenceInfo>();
            using (TextReader trFileData = new StreamReader(inputFilePath))
            {
                //Informacion de archivo
                var fileDataInfo = trFileData.ReadToEnd();
                Parser parser = new Parser();
                parser.ParseString(fileDataInfo, false);
                //Obtener el root del archivo kml.
                Kml kmlElement = (Kml)parser.Root;
                //Iterar todos los elementos de tipo linestring del archivo kml
                foreach (var polygon in kmlElement.Flatten().OfType<Polygon>())
                {
                    //Obtener el elemento placemark asociado al linestring
                    var placemark = polygon.GetParent<Placemark>();
                    string routeName = placemark.Name.ToString();
                    //Obtener el elemento Document asociado al elemento Placemark
                    var document = placemark.GetParent<Document>();
                    string geofenceTypeName = document.Name.ToString();
                    var points = polygon.OuterBoundary.LinearRing.Coordinates.Select(e => new LocationPoint(e.Latitude, e.Longitude)).ToList();
                    //Crear objeto de ruta
                    GeofenceInfo route = new GeofenceInfo
                    {
                        GeofenceName = routeName,
                        GeofenceDescription = routeName,
                        GeofencePoints = points,
                        GeofenceTypeName = geofenceTypeName
                    };
                    geofences.Add(route);
                }
                return geofences;
            }
        }

        private IEnumerable<RouteInfo> GetRoutesInfoFromFile(string inputFilePath)
        {
            List<RouteInfo> routes = new List<RouteInfo>();
            using (TextReader trFileData = new StreamReader(inputFilePath))
            {
                //Informacion de archivo
                var fileDataInfo = trFileData.ReadToEnd();
                Parser parser = new Parser();
                parser.ParseString(fileDataInfo, false);
                //Obtener el root del archivo kml.
                Kml kmlElement = (Kml)parser.Root;
                //Iterar todos los elementos de tipo linestring del archivo kml
                foreach (var linestring in kmlElement.Flatten().OfType<LineString>())
                {
                    //Obtener el elemento placemark asociado al linestring
                    var placemark = linestring.GetParent<Placemark>();
                    string routeName = placemark.Name.ToString();
                    //Obtener el elemento Document asociado al elemento Placemark
                    var document = placemark.GetParent<Document>();
                    string routeTypeName = document.Name.ToString();
                    var points = linestring.Coordinates.Select(e => new LocationPoint(e.Latitude, e.Longitude)).ToList();
                    //Crear objeto de ruta
                    RouteInfo route = new RouteInfo
                    {
                        RouteName = routeName,
                        RouteDescription = routeName,
                        RoutePoints = points,
                        RouteTypeName = routeTypeName
                    };
                    routes.Add(route);
                }
                return routes;
            }
        }

        public string BuildImportRoutes(IEnumerable<RouteInfo> routes)
        {
            string jSonVehicleRoute = string.Empty;
            if (routes != null)
            {
                jSonVehicleRoute = JsonHelper.Serialize<List<RouteInfo>>(routes.ToList());
            }
            return jSonVehicleRoute;
        }

        public string BuildImportGeofences(IEnumerable<GeofenceInfo> routes)
        {
            string jSonVehicleRoute = string.Empty;
            if (routes != null)
            {
                jSonVehicleRoute = JsonHelper.Serialize<List<GeofenceInfo>>(routes.ToList());
            }
            return jSonVehicleRoute;
        }

    }
}
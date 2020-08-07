using System.Collections.Generic;

namespace UploadGeoInfo.Models
{
    public class RouteInfo
    {
        public string RouteName { get; set; }

        public string RouteDescription { get; set; }

        public List<LocationPoint> RoutePoints { get; set; }
        public string RouteTypeName { get; set; }

        public RouteInfo()
        {
            this.RoutePoints = new List<LocationPoint>();
        }
    }
}
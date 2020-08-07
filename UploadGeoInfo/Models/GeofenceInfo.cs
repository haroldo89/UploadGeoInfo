using System.Collections.Generic;

namespace UploadGeoInfo.Models
{
    public class GeofenceInfo
    {
        public string GeofenceName { get; set; }

        public string GeofenceDescription { get; set; }

        public List<LocationPoint> GeofencePoints { get; set; }

        public string GeofenceTypeName { get; set; }

        public GeofenceInfo()
        {
            this.GeofencePoints = new List<LocationPoint>();
        }
    }
}
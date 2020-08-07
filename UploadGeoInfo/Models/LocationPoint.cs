using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UploadGeoInfo.Models
{
    public class LocationPoint
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public LocationPoint(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
    }
}
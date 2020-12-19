using System;
using System.Collections.Generic;
using System.Text;

namespace Maps
{
    public class Marker
    {
        #region properties
        public string Description { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool ShowPopup { get; set; }
        #endregion

        public static Marker ToMapMarker(string description, LatLongGPS coords, bool showPopup) => new Marker
        {
            Description = description,
            X = coords.Longitude,
            Y = coords.Latitude,
            ShowPopup = showPopup
        };
    }
}

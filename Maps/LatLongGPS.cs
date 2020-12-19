using System;
using System.Collections.Generic;
using System.Text;

namespace Maps
{
    public class LatLongGPS
    {
        #region properties
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        #endregion

        public LatLongGPS() { }
        public LatLongGPS(double latitude, double longitude): this()
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        /// <summary>
        /// Locate the position by interpolate
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="proportion"></param>
        /// <returns></returns>
        public static LatLongGPS Interpolate(LatLongGPS start, LatLongGPS end, double proportion)
        {
            return new LatLongGPS(start.Latitude + (end.Latitude - start.Latitude) * proportion, 
                                  start.Longitude + (end.Longitude - start.Longitude) * proportion);
        }
                
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Maps
{
    public static class Helpers
    {
        /// <summary>
        /// Simulate Latitude and Longitude to return a position
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public static LatLongGPS SimulatePosition(int start)
        {
            Random random = new Random(start);
            double distance = 0.01 + random.NextDouble() * 0.02;
            double angle = random.NextDouble() * Math.PI * 2;
            double latitude = distance * Math.Cos(angle);
            double longitude = distance * Math.Sin(angle);
            return new LatLongGPS { Latitude = latitude, Longitude = longitude };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.algoritm
{
    public class RouteResult
    {
        public string stopCode { get; set; }
        public string stopName { get; set; }
        public double stopLon { get; set; }
        public double stopLat { get; set; }

        public string routeShortName{ get; set; }
        public string routeLongName{ get; set; }
        
        public RouteResult(string stopCode, string stopName, double stopLon, double stopLat, string routeShortName,
            string routeLongName)
        {
            this.stopCode = stopCode;
            this.stopName = stopName;
            this.stopLon = stopLon;
            this.stopLat = stopLat;
            this.routeShortName = routeShortName;
            this.routeLongName = routeLongName;

        }
    }
}

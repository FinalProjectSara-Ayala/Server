using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.algoritm
{
    public class StationInMat
    {
        public int score { get; set; }
        public int idStation { get; set; }
        public StationInMat LastStation { get; set; }
        public List<RouteAndStop> OptionalRoutes { get; set; }
        public RouteAndStop ChosenRout { get; set; }
        public StationInMat()
        {
            OptionalRoutes = new List<RouteAndStop>();
        }
    }
}

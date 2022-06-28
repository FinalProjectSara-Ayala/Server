using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.algoritm
{
    public class Route
    {

        public int id { get; set; }
        public string tripId { get; set; }
        
        public int numStops { get; set; }
        public DateTime lastDateUsed { get; set; }

        public Route()
        {

        }
        public Route(int id,string tripId,int numStops, DateTime lastDateUsed)
        {
            this.id = id;
            this.tripId = tripId;
           
            this.numStops = numStops;
           
                this.lastDateUsed = lastDateUsed;
        }
        public Route(int id, string tripId, int numStops)
        {
            this.id = id;
            this.tripId = tripId;
           
            this.numStops = numStops;
            
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.algoritm
{
    public class DictAndList
    {
        public Dictionary<DateTime, Dictionary<string,stopTimeAndSeq>> dict { get; set; }
        public List<DateTime> sortList { get; set; }
        public DictAndList()
        {
            dict=new Dictionary<DateTime, Dictionary<string, stopTimeAndSeq>>();
            sortList = new List<DateTime>();
        }
    }
    public class Station
    {
        public int StationIndex { get; set; }
        public int stopId { get; set; }

        public Station(int stopId)
        {
            this.stopId = stopId;
        }



    }
}

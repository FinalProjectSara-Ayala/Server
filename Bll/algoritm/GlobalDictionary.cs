using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.algoritm
{
    public class GlobalDictionary
    {
        public static Dictionary<int, DictAndList> dict1 { get; set; }
        public static Dictionary<string, Route> dictTrips { get; set; }
        public GlobalDictionary()
        {
            //קריאה לפונקציה שמאתחלת את המילון של זמני העצירה בתחנות של קווים שפועלים היום 
            dict1 = ClsDb.InitStopTimes();
            //קריאה לפונקציה שמאתחלת את המילון של הקווים
            dictTrips = ClsDb.InitTrips();
            foreach (var item in dict1)
            {
                item.Value.sortList= item.Value.sortList.OrderBy(p => p).ToList();
            }
           
        }
    }
}

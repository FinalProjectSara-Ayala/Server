using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.algoritm
{
    public class DataStructure
    {
        public  Dictionary<int,DictAndList> dict1 { get; set; }
        public Dictionary<string, Route> dictTrips { get; set; }

        public List<Station> stopList{ get; set; }
        //מקבלת קואורדינטות של פקח קו אורך וקו רוחב וזמני התחלה וסיום של משמרת
        public DataStructure(double lon, double lat)
        {
            //העתקת המילון הסטטי
            dict1 = new Dictionary<int, DictAndList>(GlobalDictionary.dict1);
            //העתקת המילון הסטטי
            dictTrips = new Dictionary<string, Route>(GlobalDictionary.dictTrips);
            //קריאה לפונקציה שמאתחלת את הרשימה של התחנות ברדיוס של הפקח
            stopList = ClsDb.GetStopsInRadius(lon, lat);
            List<Station> temp = new List<Station>();
            //סינון של  התחנות כי יש תחנות ללא זמני עצירה
            foreach (Station item in stopList)
            {
                if(!dict1.ContainsKey(item.stopId))
                {
                    temp.Add(item);
                }
            }
            //מחיקת התחנות שלא עוצרים בהם קווים ביום נוכחי
            while (temp.Count > 0)
            {
                stopList.Remove(temp[0]);
                temp.Remove(temp[0]);
            }
        }

    }
    
    
}

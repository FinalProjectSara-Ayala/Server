using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dto;
using System.Data.Entity.Spatial;
using Bll.algoritm;
using System.Diagnostics;
using GoogleMaps.LocationServices;

namespace Bll
{

    public static class ClsDb
    {

        //פונקציה שמקבלת זמני התחלה וסיום של משמרת
        //שמאתחלת מילון שהמפתח שלו הוא קוד תחנה 
        //והערך זה מחלקה שמכילה מילון :
        // שהמפתח שלו זה זמן עצירה והערך זה מילון של קווים
        // שהמפתח שלו זה קוד מסלול והערך פרטי קו -מסלול
        //וכן המחלקה מכילה רשימה של זמנים
        public static Dictionary<int, DictAndList> InitStopTimes()
        {

            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                Dictionary<int, DictAndList> dict1 = new Dictionary<int, DictAndList>();
                foreach (var item in db.GetStopTimesForDict1())
                {
                    if (item.arrival_time != null)
                    {
                        if (!dict1.ContainsKey(item.stop_id))
                            dict1.Add(item.stop_id, new DictAndList());
                        if (!dict1[item.stop_id].dict.ContainsKey(new DateTime() + item.arrival_time.Value))
                        {
                            dict1[item.stop_id].dict.Add(new DateTime() + item.arrival_time.Value,
                                new Dictionary<string, stopTimeAndSeq>());
                            dict1[item.stop_id].sortList.Add(new DateTime() + item.arrival_time.Value);
                        }
                        dict1[item.stop_id].dict[new DateTime() + item.arrival_time.Value].Add(
                            item.trip_id, new stopTimeAndSeq(item.trip_id, item.stop_sequence));
                    }
                    
                }
                return dict1;


            }


        }
        //  //item1.num_stops.Value
        //פונקציה שמאתחלת מילון שהמפתח שלו הוא קוד מסלול
        //והערך הוא פרטים על המסלול
        public static Dictionary<string, Route> InitTrips()
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                Dictionary<string, Route> dictTrips = new Dictionary<string, Route>();
                foreach (var item1 in db.GetTripsForDictInit())
                {
                    if (!dictTrips.ContainsKey(item1.trip_id) && item1.last_date != null && item1.last_time != null)
                        dictTrips.Add(item1.trip_id, new Route(item1.route_id, item1.trip_id, item1.num_stops.Value,
                            item1.last_date.Value + item1.last_time.Value));
                    else if (!dictTrips.ContainsKey(item1.trip_id))
                    {
                        dictTrips.Add(item1.trip_id, new Route(item1.route_id, item1.trip_id, item1.num_stops.Value));

                    }
                }

                return dictTrips;
            }

        }












        //הוספת שיבוץ של מסלול פקח 
        public static void AddSchduling(SchedulingDto s)
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                db.schedulingTbls.Add(s.DtoToDal());
                db.SaveChanges();
            }



        }
        //מחיקת פקח כולל פרטיו ופרטי משמרת שלו
        public static void DeleteInspector(string pass)
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                int id = -1;
                List<inspectorTbl> lst = db.inspectorTbls.ToList();
                foreach (inspectorTbl tbl in lst)
                    if (tbl.inspector_password == pass)
                        id = tbl.inspector_id;
                db.workHours.RemoveRange(db.workHours.Where(x => x.inspector_id == id).ToList());
                db.SaveChanges();
                db.inspectorTbls.Remove(db.inspectorTbls.FirstOrDefault(x => x.inspector_password == pass));
                db.SaveChanges();
            }


        }
        //הוספת פקח 
        public static void AddInspector(InspectorDto i)
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                string address = i.city + ' ' + i.street;
                var locationService = new GoogleLocationService("AIzaSyBcnAApbpvw9JdCVG6zy7GU3UR05lOSJOk");

                var point = locationService.GetLatLongFromAddress(address);
                double latitude = point.Latitude;
                double longitude = point.Longitude;
                i.inspector_lat=latitude;
                i.inspector_lon=longitude;
                db.inspectorTbls.Add(i.DtoToDal());
                db.SaveChanges();
            }


        }
        //בדיקה האם קיים פקח עם הסיסמא שהתקבלה
        public static int IsExistPass(string pass)
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                List<inspectorTbl> lst = db.inspectorTbls.ToList();
                foreach (inspectorTbl item in lst)
                {
                    if (item.inspector_password == pass)
                        return 1;
                }
                return 0;
            }

        }

        public static InspectorDto GetInspectorByPass(string pass)
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                List<inspectorTbl> lst = db.inspectorTbls.ToList();
                foreach (inspectorTbl item in lst)
                {
                    if (item.inspector_password == pass)
                        return InspectorDto.DalToDto(item);
                }
                return null;
            }

        }
        //שליפת כל הפקחים מטבלת פקחים
        public static List<InspectorDto> GetAllInspector()
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                List<inspectorTbl> lst = db.inspectorTbls.ToList();
                return (lst.Select(item => InspectorDto.DalToDto(item))).ToList();
            }

        }
        //הוספת משמרת לפקח 
        public static void AddWorkHour(workHourDto i)
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                db.workHours.Add(i.DtoToDal());
                db.SaveChanges();
            }


        }
        //שליפת כל המשמרות של פקח
        public static List<workHourDto>[] GetAllWorkHoursOfInspector(Pass pass)
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                List<workHourDto>[] arr = new List<workHourDto>[6];
                for (int i = 0; i < 6; i++)
                {
                    arr[i] = new List<workHourDto>();
                }
                int id = -1;
                List<inspectorTbl> lst = db.inspectorTbls.ToList();
                foreach (inspectorTbl tbl in lst)
                    if (tbl.inspector_password == pass.pass)
                        id = tbl.inspector_id;

                List<workHour> lst2 = db.workHours.ToList();
                foreach (workHour item in lst2)
                {
                    if (item.inspector_id == id)
                    {
                        arr[item.dayWork - 1].Add(workHourDto.DalToDto(item));
                    }
                }
                return arr;
            }

        }
        public static List<workHourDto> GetAllWorkHoursOfInspectorToday(Pass pass)
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                int id = -1;

                List<inspectorTbl> lst = db.inspectorTbls.ToList();
                foreach (inspectorTbl tbl in lst)
                    if (tbl.inspector_password == pass.pass)
                        id = tbl.inspector_id;
                List<workHourDto> lst1 = new List<workHourDto>();
                List<workHour> lst2 = db.workHours.ToList();
                foreach (workHour item in lst2)
                {
                    if (item.inspector_id == id && item.dayWork == (int)DateTime.Now.DayOfWeek + 1)
                    {
                        lst1.Add(workHourDto.DalToDto(item));
                    }
                }
                return lst1;
            }

        }
        //שליפת כל המשמרות של יום נוכחי
        public static List<WorkHourAndName> GetAllWorkHoursToday()
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {

                string name;
                List<inspectorTbl> lst = db.inspectorTbls.ToList();
                
                List<WorkHourAndName> lst1 = new List<WorkHourAndName>();
                List<workHour> lst2 = db.workHours.ToList();
                foreach (workHour item in lst2)
                {
                    if ( item.dayWork == (int)DateTime.Now.DayOfWeek + 1)
                    {
                        name = db.inspectorTbls.FirstOrDefault(x => x.inspector_id == item.inspector_id).inspector_name;
                        lst1.Add(new WorkHourAndName(item.id,name,item.dayWork,item.start_shift,item.stop_shift));
                    }
                }
                return lst1;
            }

        }

        //פונקציה שמחזירה את התחנות שברדיוס של הפקח
        public static List<Station> GetStopsInRadius(double LonArea, double LatArea)
        {
            List<Station> lst = new List<Station>();
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                List<stopsTbl> lst1 = db.stopsTbls.ToList();
                lst.AddRange(
                from item in lst1
                where item.stop_id != -1 &&
                CalcDistance(LonArea, LatArea, double.Parse(item.stop_lon.ToString()),
                double.Parse(item.stop_lat.ToString())) < 700
                select new Station(item.stop_id));
                return lst;
            }
        }

        //פונקציה שמחשבת מרחק בין שני קואורדינטות
        private static double? CalcDistance(double lonA, double latA, double lonB, double latB)
        {
            double longitudeA = lonA;
            double latitudeA = latA;
            double longitudeB = lonB;
            double latitudeB = latB;
            int coordinateSystemId = 4326;
            var pointA = DbGeography.FromText(string.Format("POINT ({0} {1})", longitudeA, latitudeA), coordinateSystemId);
            var pointB = DbGeography.FromText(String.Format("POINT ({0} {1})", longitudeB, latitudeB), coordinateSystemId);
            var distanceAB = pointA.Distance(pointB); //distanceAB = 80.6382796064941 metres
            return distanceAB;
        }
        //פונקציה שמחזירה את משמרות יום נוכחי של פקח על ידי קוד פקח
        public static List<workHourDto> GetShiftToInspector(int inspectorTd)
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                List<workHourDto> lst = new List<workHourDto>();
                DateTime dateTime = DateTime.Now;
                int dayOfWeek = (int)dateTime.DayOfWeek + 1;
                List<workHour> lst1 = db.workHours.ToList();
                //מעבר על טבלת המשמרות
                foreach (workHour item in lst1)
                {
                    // בדיקה אם המשמרת שייכת לפקח ואם מדובר במשמרת של היום
                    if (item.inspector_id == inspectorTd && item.dayWork == dayOfWeek)
                        lst.Add(workHourDto.DalToDto(item));
                }
                return lst;
            }

        }

        
        //פונקציה שמחזירה משמרת של פקח על ידי מזהה משמרת
        public static workHourDto GetWorkHourById(int? id)
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                List<workHour> lst = db.workHours.ToList();
                foreach (workHour item in lst)
                {
                    if (item.id == id)
                        return workHourDto.DalToDto(item);
                }
                return null;
            }

        }

        


        //שליפת שיבוץ אחד של מסלול של פקח
        //שזמן עצירה שלו הוא בטווח הזמן שבין זמן נוכחי לעוד שעה
        public static List<SchedulingDto> GetSchedulingForInspector(int inspector_id)
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                List<SchedulingDto> scheduling_list = new List<SchedulingDto>();
                List<schedulingTbl> scheduling_list1 = db.schedulingTbls.ToList();
                foreach (schedulingTbl item in scheduling_list1)
                {

                    if (inspector_id == item.inspector_id &&item.dateToday.Value.Date==DateTime.Now.Date&&
                        TimeSpan.Compare(item.arrival_time, DateTime.Now.TimeOfDay) >= 0 &&
                        TimeSpan.Compare(item.arrival_time, DateTime.Now.AddHours(1).TimeOfDay) <= 0)
                        scheduling_list.Add(SchedulingDto.DalToDto(item));
                }
                
                
                scheduling_list = scheduling_list
                            .OrderBy(p => p.arrival_time).ToList();
                return scheduling_list;
            }

        }
        public static List<SchedulingDto> GetSchedulingForInspectortoDelete(int workHourId)
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                List<SchedulingDto> scheduling_list = new List<SchedulingDto>();
                List<schedulingTbl> scheduling_list1 = db.schedulingTbls.ToList();
                foreach (schedulingTbl item in scheduling_list1)
                { 
                    if(item.workhour_id==workHourId)
                        scheduling_list.Add(SchedulingDto.DalToDto(item));
                }

                    scheduling_list = scheduling_list
                            .OrderBy(p => p.arrival_time).ToList();
                return scheduling_list;
            }
        }

        //של פקחים id שליפת כל ה
        //public static List<int> GetInspectorId()
        //{
        //    using (inspectorsKavimDBEntities15 db = new inspectorsKavimDBEntities15())
        //    {
        //        List<int> inspector_id = new List<int>();
        //        List<inspectorTbl> lst = db.inspectorTbls.ToList();
        //        foreach (inspectorTbl item in lst)
        //        {
        //            inspector_id.Add(item.inspector_id);
        //        }
        //        return inspector_id;
        //    }

        //}

        //שליפת כל הפקחים שעובדים היום
        public static List<InspectorDto> GetInspectorsOfToday()
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                List<InspectorDto> inspector_list = new List<InspectorDto>();
                List<workHour> workHourLst = db.workHours.ToList();
                foreach (workHour item in workHourLst)
                {
                    if (item.dayWork == (int)DateTime.Now.DayOfWeek + 1)
                    {
                        InspectorDto ins = GetSpecificInspector(item.inspector_id);
                        if(inspector_list.Count!=0)
                        {
                            InspectorDto ins1 = inspector_list.Where(x => x.inspector_id == ins.inspector_id).FirstOrDefault();
                            if (ins1 == null)
                            {
                                inspector_list.Add(ins);
                            }
                        }
                        else
                        {
                            inspector_list.Add(ins);
                        }
                       
                    }
                       
                }
                  
                inspector_list = inspector_list.OrderBy(x => x.area).ToList();


                return inspector_list;
            }

        }
        //id שליפת פקח לפי 
        public static InspectorDto GetSpecificInspector(int? id)
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                List<inspectorTbl> lst = db.inspectorTbls.ToList();
                foreach (inspectorTbl item in lst)
                {
                    if (item.inspector_id == id)
                        return InspectorDto.DalToDto(item);

                }
                return null;
            }

        }
        //מחיקת משמרת של פקח 
        public static void DropScheduling(int index, List<SchedulingDto> lst)
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                db.schedulingTbls.Remove((schedulingTbl)db.schedulingTbls.
                  Where(x => x.scheduling_id == lst[index].scheduling_id));
                db.SaveChanges();
            }



        }
        //החזרת זמן אחרון שפקח עלה על קו לזמן הקודם 
        public static void TakesBackTimeTripF(string trip_id)
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                List<tripsTbl> lst = db.tripsTbls.ToList();
                foreach (tripsTbl item in lst)
                {
                    if (item.trip_id == trip_id)
                    {
                        item.last_date = item.before_last_date;
                        item.last_time = item.before_last_time;
                        db.SaveChanges();
                    }
                }

                db.SaveChanges();
            }

        }
        //עדכון זמן אחרון של עלית פקח לקו
        //ושמירת הערכים הקודמים של עלית פקח למקרה של ביטול
        public static void updateTimeTripF(string trip_id, DateTime date, TimeSpan time)
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                List<tripsTbl> lst = db.tripsTbls.ToList();
                foreach (tripsTbl item in lst)
                {
                    if (item.trip_id == trip_id)
                    {
                        item.before_last_date = item.last_date;
                        item.before_last_time = item.last_time;
                        item.last_date = date;
                        item.last_time = time;
                        db.SaveChanges();
                    }
                }
            }
        }
        //// לקושמירת זמן קודם של עלית פקח 
        //public static void updateTimeTripBeforeF(string trip_id)
        //{
        //    using (inspectorsKavimDBEntities15 db = new inspectorsKavimDBEntities15())
        //    {
        //        db.updateTimeTripBefore(trip_id);
        //        db.SaveChanges();
        //    }

        //}

        //הפונ' מחזירה את המזהה של השורה המתאימה בטבלת משמרות בשביל לשמור 
        //scheduling  ב
        public static int GetWorkHourID(int inspectorId, TimeSpan start, TimeSpan stop)
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                List<workHour> lst = db.workHours.ToList();
                foreach (workHour item in lst)
                {
                    if (item.inspector_id == inspectorId && item.start_shift == start && item.stop_shift == stop &&
                        item.dayWork == (int)DateTime.Now.DayOfWeek + 1)
                        return item.id;
                }
                return -1;
            }

        }
        //שליפת כל הפקלים של היום לפי אזורים 
        public static List<List<InspectorDto>> GetAllInspectorTodaySeperate()
        {
            List<InspectorDto> inspctrsLst = GetInspectorsOfToday();
            List<List<InspectorDto>> lst = new List<List<InspectorDto>>();
            if (inspctrsLst.Count>0) {
                lst.Add(new List<InspectorDto>());
                lst[0].Add(inspctrsLst[0]);
                inspctrsLst.RemoveAt(0);
                int i = 0;
                //חלוקה של הפקחים לרשימות לפי אזורים
                foreach (InspectorDto inspctr in inspctrsLst)
                {
                    if (inspctr.area == lst[i][0].area)
                        lst[i].Add(inspctr);
                    else
                    {
                        lst.Add(new List<InspectorDto>());
                        lst[++i].Add(inspctr);
                    }
                }
            }            
            return lst;
        }


        public static List<RouteResult> GetRoute(int workHourId)
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                List<RouteResult> routes = new List<RouteResult>();
                List<schedulingTbl> lst = db.schedulingTbls.ToList();
                List<schedulingTbl> lst1=new List<schedulingTbl>();
                foreach (schedulingTbl sched in lst)
                {
                   
                    if (sched.workhour_id == workHourId && DateTime.Now.Date == sched.dateToday.Value.Date)
                    {
                        lst1.Add(sched);
                    }
                }
                lst1 = lst1.OrderBy(p => p.arrival_time).ToList();
                foreach (schedulingTbl item in lst1)
                {                
                        routsTbl r = db.routsTbls.FirstOrDefault(x => item.route_id == x.route_id);
                        stopsTbl s = db.stopsTbls.FirstOrDefault(x => item.stop_id == x.stop_id);
                        routes.Add(new RouteResult(s.stop_code, s.stop_name, s.stop_lon.Value, s.stop_lat.Value, r.route_short_name, r.route_long_name));
                    
                }
                return routes;
            }

        }
    public static List<CitiesDto>GetCiities()
        {
            using (inspectorsDBcopyEntities3 db = new inspectorsDBcopyEntities3())
            {
                List<CitiesDto> cities = new List<CitiesDto>();
                List<cityTbl> lst =db.cityTbls.ToList();
                foreach (cityTbl c in lst)
                {
                    cities.Add(CitiesDto.DalToDto(c));
                }
              return cities;
            }
               

        }
    }
}

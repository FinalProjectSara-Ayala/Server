using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using Dto;
namespace Bll.algoritm
{
     public class Dynamic
    {
        
        bool update;// משתנה בוליאני שמסמן האם האלגורחתם אמור להתבצע במצב של איחוראוטובוס או במצב רגיל 
        StationInMat updateStation;//אוביקט מסוג איברי המטריצה שבמידה ויש איחור של אוטובוס מכיל את נתוני התחנה שבה היה איחור וממנה אמורים למצוא מסלול חדש  
        List<RouteAndStop> sortArrOfTrips;//הגדרת הרשימה למיון
        DataStructure d; // אוביקט שמכיל את מבני הנתונים עבור האלגוריתם
        InspectorDto curInspector;//פקח נוכחי       
        DateTime startShift;//זמן התחלת משמרת       
        DateTime endShift; //זמן סיום משמרת       
        int numStation;//מספר התחנות
        //בפעם הראשונה while איתחול כדי שיכנס ל 
        //מיועד לבדיקה אם נשארו מסלולים שעדיין לא נגמרה המשמרת שלהם shift ה
        int Shift = 1;              
        List<StationInMat>[] matStations;//המטריצה שבה מתבצע האלגוריתם המטריצה היא מערך של רשימות
                
        List<RouteAndStop> finallyPath;//רשימת המסלול הסופי
        StationInMat maxRoute;//משתנה ששומר במהלך האלגוריתם את המסלול עם הניקוד הגבוה ביותר
        StationInMat longestRout;//משתנה ששומר במהלך האלגוריתם את המסלול עם האורך הארוך ביותר
        bool updated = false;// משתנה שמסמן במידה ונמצאים באלגוריתם בסטטוס של איחור האם כבר נאספו קווים אופציונליים לתחנה שממנה מתחיל המסלול 
        //ctor-
        //מקבל זמן התחלת משמרת וזמן סיום משמרת וכן פרטי פקח
        public Dynamic(DateTime startShift,DateTime endShift,InspectorDto inspector)
        {
            
            curInspector = inspector;
            finallyPath = new List<RouteAndStop>();
            this.startShift = startShift;
            this.endShift = endShift;
            //מופע למחלקה שמכילה את מבנה הנתונים עבור האלגוריתם
            //ושליחה לבנאי שנאתחל את מבני הנתונים
            d = new DataStructure(curInspector.inspector_lon.Value, curInspector.inspector_lat.Value);
           
             updateStation = new StationInMat();

        }


        private bool CheckTheRoute(int routeId,StationInMat stop) {
            
            while (stop != null)
            {
                if(stop.ChosenRout.RoutId== routeId)
                    return true;
                stop = stop.LastStation;
            }
            return false;
        }
        //איתחול המטריצה כל איבר בה הוא מסוג מחלקת 
        //StationInMat
        //המטריצה מורכבת ממערך של רשימות 
        //כל איבר במערך מייצג תחנה אחרת
        //מבין התחנות של רשימת התחנות
        public void InitMatrix()
        {
            numStation=d.stopList.Count;
            matStations = new List<StationInMat>[numStation];
            for (int i = 0; i < numStation; i++)
            {
                matStations[i] = new List<StationInMat>();
                //שמירת האינדקס במטריצה שמייצג אתהתחנה ברשימה 
                d.stopList[i].StationIndex = i;
                matStations[i].Add(new StationInMat());
                //שמירת קוד התחנה במטריצה
                matStations[i][0].idStation = d.stopList[i].stopId;
            }
        }
        private void CollectOptionalRoutes(int j,int i)
        {   //הפונקציה אוספת קווים אופציונליים לתחנה נוכחית 
            //  ומחשבת ניקוד לכל קו בתחנה נוכחית
            int id;
            DateTime time;
            //עדכון הקוד של תחנה נוכחית
            //וזמן עצירה שממנו יש להתחיל חיפוש קווים אופציונליים שעוצרים בתחנה
            if (update)//אם מדובר בתחנה שבה יש איחור וממנה מתחיל שינוי מסלול
            {
                id = updateStation.idStation;
                time = new DateTime(1, 1, 1, startShift.Hour, startShift.Minute, startShift.Second);
            }
            else if (i == 0 && !updated)//אם מדובר בתחנה ראשונה במסלול או בתחנה הבאה אחרי תנה שבה היה איחור
            {
                id = matStations[j][i].idStation;
                time = new DateTime(1, 1, 1, startShift.Hour, startShift.Minute, startShift.Second);
            }
            else
            {
                id = matStations[j][i].idStation;
                time = matStations[j][i].ChosenRout.StopTime;
            }
            DateTime time5 = time;
            time5 = time5.AddMinutes(15);
            int indx;
            //קריאה לפונקציה של חיפוש בינארי שמחזירה מיקום
            //במערך הממוין של זמנים של תחנה נוכחית של הזמן שמחפשים או הבא אחריו 
            indx = BinariSearchStopTime(d.dict1[id].sortList, time);
            //כל עוד המיקום שנמצא תקין- מיועד לאיטרציה הראשונה
            //וגם לא הזמן לא גלש מטווח זמן של רבע שעה 
            while (indx >= 0 && time < time5)
            {
                if (indx < d.dict1[id].sortList.Count && d.dict1[id].sortList[indx] < time5)
                {
                    time = d.dict1[id].sortList[indx];
                    //מעבר על הקווים שעוצרים בזמן שנמצא בתחנה נוכחית 
                    foreach (var rout in d.dict1[id].dict[time])
                    {
                        //בדיקה על מנת למנוע מצב שירדו מקו אחד ויעלו עליו באותו מסלול                     
                        if (matStations[j][i].ChosenRout == null ||
                          CheckTheRoute(d.dictTrips[rout.Key].id, matStations[j][i])==false)
                        {
                            RouteAndStop op_rout = new RouteAndStop();
                            //קריאה לפונקציה שמחשבת ניקוד לקו
                            int score = CalcScoreForRout(d.dictTrips[rout.Key], rout.Value.stopSequence);
                            op_rout.Score = score;
                            //הוספת הניקוד של התחנה שמייצג את הניקוד של כל המסלול עד אליה לניקוד של הקו
                            op_rout.Score += matStations[j][i].score;
                            op_rout.StopTime = time;
                            op_rout.RoutId = d.dictTrips[rout.Key].id;
                            op_rout.TripId = rout.Key;
                            //עדכון המיקום במטריצה של התחנה שממנה יוצא הקו
                            if (update)
                            {
                                op_rout.StopIndex2 = -1;
                                op_rout.StopIndex1 = -1;
                                updateStation.OptionalRoutes.Add(op_rout);
                            }
                            else
                            {
                                op_rout.StopIndex2 = i;
                                op_rout.StopIndex1 = j;
                                matStations[j][i].OptionalRoutes.Add(op_rout);
                            }
                        }
                    }
                }
                time = time.AddMinutes(1);
                indx++;
            }
        }
        private void CompairMaxScoreAndLongest(int r, int c)
        {
            //אם זה המסלול הראשון שהסתיים או שהמסלול הזה בעל ניקוד גבוה יותר מהמקס
            //או שיש לו אותו ניקוד אבל הוא יותר ארוך
            //למסלול זה = max_rout
            //אחרת הוא לא משתנה
            maxRoute = maxRoute == null ||
                                maxRoute.score < matStations[r][c].score ||
                               (maxRoute.score == matStations[r][c].score && maxRoute.ChosenRout.StopIndex2 < c) ?
                               matStations[r][c] : maxRoute;
            //אם זה המסלול הראשון שהסתיים או שהמסלול הזה ארוך יותר מהמקס
            //או שהוא באותו אורך אבל יש לו ניקוד גבוה יותר
            //למסלול זה = longest_rout
            //אחרת הוא לא משתנה
            longestRout = longestRout == null ||
                longestRout.ChosenRout.StopIndex2 < matStations[r][c].ChosenRout.StopIndex2 ||
                (longestRout.ChosenRout.StopIndex2 == matStations[r][c].ChosenRout.StopIndex2 &&
                longestRout.score < matStations[r][c].score) ?
                 matStations[r][c] : longestRout;
        }
        private void FindTheChoosenRoute(int i, int j)
        {
            //  מעבר על המערך שמכיל את הקווים האופציונליים מהאיטרציה הקודמת
            //  כל עוד לא נמצא מסלול אפשרי להגיע איתו לתחנה נוכחית
            for (int k = sortArrOfTrips.Count - 1; k >= 0; k--)
            {
                //שליפת המסלול עם הניקוד הגבוה ביותר
                //שליחה לפונקציה שבודקת האם אפשר להגיע על ידי מסלול המקס לתחנה נוכחית
                //null אם אפשר להגיע הפונ' מחזירה את המסלול אחרת 
                RouteAndStop max_rout = sortArrOfTrips[k];
                RouteAndStop possible_rout = IsPossible(max_rout, matStations[j][0].idStation);
                if (possible_rout != null)//אם אפשר להגיע
                {
                    // possible_rout  המסלול שנבחר להגיע איתו לתחנה נוכחית הוא 
                    //מעדכנים את השדות המתאימים באיבר הנוכחי במטריצה כלומר בתחנה נוכחית
                    possible_rout.StopIndex1 = j;
                    possible_rout.StopIndex2 = i;
                    matStations[j][i].ChosenRout = possible_rout;                   
                    //אם התחנה הקודמת זו התחנה שבה היה איחור וממנה מתחיל שינוי של מסלול אז היא נחשבת תחנה ראשונה
                    //ויש לשמור את זמן העליה לאוטובוס בתחנה זו 
                    //וכן לעדכן שהתחנה הקודמת היא תחנת האיחור
                    if (max_rout.StopIndex2 == -1)
                    {
                        matStations[j][i].LastStation = updateStation;
                        matStations[j][i].LastStation.ChosenRout = new RouteAndStop();
                        matStations[j][i].LastStation.ChosenRout.StopTime = max_rout.StopTime;
                        matStations[j][i].LastStation.ChosenRout.StopIndex1 = max_rout.StopIndex1;
                    }                       
                    else
                        matStations[j][i].LastStation = matStations[max_rout.StopIndex1][max_rout.StopIndex2];
                    //אם התחנה הקודמת זו תחנה ראשונה יש לשמור את זמן העליה לאוטובוס בתחנה זו
                    if (max_rout.StopIndex2 == 0)
                    {
                        matStations[j][i].LastStation.ChosenRout = new RouteAndStop();
                        matStations[j][i].LastStation.ChosenRout.StopTime = max_rout.StopTime;
                        matStations[j][i].LastStation.ChosenRout.StopIndex1 = max_rout.StopIndex1;
                    }                                       
                    //עדכון הניקוד של תחנה נוכחית לניקוד של המסלול שנעצר בה
                    matStations[j][i].score= possible_rout.Score;
                    break;
                }
            }
        }
        //מעבר על המטריצה לחישוב המסלול המתאים ביותר לפקח
        public void PassMat(bool update, int stationforchnge)
        {            
            this.update= update;
            if (stationforchnge != -1)//אם יש איחור של אוטובוס ותחנה ממנה צריך להתחיל את המסלול
                updateStation.idStation = stationforchnge;                        
             
            int i = 0;//מייצג את אורך המסךוך          
            while (Shift > 0) //כל עוד יש מסלול שלא הסתיימה המשמרת שלו
            {
                Shift = numStation;
                List<RouteAndStop> arr = new List<RouteAndStop>(); //מערך עזר לשמירת הקווים האופציונליים             
                for (int j = 0; j < this.numStation; j++)//מעבר על התחנות
                {
                    //אם זו לא תחנה ראשונה במסלול
                    //או שהיה איחור והתחנה שבה היה האיחור טופלה כלומר נאספו ממנה קווים אופציונליים
                    if (i != 0||updated) 
                    {
                        matStations[j].Add(new StationInMat());
                        //סימון הפרמטרים של עדכון לסטטוס שהיה כבר איסוף קווים אופציונליים
                        //מהתחנה שבה התבצע איחור וכעת כל התחנות ממשיכות מסלול מתחנה זו
                        if (update)
                        {
                            update = false;
                            updated = true;
                            i--;
                        }
                        else                       
                            matStations[j][i].idStation = matStations[j][0].idStation;
                        //i במסלולים באורך j פונקציה שמחפשת מסלול אפשרי להגיע איתו לתחנה
                        FindTheChoosenRoute(i, j);
                    }
                    //בדיקה אם אוחזים כעת במצב של איחור 
                    //או שנמצאים בתחנה ראשונה ואין איחור
                    //או שנמצא קו להגיע איתו לתחנה נוכחית ולא נגמר זמן המשמרת                 
                    if (update || (i == 0 && !updated) || matStations[j][i].ChosenRout != null &&
                        matStations[j][i].ChosenRout.StopTime < endShift)
                    {
                        //הפונקציה אוספת קווים אופציונליים לתחנה נוכחית 
                        //  ומחשבת ניקוד לכל קו בתחנה נוכחית
                        CollectOptionalRoutes(j,i);     
                        //הוספת הקווים האופציונליים שנמצאו למערך העזר של הקווים
                        if (update && updateStation.OptionalRoutes != null)
                        {
                            arr.AddRange(updateStation.OptionalRoutes);
                            break;                            
                        }
                        else if (matStations[j][i].OptionalRoutes != null&& matStations[j][i].OptionalRoutes.Count>0)
                            arr.AddRange(matStations[j][i].OptionalRoutes);
                    }
                    else
                    {                       
                        Shift--;//ירדה אופציה של מסלול- אי אפשר להמשיך ממנו
                        //אם תחנה נוכחעת ממשיכה מסלול כלשהו
                        //שולחים לפונקציה שבודקת אם מסלול זה הוא האופטימלי בשלב הזה                        
                        if (matStations[j][i].ChosenRout != null)
                        {
                            CompairMaxScoreAndLongest(j,i);
                        }
                    }                   
                }
                //מיון מערך האוטובוסים
                sortArrOfTrips = arr;
                sortArrOfTrips.Sort((x, y) => x.Score.CompareTo(y.Score));
                i++;
            }           
            FindingTheFinalRoute();//מעבר על המטריצה לפי המסלול הסופי ושמירת הנתונים במסד הנתונים
        }


        private int CalcScoreForRout(Route route, int seqStop)
        {
            int score = 0;
            if (DateTime.Now.Month > route.lastDateUsed.Month)
                score += 10;
            else if (DateTime.Now.Day > route.lastDateUsed.Day)
                score += 2;
            else 
                score -=60;
            double seq = (double)seqStop / (double)route.numStops > 0.5 ? 10 - (10 * ((double)seqStop / (double)route.numStops)) :
                10 * ((double)seqStop / (double)route.numStops);
            return score + (int)seq;
        }

        
        //פונקציה שמבצעת חיפוש בינארי משודרג
        //היא מחזירה את האיבר שמחפשים או את הגדול ממנו הקרוב אליו ביותר
        private int BinariSearchStopTime(List<DateTime> arr, DateTime time)
        {           
            int n = arr.Count;
            if (n == 0)
                return -1;
            //בדיקה אם הזמן קטן מכל איברי המערך
            if (time <= arr[0])
                return 0;
            //בדיקה אם הזמן גדול מכל איברי המערך
            if (time > arr[n - 1])
                return -1;
            //ביצוע חיפוש בינארי
            int i = 0, j = n, mid = 0;
            while (i < j)
            {
                mid = (i + j) / 2;
                if (arr[mid] == time)
                    return mid;
                //אם הזמן קטן מהאיבר האמצעי חפש בתת המערך השמאלי
                if (time < arr[mid])
                {
                    //אם הזמן גדול מהקודם לאיבר האמצעי
                    //תחזיר את האיבר האמצעי כי הוא האיבר הגדול מהזמן 
                    //הקרוב אליו ביותר                    
                    if (mid > 0 && time > arr[mid - 1])
                        return mid;
                    //חזור על הפעולה בחצי המערך השמאלי                    
                    j = mid;
                }
                //אחרת אם הזמן גדול מהאיבר האמצעי               
                else
                {
                    if (mid < n - 1 && time < arr[mid + 1])
                        return mid + 1;
                    i = mid + 1;
                }
            }
            //נשאר איבר בודד 
            return mid;
        }



        //ניסוח אחר- בדיקה האם אפשר להגיע לתחנה נוכחית על ידי קו המקס
        //בדיקה האם אפשר להגיע מתחנה עם מסלול המקס לתחנה נוכחית
        private RouteAndStop IsPossible(RouteAndStop maxRout, int idStation)
        {
            //הפונ' מקבלת קו שמסיים מסלול ותחנה נוכחית
            RouteAndStop result = new RouteAndStop();
            //משתנה שמייצג את זמן העצירה שאותו מחפשים
            DateTime start = maxRout.StopTime;
            start = start.AddMinutes(5);
            //משתנה שמייצג את זמן העצירה המאוחר ביותר שמתאפשר
            DateTime stop = maxRout.StopTime;
            stop = stop.AddMinutes(30);
            //שליחה לחיפוש בינארי שמחזיר את האינדקס במערך של זמן העצירה שחיפשו
            //או את זמן העצירה הגדול הקרוב אליו ביותר
            int indSt = BinariSearchStopTime(d.dict1[idStation].sortList, start);
            //תנאי ראשון מיועד למקרה שלא קיים זמן עצירה מתאים במערך
            //תנאי שני הוא התקדמות כל עוד אין גלישה מהמערך
            //תנאי שלישי הוא התקדמות כל עוד לא היתה גלישה מטווח הזמן  
            while (indSt != -1 &&
                indSt < d.dict1[idStation].sortList.Count &&
                d.dict1[idStation].sortList[indSt] <= stop)
            {
                // בדיקה האם קו זה עוצר בתחנה נוכחית 
                if (d.dict1[idStation].dict[d.dict1[idStation].sortList[indSt]].ContainsKey(maxRout.TripId))
                {                    
                    result.StopTime = d.dict1[idStation].sortList[indSt];
                    result.Score = maxRout.Score;
                    result.RoutId = maxRout.RoutId;
                    result.TripId = maxRout.TripId;
                    return result;
                }
                indSt++;
            }
            return null;
        }


        private void FindingTheFinalRoute()
        {
            //מטרת לולאה זו היא בשביל המקרה שבפעם האחרונה של 
            //לכל המשבצות לא ימצאו אוטובוסים מתאימים  pass mat 
            //וזה אומר שהמסלולים יגמרו איטרציה אחת קודם ולכן 
            //אני עוברת על האיטרציה הקודמת ובוחרת משם את המסלול הטוב ביותר מביניהם או מהמסלול שכבר שמור 
            //maxRoute , longstRoute במשתנה    
            for (int i = 0; i < this.numStation; i++)
            {
                    if (matStations[i][matStations[i].Count - 2].ChosenRout != null)                   
                       CompairMaxScoreAndLongest(i, matStations[i].Count - 2);
            }           
            int max, longest;
            StationInMat cur_station;
            //אם הניקוד שווה 
            if (maxRoute.score == longestRout.score)
                cur_station = maxRoute;
            else//אם הניקוד של מקס יותר גדול אבל האורך של לונג יותר ארוך
            {
                max = maxRoute.score - longestRout.score;
                longest = maxRoute.ChosenRout.StopIndex2 - longestRout.ChosenRout.StopIndex2;
                //אם ההפרש בניקוד גדול מעשר לטובת המקס 
                //ווהפרש באורך הוא פחות מחמש תחנות
                if (max > 10 && longest < 5)
                    cur_station = maxRoute;
                else
                    cur_station = longestRout;
            }
            while (cur_station != null)
            //הכנסת המסלול הנבחר לרשימה מהסוף להתחלה
            { //תנאי העצירה הוא כשמגיעים לתחנה הראשונה ואין תחנה קודמת
                finallyPath.Add(cur_station.ChosenRout);
                cur_station = cur_station.LastStation;
            }
            //sql שמירת המסלול ב
            //ברשימה זו שמורים התחנה עם הקו שעוצר בה 
            //ואני שומרת את הנתונים בצורה שהתחנה שמורה עם הקו שאליו עולים ממנה
            // בשביל שליפה נוחה בריאקט
            TimeSpan st = startShift - new DateTime(startShift.Year, startShift.Month, startShift.Day, 0, 0, 0);
            TimeSpan et = endShift - new DateTime(endShift.Year, endShift.Month, endShift.Day, 0, 0, 0);
            int workHourId = ClsDb.GetWorkHourID(curInspector.inspector_id, st, et);
            foreach (RouteAndStop item in finallyPath)
            {
                if (matStations[item.StopIndex1][item.StopIndex2].LastStation!=null)
                {
                    DateTime arrivalDateTime = new DateTime(item.StopTime.Year, item.StopTime.Month, item.StopTime.Day, 0, 0, 0);
                    SchedulingDto s = new SchedulingDto(curInspector.inspector_id, item.RoutId, item.TripId,
                         matStations[item.StopIndex1][item.StopIndex2].LastStation.idStation,
                          item.StopTime - arrivalDateTime, workHourId,DateTime.Now);
                    //שמירה במסד הנתונים
                    ClsDb.AddSchduling(s);
                    //עדכון תאריך אחרון שפקח עלה על קווים אלו
                    DateTime d=new DateTime(DateTime.Now.Year, DateTime.Now.Month,DateTime.Now.Day,0,0,0);
                    ClsDb.updateTimeTripF(s.trip_id, d, s.arrival_time);
                    
                }
                
            }
        }
    }
}

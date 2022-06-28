using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using System.Xml.Linq;
using Dto;
namespace Bll.algoritm
{
    public class RealTime
    {
       

        
        public RealTime()
        {
        }

        //עוברים על רשימת הפקחים ולכל פקח שולפים רשימה של תחנות וקווים שיעבור בהם בשעה הקרובה ושולחים לבדיקה אם יש איחור 
        private void InitInspectorsSchduling(List<InspectorDto> lst1)
        {
            //מעבר על הרשימה של הפקחים
            List<SchedulingDto> schdlInspctr;           
                for (int i = 0; i < lst1.Count; i++)
                {
                //שליפת תחנות וקוים של פקח שיעבור בהם בשעה הקרובה
                    schdlInspctr = ClsDb.GetSchedulingForInspector(lst1[i].inspector_id);
                    //בדיקה אם לפקח יש נסיעות בשעה הקרובה
                    if(schdlInspctr.Count>0)
                        SendToTransportAndUpdate(schdlInspctr);


                }
        }
        //פונקציה שמקבלת רשימה של קווים
        //ושולחת לפונקציה שבודקת שקו זה אינו מאחר 
  
        //אם יש איחור שולחת לאלגוריתם שיבנה מסלול חדש החל מהתחנה שממנה יוצא הקו שמאחר
        private void SendToTransportAndUpdate(List<SchedulingDto> shiftSchedulingin)
        {
            //מעבר על רשימה של פקח
            for (int i = 0; i < shiftSchedulingin.Count; i++)
            {
                int k;
                bool fromTrnsprt;
                DateTime timeToTrnsprt =  DateTime.Now.Date+ shiftSchedulingin[i].arrival_time;
                int stop_to_trnsprt = shiftSchedulingin[i].stop_id;   
                //קריאה לפונקציה שבודקת אם יש איחור של אוטובוס
                fromTrnsprt = DataFromTransport(stop_to_trnsprt, (int)shiftSchedulingin[i].route_id, timeToTrnsprt);
                if (!fromTrnsprt)
                {
                    workHourDto h= ClsDb.GetWorkHourById(shiftSchedulingin[i].workhour_id);
                    InspectorDto inspctr = ClsDb.GetSpecificInspector(shiftSchedulingin[i].inspector_id);
                    Dynamic dinamic = new Dynamic(timeToTrnsprt,new DateTime()+ h.stop_shift, inspctr);
                    List<SchedulingDto> s = ClsDb.GetSchedulingForInspectortoDelete(shiftSchedulingin[i].workhour_id.Value);
                    for ( k = 0; k < s.Count; k++)
                    {
                        if (s[k] == shiftSchedulingin[i])
                            break;
                    }
                    //עדכון תאריך אחרון לעלית פקח לתאריך הקודם ששמור במערכת
                    for (int j =k; j < s.Count; j++)
                    {
                        ClsDb.TakesBackTimeTripF(s[j].trip_id);
                        ClsDb.DropScheduling(j, s);
                    }
                    //קריאה לפונקציות האלגוריתם לחישוב המשך מסלול חדש 
                    //החל מהתחנה שבה אוטובוס מאחר
                    dinamic.InitMatrix();
                   dinamic.PassMat(true, stop_to_trnsprt);
                    break;
                }
            }
        }

        //פונקציה שבודקת כל שעה האם יש איחור של אוטובוסים
        //שפקחים אמורים לעבור בהם בשעה הקרובה
        public void CheckForBussDelay(object sender, ElapsedEventArgs e)
        {
            //פעם בשעה מאתחלים את מבנה הנתונים של הפקחים והתחנות שהם 
            //עוברים בהם
            //עוברים על הרשימות של הפקחים 
            //ועל ידי שליחה לממשק זמן אמת של הרשות 
            //לתחבורה ציבורית בודקים האם יש איחור בטווח של יותר מרבע שעה
            //בזמני ההגעה של האוטובוס לתחנה
            //אם כן יש לחשב מסלול חדש לפקח מנקודת המקום שבו יש איחור
            DateTime curDate = DateTime.Now;
            if (curDate.Hour >= new DateTime(1, 1, 1, 6, 0, 0).Hour &&
                curDate.Hour <= new DateTime(1, 1, 1, 23, 0, 0).Hour)
            {
                //אתחול רשימה שבכל איבר בה פקחים של אזור עבודה מסוים
                List<List<InspectorDto>> lst = ClsDb.GetAllInspectorTodaySeperate();                
                //יצירת סרדים לכל אזור
                foreach (List<InspectorDto> item in lst)
                {
                    Task task = Task.Run(() =>
                    {
                        InitInspectorsSchduling(item);
                });
            }
            }

        }

        //PreviewInterval-טווח הזמן שבו יכללו ביקורי עצירה
        //StartTime -שעת התחלה של חלון זמן PreviewInterval.
        //MonitoringRef-קוד עצירה, תחנה
        //LineRef- מסנן את התגובות רק עבור נסיעות עם לייןרפ המבוקש
        
        

        // ובודקת האם התקבלו מהאתר זמני עצירה xml פונקציה העוברת על ה 
        private static bool isExpectedArrivalTime(string data)
        {
            XDocument xmlDoc = XDocument.Parse(data);
            XNamespace ns = "http://www.siri.org.uk/siri";
            var expectedArrivalTime = xmlDoc.Descendants(ns + "ExpectedArrivalTime");
            return expectedArrivalTime.Count() != 0;
        }

        //פונקציה ששולחת לאתר 
        // של הרשות לתחבורה ציבורית ובודקת האם קו אינו מאחר כלומר,
        // יגיע לתחנה מבוקשת ברבע שעה הקרובה החל מהזמן שבו אמור לעצור
        
        private bool DataFromTransport(int stop, int trip, DateTime time)
        {
            //שליחת בקשה לאתר באמצעות
            //של האתר  עם הפרמטרים  URL כתובת 
            //של קוד תחנה, קוד קו, הזמן שבו הקו אמור להגיע לתחנה
            //וטווח של רבע שעה
            //האתר מחזיר כתשובה את כל זמני העצירה של הקו בתחנה החל מהזמן שנשלח במשך בטווח שנשלח 
            string dateFormat = "" + time.Year + time.Month.ToString().PadLeft(2, '0') + time.Day.ToString().PadLeft(2, '0') +
                "T" + time.Hour.ToString().PadLeft(2, '0') + time.Minute.ToString().PadLeft(2, '0') + time.Second.ToString().PadLeft(2, '0');
            string urlAddress = "http://moran.mot.gov.il:110/Channels/HTTPChannel/SmQuery/2.8/xml?Key=AR36156187&MonitoringRef="+
                stop+"&LineRef="+trip+"&PreviewInterval=PT15M&StartTime="+dateFormat;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null)
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                string data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                //לאחר קבלת תשובה מהאתר 
                //שחזר וחיפוש אחר תגית  XML/מעבר על ה
                if(isExpectedArrivalTime(data))
                    return true;
                return false;
            }
            return false;
        }

        }
    }

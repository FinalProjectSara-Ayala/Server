using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Dto;
namespace Bll.algoritm
{
    public class Algorithm
    {
        //פונקציה שעוברת על רשימת פקחים באזור מסוים
        //ןלכל פקח שולחת לפונקציה שמוצאת מסלול עבודה למשמרות 
        //של הפקח
       public static void TreatForArea(List<InspectorDto> inspectorLst)
       {
            //מעבר על רשימת הפקחים שהתקבלה 
            foreach (InspectorDto inspctr in inspectorLst)
            {
                //שליפת המשמרות של הפקח ביום נוכחי
                List<workHourDto> workHour;
                workHour = ClsDb.GetShiftToInspector(inspctr.inspector_id);
                //מעבר על משמרות יום נוכחי של פקח
                foreach (workHourDto item in workHour)
                {
                    // Dynamic הגדרת מופע ושליחה לבנאי של מחלקת 
                    Dynamic d = new Dynamic(new DateTime() + item.start_shift, new DateTime() + item.stop_shift, inspctr);
                    //איתחול המטריצה שבה מתבצע האלגוריתם 
                    d.InitMatrix();
                    //קריאה לפונקציה שמוצאת מסלול למשמרת 
                    d.PassMat(false, -1);
                }
            }

       } 
        //פונקציה שפועלת אוטומטי פעם ב24 שעות
        //היא מפעילה את כל האלגוריתם 
        public static void MainAlgorithm(object sender, ElapsedEventArgs e)
        {
            //איתחול המבנה הגלובלי
            GlobalDictionary d = new GlobalDictionary();
            //שליפת כל הפקחים שעובדים היום
            //וחלוקתם לרשימות לפי אזורים
            List<List<InspectorDto>> lst;
            lst=ClsDb.GetAllInspectorTodaySeperate();    
            //מעבר על הפקחים שעובדים היום 
            //יצירת סרדים שיעבדו במקביל לאזורים שונים
            //בכל סרד שליחה לפונקציה שתמצא מסלולים מתאימים לפקחים באזור המסוים
            foreach (List<InspectorDto> inspctr in lst)
            {
                Task task = Task.Run(() =>
                {
                    TreatForArea(inspctr);

                });
        }
        }
    }
}

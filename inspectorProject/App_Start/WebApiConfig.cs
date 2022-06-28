using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web.Http;
using Bll;
using Bll.algoritm;
using Microsoft.AspNetCore.Cors;
namespace inspectorProject
{
    public static class WebApiConfig
    {
        
        public static void Register(HttpConfiguration config)
        {

            //אתחול טיימר שיפעל כל 24 שעות
            //const double intervalEveryDay = 24 * 60 * 60 * 1000;
            //Timer checkForTime = new Timer(intervalEveryDay);
            //checkForTime.Elapsed += new ElapsedEventHandler(Algorithm.MainAlgorithm);
            //checkForTime.Enabled = true;
            ////אתחול טיימר שיופעל כל שעה
            //const double intervalEveryDay1 = 60 * 60 * 1000;
            //Timer checkForTime1 = new Timer(intervalEveryDay1);
            //RealTime r = new RealTime();
            //checkForTime.Elapsed += new ElapsedEventHandler(r.CheckForBussDelay);
            //checkForTime.Enabled = true;

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.EnableCors();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
           
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Bll;
using Bll.algoritm;
using Dto;
using Microsoft.AspNetCore.Cors;


namespace inspectorProject.Controllers
{
    [System.Web.Http.Cors.EnableCors(origins: "*", headers: "*", methods: "*")]
    public class InspectorController : ApiController
    {
        // GET: api/Inspector
        public List<InspectorDto> Get()
        {
              // RealTime r=new RealTime();
              //  r.CheckForBussDelay();
            
         //Algorithm.MainAlgorithm();
           
            return ClsDb.GetAllInspector();
        }

        // GET: api/Inspector/5
        public void Get(string password)
        {
            ClsDb.DeleteInspector(password);
        }

        [HttpGet]
        [Route("api/Inspector/GetCities")]
        public List<CitiesDto> GetCities()
        {
            return ClsDb.GetCiities();

        }
            // POST: api/Inspector
            public void Post(InspectorDto data)
        {
            
            ClsDb.AddInspector(data);
        }
        [HttpPost]
        [Route("api/Inspector/PostDelete")]
        public void PostDelete(Pass password)
        {
            ClsDb.DeleteInspector(password.pass);
        }


        // PUT: api/Inspector/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Inspector/5
        public void Delete(int id)
        {
        }
    }
}

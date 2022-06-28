using Bll;
using Bll.algoritm;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace inspectorProject.Controllers
{
    [System.Web.Http.Cors.EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ResultController : ApiController
    {
        // GET: api/Result
        public List<WorkHourAndName> Get()
        {
          return ClsDb.GetAllWorkHoursToday();

        }

        // GET: api/Result/5
        public string Get(int id)
        {
           
            return "value";
        }

        // POST: api/Result
        public List<RouteResult> Post(Pass idWork)
        {
            return ClsDb.GetRoute(Int32.Parse(idWork.pass));
        }

        [HttpPost]
        [Route("api/Result/PostResult")]
        public List<RouteResult> PostResult(Pass idWork)
        {
            //return ClsDb.GetShiftToInspector(4);
            return ClsDb.GetRoute(Int32.Parse(idWork.pass));
        }

        
        // PUT: api/Result/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Result/5
        public void Delete(int id)
        {
        }
    }
}

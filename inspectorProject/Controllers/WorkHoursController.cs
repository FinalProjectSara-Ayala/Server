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
    public class WorkHoursController : ApiController
    {
        // GET: api/WorkHours
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/WorkHours/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/WorkHours
        public List<workHourDto> Post(Pass password)
        {
            return ClsDb.GetAllWorkHoursOfInspectorToday(password);

        }

        [HttpPost]
        [Route("api/WorkHours/PostAllWorkHours")]
        public List<workHourDto>[] PostAllWorkHours(Pass password)
        {

            return ClsDb.GetAllWorkHoursOfInspector(password);
        }

        [HttpPost]
        [Route("api/WorkHours/PostWorkHoursToInspectorToday")]
        public List<workHourDto> PostWorkHoursToInspectorToday(int id)
        {

            return ClsDb.GetShiftToInspector(id);
        }
        // PUT: api/WorkHours/5
        public void Put(int id, [FromBody]string value)
        {
            
        }

        // DELETE: api/WorkHours/5
        public void Delete(int id)
        {
        }
    }
}

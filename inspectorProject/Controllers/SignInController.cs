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
    public class SignInController : ApiController
    {
        // GET: api/SignIn
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/SignIn/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/SignIn
        public int Post(Pass data)
        {
            return ClsDb.IsExistPass(data.pass);
        }
        [HttpPost]
        [Route("api/SignIn/PostInspectorByPass")]
        public InspectorDto PostInspectorByPass(Pass data)
        {
            return ClsDb.GetInspectorByPass(data.pass);
        }

        // PUT: api/SignIn/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SignIn/5
        public void Delete(int id)
        {
        }
    }
}

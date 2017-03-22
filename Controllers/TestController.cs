using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace aspnetcore_rest_api.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [HttpPostAttribute]
        [AuthorizeAttribute]
        public string getData()
        {
            return "Success";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using aspnetcore_rest_api.Data;

namespace aspnetcore_rest_api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        
        public UserController(AppDbContext context)
        {
            _context = context;
        }
        [RouteAttribute("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _context.Users.Where(u=>u.Id == id).SingleOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            var response = new User();
            return Ok(reponse);
        }
    }
}

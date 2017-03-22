using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApplication2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore_rest_api.Data
{
    public class DbInitializer
    {
        public static async Task Initialize()
        {

            ApplicationDbContext context = new ApplicationDbContext();
            context.Database.Migrate();
            var userStore = new UserStore<ApplicationUser>(context);
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context), null,null,null,null,null);
            List<IdentityRole> roles = new List<IdentityRole>();
            roles.Add(new IdentityRole("SuperAdmin"));
            roles.Add(new IdentityRole("Administrator"));
            roles.Add(new IdentityRole("Moderetor"));
            roles.Add(new IdentityRole("User"));
            
            foreach(IdentityRole role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Name))
                {
                    await roleManager.CreateAsync(role);
                }
            }

            
        }
    }
}
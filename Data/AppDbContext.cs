using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

using aspnetcore_rest_api.Models;

namespace aspnetcore_rest_api.Data
{
    public class AppDbContext : DbContext
    {        
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<DbUser> Users {get; set;}
    }
}
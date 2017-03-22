using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aspnetcore_rest_api.Data;
using aspnetcore_rest_api.Middleware;
using aspnetcore_rest_api.Middleware.DataModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using aspnetcore_rest_api.Models;

namespace aspnetcore_rest_api
{
    public class Startup
    {
        private static readonly string secretKey = "mysupersecret_secretkey!123";
        private static readonly string issure = "TestUser";
        private static readonly string audience = "TestAudience";
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase());
            // Add framework services.
            services.AddMvc();

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            //start jwt token config
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            //generate token

            var jwtOptions = new TokenProviderOptions
            {
                Audience = audience,
                Issuer = issure,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };

            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(jwtOptions));
            
            var context = app.ApplicationServices.GetService<AppDbContext>();
            AddTestData(context);

            app.UseMvc();

            app.UseSwaggerGen();
            app.UseSwaggerUi();
        }

        private static void AddTestData(AppDbContext context)
        {
            context.Users.Add(new DbUser(){
                Id = 17,
                FirstName = "Luke",
                LastName = "Solo"
            });
            context.Users.Add(new DbUser(){
                Id = 13,
                FirstName = "Yasha",
                LastName = "Liu"
            });
            context.Users.Add(new DbUser(){
                Id = 19,
                FirstName = "Lily",
                LastName = "Huang"
            });
            context.SaveChanges();
        }
    }
}

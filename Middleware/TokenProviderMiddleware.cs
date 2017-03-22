using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using aspnetcore_rest_api.Middleware.DataModels;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using aspnetcore_rest_api.Models;
using Microsoft.AspNetCore.Identity;
using aspnetcore_rest_api.Data;

namespace aspnetcore_rest_api.Middleware
{
    public class TokenProviderMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly TokenProviderOptions _options;

        private readonly UserManager<ApplicationUser> _userManager;
        private static ApplicationDbContext db = new ApplicationDbContext();

        public TokenProviderMiddleware(RequestDelegate next, 
                                        IOptions<TokenProviderOptions> options,
                                        UserManager<ApplicationUser> userManager)
        {
            _next = next;
            _options = options;
            _userManager = userManager;
        }

        public Task Invoke(HttpContext context)
        {
            if(!context.Request.Path.Equals(_options.Path, StringComparison.Ordinal))
            {
                return _next(context);
            }
            if(!context.Request.Method.Equals("POST") || !context.Request.HasFormContentType)
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync("Bad Request");
            }
            return GenerateToken(context);
        }
        private async Task GenerateToken(HttpContext context)
        {
            var userName = context.Request.Form["username"];
            var password = context.Request.Form["password"];
            ApplicationUser user = null;
            user = db.Users.Where(x=>x.Email == userName).FirstOrDefault();
            var result = _userManager.CheckPassowrdAsync(user, password);
            if (result.Result == false)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid username or password");
                return;
            }
            var now = DateTime.UtcNow;
            var userClaims = await _userManager.GetRolesAsync(user);
            List<Claim> claims= new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userName));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));

            foreach(var x in userClaims)
            {
                claims.Add(new Claim(ClaimTypes.Role, x));
            }


            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(_options.Expiration),
                signingCredentials: _options.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_options.Expiration.TotalSeconds
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
    
        }
    }
}

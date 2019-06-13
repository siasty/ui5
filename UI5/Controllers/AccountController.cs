using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace UI5.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(IHttpContextAccessor httpContextAccessor)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }

        public string Token()
        {
            string result = "";

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("TokenProviderOptions")["Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var time = _config.GetSection("TokenProviderOptions")["expiration"];
                if(time == null) { time = "24"; }
                var _exp = DateTime.Now.AddHours(double.Parse(time));

                var token = new JwtSecurityToken(
                    issuer: _config.GetSection("TokenProviderOptions")["Issuer"],
                    audience: _config.GetSection("TokenProviderOptions")["Issuer"],
                    claims: _httpContextAccessor.HttpContext.User.Claims,
                    expires: _exp,
                    signingCredentials: creds);


                //var expiration = token.ValidTo.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")

                result = new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            { result = "Could not verify username and password" ; }
            return result;
        }
    }
}
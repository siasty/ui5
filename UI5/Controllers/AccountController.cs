using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly ILogger<AccountController> _logger;

        public AccountController(SignInManager<IdentityUser> signInManager, ILogger<AccountController> logger, IConfiguration config)
        {
            _signInManager = signInManager;
            _logger = logger;
            _config = config;
        }

        public class DataForAuthorization
        {

            [Required]
            public string Name { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

        }

        [HttpPost]
        public async Task<IActionResult> Token([FromBody] DataForAuthorization model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Name, model.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var claims = new[]
                {
                       new Claim(ClaimTypes.Name, model.Name)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("TokenProviderOptions")["Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var time = _config.GetSection("TokenProviderOptions")["expiration"];
                var _exp = DateTime.Now.AddHours(double.Parse(time));

                var token = new JwtSecurityToken(
                    issuer: _config.GetSection("TokenProviderOptions")["Issuer"],
                    audience: _config.GetSection("TokenProviderOptions")["Issuer"],
                    claims: claims,
                    expires: _exp,
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")

                });
            }
            else
                return BadRequest("Could not verify username and password");
        }
    }
}
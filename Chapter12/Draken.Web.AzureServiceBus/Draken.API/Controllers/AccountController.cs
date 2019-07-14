using Draken.Data;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Draken.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;
        public AccountController(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
        }
        [HttpPost("token")]
        public ActionResult Post(User user)
        {
            var authUser = GetUser(user);
            if (authUser != null)
            {
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email)
            };

                var token = new JwtSecurityToken
                (
                    issuer: _configuration["Security:Jwt:Issuer"],
                    audience: _configuration["Security:Jwt:Issuer"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(60),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Security:Jwt:Key"])),
                        SecurityAlgorithms.HmacSha256)
                );




                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expires = DateTime.UtcNow.AddDays(60),
                    token_type = "bearer"
                });
            }

            return NotFound();
        }

        private User GetUser(User user)
        {
            return _databaseContext.Users.FirstOrDefault(u => u.Email == user.Email
                && HashPassword(user.Password) == u.Password);
        }

        private string HashPassword(string password)
        {
            var salt = Encoding.UTF8.GetBytes(_configuration["Security:PasswordSalt"]);
            var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashedPassword;
        }
    }
}

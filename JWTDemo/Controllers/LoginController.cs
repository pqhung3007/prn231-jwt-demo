using JWTDemo.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // https://stackoverflow.com/questions/56185834/asp-net-core-api-always-returns-401-but-bearer-token-is-included
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Login(string username, string password)
        {
            User user = new User();
            user.Username = username;
            user.Password = password;
            IActionResult response = Unauthorized();

            var authenticatedUser = AuthenticateUser(user);
            if (authenticatedUser != null)
            {
                var stringToken = GenerateJsonWebToken(user);
                response = Ok(new { token = stringToken });
            }

            return response;
        }

        [Authorize]
        [HttpGet("PrivateApi")]
        public string Post()
        {
            return "This method is private";
        }

        private string GenerateJsonWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["jwt:secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["jwt:issuer"],
                audience: _config["jwt:issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
                );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }

        private User AuthenticateUser(User login)
        {
            User user = null;
            if (login.Username == "hungpq" && login.Password == "123")
            {
                user = new User { Username = "hungpq", Password = "123" };
            }
            return user;
        }
    }
}

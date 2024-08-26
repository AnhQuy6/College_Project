using CollegeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        [HttpPost]
        public ActionResult<string> Login(LoginDTO model)
        {
            if (!ModelState.IsValid) {
                return BadRequest("Please provide username or password");
            }
            LoginResponeDTO respone = new LoginResponeDTO()
            {
                Username = model.Username,
            };

            byte[] key = null;
            if (model.Policy == "Local")
                key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretForLocal"));
            else if (model.Policy == "Microsoft")
                key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretForMicrosoft"));
            else
                key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretForGoogle"));

            if (model.Username == "quygagay" && model.Password == "Anhquy123") {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                            //Username
                            new Claim(ClaimTypes.Name, model.Username),
                            //Role
                            new Claim(ClaimTypes.Role, "Admin")
                    }),
                    Expires = DateTime.Now.AddDays(100),
                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                respone.token = tokenHandler.WriteToken(token);
            }
            else
            {
                return BadRequest("Invalid username and password");
            }
            return Ok(respone);
        }
    }
}
    


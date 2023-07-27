using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealStateAPI.Data;
using RealStateAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealStateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        ApiDBContext _dbContext = new ApiDBContext();
        private IConfiguration _config;

        public UsersController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("[action]")]
        public IActionResult Register([FromBody] User user)
        {
            var userExists = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email); // check if email already exists
            if (userExists != null)
            {
                return BadRequest("User already exists with the given email");
            }
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("[action]")] // api/users/login
        public IActionResult Login([FromBody] User user)
        {
            var existingUser = _dbContext.Users.Where(u => u.Email == user.Email).FirstOrDefault();
            if (existingUser == null)
            {
                return NotFound();
            }
            else
            {
                if (existingUser.Password == user.Password)
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                    var claims = new[]
                    {
                    new Claim(ClaimTypes.Email, existingUser.Email),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                    var token = new JwtSecurityToken(
                        issuer: _config["Jwt:Issuer"],
                        audience: _config["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: credentials
                    );

                    var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(new { jwt });
                }
                else
                {
                    return Unauthorized();
                }
            }
        }

    }



}

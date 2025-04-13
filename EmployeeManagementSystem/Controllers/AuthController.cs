using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Entity;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRepository<User> _userRepo;
        private readonly IConfiguration _configuration;
        private readonly IRepository<Employee> _empRepo;

        public AuthController(IRepository<User> userRepo, IConfiguration configuration, IRepository<Employee> empRepo)
        {
            _userRepo = userRepo;
            _configuration = configuration;
            _empRepo = empRepo;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] AuthDto model)
        {
            var user=(await _userRepo.GetAll(x => x.Email == model.Email)).FirstOrDefault();
            //var user=users.Where(x=>x.Email==model.Email).FirstOrDefault();
            if (user == null)
            {
                return new BadRequestObjectResult(new { message = "user not found." });
            }
            var passwordHelper = new PasswordHelper();
            if (!passwordHelper.VerifyPassword(user.Password, model.Password))
            {
                return new BadRequestObjectResult(new { message = "email or password incorrect." });
            }
            var token = GenerateToken(user.Email, user.Role);
            return Ok(new AuthTokenDto
            {
                Id=user.Id,
                Email=user.Email,
                Token= token,
                Role=user.Role,
            });
        }
        private string GenerateToken(string email,string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtKey"]!));
            var credential=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,email),
                new Claim(ClaimTypes.Role,role),
            };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credential);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost]
        [Route("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody]ProfileDto model)
        {
            var email=User.FindFirstValue(ClaimTypes.Name);
            var user = (await _userRepo.GetAll(x => x.Email == email)).FirstOrDefault();
            var employee=(await _empRepo.GetAll(x=>x.Id == user.Id)).FirstOrDefault();
            employee.Name = model.Name;
            employee.Email = model.Email;
            employee.Phone = model.Phone;
            _empRepo.Update(employee);
            user.ProfileImage=model.ProfileImage;
            user.Email = model.Email;
            var passwordHelper = new PasswordHelper();
            user.Password=passwordHelper.HashPassword(model.Password);
            _userRepo.Update(user);
            await _userRepo.SaveChangesAsync();
            return Ok();
        }
    }
}

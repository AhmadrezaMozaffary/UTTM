using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using UTTM.Context;
using UTTM.Models;
using UTTM.Models.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UTTM.Controllers.Api
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration _config;
        private UttmDbContext _context;

        public UserController(IConfiguration config, UttmDbContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpViewModel req)
        {
            if (ModelState.IsValid)
            {
                if (UserExists(req.UserName))
                {
                    return Ok("نام کاربری قبلا انتخاب شده است");
                }

                DateTime currentDate = DateTime.Now;
                User _newUser = new()
                {
                    Id = 0,
                    UserName = req.UserName,
                    Password = EncryptPassword(req.Password),
                    Role = req.Role,
                    CreatedAt = currentDate,
                    LastUpdatedAt = currentDate
                };

                await _context.User.AddAsync(_newUser);
                Save();

                return Ok("کاربر با موفقیت ایجاد شد");
            }
            else
            {
                return BadRequest("مقادیر وارد شده صحیح نمیباشد");
            }
        }

        // GET: api/<LoginController>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)

        {
            if(!UserExists(req.UserName)){ return NotFound("نام کاربری وجود ندارد"); };

            User dbUser = await _context.User.FirstAsync<User>(_user => _user.UserName == req.UserName);

            if (dbUser != null && (DecryptPassword(dbUser.Password) == req.Password))
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
                  _config["Jwt:Issuer"],
                  null,
                  expires: DateTime.Now.AddMinutes(120),
                  signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                return Ok(token);
            }
            else
            {
                return NotFound("اطلاعات وارد شده صحیح نمیباشد");
            }
        }

        private bool UserExists(string username)
        {
            return _context.User.Any(u => u.UserName == username);
        }

        private void Save()
        {
            _context.SaveChanges();
        }


        private string EncryptPassword(string password)
        {
            return password;
        }


        private string DecryptPassword(string password)
        {
            return password;
        }
    }
}

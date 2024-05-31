using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using UTTM.Business;
using UTTM.Context;
using UTTM.Infra;
using UTTM.Infra.Interfaces;
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
    public class UserController : UttmController, IControllerBusiness<UserBusiness>
    {
        private IConfiguration _config;

        public UserBusiness Biz { get; set; }

        public UserController(IConfiguration config, UttmDbContext context) : base(context)
        {
            _config = config;
            Biz = new UserBusiness(context);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpViewModel req)
        {
            if (ModelState.IsValid)
            {
                if (Biz.UserExists(req.UserName))
                {
                    return Ok("نام کاربری قبلا انتخاب شده است");
                }

                DateTime currentDate = DateTime.Now;
                User _newUser = new()
                {
                    Id = 0,
                    UserName = req.UserName,
                    Password = Biz.HashPassword(req.Password),
                    Role = req.Role,
                    CreatedAt = currentDate,
                    LastUpdatedAt = currentDate
                };

                await Ctx.User.AddAsync(_newUser);
                Save();

                return Ok(_newUser.Id);
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
            if (!Biz.UserExists(req.UserName)) { return NotFound("نام کاربری وجود ندارد"); };

            User dbUser = await Ctx.User.FirstAsync<User>(_user => _user.UserName == req.UserName);

            if (dbUser != null && (dbUser.Password == Biz.HashPassword(req.Password)))
            {
                return Ok(Biz.GetToken(_config, dbUser));
            }
            else
            {
                return NotFound("اطلاعات وارد شده صحیح نمیباشد");
            }
        }

    }
}

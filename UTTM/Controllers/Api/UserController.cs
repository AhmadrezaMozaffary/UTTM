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
        public UserBusiness Biz { get; set; }

        public UserController(UserBusiness biz)
        {
            Biz = biz;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<int>> SignUp([FromBody] SignUpViewModel req)
        {
            if (ModelState.IsValid)
            {
                return Ok(await Biz.CreateUser(req));
            }
            else
            {
                return BadRequest("مقادیر وارد شده صحیح نمیباشد");
            }
        }

        // GET: api/<LoginController>
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequest req)
        {
            return Ok(await Biz.Login(req));
        }

    }
}

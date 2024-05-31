using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UTTM.Business;
using UTTM.Context;
using UTTM.Infra;
using UTTM.Infra.Interfaces;
using UTTM.Models;
using UTTM.Models.ViewModels;

namespace UTTM.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SettingController : UttmController, IControllerBusiness<SettingBusiness>
    {
        public SettingBusiness Biz { get; set; }

        public SettingController(SettingBusiness biz) 
        {
            Biz = biz;
        }

        [HttpGet("GetUserSetting")]
        public async Task<ActionResult<Setting>> GetByUserId(int userId)
        {
            return Ok(await Biz.GetByUserId(userId));
        }

        [HttpPost("Set")]
        public async Task<ActionResult<int>> Set([FromBody] SettingViewModel req)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            return Ok(await Biz.Set(req));
        }

    }
}

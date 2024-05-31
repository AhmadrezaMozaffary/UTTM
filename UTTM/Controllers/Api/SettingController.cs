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
        public UserBusiness UserBiz { get; set; }


        public SettingController(UttmDbContext context) : base(context)
        {
            Biz = new SettingBusiness(context);
            UserBiz = new UserBusiness(context);
        }

        [HttpGet("GetUserSetting")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            if (!UserBiz.UserExists(userId)) { return NotFound("کاربر مدنظر یافت نشد"); }

            Setting? s = await Biz.GetByUserId(userId);

            if (s == null) { return NotFound("برای کاربر مدنظر هیچ تنظیماتی یافت نشد"); }

            return Ok(s);
        }

        [HttpPost("Set")]
        public async Task<IActionResult> Set([FromBody] SettingViewModel req)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if(!UserBiz.UserExists(req.UserId)) { return NotFound("کاربر مدنظر یافت نشد"); }

            int settingId = await Biz.Set(req);

            return Ok(settingId);
        }

    }
}

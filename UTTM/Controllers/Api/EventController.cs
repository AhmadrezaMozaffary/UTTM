using Microsoft.AspNetCore.Authorization;
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
    [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Society)}")]
    public class EventController : UttmController, IControllerBusiness<EventBusiness>
    {
        public EventBusiness Biz { get; set; }
        public EventController(EventBusiness biz)
        {
            Biz = biz;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<int>> Create([FromBody] EventViewModel req)
        {
            return Ok(await Biz.CreateEvent(req));
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Event>>> GetAll()
        {
            return Ok(await Biz.GetAllEvents());
        }

        [HttpGet("GetById")]
        [AllowAnonymous]
        public async Task<ActionResult<Event>> GetById(int id)
        {
            var e = await Biz.GetEventById(id);
            return e == null ? NotFound("رویداد مدنظر یافت نشد") : Ok(e);
        }

        [HttpDelete("Remove")]
        public async Task<ActionResult<int>> Remove(int id)
        {
            return Ok(await Biz.RemoveEvent(id));
        }


    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [Authorize(Roles = $"{nameof(UserRole.Admin)}, {nameof(UserRole.Society)}")]
    public class SocietyController : UttmController, IControllerBusiness<SocietyBusiness>
    {
        public SocietyBusiness Biz { get; set; }

        public SocietyController(SocietyBusiness biz)
        {
            Biz = biz;
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Society>>> GetAll()
        {
            return Ok(await Biz.GetAllSocieties());
        }

        [HttpGet("GetById")]
        [AllowAnonymous]
        public async Task<ActionResult<Society>> GetById(int id)
        {
            return Ok(await Biz.GetSocietyById(id));
        }

        [HttpPost("Add")]
        public async Task<ActionResult<int>> Add([FromBody] SocietyViewModel req)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); };

            return Ok(await Biz.AddNewSociety(req));
        }


        [HttpPut("Edit")]
        public async Task<ActionResult<Society>> Edit([FromBody] SocietyEditModel req)
        {
            return Ok(await Biz.EditSociety(req));
        }

        [HttpPut("EditName")]
        public async Task<ActionResult<Society>> EditName([FromBody] SocietyEditNameModel req)
        {
            return Ok(await Biz.RenameSociety(req));
        }


        [HttpPut("EditAvatar")]
        public async Task<ActionResult<Society>> EditAvatar([FromBody] SocietyEditAvatarModel req)
        {
            return Ok(await Biz.ChangeAvatar(req));
        }

        [HttpPut("EditDescription")]
        public async Task<ActionResult<Society>> EditDescription([FromBody] SocietyEditDescriptionModel req)
        {
            return Ok(await Biz.EditDescription(req));
        }

        [HttpDelete("Remove")]
        [Authorize(nameof(UserRole.Admin))]
        public async Task<ActionResult<int>> Remove(int id)
        {
            return Ok(await Biz.DeleteSociety(id));
        }


    }
}


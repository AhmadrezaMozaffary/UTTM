using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class MajorController : UttmController, IControllerBusiness<MajorBusiness>
    {
        public MajorBusiness Biz { get; set; }

        public MajorController(MajorBusiness biz)
        {
            Biz = biz;
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Major>>> GetAll()
        {
            return Ok(await Biz.GetAllMajors());
        }

        [HttpPost("Add")]
        public async Task<ActionResult<int>> Add([FromBody] MajorViewModel req)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); };

            return Ok(await Biz.AddNewMajor(req));
        }

        [HttpPut("Rename")]
        public async Task<ActionResult<Major>> Rename([FromBody] MajorEditTitleModel req)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); };

            return Ok(await Biz.RenameMajor(req));
        }

        [HttpDelete("Remove")]
        public async Task<ActionResult<int>> Remove(int id)
        {
            return Ok(await Biz.DeleteMajor(id));
        }
    }
}

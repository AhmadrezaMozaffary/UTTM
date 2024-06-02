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
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class UniversityController : UttmController, IControllerBusiness<UniversityBusiness>
    {
        public UniversityBusiness Biz { get; set; }

        public UniversityController(UniversityBusiness biz)
        {
            Biz = biz;
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<ActionResult<List<University>>> GetAll()
        {
            return Ok(await Biz.GetAllUniversities());
        }

        [HttpPost("Add")]
        public async Task<ActionResult<int>> Add([FromBody] UniversityViewModel req)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); };

            return Ok(await Biz.AddNewUniversity(req));
        }

        [HttpPut("Edit")]
        public async Task<ActionResult<University>> Edit(University req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(Biz.EditExistingUniversity(req));
        }

        [HttpDelete("Remove")]
        public async Task<ActionResult<int>> Remove(int id)
        {
            return Ok(await Biz.RemoveUniversity(id));
        }
    }
}

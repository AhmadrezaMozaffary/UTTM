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
    [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Society)}")]
    public class ProfessorController : UttmController, IControllerBusiness<ProfessorBusiness>
    {
        public ProfessorBusiness Biz { get; set; }

        public ProfessorController(ProfessorBusiness biz)
        {
            Biz = biz;
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Professor>>> GetAll()
        {
            return Ok(await Biz.GetAllProfessors());
        }

        [HttpGet("GetById")]
        [AllowAnonymous]
        public async Task<ActionResult<Professor>> GetById(int id)
        {
            return Ok(await Biz.GetProfessor(id));
        }

        [HttpPost("Add")]
        public async Task<ActionResult<int>> AddNew([FromBody] ProfessorViewModel req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(await Biz.AddNewProfessor(req));
        }

        [HttpPut("Edit")]
        public async Task<ActionResult<Professor>> Edit([FromBody] ProfessorEditViewModel req)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            return Ok(await Biz.EditProfessor(req));

        }

        [HttpDelete("Remove")]
        public async Task<ActionResult<int>> Remove(int id)
        {
            return Ok(await Biz.DeleteProfessor(id));
        }

    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UTTM.Context;
using UTTM.Infra;
using UTTM.Models;
using UTTM.Models.ViewModels;

namespace UTTM.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorController : UttmController
    {
        public ProfessorController(UttmDbContext context) : base(context)
        {
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            List<Professor> professors = await Ctx.Professor.ToListAsync();
            return Ok(professors);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            Professor? p = await GetProfessor(id);

            if (p == null) return NotFound("استاد مد نظر یافت نشد");

            return Ok(p);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddNew([FromBody] ProfessorViewModel req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!UserExists(req.UserId)) return NotFound("کاربر یافت نشد");

            if (!SocietyExists(req.SocietyId)) return NotFound("انجمن یافت نشد");

            if (CurrentProfessorExists(req.UserId)) return Unauthorized("برای کاربر انتخاب شده قبلا استاد اضافه شده است");

            if (!UserCanBeProfessor(req.UserId)) return Unauthorized("کابر از نوع استاد نیست");

            Professor _newProfessor = new()
            {
                Id = 0,
                FirstName = req.FirstName,
                LastName = req.LastName,
                Avatar = req.Avatar,
                Discription = req.Discription,
                Rate = req.Rate,
                EmailAddress = req.EmailAddress,
                UserId = req.UserId,
                SocietyId = req.SocietyId
            };

            await Ctx.Professor.AddAsync(_newProfessor);
            Save();

            return Ok(_newProfessor.Id);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] ProfessorEditViewModel req)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            Professor? p = await GetProfessor(req.Id);

            if (p == null) return NotFound("استاد جهت ویرایش یافت نشد");

            p.FirstName = req.FirstName;
            p.LastName = req.LastName;
            p.Avatar   = req.Avatar;
            p.Discription = req.Discription;
            p.Rate = req.Rate;
            p.EmailAddress = req.EmailAddress;

            Save();

            return Ok(p);   

        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove(int id)
        {
            Professor? p = await GetProfessor(id);

            if(p == null) return NotFound("استاد مد نظر یافت نشد");

            Ctx.Professor.Remove(p);
            Save();

            return Ok(p.Id);
        }

        #region Helpers
        private bool CurrentProfessorExists(int userId)
        {
            return Ctx.Professor.Where(s => s.UserId == userId).Any();
        }
        private Task<Professor?> GetProfessor(int id)
        {
            return Ctx.Professor.FirstOrDefaultAsync(x => x.Id == id);
        }
        private bool UserExists(int userId)
        {
            return Ctx.User.Any(u => u.Id == userId);
        }
        private bool SocietyExists(int societyId)
        {
            return Ctx.Society.Any(s => s.Id == societyId);
        }
        private bool UserCanBeProfessor(int uId)
        {
            return Ctx.User.First(u => u.Id == uId).Role == UserRole.Professor;
        }
        #endregion
    }
}

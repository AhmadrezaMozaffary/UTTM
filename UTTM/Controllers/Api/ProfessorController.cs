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
        public UserBusiness UserBiz { get; set; }

        public SocietyBusiness SocietyBiz { get; set; }

        public ProfessorController(UttmDbContext context) : base(context)
        {
            Biz = new ProfessorBusiness(context);
            UserBiz = new UserBusiness(context);
            SocietyBiz = new SocietyBusiness(context);
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            List<Professor> professors = await Ctx.Professor.ToListAsync();
            return Ok(professors);
        }

        [HttpGet("GetById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            Professor? p = await Biz.GetProfessor(id);

            if (p == null) return NotFound("استاد مد نظر یافت نشد");

            return Ok(p);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddNew([FromBody] ProfessorViewModel req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!UserBiz.UserExists(req.UserId)) return NotFound("کاربر یافت نشد");

            if (!SocietyBiz.SocietyExists(req.SocietyId)) return NotFound("انجمن یافت نشد");

            if (Biz.CurrentProfessorExists(req.UserId)) return Unauthorized("برای کاربر انتخاب شده قبلا استاد اضافه شده است");

            if (!UserBiz.UserCanBeTypeOf(UserRole.Professor, req.UserId)) return Unauthorized("کابر از نوع استاد نیست");

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

            Professor? p = await Biz.GetProfessor(req.Id);

            if (p == null) return NotFound("استاد جهت ویرایش یافت نشد");

            p.FirstName = req.FirstName;
            p.LastName = req.LastName;
            p.Avatar = req.Avatar;
            p.Discription = req.Discription;
            p.Rate = req.Rate;
            p.EmailAddress = req.EmailAddress;

            Save();

            return Ok(p);

        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove(int id)
        {
            Professor? p = await Biz.GetProfessor(id);

            if (p == null) return NotFound("استاد مد نظر یافت نشد");

            Ctx.Professor.Remove(p);
            Save();

            return Ok(p.Id);
        }

    }
}

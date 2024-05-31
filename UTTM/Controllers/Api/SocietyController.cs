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
    [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Society)}")]
    public class SocietyController : UttmController, IControllerBusiness<SocietyBusiness>
    {
        public SocietyBusiness Biz { get; set; }

        public UserBusiness UserBiz { get; set; }

        public MajorBusiness MajorBiz { get; set; }

        public SocietyController(UttmDbContext context) : base(context)
        {
            Biz = new SocietyBusiness(context);
            UserBiz = new UserBusiness(context);
            MajorBiz = new MajorBusiness(context);
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            List<Society> societies = await Ctx.Society.ToListAsync();

            return Ok(societies);
        }

        [HttpGet("GetById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            Society? _society = Biz.SocietyExists(id) ? await Ctx.Society.FirstAsync(s => s.Id == id) : null;

            if (_society == null) { return NotFound("انجمن مد نظر یافت نشد"); }

            return Ok(_society);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] SocietyViewModel req)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); };

            if (!MajorBiz.MajorExists(req.MajorId) || !UserBiz.UserExists(req.UserId)) { return BadRequest("کاربر یا رشته انتخاب شده وجود ندارد"); }

            if (Biz.SocietyExistsForThisMajor(req.MajorId)) { return Unauthorized("برای این رشته قبلا انجمن ثبت شده است"); }

            Society _newSociety = new()
            {
                Id = 0,
                Name = req.Name,
                Avatar = req.Avatar,
                Description = req.Description,
                UserId = req.UserId,
                MajorId = req.MajorId,
            };

            await Ctx.Society.AddAsync(_newSociety);
            Save();

            return Ok(_newSociety.Id);

        }


        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] SocietyEditModel req)
        {
            Society? _existingRecord = await Ctx.Society.FirstOrDefaultAsync(x => x.Id == req.Id);

            if (_existingRecord == null) { return NotFound("رکوردی جهت ویرایش وجود ندارد"); }

            _existingRecord.Name = req.Name;

            _existingRecord.Avatar = req.Avatar;

            _existingRecord.Description = req.Description;

            Ctx.Society.Update(_existingRecord);
            Save();

            return Ok(_existingRecord);
        }

        [HttpPut("EditName")]
        public async Task<IActionResult> EditName([FromBody] SocietyEditNameModel req)
        {
            Society? _existingRecord = await Ctx.Society.FirstOrDefaultAsync(x => x.Id == req.Id);

            if (_existingRecord == null) { return NotFound("رکوردی جهت ویرایش وجود ندارد"); }

            _existingRecord.Name = req.Name;

            Ctx.Society.Update(_existingRecord);
            Save();

            return Ok(_existingRecord);
        }


        [HttpPut("EditAvatar")]
        public async Task<IActionResult> EditAvatar([FromBody] SocietyEditAvatarModel req)
        {
            Society? _existingRecord = await Ctx.Society.FirstOrDefaultAsync(x => x.Id == req.Id);

            if (_existingRecord == null) { return NotFound("رکوردی جهت ویرایش وجود ندارد"); }

            _existingRecord.Avatar = req.Avatar;

            Ctx.Society.Update(_existingRecord);
            Save();

            return Ok(_existingRecord);
        }

        [HttpPut("EditDescription")]
        public async Task<IActionResult> EditDescription([FromBody] SocietyEditDescriptionModel req)
        {
            Society? _existingRecord = await Ctx.Society.FirstOrDefaultAsync(x => x.Id == req.Id);

            if (_existingRecord == null) { return NotFound("رکوردی جهت ویرایش وجود ندارد"); }

            _existingRecord.Description = req.Description;

            Ctx.Society.Update(_existingRecord);
            Save();

            return Ok(_existingRecord);
        }

        [HttpDelete("Remove")]
        [Authorize(nameof(UserRole.Admin))]
        public async Task<IActionResult> Remove(int id)
        {
            Society? _society = Biz.SocietyExists(id) ? await Ctx.Society.FirstAsync(s => s.Id == id) : null;

            if (_society == null) { return NotFound("انجمن مد نظر یافت نشد"); }

            Ctx.Society.Remove(_society);
            Save();

            return Ok();
        }


    }
}


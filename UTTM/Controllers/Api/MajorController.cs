﻿using Microsoft.AspNetCore.Http;
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
    public class MajorController : UttmController
    {
        public MajorController(UttmDbContext context) : base(context)
        {
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            List<Major> _majors = await Ctx.Major.ToListAsync();

            return Ok(_majors);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] MajorViewModel req)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); };

            if (MajorExists(req)) { return Unauthorized("این رشته قبلا در این دانشگاه اضافه شده است"); };

            if (!UniversityExists(req.UniversityId)) { return BadRequest("دانشگاه انتخاب شده معتبر نیست یا وجود ندارد"); };

            Major _newMajor = new()
            {
                Id = 0,
                Title = req.Title,
                UniversityId = req.UniversityId
            };

            await Ctx.Major.AddAsync(_newMajor);
            Save();

            return Ok(_newMajor.Id);

        }

        [HttpPut("Rename")]
        public async Task<IActionResult> Rename([FromBody] MajorEditTitleModel req)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); };

            Major? _existingMajor = await Ctx.Major.FirstOrDefaultAsync(m => m.Id == req.Id);

            if (_existingMajor == null) { return NotFound("رشته ای برای ویرایش یافت نشد"); };

            _existingMajor.Title = req.Title;

            Save();

            return Ok(_existingMajor);
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove(int id)
        {
            Major? _major = await Ctx.Major.FirstOrDefaultAsync(m => m.Id == id);

            if (_major == null) { return NotFound("رشته ای جهت حذف وجود ندارد"); };

            Ctx.Major.Remove(_major);

            Save();

            return Ok();
        }

        #region  Helpers
        private bool MajorExists(MajorViewModel req)
        {
            bool exists = Ctx.Major.Where(m => m.UniversityId == req.UniversityId).Any(m => m.Title == req.Title);

            return exists;
        }

        private bool UniversityExists(int uniId)
        {
            return Ctx.University.Any(u => u.Id == uniId);
        }

        #endregion
    }
}
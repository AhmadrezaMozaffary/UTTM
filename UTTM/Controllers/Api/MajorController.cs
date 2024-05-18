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
    public class MajorController : UttmController, IControllerBusiness<MajorBusiness>
    {
        public MajorBusiness Biz { get; set; }

        public UniversityBusiness UniBiz { get; set; }

        public MajorController(UttmDbContext context) : base(context)
        {
            Biz = new MajorBusiness(context);
            UniBiz = new UniversityBusiness(context);
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

            if (Biz.MajorExists(req)) { return Unauthorized("این رشته قبلا در این دانشگاه اضافه شده است"); };

            if (!UniBiz.UniversityExists(req.UniversityId)) { return BadRequest("دانشگاه انتخاب شده معتبر نیست یا وجود ندارد"); };

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
    }
}

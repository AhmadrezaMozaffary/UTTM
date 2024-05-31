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
    [Authorize]
    public class StudentController : UttmController, IControllerBusiness<StudentBusiness>
    {
        public StudentBusiness Biz { get; set; }

        public UserBusiness UserBiz { get; set; }

        public SocietyBusiness SocietyBiz { get; set; }


        public StudentController(UttmDbContext context) : base(context)
        {
            Biz = new StudentBusiness(context);
            UserBiz = new UserBusiness(context);
            SocietyBiz = new SocietyBusiness(context);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            List<Student> stds = await Ctx.Student.ToListAsync();
            return Ok(stds);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            Student? _std = await Biz.GetStudent(id);

            if (_std == null) return NotFound("دانشجو مد نظر یافت نشد");

            return Ok(_std);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddNew([FromBody] StudentViewModel req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!UserBiz.UserExists(req.UserId)) return NotFound("کاربر یافت نشد");

            if (!SocietyBiz.SocietyExists(req.SocietyId)) return NotFound("انجمن یافت نشد");

            if (Biz.CurrentStudentExists(req.UserId)) return Unauthorized("برای کاربر انتخاب شده قبلا دانشجو اضافه شده است");

            if (!UserBiz.UserCanBeTypeOf(UserRole.Student, req.UserId)) return Unauthorized("کابر از نوع دانشجو نیست");

            Student _newStd = new()
            {
                Id = 0,
                FirstName = req.FirstName,
                LastName = req.LastName,
                Avatar = req.Avatar,
                UserId = req.UserId,
                SocietyId = req.SocietyId
            };

            await Ctx.Student.AddAsync(_newStd);
            Save();

            return Ok(_newStd.Id);

        }

        [HttpPut("Edit")]
        public async Task<IActionResult> EditStudent([FromBody] StudentEditViewModel req)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            Student? _existingStd = await Biz.GetStudent(req.Id);

            if (_existingStd == null) return NotFound("دانشجو جهت ویرایش یافت نشد");

            _existingStd.FirstName = req.FirstName;
            _existingStd.LastName = req.LastName;
            _existingStd.Avatar = req.Avatar;

            Save();

            return Ok(_existingStd);
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove(int id)
        {
            Student? _std = await Biz.GetStudent(id);

            if (_std == null) return NotFound("دانشجو مد نظر یافت نشد");

            Ctx.Student.Remove(_std);
            Save();

            return Ok(_std);
        }

    }
}

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
    public class StudentController : UttmController
    {
        public StudentController(UttmDbContext context) : base(context)
        {
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
            Student? _std = await GetStudent(id);

            if (_std == null) return NotFound("دانشجو مد نظر یافت نشد");

            return Ok(_std);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddNew([FromBody] StudentViewModel req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!UserExists(req.UserId)) return NotFound("کاربر یافت نشد");

            if (!SocietyExists(req.SocietyId)) return NotFound("انجمن یافت نشد");

            if(CurrentStudentExists(req.UserId)) return Unauthorized("برای کاربر انتخاب شده قبلا دانشجو اضافه شده است");

            if (!UserCanBeStudent(req.UserId)) return Unauthorized("کابر از نوع دانشجو نیست");

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

            if (!UserExists(req.UserId)) return NotFound("کاربر یافت نشد");

            if (!SocietyExists(req.SocietyId)) return NotFound("انجمن یافت نشد");

            if (CurrentStudentExists(req.UserId)) return Unauthorized("برای کاربر انتخاب شده قبلا دانشجو اضافه شده است");

            if (!UserCanBeStudent(req.UserId)) return Unauthorized("کابر از نوع دانشجو نیست");

            Student? _existingStd = await GetStudent(req.Id);

            if (_existingStd == null) return NotFound("دانشجو جهت ویرایش یافت نشد");

            _existingStd.FirstName = req.FirstName;
            _existingStd.LastName = req.LastName;
            _existingStd.Avatar = req.Avatar;
            _existingStd.UserId = req.UserId;
            _existingStd.SocietyId = req.SocietyId;

            Save();

            return Ok(_existingStd);
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove(int id)
        {
            Student? _std = await GetStudent(id);

            if (_std == null) return NotFound("دانشجو مد نظر یافت نشد");

            Ctx.Student.Remove(_std);
            Save();

            return Ok(_std);
        }

        #region Helpers
        private bool CurrentStudentExists(int userId)
        {
            return Ctx.Student.Where(s => s.UserId == userId).Any();
        }

        private bool UserCanBeStudent(int userId)
        {
            UserRole? role = Ctx.User.First(u => u.Id == userId).Role;

            return  role == UserRole.Student;
        }

        private Task<Student?> GetStudent(int id)
        {
            return Ctx.Student.FirstOrDefaultAsync(s => s.Id == id);
        }

        private bool UserExists(int userId)
        {
            return Ctx.User.Any(u => u.Id == userId);
        }

        private bool SocietyExists(int societyId)
        {
            return Ctx.Society.Any(s => s.Id == societyId);
        }

        #endregion
    }
}

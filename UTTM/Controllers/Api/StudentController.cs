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

        public StudentController(StudentBusiness biz)
        {
            Biz = biz;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<Student>>> GetAll()
        {      
            return Ok(await Biz.GetAllStudents());
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<Student>> GetById(int id)
        {
            return Ok(await Biz.GetStudent(id));
        }

        [HttpPost("Add")]
        public async Task<ActionResult<int>> AddNew([FromBody] StudentViewModel req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(Biz.AddNewStudent(req));

        }

        [HttpPut("Edit")]
        public async Task<ActionResult<Student>> EditStudent([FromBody] StudentEditViewModel req)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            return Ok(await Biz.EditStudent(req));
        }

        [HttpDelete("Remove")]
        public async Task<ActionResult<int>> Remove(int id)
        {
            return Ok(await Biz.DeleteStudent(id));
        }

    }
}

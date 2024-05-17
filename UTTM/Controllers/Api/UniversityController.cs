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
    public class UniversityController : UttmController
    {
        public UniversityController(UttmDbContext context) : base(context)
        {
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            List<University> _universities = await Ctx.University.ToListAsync();

            return Ok(_universities);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] UniversityViewModel req)
        {
            if (!ModelState.IsValid) { return BadRequest(); };

            if(UniversityExists(req.Name)) { return Unauthorized("دانشگاه قبلا اضافه شده است"); };

            University _uni = new()
            {
                Id = 0,
                Name = req.Name,
                Logo = req.Logo,
            };

            await Ctx.University.AddAsync(_uni);
            Save();

            return Ok(_uni.Id);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(University req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            University? _existingUni = await Ctx.University.FirstOrDefaultAsync(u => u.Id == req.Id);

            if (_existingUni == null) { return NotFound("دانشگاهی برای ویرایش یافت نشد"); };

            _existingUni.Name = req.Name;

            _existingUni.Logo = req.Logo;

            Save();

            return Ok(_existingUni);
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove(int id)
        {
            University? _uni = await Ctx.University.FirstOrDefaultAsync(u => u.Id == id);

            if (_uni == null) { return NotFound("دانشگاه جهت حذف وجود ندارد"); };

            Ctx.University.Remove(_uni); 
            
            Save();
            
            return Ok();
        }

        #region Helpers

        private bool UniversityExists(string uniName)
        {
            return Ctx.University.Any(u => u.Name == uniName);  
        }

        #endregion
    }
}

﻿using Microsoft.AspNetCore.Authorization;
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
    [Authorize(nameof(UserRole.Admin))]
    public class UniversityController : UttmController, IControllerBusiness<UniversityBusiness>
    {
        public UniversityBusiness Biz { get ; set; }

        public UniversityController(UttmDbContext context) : base(context)
        {
            Biz = new UniversityBusiness(context);
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            List<University> _universities = await Ctx.University.ToListAsync();

            return Ok(_universities);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] UniversityViewModel req)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); };

            if(Biz.UniversityExists(req.Name)) { return Unauthorized("دانشگاه قبلا اضافه شده است"); };

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

            if (_uni == null) { return NotFound("دانشگاهی جهت حذف وجود ندارد"); };

            Ctx.University.Remove(_uni); 
            
            Save();
            
            return Ok();
        }
    }
}

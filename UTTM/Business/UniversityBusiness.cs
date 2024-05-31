using Microsoft.EntityFrameworkCore;
using UTTM.Business.Interfaces;
using UTTM.Context;
using UTTM.Infra;
using UTTM.Models;
using UTTM.Models.ViewModels;

namespace UTTM.Business
{
    public class UniversityBusiness : BusinessBase, IUniversityBusiness
    {
        public UniversityBusiness(UttmDbContext ctx) : base(ctx)
        {
        }
        public async Task<List<University>> GetAllUniversities()
        {
            return await ctx.University.ToListAsync();
        }

        public async Task<int> AddNewUniversity(UniversityViewModel req)
        {
            if (UniversityExists(req.Name)) { throw new Exception("دانشگاه قبلا اضافه شده است"); };

            University _uni = new()
            {
                Id = 0,
                Name = req.Name,
                Logo = req.Logo,
            };

            await ctx.University.AddAsync(_uni);
            Save();

            return _uni.Id;
        }

        public async Task<University> EditExistingUniversity(University req)
        {

            University? _existingUni = await ctx.University.FirstOrDefaultAsync(u => u.Id == req.Id);

            if (_existingUni == null) { throw new Exception("دانشگاهی برای ویرایش یافت نشد"); };

            _existingUni.Name = req.Name;
            _existingUni.Logo = req.Logo;
            Save();

            return _existingUni;
         }

        public async Task<int> RemoveUniversity(int id)
        {
            University? _uni = await ctx.University.FirstOrDefaultAsync(u => u.Id == id);

            if (_uni == null) { throw new Exception("دانشگاهی جهت حذف وجود ندارد"); };

            ctx.University.Remove(_uni);
            Save();

            return _uni.Id;
        }
        public bool UniversityExists(string uniName)
        {
            return ctx.University.Any(u => u.Name == uniName);
        }


        public bool UniversityExists(int uniId)
        {
            return ctx.University.Any(u => u.Id == uniId);
        }
    }
}

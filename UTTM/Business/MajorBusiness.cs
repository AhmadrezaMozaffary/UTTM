using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UTTM.Business.Interfaces;
using UTTM.Context;
using UTTM.Infra;
using UTTM.Models;
using UTTM.Models.ViewModels;

namespace UTTM.Business
{
    public class MajorBusiness : BusinessBase, IMajorBusiness
    {
        public UniversityBusiness UniBiz { get; set; }

        public MajorBusiness(UttmDbContext ctx, UniversityBusiness uniBiz) : base(ctx)
        {
            UniBiz = uniBiz;
        }

        public async Task<List<Major>> GetAllMajors()
        {
            return await ctx.Major.ToListAsync();
        }

        public async Task<int> AddNewMajor(MajorViewModel req)
        {
            if (MajorExists(req)) { throw new Exception("این رشته قبلا در این دانشگاه اضافه شده است"); };

            if (!UniBiz.UniversityExists(req.UniversityId)) { throw new Exception("دانشگاه انتخاب شده معتبر نیست یا وجود ندارد"); };

            Major _newMajor = new()
            {
                Id = 0,
                Title = req.Title,
                UniversityId = req.UniversityId
            };

            await ctx.Major.AddAsync(_newMajor);
            Save();

            return _newMajor.Id;
        }

        public async Task<Major> GetMajorById(int id)
        {
            Major? m = await ctx.Major.FirstOrDefaultAsync(m => m.Id == id);

            if (m == null) { throw new Exception("رشته ای برای ویرایش یافت نشد"); };

            return m;
        }

        public async Task<Major> RenameMajor(MajorEditTitleModel req)
        {
            Major m = await GetMajorById(req.Id);

            m.Title = req.Title;
            Save();

            return m;
        }

        public async Task<int> DeleteMajor(int id)
        {
            Major? _major = await GetMajorById(id);

            ctx.Major.Remove(_major);
            Save();

            return _major.Id;
        }

        public bool MajorExists(int majorId)
        {
            return ctx.Major.Any(m => m.Id == majorId);
        }

        public bool MajorExists(MajorViewModel req)
        {
            bool exists = ctx.Major.Where(m => m.UniversityId == req.UniversityId).Any(m => m.Title == req.Title);

            return exists;
        }

    }
}

using Microsoft.EntityFrameworkCore;
using UTTM.Business.Interfaces;
using UTTM.Context;
using UTTM.Infra;
using UTTM.Models;
using UTTM.Models.ViewModels;

namespace UTTM.Business
{
    public class SocietyBusiness : BusinessBase, ISocietyBusiness
    {
        public UserBusiness UserBiz { get; set; }
        public MajorBusiness MajorBiz { get; set; }
        public SocietyBusiness(UttmDbContext ctx, UserBusiness userBiz, MajorBusiness majorBiz) : base(ctx)
        {
            UserBiz = userBiz;
            MajorBiz = majorBiz;
        }

        public async Task<List<Society>> GetAllSocieties()
        {
            return await ctx.Society.ToListAsync();
        }

        public async Task<Society> GetSocietyById(int id)
        {
            Society? _society = SocietyExists(id) ? await ctx.Society.FirstAsync(s => s.Id == id) : null;

            if (_society == null) { throw new Exception("انجمن مد نظر یافت نشد"); }

            return _society;

        }

        public async Task<int> AddNewSociety(SocietyViewModel req)
        {
            if (!MajorBiz.MajorExists(req.MajorId) || !UserBiz.UserExists(req.UserId)) { throw new Exception("کاربر یا رشته انتخاب شده وجود ندارد"); }

            if (SocietyExistsForThisMajor(req.MajorId)) { throw new Exception("برای این رشته قبلا انجمن ثبت شده است"); }

            Society _newSociety = new()
            {
                Id = 0,
                Name = req.Name,
                Avatar = req.Avatar,
                Description = req.Description,
                UserId = req.UserId,
                MajorId = req.MajorId,
            };

            await ctx.Society.AddAsync(_newSociety);
            Save();

            return _newSociety.Id;
        }

        public async Task<Society> EditSociety(SocietyEditModel req)
        {
            Society _existingRecord = await GetSocietyById(req.Id);

            _existingRecord.Name = req.Name;

            _existingRecord.Avatar = req.Avatar;

            _existingRecord.Description = req.Description;

            ctx.Society.Update(_existingRecord);
            Save();

            return _existingRecord;
        }

        public async Task<Society> RenameSociety(SocietyEditNameModel req)
        {
            Society _existingRecord = await GetSocietyById(req.Id);

            _existingRecord.Name = req.Name;

            ctx.Society.Update(_existingRecord);
            Save();

            return _existingRecord;
        }

        public async Task<Society> ChangeAvatar(SocietyEditAvatarModel req)
        {
            Society _existingRecord = await GetSocietyById(req.Id);

            _existingRecord.Avatar = req.Avatar;

            ctx.Society.Update(_existingRecord);
            Save();

            return _existingRecord;
        }

        public async Task<Society> EditDescription(SocietyEditDescriptionModel req)
        {
            Society _existingRecord = await GetSocietyById(req.Id);

            _existingRecord.Description = req.Description;

            ctx.Society.Update(_existingRecord);
            Save();

            return _existingRecord;
        }

        public async Task<int> DeleteSociety(int id)
        {
            Society _society = await GetSocietyById(id);

            ctx.Society.Remove(_society);
            Save();

            return _society.Id;
        }


        public bool SocietyExists(int societyId)
        {
            return ctx.Society.Any(s => s.Id == societyId);
        }

        public bool SocietyExistsForThisMajor(int majorId)
        {
            return ctx.Society.Any(s => s.MajorId == majorId);
        }
    }
}

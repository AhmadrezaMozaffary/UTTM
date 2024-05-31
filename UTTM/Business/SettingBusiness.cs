using Microsoft.EntityFrameworkCore;
using UTTM.Business.Interfaces;
using UTTM.Context;
using UTTM.Infra;
using UTTM.Models;
using UTTM.Models.ViewModels;

namespace UTTM.Business
{
    public class SettingBusiness : BusinessBase, ISettingBusiness
    {
        public UserBusiness UserBiz { get; set; }

        public SettingBusiness(UttmDbContext ctx, UserBusiness userBiz) : base(ctx)
        {
            UserBiz = userBiz;
        }

        public async Task<Setting?> GetByUserId(int userId)
        {
            if (!UserBiz.UserExists(userId)) { throw new Exception("کاربر مدنظر یافت نشد"); }

            Setting? s = await ctx.Setting.FirstOrDefaultAsync(u => u.UserId == userId); ;

            if (s == null) { throw new Exception("برای کاربر مدنظر هیچ تنظیماتی یافت نشد"); }

            return s;
        }

        public bool UserHaveSetting(int userId)
        {
            return ctx.Setting.Any(s => s.UserId == userId);
        }

        public async Task<int> Set(SettingViewModel reqBody)
        {
            if (!UserBiz.UserExists(reqBody.UserId)) { throw new Exception("کاربر مدنظر یافت نشد"); }

            if (UserHaveSetting(reqBody.UserId)) { return await Update(reqBody); }
            else
            {
                Setting s = new()
                {
                    Id = 0,
                    Config = reqBody.Config,
                    UserId = reqBody.UserId,
                };

                await ctx.Setting.AddAsync(s);

                Save();
                return s.Id;
            }
        }

        private async Task<int> Update(SettingViewModel reqBody)
        {
            Setting? s = await GetByUserId(reqBody.UserId);

            s.Id = reqBody.Id;
            s.UserId = reqBody.UserId;
            s.Config = reqBody.Config;

            Save();
            return s.Id;
        }
    }
}

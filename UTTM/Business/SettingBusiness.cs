using Microsoft.EntityFrameworkCore;
using UTTM.Context;
using UTTM.Infra;
using UTTM.Models;
using UTTM.Models.ViewModels;

namespace UTTM.Business
{
    public class SettingBusiness : BusinessBase
    {
        public SettingBusiness(UttmDbContext ctx) : base(ctx)
        {
        }

        public async Task<Setting?> GetByUserId(int userId)
        {
            return await Ctx.Setting.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public bool UserHaveSetting(int userId)
        {
            return Ctx.Setting.Any(s => s.UserId == userId);
        }

        public async Task<int> Set(SettingViewModel reqBody)
        {
            if (UserHaveSetting(reqBody.UserId)) { return await Update(reqBody); }
            else
            {
                Setting s = new()
                {
                    Id = 0,
                    Config = reqBody.Config,
                    UserId = reqBody.UserId,
                };

                await Ctx.Setting.AddAsync(s);

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
            return 1;
        }
    }
}

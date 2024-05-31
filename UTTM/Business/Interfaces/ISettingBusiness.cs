using UTTM.Models;
using UTTM.Models.ViewModels;

namespace UTTM.Business.Interfaces
{
    public interface ISettingBusiness
    {
        UserBusiness UserBiz { get; set; }

        Task<Setting?> GetByUserId(int userId);
        Task<int> Set(SettingViewModel reqBody);
        bool UserHaveSetting(int userId);
    }
}
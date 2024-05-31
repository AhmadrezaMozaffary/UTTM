using UTTM.Models;
using UTTM.Models.ViewModels;

namespace UTTM.Business.Interfaces
{
    public interface ISocietyBusiness
    {
        MajorBusiness MajorBiz { get; set; }
        UserBusiness UserBiz { get; set; }

        Task<int> AddNewSociety(SocietyViewModel req);
        Task<Society> ChangeAvatar(SocietyEditAvatarModel req);
        Task<int> DeleteSociety(int id);
        Task<Society> EditDescription(SocietyEditDescriptionModel req);
        Task<Society> EditSociety(SocietyEditModel req);
        Task<List<Society>> GetAllSocieties();
        Task<Society> GetSocietyById(int id);
        Task<Society> RenameSociety(SocietyEditNameModel req);
        bool SocietyExists(int societyId);
        bool SocietyExistsForThisMajor(int majorId);
    }
}
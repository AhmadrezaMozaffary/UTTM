using UTTM.Models;
using UTTM.Models.ViewModels;

namespace UTTM.Business.Interfaces
{
    public interface IUniversityBusiness
    {
        Task<int> AddNewUniversity(UniversityViewModel req);
        Task<University> EditExistingUniversity(University req);
        Task<List<University>> GetAllUniversities();
        Task<int> RemoveUniversity(int id);
        bool UniversityExists(int uniId);
        bool UniversityExists(string uniName);
    }
}
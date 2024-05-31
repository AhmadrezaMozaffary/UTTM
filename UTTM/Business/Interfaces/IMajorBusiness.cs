using UTTM.Models;
using UTTM.Models.ViewModels;

namespace UTTM.Business.Interfaces
{
    public interface IMajorBusiness
    {
        UniversityBusiness UniBiz { get; set; }

        Task<int> AddNewMajor(MajorViewModel req);
        Task<int> DeleteMajor(int id);
        Task<List<Major>> GetAllMajors();
        Task<Major> GetMajorById(int id);
        bool MajorExists(int majorId);
        bool MajorExists(MajorViewModel req);
        Task<Major> RenameMajor(MajorEditTitleModel req);
    }
}
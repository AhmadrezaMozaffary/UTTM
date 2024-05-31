using UTTM.Models;
using UTTM.Models.ViewModels;

namespace UTTM.Business.Interfaces
{
    public interface IProfessorBusiness
    {
        SocietyBusiness SocietyBiz { get; set; }
        UserBusiness UserBiz { get; set; }

        Task<int> AddNewProfessor(ProfessorViewModel req);
        bool CurrentProfessorExists(int userId);
        Task<int> DeleteProfessor(int id);
        Task<Professor> EditProfessor(ProfessorEditViewModel req);
        Task<List<Professor>> GetAllProfessors();
        Task<Professor?> GetProfessor(int id);
    }
}
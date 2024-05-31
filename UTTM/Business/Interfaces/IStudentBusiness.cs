using UTTM.Models;
using UTTM.Models.ViewModels;

namespace UTTM.Business.Interfaces
{
    public interface IStudentBusiness
    {
        Task<int> AddNewStudent(StudentViewModel req);
        bool CurrentStudentExists(int userId);
        Task<int> DeleteStudent(int id);
        Task<Student> EditStudent(StudentEditViewModel req);
        Task<List<Student>> GetAllStudents();
        Task<Student?> GetStudent(int id);
    }
}
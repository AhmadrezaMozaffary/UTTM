using UTTM.Controllers.Api;
using UTTM.Models;
using UTTM.Models.ViewModels;

namespace UTTM.Business.Interfaces
{
    public interface IUserBusiness
    {
        Task<int> CreateUser(SignUpViewModel req);
        string GetToken(User user);
        string HashPassword(string password);
        Task<string> Login(LoginRequest user);
        bool UserCanBeTypeOf(UserRole targetRole, int userId);
        bool UserExists(int userId);
        bool UserExists(string username);
    }
}
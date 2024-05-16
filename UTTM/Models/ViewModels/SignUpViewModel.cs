namespace UTTM.Models.ViewModels
{
    public class SignUpViewModel
    {
        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public UserRole Role { get; set; }
    }
}

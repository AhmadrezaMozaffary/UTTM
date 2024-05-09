using System.ComponentModel.DataAnnotations;

namespace UTTM.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
        
        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set;}
    }

    public enum UserRole
    {
        Admin = 1,
        Student = 2,
        Society = 3,
        Professor = 4
    }
}

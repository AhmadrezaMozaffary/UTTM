using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UTTM.Models
{
    public class Professor
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;

        public string Discription { get; set; } = string.Empty;

        public double Rate { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        public int UserId { get; set; }

        public int SocietyId { get; set; }

        public User User { get; set; }

        public Society Society { get; set; }
    }
}

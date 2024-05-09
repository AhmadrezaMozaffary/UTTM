using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UTTM.Models
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }

        public string Config { get; set; } = string.Empty;

        public int UserId { get; set; }

        public User User { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace UTTM.Models
{
    public class University
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Logo { get; set; } = string.Empty;
    }
}

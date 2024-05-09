using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UTTM.Models
{
    public class Major
    {
        [Key]
        public int Id { get; set;}

        public string Title { get; set; } = string.Empty;

        public int UniversityId { get; set; }

        public University University { get; set;}
    }
}

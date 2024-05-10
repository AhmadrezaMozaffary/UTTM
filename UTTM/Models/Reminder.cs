using System.ComponentModel.DataAnnotations;

namespace UTTM.Models
{
    public class Reminder
    {
        [Key]
        public int Id { get; set; }

        public int CourseId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public ReminderSendType SendType { get; set; }

        public DateTime RemindAt { get; set; }

        public Course Course { get; set; }
    }

    public enum ReminderSendType
    {
        Alert = 1,
        Email = 2,
        SMS = 3,
        EmailSMS = 4,
        AlertSMS = 5,
        AlertEmail = 6,
        All = 7,
    }
}

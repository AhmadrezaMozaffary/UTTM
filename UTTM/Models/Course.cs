using System.ComponentModel.DataAnnotations;

namespace UTTM.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        public string CouresTitle { get; set; } = string.Empty;

        public CourseType Type { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public DaysOfWeek Day { get; set; }

        public WeekType InitialWeekType { get; set; }

        public DateTime FinalExamAt { get; set; }

        public int StudentId { get; set; }

        public Student Student { get; set; }
    }

    public enum CourseType
    {
        JustOddWeeks = 1,
        JustEvenWeeks = 2,
        Weekly = 3
    }

    public enum DaysOfWeek
    {
        // Starts from 0 to have more integrity with Javascript 
        SUNDAY = 0,
        MONDAY = 1,
        TUESDAY = 2,
        WEDNESDAY = 3,
        THURSDAY = 4,
        FRIDAY = 5,
        SATURDAY = 6,
    }

    public enum WeekType
    {
        Odd = 1,
        Even = 2
    }
}

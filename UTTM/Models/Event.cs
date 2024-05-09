using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UTTM.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public EventStatus Status { get; set; }

        public EventTarget Target { get; set; }

        public EventPlatform Platform { get; set; }

        public int TotalCapacity { get; set; }

        public int AvailableCapacity { get; set; }

        public DateTime CratedAt { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public int SocietyId { get; set; }

        public Society Society { get; set; }
    }

    public enum EventStatus
    {
        Available = 1,
        Expired = 2,
        Canceled = 3,
        Full = 4,
    }

    public enum EventTarget
    {
        All = 1, SameSociety = 2
    }

    public enum EventPlatform
    {
        OnSite = 1, Online = 2
    }
}

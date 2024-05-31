namespace UTTM.Models.ViewModels
{
    public class EventViewModel
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public EventStatus Status { get; set; }

        public EventTarget Target { get; set; }

        public EventPlatform Platform { get; set; }

        public int TotalCapacity { get; set; }

        public int AvailableCapacity { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public int SocietyId { get; set; }
    }

}

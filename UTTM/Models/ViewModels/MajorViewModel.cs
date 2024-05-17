namespace UTTM.Models.ViewModels
{
    public class MajorViewModel
    {
        public string Title { get; set; } = string.Empty;

        public int UniversityId { get; set; }

    }

    public class MajorEditTitleModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
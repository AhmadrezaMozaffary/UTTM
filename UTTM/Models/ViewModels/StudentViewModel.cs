namespace UTTM.Models.ViewModels
{
    public class StudentViewModel
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;

        public int UserId { get; set; }

        public int SocietyId { get; set; }
    }

    public class StudentEditViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;
    }

}

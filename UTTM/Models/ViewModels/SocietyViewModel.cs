namespace UTTM.Models.ViewModels
{
    public class SocietyViewModel
    {
        public string Name { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int UserId { get; set; }

        public int MajorId { get; set; }
    }

    public class SocietyEditModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }

    public class SocietyEditNameModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class SocietyEditAvatarModel
    {
        public int Id { get; set; }
        public string Avatar { get; set; } = string.Empty;
    }

    public class SocietyEditDescriptionModel
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}

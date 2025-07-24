namespace OnlineEducationPlatform.Web.ViewModels
{
    public class ClassTeacherViewModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string TeacherId { get; set; }
        public List<ApplicationUser> Teachers { get; set; } = new List<ApplicationUser>();

    }
}

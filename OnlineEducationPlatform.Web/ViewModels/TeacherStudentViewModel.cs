namespace OnlineEducationPlatform.Web.ViewModels
{
    public class TeacherStudentViewModel
    {
        public List<ApplicationUser> Students { get; set; } = new List<ApplicationUser>();
        public List<ApplicationUser> Teachers { get; set; } = new List<ApplicationUser>();

    }
}

namespace OnlineEducationPlatform.Web.ViewModels
{
    public class StudentDashboardViewModel
    {
        public Models.Class Class { get; set; }
        public List<Models.Subject> Subjects { get; set; } = new List<Models.Subject>();
    }
}

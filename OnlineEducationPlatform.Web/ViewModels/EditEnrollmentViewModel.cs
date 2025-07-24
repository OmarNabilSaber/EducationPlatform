namespace OnlineEducationPlatform.Web.ViewModels
{
    public class EditEnrollmentViewModel
    {
        public string StudentId { get; set; }
        public int ClassId { get; set; }
        public List<ApplicationUser> Students { get; set; }
        public List<Class> Classes { get; set; }
        public Dictionary<string, int> StudentClassAssignments { get; set; } = new Dictionary<string, int>();
    }
}

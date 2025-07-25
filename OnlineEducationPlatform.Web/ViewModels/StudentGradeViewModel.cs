namespace OnlineEducationPlatform.Web.ViewModels
{
    public class StudentGradeViewModel
    {
        public string StudintId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public bool IsSubmitted { get; set; }
        public int? Score { get; set; }
    }
}

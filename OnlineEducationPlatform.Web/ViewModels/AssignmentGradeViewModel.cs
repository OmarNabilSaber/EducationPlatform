namespace OnlineEducationPlatform.Web.ViewModels
{
    public class AssignmentGradeViewModel
    {
        public int AssignmentId { get; set; }
        public string AssignmentName { get; set; }
        public int TotalScore { get; set; }
        public DateTime DueDate { get; set; }
        public string ClassName { get; set; }
        public int ClassId { get; set; }
        public List<StudentGradeViewModel> StudentGrades { get; set; }

    }
}

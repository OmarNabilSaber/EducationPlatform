namespace OnlineEducationPlatform.Web.ViewModels
{
    public class ExamGradeViewModel
    {
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public int PassingScore { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public string ClassName { get; set; }
        public int ClassId { get; set; }
        public List<StudentGradeViewModel> StudentGrades { get; set; }
    }
}

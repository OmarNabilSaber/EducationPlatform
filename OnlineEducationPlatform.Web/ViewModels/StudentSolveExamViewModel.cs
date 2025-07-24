namespace OnlineEducationPlatform.Web.ViewModels
{
    public class StudentSolveExamViewModel
    {
        public int ExamId { get; set; }
        public string Title { get; set; }
        public int TimeLimitMinutes { get; set; }
        public List<StudentQuestionAnswerViewModel> Questions { get; set; }
    }
}

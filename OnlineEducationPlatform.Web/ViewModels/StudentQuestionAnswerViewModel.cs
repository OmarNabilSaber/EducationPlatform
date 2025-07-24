namespace OnlineEducationPlatform.Web.ViewModels
{
    public class StudentQuestionAnswerViewModel
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public List<string> Options { get; set; }
        public string Answer { get; set; } // Student's answer
        public string CorrectAnswer { get; set; } // Student's answer

    }
}

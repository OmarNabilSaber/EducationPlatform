namespace OnlineEducationPlatform.Web.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public int Points { get; set; }

        // For multiple choice
        public List<string>? Options { get; set; } // Serialized list of choices
        public string? CorrectAnswer { get; set; }

        // Relationships
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
    }
}

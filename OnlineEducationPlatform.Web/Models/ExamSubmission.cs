using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineEducationPlatform.Web.Models
{
    public class ExamSubmission
    {
        public int SubmissionId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? SubmittedAt { get; set; }
        [NotMapped]
        public Dictionary<int, string>? Answers
        {
            get => string.IsNullOrEmpty(AnswersJson)
                ? new Dictionary<int, string>()
                : System.Text.Json.JsonSerializer.Deserialize<Dictionary<int, string>>(AnswersJson);
            set => AnswersJson = System.Text.Json.JsonSerializer.Serialize(value);
        }

        // This gets stored in the DB
        public string AnswersJson { get; set; }
        public int? Score { get; set; }

        // Relationships
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public string StudentId { get; set; }
        public Student Student { get; set; }
    }
}

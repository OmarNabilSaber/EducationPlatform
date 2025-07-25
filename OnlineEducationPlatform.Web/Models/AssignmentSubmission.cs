namespace OnlineEducationPlatform.Web.Models
{
    public class AssignmentSubmission
    {
        public int AssignmentId { get; set; }
        public string StudentId { get; set; }
        public string FilePath { get; set; }
        public DateTime SubmittedAt { get; set; }
        public int? Score { get; set; }
        public Assignment Assignment { get; set; }
        public Student Student { get; set; }
    }
}

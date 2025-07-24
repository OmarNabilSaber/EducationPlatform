namespace OnlineEducationPlatform.Web.Models
{
    public class AssignmentSubmission
    {
        public int AssignmentId { get; set; }
        public string StudentId { get; set; }
        public string FilePath { get; set; }
        public DateTime SubmittedAt { get; set; }
        public decimal? Score { get; set; }
    }
}

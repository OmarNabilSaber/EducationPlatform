namespace OnlineEducationPlatform.Web.Models
{
    public class Assignment
    {
        public int AssignmentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public DateTime DueDate { get; set; }
        public int TotalScore { get; set; }
        // Relationships
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public int ClassId { get; set; }
        public Class Class { get; set; }
        public ICollection<AssignmentSubmission> Submissions { get; set; } = new List<AssignmentSubmission>();
    }
}

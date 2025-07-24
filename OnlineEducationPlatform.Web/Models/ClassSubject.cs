namespace OnlineEducationPlatform.Web.Models
{
    public class ClassSubject
    {
        public int ClassId { get; set; }
        public Class Class { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        // Additional join table properties if needed
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
    }
}

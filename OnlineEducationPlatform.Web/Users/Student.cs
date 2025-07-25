namespace OnlineEducationPlatform.Web.Models
{
    public class Student: ApplicationUser
    {
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<ExamSubmission> ExamSubmissions { get; set; }
        public ICollection<AssignmentSubmission> AssignmentSubmission { get; set; }
    }
}

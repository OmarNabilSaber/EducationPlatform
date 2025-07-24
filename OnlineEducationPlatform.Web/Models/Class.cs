namespace OnlineEducationPlatform.Web.Models
{
    public class Class
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string? TeacherId { get; set; }
        public ApplicationUser Teacher { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<ClassSubject> Subjects { get; set; }
        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<Exam> Exams { get; set; }
    }
}

namespace OnlineEducationPlatform.Web.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Many-to-many with Class
        public ICollection<ClassSubject> Classes { get; set; }
        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<Exam> Exams { get; set; }

    }
}

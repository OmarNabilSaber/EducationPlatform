namespace OnlineEducationPlatform.Web.Models
{
    public class Enrollment
    {
        public string StudentId { get; set; }
        public int ClassId { get; set; }
        public DateTime EnrollDate { get; set; }
        public Class Class { get; set; }
        public Student Student { get; set; }
    }
}

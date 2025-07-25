namespace OnlineEducationPlatform.Web.Models
{
    public class ClassSubjectEditRequest
    {
        public int ClassId { get; set; }
        public List<int> SubjectIds { get; set; }
    }
}

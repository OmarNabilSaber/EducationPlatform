using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;
namespace OnlineEducationPlatform.Web.Models
{
    public class Exam
    {
        public int ExamId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Instructions { get; set; }
        [Required]
        public DateTime AvailableFrom { get; set; }
        [Required]
        public DateTime AvailableTo { get; set; }
        [Required]
        public int TimeLimitMinutes { get; set; }
        [Required]
        public int PassingScore { get; set; }
        [NotMapped]
        public bool IsAvailable => DateTime.UtcNow >= AvailableFrom && DateTime.UtcNow <= AvailableTo;

        // Relationships
        [Required]
        public int ClassId { get; set; }
        public Class Class { get; set; }
        [Required]
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<ExamSubmission> Submissions { get; set; }
    }
}

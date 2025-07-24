using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineEducationPlatform.Web.ViewModels
{
    public class ExamViewModel : IValidatableObject
    {
        public int ExamId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Instructions { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Available From")]
        public DateTime AvailableFrom { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Available To")]
        public DateTime AvailableTo { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Time limit must be at least 1 minute.")]
        public int TimeLimitMinutes { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Passing score must be at least 1.")]
        public int PassingScore { get; set; }
        [Required]
        public int ClassId { get; set; }
        public List<SelectListItem> Classes { get; set; } = new List<SelectListItem>();
        [Required]
        public int SubjectId { get; set; }
        public List<SelectListItem> Subjects { get; set; } = new List<SelectListItem>();
        [Required]
        public ICollection<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Questions == null || !Questions.Any())
            {
                yield return new ValidationResult("At least one question is required.", new[] { nameof(Questions) });
            }
            else
            {
                int totalPoints = 0;
                int qIndex = 0;
                foreach (var q in Questions)
                {
                    qIndex++;
                    if (q.Options == null || q.Options.Count < 2)
                    {
                        yield return new ValidationResult($"Question #{qIndex} must have at least two options.", new[] { $"Questions[{qIndex - 1}].Options" });
                    }
                    if (string.IsNullOrWhiteSpace(q.CorrectAnswer) || q.Options == null || !q.Options.Contains(q.CorrectAnswer))
                    {
                        yield return new ValidationResult($"Question #{qIndex} correct answer must be one of the options.", new[] { $"Questions[{qIndex - 1}].CorrectAnswer" });
                    }
                    totalPoints += (int)q.Points;
                }
                if (PassingScore > totalPoints)
                {
                    yield return new ValidationResult($"Passing score cannot be greater than the sum of all question points ({totalPoints}).", new[] { nameof(PassingScore) });
                }
            }
            if (AvailableFrom < DateTime.Now)
            {
                yield return new ValidationResult("Available From must be in the future.", new[] { nameof(AvailableFrom) });
            }
            if (AvailableTo < AvailableFrom)
            {
                yield return new ValidationResult("Available To must be after Available From.", new[] { nameof(AvailableTo) });
            }
        }
    }
}

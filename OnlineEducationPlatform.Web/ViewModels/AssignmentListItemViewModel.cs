using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OnlineEducationPlatform.Web.ViewModels
{
    public class AssignmentListItemViewModel
    {
        public int AssignmentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string SubjectName { get; set; }
        public string AssignmentPath { get; set; }
        public string? SubmitionPath { get; set; }
        public bool HasSubmitted { get; set; }
        public int TotalScore { get; set; }
        public int? Score { get; set; }
        public IFormFile AssignmentFile { get; set; }
        public IFormFile SubmissionFile { get; set; }
    }
}

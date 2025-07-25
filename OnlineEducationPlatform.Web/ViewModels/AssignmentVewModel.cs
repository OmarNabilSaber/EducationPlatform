using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineEducationPlatform.Web.ViewModels
{
    public class AssignmentVewModel
    {
        public int AssignmentId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DueDate { get; set; }
        [Required]
        public int ClassId { get; set; }
        public List<SelectListItem> Classes { get; set; } = new List<SelectListItem>();
        [Required]
        public int SubjectId { get; set; }
        public List<SelectListItem> Subjects { get; set; } = new List<SelectListItem>();
        public string FilePath { get; set; }
        public IFormFile AssignmentFile { get; set; }
        [Required]
        public int TotalScore { get; set; }
    }
}

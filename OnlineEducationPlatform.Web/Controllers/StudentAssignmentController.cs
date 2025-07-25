using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineEducationPlatform.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using OnlineEducationPlatform.Web.ViewModels;
using OnlineEducationPlatform.Web.Models;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace OnlineEducationPlatform.Web.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentAssignmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentAssignmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var studentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var assignments = _context.Assignments
                .Include(a => a.Submissions)
                .Select(a => new AssignmentListItemViewModel
                {
                    AssignmentId = a.AssignmentId,
                    Title = a.Title,
                    DueDate = a.DueDate,
                    Description = a.Description,
                    SubjectName = a.Subject.Name,
                    AssignmentPath = a.FilePath,
                    HasSubmitted = a.Submissions.Any(s => s.StudentId == studentId),
                    TotalScore = a.TotalScore,
                    Score = a.Submissions
                            .Where(s => s.StudentId == studentId)
                            .Select(s => s.Score)
                            .FirstOrDefault(),
                    SubmitionPath = a.Submissions
                                    .Where(s => s.StudentId == studentId)
                                    .Select(s => s.FilePath)
                                    .FirstOrDefault(),
                }).ToList();
            return View(assignments);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignments
                .Include(a => a.Class)
                .Include(a => a.Subject)
                .FirstOrDefaultAsync(m => m.AssignmentId == id);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }
        [HttpPost]
        public IActionResult TestUpload(IFormFile SubmissionFile)
        {
            Console.WriteLine("TestUpload called. File: " + (SubmissionFile != null ? SubmissionFile.FileName : "null"));
            return Json(new { success = SubmissionFile != null });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(int AssignmentId, IFormFile SubmissionFile)
        {
            if (SubmissionFile == null)
                return Json(new { success = false, error = "No file uploaded." });
            if (Path.GetExtension(SubmissionFile.FileName).ToLower() != ".pdf")
                return Json(new { success = false, error = "Only PDF files are allowed. Please upload a valid PDF file." });
            if (SubmissionFile.Length > 5 * 1024 * 1024)
                return Json(new { success = false, error = "File size must not exceed 5MB." });
            var assignment = await _context.Assignments.FindAsync(AssignmentId);
            if (assignment == null)
                return Json(new { success = false, error = "Assignment not found. Please refresh the page and try again." });
            var studentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(studentId))
                return Json(new { success = false, error = "User not found. Please log in again." });
            // Save file
            var submissionsFolder = Path.Combine("wwwroot", "Assignments", "Submissions");
            if (!Directory.Exists(submissionsFolder))
                Directory.CreateDirectory(submissionsFolder);
            var fileName = $"submission_{assignment.AssignmentId}_{studentId}_{Guid.NewGuid()}.pdf";
            var filePath = Path.Combine(submissionsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await SubmissionFile.CopyToAsync(stream);
            }
            // Save submission
            var submission = new AssignmentSubmission
            {
                AssignmentId = assignment.AssignmentId,
                StudentId = studentId,
                FilePath = $"/Assignments/Submissions/{fileName}",
                SubmittedAt = DateTime.Now
            };
            _context.AssignmentSubmission.Add(submission);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}

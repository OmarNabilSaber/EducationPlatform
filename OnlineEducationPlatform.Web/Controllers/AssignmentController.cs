using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineEducationPlatform.Infrastructure.Data;
using OnlineEducationPlatform.Web.Models;
using OnlineEducationPlatform.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineEducationPlatform.Web.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly string _assignmentFolder = "wwwroot/Assignments";

        public AssignmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Assignment
        [Authorize(Roles = "Admin,Instructor,Student")]
        public async Task<IActionResult> Index()
        {
            var assignments = new List<Assignment>();
            if (User.IsInRole("Admin"))
            {
                assignments = await _context.Assignments
                    .Include(a => a.Class)
                    .Include(a => a.Subject)
                    .ToListAsync();
            }
            else if (User.IsInRole("Student"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var enrollment = _context.Enrollments
                                        .FirstOrDefault(u => u.StudentId == userId);
                var classId = enrollment?.ClassId;
                assignments = _context.Assignments
                    .Include(a => a.Class)
                    .Include(a => a.Subject)
                    .Where(a => a.ClassId == classId).ToList(); ;
            }
            else if (User.IsInRole("Instructor"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var _class = _context.Classes.FirstOrDefault(c => c.TeacherId == userId);
                var classId = _class?.ClassId;
                assignments = _context.Assignments
                    .Include(a => a.Class)
                    .Include(a => a.Subject)
                    .Where(a => a.ClassId == classId).ToList(); ;
            }
            return View(assignments);
        }

        // GET: Assignment/Details/5
        [Authorize(Roles = "Admin,Instructor,Student")]
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

        // GET: Assignment/Create
        [Authorize(Roles = "Instructor")]
        public IActionResult Create()
        {
            var model = new AssignmentVewModel
            {
                Classes = _context.Classes.Select(c => new SelectListItem { Value = c.ClassId.ToString(), Text = c.ClassName }).ToList(),
                Subjects = new List<SelectListItem>()
            };
            return View(model);
        }

        // POST: Assignment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Create(AssignmentVewModel model)
        {
            model.Classes = _context.Classes.Select(c => new SelectListItem { Value = c.ClassId.ToString(), Text = c.ClassName }).ToList();
            model.Subjects = _context.Subjects.Where(s => s.Classes.Any(c => c.ClassId == model.ClassId)).Select(s => new SelectListItem { Value = s.SubjectId.ToString(), Text = s.Name }).ToList();
            ModelState.Remove("FilePath");
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string filePath = null;
            if (model.AssignmentFile != null)
            {
                if (Path.GetExtension(model.AssignmentFile.FileName).ToLower() != ".pdf")
                {
                    ModelState.AddModelError("AssignmentFile", "Only PDF files are allowed.");
                    return View(model);
                }
                if (!Directory.Exists(_assignmentFolder))
                    Directory.CreateDirectory(_assignmentFolder);
                var fileName = $"assignment_{Guid.NewGuid()}.pdf";
                filePath = Path.Combine(_assignmentFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.AssignmentFile.CopyToAsync(stream);
                }
            }

            var assignment = new Assignment
            {
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate,
                ClassId = model.ClassId,
                SubjectId = model.SubjectId,
                FilePath = filePath != null ? $"/Assignments/{Path.GetFileName(filePath)}" : null
            };
            _context.Add(assignment);
            var subject = _context.Subjects.FirstOrDefault( s=> s.SubjectId == model.SubjectId );
            SendNotificaion(model.ClassId, model.SubjectId, $"new assignment has been added to the subject {subject?.Name}");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Assignment/Edit/5
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }
            var model = new AssignmentVewModel
            {
                AssignmentId = assignment.AssignmentId,
                Title = assignment.Title,
                Description = assignment.Description,
                DueDate = assignment.DueDate,
                ClassId = assignment.ClassId,
                SubjectId = assignment.SubjectId,
                FilePath = assignment.FilePath,
                Classes = _context.Classes.Select(c => new SelectListItem { Value = c.ClassId.ToString(), Text = c.ClassName }).ToList(),
                Subjects = _context.Subjects.Where(s => s.Classes.Any(c => c.ClassId == assignment.ClassId)).Select(s => new SelectListItem { Value = s.SubjectId.ToString(), Text = s.Name }).ToList()
            };
            return View(model);
        }

        // POST: Assignment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Edit(int id, AssignmentVewModel model)
        {
            model.Classes = _context.Classes.Select(c => new SelectListItem { Value = c.ClassId.ToString(), Text = c.ClassName }).ToList();
            model.Subjects = _context.Subjects.Where(s => s.Classes.Any(c => c.ClassId == model.ClassId)).Select(s => new SelectListItem { Value = s.SubjectId.ToString(), Text = s.Name }).ToList();
            ModelState.Remove("FilePath");
            ModelState.Remove("AssignmentFile");
            if (id != model.AssignmentId)
            {
                return NotFound();
            }

            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }

            // Validate DueDate if changed
            if (model.DueDate < DateTime.Now)
            {
                ModelState.AddModelError("DueDate", "Due date cannot be in the past.");
            }

            if (model.AssignmentFile != null)
            {
                if (Path.GetExtension(model.AssignmentFile.FileName).ToLower() != ".pdf")
                {
                    ModelState.AddModelError("AssignmentFile", "Only PDF files are allowed.");
                }
            }

            if (!ModelState.IsValid)
            {
                // Keep the existing file path in the model for display
                model.FilePath = assignment.FilePath;
                return View(model);
            }

            assignment.Title = model.Title;
            assignment.Description = model.Description;
            assignment.DueDate = model.DueDate;
            assignment.ClassId = model.ClassId;
            assignment.SubjectId = model.SubjectId;

            if (model.AssignmentFile != null)
            {
                if (!Directory.Exists(_assignmentFolder))
                    Directory.CreateDirectory(_assignmentFolder);
                var fileName = $"assignment_{Guid.NewGuid()}.pdf";
                var filePath = Path.Combine(_assignmentFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.AssignmentFile.CopyToAsync(stream);
                }
                assignment.FilePath = $"/Assignments/{fileName}";
            }
            // If no new file is uploaded, keep the existing file path

            _context.Update(assignment);
            var subject = _context.Subjects.FirstOrDefault(s => s.SubjectId == model.SubjectId);
            SendNotificaion(model.ClassId, model.SubjectId, $"assignment {model.Title} has been in subject {subject?.Name}");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Assignment/Delete/5
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Assignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment != null)
            {
                _context.Assignments.Remove(assignment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Instructor")]
        public JsonResult GetSubjectsForClass(int classId)
        {
            var subjects = _context.ClassSubjects
                .Where(cs => cs.ClassId == classId)
                .Select(cs => cs.Subject)
                .Select(s => new { s.SubjectId, s.Name })
                .ToList();
            return Json(subjects);
        }

        private void SendNotificaion(int classId, int SubjectId,string message = "new assignment notification")
        {
            var _class = _context.Classes.FirstOrDefault(Class => Class.ClassId == classId);
            var studentsId = _context.Enrollments.Where(c => c.ClassId == classId).Select(s => s.StudentId);
            foreach (var studentId in studentsId)
            {
                Notification.SendNotification(_context, studentId, message, "assignment Notification");
            }
        }
        
    }
}

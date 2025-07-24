using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnlineEducationPlatform.Infrastructure.Data;
using OnlineEducationPlatform.Web.Models;
using System.Security.Claims;

namespace OnlineEducationPlatform.Web.Controllers
{
    [Authorize]
    public class SubjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SubjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var s = _context.Subjects.ToList();
            if (s == null || !s.Any())
            {
                var subjects = new List<SubjectViewModel>();
                return View(subjects);
            }
            SubjectViewModel[] subjectViewModels = s.Select(subject => new SubjectViewModel
            {
                Id = subject.SubjectId,
                Name = subject.Name,
                Description = subject.Description
            }).ToArray();
            return View(subjectViewModels);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(SubjectViewModel subjectViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var subject = new Subject
            {
                Name = subjectViewModel.Name,
                Description = subjectViewModel.Description
            };
            _context.Subjects.Add(subject);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var subject = _context.Subjects.Find(id);
            if (subject == null)
            {
                return NotFound();
            }
            _context.Subjects.Remove(subject);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var subject = _context.Subjects.Find(id);
            if (subject == null)
            {
                return NotFound();
            }
            var subjectViewModel = new SubjectViewModel
            {
                Id = subject.SubjectId,
                Name = subject.Name,
                Description = subject.Description
            };
            return View(subjectViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, SubjectViewModel subjectViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var subject = _context.Subjects.Find(id);
            if (subject == null)
            {
                return NotFound();
            }
            subject.Name = subjectViewModel.Name;
            subject.Description = subjectViewModel.Description;
            _context.Subjects.Update(subject);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Instructor,Student")]
        public IActionResult Details(int id)
        {
            var subject = _context.Subjects.Find(id);
            List<Assignment> assignments = new List<Assignment>();
            if (subject == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Admin"))
            {
                assignments = _context.Assignments
                .Where(a => a.SubjectId == subject.SubjectId)
                .ToList();
            }
            else if (User.IsInRole("Student"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var enrollment =  _context.Enrollments
                                        .FirstOrDefault(u => u.StudentId == userId);
                var classId = enrollment?.ClassId;
                assignments = _context.Assignments.Where(a => a.ClassId == classId && a.SubjectId == subject.SubjectId).ToList(); ;
            }
            else if (User.IsInRole("Instructor"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var _class = _context.Classes.FirstOrDefault(c => c.TeacherId == userId);
                var classId = _class?.ClassId;
                assignments = _context.Assignments.Where(a => a.ClassId == classId && a.SubjectId == subject.SubjectId).ToList(); ;
            }

            var subjectDetailsViewModel = new SubjectDetailsViewModel
            {
                Id = subject.SubjectId,
                Name = subject.Name,
                Description = subject.Description,
                Class = _context.ClassSubjects
                    .Where(cs => cs.SubjectId == subject.SubjectId)
                    .Select(cs => cs.Class)
                    .ToList(),

                AddedDate = _context.ClassSubjects
                    .Where(s => s.SubjectId == subject.SubjectId)
                    .Select(s => s.AddedDate)
                    .ToList(),

                Assignments = assignments
            };
            return View(subjectDetailsViewModel);
        }
    }
}
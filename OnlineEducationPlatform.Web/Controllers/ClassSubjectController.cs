using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineEducationPlatform.Infrastructure.Data;
using OnlineEducationPlatform.Web.ViewModels;

namespace OnlineEducationPlatform.Web.Controllers
{
    public class ClassSubjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ClassSubjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var _class = _context.Classes
                        .Include(c => c.Teacher)
                        .FirstOrDefault(c => c.ClassId == Id);

            if (_class == null)
            {
                return NotFound();
            }
            var classSubjectsViewModel = new ClassSubjectsViewModel()
            {
                Class = _class,
                ClassSubjects = _context.ClassSubjects.Where(cs => cs.ClassId == Id).ToList(),
                AllSubjects = _context.Subjects.ToList()
            };
            return View(classSubjectsViewModel);
        }


        [HttpPost]
        public IActionResult Edit([FromBody] ClassSubjectEditRequest request)
        {
            if (request == null || request.ClassId == 0)
            {
                return Json(new { success = false, error = "Invalid request." });
            }
            try
            {
                var classId = request.ClassId;
                var selectedSubjectIds = request.SubjectIds ?? new List<int>();

                // Get current assignments
                var currentAssignments = _context.ClassSubjects.Where(cs => cs.ClassId == classId).ToList();

                // Remove unselected subjects
                var toRemove = currentAssignments.Where(cs => !selectedSubjectIds.Contains(cs.SubjectId)).ToList();
                _context.ClassSubjects.RemoveRange(toRemove);

                // Add new assignments
                var toAdd = selectedSubjectIds
                    .Where(sid => !currentAssignments.Any(cs => cs.SubjectId == sid))
                    .Select(sid => new Models.ClassSubject { ClassId = classId, SubjectId = sid })
                    .ToList();
                _context.ClassSubjects.AddRange(toAdd);

                _context.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}

public class ClassSubjectEditRequest
{
    public int ClassId { get; set; }
    public List<int> SubjectIds { get; set; }
}

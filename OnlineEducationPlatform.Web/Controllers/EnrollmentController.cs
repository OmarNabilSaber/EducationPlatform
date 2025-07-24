using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineEducationPlatform.Infrastructure.Data;
using OnlineEducationPlatform.Web.Models;
using System.Collections.Generic; // Added for Dictionary
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationPlatform.Web.Controllers
{
    [Authorize(Roles = "Admin,Instructor")]
    public class EnrollmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EnrollmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Enrollment/Edit/5 (id = Enrollment composite key or StudentId)
        public async Task<IActionResult> Edit()
        {
            var students = await _context.Users.Where(u => u.Role == "Student").ToListAsync();
            var classes = await _context.Classes.ToListAsync();
            var enrollments = await _context.Enrollments.ToListAsync();
            var studentClassAssignments = students.ToDictionary(
                s => s.Id,
                s => enrollments.FirstOrDefault(e => e.StudentId == s.Id)?.ClassId ?? 0
            );
            var model = new EditEnrollmentViewModel
            {
                Students = students,
                Classes = classes,
                StudentClassAssignments = studentClassAssignments
            };
            return View(model);
        }

        // POST: Enrollment/Edit/
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Dictionary<string, int> ClassAssignments)
        {
            if (ClassAssignments == null)
            {
                return Json(new { success = false, message = "No assignments submitted." });
            }

            foreach (var assignment in ClassAssignments)
            {
                var studentId = assignment.Key;
                var classId = assignment.Value;
                var enrollment = await _context.Enrollments.FirstOrDefaultAsync(e => e.StudentId == studentId);
                if (classId == 0)
                {
                    // Remove enrollment if exists
                    if (enrollment != null)
                    {
                        _context.Enrollments.Remove(enrollment);
                    }
                }
                else
                {
                    if (enrollment == null)
                    {
                        // Add new enrollment
                        enrollment = new Enrollment
                        {
                            StudentId = studentId,
                            ClassId = classId,
                            EnrollDate = System.DateTime.Now
                        };
                        _context.Enrollments.Add(enrollment);
                    }
                    else if (enrollment.ClassId != classId)
                    {
                        // Remove old enrollment and add new one
                        _context.Enrollments.Remove(enrollment);
                        await _context.SaveChangesAsync(); // Save removal before adding new
                        var newEnrollment = new Enrollment
                        {
                            StudentId = studentId,
                            ClassId = classId,
                            EnrollDate = System.DateTime.Now
                        };
                        _context.Enrollments.Add(newEnrollment);
                    }
                    // else: no change needed
                }
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Assignments saved successfully." });
        }
    }

    
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using OnlineEducationPlatform.Infrastructure.Data;
using OnlineEducationPlatform.Web.ViewModels;
using System.Linq;
using System.Security.Claims;

namespace OnlineEducationPlatform.Web.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var enrollment = _context.Enrollments   
                .Include(e => e.Class)
                .ThenInclude(c => c.Teacher)
                .FirstOrDefault(e => e.StudentId == userId);
            var viewModel = new StudentDashboardViewModel();
            if (enrollment != null)
            {
                viewModel.Class = enrollment.Class;
                // Get subjects for the class
                viewModel.Subjects = _context.ClassSubjects
                    .Where(cs => cs.ClassId == enrollment.ClassId)
                    .Include(cs => cs.Subject)
                    .Select(cs => cs.Subject)
                    .ToList();
            }
            return View(viewModel);
        }
    }
}

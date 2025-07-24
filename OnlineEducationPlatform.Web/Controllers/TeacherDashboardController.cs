using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using OnlineEducationPlatform.Infrastructure.Data;
using OnlineEducationPlatform.Web.ViewModels;
using System.Linq;

namespace OnlineEducationPlatform.Web.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class TeacherDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public TeacherDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var classes = _context.Classes.Where(c => c.TeacherId == userId).ToList();
            var viewModel = new TeacherDashboardViewModel
            {
                Classes = classes
            };
            return View(viewModel);
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace OnlineEducationPlatform.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "AdminDashboard");
            }
            else if (User.Identity.IsAuthenticated && User.IsInRole("Student"))
            {
                return RedirectToAction("Index", "StudentDashboard");
            }
            else if(User.Identity.IsAuthenticated && User.IsInRole("Instructor"))
            {
                return RedirectToAction("Index", "TeacherDashboard");
            }
            return RedirectToAction("Login", "Account");

        }
    }
}

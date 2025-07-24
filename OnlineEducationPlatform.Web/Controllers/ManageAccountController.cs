using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineEducationPlatform.Web.Users;

namespace OnlineEducationPlatform.Web.Controllers
{
    public class ManageAccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageAccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("AccessDenied", "Account");

            var model = new EditProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(EditProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return Forbid();

            user.FullName = model.FullName;
            user.Email = model.Email;

            // Extract username from email (everything before the @)
            var emailParts = model.Email.Split('@');
            user.UserName = emailParts.Length > 0 ? emailParts[0] : model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Profile updated successfully.";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsEmailAvailable(string email)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser == null || existingUser.Id == currentUser.Id)
            {
                return Json(true);
            }

            return Json("Email is already taken");
        }

    }
}

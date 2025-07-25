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
            ModelState.Remove("ConfirmNewPassword");
            ModelState.Remove("NewPassword");
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

            // Password change logic
            if (!string.IsNullOrEmpty(model.NewPassword) || !string.IsNullOrEmpty(model.ConfirmNewPassword))
            {
                if (string.IsNullOrEmpty(model.OldPassword))
                {
                    ModelState.AddModelError("OldPassword", "Old password is required to change your password.");
                    return View(model);
                }
                if (model.NewPassword != model.ConfirmNewPassword)
                {
                    ModelState.AddModelError("ConfirmNewPassword", "New password and confirmation do not match.");
                    return View(model);
                }
                var passwordCheck = await _userManager.CheckPasswordAsync(user, model.OldPassword);
                if (!passwordCheck)
                {
                    ModelState.AddModelError("OldPassword", "Old password is incorrect.");
                    return View(model);
                }
                var passwordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                        ModelState.AddModelError("NewPassword", error.Description);
                    return View(model);
                }
            }

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

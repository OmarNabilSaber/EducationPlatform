﻿using Microsoft.AspNetCore.Mvc;

namespace OnlineEducationPlatform.Web.Controllers
{
    public class AdminDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

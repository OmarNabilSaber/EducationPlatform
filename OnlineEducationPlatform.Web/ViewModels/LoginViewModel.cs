﻿using System.ComponentModel.DataAnnotations;

namespace OnlineEducationPlatform.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; } 

    }
}

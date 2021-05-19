using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Shared.Models.Account
{
    public class PasswordRequest
    {
        [Required]
        [Display(Name = "Old password")]
        public string OldPassword { get; set; }

        [Required]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }
    }
}

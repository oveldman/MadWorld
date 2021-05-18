using System;
namespace Website.Shared.Models.Account
{
    public class PasswordRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string PasswordConfirm { get; set; }
    }
}

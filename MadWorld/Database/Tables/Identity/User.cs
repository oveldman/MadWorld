using System;
using Microsoft.AspNetCore.Identity;

namespace Database.Tables.Identity
{
    public class User : IdentityUser
    {
        public bool TwoFactorOn { get; set; }
        public string TwoFactorSecret { get; set; }
        public Guid? TwoFactorSession { get; set; }
        public DateTime? TwoFactorSessionExpire { get; set; }
    }
}

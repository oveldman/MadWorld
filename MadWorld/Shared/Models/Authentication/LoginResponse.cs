using System;
using System.Collections.Generic;

namespace Website.Shared.Models.Authentication
{
    public class LoginResponse : BaseModel
    {
        public bool RequiresTwoFactor { get; set; }
        public Guid? TwoFactorSession { get; set; }

        public string Name { get; set; }
        public string AccessToken { get; set; }
        public string Type { get; set; }
        public string ExpiresIn { get; set; }
        public string Username { get; set; }
        public string Issued { get; set; }
        public string Expires { get; set; }
        public List<string> Roles { get; set; }
    }
}

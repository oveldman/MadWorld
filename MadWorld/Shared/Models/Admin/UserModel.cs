using System;
namespace Website.Shared.Models.Admin
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Website.Shared.Models.Admin
{
    public class UserModel
    {
        public string Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        public string Password { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool IsNew { get; set; }
        public List<RoleModel> Roles { get; set; }
    }
}

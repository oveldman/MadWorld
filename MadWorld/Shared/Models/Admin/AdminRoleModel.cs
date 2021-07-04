using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Shared.Models.Admin
{
    public class AdminRoleModel
    {
        [Required]
        public string ID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Website.Shared.Opions
{
    public static class UserRoles
    {
        public static string Admin { get; set; } = "Admin";

        public static List<string> GetAllPropertyNames()
        {
            return typeof(UserRoles).GetProperties().Select(p => p.Name).ToList();
        }
    }
}

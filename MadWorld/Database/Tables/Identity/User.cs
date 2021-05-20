﻿using System;
using Microsoft.AspNetCore.Identity;

namespace Database.Tables.Identity
{
    public class User : IdentityUser
    {
        public string TwoFactorSecret { get; set; }
    }
}

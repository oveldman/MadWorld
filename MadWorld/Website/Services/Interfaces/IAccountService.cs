﻿using System;
using System.Threading.Tasks;
using Website.Shared.Models;
using Website.Shared.Models.Account;

namespace Website.Services.Interfaces
{
    public interface IAccountService
    {
        Task<BaseModel> ChangePassword(PasswordRequest passwordRequest);
    }
}

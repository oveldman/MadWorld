using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Shared.Models;
using Website.Shared.Models.Admin;

namespace Website.Services.Interfaces
{
    public interface IAdminService
    {
        Task<AdminModel> GetIndex();
        Task<List<UserModel>> GetUsers();
    }
}

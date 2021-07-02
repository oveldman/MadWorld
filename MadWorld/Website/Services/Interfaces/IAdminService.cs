using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Shared.Models;
using Website.Shared.Models.Admin;

namespace Website.Services.Interfaces
{
    public interface IAdminService
    {
        Task<BaseModel> DeleteUser(string id);
        Task<AdminModel> GetIndex();
        Task<List<UserModel>> GetUsers();
        Task<UserModel> GetUser(Guid id);
        Task<BaseModel> SaveUser(UserModel user);
    }
}

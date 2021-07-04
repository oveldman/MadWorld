using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Shared.Models;
using Website.Shared.Models.Admin;

namespace Website.Services.Interfaces
{
    public interface IRoleService
    {
        Task<BaseModel> Add(AdminRoleModel role);
        Task<BaseModel> AddStandard();
        Task<BaseModel> Delete(AdminRoleModel role);
        Task<List<AdminRoleModel>> GetAll();
    }
}

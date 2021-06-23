using System;
using System.Threading.Tasks;
using Website.Shared.Models;

namespace Website.Services.Interfaces
{
    public interface IStorageAuthenticatedService
    {
        Task<BaseModel> Create(AddFileRequest request);
        Task<BaseModel> Delete(Guid id);
        Task<FilesResponse> GetAll();
    }
}

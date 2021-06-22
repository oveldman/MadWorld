using System;
using System.Threading.Tasks;
using Website.Shared.Models;

namespace Website.Services.Interfaces
{
    public interface IStorageAnonymousService
    {
        Task<FileResponse> GetFile(Guid? id);
    }
}

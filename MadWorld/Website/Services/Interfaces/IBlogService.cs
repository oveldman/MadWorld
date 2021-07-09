using System;
using System.Threading.Tasks;
using Website.Shared.Models;

namespace Website.Services.Interfaces
{
    public interface IBlogService
    {
        Task<BlogsModel> GetAll(int page, int totalPosts);
    }
}

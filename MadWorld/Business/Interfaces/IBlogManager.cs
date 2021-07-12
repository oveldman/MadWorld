using System;
using Website.Shared.Models;

namespace Business.Interfaces
{
    public interface IBlogManager
    {
        BlogsModel GetBlog(int page, int totalPosts);
    }
}

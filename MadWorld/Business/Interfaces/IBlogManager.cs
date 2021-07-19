using System;
using Website.Shared.Models;
using Website.Shared.Models.Admin;

namespace Business.Interfaces
{
    public interface IBlogManager
    {
        BlogsModel GetBlog(int page, int totalPosts);
        AdminBlogModel GetBlog();
        AdminPostModel GetPost(Guid id);
        BaseModel DeletePost(Guid id);
        BaseModel SavePost(AdminPostModel model);
    }
}

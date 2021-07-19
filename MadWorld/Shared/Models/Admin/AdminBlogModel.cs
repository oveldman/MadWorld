using System;
using System.Collections.Generic;

namespace Website.Shared.Models.Admin
{
    public class AdminBlogModel : BaseModel
    {
       public List<AdminPostModel> Posts { get; set; } = new();
    }
}

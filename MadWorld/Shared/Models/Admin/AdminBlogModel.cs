using System;
using System.Collections.Generic;

namespace Website.Shared.Models.Admin
{
    public class AdminBlogModel
    {
        List<AdminPostModel> Posts { get; set; } = new();
    }
}

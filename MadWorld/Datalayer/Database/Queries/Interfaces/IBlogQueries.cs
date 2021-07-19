using System;
using System.Collections.Generic;
using Datalayer.Database.Tables;

namespace Datalayer.Database.Queries.Interfaces
{
    public interface IBlogQueries
    {
        int CountPosts();
        List<Post> GetAllPosts();
        List<Post> GetPosts(int page, int totalPosts);
    }
}

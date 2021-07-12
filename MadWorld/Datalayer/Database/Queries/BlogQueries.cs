using System;
using System.Collections.Generic;
using System.Linq;
using Datalayer.Database.Queries.Interfaces;
using Datalayer.Database.Tables;

namespace Datalayer.Database.Queries
{
    public class BlogQueries : IBlogQueries
    {
        private MadWorldContext _context;

        public BlogQueries(MadWorldContext context)
        {
            _context = context;
        }

        public int CountPosts()
        {
            return _context.Posts.Count();
        }

        public List<Post> GetPosts(int page, int totalPosts)
        {
            if (page < 0 || totalPosts < 1)
            {
                return new List<Post>();
            }

            int skipPosts = page * totalPosts;

            return _context.Posts
                              .OrderByDescending(p => p.Created)
                              .Skip(skipPosts)
                              .Take(totalPosts)
                              .ToList();
        }
    }
}

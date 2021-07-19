using System;
using System.Collections.Generic;
using System.Linq;
using Business.Interfaces;
using Datalayer.Database.Queries.Interfaces;
using Datalayer.Database.Tables;
using Datalayer.FileStorage.Interfaces;
using Datalayer.FileStorage.Models;
using Website.Shared.Models;
using Website.Shared.Models.Admin;

namespace Business
{
    public class BlogManager : IBlogManager
    {
        IBlogQueries _blogQueries;
        private IStorageManager _storageManager;

        public BlogManager(IBlogQueries blogQueries, IStorageManager storageManager)
        {
            _blogQueries = blogQueries;
            _storageManager = storageManager;
        }

        public BaseModel DeletePost(Guid id)
        {
            throw new NotImplementedException();
        }

        public BlogsModel GetBlog(int page, int totalPosts)
        {
            int countPosts = _blogQueries.CountPosts();
            List<Post> posts = _blogQueries.GetPosts(page, totalPosts);

            BlogsModel model = new()
            {
                TotalPages = countPosts / totalPosts,
                CurrentPage = page,
                MaxRetrievedPosts = totalPosts,
                Posts = new()
            };

            foreach (Post dbPost in posts)
            {
                string body = _storageManager.DownloadString(StoragePaths.BlogFiles, $"{dbPost.FileID}.html");

                model.Posts.Add(new PostModel {
                    Body = body,
                    Created = dbPost.Created,
                    Title = dbPost.Title,
                    WriterName = "Oscar Veldman"
                });
            }

            return model;
        }

        public AdminBlogModel GetBlog()
        {
            throw new NotImplementedException();
        }

        public AdminPostModel GetPost(Guid id)
        {
            throw new NotImplementedException();
        }

        public BaseModel SavePost(AdminPostModel model)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Datalayer.Database;
using Datalayer.Database.Queries;
using Datalayer.Database.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit;

namespace Tests.Datalayer.Database.Queries
{
    public class BlogQueriesTest
    {
        DbContextOptions<MadWorldContext> Options;

        [Fact]
        public void CountPosts_NoParameters_NumberFour()
        {
            // No Test data

            // Setup
            BlogQueries blogQueries = SetupBlogQuery();

            // Act
            int countPosts = blogQueries.CountPosts();

            // Assert
            int expectedCount = 4;
            Assert.Equal(expectedCount, countPosts);

            // Teardown
            TearDown();
        }

        [Fact]
        public void GetPosts_FirstPageTakeTwoPosts_ReturnTwoPosts()
        {
            // Test data
            int page = 0;
            int totalPosts = 2;

            // Setup
            BlogQueries blogQueries = SetupBlogQuery();

            // Act
            List<Post> posts = blogQueries.GetPosts(page, totalPosts);

            // Assert
            Assert.True(posts.Count() == 2, "Expected a list of two post items");
            Assert.Equal("Random", posts[0].Title);
            Assert.Equal("V3 Test", posts[1].Title);

            // Teardown
            TearDown();
        }

        [Fact]
        public void GetPosts_SecondPageTakeTwoPosts_ReturnTwoPosts()
        {
            // Test data
            int page = 1;
            int totalPosts = 2;

            // Setup
            BlogQueries blogQueries = SetupBlogQuery();

            // Act
            List<Post> posts = blogQueries.GetPosts(page, totalPosts);

            // Assert
            Assert.True(posts.Count() == 2, "Expected a list of two post items");
            Assert.Equal("Test V2", posts[0].Title);
            Assert.Equal("Test", posts[1].Title);

            // Teardown
            TearDown();
        }

        [Fact]
        public void GetPosts_FirstPageTakeTenPosts_ReturnFourPosts()
        {
            // Test data
            int page = 0;
            int totalPosts = 10;

            // Setup
            BlogQueries blogQueries = SetupBlogQuery();

            // Act
            List<Post> posts = blogQueries.GetPosts(page, totalPosts);

            // Assert
            Assert.True(posts.Count() == 4, "Expected a list of four post items");

            // Teardown
            TearDown();
        }

        [Fact]
        public void GetPosts_FirstPageTakeTenPosts_ReturnZeroPosts()
        {
            // Test data
            int page = 0;
            int totalPosts = 10;

            // Setup
            BlogQueries blogQueries = SetupBlogQuery(new List<Post>());

            // Act
            List<Post> posts = blogQueries.GetPosts(page, totalPosts);

            // Assert
            Assert.True(posts.Count() == 0, "Expected a list of zero post items");

            // Teardown
            TearDown();
        }

        [Fact]
        public void GetPosts_PageTwentyAndTakeTwentyPosts_ReturnZeroPosts()
        {
            // Test data
            int page = 20;
            int totalPosts = 20;

            // Setup
            BlogQueries blogQueries = SetupBlogQuery(new List<Post>());

            // Act
            List<Post> posts = blogQueries.GetPosts(page, totalPosts);

            // Assert
            Assert.True(posts.Count() == 0, "Expected a list of zero post items");

            // Teardown
            TearDown();
        }

        [Fact]
        public void GetPosts_NegativeValues_ReturnZeroPosts()
        {
            // Test data
            int page = -1;
            int totalPosts = -2;

            // Setup
            BlogQueries blogQueries = SetupBlogQuery();

            // Act
            List<Post> posts = blogQueries.GetPosts(page, totalPosts);

            // Assert
            Assert.True(posts.Count() == 0, "Expected a list of zero post items");

            // Teardown
            TearDown();
        }

        private BlogQueries SetupBlogQuery()
        {
            return SetupBlogQuery(GetDataSet());
        }

        private BlogQueries SetupBlogQuery(List<Post> testData)
        {
            Options = new DbContextOptionsBuilder<MadWorldContext>()
                        .UseInMemoryDatabase(databaseName: "BlogDatabase", new InMemoryDatabaseRoot())
                        .Options;

            using (var context = new MadWorldContext(Options))
            {
                context.Posts.AddRange(testData);
                context.SaveChanges();
            }

            return new BlogQueries(new MadWorldContext(Options));
        }

        private List<Post> GetDataSet()
        {
            return new List<Post>
            {
                new Post
                {
                    ID = new Guid("cc83cebf-7aa2-49ba-afbf-a66c43778c5f"),
                    Created = new DateTime(2021, 6, 4, 11, 0, 0),
                    Title = "Test",
                    FileID = new Guid("cc83cebf-7aa2-49ba-afbf-a66c43778c5f"),
                    WriterID = new Guid("cc83cebf-7aa2-49ba-afbf-a66c43778c5f")
                },
                new Post
                {
                    ID = new Guid("11787f4f-d744-4196-882f-abf15fea7d89"),
                    Created = new DateTime(2021, 7, 4, 9, 0, 0),
                    Title = "Test V2",
                    FileID = new Guid("11787f4f-d744-4196-882f-abf15fea7d89"),
                    WriterID = new Guid("11787f4f-d744-4196-882f-abf15fea7d89")
                },
                new Post
                {
                    ID = new Guid("c7422fca-52ca-4da4-9a59-47c8d9cdb788"),
                    Created = new DateTime(2021, 8, 4, 14, 0, 0),
                    Title = "V3 Test",
                    FileID = new Guid("c7422fca-52ca-4da4-9a59-47c8d9cdb788"),
                    WriterID = new Guid("c7422fca-52ca-4da4-9a59-47c8d9cdb788")
                },
                new Post
                {
                    ID = new Guid("f373dce0-1ff7-4f9f-b747-17f6236a8711"),
                    Created = new DateTime(2021, 9, 4, 2, 0, 0),
                    Title = "Random",
                    FileID = new Guid("f373dce0-1ff7-4f9f-b747-17f6236a8711"),
                    WriterID = new Guid("f373dce0-1ff7-4f9f-b747-17f6236a8711")
                }
            };
        }

        private void TearDown()
        {
            using (var context = new MadWorldContext(Options))
            {
                context.Database.EnsureDeleted();
                context.SaveChanges();
            }
        }
    }
}

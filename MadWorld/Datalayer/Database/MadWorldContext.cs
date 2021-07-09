using System;
using Datalayer.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace Datalayer.Database
{
    public class MadWorldContext : DbContext
    {
        public DbSet<FileInfo> Files { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Resume> Resumes { get; set; }

        public MadWorldContext(DbContextOptions<MadWorldContext> options) : base(options)
        {
        }
    }
}

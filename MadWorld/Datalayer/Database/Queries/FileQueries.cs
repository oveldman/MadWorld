using System;
using System.Collections.Generic;
using System.Linq;
using Datalayer.Database.Models;
using Datalayer.Database.Queries.Interfaces;
using Datalayer.Database.Tables;

namespace Datalayer.Database.Queries
{
    public class FileQueries : IFileQueries
    {
        private MadWorldContext _context;

        public FileQueries(MadWorldContext context)
        {
            _context = context;
        }

        public DbResult Add(FileInfo file)
        {
            try
            {
                _context.Files.Add(file);
                _context.SaveChanges();
                return new DbResult {
                    Succeed = true
                };
            }
            catch (Exception ex)
            {
                return new DbResult
                {
                    ErrorMessage = ex.Message
                };
            }
        }

        public FileInfo Get(Guid id, FileType accessType)
        {
            return _context.Files.FirstOrDefault(f => f.ID == id && f.AccessType == accessType);
        }

        public FileInfo Get(Guid id)
        {
            return _context.Files.FirstOrDefault(f => f.ID == id);
        }

        public List<FileInfo> GetAll()
        {
            return _context.Files.ToList();
        }
    }
}

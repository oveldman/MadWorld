using System;
using System.Linq;
using Datalayer.Database.Queries.Interfaces;
using Datalayer.Database.Tables;

namespace Datalayer.Database.Queries
{
    public class ResumeQueries : IResumeQueries
    {
        private MadWorldContext _context;

        public ResumeQueries(MadWorldContext context)
        {
            _context = context;
        }

        public Resume GetLastResume()
        {
            return _context.Resumes.OrderBy(r => r.Created).LastOrDefault();
        }
    }
}

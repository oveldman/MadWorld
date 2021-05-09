using System;
using System.Linq;
using Database.Queries.Interfaces;
using Database.Tables;

namespace Database.Queries
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

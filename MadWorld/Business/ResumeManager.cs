using System;
using Business.Interfaces;
using Database.Queries.Interfaces;
using Database.Tables;

namespace Business
{
    public class ResumeManager : IResumeManager
    {
        private IResumeQueries _resumeQueries;

        public ResumeManager(IResumeQueries resumeQueries)
        {
            _resumeQueries = resumeQueries;
        }

        public Resume GetLastResume()
        {
            return _resumeQueries.GetLastResume();
        }
    }
}

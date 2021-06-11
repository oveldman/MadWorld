using System;
using Business.Interfaces;
using Datalayer.Database.Queries.Interfaces;
using Datalayer.Database.Tables;

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

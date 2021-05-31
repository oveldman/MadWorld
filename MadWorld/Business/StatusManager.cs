using System;
using Business.Interfaces;
using Database.Queries.Interfaces;

namespace Business
{
    public class StatusManager : IStatusManager
    {
        private IGeneralQueries _queries;

        public StatusManager(IGeneralQueries queries)
        {
            _queries = queries;
        }

        public bool IsDatabaseAuthenticationOnline()
        {
            return _queries.IsAuthenticationOnline();
        }

        public bool IsDatabaseMadWorldOnline()
        {
            return _queries.IsMadWorldOnline();
        }
    }
}

using System;
using Datalayer.Database.Queries.Interfaces;

namespace Datalayer.Database.Queries
{
    public class GeneralGueries : IGeneralQueries
    {
        private AuthenticationContext _authenticationContext;
        private MadWorldContext _madWorldContext;

        public GeneralGueries(AuthenticationContext authenticationContext, MadWorldContext madWorldContext)
        {
            _authenticationContext = authenticationContext;
            _madWorldContext = madWorldContext;
        }

        public bool IsAuthenticationOnline()
        {
            try
            {
                return _authenticationContext.Database.CanConnect();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsMadWorldOnline()
        {
            try
            {
                return _madWorldContext.Database.CanConnect();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

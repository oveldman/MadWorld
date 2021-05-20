using System;
using System.Linq;
using Database.Queries.Interfaces;
using Database.Tables.Identity;

namespace Database.Queries
{
    public class AccountQueries : IAccountQueries
    {
        private AuthenticationContext _context;

        public AccountQueries(AuthenticationContext context)
        {
            _context = context;
        }

        public bool SetSecretToken(string username, string twofactorSecret)
        {
            User user = _context.Users.FirstOrDefault(u => u.UserName.Equals(username));

            if (user != null)
            {
                user.TwoFactorSecret = twofactorSecret;
                _context.SaveChanges();

                return true;
            }

            return false;
        }
    }
}

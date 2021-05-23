using System;
using System.Collections.Generic;
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

        public User FindUserBySession(Guid? session)
        {
            return _context.Users.FirstOrDefault(u => u.TwoFactorSession == session);
        }

        public List<User> GetUsers()
        {
            return _context.Users.ToList();
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

        public bool SetTwoFactorEnabled(string username, bool enabled)
        {
            User user = _context.Users.FirstOrDefault(u => u.UserName.Equals(username));

            if (user != null)
            {
                user.TwoFactorOn = enabled;
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public bool SetTwoFactorSession(string username, Guid? twoFactorSession)
        {
            User user = _context.Users.FirstOrDefault(u => u.UserName.Equals(username));

            if (user != null)
            {
                user.TwoFactorSession = twoFactorSession;
                user.TwoFactorSessionExpire = DateTime.Now.AddMinutes(15);
                _context.SaveChanges();

                return true;
            }

            return false;
        }
    }
}

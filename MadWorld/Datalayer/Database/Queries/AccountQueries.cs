using System;
using System.Collections.Generic;
using System.Linq;
using Datalayer.Database.Queries.Interfaces;
using Datalayer.Database.Tables.Identity;

namespace Datalayer.Database.Queries
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

        public User GetUserByID(Guid id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id.ToString());
        }

        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public bool SaveUser(User user)
        {
            _context.Add(user);
            _context.SaveChangesAsync();

            return true;
        }

        public bool SetSecretToken(string username, string twofactorSecret)
        {
            User user = _context.Users.FirstOrDefault(u => u.UserName.Equals(username));

            if (user is not null)
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

            if (user is not null)
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

            if (user is not null)
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

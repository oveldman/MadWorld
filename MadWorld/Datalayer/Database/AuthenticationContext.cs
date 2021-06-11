using System;
using Datalayer.Database.Tables.Identity;
using IdentityServer4.EntityFramework.Extensions;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Datalayer.Database
{
    public class AuthenticationContext : ApiAuthorizationDbContext<User>
    {
        private IOptions<OperationalStoreOptions> _operationalStoreOptions;

        public AuthenticationContext(
            DbContextOptions<AuthenticationContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
            _operationalStoreOptions = operationalStoreOptions;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigurePersistedGrantContext(_operationalStoreOptions.Value);
        }
    }
}

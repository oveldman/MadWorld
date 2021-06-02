﻿using System;
using Business;
using Common;
using Database.Queries.Interfaces;
using Database.Tables.Identity;
using Tests.Fake.Business.UserExtremeManager;
using Xunit;

namespace Tests.Business
{
    public class UserExtremeManagerTest
    {
        [Fact]
        public void FindUserByTwoFactorSession_Session_UserFoundSessionNotValid()
        {
            // Test data
            Guid? session = Guid.Parse("6b976751-e0c9-43c6-9aef-57bfba4ecfa9");

            // Setup
            IAccountQueries accountQueries = new AccountQueriesUserFoundBySession();
            UserExtremeManager userManager = new UserExtremeManager(accountQueries);
            SystemTime.SetDateTime(DateTime.Parse("05/24/2021 10:30:00"));

            // Act
            User user = userManager.FindUserByTwoFactorSession(session);

            // Assert
            Assert.Null(user);

            // Teardown
            SystemTime.ResetDateTime();
        }

        [Fact]
        public void FindUserByTwoFactorSession_Session_UserFoundSessionValid()
        {
            // Test data
            Guid? session = Guid.Parse("6b976751-e0c9-43c6-9aef-57bfba4ecfa9");

            // Setup
            IAccountQueries accountQueries = new AccountQueriesUserFoundBySession();
            UserExtremeManager userManager = new UserExtremeManager(accountQueries);
            SystemTime.SetDateTime(DateTime.Parse("05/24/2021 09:30:00"));

            // Act
            User user = userManager.FindUserByTwoFactorSession(session);

            // Assert
            Assert.Equal(user.TwoFactorSession, session);

            // Teardown
            SystemTime.ResetDateTime();
        }

        [Fact]
        public void FindUserByTwoFactorSession_Session_UserNotFound()
        {
            // Test data
            Guid? session = Guid.Parse("6b976751-e0c9-43c6-9aef-57bfba4ecfa9");

            // Setup
            IAccountQueries accountQueries = new AccountQueriesUserNotFound();
            UserExtremeManager userManager = new UserExtremeManager(accountQueries);
            SystemTime.SetDateTime(DateTime.Parse("05/24/2021 10:30:00"));

            // Act
            User user = userManager.FindUserByTwoFactorSession(session);

            // Assert
            Assert.Null(user);

            // Teardown
            SystemTime.ResetDateTime();
        }
    }
}
using System;
using AutoFixture.Xunit2;
using Business;
using Common;
using Database.Queries.Interfaces;
using Database.Tables.Identity;
using FluentAssertions;
using Moq;
using Tests.Setup;
using Xunit;

namespace Tests.Business
{
    public class UserExtremeManagerTest
    {
        [Theory]
        [AutoDomainData]
        public void FindUserByTwoFactorSession_Session_UserFoundSessionNotValid(
            [Frozen] Mock<IAccountQueries> accountQueriesMock,
            Guid? session,
            User userFromMockDB,
            UserExtremeManager userManager
            )
        {
            // Test data
            userFromMockDB.TwoFactorSession = session;
            userFromMockDB.TwoFactorSessionExpire = DateTime.Parse("05/24/2021 10:00:00");

            // Setup
            accountQueriesMock.Setup(queries => queries.FindUserBySession(session)).Returns(userFromMockDB);

            SystemTime.SetDateTime(DateTime.Parse("05/24/2021 10:30:00"));

            // Act
            User user = userManager.FindUserByTwoFactorSession(session);

            // Assert
            Assert.Null(user);

            // Teardown
            SystemTime.ResetDateTime();
        }

        [Theory]
        [AutoDomainData]
        public void FindUserByTwoFactorSession_Session_UserFoundSessionValid(
            [Frozen] Mock<IAccountQueries> accountQueriesMock,
            Guid? session,
            User userFromMockDB,
            UserExtremeManager userManager
            )
        {
            // Test data
            userFromMockDB.TwoFactorSession = session;
            userFromMockDB.TwoFactorSessionExpire = DateTime.Parse("05/24/2021 10:00:00");

            // Setup
            accountQueriesMock.Setup(queries => queries.FindUserBySession(session)).Returns(userFromMockDB);
            SystemTime.SetDateTime(DateTime.Parse("05/24/2021 09:30:00"));

            // Act
            User user = userManager.FindUserByTwoFactorSession(session);

            // Assert
            user.Should().BeEquivalentTo(userFromMockDB);

            // Teardown
            SystemTime.ResetDateTime();
        }

        [Theory]
        [AutoDomainData]
        public void FindUserByTwoFactorSession_Session_UserNotFound(
            [Frozen] Mock<IAccountQueries> accountQueriesMock,
            Guid? session,
            UserExtremeManager userManager
            )
        {
            // Setup
            accountQueriesMock.Setup(queries => queries.FindUserBySession(session)).Returns<User>(null);
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
